using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Repository
{
    public class DbRepository<T> where T:Entity
    {
        ConnectionMultiplexer _connection = ConnectionMultiplexer.Connect("localhost");

        public void Set(T person)
        {
            IDatabase db = _connection.GetDatabase();
            db.HashSet(person.Id, ToHashEntry(person));
        }

        public T Get(string id)
        {
            IDatabase db = _connection.GetDatabase();
            var valueSet = db.HashGetAll(id);

            if (Convert.ToDateTime(valueSet.First(x=>x.Name == "deleteDate").Value.ToString()) < DateTime.Now)
            {
                db.KeyDelete(id);
                return null;
            }

            var instance = MapCach(valueSet);

            return instance;
        }

        private T MapCach(HashEntry[] valueSet)
        {
            var instance = Activator.CreateInstance<T>();
            var propertyList = instance.GetType().GetProperties();

            foreach (var propertyInfo in propertyList)
            {
                var hash = valueSet.FirstOrDefault(x => x.Name == propertyInfo.Name);

                if (hash != null)
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(hash.Value.ToString(), propertyInfo.PropertyType));
                }
            }

            return instance;
        }
        

        private HashEntry[] ToHashEntry(T person)
        {
            var resultHashEntry = new List<HashEntry>();
            var properties = person.GetType().GetProperties();
            var attribute = person.GetType().GetCustomAttribute<TimerAttribute>();
            DateTime deleteDate = DateTime.Now;
            if (attribute != null)
            {
                deleteDate = DateTime.Now + attribute.CachTime;
            }
            resultHashEntry.Add(new HashEntry(nameof(deleteDate), deleteDate.ToString()));
            resultHashEntry.AddRange(properties
                .Where(x => x.GetValue(person) != null)
                .Select(proprty => new HashEntry(proprty.Name, proprty.GetValue(person).ToString())));
            return resultHashEntry.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Repository;

namespace RedisCache
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new DbRepository<Person>();
            var person = new Person
            {
                Name = "Pawel",
                Age = 27
            };

            repository.Set(person);
            Console.WriteLine("add person redis");
            var result = repository.Get(person.Id);
            Console.WriteLine("get object from redis.");
            Console.WriteLine(result);
            Console.WriteLine("wait 10 second");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            result= repository.Get(person.Id);
            if (result == null)
            {
                Console.WriteLine("object was delete from redis");
            }

            Console.ReadLine();


        }
    }
}

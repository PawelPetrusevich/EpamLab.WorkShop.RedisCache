using System.Runtime.Serialization;

namespace Repository
{
    [DataContract]
    public class Person : Entity
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int Age { get; set; }
        
    }
}
using System;

namespace Repository
{

    [Timer(10)]
    public class Entity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
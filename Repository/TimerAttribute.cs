using System;

namespace Repository
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TimerAttribute : Attribute
    {
        public TimeSpan CachTime { get; set; }

        public TimerAttribute(int second)
        {
            CachTime = TimeSpan.FromSeconds(second);
        }
    }
}
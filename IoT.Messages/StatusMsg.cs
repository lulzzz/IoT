using System;

namespace IoT.Messages
{
    public class StatusMsg
    {
        public bool Configured { get; set; }
        public DateTime TimeStamp => DateTime.UtcNow;
    }
}

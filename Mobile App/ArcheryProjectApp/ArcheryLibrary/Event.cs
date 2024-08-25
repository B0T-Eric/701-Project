using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; } 
        public string EventDescription { get; set; }
        public string EventType { get; set; }

        public Event(int eventId, string eventName, string eventDescription, string eventType)
        {
            EventId = eventId;
            EventName = eventName;
            EventDescription = eventDescription;
            EventType = eventType;
        }
    }
}

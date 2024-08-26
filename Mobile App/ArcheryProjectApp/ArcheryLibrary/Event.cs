using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Event
    {
        //Id from database (api or local)
        public int? EventId { get; set; }
        public string EventName { get; set; } 
        public string? EventDescription { get; set; }
        public string EventType { get; set; }
        public DateOnly EventDate {  get; set; }
        //constructor for loading a registered event
        public Event(int eventId, string eventName, string eventDescription, string eventType)
        {
            EventId = eventId;
            EventName = eventName;
            EventDescription = eventDescription;
            EventType = eventType;
        }
        //constructor for creating a new Event in app
        public Event(string eventName, string? eventDescription, string eventType)
        {
            EventName = eventName;
            if(eventDescription != null)
            {
                EventDescription = eventDescription;
            }
            EventType = eventType;
        }
        

    }
}

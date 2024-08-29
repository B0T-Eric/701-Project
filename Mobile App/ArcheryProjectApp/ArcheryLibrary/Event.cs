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
        public int? Id { get; set; }
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string Type { get; set; }
        public DateOnly Date {  get; set; }
        public ScoreCard ScoreCard { get; set; }
        //constructor for loading a registered event
        public Event(int eventId, string eventName, string eventDescription, string eventType, DateOnly eventDate, ScoreCard scoreCard)
        {
            Date = eventDate;
            Id = eventId;
            Name = eventName;
            Description = eventDescription;
            Type = eventType;
            ScoreCard = scoreCard;
        }
        //constructor for creating a new Event in app
        public Event(string eventName, string? eventDescription, string eventType, DateOnly eventDate)
        {
            Date = eventDate;
            Name = eventName;
            if(eventDescription != null)
            {
                Description = eventDescription;
            }
            Type = eventType;
        }
        //empty event constructor?
        public Event() 
        {
        }
        

    }
}

namespace ArcheryLibrary
{
    public class Event
    {
        //Id from database (api or local)
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string Type { get; set; }
        public DateOnly Date {  get; set; }

        //Round Count, This property determines how many rounds in this event. set on creation.
        public int RoundCount { get; set; }
        //Environment, Indoor or Outdoor set on creation.
        public string Environment { get; set; }
        //Weather, If outdoor use weather to describe weather conditions.
        public string? Weather { get; set; }
        //Division, Archers division. Set on creation.
        public string Division { get; set; }
        //Rounds.
        public List<Round> Rounds { get; set; }
        public int EventTotal { get; set; }

        //constructor for loading a registered event
        public Event(int eventId, string eventName, string eventDescription, string eventType, DateOnly eventDate,
            int roundCount, string environment, string weather, string division, List<Round> rounds)
        {
            Date = eventDate;
            Id = eventId;
            Name = eventName;
            Description = eventDescription;
            Type = eventType;
            RoundCount = roundCount;
            Environment = environment;
            Weather = weather;
            Division = division;
            Rounds = rounds;
        }
        //constructor for in app making but with no scorecard info
        public Event(int eventId, string eventName, string eventDescription, string eventType, DateOnly eventDate)
        {
            Date = eventDate;
            Id = eventId;
            Name = eventName;
            Description = eventDescription;
            Type = eventType;
            Rounds = new List<Round>();
        }
        //constructor for creating a new Event in app
        public Event(string eventName, string? eventDescription, string eventType, DateOnly eventDate, int roundCount, string environment, string weather, string division)
        {
            Date = eventDate;
            Name = eventName;
            if(eventDescription != null)
            {
                Description = eventDescription;
            }
            Type = eventType;
            RoundCount = roundCount;
            Environment = environment;
            Weather = weather;
            Division = division;
        }
        //empty event constructor?
        public Event() 
        {
        }
        
        public int GetEventAverage()
        {
            int average = 0;
            foreach( Round round in Rounds )
            {
                average += round.RoundTotal;
            }
            average /= RoundCount;
            return average;
        }
        public int GetEventTotal()
        {
            EventTotal = 0;
            foreach ( Round round in Rounds ) 
            {
                EventTotal += round.RoundTotal;
            }
            return EventTotal;

        }
    }
}

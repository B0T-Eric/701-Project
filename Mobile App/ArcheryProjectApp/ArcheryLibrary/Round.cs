using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Round
    {
        public string Type { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateOnly EventDate { get; set; }
        public int EndCount { get; set; }
        public int ShotsPerEnd { get; set; }
        public Target Target { get; set; }
        public string Environment { get; set; }
        public string? Weather { get; set; }
        public Division Division { get; set; }
        public List<List<int>> Ends { get; set; } = new List<List<int>>();
        //template creation
        public Round(string type, string eventName, string eventType, DateOnly date, string environment, string? weather, Division division, int ends, int shots, Target target)
        {
            Type = type;
            EventName = eventName;
            EventDate = date;
            EventType = eventType;
            Environment = environment;
            if(weather != null) 
            {
                Weather = weather;
            }
            Division = division;
            EndCount = ends;
            ShotsPerEnd = shots;
            Target = target;
        }

        protected Round(string type, string eventName, string eventType, DateOnly eventDate, int endCount, int shotsPerEnd, Target target, string environment, string? weather, Division division, List<List<int>> ends)
        {
            Type = type;
            EventName = eventName;
            EventType = eventType;
            EventDate = eventDate;
            EndCount = endCount;
            ShotsPerEnd = shotsPerEnd;
            Target = target;
            Environment = environment;
            if(weather != null)
            {
                Weather = weather;
            }
            Division = division;
            Ends = ends;
        }
        //load round

    }
}

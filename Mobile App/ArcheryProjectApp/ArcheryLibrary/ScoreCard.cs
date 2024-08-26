using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class ScoreCard
    {
        //Event Name, This property is a name of the event set by user or admin on creation, editable
        //public string EventName { get; set; }
        //Event Type, This property will be set on creation which determines whether it's not the event is a practice or competition
        //public string EventType { get; set; }
        //Event Date, This property is set for the day of the event. set on creation.
        //public DateOnly EventDate { get; set; }
        //Round Count, This property determines how many rounds in this event. set on creation.
        public int RoundCount { get; set; }
        //Target, Target Face used in each round. set on creation.
        //public Target? Target { get; set; }
        //Environment, Indoor or Outdoor set on creation.
        public string Environment { get; set; }
        //Weather, If outdoor use weather to describe weather conditions.
        public string? Weather { get; set; }
        //Division, Archers division. Set on creation.
        public Division Division { get; set; }
        //Rounds.
        public List<Round> Rounds { get; set; }

        public ScoreCard( int roundCount, string environment, string? weather, Division division)
        {
            RoundCount = roundCount;
            Environment = environment;
            Weather = weather;
            Division = division;
        }
    }
}

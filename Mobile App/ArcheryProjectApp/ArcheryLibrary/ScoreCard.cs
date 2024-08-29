using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class ScoreCard
    {
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

        public ScoreCard( int roundCount, string environment, string? weather, string division)
        {
            RoundCount = roundCount;
            Environment = environment;
            Weather = weather;
            Division = division;
        }
    }
}

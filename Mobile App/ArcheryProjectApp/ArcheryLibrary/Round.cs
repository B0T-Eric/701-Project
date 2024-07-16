using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    internal abstract class Round
    {
        public string Type { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateOnly EventDate { get; set; }
        public int EndCount { get; set; }
        public int ShotsPerEnd { get; set; }
        public Target Target { get; set; }
        public string Environment { get; set; }
        public string Weather { get; set; }
        public Division Division { get; set; }
        public List<List<int>> Ends { get; set; }

        public Round()
        {
        }
    }
}

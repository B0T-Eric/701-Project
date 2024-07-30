using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Flint : Round
    {
        public Flint(string type, string eventName, string eventType, DateOnly date, string environment, string? weather, Division division, int ends, int shots, Target target) : base(type, eventName, eventType, date, environment, weather, division, ends, shots, target)
        {
        }

        public List<Target> TargetsPerEnd { get; set; }
        public List<double> DistancePerEnd { get; set; }
        public bool WalkUp { get; set; }
        public bool WalkBack { get; set; }
        public bool Stationary { get; set; }

    }
}

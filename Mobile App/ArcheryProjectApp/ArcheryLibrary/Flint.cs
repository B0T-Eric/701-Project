using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    internal class Flint : Round
    {
        public List<Target> TargetsPerEnd { get; set; }
        public List<double> DistancePerEnd { get; set; }
        public bool WalkUp { get; set; }
        public bool WalkBack { get; set; }
        public bool Stationary { get; set; }

        public Flint()
        {
        }
    }
}

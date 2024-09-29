using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class End
    {
        //walk up walk back stationary end position.
        public ShootingPosition Position { get; set; }
        //amount of end fields to generate.
        public int ArrowCount { get; set; }
        //flint end distance
        public string? Distance { get; set; }
        //flint end target
        public Target? Target { get; set; }
        public List<int> Score { get; set; }
        //target and distance can be null as flint is optional.
        //constructor for making an end in app
        public End(ShootingPosition position, int arrowCount, string? distance, Target? target) 
        {
            Position = position;
            ArrowCount = arrowCount;
            Distance = distance;
            this.Target = target;
            Score = new List<int>();
        }

        public End(ShootingPosition position, int arrowCount, string? distance, Target? target, List<int> score) : this(position, arrowCount, distance, target)
        {
            Score = score;
        }
        public End()
        {

        }
    }
}

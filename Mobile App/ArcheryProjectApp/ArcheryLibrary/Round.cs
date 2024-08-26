using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Round
    {
        //Targets Per End, if flint round assign a target to each end
        public List<Target>? TargetsPerEnd { get; set; }
        //Distance Per End, if flint round assign distance to each end
        public List<double>? DistancePerEnd { get; set; }
        //a toggle for whether you walk up each arrow or not
        public bool? WalkUp { get; set; }
        //a toggle for whether you walk back each arrow or not
        public bool? WalkBack { get; set; }
        //a toggle whether or not you stand still or not.
        public bool? Stationary { get; set; }
        //Round Type - Flint or Standard
        public string? Type { get; set; }
        //End Count, This property determines how many ends inside of the rounds. set on creation.
        public int EndCount { get; set; }
        //Shots Per End, The amount of shots per end. set on creation.
        public int ShotsPerEnd { get; set; }
        //target
        public List<Target> targets { get; set; }
        //distance
        public float Distance { get; set; }


    }
}

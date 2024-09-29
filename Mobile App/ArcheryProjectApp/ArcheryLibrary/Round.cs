﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public enum ShootingPosition { Stationary, WalkBack, WalkUp }
    public class Round
    {
        //shooting Position of each end
        public ShootingPosition PositionType { get; set; }
        //Round Type - Flint or Standard
        public string Type { get; set; }
        //End Count, This property determines how many ends inside of the rounds. set on creation.
        public int EndCount { get; set; }
        //Shots Per End, The amount of shots per end. set on creation.
        public int ShotsPerEnd { get; set; }
        //target for standard round
        public Target? Target { get; set; }
        //distance for standard round 
        public string? Distance { get; set; }
        //end list which contains details for scoring and flint end information
        public List<End> Ends { get; set; }
        public Round()
        {
            Ends = new List<End>();
        }
    }
}

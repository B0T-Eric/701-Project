using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public enum ShootingPosition { Stationary, WalkBack, WalkUp }
    public class Round : INotifyPropertyChanged
    {
        public int Id { get; set; }
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
        //round totals
        private int roundTotal;
        public int RoundTotal 
        {
            get => roundTotal;
            set
            {
                if (roundTotal != value)
                {
                    roundTotal = value;
                    OnPropertyChanged(nameof(RoundTotal));
                }
            }
        }
        //total x's
        private int xTotal;
        public int XTotal 
        {
            get => xTotal;
            set
            {
                if (xTotal != value)
                {
                    xTotal = value;
                    OnPropertyChanged(nameof(XTotal));
                }
            }
        }

        public bool IsComplete { get; set; }
        public Round()
        {
            Ends = new List<End>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int GetRoundAverage()
        {
            return RoundTotal / Ends.Count;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

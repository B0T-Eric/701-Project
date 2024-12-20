﻿using System.ComponentModel;

namespace ArcheryLibrary
{
    public class End : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //end id for local storage
        public int EndId { get; set; }
        //Number displayed on the score cards
        public int EndNum { get; set; }
        //walk up walk back stationary end position.
        public ShootingPosition Position { get; set; }
        //amount of end fields to generate.
        public int ArrowCount { get; set; }
        //flint end distance
        public string? Distance { get; set; }
        //flint end target
        public Target? Target { get; set; }
        public List<string> Score { get; set; } = new List<string>();

        private int _xCount;
        private int _endTotal;
        private int _runningTotal;
        public int XCount
        {
            get => _xCount;
            set
            {
                if (_xCount != value)
                {
                    _xCount = value;
                    OnPropertyChanged(nameof(XCount));
                }
            }
        }
        public int EndTotal
        {
            get => _endTotal;
            set
            {
                if (_endTotal != value)
                {
                    _endTotal = value;
                    OnPropertyChanged(nameof(EndTotal));
                }
            }
        }
        public int RunningTotal
        {
            get => _runningTotal;
            set
            {
                if (_runningTotal != value)
                {
                    _runningTotal = value;
                    OnPropertyChanged(nameof(RunningTotal));
                }
            }
        }
        //target and distance can be null as flint is optional.
        //constructor for making an end in app
        public End(int endNum,ShootingPosition position, int arrowCount, string? distance, Target? target) 
        {
            EndNum = endNum;
            Position = position;
            ArrowCount = arrowCount;
            Distance = distance;
            this.Target = target;
        }

        public End(int endNum,ShootingPosition position, int arrowCount, string? distance, Target? target, List<string> score) : this(endNum,position, arrowCount, distance, target)
        {
            Score = score;
        }
        public End(int num)
        {
            EndNum = num;
        }
        public End()
        {

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetEndTotals(Target? target)
        {
            this._xCount = this.Score.Count(s => s.Equals("X",StringComparison.OrdinalIgnoreCase));
            int total = 0;
            if (this.Target == null) //if there is no target for the end use the round target which is param.
            {
                foreach (var score in this.Score)
                {
                    if (int.TryParse(score, out int parsedScore))
                    {
                        total += parsedScore;
                    }
                    else if (score.Equals("X", StringComparison.OrdinalIgnoreCase))
                    {
                        total += target.ZoneValues[^1];
                    }
                }
            }
            else //if the end has a target use it
            {
                foreach (var score in this.Score)
                {
                    if (int.TryParse(score, out int parsedScore))
                    {
                        total += parsedScore;
                    }
                    else if (score.Equals("X", StringComparison.OrdinalIgnoreCase))
                    {
                        total += this.Target.ZoneValues[^1];
                    }
                    else if (score.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }
            }
            this._endTotal = total;
        }
    }
}

using System;
using MOE.Common.Business.SplitFail;

namespace MOE.Common.Business.CustomReport
{
    public class Cycle
    {
        public enum TerminationCause
        {
            MaxOut,
            GapOut,
            ForceOff,
            Unknown
        }

        public DateTime _CycleEnd;

        public SplitFailDetectorActivationCollection Activations = new SplitFailDetectorActivationCollection();

        private double totalGreenTime = -1;

        private double totalRedTime = -1;


        public Cycle(DateTime cycleStart, DateTime changeToGreen,
            DateTime changeToYellow, DateTime changeToRed, DateTime cycleEnd)
        {
            CycleStart = cycleStart;
            ChangeToGreen = changeToGreen;
            BeginYellowClear = changeToYellow;
            ChangeToRed = changeToRed;
            _CycleEnd = cycleEnd;
        }

        public TerminationCause TerminationEvent { get; set; }

        public DateTime CycleStart { get; set; }

        public DateTime ChangeToGreen { get; set; }

        public DateTime BeginYellowClear { get; set; }

        public DateTime CycleEnd
        {
            get => _CycleEnd;
            set => _CycleEnd = value;
        }

        public DateTime ChangeToRed { get; set; }

        public DateTime EndYellowClearance { get; set; }

        public double TotalRedTime
        {
            get
            {
                if (totalRedTime == -1)
                    totalRedTime = (ChangeToRed - CycleEnd).TotalSeconds;
                return totalRedTime;
            }
        }

        public double TotalGreenTime
        {
            get
            {
                if (totalGreenTime == -1)
                    totalGreenTime = (BeginYellowClear - ChangeToGreen).TotalSeconds;
                return totalGreenTime;
            }
        }

        public double TotalTime => (CycleEnd - CycleStart).TotalSeconds;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SplitFail.SplitFailDetectorActivationCollection Activations = new SplitFail.SplitFailDetectorActivationCollection();

        private TerminationCause _TerminationEvent;

        public TerminationCause TerminationEvent
        {
            get {return _TerminationEvent;}
            set { _TerminationEvent = value; }
        }

        private DateTime _CycleStart;

        public DateTime CycleStart
        {
            get { return _CycleStart; }
            set { _CycleStart = value; }
        }

        private DateTime _ChangeToGreen;

        public DateTime ChangeToGreen
        {
            get { return _ChangeToGreen; }
            set { _ChangeToGreen = value; }
        }

        private DateTime _BeginYellowClear;

        public DateTime BeginYellowClear
        {
            get { return _BeginYellowClear; }
            set { _BeginYellowClear = value; }
        }

        private DateTime _ChangeToRed;

        public DateTime _CycleEnd;
        public DateTime CycleEnd
        {
            get { return _CycleEnd; }
            set { _CycleEnd = value; }
        }

        public DateTime ChangeToRed
        {
            get { return _ChangeToRed; }
            set { _ChangeToRed = value; }
        }

        private DateTime _EndYellowClearance;
        public DateTime EndYellowClearance
        {
            get { return _EndYellowClearance; }
            set { _EndYellowClearance = value; }
        }

        private double totalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (totalRedTime == -1)
                {
                    totalRedTime = (ChangeToRed - CycleEnd).TotalSeconds;
                }
                return totalRedTime;
            }
        }

        private double totalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                if (totalGreenTime == -1)
                {
                    totalGreenTime = (BeginYellowClear - ChangeToGreen).TotalSeconds;
                }
                return totalGreenTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return (CycleEnd - CycleStart).TotalSeconds;
            }
        }
        
        
        public Cycle(DateTime cycleStart, DateTime changeToGreen, 
            DateTime changeToYellow, DateTime changeToRed, DateTime cycleEnd)
        {
            _CycleStart = cycleStart;
            _ChangeToGreen = changeToGreen;
            _BeginYellowClear = changeToYellow;
            _ChangeToRed = changeToRed;
            _CycleEnd = cycleEnd;
        }
    }
}

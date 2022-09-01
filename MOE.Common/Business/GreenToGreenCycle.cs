using System;

namespace MOE.Common.Business
{
    public class GreenToGreenCycle
    {
        public enum EventType
        {
            ChangeToRed,
            ChangeToGreen,
            ChangeToYellow,
            GreenTermination,
            BeginYellowClearance,
            EndYellowClearance,
            Unknown
        }

        public GreenToGreenCycle(DateTime firstGreenEvent, DateTime redEvent, DateTime yellowEvent,
            DateTime lastGreenEvent)
        {
            StartTime = firstGreenEvent;
            RedEvent = redEvent;
            RedLineY = (RedEvent - StartTime).TotalSeconds;
            YellowEvent = yellowEvent;
            YellowLineY = (yellowEvent - StartTime).TotalSeconds;
            EndTime = lastGreenEvent;
            GreenLineY = (lastGreenEvent - StartTime).TotalSeconds;
            //PreemptCollection = new List<DetectorDataPoint>();
        }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double GreenLineY { get; }
        public double YellowLineY { get; }

        public double RedLineY { get; }

        //public List<DetectorDataPoint> PreemptCollection { get; }
        public DateTime RedEvent { get; }

        public DateTime YellowEvent { get; }


        public double TotalGreenTime => (YellowEvent - StartTime).TotalSeconds;
        public double TotalYellowTime => (RedEvent - YellowEvent).TotalSeconds;
        public double TotalRedTime => (EndTime - RedEvent).TotalSeconds;
        public double TotalTime => (EndTime - StartTime).TotalSeconds;
        public double TotalGreenTimeMilliseconds => (YellowEvent - StartTime).TotalMilliseconds;
        public double TotalYellowTimeMilliseconds => (RedEvent - YellowEvent).TotalMilliseconds;
        public double TotalRedTimeMilliseconds => (EndTime - RedEvent).TotalMilliseconds;
        public double TotalTimeMilliseconds => (EndTime - StartTime).TotalMilliseconds;
    }
}
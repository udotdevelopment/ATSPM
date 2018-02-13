using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    /// <summary>
    /// Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class GreenToGreenCycle
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public enum EventType {ChangeToRed, ChangeToGreen, ChangeToYellow, GreenTermination, BeginYellowClearance, EndYellowClearance,Unknown };
        public double GreenLineY { get; private set; }
        public double YellowLineY { get; private set; }
        public double RedLineY { get; private set; }
        //public List<DetectorDataPoint> PreemptCollection { get; }
        public DateTime RedEvent { get; private set; }
        public DateTime YellowEvent { get; private set; }


        
        public double TotalGreenTime => (YellowEvent - StartTime).TotalSeconds;
        public double TotalYellowTime => (RedEvent - YellowEvent).TotalSeconds;
        public double TotalRedTime => (EndTime - RedEvent).TotalSeconds;
        public double TotalTime => (EndTime - StartTime).TotalSeconds;
        public double TotalGreenTimeMilliseconds => (YellowEvent - StartTime).TotalMilliseconds;
        public double TotalYellowTimeMilliseconds => (RedEvent - YellowEvent).TotalMilliseconds;
        public double TotalRedTimeMilliseconds => (EndTime - RedEvent).TotalMilliseconds;
        public double TotalTimeMilliseconds => (EndTime - StartTime).TotalMilliseconds;

        public GreenToGreenCycle(DateTime firstGreenEvent, DateTime redEvent, DateTime yellowEvent, DateTime lastGreenEvent)
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



    }
}

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
    public class RedToRedCycle
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public enum EventType {ChangeToRed, ChangeToGreen, ChangeToYellow, GreenTermination, BeginYellowClearance, EndYellowClearance,Unknown };
        public double GreenLineY { get; private set; }
        public double YellowLineY { get; private set; }
        public double RedLineY { get; private set; }
        //public List<DetectorDataPoint> PreemptCollection { get; }
        public DateTime GreenEvent { get; private set; }
        public DateTime YellowEvent { get; private set; }

        //public double TotalDelay
        //{
        //    get
        //    {
        //        return DetectorEvents.Sum(d => d.Delay);
        //    }
        //}

        //private double _totalVolume = -1;
        //public double TotalVolume
        //{
        //    get
        //    {
        //        if (_totalVolume == -1)
        //            _totalVolume = DetectorEvents.Count;
        //        return _totalVolume;
        //    }

        //}
        
        public double TotalGreenTime => (YellowEvent - GreenEvent).TotalSeconds;
        public double TotalYellowTime => (EndTime - YellowEvent).TotalSeconds;
        public double TotalRedTime => (GreenEvent - StartTime).TotalSeconds;
        public double TotalTime => (EndTime - StartTime).TotalSeconds;

        public RedToRedCycle(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent)
        {
            StartTime = firstRedEvent;
            GreenEvent = greenEvent;
            GreenLineY = (greenEvent - StartTime).TotalSeconds;
            YellowEvent = yellowEvent;
            YellowLineY = (yellowEvent - StartTime).TotalSeconds;
            EndTime = lastRedEvent;
            RedLineY = (lastRedEvent - StartTime).TotalSeconds;
            //PreemptCollection = new List<DetectorDataPoint>();
        }


        //public void AddPreempt(DetectorDataPoint ddp)
        //{
        //    PreemptCollection.Add(ddp);
        //}

        //public void ClearDetectorData()
        //{
        //    _totalArrivalOnRed = -1;
        //    _totalGreenTime = -1;
        //    _totalArrivalOnGreen = -1;
        //    _totalVolume = -1;
        //    DetectorEvents.Clear();
        //}
    }
}

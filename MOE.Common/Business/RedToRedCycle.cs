using System;

namespace MOE.Common.Business
{
    /// <summary>
    ///     Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class RedToRedCycle
    {
        public enum EventType
        {
            ChangeToRed,
            ChangeToGreen,
            ChangeToYellow,
            GreenTermination,
            BeginYellowClearance,
            EndYellowClearance,
            Unknown,
            ChangeToEndMinGreen,
            ChangeToEndOfRedClearance,
            OverLapDark
        }

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

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double GreenLineY { get; }
        public double YellowLineY { get; }

        public double RedLineY { get; }

        //public List<DetectorDataPoint> PreemptCollection { get; }
        public DateTime GreenEvent { get; }

        public DateTime YellowEvent { get; }

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
        public double TotalGreenTimeMilliseconds => (YellowEvent - GreenEvent).TotalMilliseconds;
        public double TotalYellowTimeMilliseconds => (EndTime - YellowEvent).TotalMilliseconds;
        public double TotalRedTimeMilliseconds => (GreenEvent - StartTime).TotalMilliseconds;
        public double TotalTimeMilliseconds => (EndTime - StartTime).TotalMilliseconds;


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
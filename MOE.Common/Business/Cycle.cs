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
    public class Cycle
    {
        public enum NextEventResponse{GroupOk, GroupMissingData, GroupComplete};
        public enum EventType {ChangeToRed, ChangeToGreen, ChangeToYellow, GreenTermination, BeginYellowClearance, EndYellowClearance,Unknown };
        public enum TerminationType { ForceOff, GapOut, MaxOut, Unknown };
        public DateTime StartTime { get; private set; }
        public List<Models.Speed_Events> SpeedsForCycle;
        public DateTime EndTime { get; private set; }
        public double GreenLineY { get; private set; }
        public double YellowLineY { get; private set; }
        public double RedLineY { get; private set; }
        public TerminationType Termination { get; set; }
        public NextEventResponse Status { get; protected set; }
        public List<DetectorDataPoint> DetectorEvents { get; private set; }
        public List<DetectorDataPoint> PreemptCollection { get; }
        public DateTime GreenEvent { get; private set; }
        public DateTime YellowEvent { get; private set; }
        private double _totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get {
                if (_totalArrivalOnGreen == -1)
                    _totalArrivalOnGreen = DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnGreen);
                return _totalArrivalOnGreen;
            }
        }

        private double _totalArrivalOnYellow = -1;
        public double TotalArrivalOnYellow
        {
            get
            {
                if (_totalArrivalOnYellow == -1)
                    _totalArrivalOnYellow = DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnYellow);
                return _totalArrivalOnYellow;
            }
        }

        private double _totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                if(_totalArrivalOnRed == -1)
                    _totalArrivalOnRed = DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnRed);
                return _totalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return DetectorEvents.Sum(d => d.Delay);
            }
        }

        private double _totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (_totalVolume == -1)
                    _totalVolume = DetectorEvents.Count;
                return _totalVolume;
            }

        }

        private double _totalGreenTime = -1;
        public double TotalGreenTime
        {
            get{
                if (_totalGreenTime == -1)
                {
                    _totalGreenTime = (YellowEvent - GreenEvent).TotalSeconds;
                }
                return _totalGreenTime;
            }
        }

        private double _totalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (_totalYellowTime == -1)
                {
                    _totalYellowTime = (EndTime - YellowEvent).TotalSeconds;
                }
                return _totalYellowTime;
            }
        }

        private double _totalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (_totalRedTime == -1)
                {
                    _totalRedTime = (GreenEvent - StartTime).TotalSeconds;
                }
                return _totalRedTime;
            }
        }

        public double TotalTime => (EndTime - StartTime).TotalSeconds;

        public Cycle(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent)
        {
            StartTime = firstRedEvent;
            GreenEvent = greenEvent;
            GreenLineY = (greenEvent - StartTime).TotalSeconds;
            YellowEvent = yellowEvent;
            YellowLineY = (yellowEvent - StartTime).TotalSeconds;
            EndTime = lastRedEvent;
            RedLineY = (lastRedEvent - StartTime).TotalSeconds;
            DetectorEvents = new List<DetectorDataPoint>();
            PreemptCollection = new List<DetectorDataPoint>();
        }

        public void AddDetectorData(DetectorDataPoint ddp)
        {
            DetectorEvents.Add(ddp);
        }

        public void AddPreempt(DetectorDataPoint ddp)
        {
            PreemptCollection.Add(ddp);
        }

        public void ClearDetectorData()
        {
            _totalArrivalOnRed = -1;
            _totalGreenTime = -1;
            _totalArrivalOnGreen = -1;
            _totalVolume = -1;
            DetectorEvents.Clear();
        }

        public void FindSpeedEventsForCycle(List<Models.Speed_Events> speeds)
        {
            SpeedsForCycle = (from r in speeds
                             where r.timestamp > this.StartTime
                             && r.timestamp < this.EndTime
                             select r).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    /// <summary>
    ///     Data that represents a red to red cycle for a signal phase
    /// </summary>
    public class CyclePcd : RedToRedCycle
    {
        public CyclePcd(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent) :
            base(firstRedEvent, greenEvent, yellowEvent, lastRedEvent)
        {
            DetectorEvents = new List<DetectorDataPoint>();
        }

        public List<DetectorDataPoint> DetectorEvents { get; }

        public double TotalArrivalOnGreen => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnGreen);
        public double TotalArrivalOnYellow => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnYellow);
        public double TotalArrivalOnRed => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnRed);
        public double TotalDelay => DetectorEvents.Sum(d => d.Delay);
        public double TotalVolume => DetectorEvents.Count(d => d.TimeStamp >= StartTime && d.TimeStamp < EndTime);

        public void AddDetectorData(DetectorDataPoint ddp)
        {
            DetectorEvents.Add(ddp);
        }

        public void AddSecondsToDetectorEvents(int seconds)
        {
            foreach (var detectorDataPoint in DetectorEvents)
            {
                detectorDataPoint.AddSeconds(seconds);
            }
        }
    }
}
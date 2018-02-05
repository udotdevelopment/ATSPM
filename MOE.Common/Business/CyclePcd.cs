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
    public class CyclePcd:RedToRedCycle
    {
        public List<DetectorDataPoint> DetectorEvents { get; private set; }

        public double TotalArrivalOnGreen => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnGreen);
        public double TotalArrivalOnYellow => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnYellow);
        public double TotalArrivalOnRed => DetectorEvents.Count(d => d.ArrivalType == ArrivalType.ArrivalOnRed);
        public double TotalDelay => DetectorEvents.Sum(d => d.Delay);
        public double TotalVolume => DetectorEvents.Count;
        

        public CyclePcd(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent):base(firstRedEvent, greenEvent, yellowEvent, lastRedEvent)
        {
            DetectorEvents = new List<DetectorDataPoint>();
           
        }

        public void AddDetectorData(DetectorDataPoint ddp)
        {
            DetectorEvents.Add(ddp);
        }

  
    }
}

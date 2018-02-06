using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class CycleSpeed:RedToRedCycle
    {
        public List<Speed_Events> SpeedEvents { get; set; }
        

        public CycleSpeed(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent):base(firstRedEvent, greenEvent, yellowEvent, lastRedEvent)
        {
        }

        public void FindSpeedEventsForCycle(List<Models.Speed_Events> speeds)
        {
            SpeedEvents = speeds.Where(s => s.timestamp >= this.GreenEvent.AddSeconds(15) && s.timestamp < this.YellowEvent && s.MPH >= 5).ToList();
        }
    }
}

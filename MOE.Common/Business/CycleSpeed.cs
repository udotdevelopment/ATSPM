using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class CycleSpeed : RedToRedCycle
    {
        public CycleSpeed(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent) :
            base(firstRedEvent, greenEvent, yellowEvent, lastRedEvent)
        {
        }

        public List<Speed_Events> SpeedEvents { get; set; }

        public void FindSpeedEventsForCycle(List<Speed_Events> speeds)
        {
            SpeedEvents = speeds.Where(s =>
                s.timestamp >= GreenEvent.AddSeconds(15) && s.timestamp < YellowEvent && s.MPH >= 5).ToList();
        }
    }
}
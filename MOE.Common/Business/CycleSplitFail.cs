using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business
{
    public class CycleSplitFail:RedToRedCycle
    {
        public enum TerminationType
        {
            MaxOut,
            GapOut,
            ForceOff,
            Unknown
        }
        public TerminationType TerminationEvent { get; private set; }
        public int FailsInCycle { get; private set; }
        public double RedOccupancy { get; private set; }
        public double GreenOccupancy { get; private set; }


        public CycleSplitFail(DateTime firstRedEvent, DateTime greenEvent, DateTime yellowEvent, DateTime lastRedEvent, TerminationType terminationType,
            List<SplitFailDetectorActivation> detectorActivations):base(firstRedEvent, greenEvent, yellowEvent, lastRedEvent)
        {
            TerminationEvent = terminationType;
            RedOccupancy = GetOccupancy(StartTime, GreenEvent, detectorActivations.Where(d => d.DetectorOff > StartTime && d.DetectorOn < GreenEvent).ToList(), TotalRedTime);
            GreenOccupancy = GetOccupancy(GreenEvent, YellowEvent, detectorActivations.Where(d => d.DetectorOff > GreenEvent && d.DetectorOn < EndTime).ToList(), TotalGreenTime);
        }
        
        private double GetOccupancy(DateTime start, DateTime end, List<SplitFailDetectorActivation> cycleDetectorActivations,
            double periodTime)
        {
            double occupancy = 0;
            double time = Convert.ToInt32(periodTime * 1000);
            foreach (SplitFailDetectorActivation detectorActivation in cycleDetectorActivations)
            {
                if (detectorActivation.DetectorOn < start)
                {
                    occupancy += (detectorActivation.DetectorOff - start).TotalMilliseconds;
                }
                else if (detectorActivation.DetectorOff > end)
                {
                    occupancy += (end - detectorActivation.DetectorOn).TotalMilliseconds;
                }
                else
                {
                    occupancy += detectorActivation.Duration;
                }
            }
            if (time > 0)
            {
                return occupancy / time;
            }
            return 0;
        }

        
    }
}

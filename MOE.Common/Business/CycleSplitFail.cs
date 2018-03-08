using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.SplitFail;

namespace MOE.Common.Business
{
    public class CycleSplitFail : GreenToGreenCycle
    {
        public enum TerminationType
        {
            MaxOut,
            GapOut,
            ForceOff,
            Unknown
        }

        private readonly int _firstSecondsOfRed;

        public CycleSplitFail(DateTime firstGreenEvent, DateTime redEvent, DateTime yellowEvent,
            DateTime lastGreenEvent, TerminationType terminationType,
            int firstSecondsOfRed) : base(firstGreenEvent, redEvent, yellowEvent, lastGreenEvent)
        {
            _firstSecondsOfRed = firstSecondsOfRed;
            TerminationEvent = terminationType;
        }

        public TerminationType TerminationEvent { get; }
        public double RedOccupancyTimeInMilliseconds { get; private set; }
        public double GreenOccupancyTimeInMilliseconds { get; private set; }
        public double GreenOccupancyPercent { get; private set; }
        public double RedOccupancyPercent { get; private set; }
        public bool IsSplitFail { get; private set; }
        public List<SplitFailDetectorActivation> ActivationsDuringGreen { get; set; }
        public List<SplitFailDetectorActivation> ActivationsDuringRed { get; set; }


        public void SetDetectorActivations(List<SplitFailDetectorActivation> detectorActivations)
        {
            var redPeriodToAnalyze = RedEvent.AddSeconds(_firstSecondsOfRed);

            ActivationsDuringRed = detectorActivations.Where
                //detStart AFTER redStart and Before red+AnalaysTime
                (d => d.DetectorOn >= RedEvent && d.DetectorOn < redPeriodToAnalyze

                      //detOff After redStart and Before red+AnalaysTime
                      || d.DetectorOff >= RedEvent && d.DetectorOff < redPeriodToAnalyze

                      //DetStart BEFORE redStart and detOff after cycleEnd
                      || d.DetectorOn <= RedEvent && d.DetectorOff >= EndTime).OrderBy(d => d.DetectorOn).ToList();
            //if (activationsDuringRed.Count == 0)
            //{
            //    RedOccupancyTime = CheckForDetectorActivationBiggerThanPeriod(RedEvent, redPeriodToAnalyze, detectorActivations);
            //}
            //else
            //{
            RedOccupancyTimeInMilliseconds = GetOccupancy(RedEvent, redPeriodToAnalyze, ActivationsDuringRed);
            //}
            ActivationsDuringGreen = detectorActivations.Where(d =>
                d.DetectorOn >= StartTime && d.DetectorOn < YellowEvent ||
                d.DetectorOff >= StartTime && d.DetectorOff < YellowEvent ||
                d.DetectorOn <= StartTime && d.DetectorOff >= YellowEvent).OrderBy(d => d.DetectorOn).ToList();
            //if (activationsDuringGreen.Count == 0)
            //{
            //    GreenOccupancyTime = CheckForDetectorActivationBiggerThanPeriod(StartTime, YellowEvent, detectorActivations);
            //}
            //else
            //{
            GreenOccupancyTimeInMilliseconds = GetOccupancy(StartTime, YellowEvent, ActivationsDuringGreen);
            //}
            double millisecondsOfRedStart = _firstSecondsOfRed * 1000;
            RedOccupancyPercent = RedOccupancyTimeInMilliseconds / millisecondsOfRedStart * 100;
            GreenOccupancyPercent = GreenOccupancyTimeInMilliseconds / TotalGreenTimeMilliseconds * 100;
            IsSplitFail = GreenOccupancyPercent > 79 && RedOccupancyPercent > 79;
        }


        private double CheckForDetectorActivationBiggerThanPeriod(DateTime startTime, DateTime endTime,
            List<SplitFailDetectorActivation> detectorActivations)
        {
            if (detectorActivations.Count(d => d.DetectorOn < startTime && endTime > d.DetectorOff) > 0)
                return (endTime - startTime).Milliseconds;
            return 0;
        }

        private double GetOccupancy(DateTime start, DateTime end,
            List<SplitFailDetectorActivation> cycleDetectorActivations)
        {
            double occupancy = 0;
            foreach (var detectorActivation in cycleDetectorActivations)
                if (detectorActivation.DetectorOn <= start && detectorActivation.DetectorOff >= end)
                    occupancy += (end - start).TotalMilliseconds;
                else if (detectorActivation.DetectorOn <= start && detectorActivation.DetectorOff <= end)
                    occupancy += (detectorActivation.DetectorOff - start).TotalMilliseconds;
                else if (detectorActivation.DetectorOn >= start && detectorActivation.DetectorOff >= end)
                    occupancy += (end - detectorActivation.DetectorOn).TotalMilliseconds;
                else
                    occupancy += detectorActivation.DurationInMilliseconds;
            return occupancy;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public class PlanPcd : Plan
    {
        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        public PlanPcd(DateTime start, DateTime end, int planNumber, List<CyclePcd> cyclesForPlan) : base(start, end,
            planNumber)
        {
            TotalTime = cyclesForPlan.Sum(d => d.TotalTime);
            TotalRedTime = cyclesForPlan.Sum(d => d.TotalRedTime);
            TotalYellowTime = cyclesForPlan.Sum(d => d.TotalYellowTime);
            TotalGreenTime = cyclesForPlan.Sum(d => d.TotalGreenTime);
            TotalVolume = cyclesForPlan.Sum(d => d.TotalVolume);
            TotalDelay = cyclesForPlan.Sum(d => d.TotalDelay);
            TotalArrivalOnRed = cyclesForPlan.Sum(d => d.TotalArrivalOnRed);
            TotalArrivalOnYellow = cyclesForPlan.Sum(d => d.TotalArrivalOnYellow);
            TotalArrivalOnGreen = cyclesForPlan.Sum(d => d.TotalArrivalOnGreen);
            TotalCycles = cyclesForPlan.Count;
            TotalDetectorHits = cyclesForPlan.Sum(c => c.TotalVolume);
        }

        public double TotalDetectorHits { get; }

        public double PercentGreenTime
        {
            get
            {
                if (TotalTime > 0)
                    return Math.Round(TotalGreenTime / TotalTime * 100);
                return 0;
            }
        }

        public double AvgDelay => TotalDelay / TotalVolume;

        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(TotalArrivalOnGreen / TotalVolume * 100);
                return 0;
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(PercentArrivalOnGreen / PercentGreenTime, 2);
                return 0;
            }
        }

        public double TotalArrivalOnGreen { get; }
        public double TotalArrivalOnYellow { get; }
        public double TotalArrivalOnRed { get; }
        public double TotalDelay { get; }
        public double TotalVolume { get; }
        public double TotalGreenTime { get; }
        public double TotalYellowTime { get; }
        public double TotalRedTime { get; }
        public double TotalTime { get; }
        public int TotalCycles { get; }
    }
}
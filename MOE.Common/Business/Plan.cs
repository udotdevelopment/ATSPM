using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class Plan: PlanBase
    {
  
        public int TotalDetectorHits { get; set; }
        public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                {
                    return Math.Round(((TotalGreenTime / TotalTime) * 100));
                }
                return 0;
            }
        }
        public int CycleCount { get; set; }
        public int CycleLength { get; set; }
        public int OffsetLength { get; set; }
        public double AvgDelay => TotalDelay/TotalVolume;
        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round(((TotalArrivalOnGreen / TotalVolume) * 100));
                }
                return 0;
            }
        }
        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((PercentArrivalOnGreen / PercentGreen),2);
                }
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
        
        public string SignalId { get; set; }
        public int PhaseNumber { get; set; }
        public int TotalCycles { get; }
        
        public Plan(DateTime start, DateTime end, int planNumber, Models.Approach approach, List<PhaseCycleBase> cyclesForPlan)
        {
            SignalId = approach.SignalID;
            PhaseNumber = approach.ProtectedPhaseNumber;
            PlanStart = start;
            PlanEnd = end;
            PlanNumber = planNumber;
            TotalTime = cyclesForPlan.Sum(d => d.TotalTime);
            TotalRedTime = cyclesForPlan.Sum(d => d.TotalRedTime);
            TotalYellowTime = cyclesForPlan.Sum(d => d.YellowTime);
            TotalGreenTime = cyclesForPlan.Sum(d => d.GreenTime);
            TotalVolume = cyclesForPlan.Sum(d => d.TotalVolume);
            TotalDelay = cyclesForPlan.Sum(d => d.TotalDelay);
            TotalArrivalOnRed = cyclesForPlan.Sum(d => d.TotalArrivalOnRed);
            TotalArrivalOnYellow = cyclesForPlan.Sum(d => d.TotalArrivalOnYellow);
            TotalArrivalOnGreen = cyclesForPlan.Sum(d => d.TotalArrivalOnGreen);
            TotalCycles = cyclesForPlan.Count;
        }


        public Plan(DateTime start, DateTime end, int planNumber)
        {
            PlanStart = start;
            PlanEnd = end;
            PlanNumber = planNumber;
        }


        
    }
}

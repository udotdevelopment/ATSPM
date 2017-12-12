using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.Speed
{
    public class DetectorSpeed
    {
        public List<PlanSpeed> Plans { get; set; } 
        public int TotalDetectorHits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CycleSpeed> Cycles { get; set; }
        public AvgSpeedBucketCollection AvgSpeedBucketCollection { get; set; }

        public DetectorSpeed(Models.Detector detector, DateTime startDate, DateTime endDate, int binSize, bool getPermissivePhase)
        {
            StartDate = startDate;
            EndDate = endDate;
            Cycles = CycleFactory.GetSpeedCycles(startDate, endDate, getPermissivePhase, detector);
            TotalDetectorHits = Cycles.Sum(c => c.SpeedEvents.Count);
            Plans = PlanFactory.GetSpeedPlans(Cycles,startDate, endDate, detector.Approach);
            int movementDelay = 0;
            if (detector.MovementDelay != null)
            {
                movementDelay = detector.MovementDelay.Value;
            }
            AvgSpeedBucketCollection = new AvgSpeedBucketCollection(startDate, endDate, binSize, movementDelay, Cycles);
        }

        

        
    }
}

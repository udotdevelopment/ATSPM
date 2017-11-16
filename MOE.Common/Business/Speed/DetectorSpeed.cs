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
        private readonly ISpeedEventRepository _speedEventRepository = SpeedEventRepositoryFactory.Create();
        private readonly IControllerEventLogRepository _controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
        public int TotalDetectorHits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CycleSpeed> Cycles { get; set; }
        public AvgSpeedBucketCollection AvgSpeedBucketCollection { get; set; }

        public DetectorSpeed(Models.Detector detector, DateTime startDate, DateTime endDate, int binSize, bool getPermissivePhase)
        {
            var speedEvents = _speedEventRepository.GetSpeedEventsByDetector(startDate, endDate, detector, detector.MinSpeedFilter ?? 5);
            StartDate = startDate;
            EndDate = endDate;
            TotalDetectorHits = speedEvents.Count;
            Cycles = CycleFactory.GetSpeedCycles(startDate, endDate, speedEvents, getPermissivePhase, detector.Approach);
            Plans = PlanFactory.GetSpeedPlans(Cycles,startDate, endDate, detector.Approach);
            AvgSpeedBucketCollection = new AvgSpeedBucketCollection(startDate, endDate, binSize, detector.MovementDelay.HasValue?0:detector.MovementDelay.Value, Cycles);
        }

        

        
    }
}

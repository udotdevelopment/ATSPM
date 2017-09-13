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
        public PlanCollection Plans { get; set; } 
        private readonly ISpeedEventRepository _speedEventRepository = SpeedEventRepositoryFactory.Create();
        private readonly IControllerEventLogRepository _controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
        public int TotalDetectorHits { get; set; }

        public DetectorSpeed(Models.Detector detector, DateTime startDate, DateTime endDate, int binSize)
        {
            var phaseevents = _controllerEventLogRepository.GetEventsByEventCodesParam(detector.Approach.SignalID, startDate, 
                endDate, new List<int>() { 0, 1, 7, 8, 9, 10, 11 }, detector.Approach.ProtectedPhaseNumber);
            var detEvents = new List<Controller_Event_Log>();
            var preemptEvents = new List<Controller_Event_Log>();
            var speedEvents = _speedEventRepository.GetSpeedEventsByDetector(startDate, endDate, detector);
            TotalDetectorHits = speedEvents.Count;
            Plans = new PlanCollection(phaseevents, detEvents, startDate, endDate, detector.Approach, preemptEvents);
            foreach (MOE.Common.Business.Plan plan in Plans.PlanList)
            {
                foreach (Cycle cycle in plan.CycleCollection)
                {
                    cycle.FindSpeedEventsForCycle(speedEvents);
                }
                plan.AvgSpeedBucketCollection = new AvgSpeedBucketCollection(plan.StartTime, plan.EndTime, plan.CycleCollection, 
                    binSize, detector.MinSpeedFilter ?? 5, detector.MovementDelay ?? 0);
            }
        }
    }
}

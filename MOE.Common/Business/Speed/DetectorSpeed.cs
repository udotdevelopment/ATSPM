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
        public List<Plan> Plans { get; set; } 
        private readonly ISpeedEventRepository _speedEventRepository = SpeedEventRepositoryFactory.Create();
        private readonly IControllerEventLogRepository _controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
        public int TotalDetectorHits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PhaseCycleBase> Cycles { get; set; }

        public DetectorSpeed(Models.Detector detector, DateTime startDate, DateTime endDate, int binSize)
        {
            var phaseevents = _controllerEventLogRepository.GetEventsByEventCodesParam(detector.Approach.SignalID, startDate, 
                endDate, new List<int>() { 0, 1, 7, 8, 9, 10, 11 }, detector.Approach.ProtectedPhaseNumber);
            var detEvents = new List<Controller_Event_Log>();
            var preemptEvents = new List<Controller_Event_Log>();
            var speedEvents = _speedEventRepository.GetSpeedEventsByDetector(startDate, endDate, detector, detector.MinSpeedFilter ?? 5);
            StartDate = startDate;
            EndDate = endDate;
            TotalDetectorHits = speedEvents.Count;
            Cycles = Business.PhaseCycleFactory.GetCycles(1, phaseevents, StartDate, EndDate);
            Plans = PlanFactory.GetSpeedPlans(detector.Approach.SignalID, StartDate, EndDate);
            
            foreach (Plan plan in Plans)
            {
                //foreach (Cycle cycle in plan.CycleCollection)
                //{
                //    cycle.FindSpeedEventsForCycle(speedEvents);
                //}
                plan.AvgSpeedBucketCollection = new AvgSpeedBucketCollection(plan, binSize, detector);
            }
        }


    }
}

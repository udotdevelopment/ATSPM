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
        public Cycle Cycles { get; set; }

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
            Cycles = Business.CycleFactory.GetSpeedCycles();
            Plans = PlanFactory.GetSpeedPlans();
            GetPlans(detector);
            foreach (Plan plan in Plans)
            {
                //foreach (Cycle cycle in plan.CycleCollection)
                //{
                //    cycle.FindSpeedEventsForCycle(speedEvents);
                //}
                plan.AvgSpeedBucketCollection = new AvgSpeedBucketCollection(plan, binSize, detector);
            }
        }


        private void GetPlans(Models.Detector detector)
        {
            var celRepository = ControllerEventLogRepositoryFactory.Create();
            List<Controller_Event_Log> planEvents = new List<Controller_Event_Log>();
            var firstPlanEvent = celRepository.GetFirstEventBeforeDate(detector.Approach.SignalID, 131, StartDate);
            if (firstPlanEvent != null)
            {
                firstPlanEvent.Timestamp = StartDate;
                planEvents.Add(firstPlanEvent);
            }
            else
            {
                firstPlanEvent = new Controller_Event_Log();
                firstPlanEvent.Timestamp = StartDate;
                planEvents.Add(firstPlanEvent);
            }
            var tempPlanEvents = celRepository.GetSignalEventsByEventCode(detector.Approach.SignalID, StartDate, EndDate, 131);
            if (tempPlanEvents != null)
            {
                planEvents.AddRange(tempPlanEvents.OrderBy(e => e.Timestamp).Distinct());
                AddPlans(planEvents, detector);
            }
        }

        private void AddPlans(List<Controller_Event_Log> planEvents, Models.Detector detector)
        {
            for (int i = 0; i < planEvents.Count; i++)
            {
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != EndDate)
                    {
                        //var planCycles = Cycles
                        //    .Where(c => c.StartTime >= planEvents[i].Timestamp && c.StartTime < EndDate).ToList();
                        Plans.Add(new Plan(planEvents[i].Timestamp, EndDate, planEvents[i].EventParam, detector.Approach, planCycles));
                    }
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                    {
                        //var planCycles = Cycles
                        //    .Where(c => c.StartTime >= planEvents[i].Timestamp && c.StartTime < planEvents[i + 1].Timestamp).ToList();
                        Plans.Add(new Plan(planEvents[i].Timestamp, planEvents[i + 1].Timestamp, planEvents[i].EventParam, Approach, planCycles));
                    }
                }
            }
        }
    }
}

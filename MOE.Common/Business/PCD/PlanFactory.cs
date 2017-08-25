using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.PCD
{
    public class PlanFactory
    {
        public List<Models.Controller_Event_Log> PlanEvents { get; set; }
        private List<PCD.Plan> Plans = new List<PCD.Plan>();
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private string SignalID { get; set; }

        public PlanFactory(List<Models.Controller_Event_Log> planEvents,
            DateTime startDate, DateTime endDate, string signalID)
        {
            PlanEvents = planEvents;
            StartDate = startDate;
            EndDate = endDate;
            SignalID = signalID;
            Plans = new List<Plan>();
        }

        public List<PCD.Plan> CreatePlanList()
        {
            Plans.Clear();
            for (int i = 0; i < PlanEvents.Count; i++)
            {
                if (PlanEvents.Count - 1 == i)
                {
                    AddLastPlan(PlanEvents[i]);
                }
                else
                {
                    AddPlan(PlanEvents[i], PlanEvents[i + 1]);
                }
            }
            AddStartingPlan();
            return Plans;
        }

        private void AddPlan(Models.Controller_Event_Log startPlanEvent, 
            Models.Controller_Event_Log endPlanEvent)
        {
            if (startPlanEvent.Timestamp != endPlanEvent.Timestamp)
            {
                Plan plan = new Plan(startPlanEvent.Timestamp,
                    endPlanEvent.Timestamp,
                    startPlanEvent.EventParam);
                Plans.Add(plan);
            }
        }

        private void AddLastPlan(Models.Controller_Event_Log planEvent)
        {
            if (planEvent.Timestamp != EndDate)
            {
                PCD.Plan plan = new PCD.Plan(planEvent.Timestamp,
                    EndDate, planEvent.EventParam);
                Plans.Add(plan);
            }
        }

        private void AddStartingPlan()
        {
            if (Plans.Count > 0 && Plans[0].PlanStart != StartDate)
            {
                Models.Repositories.IControllerEventLogRepository controllerEventLogRepository =
                    Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                Models.Controller_Event_Log planEvent = controllerEventLogRepository.GetFirstEventBeforeDate(SignalID,
                    131, Plans[0].PlanStart);
                if (planEvent != null)
                {
                    Plan plan = new Plan(StartDate,
                            Plans[0].PlanStart,
                            planEvent.EventParam);
                    Plans.Insert(0, plan);
                }
                else
                {
                    Plan plan = new Plan(StartDate,
                            Plans[0].PlanStart,
                            0);
                    Plans.Insert(0, plan);
                }
            }
        }
    }
}

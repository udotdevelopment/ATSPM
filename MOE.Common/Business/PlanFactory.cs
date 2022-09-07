using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public static class PlanFactory
    {
        public static List<PlanPcd> GetPcdPlans(List<CyclePcd> cycles, DateTime startDate,
            DateTime endDate, Approach approach, SPM db)
        {
            var planEvents = GetPlanEvents(startDate, endDate, approach.SignalID, db);
            var plans = new List<PlanPcd>();
            for (var i = 0; i < planEvents.Count; i++)
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != endDate)
                    {
                        var planCycles = cycles
                            .Where(c => c.StartTime >= planEvents[i].Timestamp && c.StartTime < endDate).ToList();
                        plans.Add(new PlanPcd(planEvents[i].Timestamp, endDate, planEvents[i].EventParam, planCycles));
                    }
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                    {
                        var planCycles = cycles.Where(c =>
                                c.StartTime >= planEvents[i].Timestamp && c.StartTime < planEvents[i + 1].Timestamp)
                            .ToList();
                        plans.Add(new PlanPcd(planEvents[i].Timestamp, planEvents[i + 1].Timestamp,
                            planEvents[i].EventParam, planCycles));
                    }
                }
            return plans;
        }

        public static List<Controller_Event_Log> GetPlanEvents(DateTime startDate, DateTime endDate, string signalId, SPM db)
        {
            IControllerEventLogRepository celRepository;
            if (db == null)
            {
                celRepository = ControllerEventLogRepositoryFactory.Create();
            }
            else
            {
                celRepository = ControllerEventLogRepositoryFactory.Create(db);
            }
            var planEvents = new List<Controller_Event_Log>();
            var tempPlanEvents = celRepository.GetSignalEventsByEventCode(signalId, startDate, endDate, 131)
                .OrderBy(e => e.Timestamp).ToList();
            if(tempPlanEvents.Any() &&  tempPlanEvents.First().Timestamp != startDate)
            {
                GetFirstPlan(startDate, signalId, celRepository, planEvents);
            }
            else if (!planEvents.Any())
            {
                GetFirstPlan(startDate, signalId, celRepository, planEvents);
            }
            tempPlanEvents.Add(new Controller_Event_Log { SignalID = signalId, EventCode = 131, EventParam = 254, Timestamp = endDate });

            for (var x = 0; x < tempPlanEvents.Count(); x++)
                if (x + 2 < tempPlanEvents.Count())
                {
                    if (tempPlanEvents[x].EventParam == tempPlanEvents[x + 1].EventParam)
                    {
                        planEvents.Add(tempPlanEvents[x]);
                        x++;
                    }
                    else
                    {
                        planEvents.Add(tempPlanEvents[x]);
                    }
                }
                else
                {
                    if (tempPlanEvents.Count >= 2 && tempPlanEvents.Last().EventCode ==
                        tempPlanEvents[tempPlanEvents.Count() - 2].EventCode)
                        planEvents.Add(tempPlanEvents[tempPlanEvents.Count() - 2]);
                    else
                        planEvents.Add(tempPlanEvents.Last());
                }

            return planEvents;
        }

        private static void GetFirstPlan(DateTime startDate, string signalId, IControllerEventLogRepository celRepository, List<Controller_Event_Log> planEvents)
        {
            var firstPlanEvent = celRepository.GetFirstEventBeforeDate(signalId, 131, startDate);
            if (firstPlanEvent != null)
            {
                firstPlanEvent.Timestamp = startDate;
                planEvents.Add(firstPlanEvent);
            }
            else
            {
                firstPlanEvent = new Controller_Event_Log
                {
                    Timestamp = startDate,
                    EventCode = 131,
                    EventParam = 0,
                    SignalID = signalId
                };
                planEvents.Insert(0,firstPlanEvent);
            }
        }

        public static List<Plan> GetBasicPlans(DateTime startDate, DateTime endDate, string signalId, SPM db)
        {
            var planEvents = GetPlanEvents(startDate, endDate, signalId, db);
            var plans = new List<Plan>();
            for (var i = 0; i < planEvents.Count; i++)
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != endDate)
                        plans.Add(new Plan(planEvents[i].Timestamp, endDate, planEvents[i].EventParam));
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                        plans.Add(new Plan(planEvents[i].Timestamp, planEvents[i + 1].Timestamp,
                            planEvents[i].EventParam));
                }
            return plans;
        }

        public static List<PlanSplitMonitor> GetSplitMonitorPlans(DateTime startDate, DateTime endDate, string signalId)
        {
            var planEvents = GetPlanEvents(startDate, endDate, signalId, null);
            var plans = new List<PlanSplitMonitor>();
            for (var i = 0; i < planEvents.Count; i++)
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != endDate)
                        plans.Add(new PlanSplitMonitor(planEvents[i].Timestamp, endDate, planEvents[i].EventParam));
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                        plans.Add(new PlanSplitMonitor(planEvents[i].Timestamp, planEvents[i + 1].Timestamp,
                            planEvents[i].EventParam));
                }
            return plans;
        }

        public static List<PlanSpeed> GetSpeedPlans(List<CycleSpeed> cycles, DateTime startDate,
            DateTime endDate, Approach approach)
        {
            var planEvents = GetPlanEvents(startDate, endDate, approach.SignalID, null);
            var plans = new List<PlanSpeed>();
            for (var i = 0; i < planEvents.Count; i++)
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != endDate)
                    {
                        var planCycles = cycles
                            .Where(c => c.StartTime >= planEvents[i].Timestamp && c.StartTime < endDate).ToList();
                        plans.Add(new PlanSpeed(planEvents[i].Timestamp, endDate, planEvents[i].EventParam,
                            planCycles));
                    }
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                    {
                        var planCycles = cycles.Where(c =>
                                c.StartTime >= planEvents[i].Timestamp && c.StartTime < planEvents[i + 1].Timestamp)
                            .ToList();
                        plans.Add(new PlanSpeed(planEvents[i].Timestamp, planEvents[i + 1].Timestamp,
                            planEvents[i].EventParam, planCycles));
                    }
                }
            return plans;
        }


        //public static List<Plan> GetSplitMonitorlans(List<Models.Controller_Event_Log> cycleEvents,
        //    List<Models.Controller_Event_Log> detectorEvents, DateTime startdate,
        //    DateTime enddate, Models.Approach approach, List<Models.Controller_Event_Log> preemptEvents)
        //{
        //    return new List<Plan>();
        //}

        //public PlanCollection(DateTime startdate,
        //    DateTime enddate, string signalId)
        //{
        //    GetSimplePlanCollection(startdate, enddate, signalId);
        //}

        //public void GetPlanCollection(DateTime startDate, DateTime endDate,
        //    List<Models.Controller_Event_Log> cycleEvents,
        //    List<Models.Controller_Event_Log> detectorEvents,
        //    List<Models.Controller_Event_Log> preemptEvents)
        //{
        //    MOE.Common.Business.PlansBase ds = new PlansBase(Approach.SignalID, startDate, endDate);

        //    for (int i = 0; i < ds.Events.Count; i++)
        //    {
        //        //if this is the last plan then we want the end of the plan
        //        //to cooincide with the end of the graph
        //        if (ds.Events.Count - 1 == i)
        //        {
        //            if (ds.Events[i].Timestamp != endDate)
        //            {
        //                //Plan plan = new Plan(ds.Events[i].Timestamp, endDate, ds.Events[i].EventParam,
        //                //    cycleEvents, detectorEvents, preemptEvents, Approach);
        //                //this.AddItem(plan);
        //            }
        //        }
        //        //else we add the plan with the next plans' time stamp as the end of the plan
        //        else
        //        {
        //            if (ds.Events[i].Timestamp != ds.Events[i + 1].Timestamp)
        //            {
        //                //Plan plan = new Plan(ds.Events[i].Timestamp, ds.Events[i + 1].Timestamp,
        //                //    ds.Events[i].EventParam, cycleEvents, detectorEvents, preemptEvents, Approach);
        //                //this.AddItem(plan);
        //            }

        //        }
        //    }
        //}

        //public void LinkPivotAddDetectorData(List<Models.Controller_Event_Log> detectorEvents)
        //{

         //   var cleanedEvents = RemoveDuplicateEvents(events);

        //public void GetSimplePlanCollection(DateTime startDate, DateTime endDate, string signalId)
        //{

        //    MOE.Common.Business.PlansBase ds = new PlansBase(signalId, startDate, endDate);


        //    for (int i = 0; i < ds.Events.Count; i++)
        //    {
        //        //if this is the last plan then we want the end of the plan
        //        //to cooincide with the end of the graph
        //        if (ds.Events.Count - 1 == i)
        //        {
        //            if (ds.Events[i].Timestamp != endDate)
        //            {
        //                Plan plan = new Plan(ds.Events[i].Timestamp, endDate, ds.Events[i].EventParam);
        //                this.AddItem(plan);
        //            }
        //        }
        //        //else we add the plan with the next plans' time stamp as the end of the plan
        //        else
        //        {
        //            if (ds.Events[i].Timestamp != ds.Events[i + 1].Timestamp)
        //            {
        //                Plan plan = new Plan(ds.Events[i].Timestamp, ds.Events[i + 1].Timestamp, ds.Events[i].EventParam);
        //                this.AddItem(plan);
        //            }

        //        }
        //    }
        //}

        //public void FillMissingSplits()
        //{
        //    int highestSplit = 0;
        //    foreach (Business.Plan plan in PlanList)
        //    {
        //        int testSplit = plan.FindHighestRecordedSplitPhase();
        //        if (highestSplit < testSplit)
        //        {
        //            highestSplit = testSplit;
        //        }
        //    }

        //    foreach (Business.Plan plan in PlanList)
        //    {
        //        plan.FillMissingSplits(highestSplit);
        //    }
        //}
        //public void AddItem(Plan item)
        //{
        //    this.PlanList.Add(item);
        //}

        //public static void SetSimplePlanStrips(PlanCollection PlanCollection, Chart Chart, DateTime StartDate, ControllerEventLogs EventLog)
        //{
        //    int backGroundColor = 1;
        //    foreach (MOE.Common.Business.Plan plan in PlanCollection.PlanList)
        //    {
        //        StripLine stripline = new StripLine();
        //        //Creates alternating backcolor to distinguish the plans
        //        if (backGroundColor % 2 == 0)
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightGray);
        //        }
        //        else
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
        //        }

        //        //Set the stripline properties
        //        stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
        //        stripline.Interval = 1;
        //        stripline.IntervalOffset = (plan.StartTime - StartDate).TotalHours;
        //        stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
        //        stripline.StripWidthType = DateTimeIntervalType.Hours;

        //        Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

        //        //Add a corrisponding custom label for each strip
        //        CustomLabel Plannumberlabel = new CustomLabel();
        //        Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
        //        Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
        //        switch (plan.PlanNumber)
        //        {
        //            case 254:
        //                Plannumberlabel.Text = "Free";
        //                break;
        //            case 255:
        //                Plannumberlabel.Text = "Flash";
        //                break;
        //            case 0:
        //                Plannumberlabel.Text = "Unknown";
        //                break;
        //            default:
        //                Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

        //                break;
        //        }
        //        Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        Plannumberlabel.ForeColor = Color.Black;
        //        Plannumberlabel.RowIndex = 6;


        //        Chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

        //        CustomLabel planPreemptsLabel = new CustomLabel();
        //        planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
        //        planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

        //        var c = from MOE.Common.Models.Controller_Event_Log r in EventLog.Events
        //                where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
        //                select r;


        //        string premptCount = c.Count().ToString();
        //        planPreemptsLabel.Text = "Preempts Serviced During Plan: " + premptCount;
        //        planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        planPreemptsLabel.ForeColor = Color.Red;
        //        planPreemptsLabel.RowIndex = 7;

        //        Chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

        //        backGroundColor++;

        //    }
        //}

        //public static void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate)
        //{
        //    int backGroundColor = 1;
        //    foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
        //    {
        //        StripLine stripline = new StripLine();
        //        //Creates alternating backcolor to distinguish the plans
        //        if (backGroundColor % 2 == 0)
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightGray);
        //        }
        //        else
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
        //        }

        //        //Set the stripline properties
        //        stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
        //        stripline.Interval = 1;
        //        stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
        //        stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
        //        stripline.StripWidthType = DateTimeIntervalType.Hours;

        //        chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

        //        //Add a corrisponding custom label for each strip
        //        CustomLabel Plannumberlabel = new CustomLabel();
        //        Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
        //        Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
        //        switch (plan.PlanNumber)
        //        {
        //            case 254:
        //                Plannumberlabel.Text = "Free";
        //                break;
        //            case 255:
        //                Plannumberlabel.Text = "Flash";
        //                break;
        //            case 0:
        //                Plannumberlabel.Text = "Unknown";
        //                break;
        //            default:
        //                Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

        //                break;
        //        }
        //        Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        Plannumberlabel.ForeColor = Color.Black;
        //        Plannumberlabel.RowIndex = 6;


        //        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


        //        backGroundColor++;

        //    }


        //}
        public static List<PlanSplitFail> GetSplitFailPlans(List<CycleSplitFail> cycles, SplitFailOptions options,
            Approach approach, SPM db)
        {
            var planEvents = GetPlanEvents(options.StartDate, options.EndDate, approach.SignalID, db);
            var plans = new List<PlanSplitFail>();
            for (var i = 0; i < planEvents.Count; i++)
                if (planEvents.Count - 1 == i)
                {
                    if (planEvents[i].Timestamp != options.EndDate)
                    {
                        var planCycles = cycles.Where(c =>
                            c.StartTime >= planEvents[i].Timestamp && c.StartTime < options.EndDate).ToList();
                        plans.Add(new PlanSplitFail(planEvents[i].Timestamp, options.EndDate, planEvents[i].EventParam,
                            planCycles));
                    }
                }
                else
                {
                    if (planEvents[i].Timestamp != planEvents[i + 1].Timestamp)
                    {
                        var planCycles = cycles.Where(c =>
                                c.StartTime >= planEvents[i].Timestamp && c.StartTime < planEvents[i + 1].Timestamp)
                            .ToList();
                        plans.Add(new PlanSplitFail(planEvents[i].Timestamp, planEvents[i + 1].Timestamp,
                            planEvents[i].EventParam, planCycles));
                    }
                }
            return plans;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public static class PlanFactory
    {      
        public static List<Plan> GetPcdPlans(List<Models.Controller_Event_Log> cycleEvents,
            List<Models.Controller_Event_Log> detectorEvents, DateTime startdate,
            DateTime enddate, Models.Approach approach, List<Models.Controller_Event_Log> preemptEvents)
        {
            return new List<Plan>();
        }

        public static List<PlanBase> GetPlansFromDatabase(string signalId, DateTime startDate, DateTime endDate)
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository eventLogRepo =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            List<PlanBase> plans = new List<PlanBase>();

            var events = eventLogRepo.GetSignalEventsByEventCode(signalId, startDate, endDate, 131);

            Models.Controller_Event_Log tempEvent = new Models.Controller_Event_Log
            {
                SignalID = signalId,
                Timestamp = startDate,
                EventCode = 131,
                EventParam = ControllerEventLogs.GetPreviousPlan(signalId, startDate)
            };

            events.Insert(0, tempEvent);
            //}


            var cleanedEvents = RemoveDuplicateEvents(events);


            return CreatePlansFromEvents(endDate, cleanedEvents);
        }

        private static List<PlanBase> CreatePlansFromEvents(DateTime endDate, List<Controller_Event_Log> events)
        {
            List<PlanBase>plans = new List<PlanBase>();
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].EventCode == 131)
                {
                    var p = new PlanBase();
                    p.PlanStart = events[i].Timestamp;
                    p.PlanNumber = events[i].EventCode;
                    if (i + 2 > events.Count)
                    {
                        p.PlanEnd = endDate;
                    }
                    else
                    {
                        p.PlanEnd = events[i + 1].Timestamp;
                    }
                    plans.Add(p);
                }
            }
            return plans;
        }

        private static List<Models.Controller_Event_Log> RemoveDuplicateEvents(List<Controller_Event_Log> events)
        {
            //Remove Duplicate Plans
            int x = -1;
            List<Models.Controller_Event_Log> temp = new List<Models.Controller_Event_Log>();
            foreach (Models.Controller_Event_Log cel in events)
            {
                temp.Add(cel);
            }
            foreach (Models.Controller_Event_Log cel in temp)
            {
                if (x == -1)
                {
                    x = cel.EventParam;
                }
                else if (x != cel.EventParam)
                {
                    x = cel.EventParam;
                }
                else if (x == cel.EventParam)
                {
                    x = cel.EventParam;
                    events.Remove(cel);
                }
            }

            return events;
        }

        public static List<Plan> GetSpeedPlans(List<Models.Controller_Event_Log> cycleEvents,
            List<Models.Controller_Event_Log> detectorEvents, DateTime startdate,
            DateTime enddate, Models.Approach approach, List<Models.Controller_Event_Log> preemptEvents)
        {
            return new List<Plan>();
        }

        public static List<Plan> GetSplitMonitorPlans(List<Models.Controller_Event_Log> cycleEvents,
            List<Models.Controller_Event_Log> detectorEvents, DateTime startdate,
            DateTime enddate, Models.Approach approach, List<Models.Controller_Event_Log> preemptEvents)
        {
            return new List<Plan>();
        }


        

        //public void LinkPivotAddDetectorData(List<Models.Controller_Event_Log> detectorEvents)
        //{

        //    foreach (Plans plan in this.PlanList)
        //    {
        //        plan.LinkPivotAddDetectorData(detectorEvents);
        //    }
        //}






        public static void SetSimplePlanStrips(List<Business.Plan> plans, Chart chart, DateTime startDate, ControllerEventLogs eventLog)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in plans)
            {
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.PlanStart - startDate).TotalHours;
                stripline.StripWidth = (plan.PlanEnd - plan.PlanStart).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel planNumberLabel = new CustomLabel();
                planNumberLabel.FromPosition = plan.PlanStart.ToOADate();
                planNumberLabel.ToPosition = plan.PlanEnd.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        planNumberLabel.Text = "Free";
                        break;
                    case 255:
                        planNumberLabel.Text = "Flash";
                        break;
                    case 0:
                        planNumberLabel.Text = "Unknown";
                        break;
                    default:
                        planNumberLabel.Text = "Plans " + plan.PlanNumber.ToString();

                        break;
                }
                planNumberLabel.LabelMark = LabelMarkStyle.LineSideMark;
                planNumberLabel.ForeColor = Color.Black;
                planNumberLabel.RowIndex = 6;


                chart.ChartAreas[0].AxisX2.CustomLabels.Add(planNumberLabel);

                CustomLabel planPreemptsLabel = new CustomLabel();
                planPreemptsLabel.FromPosition = plan.PlanStart.ToOADate();
                planPreemptsLabel.ToPosition = plan.PlanEnd.ToOADate();

                var c = from MOE.Common.Models.Controller_Event_Log r in eventLog.Events
                        where r.EventCode == 107 && r.Timestamp > plan.PlanStart && r.Timestamp < plan.PlanEnd
                        select r;



                string premptCount = c.Count().ToString();
                planPreemptsLabel.Text = "Preempts Serviced During Plans: " + premptCount;
                planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                planPreemptsLabel.ForeColor = Color.Red;
                planPreemptsLabel.RowIndex = 7;

                chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

                backGroundColor++;

            }
        }

        public static void SetSimplePlanStrips(List<PlanBase> plans, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.PlanBase plan in plans)
            {
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.PlanStart - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.PlanEnd - plan.PlanStart).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.PlanStart.ToOADate();
                Plannumberlabel.ToPosition = plan.PlanEnd.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        Plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        Plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        Plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        Plannumberlabel.Text = "Plans " + plan.PlanNumber.ToString();

                        break;
                }
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                backGroundColor++;

            }


        }
    }
}

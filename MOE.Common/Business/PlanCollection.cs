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

namespace MOE.Common.Business
{
    public class PlanCollection
    {      

        public List<Plan> PlanList = new List<Plan>();


        private Models.Approach approach = new Models.Approach();
        public Models.Approach Approach
        {
            get { return approach; }
        }


        public PlanCollection(List<Models.Controller_Event_Log> cycleEvents,
            List<Models.Controller_Event_Log> detectorEvents, DateTime startdate,
            DateTime enddate, Models.Approach approach, List<Models.Controller_Event_Log> preemptEvents)
        {
            this.approach = approach;
            GetPlanCollection(startdate, enddate,
                cycleEvents, detectorEvents, preemptEvents);
        }

        public PlanCollection(DateTime startdate,
            DateTime enddate, string signalId)
        {
            GetSimplePlanCollection(startdate, enddate, signalId);
        }

        public void GetPlanCollection(DateTime startDate, DateTime endDate,
            List<Models.Controller_Event_Log> cycleEvents,
            List<Models.Controller_Event_Log> detectorEvents,
            List<Models.Controller_Event_Log> preemptEvents)
        {
            MOE.Common.Business.PlansBase ds = new PlansBase(approach.SignalID, startDate, endDate);

            for (int i = 0; i < ds.Events.Count; i++)
            {
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (ds.Events.Count - 1 == i)
                {
                    if (ds.Events[i].Timestamp != endDate)
                    {
                        Plan plan = new Plan(ds.Events[i].Timestamp, endDate, ds.Events[i].EventParam,
                            cycleEvents, detectorEvents, preemptEvents, Approach);
                        this.AddItem(plan);
                    }
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {
                    if (ds.Events[i].Timestamp != ds.Events[i + 1].Timestamp)
                    {
                        Plan plan = new Plan(ds.Events[i].Timestamp, ds.Events[i + 1].Timestamp,
                            ds.Events[i].EventParam, cycleEvents, detectorEvents, preemptEvents, Approach);
                        this.AddItem(plan);
                    }

                }
            }
        }

        public void LinkPivotAddDetectorData(List<Models.Controller_Event_Log> detectorEvents)
        {

            foreach (Plan plan in this.PlanList)
            {
                plan.LinkPivotAddDetectorData(detectorEvents);
            }
        }

        public void GetSimplePlanCollection(DateTime startDate, DateTime endDate, string signalId)
        {

            MOE.Common.Business.PlansBase ds = new PlansBase(signalId, startDate, endDate);


            for (int i = 0; i < ds.Events.Count; i++)
            {
                //if this is the last plan then we want the end of the plan
                //to cooincide with the end of the graph
                if (ds.Events.Count - 1 == i)
                {
                    if (ds.Events[i].Timestamp != endDate)
                    {
                        Plan plan = new Plan(ds.Events[i].Timestamp, endDate, ds.Events[i].EventParam);
                        this.AddItem(plan);
                    }
                }
                //else we add the plan with the next plans' time stamp as the end of the plan
                else
                {
                    if (ds.Events[i].Timestamp != ds.Events[i + 1].Timestamp)
                    {
                        Plan plan = new Plan(ds.Events[i].Timestamp, ds.Events[i + 1].Timestamp, ds.Events[i].EventParam);
                        this.AddItem(plan);
                    }

                }
            }
        }

        public void FillMissingSplits()
        {
            int highestSplit = 0;
            foreach (Business.Plan plan in PlanList)
            {
                int testSplit = plan.FindHighestRecordedSplitPhase();
                if (highestSplit < testSplit)
                {
                    highestSplit = testSplit;
                }
            }

            foreach (Business.Plan plan in PlanList)
            {
                plan.FillMissingSplits(highestSplit);
            }
        }
        public void AddItem(Plan item)
        {
            this.PlanList.Add(item);
        }

        public static void SetSimplePlanStrips(PlanCollection PlanCollection, Chart Chart, DateTime StartDate, ControllerEventLogs EventLog)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in PlanCollection.PlanList)
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
                stripline.IntervalOffset = (plan.StartTime - StartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                Chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

                CustomLabel planPreemptsLabel = new CustomLabel();
                planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
                planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

                var c = from MOE.Common.Models.Controller_Event_Log r in EventLog.Events
                        where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
                        select r;



                string premptCount = c.Count().ToString();
                planPreemptsLabel.Text = "Preempts Serviced During Plan: " + premptCount;
                planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                planPreemptsLabel.ForeColor = Color.Red;
                planPreemptsLabel.RowIndex = 7;

                Chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

                backGroundColor++;

            }
        }

        public static void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
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
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

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

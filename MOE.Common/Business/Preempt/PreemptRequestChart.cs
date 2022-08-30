using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.Preempt
{
    public class PreemptRequestChart
    {
        public Chart PreemptServiceRequestChart { get; set; }

        public PreemptRequestChart(PreemptServiceRequestOptions options, ControllerEventLogs dttb)
        {
            Options = options;
            //Set the chart properties
            PreemptServiceRequestChart = ChartFactory.CreateDefaultChart(options);
            PreemptServiceRequestChart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            ChartFactory.SetImageProperties(PreemptServiceRequestChart);
            PreemptServiceRequestChart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            PreemptServiceRequestChart.BorderSkin.BorderColor = Color.Black;
            PreemptServiceRequestChart.BorderSkin.BorderWidth = 1;
            var reportTimespan = Options.EndDate - Options.StartDate;

            SetChartTitle();

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            PreemptServiceRequestChart.Legends.Add(chartLegend);
            PreemptServiceRequestChart.ChartAreas[0].AxisY.Maximum = 10;
            PreemptServiceRequestChart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.False;

            // top chart

            PreemptServiceRequestChart.ChartAreas[0].AxisY.Minimum = 0;
            PreemptServiceRequestChart.ChartAreas[0].AxisY.Title = "Preempt Number";
            PreemptServiceRequestChart.ChartAreas[0].AxisY.Interval = 1;
            PreemptServiceRequestChart.ChartAreas[0].AxisX.Title = "Time (Hours:Minutes)";
            PreemptServiceRequestChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
            PreemptServiceRequestChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm";
            if (reportTimespan.Days <= 1)
                if (reportTimespan.Hours > 1)
                    PreemptServiceRequestChart.ChartAreas[0].AxisX.Interval = 1;
                else
                    PreemptServiceRequestChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm";
            PreemptServiceRequestChart.ChartAreas[0].AxisX.Minimum = Options.StartDate.ToOADate();
            PreemptServiceRequestChart.ChartAreas[0].AxisX.Maximum = Options.EndDate.ToOADate();

            //Add the point series

            var PreemptSeries = new Series();
            PreemptSeries.ChartType = SeriesChartType.Point;
            PreemptSeries.BorderDashStyle = ChartDashStyle.Dash;
            PreemptSeries.MarkerStyle = MarkerStyle.Diamond;
            PreemptSeries.Color = Color.Black;
            PreemptSeries.Name = "Preempt Request";
            PreemptSeries.XValueType = ChartValueType.DateTime;


            //Add the Posts series to ensure the chart is the size of the selected timespan
            var posts = new Series();
            posts.IsVisibleInLegend = false;
            posts.ChartType = SeriesChartType.Point;
            posts.Color = Color.White;
            posts.Name = "Posts";
            posts.XValueType = ChartValueType.DateTime;

            PreemptServiceRequestChart.Series.Add(posts);
            PreemptServiceRequestChart.Series.Add(PreemptSeries);
            PreemptServiceRequestChart.Height = 200;
            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            //chart.Series["Posts"].Points.AddXY(Options.StartDate, 0);
            //chart.Series["Posts"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(PreemptServiceRequestChart, Options.StartDate, Options.EndDate, dttb, Options.SignalID);
            var plans = PlanFactory.GetBasicPlans(Options.StartDate, Options.EndDate, Options.SignalID, null);
            SetSimplePlanStrips(plans, PreemptServiceRequestChart, Options.StartDate, dttb);
        }

        public PreemptServiceRequestOptions Options { get; set; }

        private void SetChartTitle()
        {
            PreemptServiceRequestChart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            PreemptServiceRequestChart.Titles.Add(
                ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
        }

        protected void AddDataToChart(Chart chart, DateTime startDate,
            DateTime endDate, ControllerEventLogs DTTB, string signalid)
        {
            var maxprempt = 0;
            foreach (var row in DTTB.Events)
                if (row.EventCode == 102)
                {
                    chart.Series["Preempt Request"].Points.AddXY(row.Timestamp, row.EventParam);
                    if (row.EventParam > maxprempt)
                        maxprempt = row.EventParam;
                }
            if (maxprempt > 10)
                chart.ChartAreas[0].AxisY.Maximum = maxprempt;
            else
                chart.ChartAreas[0].AxisY.Maximum = 10;
        }

        protected void SetSimplePlanStrips(List<Plan> plans, Chart chart, DateTime graphStartDate,
            ControllerEventLogs DTTB)
        {
            var backGroundColor = 1;
            foreach (var plan in plans)
            {
                var stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                else
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                var Plannumberlabel = new CustomLabel();
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
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber;

                        break;
                }
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;

                var planPreemptsLabel = new CustomLabel();
                planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
                planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

                var c = from Controller_Event_Log r in DTTB.Events
                        where r.EventCode == 102 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
                        select r;

                var premptCount = c.Count().ToString();
                planPreemptsLabel.Text = "Preempts Requested During Plan: " + premptCount;
                planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                planPreemptsLabel.ForeColor = Color.Red;
                planPreemptsLabel.RowIndex = 7;

                backGroundColor++;
            }
        }
    }
}
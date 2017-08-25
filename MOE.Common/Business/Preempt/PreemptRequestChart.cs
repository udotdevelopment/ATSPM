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

namespace MOE.Common.Business.Preempt
{
    public class PreemptRequestChart
    {
        public Chart chart = new Chart();
        public WCFServiceLibrary.PreemptServiceRequestOptions Options { get; set; }

        public PreemptRequestChart(WCFServiceLibrary.PreemptServiceRequestOptions options,
            MOE.Common.Business.ControllerEventLogs DTTB)
        {
            this.Options = options;
            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 200;
            chart.Width = 1100;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;
            TimeSpan reportTimespan = Options.EndDate - Options.StartDate;

            SetChartTitle();        
            
            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);

            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            //if (double.TryParse(yAxisMax, out y))
            //{
            //    chartArea.AxisY.Maximum = y;
            //}
            //else
            //{
            chartArea.AxisY.Maximum = 10;
            //}
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Preempt Number";
            chartArea.AxisY.Interval = 1;
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            if (reportTimespan.Days < 1)
            {
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                }
            }

            chart.ChartAreas.Add(chartArea);

            //Add the point series

            Series PreemptSeries = new Series();
            PreemptSeries.ChartType = SeriesChartType.Point;
            PreemptSeries.BorderDashStyle = ChartDashStyle.Dash;
            PreemptSeries.MarkerStyle = MarkerStyle.Diamond;
            PreemptSeries.Color = Color.Black;
            PreemptSeries.Name = "Preempt Request";
            PreemptSeries.XValueType = ChartValueType.DateTime;

            

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series posts = new Series();
            posts.IsVisibleInLegend = false;
            posts.ChartType = SeriesChartType.Point;
            posts.Color = Color.White;
            posts.Name = "Posts";
            posts.XValueType = ChartValueType.DateTime;

            chart.Series.Add(posts);
            chart.Series.Add(PreemptSeries);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(Options.StartDate, 0);
            chart.Series["Posts"].Points.AddXY(Options.EndDate, 0);

            AddDataToChart(chart, Options.StartDate, Options.EndDate, DTTB, Options.SignalID);
            PlanCollection plans = new PlanCollection(Options.StartDate, Options.EndDate, Options.SignalID);
            SetSimplePlanStrips(plans, chart, Options.StartDate, DTTB);

        }

        private void SetChartTitle()
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
        }

        protected void AddDataToChart(Chart chart,  DateTime startDate,
     DateTime endDate,  MOE.Common.Business.ControllerEventLogs DTTB, string signalid)
        {
            int maxprempt = 0;
            foreach (MOE.Common.Models.Controller_Event_Log row in DTTB.Events)
            {
                if (row.EventCode == 102)
                {
                    chart.Series["Preempt Request"].Points.AddXY(row.Timestamp, row.EventParam);
                    if (row.EventParam > maxprempt)
                    {
                        maxprempt = row.EventParam;
                    }
                }
            }
            if (maxprempt > 10)
            {
                chart.ChartAreas[0].AxisY.Maximum = maxprempt;
            }
            else
            {
                chart.ChartAreas[0].AxisY.Maximum = 10;
            }
        }

        protected void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate, MOE.Common.Business.ControllerEventLogs DTTB)
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


                chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

                CustomLabel planPreemptsLabel = new CustomLabel();
                planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
                planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

                var c = from MOE.Common.Models.Controller_Event_Log r in DTTB.Events
                        where r.EventCode == 102 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
                        select r;



                string premptCount = c.Count().ToString();
                planPreemptsLabel.Text = "Preempts Requested During Plan: " + premptCount;
                planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                planPreemptsLabel.ForeColor = Color.Red;
                planPreemptsLabel.RowIndex = 7;

                chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

                backGroundColor++;

            }
        }

    }
}

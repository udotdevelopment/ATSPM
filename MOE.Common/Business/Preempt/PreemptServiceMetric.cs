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
    public class PreemptServiceMetric
    {
        public Chart chart = new Chart();
        public WCFServiceLibrary.PreemptServiceMetricOptions Options { get; set; }

        public PreemptServiceMetric(WCFServiceLibrary.PreemptServiceMetricOptions options,
            MOE.Common.Business.ControllerEventLogs  DTTB)
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
            PreemptSeries.Name = "Preempt Service";
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
            PlanCollection.SetSimplePlanStrips(plans, chart, Options.StartDate, DTTB);

        }

        private void SetChartTitle()
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(Options.SignalID, Options.StartDate, Options.EndDate));
        }

        private string GetSignalLcation(string SignalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository sr = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();

            string location = sr.GetSignalLocation(SignalID);

            return location;
            
        }

        protected void AddDataToChart(Chart chart,  DateTime startDate,
     DateTime endDate, MOE.Common.Business.ControllerEventLogs DTTB, string signalid)
        {
            int maxprempt = 0;
            foreach (MOE.Common.Models.Controller_Event_Log row in DTTB.Events)
            {
                if (row.EventCode == 105)
                {
                    chart.Series["Preempt Service"].Points.AddXY(row.Timestamp, row.EventParam);
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






    }
}

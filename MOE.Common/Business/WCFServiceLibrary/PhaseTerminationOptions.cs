using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PhaseTerminationOptions : MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Consecutive Count")]
        public int SelectedConsecutiveCount { get; set; }
        [DataMember]
        public List<int> ConsecutiveCountList { get; set; }
        [DataMember]
        [Display(Name = "Show Ped Activity")]
        public bool ShowPedActivity { get; set; }
        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStripes { get; set; }
        [DataMember]
        public bool ShowArrivalsOnGreen { get; set; }


        public PhaseTerminationOptions(DateTime startDate,
            double yAxisMax,
            DateTime endDate,
            string signalId,
            bool showPedActivity,
            int consecutiveCount,
            bool showPlanStripes)
        {
            SignalID = signalId;
            YAxisMax = yAxisMax;
            YAxisMin = 0;
            Y2AxisMax = 0;
            Y2AxisMin = 0;
            MetricTypeID = 1;
            //ConsecutiveCount = consecutiveCount;
            ShowPedActivity = showPedActivity;
            ShowPlanStripes = showPlanStripes;
        }

        public PhaseTerminationOptions()
        {
            ConsecutiveCountList = new List<int>();
            ConsecutiveCountList.Add(1);
            ConsecutiveCountList.Add(2);
            ConsecutiveCountList.Add(3);
            ConsecutiveCountList.Add(4);
            ConsecutiveCountList.Add(5);
            MetricTypeID = 1;
            ShowArrivalsOnGreen = true;
            SetDefaults();
        }
        public void SetDefaults()
        {
            ShowPlanStripes = true;
            ShowPedActivity = true;
            SelectedConsecutiveCount = 1;
            CreateLegend();
        }

        private void CreateLegend()
        {
            Chart dummychart = new Chart();
            ChartArea chartarea1 = new ChartArea();
            dummychart.ImageType = ChartImageType.Jpeg;
            dummychart.Height = 200;

            dummychart.Width = 1100;

            dummychart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            dummychart.BorderlineDashStyle = ChartDashStyle.Dot;

            Series PedActivity = new Series();
            Series GapoutSeries = new Series();
            Series MaxOutSeries = new Series();
            Series ForceOffSeries = new Series();
            Series UnknownSeries = new Series();

            PedActivity.Name = "Ped Activity";
            GapoutSeries.Name = "Gap Out";
            MaxOutSeries.Name = "Max Out";
            ForceOffSeries.Name = "Force Off";
            UnknownSeries.Name = "Unknown";


            PedActivity.MarkerStyle = MarkerStyle.Cross;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            MaxOutSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            UnknownSeries.MarkerStyle = MarkerStyle.Circle;

            GapoutSeries.Color = Color.OliveDrab;
            PedActivity.Color = Color.DarkGoldenrod;
            MaxOutSeries.Color = Color.Red;
            ForceOffSeries.Color = Color.MediumBlue;           
            UnknownSeries.Color = Color.FromArgb(255,255,0);

        
            dummychart.Series.Add(GapoutSeries);
            dummychart.Series.Add(MaxOutSeries);
            dummychart.Series.Add(ForceOffSeries);
            dummychart.Series.Add(UnknownSeries);
            dummychart.Series.Add(PedActivity);


            dummychart.ChartAreas.Add(chartarea1);

            Legend dummychartLegend = new Legend();
            dummychartLegend.Name = "DummyLegend";

            dummychartLegend.IsDockedInsideChartArea = true;

            dummychartLegend.Title = "Chart Legend";
            dummychartLegend.Docking = Docking.Top;
            dummychartLegend.Alignment = StringAlignment.Center;
            dummychart.Legends.Add(dummychartLegend);



            dummychart.SaveImage(MetricFileLocation + "PPTLegend.jpeg", System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

            ReturnList.Add(MetricWebPath + "PPTLegend.jpeg");

      
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            string location = GetSignalLocation();
            Chart chart = new Chart();


            MOE.Common.Business.AnalysisPhaseCollection analysisPhaseCollection =
                           new MOE.Common.Business.AnalysisPhaseCollection(SignalID, StartDate,
                               EndDate, SelectedConsecutiveCount);

            //If there are phases in the collection add the charts
            if (analysisPhaseCollection.Items.Count > 0)
            {
                chart = GetNewTermEventChart(StartDate, EndDate, SignalID, location,
                    SelectedConsecutiveCount, analysisPhaseCollection.MaxPhaseInUse, ShowPedActivity);

                AddTermEventDataToChart(chart, StartDate, EndDate, analysisPhaseCollection, SignalID,
                    ShowPedActivity, ShowPlanStripes);
            }

            string chartName = CreateFileName();
            List<Title> removethese = new List<Title>();

            foreach (Title t in chart.Titles)
            {
                if (t.Text == "" || t.Text == null)
                {
                    removethese.Add(t);
                }
            }
            foreach (Title t in removethese)
            {
                chart.Titles.Remove(t);
            }

            //Save an image of the chart
            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

            ReturnList.Add(MetricWebPath + chartName);

            return ReturnList;
        }

        protected Chart GetNewTermEventChart(DateTime graphStartDate, DateTime graphEndDate,
       string signalId, string location, int consecutiveCount,
       int maxPhaseInUse, bool showPedWalkStartTime)
        {
            Chart chart = new Chart();
            string extendedDirection = string.Empty;

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineDashStyle = ChartDashStyle.Dot;
            TimeSpan reportTimespan = EndDate - StartDate;

            SetChartTitle(chart);
            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Maximum = (maxPhaseInUse + .5);
            chartArea.AxisY.Minimum = -.5;

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            if (reportTimespan.Days < 1)
            {
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX2.Interval = 1;
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                    chartArea.AxisX2.LabelStyle.Format = "HH:mm";
                }
            }
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;

            chartArea.AxisY.Title = "Phase Number";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.Interval = 1;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.IntervalOffset = .5;

            chart.ChartAreas.Add(chartArea);

            Series GapoutSeries = new Series();
            GapoutSeries.ChartType = SeriesChartType.Point;
            GapoutSeries.Color = Color.OliveDrab;
            GapoutSeries.Name = "GapOut";
            GapoutSeries.XValueType = ChartValueType.DateTime;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            GapoutSeries.MarkerSize = 3;

            Series MaxOutSeries = new Series();
            MaxOutSeries.ChartType = SeriesChartType.Point;
            MaxOutSeries.Color = Color.Red;
            MaxOutSeries.Name = "MaxOut";
            MaxOutSeries.XValueType = ChartValueType.DateTime;
            MaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            MaxOutSeries.MarkerSize = 4;

            Series ForceOffSeries = new Series();
            ForceOffSeries.ChartType = SeriesChartType.Point;
            ForceOffSeries.Color = Color.MediumBlue;
            ForceOffSeries.Name = "ForceOff";
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerSize = 4;

            Series UnknownTermination = new Series();
            UnknownTermination.ChartType = SeriesChartType.Point;
            UnknownTermination.Color = Color.FromArgb(255, 255, 0); ;
            UnknownTermination.MarkerBorderColor = Color.FromArgb(255, 255, 0); ;
           
            UnknownTermination.Name = "Unknown";
            UnknownTermination.MarkerStyle = MarkerStyle.Circle;
            UnknownTermination.MarkerSize = 4;

            Series PedSeries = new Series();
            PedSeries.ChartType = SeriesChartType.Point;
            PedSeries.Color = Color.OrangeRed;
            PedSeries.Name = "Ped Walk Begin";
            //PedSeries.MarkerImage = 
            PedSeries.MarkerStyle = MarkerStyle.Triangle;
            PedSeries.MarkerSize = 3;

            if (showPedWalkStartTime)
            {
                PedSeries.IsVisibleInLegend = true;
            }
            else
            {
                PedSeries.IsVisibleInLegend = false;
            }


            

            
            chart.Series.Add(GapoutSeries);
            chart.Series.Add(MaxOutSeries);
            chart.Series.Add(ForceOffSeries);
            chart.Series.Add(PedSeries);
            chart.Series.Add(UnknownTermination);
            

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;
            //GapoutSeries.MarkerSize = 0;
            chart.Series.Add(testSeries);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);
            return chart;
        }

        private void SetChartTitle(Chart chart)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(this.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(this.SignalID, this.StartDate, this.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetTitle("Currently showing Force-Offs, Max-Outs and Gap-Outs with a consecutive occurrence of " +
                this.SelectedConsecutiveCount.ToString() + " or more. \n  Pedestrian events are never filtered"));
        }

        protected void AddTermEventDataToChart(Chart chart, DateTime startDate,
          DateTime endDate, MOE.Common.Business.AnalysisPhaseCollection analysisPhaseCollection,
            string signalId, bool showVolume, bool showPlanStripes)
        {
            foreach (MOE.Common.Business.AnalysisPhase phase in analysisPhaseCollection.Items)
            {

                if (phase.TerminationEvents.Count > 0)
                {

                    foreach (MOE.Common.Models.Controller_Event_Log TermEvent in phase.ConsecutiveGapOuts)
                    {

                        chart.Series["GapOut"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Models.Controller_Event_Log TermEvent in phase.ConsecutiveMaxOut)
                    {
                        chart.Series["MaxOut"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Models.Controller_Event_Log TermEvent in phase.ConsecutiveForceOff)
                    {
                        chart.Series["ForceOff"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);
                    }

                    foreach (MOE.Common.Models.Controller_Event_Log TermEvent in phase.UnknownTermination)
                    {
                        chart.Series["Unknown"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);
                    }

                    if (ShowPedActivity)
                    {
                        foreach (MOE.Common.Models.Controller_Event_Log PedEvent in phase.PedestrianEvents)
                        {
                            if (PedEvent.EventCode == 23)
                            {
                                chart.Series["Ped Walk Begin"].Points.AddXY(PedEvent.Timestamp, (phase.PhaseNumber + .3));
                            }
                        }
                    }

                }
                if (showPlanStripes)
                {
                    PlanCollection.SetSimplePlanStrips(analysisPhaseCollection.Plans, chart, startDate);
                }
                if (YAxisMax != null)
                {
                    chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value + .5;
                }
            }

        }

    }

}

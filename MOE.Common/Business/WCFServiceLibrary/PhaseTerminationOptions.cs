using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PhaseTerminationOptions : MetricOptions
    {
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
            ConsecutiveCountList = new List<int>() { 1, 2, 3, 4, 5};
            MetricTypeID = 1;
            ShowArrivalsOnGreen = true;
            SetDefaults();
        }

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

        private void CreateLegend()
        {
            var dummychart = new Chart();
            var chartarea1 = new ChartArea();
            ChartFactory.SetImageProperties(dummychart);
            dummychart.BorderlineDashStyle = ChartDashStyle.Dot;

            var PedActivity = new Series();
            var GapoutSeries = new Series();
            var MaxOutSeries = new Series();
            var ForceOffSeries = new Series();
            var UnknownSeries = new Series();

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
            UnknownSeries.Color = Color.FromArgb(255, 255, 0);


            dummychart.Series.Add(GapoutSeries);
            dummychart.Series.Add(MaxOutSeries);
            dummychart.Series.Add(ForceOffSeries);
            dummychart.Series.Add(UnknownSeries);
            dummychart.Series.Add(PedActivity);


            dummychart.ChartAreas.Add(chartarea1);

            var dummychartLegend = new Legend();
            dummychartLegend.Name = "DummyLegend";

            dummychartLegend.IsDockedInsideChartArea = true;

            dummychartLegend.Title = "Chart Legend";
            dummychartLegend.Docking = Docking.Top;
            dummychartLegend.Alignment = StringAlignment.Center;
            dummychart.Legends.Add(dummychartLegend);

            dummychart.Height = 100;
            dummychart.SaveImage(MetricFileLocation + "PPTLegend.jpeg", ChartImageFormat.Jpeg);

            ReturnList.Add(MetricWebPath + "PPTLegend.jpeg");
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var location = GetSignalLocation();
            var chart = ChartFactory.CreateDefaultChartNoX2Axis(this);

            CreateLegend();
            var analysisPhaseCollection =
                new AnalysisPhaseCollection(SignalID, StartDate,
                    EndDate, SelectedConsecutiveCount);

            //If there are phases in the collection add the charts
            if (analysisPhaseCollection.Items.Count > 0)
            {
                chart = GetNewTermEventChart(StartDate, EndDate, SignalID, location,
                    SelectedConsecutiveCount, analysisPhaseCollection.MaxPhaseInUse, ShowPedActivity);

                AddTermEventDataToChart(chart, StartDate, EndDate, analysisPhaseCollection, SignalID,
                    ShowPedActivity, ShowPlanStripes);
            }

            var chartName = CreateFileName();
            var removethese = new List<Title>();

            foreach (var t in chart.Titles)
                if (t.Text == "" || t.Text == null)
                    removethese.Add(t);
            foreach (var t in removethese)
                chart.Titles.Remove(t);

            //Save an image of the chart
            chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

            ReturnList.Add(MetricWebPath + chartName);

            return ReturnList;
        }

        protected Chart GetNewTermEventChart(DateTime graphStartDate, DateTime graphEndDate,
            string signalId, string location, int consecutiveCount,
            int maxPhaseInUse, bool showPedWalkStartTime)
        {
            var chart = ChartFactory.CreateDefaultChartNoX2Axis(this);

            

            //Set the chart properties
            //ChartFactory.SetImageProperties(chart);
            chart.BorderlineDashStyle = ChartDashStyle.Dot;
            //var reportTimespan = EndDate - StartDate;

            SetChartTitle(chart);
            //Create the chart area
            
            chart.ChartAreas[0].AxisY.Maximum = maxPhaseInUse + .5;
            chart.ChartAreas[0].AxisY.Minimum = -.5;
            //chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
            /*
            chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = true;
            chart.ChartAreas[0].AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chart.ChartAreas[0].AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            */
            chart.ChartAreas[0].AxisY.Title = "Phase Number";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart.ChartAreas[0].AxisY.Interval = 1;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart.ChartAreas[0].AxisY.IntervalOffset = .5;

            
            var GapoutSeries = new Series();
            GapoutSeries.ChartType = SeriesChartType.Point;
            GapoutSeries.Color = Color.OliveDrab;
            GapoutSeries.Name = "GapOut";
            GapoutSeries.XValueType = ChartValueType.DateTime;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            GapoutSeries.MarkerSize = 3;

            var MaxOutSeries = new Series();
            MaxOutSeries.ChartType = SeriesChartType.Point;
            MaxOutSeries.Color = Color.Red;
            MaxOutSeries.Name = "MaxOut";
            MaxOutSeries.XValueType = ChartValueType.DateTime;
            MaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            MaxOutSeries.MarkerSize = 4;

            var ForceOffSeries = new Series();
            ForceOffSeries.ChartType = SeriesChartType.Point;
            ForceOffSeries.Color = Color.MediumBlue;
            ForceOffSeries.Name = "ForceOff";
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerSize = 4;

            var UnknownTermination = new Series();
            UnknownTermination.ChartType = SeriesChartType.Point;
            UnknownTermination.Color = Color.FromArgb(255, 255, 0);
            ;
            UnknownTermination.MarkerBorderColor = Color.FromArgb(255, 255, 0);
            ;

            UnknownTermination.Name = "Unknown";
            UnknownTermination.MarkerStyle = MarkerStyle.Circle;
            UnknownTermination.MarkerSize = 4;

            var PedSeries = new Series();
            PedSeries.ChartType = SeriesChartType.Point;
            PedSeries.Color = Color.OrangeRed;
            PedSeries.Name = "Ped Walk Begin";
            //PedSeries.MarkerImage = 
            PedSeries.MarkerStyle = MarkerStyle.Triangle;
            PedSeries.MarkerSize = 3;

            if (showPedWalkStartTime)
                PedSeries.IsVisibleInLegend = true;
            else
                PedSeries.IsVisibleInLegend = false;


            chart.Series.Add(GapoutSeries);
            chart.Series.Add(MaxOutSeries);
            chart.Series.Add(ForceOffSeries);
            chart.Series.Add(PedSeries);
            chart.Series.Add(UnknownTermination);

            //var dummychartLegend = new Legend();

            //dummychartLegend.IsDockedInsideChartArea = true;

            //dummychartLegend.Title = "Chart Legend";
            //dummychartLegend.Docking = Docking.Top;
            //dummychartLegend.Alignment = StringAlignment.Center;
            //chart.Legends.Add(dummychartLegend);


            //Add the Posts series to ensure the chart is the size of the selected timespan
            var testSeries = new Series();
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
            chart.Titles.Add(ChartTitleFactory.GetChartName(MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(SignalID, StartDate, EndDate));
            chart.Titles.Add(ChartTitleFactory.GetTitle(
                "Currently showing Force-Offs, Max-Outs and Gap-Outs with a consecutive occurrence of " +
                SelectedConsecutiveCount + " or more. \n  Pedestrian events are never filtered"));
        }

        protected void AddTermEventDataToChart(Chart chart, DateTime startDate,
            DateTime endDate, AnalysisPhaseCollection analysisPhaseCollection,
            string signalId, bool showVolume, bool showPlanStripes)
        {
            foreach (var phase in analysisPhaseCollection.Items)
            {
                if (phase.TerminationEvents.Count > 0)
                {
                    foreach (var TermEvent in phase.ConsecutiveGapOuts)
                        chart.Series["GapOut"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);

                    foreach (var TermEvent in phase.ConsecutiveMaxOut)
                        chart.Series["MaxOut"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);

                    foreach (var TermEvent in phase.ConsecutiveForceOff)
                        chart.Series["ForceOff"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);

                    foreach (var TermEvent in phase.UnknownTermination)
                        chart.Series["Unknown"].Points.AddXY(TermEvent.Timestamp, phase.PhaseNumber);

                    if (ShowPedActivity)
                        foreach (var PedEvent in phase.PedestrianEvents)
                            if (PedEvent.EventCode == 23)
                                chart.Series["Ped Walk Begin"].Points.AddXY(PedEvent.Timestamp, phase.PhaseNumber + .3);
                }
                if (showPlanStripes)
                    SetSimplePlanStrips(analysisPhaseCollection.Plans, chart, startDate);
                if (YAxisMax != null)
                    chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value + .5;
            }
        }

        public static void SetSimplePlanStrips(List<PlanSplitMonitor> plans, Chart chart, DateTime graphStartDate)
        {
            var backGroundColor = 1;
            foreach (Plan plan in plans)
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


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                backGroundColor++;
            }
        }
    }
}
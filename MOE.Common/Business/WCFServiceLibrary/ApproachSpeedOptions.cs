using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Speed;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachSpeedOptions : MetricOptions
    {
        public ApproachSpeedOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax,
            double yAxisMin,
            int binSize, int dotSize, bool showPlanStatistics, int metricTypeID, bool showPostedSpeed,
            bool showAverageSpeed, bool show85Percentile, bool show15Percentile)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;
            YAxisMin = yAxisMin;
            SelectedBinSize = binSize;
            ShowPlanStatistics = showPlanStatistics;
            ShowPostedSpeed = showPostedSpeed;
            ShowAverageSpeed = showAverageSpeed;
            Show85Percentile = show85Percentile;
            Show15Percentile = show15Percentile;
            MetricTypeID = metricTypeID;
        }


        public ApproachSpeedOptions()
        {
            BinSizeList = new List<int>() { 5, 15 };
            MetricTypeID = 10;
            SetDefaults();
        }

        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }

        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }

        [DataMember]
        [Display(Name = "Show Posted Speed")]
        public bool ShowPostedSpeed { get; set; }

        [DataMember]
        [Display(Name = "Show Average Speed")]
        public bool ShowAverageSpeed { get; set; }

        [DataMember]
        [Display(Name = "Show 85% Speeds")]
        public bool Show85Percentile { get; set; }

        [DataMember]
        [Display(Name = "Show 15% Speeds")]
        public bool Show15Percentile { get; set; }

        public List<DetectorSpeed> SpeedDetectors { get; set; } = new List<DetectorSpeed>();

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var sr = SignalsRepositoryFactory.Create();
            var signal = sr.GetVersionOfSignalByDate(SignalID, StartDate);
            var speedApproaches = signal.GetApproachesForSignalThatSupportMetric(10);
            if (speedApproaches.Count > 0)
                foreach (var approach in speedApproaches)
                {
                    var speedDets = approach.GetDetectorsForMetricType(10);
                    foreach (var det in speedDets)
                    {
                        var chart = GetNewSpeedChart(det);
                        AddSpeedDataToChart(chart, det, StartDate, EndDate, SelectedBinSize);
                        var chartName = CreateFileName();
                        chart.ImageLocation = MetricFileLocation + chartName;
                        chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                        ReturnList.Add(MetricWebPath + chartName);
                    }
                }
            return ReturnList;
        }

        private Chart GetNewSpeedChart(Models.Detector detector)
        {
            var chart = ChartFactory.CreateDefaultChart(this);


            ChartFactory.SetImageProperties(chart);


            //Set the chart title
            SetChartTitles(chart, detector);

            //Create the chart legend
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);

            //Create the chart area
           
            if (YAxisMax > 0)
                chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value;
            else
                chart.ChartAreas[0].AxisY.Maximum = 60;

            if (YAxisMin > 0)
                chart.ChartAreas[0].AxisY.Minimum = YAxisMin;
            else
                chart.ChartAreas[0].AxisY.Minimum = 0;

            chart.ChartAreas[0].AxisY.Title = "Average Speed";


            chart.ChartAreas[0].AxisY.Title = "MPH";
            chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart.ChartAreas[0].AxisY.Interval = 5;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;


            if (ShowAverageSpeed)
            {
                var averageSeries = new Series();
                averageSeries.ChartType = SeriesChartType.Line;
                averageSeries.Color = Color.Red;
                averageSeries.Name = "Average MPH";
                averageSeries.XValueType = ChartValueType.DateTime;
                chart.Series.Add(averageSeries);
            }

            if (Show85Percentile)
            {
                var eightyFifthSeries = new Series();
                eightyFifthSeries.ChartType = SeriesChartType.Line;
                eightyFifthSeries.Color = Color.Blue;
                eightyFifthSeries.Name = "85th Percentile Speed";
                eightyFifthSeries.XValueType = ChartValueType.DateTime;
                chart.Series.Add(eightyFifthSeries);
            }

            if (Show15Percentile)
            {
                var fifteenthSeries = new Series();
                fifteenthSeries.ChartType = SeriesChartType.Line;
                fifteenthSeries.Color = Color.Black;
                fifteenthSeries.Name = "15th Percentile Speed";
                fifteenthSeries.XValueType = ChartValueType.DateTime;
                chart.Series.Add(fifteenthSeries);
            }

            if (ShowPostedSpeed)
            {
                var postedspeedSeries = new Series();
                postedspeedSeries.ChartType = SeriesChartType.Line;
                postedspeedSeries.Color = Color.DarkGreen;
                postedspeedSeries.Name = "Posted Speed";
                postedspeedSeries.BorderWidth = 2;
                chart.Series.Add(postedspeedSeries);
            }

            return chart;
        }

        private void SetChartTitles(Chart chart, Models.Detector detector)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                SignalID, StartDate, EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(detector.Approach, false));
            chart.Titles.Add(ChartTitleFactory.GetTitle("Detection Type: " + detector.DetectionHardware.Name +
                                                        "; Speed Accuracy +/- 2 mph" + "\n" +
                                                        "Detector Distance from Stop Bar: " +
                                                        detector.DistanceFromStopBar + " feet; "
                                                        + "\n" +
                                                        "Includes records over 5mph that occur between 15s after start of green to start of yellow."));
        }

        protected void AddSpeedDataToChart(Chart chart, Models.Detector detector, DateTime startDate,
            DateTime endDate, int binSize)
        {
            var speedDetector = new DetectorSpeed(detector, startDate, endDate, binSize, false);
            foreach (var bucket in speedDetector.AvgSpeedBucketCollection.AvgSpeedBuckets)
            {
                if(ShowAverageSpeed)
                chart.Series["Average MPH"].Points.AddXY(bucket.StartTime, bucket.AvgSpeed);
                if(Show85Percentile)
                chart.Series["85th Percentile Speed"].Points.AddXY(bucket.StartTime, bucket.EightyFifth);
                if(Show15Percentile)
                chart.Series["15th Percentile Speed"].Points.AddXY(bucket.StartTime, bucket.FifteenthPercentile);
                if (ShowPostedSpeed)
                    chart.Series["Posted Speed"].Points.AddXY(bucket.StartTime, detector.Approach.MPH);
            }
            if(SpeedDetectors == null)
                SpeedDetectors = new List<DetectorSpeed>();
            SpeedDetectors.Add(speedDetector);
            if (ShowPlanStatistics)
                SetSpeedPlanStrips(speedDetector.Plans, chart, startDate, detector.MinSpeedFilter ?? 0);
        }

        protected void SetSpeedPlanStrips(List<PlanSpeed> plans, Chart chart, DateTime graphStartDate,
            int minSpeedFilter)
        {
            var backGroundColor = 1;
            foreach (var plan in plans)
            {
                var stripline = new StripLine();
                float currentSize;
                currentSize = 3;
                stripline.Font = new Font(stripline.Font.Name, currentSize, stripline.Font.Style, stripline.Font.Unit);
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
                var plannumberLabel = new CustomLabel();
                plannumberLabel.FromPosition = plan.StartTime.ToOADate();
                plannumberLabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        plannumberLabel.Text = "Free";
                        break;
                    case 255:
                        plannumberLabel.Text = "Flash";
                        break;
                    case 0:
                        plannumberLabel.Text = "Unknown";
                        break;
                    default:
                        plannumberLabel.Text = "Plan " + plan.PlanNumber;

                        break;
                }

                plannumberLabel.LabelMark = LabelMarkStyle.LineSideMark;
                plannumberLabel.ForeColor = Color.Black;
                plannumberLabel.RowIndex = 4;

                var avgLabel = new CustomLabel();

                avgLabel.FromPosition = plan.StartTime.ToOADate();
                avgLabel.ToPosition = plan.EndTime.ToOADate();
                avgLabel.Text = "Avg " + plan.AvgSpeed + "\n" +
                                "StDev " + plan.StdDevAvgSpeed;
                avgLabel.LabelMark = LabelMarkStyle.LineSideMark;
                avgLabel.ForeColor = Color.Red;
                avgLabel.RowIndex = 2;

                var eightyfifthLabel = new CustomLabel();
                eightyfifthLabel.FromPosition = plan.StartTime.ToOADate();
                eightyfifthLabel.ToPosition = plan.EndTime.ToOADate();
                eightyfifthLabel.Text = "85% " + plan.EightyFifth + "\n";
                eightyfifthLabel.LabelMark = LabelMarkStyle.LineSideMark;
                eightyfifthLabel.ForeColor = Color.Blue;
                eightyfifthLabel.RowIndex = 3;

                var fifthteenthLabel = new CustomLabel();
                fifthteenthLabel.FromPosition = plan.StartTime.ToOADate();
                fifthteenthLabel.ToPosition = plan.EndTime.ToOADate();
                fifthteenthLabel.Text = "15% " + plan.Fifteenth + "\n";
                fifthteenthLabel.LabelMark = LabelMarkStyle.LineSideMark;
                fifthteenthLabel.ForeColor = Color.Black;
                fifthteenthLabel.RowIndex = 1;

                if (ShowPlanStatistics)
                {
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(plannumberLabel);
                    if (ShowAverageSpeed)
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgLabel);
                    if (Show85Percentile)
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(eightyfifthLabel);
                    if (Show15Percentile)
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(fifthteenthLabel);
                }
                backGroundColor++;
            }
        }
    }
}
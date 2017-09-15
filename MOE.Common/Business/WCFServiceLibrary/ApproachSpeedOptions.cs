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
using System.ComponentModel.DataAnnotations;
using MOE.Common.Business.Speed;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachSpeedOptions: MetricOptions
    {
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

        public ApproachSpeedOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double yAxisMin,
            int binSize, int dotSize, bool showPlanStatistics, int metricTypeID, bool showPostedSpeed,
            bool showAverageSpeed, bool show85Percentile)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            YAxisMin = yAxisMin;
            SelectedBinSize = binSize;
            ShowPlanStatistics = showPlanStatistics;
            ShowPostedSpeed = showPostedSpeed;
            ShowAverageSpeed = showAverageSpeed;
            Show85Percentile = show85Percentile;
            MetricTypeID = metricTypeID;
        }
        public ApproachSpeedOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 10;
            SetDefaults();
        }
        public void SetDefaults()
        {
            YAxisMax = 60;
            YAxisMin = 0;
            ShowPlanStatistics = true;
            ShowAverageSpeed = true;
            ShowPostedSpeed = true;
            Show85Percentile = true;
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            string location = GetSignalLocation();
            MOE.Common.Models.Repositories.ISignalsRepository sr = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Signal signal = sr.GetSignalBySignalID(SignalID);

            List<Approach> SpeedApproaches = signal.GetApproachesForSignalThatSupportMetric(10);

            //If there are phases in the database add the charts
            if (SpeedApproaches.Count > 0)
            {
                foreach (Approach approach in SpeedApproaches)
                {
                    List<Models.Detector> speedDets = approach.GetDetectorsForMetricType(10);

                    foreach (Models.Detector det in speedDets)
                    {
                        //Display the PDC chart
                        Chart chart = GetNewSpeedChart(location, det);

                        AddSpeedDataToChart(chart, det, StartDate, EndDate, SelectedBinSize);

                        string chartName = CreateFileName();

                        //Save an image of the chart
                        chart.ImageLocation = MetricFileLocation + chartName;
                        chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        ReturnList.Add(MetricWebPath + chartName);
                    }
                }
            }
            return ReturnList;
        }

        private Chart GetNewSpeedChart(string location, Models.Detector detector)
        {
         
            Chart chart = new Chart();

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;

            //Set the chart title
            SetChartTitles(chart, detector);

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);

            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            if (YAxisMax > 0)
            {
                chartArea.AxisY.Maximum = YAxisMax.Value;
            }
            else
            {
                chartArea.AxisY.Maximum = 60;
            }

            if (Y2AxisMax != null && Y2AxisMax > 0)
            {
                chartArea.AxisY.Minimum = Y2AxisMax.Value;
            }
            else
            {
                chartArea.AxisY.Minimum = 20;
            }

            chartArea.AxisY.Title = "Average Speed";


            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;

            chartArea.AxisY.Title = "MPH";
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.Interval = 5;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas.Add(chartArea);

            //Add the point series
            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Line;
            pointSeries.Color = Color.Red;
            pointSeries.Name = "Average MPH";
            pointSeries.XValueType = ChartValueType.DateTime;
            //pointSeries.MarkerSize = Convert.ToInt32(uxDotSizeDropDownList.SelectedValue);

            Series eightyFifthSeries = new Series();
            eightyFifthSeries.ChartType = SeriesChartType.Line;
            eightyFifthSeries.Color = Color.Blue;
            eightyFifthSeries.Name = "85th Percentile Speed";
            eightyFifthSeries.XValueType = ChartValueType.DateTime;
            //pointSeries.MarkerSize = Convert.ToInt32(uxDotSizeDropDownList.SelectedValue);

            Series fifteenthSeries = new Series();
            fifteenthSeries.ChartType = SeriesChartType.Line;
            fifteenthSeries.Color = Color.Orange;
            fifteenthSeries.Name = "15th Percentile Speed";
            fifteenthSeries.XValueType = ChartValueType.DateTime;

            //Add the Posted Speed series
            Series postedspeedSeries = new Series();
            postedspeedSeries.ChartType = SeriesChartType.Line;
            postedspeedSeries.Color = Color.DarkGreen;
            postedspeedSeries.Name = "Posted Speed";
            //greenSeries.XValueType = ChartValueType.DateTime;
            postedspeedSeries.BorderWidth = 2;

            chart.Series.Add(postedspeedSeries);
            chart.Series.Add(eightyFifthSeries);
            chart.Series.Add(fifteenthSeries);
            chart.Series.Add(pointSeries);

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = 0;
            chart.Series.Add(testSeries);


            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(StartDate, 0);
            chart.Series["Posts"].Points.AddXY(EndDate, 0);

            return chart;       
        }

        private void SetChartTitles(Chart chart, MOE.Common.Models.Detector detector)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(this.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                this.SignalID, this.StartDate, this.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(detector.Approach.ProtectedPhaseNumber, detector.Approach.DirectionType.Description));
            chart.Titles.Add(ChartTitleFactory.GetTitle("Detection Type: " + detector.DetectionHardware.Name + "; Speed Accuracy +/- 2 mph" + "\n" + "Detector Distance from Stop Bar: " + detector.DistanceFromStopBar.ToString() + " feet; "
                + "\n" + "Includes records over 5mph that occur between 15s after start of green to start of yellow."));
        }

        protected void AddSpeedDataToChart(Chart chart,Models.Detector detector, DateTime startDate,
            DateTime endDate, int binSize)
        {
            DetectorSpeed detectorSpeed = new DetectorSpeed(detector, startDate, endDate, binSize);
            foreach (Plan plan in detectorSpeed.Plans.PlanList)
            {
                if (plan.AvgSpeedBucketCollection.Items.Count > 0)
                {
                    foreach (AvgSpeedBucket bucket in plan.AvgSpeedBucketCollection.Items)
                    {
                        chart.Series["Average MPH"].Points.AddXY(bucket.StartTime, bucket.AvgSpeed);
                        chart.Series["85th Percentile Speed"].Points.AddXY(bucket.StartTime, bucket.EightyFifth);
                        chart.Series["15th Percentile Speed"].Points.AddXY(bucket.StartTime, bucket.FifteenthPercentile);
                        if (ShowPlanStatistics && ShowPostedSpeed)
                        {
                            chart.Series["Posted Speed"].Points.AddXY(bucket.StartTime, detector.Approach.MPH);
                        }
                    }
                }
            }
            if (ShowPlanStatistics)
            {
                SetSpeedPlanStrips(detectorSpeed.Plans, chart, startDate, detector.MinSpeedFilter ?? 0);
            }
        }

        protected void SetSpeedPlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate, int minSpeedFilter)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
            {
                plan.SetSpeedStatistics(minSpeedFilter);

                StripLine stripline = new StripLine();
                float currentSize;
                currentSize = 3;
                stripline.Font = new Font(stripline.Font.Name, currentSize, stripline.Font.Style, stripline.Font.Unit);
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
                CustomLabel plannumberLabel = new CustomLabel();
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
                        plannumberLabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }

                plannumberLabel.LabelMark = LabelMarkStyle.LineSideMark;
                plannumberLabel.ForeColor = Color.Black;
                plannumberLabel.RowIndex = 3;

                CustomLabel avgLabel = new CustomLabel();

                avgLabel.FromPosition = plan.StartTime.ToOADate();
                avgLabel.ToPosition = plan.EndTime.ToOADate();
                avgLabel.Text = "Ave Sp " + plan.AvgSpeed.ToString() + "\n" +
                    "Std Dev " + plan.StdDevAvgSpeed.ToString();
                avgLabel.LabelMark = LabelMarkStyle.LineSideMark;
                avgLabel.ForeColor = Color.Red;
                avgLabel.RowIndex = 1;

                CustomLabel eightyfifthLabel = new CustomLabel();
                eightyfifthLabel.FromPosition = plan.StartTime.ToOADate();
                eightyfifthLabel.ToPosition = plan.EndTime.ToOADate();
                eightyfifthLabel.Text = "85% Sp " + plan.EightyFifth.ToString() + "\n";
                eightyfifthLabel.LabelMark = LabelMarkStyle.LineSideMark;
                eightyfifthLabel.ForeColor = Color.Blue;
                eightyfifthLabel.RowIndex = 2;

                if (ShowPlanStatistics)
                {
                    chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(plannumberLabel);
                    if (ShowAverageSpeed)
                    {
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgLabel);
                    }
                    if (Show85Percentile)
                    {
                        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(eightyfifthLabel);
                    }
                }
                backGroundColor++;
            }
        }
    }
}

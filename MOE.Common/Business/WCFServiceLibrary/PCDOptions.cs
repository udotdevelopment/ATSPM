using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PCDOptions : MetricOptions
    {
        

        public PCDOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, int dotSize, bool showPlanStatistics, bool showVolumes, int metricTypeID,
            bool showArrivalsOnGreen)
        {

            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            SelectedDotSize = dotSize;
            ShowPlanStatistics = showPlanStatistics;
            ShowVolumes = showVolumes;
            ShowArrivalsOnGreen = showArrivalsOnGreen;
            MetricTypeID = 6;
            StartDate = startDate;
            EndDate = endDate;
        }

        public PCDOptions()
        {
            VolumeBinSizeList = new List<int>();
            VolumeBinSizeList.Add(15);
            VolumeBinSizeList.Add(5);
            DotSizeList = new List<DotSizeItem>();
            DotSizeList.Add(new DotSizeItem(1, "Small"));
            DotSizeList.Add(new DotSizeItem(2, "Large"));
            MetricTypeID = 6;
            ShowArrivalsOnGreen = true;
            SetDefaults();
        }

        [Required]
        [Display(Name = "Volume Bin Size")]
        [DataMember]
        public int SelectedBinSize { get; set; }

        public List<int> VolumeBinSizeList { get; set; }

        [Required]
        [Display(Name = "Dot Size")]
        [DataMember]
        public int SelectedDotSize { get; set; }

        public List<DotSizeItem> DotSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }

        [DataMember]
        [Display(Name = "Show Volumes")]
        public bool ShowVolumes { get; set; }

        [DataMember]
        public bool ShowArrivalsOnGreen { get; set; }

        public Models.Signal Signal { get; set; }

        public void SetDefaults()
        {
            YAxisMax = 150;
            Y2AxisMax = 2000;
            ShowPlanStatistics = true;
            ShowVolumes = true;
            MetricTypeID = 6;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalRepository =
                SignalsRepositoryFactory.Create();
            Signal = signalRepository.GetVersionOfSignalByDate(SignalID, StartDate);
            MetricTypeID = 6;

            var chart = new Chart();
            var location = GetSignalLocation();
            var metricApproaches = Signal.GetApproachesForSignalThatSupportMetric(MetricTypeID);

            if (metricApproaches.Count > 0)
                foreach (var approach in metricApproaches)
                {
                    var signalPhase = new SignalPhase(StartDate, EndDate, approach,
                        ShowVolumes, SelectedBinSize, MetricTypeID, false);
                    chart = GetNewChart(approach);
                    AddDataToChart(chart, signalPhase);
                    var chartName = CreateFileName();
                    chart.ImageLocation = MetricFileLocation + chartName;
                    chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                    ReturnList.Add(MetricWebPath + chartName);
                }
            return ReturnList;
        }


        private Chart GetNewChart(Approach approach)
        {
            var chart = ChartFactory.CreateDefaultChart(this);
            CreateChartLegend(chart);
            if (ShowVolumes)
                CreateVolumeSeries(chart);
            else
                chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            CreateDetectorSeries(chart);
            CreateGreenSeries(chart);
            CreateYellowSeries(chart);
            CreateRedSeries(chart);
            return chart;
        }

        private void SetSeriesLineWidth(Series series)
        {
            if (SelectedDotSize == 2)
                series.BorderWidth = 3;
        }

        private void CreateRedSeries(Chart chart)
        {
            var redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            SetSeriesLineWidth(redSeries);
            chart.Series.Add(redSeries);
        }

        private void CreateYellowSeries(Chart chart)
        {
            var yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            SetSeriesLineWidth(yellowSeries);
            chart.Series.Add(yellowSeries);
        }

        private void CreateVolumeSeries(Chart chart)
        {
            var volumeSeries = new Series();
            volumeSeries.ChartType = SeriesChartType.Line;
            volumeSeries.Color = Color.Black;
            volumeSeries.Name = "Volume Per Hour";
            volumeSeries.XValueType = ChartValueType.DateTime;

            volumeSeries.YAxisType = AxisType.Secondary;
            SetSeriesLineWidth(volumeSeries);
            chart.Series.Add(volumeSeries);
        }

        private void CreateGreenSeries(Chart chart)
        {
            var greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 1;
            SetSeriesLineWidth(greenSeries);
            chart.Series.Add(greenSeries);
        }

        private void CreateDetectorSeries(Chart chart)
        {
            var pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = SelectedDotSize;
            chart.Series.Add(pointSeries);
        }

        private void CreateChartLegend(Chart chart)
        {
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chartLegend.CustomItems.Add(Color.Blue, "AoG - Arrival On Green");
            chartLegend.CustomItems.Add(Color.Blue, "GT - Green Time");
            chartLegend.CustomItems.Add(Color.Maroon, "PR - Platoon Ratio");
            chart.Legends.Add(chartLegend);
        }

        private void SetChartTitle(Chart chart, Approach approach, Dictionary<string, string> statistics)
        {
            var detectorsForMetric = approach.GetDetectorsForMetricType(MetricTypeID);
            var message = "\n Advanced detector located " + detectorsForMetric.FirstOrDefault().DistanceFromStopBar +
                          " ft. upstream of stop bar";
            chart.Titles.Add(ChartTitleFactory.GetChartName(MetricTypeID));
            chart.Titles.Add(
                ChartTitleFactory.GetSignalLocationAndDateRangeAndMessage(approach.SignalID, StartDate, EndDate,
                    message));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(approach.ProtectedPhaseNumber,
                approach.DirectionType.Description));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        private void AddDataToChart(Chart chart, SignalPhase signalPhase)
        {
            double totalDetectorHits = 0;
            double totalOnGreenArrivals = 0;
            foreach (var cycle in signalPhase.Cycles)
            {
                totalOnGreenArrivals += AddCycleToChart(chart, cycle);
                totalDetectorHits += cycle.DetectorEvents.Count;
            }
            if (ShowVolumes)
                AddVolumeToChart(chart, signalPhase.Volume);
            if (ShowArrivalsOnGreen)
                AddArrivalOnGreen(chart, totalOnGreenArrivals, totalDetectorHits, signalPhase.Approach);
            if (ShowPlanStatistics)
                SetPlanStrips(signalPhase.Plans, chart);
        }

        private void AddArrivalOnGreen(Chart chart, double totalOnGreenArrivals, double totalDetectorHits,
            Approach approach)
        {
            double percentArrivalOnGreen = 0;
            if (totalDetectorHits > 0)
                percentArrivalOnGreen = totalOnGreenArrivals / totalDetectorHits * 100;
            var statistics = new Dictionary<string, string>();
            statistics.Add("AoG", Math.Round(percentArrivalOnGreen) + "%");
            SetChartTitle(chart, approach, statistics);
        }

        private void AddVolumeToChart(Chart chart, VolumeCollection volumeCollection)
        {
            foreach (var v in volumeCollection.Items)
                chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
        }

        private double AddCycleToChart(Chart chart, CyclePcd cycle)
        {
            double totalOnGreenArrivals = 0;
            chart.Series["Change to Green"].Points.AddXY(cycle.GreenEvent, cycle.GreenLineY);
            chart.Series["Change to Yellow"].Points.AddXY(cycle.YellowEvent, cycle.YellowLineY);
            chart.Series["Change to Red"].Points.AddXY(cycle.EndTime, cycle.RedLineY);
            foreach (var detectorPoint in cycle.DetectorEvents)
            {
                chart.Series["Detector Activation"].Points.AddXY(
                    //pcd.StartTime, 
                    detectorPoint.TimeStamp,
                    detectorPoint.YPoint);
                if (detectorPoint.YPoint > cycle.GreenLineY && detectorPoint.YPoint < cycle.RedLineY)
                    totalOnGreenArrivals++;
            }
            return totalOnGreenArrivals;
        }


        /// <summary>
        ///     Adds plan strips to the chart
        /// </summary>
        /// <param name="plans"></param>
        /// <param name="chart"></param>
        /// <param name="StartDate"></param>
        protected void SetPlanStrips(List<PlanPcd> plans, Chart chart)
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
                stripline.IntervalOffset = (plan.StartTime - StartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                var aogLabel = new CustomLabel();
                aogLabel.FromPosition = plan.StartTime.ToOADate();
                aogLabel.ToPosition = plan.EndTime.ToOADate();
                aogLabel.Text = plan.PercentArrivalOnGreen + "% AoG\n" +
                                plan.PercentGreenTime + "% GT";

                aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                aogLabel.ForeColor = Color.Blue;
                aogLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                var statisticlabel = new CustomLabel();
                statisticlabel.FromPosition = plan.StartTime.ToOADate();
                statisticlabel.ToPosition = plan.EndTime.ToOADate();
                statisticlabel.Text =
                    plan.PlatoonRatio + " PR";
                statisticlabel.ForeColor = Color.Maroon;
                statisticlabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);


                //Change the background color counter for alternating color
                backGroundColor++;
            }
        }
    }
}
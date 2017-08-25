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

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    
    public class PCDOptions: MetricOptions
    {
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
        public MOE.Common.Models.Signal Signal { get; set; }

        private int MetricTypeID = 6;
        
        public PCDOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, int dotSize, bool showPlanStatistics, bool showVolumes, int metricTypeID, bool showArrivalsOnGreen)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            SelectedDotSize = dotSize;
            ShowPlanStatistics = showPlanStatistics;
            ShowVolumes = showVolumes;
            MetricTypeID = metricTypeID;
            ShowArrivalsOnGreen = showArrivalsOnGreen;            
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

        public void SetDefaults()
        {
            YAxisMax = 150;
            Y2AxisMax = 2000;
            ShowPlanStatistics = true;
            ShowVolumes = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Signal = signalRepository.GetSignalBySignalID(SignalID);
            this.MetricTypeID = 6;
            
            Chart chart = new Chart();            
            string location = GetSignalLocation();

            //SignalPhaseCollection signalphasecollection = new SignalPhaseCollection(
            //    StartDate,
            //    EndDate, 
            //    SignalID,
            //    ShowVolumes,
            //    SelectedBinSize, 
            //    MetricTypeID);

                //If there are phases in the database add the charts
                //if (signalphasecollection.SignalPhaseList.Count > 0)
                //{
                //    foreach (MOE.Common.Business.SignalPhase signalPhase in signalphasecollection.SignalPhaseList)
                //    {
                //        if (signalPhase.Plans.PlanList.Count > 0)
                //        {
                //            chart = GetNewChart(signalPhase.Approach);
                //            AddDataToChart(chart, signalPhase);
                //            string chartName = CreateFileName();
                //            chart.ImageLocation = MetricFileLocation + chartName;
                //            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                //            ReturnList.Add(MetricWebPath + chartName);
                //    }

                //}
            List<Approach> metricApproaches = Signal.GetApproachesForSignalThatSupportMetric(this.MetricTypeID);

            if (metricApproaches.Count > 0)
                {
                    foreach (Approach approach in metricApproaches)
                    {
                        
                        MOE.Common.Business.SignalPhase signalPhase = new SignalPhase(StartDate, EndDate, approach,
                            ShowVolumes, SelectedBinSize, MetricTypeID);

                        chart = GetNewChart(approach);
                        AddDataToChart(chart, signalPhase);
                        string chartName = CreateFileName();
                        chart.ImageLocation = MetricFileLocation + chartName;
                        chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                        ReturnList.Add(MetricWebPath + chartName);


                    }
                }
            
           

            return ReturnList;
        }


        private Chart GetNewChart(MOE.Common.Models.Approach approach)
        {
            Chart chart = MOE.Common.Business.ChartFactory.CreateDefaultChart(this);
            CreateChartLegend(chart);
            if (ShowVolumes)
            {
                CreateVolumeSeries(chart);
            }
            else
            {
                chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            }
            CreateDetectorSeries(chart);
            CreateGreenSeries(chart);
            CreateYellowSeries(chart);
            CreateRedSeries(chart);
            return chart;
        }

        private void SetSeriesLineWidth(Series series)
        {
            if(SelectedDotSize == 2 )
            {
                series.BorderWidth = 3;
            }
        }

        private void CreateRedSeries(Chart chart)
        {
            Series redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            SetSeriesLineWidth(redSeries);
            chart.Series.Add(redSeries);
            
        }

        private void CreateYellowSeries(Chart chart)
        {
            Series yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            SetSeriesLineWidth(yellowSeries);
            chart.Series.Add(yellowSeries);
        }

        private void CreateVolumeSeries(Chart chart)
        {
            Series volumeSeries = new Series();
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
            Series greenSeries = new Series();
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
            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = SelectedDotSize;
            chart.Series.Add(pointSeries);
        }

        private void CreateChartLegend(Chart chart)
        {
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chartLegend.CustomItems.Add(Color.Blue, "AoG - Arrival On Green");
            chartLegend.CustomItems.Add(Color.Blue, "GT - Green Time");
            chartLegend.CustomItems.Add(Color.Maroon, "PR - Platoon Ratio");
            chart.Legends.Add(chartLegend);
        }

        private void SetChartTitle(Chart chart, Approach approach, Dictionary<string, string> statistics)
        {
            var detectorsForMetric = approach.GetDetectorsForMetricType(this.MetricTypeID);
            string message = "\n Advanced detector located " + detectorsForMetric.FirstOrDefault().DistanceFromStopBar.ToString() + " ft. upstream of stop bar";
            chart.Titles.Add(ChartTitleFactory.GetChartName(this.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRangeAndMessage(approach.SignalID, this.StartDate, this.EndDate, message));
            chart.Titles.Add(ChartTitleFactory.GetPhaseAndPhaseDescriptions(approach.ProtectedPhaseNumber, approach.DirectionType.Description));
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
        }

        private void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase)
        {
            double totalDetectorHits = 0;
            double totalOnGreenArrivals = 0;
            foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
            {
                if (plan.CycleCollection.Count > 0)
                {
                    foreach (MOE.Common.Business.Cycle pcd in plan.CycleCollection)
                    {
                        totalOnGreenArrivals += AddCycleToChart(chart, pcd);
                        totalDetectorHits += pcd.DetectorCollection.Count;
                    }
                }
            }
            if (ShowVolumes)
            {
                AddVolumeToChart(chart, signalPhase.Volume);
            }
            if (ShowArrivalsOnGreen)
            {
                AddArrivalOnGreen(chart, totalOnGreenArrivals, totalDetectorHits, signalPhase.Approach);
            }
            if (ShowPlanStatistics)
            {
                SetPlanStrips(signalPhase.Plans.PlanList, chart);
            }
        }

        private void AddArrivalOnGreen(Chart chart, double totalOnGreenArrivals, double totalDetectorHits, MOE.Common.Models.Approach approach)
        {
            double percentArrivalOnGreen = 0;
            if (totalDetectorHits > 0)
            {
                percentArrivalOnGreen = (totalOnGreenArrivals / totalDetectorHits) * 100;
            }
            Dictionary<string, string> statistics = new Dictionary<string,string>();
            statistics.Add("AoG", Math.Round(percentArrivalOnGreen).ToString()+"%");
            SetChartTitle(chart, approach, statistics);
        }

        private void AddVolumeToChart(Chart chart, VolumeCollection volumeCollection)
        {
            foreach (MOE.Common.Business.Volume v in volumeCollection.Items)
            {
                chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
            }
        }

        private double AddCycleToChart(Chart chart, Cycle pcd)
        {

            double totalOnGreenArrivals = 0;
            chart.Series["Change to Green"].Points.AddXY(pcd.GreenEvent, pcd.GreenLineY);
            chart.Series["Change to Yellow"].Points.AddXY(pcd.YellowEvent, pcd.YellowLineY);
            chart.Series["Change to Red"].Points.AddXY(pcd.EndTime, pcd.RedLineY);
            foreach (MOE.Common.Business.DetectorDataPoint detectorPoint in pcd.DetectorCollection)
            {
                chart.Series["Detector Activation"].Points.AddXY(
                    //pcd.StartTime, 
                    detectorPoint.TimeStamp,
                    detectorPoint.YPoint);
                if (detectorPoint.YPoint > pcd.GreenLineY && detectorPoint.YPoint < pcd.RedLineY)
                {
                    totalOnGreenArrivals++;
                }
            }
            return totalOnGreenArrivals;
        }


        /// <summary>
        /// Adds plan strips to the chart
        /// </summary>
        /// <param name="PlanCollection"></param>
        /// <param name="Chart"></param>
        /// <param name="StartDate"></param>
        protected void SetPlanStrips(List<MOE.Common.Business.Plan> PlanCollection, Chart Chart)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in PlanCollection)
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
                stripline.IntervalOffset = (plan.StartTime - StartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;               
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                CustomLabel aogLabel = new CustomLabel();
                aogLabel.FromPosition = plan.StartTime.ToOADate();
                aogLabel.ToPosition = plan.EndTime.ToOADate();
                aogLabel.Text = plan.PercentArrivalOnGreen.ToString() + "% AoG\n" +
                    plan.PercentGreen.ToString() + "% GT";

                aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                aogLabel.ForeColor = Color.Blue;
                aogLabel.RowIndex = 2;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                CustomLabel statisticlabel = new CustomLabel();
                statisticlabel.FromPosition = plan.StartTime.ToOADate();
                statisticlabel.ToPosition = plan.EndTime.ToOADate();
                statisticlabel.Text =
                    plan.PlatoonRatio.ToString() + " PR";
                statisticlabel.ForeColor = Color.Maroon;
                statisticlabel.RowIndex = 1;
                Chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);

            

                //Change the background color counter for alternating color
                backGroundColor++;

            }
        }
    }
    
}

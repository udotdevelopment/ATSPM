using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.ApproachVolume;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachVolumeOptions : MetricOptions
    {
        public List<MetricInfo> MetricInfoList;


        public ApproachVolumeOptions(string signalID, DateTime startDate, DateTime endDate, double? yAxisMax,
            int binSize, bool showDirectionalSplits,
            bool showTotalVolume, bool ShowNbEbVolume, bool ShowSbWbVolume, bool showTMCDetection,
            bool showAdvanceDetection)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;

            SelectedBinSize = binSize;
            ShowTotalVolume = showTotalVolume;
            ShowDirectionalSplits = showDirectionalSplits;
            ShowNbEbVolume = ShowNbEbVolume;
            ShowSbWbVolume = ShowSbWbVolume;
            ShowTMCDetection = showTMCDetection;
            ShowAdvanceDetection = showAdvanceDetection;
        }

        public ApproachVolumeOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 7;
            SetDefaults();
        }

        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }

        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Directional Splits")]
        public bool ShowDirectionalSplits { get; set; }

        [DataMember]
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolume { get; set; }

        [DataMember]
        [Display(Name = "Show NB/EB Volume")]
        public bool ShowNbEbVolume { get; set; }

        [DataMember]
        [Display(Name = "Show SB/WB Volume")]
        public bool ShowSbWbVolume { get; set; }

        [DataMember]
        [Display(Name = "Show TMC Detection")]
        public bool ShowTMCDetection { get; set; }

        [DataMember]
        [Display(Name = "Show Advance Detection")]
        public bool ShowAdvanceDetection { get; set; }

        public void SetDefaults()
        {
            YAxisMin = 0;
            YAxisMax = null;
            ShowDirectionalSplits = false;
            ShowTotalVolume = false;
            ShowNbEbVolume = true;
            ShowSbWbVolume = true;
            ShowTMCDetection = true;
            ShowAdvanceDetection = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var returnList = new List<string>();
            MetricInfoList = new List<MetricInfo>();
            var signalsRepository =
                SignalsRepositoryFactory.Create();
            //var signal = signalsRepository.GetSignalBySignalID(this.SignalID);    
            var signal = signalsRepository.GetVersionOfSignalByDate(SignalID, StartDate);
            var NSAdvanceVolumeApproaches = new List<Approach>();
            var NSTMCVolumeApproaches = new List<Approach>();
            var EWAdvanceVolumeApproaches = new List<Approach>();
            var EWTMCVolumeApproaches = new List<Approach>();

            //Sort the approaches by metric type and direction

            foreach (var a in signal.Approaches)
            {
                if (a.DirectionType.Description == "Northbound" && a.GetDetectorsForMetricType(6).Count > 0)
                {
                    var av = new Approach(a);
                    NSAdvanceVolumeApproaches.Add(av);
                }

                if (a.DirectionType.Description == "Northbound" && a.GetDetectorsForMetricType(5).Count > 0)
                {
                    var av = new Approach(a);
                    NSTMCVolumeApproaches.Add(av);
                }
                if (a.DirectionType.Description == "Southbound" && a.GetDetectorsForMetricType(6).Count > 0)
                {
                    var av = new Approach(a);
                    NSAdvanceVolumeApproaches.Add(av);
                }

                if (a.DirectionType.Description == "Southbound" && a.GetDetectorsForMetricType(5).Count > 0)
                {
                    var av = new Approach(a);
                    NSTMCVolumeApproaches.Add(av);
                }
                if (a.DirectionType.Description == "Eastbound" && a.GetDetectorsForMetricType(6).Count > 0)
                {
                    var av = new Approach(a);
                    EWAdvanceVolumeApproaches.Add(av);
                }

                if (a.DirectionType.Description == "Eastbound" && a.GetDetectorsForMetricType(5).Count > 0)
                {
                    var av = new Approach(a);
                    EWTMCVolumeApproaches.Add(av);
                }
                if (a.DirectionType.Description == "Westbound" && a.GetDetectorsForMetricType(6).Count > 0)
                {
                    var av = new Approach(a);
                    EWAdvanceVolumeApproaches.Add(av);
                }

                if (a.DirectionType.Description == "Westbound" && a.GetDetectorsForMetricType(5).Count > 0)
                {
                    var av = new Approach(a);
                    EWTMCVolumeApproaches.Add(av);
                }
            }


            var location = GetSignalLocation();

            //create the charts for each metric type and direction

            if (ShowAdvanceDetection && NSAdvanceVolumeApproaches.Count > 0)
            {
                var AVC =
                    new ApproachVolumeChart(
                        StartDate, EndDate, SignalID, location, "Northbound", "Southbound", this,
                        NSAdvanceVolumeApproaches, true);

                var chartName = CreateFileName();


                //Save an image of the chart
                AVC.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

                AVC.info.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(AVC.info);
            }

            if (ShowTMCDetection && NSTMCVolumeApproaches.Count > 0)
            {
                var AVC =
                    new ApproachVolumeChart(StartDate, EndDate, SignalID,
                        location, "Northbound", "Southbound", this, NSTMCVolumeApproaches, false);

                var chartName = CreateFileName();


                //Save an image of the chart
                AVC.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

                AVC.info.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(AVC.info);
            }

            if (ShowAdvanceDetection && EWAdvanceVolumeApproaches.Count > 0)
            {
                var AVC =
                    new ApproachVolumeChart(StartDate, EndDate,
                        SignalID, location, "Eastbound", "Westbound", this, EWAdvanceVolumeApproaches, true);

                var chartName = CreateFileName();


                //Save an image of the chart
                AVC.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

                AVC.info.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(AVC.info);
            }

            if (ShowTMCDetection && EWTMCVolumeApproaches.Count > 0)
            {
                var AVC =
                    new ApproachVolumeChart(StartDate, EndDate,
                        SignalID, location, "Eastbound", "Westbound", this, EWTMCVolumeApproaches, false);

                var chartName = CreateFileName();


                //Save an image of the chart
                AVC.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);


                AVC.info.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(AVC.info);
            }


            return returnList;
        }
    }
}
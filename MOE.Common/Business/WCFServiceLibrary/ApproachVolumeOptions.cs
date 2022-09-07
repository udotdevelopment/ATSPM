using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.ApproachVolume;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Approach = MOE.Common.Business.ApproachVolume.Approach;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachVolumeOptions : MetricOptions
    {
        public List<MetricInfo> MetricInfoList;


        public ApproachVolumeOptions(string signalId, DateTime startDate, DateTime endDate, double? yAxisMax,
            int binSize, bool showDirectionalSplits, bool showTotalVolume, bool showNbEbVolume, bool showSbWbVolume, bool showTmcDetection,
            bool showAdvanceDetection)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;
            SelectedBinSize = binSize;
            ShowTotalVolume = showTotalVolume;
            ShowDirectionalSplits = showDirectionalSplits;
            ShowNbEbVolume = showNbEbVolume;
            ShowSbWbVolume = showSbWbVolume;
            ShowTMCDetection = showTmcDetection;
            ShowAdvanceDetection = showAdvanceDetection;
            MetricTypeID = 7;
        }

        public ApproachVolumeOptions()
        {
            BinSizeList = new List<int>() { 5, 15 };
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

        public override List<string> CreateMetric()
        {
            base.CreateMetric();

            var returnList = new List<string>();
            MetricInfoList = new List<MetricInfo>();

            var signalsRepository =SignalsRepositoryFactory.Create();
            var directionRepository = DirectionTypeRepositoryFactory.Create();
            var allDirections = directionRepository.GetAllDirections();
            var signal = signalsRepository.GetVersionOfSignalByDate(SignalID, StartDate);
            var directions = signal.GetAvailableDirections();
            if (directions.Any(d => d.Description == "Northbound") || directions.Any(d => d.Description == "Southbound"))
            {
                DirectionType northboundDirection = allDirections.FirstOrDefault(d => d.Description == "Northbound");
                DirectionType southboundDirection = allDirections.FirstOrDefault(d => d.Description == "Southbound");
                List<Models.Approach> northboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == northboundDirection.DirectionTypeID && a.ProtectedPhaseNumber !=0).ToList();
                List<Models.Approach> southboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == southboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                if (northboundApproaches.Count > 0 || southboundApproaches.Count >0)
                {
                    CreateAndSaveCharts(northboundDirection, southboundDirection, northboundApproaches, southboundApproaches);
                }
            }
            if (directions.Any(d => d.Description == "Westbound") || directions.Any(d => d.Description == "Eastbound"))
            {
                DirectionType eastboundDirection = allDirections.FirstOrDefault(d => d.Description == "Eastbound");
                DirectionType westboundDirection = allDirections.FirstOrDefault(d => d.Description == "Westbound");
                var eastboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == eastboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                var westboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == westboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                if (eastboundApproaches.Count > 0 || westboundApproaches.Count > 0)
                {
                    CreateAndSaveCharts(eastboundDirection, westboundDirection, eastboundApproaches, westboundApproaches);
                }
            }
            if (directions.Any(d => d.Description == "Northeast") || directions.Any(d => d.Description == "Southwest"))
            {
                DirectionType eastboundDirection = allDirections.FirstOrDefault(d => d.Description == "Northeast");
                DirectionType westboundDirection = allDirections.FirstOrDefault(d => d.Description == "Southwest");
                var eastboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == eastboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                var westboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == westboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                if (eastboundApproaches.Count > 0 || westboundApproaches.Count > 0)
                {
                    CreateAndSaveCharts(eastboundDirection, westboundDirection, eastboundApproaches, westboundApproaches);
                }
            }
            if (directions.Any(d => d.Description == "Northwest") || directions.Any(d => d.Description == "Southeast"))
            {
                DirectionType eastboundDirection = allDirections.FirstOrDefault(d => d.Description == "Southeast");
                DirectionType westboundDirection = allDirections.FirstOrDefault(d => d.Description == "Northwest");
                var eastboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == eastboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                var westboundApproaches = signal.Approaches
                    .Where(a => a.DirectionTypeID == westboundDirection.DirectionTypeID && a.ProtectedPhaseNumber != 0).ToList();
                if (eastboundApproaches.Count > 0 || westboundApproaches.Count > 0)
                {
                    CreateAndSaveCharts(eastboundDirection, westboundDirection, eastboundApproaches, westboundApproaches);
                }
            }
            return returnList;
        }

        private void CreateAndSaveCharts(DirectionType primaryDirection, DirectionType opposingDirection, List<Models.Approach> primaryApproaches, List<Models.Approach> opposingApproaches)
        {
            ApproachVolume.ApproachVolume advanceCountApproachVolume =
                new ApproachVolume.ApproachVolume(primaryApproaches, opposingApproaches, this, primaryDirection, opposingDirection, 2);
            ApproachVolume.ApproachVolume laneByLaneCountApproachVolume =
                new ApproachVolume.ApproachVolume(primaryApproaches, opposingApproaches, this, primaryDirection, opposingDirection, 4);
            if (advanceCountApproachVolume.PrimaryDirectionVolume != null &&
                advanceCountApproachVolume.OpposingDirectionVolume != null && advanceCountApproachVolume.MetricInfo.CombinedVolume >0 && ShowAdvanceDetection)
            {
                string chartName = CreateFileName();
                ApproachVolumeChart chart = new ApproachVolumeChart(this, advanceCountApproachVolume, primaryDirection, opposingDirection);
                chart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                advanceCountApproachVolume.MetricInfo.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(advanceCountApproachVolume.MetricInfo);
            }
            if (laneByLaneCountApproachVolume.PrimaryDirectionVolume != null &&
                laneByLaneCountApproachVolume.OpposingDirectionVolume != null && laneByLaneCountApproachVolume.MetricInfo.CombinedVolume > 0 && ShowTMCDetection)
            {
                string chartName = CreateFileName();
                ApproachVolumeChart chart = new ApproachVolumeChart(this, laneByLaneCountApproachVolume, primaryDirection, opposingDirection);
                chart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                laneByLaneCountApproachVolume.MetricInfo.ImageLocation = MetricWebPath + chartName;
                MetricInfoList.Add(laneByLaneCountApproachVolume.MetricInfo);
            }
        }
    }
}
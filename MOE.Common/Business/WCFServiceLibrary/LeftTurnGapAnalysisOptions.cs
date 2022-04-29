using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.Common.Business;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class LeftTurnGapAnalysisOptions : MetricOptions
    {
        public const int EVENT_GREEN = 1;
        public const int EVENT_RED = 10;
        public const int EVENT_DET = 81;

        public LeftTurnGapAnalysisOptions(string signalId, DateTime startDate, DateTime endDate, double gap1Min,
            double gap1Max, double gap2Min, double gap2Max, double gap3Min, double gap3Max, double gap4Min,
            double trendLineGapThreshold)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            Gap1Min = gap1Min;
            Gap1Max = gap1Max;
            Gap2Min = gap2Min;
            Gap2Max = gap2Max;
            Gap3Min = gap3Min;
            Gap3Max = gap3Max;
            Gap4Min = gap4Min;
            TrendLineGapThreshold = trendLineGapThreshold;
        }

        public LeftTurnGapAnalysisOptions(string signalId, DateTime startDate, DateTime endDate, double gap1Min,
            double gap1Max, double gap2Min, double gap2Max, double gap3Min, double gap3Max, double gap4Min, double? gap4Max, double? gap5Min, double? gap5Max, 
            double? gap6Min, double? gap6Max, double? gap7Min, double? gap7Max, double? gap8Min, double? gap8Max, double? gap9Min, double? gap9Max, 
            double? gap10Min, double? gap10Max, double? gap11Min, double? gap11Max, double? sumGapDuration1, double? sumGapDuration2, double? sumGapDuration3,
            double trendLineGapThreshold)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            Gap1Min = gap1Min;
            Gap1Max = gap1Max;
            Gap2Min = gap2Min;
            Gap2Max = gap2Max;
            Gap3Min = gap3Min;
            Gap3Max = gap3Max;
            Gap4Min = gap4Min;
            Gap4Max = gap4Max;
            Gap5Min = gap5Min;
            Gap5Max = gap5Max;
            Gap6Min = gap6Min;
            Gap6Max = gap6Max;
            Gap7Min = gap7Min;
            Gap7Max = gap7Max;
            Gap8Min = gap8Min;
            Gap8Max = gap8Max;
            Gap9Min = gap9Min;
            Gap9Max = gap9Max;
            Gap10Min = gap10Min;
            Gap10Max = gap10Max;
            Gap11Min = gap11Min;
            Gap11Max = gap11Max;
            SumDurationGap1 = sumGapDuration1;
            SumDurationGap2 = sumGapDuration2;
            SumDurationGap3 = sumGapDuration3;
            TrendLineGapThreshold = trendLineGapThreshold;
            BinSize = 15;
        }

        public LeftTurnGapAnalysisOptions()
        {
            YAxisMax = 60;
            SetDefaults();
        }

        [DataMember]
        [Display(Name = "Gap 1 Minimum (seconds) ")]
        public double Gap1Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 1 Maximum (seconds)")]
        public double Gap1Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 2 Minimum (seconds) ")]
        public double Gap2Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 2 Maximum (seconds)")]
        public double Gap2Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 3 Minimum (seconds) ")]
        public double Gap3Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 3 Maximum (seconds)")]
        public double Gap3Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 4 Minimum (seconds) ")]
        public double Gap4Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 4 Maximum (seconds) ")]
        public double? Gap4Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 5 Minimum (seconds) ")]
        public double? Gap5Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 5 Maximum (seconds) ")]
        public double? Gap5Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 6 Minimum (seconds) ")]
        public double? Gap6Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 6 Maximum (seconds) ")]
        public double? Gap6Max { get; set; }
        [DataMember]
        [Display(Name = "Gap 7 Minimum (seconds) ")]
        public double? Gap7Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 7 Maximum (seconds) ")]
        public double? Gap7Max { get; set; }
        [DataMember]
        [Display(Name = "Gap 8 Minimum (seconds) ")]
        public double? Gap8Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 8 Maximum (seconds) ")]
        public double? Gap8Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 9 Minimum (seconds) ")]
        public double? Gap9Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 9 Maximum (seconds) ")]
        public double? Gap9Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 10 Minimum (seconds) ")]
        public double? Gap10Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 10 Maximum (seconds) ")]
        public double? Gap10Max { get; set; }

        [DataMember]
        [Display(Name = "Gap 11 Minimum (seconds) ")]
        public double? Gap11Min { get; set; }

        [DataMember]
        [Display(Name = "Gap 11 Maximum (seconds) ")]
        public double? Gap11Max { get; set; }

        [DataMember]
        [Display(Name = "Sum Duration Gap 1 (seconds) ")]
        public double? SumDurationGap1 { get; set; }

        [DataMember]
        [Display(Name = "Sum Duration Gap 2 (seconds) ")]
        public double? SumDurationGap2 { get; set; }

        [DataMember]
        [Display(Name = "Sum Duration Gap 3 (seconds) ")]
        public double? SumDurationGap3 { get; set; }

        [DataMember]
        public double TrendLineGapThreshold { get; set; }

        [DataMember]
        public double BinSize { get; set; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalsRepository = SignalsRepositoryFactory.Create();
            var signal = signalsRepository.GetVersionOfSignalByDate(SignalID, StartDate);

            CreateLegend();

            var eventLogs = new ControllerEventLogs(SignalID, StartDate, EndDate,
                new List<int> {EVENT_DET, EVENT_GREEN, EVENT_RED});
            
            //Get phase + check for opposing phase before creating chart
            var ebPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 6);
            if (ebPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 2))
            {
                var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(ebPhase, eventLogs, this);
                CreateChart(leftTurnGapData);
            }

            var nbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 8);
            if (nbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 4))
            {
                var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(nbPhase, eventLogs, this);
                CreateChart(leftTurnGapData);
            }

            var wbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 2);
            if (wbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 6))
            {
                var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(wbPhase, eventLogs, this);
                CreateChart(leftTurnGapData);
            }

            var sbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 4);
            if (sbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 8))
            {
                var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(sbPhase, eventLogs, this);
                CreateChart(leftTurnGapData);
            }

            return ReturnList;
        }

        private void CreateChart(LeftTurnGapAnalysis.LeftTurnGapAnalysis gapData)
        {
            //var phaseEvents = new List<Controller_Event_Log>();

            //phaseEvents.AddRange(eventLogs.Events.Where(x =>
            //    x.EventParam == approach.ProtectedPhaseNumber &&
            //    (x.EventCode == EVENT_GREEN || x.EventCode == EVENT_RED)));

            //var detectorsToUse = new List<Models.Detector>();
            //var detectionTypeStr = "Lane-By-Lane Count";

            ////Use only lane-by-lane count detectors if they exists, otherwise check for stop bar
            //detectorsToUse = approach.GetAllDetectorsOfDetectionType(4);

            //if (!detectorsToUse.Any())
            //{
            //    detectorsToUse = approach.GetAllDetectorsOfDetectionType(6);
            //    detectionTypeStr = "Stop Bar Presence";

            //    //If no detectors of either type for this approach, skip it
            //    if (!detectorsToUse.Any())
            //        return;
            //}

            //foreach (var detector in detectorsToUse)
            //{
            //    // Check for thru, right, thru-right, and thru-left
            //    if (!IsThruDetector(detector)) continue;

            //    phaseEvents.AddRange(eventLogs.Events.Where(x =>
            //        x.EventCode == EVENT_DET && x.EventParam == detector.DetChannel));
            //}

            if (gapData.Gaps1.Any() || gapData.Gaps2.Any() || gapData.Gaps3.Any() || gapData.Gaps4.Any())
            {
                var leftTurnChart = new LeftTurnGapAnalysisChart(this, gapData,
                    GetOpposingPhase(gapData.Approach.ProtectedPhaseNumber));
                var chart = leftTurnChart.Chart;
                var chartName = CreateFileName();
                chart.SaveImage(MetricFileLocation + chartName);
                ReturnList.Add(MetricWebPath + chartName);
            }
        }

        private bool IsThruDetector(Models.Detector detector)
        {
            return detector.MovementTypeID == 1 || detector.MovementTypeID == 2 ||
                   detector.MovementTypeID == 4 || detector.MovementTypeID == 5;
        }

        private int GetOpposingPhase(int phase)
        {
            switch (phase)
            {
                case 2:
                    return 6;
                case 4:
                    return 8;
                case 6:
                    return 2;
                case 8:
                    return 4;
                default:
                    return 0;
            }
        }

        public Series CreateChartSeries(Color color, SeriesChartType seriesChartType, ChartValueType xValueType,
            AxisType yAxisType, string name)
        {
            return new Series
            {
                ChartType = seriesChartType,
                XValueType = xValueType,
                YAxisType = yAxisType,
                Color = color,
                IsVisibleInLegend = true,
                Name = name
            };
        }

        private void CreateLegend()
        {
            var dummyChart = new Chart();
            var chartArea = new ChartArea();
            ChartFactory.SetImageProperties(dummyChart);
            dummyChart.BorderlineDashStyle = ChartDashStyle.Dot;

            var shortGapSeries = CreateChartSeries(Color.Red, SeriesChartType.StackedColumn, ChartValueType.DateTime,
                AxisType.Primary, $"{Gap1Min} - {Gap1Max} seconds");
            var mediumGapSeries = CreateChartSeries(Color.LawnGreen, SeriesChartType.StackedColumn,
                ChartValueType.DateTime, AxisType.Primary, $"{Gap2Min} - {Gap2Max} seconds");
            var largeGapSeries = CreateChartSeries(Color.Green, SeriesChartType.StackedColumn, ChartValueType.DateTime,
                AxisType.Primary, $"{Gap3Min} - {Gap3Max} seconds");
            var hugeGapSeries = CreateChartSeries(Color.LightSeaGreen, SeriesChartType.StackedColumn,
                ChartValueType.DateTime, AxisType.Primary, $"{Gap4Min}+ seconds");
            var percentTurnableSeries = CreateChartSeries(Color.Blue, SeriesChartType.Line, ChartValueType.DateTime,
                AxisType.Secondary, $"% of Green Time where Gaps ≥ {TrendLineGapThreshold} seconds");

            //Reverse order for the legend
            dummyChart.Series.Add(percentTurnableSeries);
            dummyChart.Series.Add(hugeGapSeries);
            dummyChart.Series.Add(largeGapSeries);
            dummyChart.Series.Add(mediumGapSeries);
            dummyChart.Series.Add(shortGapSeries);

            dummyChart.ChartAreas.Add(chartArea);

            var dummyChartLegend = new Legend
            {
                Name = "DummyLegend",
                IsDockedInsideChartArea = true,
                Title = "Chart Legend",
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                TextWrapThreshold = 0
            };

            dummyChart.Legends.Add(dummyChartLegend);

            //Randomize the legend name to prevent caching if options change 
            var chartName = "LTGALegend-";
            var r = new Random();
            chartName += r.Next().ToString();
            chartName += ".jpeg";

            dummyChart.Height = 100;
            dummyChart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

            ReturnList.Add(MetricWebPath + chartName);
        }
    }
}

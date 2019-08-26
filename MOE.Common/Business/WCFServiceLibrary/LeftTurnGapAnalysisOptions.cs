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

        public LeftTurnGapAnalysisOptions()
        {
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
        public double TrendLineGapThreshold { get; set; }

        [DataMember]
        public double BinSize { get; set; }

        public void SetDefaults()
        {
            YAxisMax = 60;
            Gap1Min = 1;
            Gap1Max = 3.3;
            Gap2Min = 3.3;
            Gap2Max = 3.7;
            Gap3Min = 3.7;
            Gap3Max = 7.4;
            Gap4Min = 7.4;
            TrendLineGapThreshold = 7.4;
            BinSize = 15;
        }

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
                CreateChart(ebPhase, eventLogs, signal);

            var nbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 8);
            if (nbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 4))
                CreateChart(nbPhase, eventLogs, signal);

            var wbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 2);
            if (wbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 6))
                CreateChart(wbPhase, eventLogs, signal);

            var sbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 4);
            if (sbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 8))
                CreateChart(sbPhase, eventLogs, signal);

            return ReturnList;
        }

        private void CreateChart(Approach approach, ControllerEventLogs eventLogs, Signal signal)
        {
            var phaseEvents = new List<Controller_Event_Log>();

            phaseEvents.AddRange(eventLogs.Events.Where(x =>
                x.EventParam == approach.ProtectedPhaseNumber &&
                (x.EventCode == EVENT_GREEN || x.EventCode == EVENT_RED)));
            foreach (var detector in approach.Detectors)
            {
                // Check for thru, right, thru-right, and thru-left
                if (!IsThruDetector(detector)) continue;

                phaseEvents.AddRange(eventLogs.Events.Where(x =>
                    x.EventCode == 81 && x.EventParam == detector.DetChannel));
            }

            if (phaseEvents.Any())
            {
                var leftTurnChart = new LeftTurnGapAnalysisChart(this, signal, approach, phaseEvents,
                    GetOpposingPhase(approach.ProtectedPhaseNumber), StartDate, EndDate);
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
                AxisType.Secondary, $"% Green Time > {TrendLineGapThreshold} seconds");

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
                Alignment = StringAlignment.Center
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

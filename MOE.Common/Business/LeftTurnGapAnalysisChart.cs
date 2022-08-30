using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class LeftTurnGapAnalysisChart
    {
        public Chart Chart;
        private Signal _signal;
        private Approach _approach;
        private readonly List<Controller_Event_Log> _events;
        private readonly int _opposingPhase;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly LeftTurnGapAnalysisOptions _options;
        private readonly string _detectionType;

        private const int CYCLE_GROUP_COUNT = 10;

        public class PhaseLeftTurnGapTracker
        {
            public DateTime GreenTime;
            public int ShortGapCounter;
            public int MediumGapCounter;
            public int LargeGapCounter;
            public int HugeGapCounter;
            public double PercentPhaseTurnable;
        }

        public LeftTurnGapAnalysisChart(LeftTurnGapAnalysisOptions options, Signal signal, Approach approach,
            List<Controller_Event_Log> events, int opposingPhase, DateTime startDate, DateTime endDate, string detectionType)
        {
            _signal = signal;
            _options = options;
            _approach = approach;
            _events = events;
            _opposingPhase = opposingPhase;
            _startDate = startDate;
            _endDate = endDate;
            _detectionType = detectionType;

            options.Y2AxisMax = 100;
            options.Y2AxisTitle = $"% Of Gap Time > {options.TrendLineGapThreshold} seconds";

            Chart = ChartFactory.CreateDefaultChartNoX2Axis(options);
            SetChartTitle(Chart, options, signal, approach, detectionType);
            ChartFactory.SetImageProperties(Chart);

            Chart.ChartAreas[0].AxisY.Title = "# Gaps";
            Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Auto;
            Chart.ChartAreas[0].AxisY.Interval = 20;

            AddDataToChart();
        }

        private void SetChartTitle(Chart chart, LeftTurnGapAnalysisOptions options, Signal signal, Approach approach, string detectionType)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(signal.SignalID, options.StartDate,
                options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(_opposingPhase));
            chart.Titles.Add(detectionType);
        }

        protected void AddDataToChart()
        {
            #region Series Definition

            //Y1 Series
            var shortGapSeries = _options.CreateChartSeries(Color.Red, SeriesChartType.StackedColumn,
                ChartValueType.DateTime,
                AxisType.Primary, $"{_options.Gap1Min} - {_options.Gap1Max} seconds");

            var mediumGapSeries = _options.CreateChartSeries(Color.LawnGreen, SeriesChartType.StackedColumn,
                ChartValueType.DateTime, AxisType.Primary, $"{_options.Gap2Min} - {_options.Gap2Max} seconds");

            var largeGapSeries = _options.CreateChartSeries(Color.Green, SeriesChartType.StackedColumn,
                ChartValueType.DateTime,
                AxisType.Primary, $"{_options.Gap3Min} - {_options.Gap3Max} seconds");

            var hugeGapSeries = _options.CreateChartSeries(Color.LightSeaGreen, SeriesChartType.StackedColumn,
                ChartValueType.DateTime, AxisType.Primary, $"{_options.Gap4Min}+ seconds");

            //Y2 Series
            var percentTurnableSeries = _options.CreateChartSeries(Color.Blue, SeriesChartType.Line,
                ChartValueType.DateTime,
                AxisType.Secondary, $"% Of Gap Time > {_options.TrendLineGapThreshold} seconds");
            percentTurnableSeries.BorderWidth = 2;

            #endregion

            var greenList = _events.Where(x => x.EventCode == LeftTurnGapAnalysisOptions.EVENT_GREEN)
                .OrderBy(x => x.Timestamp);
            var redList = _events.Where(x => x.EventCode == LeftTurnGapAnalysisOptions.EVENT_RED)
                .OrderBy(x => x.Timestamp).ToList();
            var orderedDetectorCallList = _events.Where(x => x.EventCode == LeftTurnGapAnalysisOptions.EVENT_DET)
                .OrderBy(x => x.Timestamp).ToList();
            
            var phaseTrackerList = GetGapsFromControllerData(greenList, redList, orderedDetectorCallList);

            var highestTotal = 0;

            for (var lowerTimeLimit = _startDate; lowerTimeLimit < _endDate; lowerTimeLimit = lowerTimeLimit.AddMinutes(_options.BinSize))
            {
                var upperTimeLimit = lowerTimeLimit.AddMinutes(_options.BinSize);
                var items = phaseTrackerList.Where(x => x.GreenTime >= lowerTimeLimit && x.GreenTime < upperTimeLimit).ToList();

                if (!items.Any()) continue;

                shortGapSeries.Points.AddXY(upperTimeLimit, items.Sum(x => x.ShortGapCounter));
                mediumGapSeries.Points.AddXY(upperTimeLimit, items.Sum(x => x.MediumGapCounter));
                largeGapSeries.Points.AddXY(upperTimeLimit, items.Sum(x => x.LargeGapCounter));
                hugeGapSeries.Points.AddXY(upperTimeLimit, items.Sum(x => x.HugeGapCounter));

                var localTotal = items.Sum(x => x.ShortGapCounter) + items.Sum(x => x.MediumGapCounter)
                                                                   + items.Sum(x => x.LargeGapCounter) +
                                                                   items.Sum(x => x.HugeGapCounter);
                percentTurnableSeries.Points.AddXY(upperTimeLimit,
                    items.Average(x => x.PercentPhaseTurnable) * 100);
                if (localTotal > highestTotal)
                    highestTotal = localTotal;
            }

            //Find the highest max and round up to the next 100
            Chart.ChartAreas[0].AxisY.Maximum = Math.Ceiling(highestTotal / 100d) * 100;

            //Y1
            Chart.Series.Add(shortGapSeries);
            Chart.Series.Add(mediumGapSeries);
            Chart.Series.Add(largeGapSeries);
            Chart.Series.Add(hugeGapSeries);

            //Y2
            Chart.Series.Add(percentTurnableSeries);
        }

        private List<PhaseLeftTurnGapTracker> GetGapsFromControllerData(IEnumerable<Controller_Event_Log> greenList,
            List<Controller_Event_Log> redList, List<Controller_Event_Log> orderedDetectorCallList)
        {
            var phaseTrackerList = new List<PhaseLeftTurnGapTracker>();

            foreach (var green in greenList)
            {
                //Find the corresponding red
                var red = redList.Where(x => x.Timestamp > green.Timestamp).OrderBy(x => x.Timestamp).FirstOrDefault();
                if (red == null)
                    continue;

                double trendLineGapTimeCounter = 0;

                var phaseTracker = new PhaseLeftTurnGapTracker {GreenTime = green.Timestamp};

                var gapsList = new List<Controller_Event_Log>();
                gapsList.Add(green);
                gapsList.AddRange(orderedDetectorCallList.Where(x =>
                    x.Timestamp > green.Timestamp && x.Timestamp < red.Timestamp));
                gapsList.Add(red);

                for (var i = 1; i < gapsList.Count; i++)
                {
                    var gap = gapsList[i].Timestamp.TimeOfDay.TotalSeconds -
                              gapsList[i - 1].Timestamp.TimeOfDay.TotalSeconds;

                    if (gap < 0) continue;

                    AddGapToCounters(phaseTracker, gap);

                    if (gap >= _options.TrendLineGapThreshold)
                    {
                        trendLineGapTimeCounter += gap;
                    }
                }

                //Decimal rounding errors can cause the number to be > 100
                var percentTurnable =
                    Math.Min(trendLineGapTimeCounter / (red.Timestamp - green.Timestamp).TotalSeconds, 100);
                phaseTracker.PercentPhaseTurnable = percentTurnable;

                phaseTrackerList.Add(phaseTracker);
            }

            return phaseTrackerList;
        }

        public void AddGapToCounters(PhaseLeftTurnGapTracker phaseTracker, double gap)
        {
            if (gap > _options.Gap1Min && gap <= _options.Gap1Max)
            {
                phaseTracker.ShortGapCounter++;
            }
            else if (gap > _options.Gap2Min && gap <= _options.Gap2Max)
            {
                phaseTracker.MediumGapCounter++;
            }
            else if (gap > _options.Gap3Min && gap <= _options.Gap3Max)
            {
                phaseTracker.LargeGapCounter++;
            }
            else if (gap > _options.Gap4Min)
            {
                phaseTracker.HugeGapCounter++;
            }
        }
    }
}
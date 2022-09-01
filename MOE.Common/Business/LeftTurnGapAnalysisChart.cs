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
        private readonly List<Controller_Event_Log> _events;
        private readonly int _opposingPhase;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly LeftTurnGapAnalysisOptions _options;

        public LeftTurnGapAnalysis.LeftTurnGapAnalysis GapData { get; }

        public class PhaseLeftTurnGapTracker
        {
            public DateTime GreenTime;
            public int GapCounter1;
            public int GapCounter2;
            public int GapCounter3;
            public int GapCounter4;
            public int GapCounter5;
            public int GapCounter6;
            public int GapCounter7;
            public int GapCounter8;
            public int GapCounter9;
            public int GapCounter10;
            public int GapCounter11;
            public double PercentPhaseTurnable;
        }

        public LeftTurnGapAnalysisChart(LeftTurnGapAnalysisOptions options, LeftTurnGapAnalysis.LeftTurnGapAnalysis gapData,
             int opposingPhase)
        {
            _options = options;
            GapData = gapData;
            _opposingPhase = opposingPhase;

            options.Y2AxisMax = 100;
            options.Y2AxisTitle = $"% of Green Time where Gap ≥ {options.TrendLineGapThreshold} seconds";

            Chart = ChartFactory.CreateDefaultChartNoX2Axis(options);
            SetChartTitle(Chart, options, GapData.Approach.Signal, GapData.Approach, GapData.DetectionTypeStr);
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
            Title phaseTitle = ChartTitleFactory.GetPhase(_opposingPhase);
            phaseTitle.Text = ("Left Turn Crossing " + phaseTitle.Text);
            chart.Titles.Add(phaseTitle);
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

            foreach (var gap in GapData.Gaps1)
            {
                shortGapSeries.Points.AddXY(gap.Key, gap.Value);
            }
            foreach (var gap in GapData.Gaps2)
            {
                mediumGapSeries.Points.AddXY(gap.Key, gap.Value);
            }
            foreach (var gap in GapData.Gaps3)
            {
                largeGapSeries.Points.AddXY(gap.Key, gap.Value);
            }
            foreach (var gap in GapData.Gaps4)
            {
                hugeGapSeries.Points.AddXY(gap.Key, gap.Value);
            }
            foreach (var percent in GapData.PercentTurnableSeries)
            {
                percentTurnableSeries.Points.AddXY(percent.Key, percent.Value);
            }
            

            //Find the highest max and round up to the next 100
            Chart.ChartAreas[0].AxisY.Maximum = Math.Ceiling(GapData.HighestTotal / 100d) * 100;

            //Y1
            Chart.Series.Add(shortGapSeries);
            Chart.Series.Add(mediumGapSeries);
            Chart.Series.Add(largeGapSeries);
            Chart.Series.Add(hugeGapSeries);

            //Y2
            Chart.Series.Add(percentTurnableSeries);
        }

        //private List<PhaseLeftTurnGapTracker> GetGapsFromControllerData(IEnumerable<Controller_Event_Log> greenList,
        //    List<Controller_Event_Log> redList, List<Controller_Event_Log> orderedDetectorCallList)
        //{
        //    var phaseTrackerList = new List<PhaseLeftTurnGapTracker>();

        //    foreach (var green in greenList)
        //    {
        //        //Find the corresponding red
        //        var red = redList.Where(x => x.Timestamp > green.Timestamp).OrderBy(x => x.Timestamp).FirstOrDefault();
        //        if (red == null)
        //            continue;

        //        double trendLineGapTimeCounter = 0;

        //        var phaseTracker = new PhaseLeftTurnGapTracker {GreenTime = green.Timestamp};

        //        var gapsList = new List<Controller_Event_Log>();
        //        gapsList.Add(green);
        //        gapsList.AddRange(orderedDetectorCallList.Where(x =>
        //            x.Timestamp > green.Timestamp && x.Timestamp < red.Timestamp));
        //        gapsList.Add(red);

        //        for (var i = 1; i < gapsList.Count; i++)
        //        {
        //            var gap = gapsList[i].Timestamp.TimeOfDay.TotalSeconds -
        //                      gapsList[i - 1].Timestamp.TimeOfDay.TotalSeconds;

        //            if (gap < 0) continue;

        //            AddGapToCounters(phaseTracker, gap);

        //            if (gap >= _options.TrendLineGapThreshold)
        //            {
        //                trendLineGapTimeCounter += gap;
        //            }
        //        }

        //        //Decimal rounding errors can cause the number to be > 100
        //        var percentTurnable =
        //            Math.Min(trendLineGapTimeCounter / (red.Timestamp - green.Timestamp).TotalSeconds, 100);
        //        phaseTracker.PercentPhaseTurnable = percentTurnable;

        //        phaseTrackerList.Add(phaseTracker);
        //    }

        //    return phaseTrackerList;
        //}

        //public void AddGapToCounters(PhaseLeftTurnGapTracker phaseTracker, double gap)
        //{
        //    if (gap > _options.Gap1Min && gap <= _options.Gap1Max)
        //    {
        //        phaseTracker.GapCounter1++;
        //    }
        //    else if (gap > _options.Gap2Min && gap <= _options.Gap2Max)
        //    {
        //        phaseTracker.GapCounter2++;
        //    }
        //    else if (gap > _options.Gap3Min && gap <= _options.Gap3Max)
        //    {
        //        phaseTracker.GapCounter3++;
        //    }
        //    else if (gap > _options.Gap4Min)
        //    {
        //        phaseTracker.GapCounter4++;
        //    }
        //}
    }
}
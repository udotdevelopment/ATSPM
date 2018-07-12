using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public abstract class DetectorAggregationMetricOptions : ApproachAggregationMetricOptions
    {
        public override string YAxisTitle { get; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            GetSignalObjects();
            if (SelectedXAxisType == XAxisType.TimeOfDay &&
                TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
            {
                TimeOptions.TimeOption = BinFactoryOptions.TimeOptions.TimePeriod;
                TimeOptions.TimeOfDayStartHour = 0;
                TimeOptions.TimeOfDayStartMinute = 0;
                TimeOptions.TimeOfDayEndHour = 23;
                TimeOptions.TimeOfDayEndMinute = 59;
                if (TimeOptions.DaysOfWeek == null)
                    TimeOptions.DaysOfWeek = new List<DayOfWeek>
                    {
                        DayOfWeek.Sunday,
                        DayOfWeek.Monday,
                        DayOfWeek.Tuesday,
                        DayOfWeek.Wednesday,
                        DayOfWeek.Thursday,
                        DayOfWeek.Friday,
                        DayOfWeek.Saturday
                    };
            }
            return ReturnList;
        }


        protected override void GetChartByXAxisAggregation()
        {
            switch (SelectedXAxisType)
            {
                case XAxisType.Time:
                    GetTimeCharts();
                    break;
                case XAxisType.TimeOfDay:
                    GetTimeOfDayCharts();
                    break;
                case XAxisType.Approach:
                    GetApproachCharts();
                    break;
                case XAxisType.Direction:
                    GetDirectionCharts();
                    break;
                case XAxisType.Signal:
                    GetSignalCharts();
                    break;
                case XAxisType.Detector:
                    GetDetectorCharts();
                    break;
                default:
                    throw new Exception("Invalid X-Axis");
            }
        }

        private void GetDetectorCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateStringXIntYChart(this);
                        GetApproachXAxisChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                case SeriesType.Detector:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateStringXIntYChart(this);
                        GetApproachXAxisDetectorSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetApproachXAxisDetectorSeriesChart(Models.Signal signal, Chart chart)
        {
            var series = CreateSeries(0, signal.SignalDescription);
            var i = 1;
            foreach (var approach in signal.Approaches)
            foreach (var detector in approach.Detectors)
            {
                var binsContainers = GetBinsContainersByDetector(detector);
                var dataPoint = new DataPoint();
                dataPoint.XValue = i;
                dataPoint.Color = GetSeriesColorByNumber(i);
                if (SelectedAggregationType == AggregationType.Sum)
                    dataPoint.SetValueY(binsContainers.FirstOrDefault().SumValue);
                else
                    dataPoint.SetValueY(binsContainers.FirstOrDefault().AverageValue);
                dataPoint.AxisLabel = detector.Description;
                series.Points.Add(dataPoint);
                i++;
            }
            chart.Series.Add(series);
        }


        protected override void GetTimeCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeXAxisApproachSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                case SeriesType.Direction:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeXAxisDirectionSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    ;
                    break;
                case SeriesType.Detector:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeXAxisDetectorSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                case SeriesType.Signal:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeXAxisSignalSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                case SeriesType.Route:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    SetTimeXAxisRouteSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetTimeXAxisDetectorSeriesChart(Models.Signal signal, Chart chart)
        {
            var i = 1;
            foreach (var approach in signal.Approaches)
            foreach (var detector in approach.Detectors)
            {
                var binsContainers = GetBinsContainersByDetector(detector);
                var series = CreateSeries(i, detector.Description);
                if ((TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Month ||
                     TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Year) &&
                    TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
                    foreach (var binsContainer in binsContainers)
                    {
                        var dataPoint = SelectedAggregationType == AggregationType.Sum
                            ? GetContainerDataPointForSum(binsContainer)
                            : GetContainerDataPointForAverage(binsContainer);
                        series.Points.Add(dataPoint);
                    }
                else
                    foreach (var bin in binsContainers.FirstOrDefault()?.Bins)
                    {
                        var dataPoint = SelectedAggregationType == AggregationType.Sum
                            ? GetDataPointForSum(bin)
                            : GetDataPointForAverage(bin);
                        series.Points.Add(dataPoint);
                    }
                chart.Series.Add(series);
                i++;
            }
        }

        protected override void GetSignalCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    chart = ChartFactory.CreateStringXIntYChart(this);
                    GetSignalsXAxisPhaseNumberSeriesChart(Signals, chart);
                    break;
                case SeriesType.Direction:
                    chart = ChartFactory.CreateStringXIntYChart(this);
                    GetSignalsXAxisDirectionSeriesChart(Signals, chart);
                    break;
                case SeriesType.Signal:
                    chart = ChartFactory.CreateStringXIntYChart(this);
                    GetSignalsXAxisSignalSeriesChart(Signals, chart);
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
            SaveChartImage(chart);
        }


        protected override void GetTimeOfDayCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeOfDayXAxisApproachSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                case SeriesType.Direction:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeOfDayXAxisDirectionSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    ;
                    break;
                case SeriesType.Signal:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeOfDayXAxisSignalSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                case SeriesType.Route:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeOfDayXAxisRouteSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                case SeriesType.Detector:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeOfDayXAxisDetectorSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetTimeOfDayXAxisDetectorSeriesChart(Models.Signal signal, Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
            var seriesList = new ConcurrentBag<Series>();
            foreach (var approach in signal.Approaches)
            {
                var detectors = approach.Detectors.ToList();
                Parallel.For(0, detectors.Count, i =>
                {
                    var binsContainers = GetBinsContainersByDetector(detectors[i]);
                    var series = CreateSeries(i, detectors[i].Description);
                    SetTimeAggregateSeries(series, binsContainers);
                    seriesList.Add(series);
                    ;
                });
            }
            var orderedSeries = seriesList.OrderBy(s => s.Name).ToList();
            var colorCode = 1;
            foreach (var series in orderedSeries)
            {
                series.Color = GetSeriesColorByNumber(colorCode);
                chart.Series.Add(series);
            }
        }


        protected abstract List<BinsContainer> GetBinsContainersByDetector(Models.Detector detector);
    }
}
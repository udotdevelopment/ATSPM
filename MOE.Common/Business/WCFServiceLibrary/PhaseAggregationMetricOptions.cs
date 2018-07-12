using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public abstract class PhaseAggregationMetricOptions : SignalAggregationMetricOptions
    {
        public override string YAxisTitle { get; }
        public List<int> AvailablePhaseNumbers { get; set; }

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
                    GetPhaseCharts();
                    break;
                case XAxisType.Signal:
                    GetSignalCharts();
                    break;
                default:
                    throw new Exception("Invalid X-Axis");
            }
        }

        

        public List<DirectionType> GetFilteredDirections()
        {
            var direcitonRepository = DirectionTypeRepositoryFactory.Create();
            var includedDirections = FilterDirections.Where(f => f.Include).Select(f => f.DirectionTypeId).ToList();
            var directionsList = direcitonRepository.GetDirectionsByIDs(includedDirections);
            return directionsList;
        }

        protected void GetPhaseCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateStringXIntYChart(this);
                        GetPhaseXAxisChart(signal, chart);
                        if (ShowEventCount)
                        {
                            SetApproachXAxisSignalSeriesForEventCount(chart, signal);
                        }
                        SaveChartImage(chart);
                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        protected override void GetTimeCharts()
        {
            Chart chart = null;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        var availablePhaseNumbers = GetAvailablePhaseNumbers(signal);
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeXAxisApproachSeriesChart(signal, chart, availablePhaseNumbers);
                        if (ShowEventCount)
                        {
                            SetTimeXAxisSignalSeriesForEventCount(chart, signal);
                        }
                    }
                    break;
                case SeriesType.Signal:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeXAxisSignalSeriesChart(Signals, chart);
                    if (ShowEventCount)
                    {
                        SetTimeXAxisRouteSeriesForEventCount(Signals, chart);

                    }
                    break;
                case SeriesType.Route:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    SetTimeXAxisRouteSeriesChart(Signals, chart);
                    if (ShowEventCount)
                    {
                        SetTimeXAxisRouteSeriesForEventCount(Signals, chart);

                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
            SaveChartImage(chart);
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
                case SeriesType.Signal:
                    chart = ChartFactory.CreateStringXIntYChart(this);
                    GetSignalsXAxisSignalSeriesChart(Signals, chart);
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
            if (ShowEventCount)
            {
                SetSignalsXAxisSignalSeriesForEventCount(Signals, chart);
            }
            SaveChartImage(chart);
        }

        protected void GetTimeOfDayXAxisPhaseSeriesChart(Models.Signal signal, Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
            var seriesList = new ConcurrentBag<Series>();
            var availablePhaseNumbers = GetAvailablePhaseNumbers(signal);
            try
            {
                Parallel.For(0, availablePhaseNumbers.Count, i =>
                {
                    var binsContainers = GetBinsContainersByPhaseNumber(signal, availablePhaseNumbers[i]);
                    var series = CreateSeries(i, "Phase " + availablePhaseNumbers[i].ToString());
                    SetTimeAggregateSeries(series, binsContainers);
                    seriesList.Add(series);
                });
            }
            catch (Exception e)
            {
                throw;
            }
            var orderedSeries = seriesList.OrderBy(s => s.Name).ToList();
            foreach (var series in orderedSeries)
                chart.Series.Add(series);
        }


        protected override void GetTimeOfDayCharts()
        {
            Chart chart = null;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> {signal});
                        GetTimeOfDayXAxisPhaseSeriesChart(signal, chart);
                        if (ShowEventCount)
                        {
                            SetTimeofDayAxisSignalSeriesForEventCount(signal, chart);
                        }
                    }
                    break;
                case SeriesType.Signal:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeOfDayXAxisSignalSeriesChart(Signals, chart);
                    if (ShowEventCount)
                    {
                        SetTimeOfDayAxisRouteSeriesForEventCount(Signals, chart);
                    }
                    break;
                case SeriesType.Route:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeOfDayXAxisRouteSeriesChart(Signals, chart);
                    if (ShowEventCount)
                    {
                        SetTimeOfDayAxisRouteSeriesForEventCount(Signals, chart);
                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
            SaveChartImage(chart);
        }
        

        private void SetTimeofDayAxisSignalSeriesForEventCount(Models.Signal signal, Chart chart)
        {
            var eventCountOptions = new  SignalEventCountAggregationOptions(this);
            var binsContainers = eventCountOptions.GetBinsContainersByRoute(new List<Models.Signal> { signal });
            Series series = CreateEventCountSeries();
            eventCountOptions.SetTimeAggregateSeries(series, binsContainers);
            chart.Series.Add(series);
        }





        protected void GetSignalsXAxisPhaseNumberSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            var availablePhaseNumbers = new List<int>();
            foreach (var signal in signals)
                availablePhaseNumbers.AddRange(GetAvailablePhaseNumbers(signal));
            availablePhaseNumbers = availablePhaseNumbers.Distinct().ToList();
            var colorCode = 1;
            foreach (var phaseNumber in availablePhaseNumbers)
            {
                var seriesName = "Phase " + phaseNumber;
                Series series = GetSignalXAxisPhaseNumberSeries(signals, colorCode, phaseNumber, seriesName);
                colorCode++;
                chart.Series.Add(series);
            }
        }

        public Series GetSignalXAxisPhaseNumberSeries(List<Models.Signal> signals, int colorCode, int phaseNumber, string seriesName)
        {
            var series = CreateSeries(colorCode, seriesName);
            foreach (var signal in signals)
            {
                var binsContainers = GetBinsContainersByPhaseNumber(signal, phaseNumber);
                var dataPoint = new DataPoint();
                dataPoint.SetValueY(SelectedAggregationType == AggregationType.Sum
                    ? binsContainers.Sum(b => b.SumValue)
                    : binsContainers.Average(b => b.SumValue));
                dataPoint.AxisLabel = signal.SignalDescription;
                series.Points.Add(dataPoint);
            }

            return series;
        }


       

        protected void GetPhaseXAxisChart(Models.Signal signal, Chart chart)
        {
            Series series = GetPhaseXAxisPhaseSeries(signal, 0);
            chart.Series.Add(series);
        }

        public Series GetPhaseXAxisPhaseSeries(Models.Signal signal, int colorCode)
        {
            var series = CreateSeries(colorCode, signal.SignalDescription);
            var i = 1;
            var phaseNumbers = signal.GetPhasesForSignal();
            foreach (var phaseNumber in phaseNumbers)
            {
                var dataPoint = new DataPoint();
                dataPoint.XValue = i;
                if (SelectedAggregationType == AggregationType.Sum)
                    dataPoint.SetValueY(GetSumByPhaseNumber(signal, phaseNumber));
                else
                    dataPoint.SetValueY(GetAverageByPhaseNumber(signal, phaseNumber));
                dataPoint.AxisLabel = "Phase " + phaseNumber;
                dataPoint.Color = GetSeriesColorByNumber(i);
                series.Points.Add(dataPoint);
                i++;
            }
            return series;
        }

        protected void GetTimeXAxisApproachSeriesChart(Models.Signal signal, Chart chart, List<int> availablePhaseNumbers)
        {
            var i = 1;
            foreach (var phase in availablePhaseNumbers)
            {
                GetPhaseTimeSeries(chart, i, phase, signal);
                i++;
            }
        }
        

        private void GetPhaseTimeSeries(Chart chart, int i, int phaseNumber,
            Models.Signal signal)
        {
            Series series = GetTimeXAxisPhaseSeries(i, phaseNumber, signal);
            chart.Series.Add(series);
        }

        private Series GetTimeXAxisPhaseSeries(int colorCode, int phaseNumber, Models.Signal signal)
        {
            var binsContainers = GetBinsContainersByPhaseNumber(signal, phaseNumber);
            var series = CreateSeries(colorCode, "Phase " + phaseNumber);
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

            return series;
        }


        public virtual void SetApproachXAxisSignalSeriesForEventCount(Chart chart, Models.Signal signal)
        {
            var eventCountOptions = new SignalEventCountAggregationOptions(this);
            Series series = eventCountOptions.GetSignalsXAxisSignalSeries(new List<Models.Signal>{signal}, signal.SignalDescription);
            
            chart.Series.Add(SetEventCountSeries(series));
        }


        public virtual void SetTimeXAxisSignalSeriesForEventCount(Chart chart, Models.Signal signal)
        {
            var eventCountOptions = new SignalEventCountAggregationOptions(this);
            Series series = eventCountOptions.GetTimeXAxisSignalSeries(signal);
            
            chart.Series.Add(SetEventCountSeries(series));
        }

        public override void SetSignalsXAxisSignalSeriesForEventCount(List<Models.Signal> signals, Chart chart)
        {
            var eventCountOptions = new SignalEventCountAggregationOptions(this);
            Series series = eventCountOptions.GetSignalsXAxisSignalSeries(signals, "Event Count");
            chart.Series.Add(SetEventCountSeries(series));
        }

        public override void SetTimeXAxisRouteSeriesForEventCount(List<Models.Signal> signals, Chart chart)
        {
            var eventCountOptions = new SignalEventCountAggregationOptions(this);
            Series series = eventCountOptions.GetTimeXAxisRouteSeries(signals);
            chart.Series.Add(SetEventCountSeries(series));
        }

        public override void SetTimeOfDayAxisRouteSeriesForEventCount(List<Models.Signal> signals, Chart chart)
        {
            var eventCountOptions = new SignalEventCountAggregationOptions(this);
            Series eventCountSeries = CreateEventCountSeries();
            var eventBinsContainers = eventCountOptions.GetBinsContainersByRoute(signals);
            eventCountOptions.SetTimeAggregateSeries(eventCountSeries, eventBinsContainers);
            chart.Series.Add(eventCountSeries);
        }

        protected abstract List<int> GetAvailablePhaseNumbers(Models.Signal signal);
        protected abstract int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber);

    }
}
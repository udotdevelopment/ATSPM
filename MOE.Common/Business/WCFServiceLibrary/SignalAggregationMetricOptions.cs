using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public enum AggregationType
    {
        Sum,
        Average
    }

    public enum SeriesType
    {
        Signal,
        PhaseNumber,
        Direction,
        Route,
        Detector
    }

    public enum XAxisType
    {
        Time,
        TimeOfDay,
        Direction,
        Phase,
        Signal,
        Detector
    }

    public enum Dimension
    {
        TwoDimensional,
        ThreeDimensional
    }

    [DataContract]
    public class AggregatedDataType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DataName { get; set; }
    }

    [DataContract]
    public abstract class SignalAggregationMetricOptions : MetricOptions
    {
        [DataMember] public List<AggregatedDataType> AggregatedDataTypes;

        [DataMember]
        public int SeriesWidth { get; set; }

        [DataMember]
        public BinFactoryOptions TimeOptions { get; set; }

        [DataMember]
        public SeriesChartType SelectedChartType { get; set; }

        [DataMember]
        public AggregationType SelectedAggregationType { get; set; }

        [DataMember]
        public XAxisType SelectedXAxisType { get; set; }

        [DataMember]
        public SeriesType SelectedSeries { get; set; }

        [DataMember]
        public Dimension SelectedDimension { get; set; }

        [DataMember]
        public List<FilterSignal> FilterSignals { get; set; } = new List<FilterSignal>();

        [DataMember]
        public List<FilterDirection> FilterDirections { get; set; }

        [DataMember]
        public List<FilterMovement> FilterMovements { get; set; }

        [DataMember]
        public AggregatedDataType SelectedAggregatedDataType { get; set; }

        public List<Models.Signal> Signals { get; set; } = new List<Models.Signal>();

        public virtual string ChartTitle
        {
            get
            {
                string chartTitle;
                chartTitle = "AggregationChart\n";
                chartTitle += TimeOptions.Start.ToString();
                if (TimeOptions.End > TimeOptions.Start)
                    chartTitle += " to " + TimeOptions.End + "\n";
                if (TimeOptions.DaysOfWeek != null)
                    foreach (var dayOfWeek in TimeOptions.DaysOfWeek)
                        chartTitle += dayOfWeek + " ";
                if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute != null &&
                    TimeOptions.TimeOfDayEndHour != null && TimeOptions.TimeOfDayEndMinute != null)
                    chartTitle += "Limited to: " +
                                  new TimeSpan(0, TimeOptions.TimeOfDayStartHour.Value,
                                      TimeOptions.TimeOfDayStartMinute.Value, 0) + " to " + new TimeSpan(0,
                                      TimeOptions.TimeOfDayEndHour.Value,
                                      TimeOptions.TimeOfDayEndMinute.Value, 0) + "\n";
                chartTitle += TimeOptions.SelectedBinSize + " bins ";
                chartTitle += SelectedXAxisType + " Aggregation ";
                chartTitle += SelectedAggregationType.ToString();
                return chartTitle;
            }
        }

        public abstract string YAxisTitle { get; }
        public bool ShowEventCount { get; set; }


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
            GetChartByXAxisAggregation();
            return ReturnList;
        }

        protected virtual void GetChartByXAxisAggregation()
        {
            switch (SelectedXAxisType)
            {
                case XAxisType.Time:
                    GetTimeCharts();
                    break;
                case XAxisType.TimeOfDay:
                    GetTimeOfDayCharts();
                    break;
                case XAxisType.Signal:
                    GetSignalCharts();
                    break;
                default:
                    throw new Exception("Invalid X-Axis");
            }
        }


        protected void GetSignalObjects()
        {
            try
            {
                if (Signals == null)
                    Signals = new List<Models.Signal>();
                if (Signals.Count == 0)
                {
                    var signalRepository = SignalsRepositoryFactory.Create();
                    foreach (var filterSignal in FilterSignals)
                        if (!filterSignal.Exclude)
                        {
                            var signals =
                                signalRepository.GetSignalsBetweenDates(filterSignal.SignalId, StartDate, EndDate);
                            foreach (var signal in signals)
                            {
                                RemoveApproachesByFilter(filterSignal, signal);
                                signal.Approaches = signal.Approaches.OrderBy(a => a.ProtectedPhaseNumber).ToList();
                            }
                            Signals.AddRange(signals);
                        }
                }
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().GetName().ToString(),
                    GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("Unable to apply signal filter");
            }
        }

        protected void SetMetricType()
        {
            var metricTypeRepository = MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
        }

        protected void SaveChartImage(Chart chart)
        {
            var chartName = CreateFileName(MetricType.Abbreviation);
            MetricFileLocation = ConfigurationManager.AppSettings["ImageLocation"];
            MetricWebPath = ConfigurationManager.AppSettings["ImageWebLocation"];
            chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
            //SerializeMetricData(chart);
        }

        protected string CreateFileName(string MetricAbbreviation)
        {
            var fileName = MetricAbbreviation +
                           "-" +
                           StartDate.Year +
                           StartDate.ToString("MM") +
                           StartDate.ToString("dd") +
                           StartDate.ToString("HH") +
                           StartDate.ToString("mm") +
                           "-" +
                           EndDate.Year +
                           EndDate.ToString("MM") +
                           EndDate.ToString("dd") +
                           EndDate.ToString("HH") +
                           EndDate.ToString("mm-");
            var r = new Random();
            fileName += r.Next().ToString();
            fileName += ".jpg";
            return fileName;
        }


        protected virtual void GetTimeCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.Signal:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeXAxisSignalSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                case SeriesType.Route:
                    chart = ChartFactory.CreateTimeXIntYChart(this, Signals);
                    GetTimeXAxisRouteSeriesChart(Signals, chart);
                    SaveChartImage(chart);
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        protected virtual void GetSignalCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.Signal:
                    chart = ChartFactory.CreateStringXIntYChart(this);
                    GetSignalsXAxisSignalSeriesChart(Signals, chart);
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
            SaveChartImage(chart);
        }

        protected void GetSignalsXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            var seriesName = "Signals";
            var series = CreateSeries(0, seriesName);
            Series eventCountSeries = CreateEventCountSeries();
            foreach (var signal in signals)
            {
                var binsContainers = GetBinsContainersBySignal(signal);
                var dataPoint = new DataPoint();
                dataPoint.SetValueY(SelectedAggregationType == AggregationType.Sum
                    ? binsContainers.Sum(b => b.SumValue)
                    : Convert.ToInt32(Math.Round(binsContainers.Sum(b => b.SumValue) / (double) signals.Count)));
                dataPoint.AxisLabel = signal.SignalDescription;
                series.Points.Add(dataPoint);
                if (ShowEventCount)
                {
                    DataPoint eventCountDataPoint = new DataPoint();
                    eventCountDataPoint.SetValueY(binsContainers.Sum(b => b.Y2AxisValue));
                    eventCountDataPoint.AxisLabel = signal.SignalDescription;
                    eventCountSeries.Points.Add(eventCountDataPoint);
                }
            }
            chart.Series.Add(series);
            if (ShowEventCount)
            {
                chart.Series.Add(eventCountSeries);
            }
        }

        protected void GetTimeXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            var series = CreateSeries(0, "Route");
            Series eventCountSeries = new Series();
            if (ShowEventCount)
            {
                eventCountSeries = CreateEventCountSeries();
            }
            var binsContainers = GetBinsContainersByRoute(signals);
            foreach (var binsContainer in binsContainers)
            {
                foreach (var bin in binsContainer.Bins)
                {
                    series.Points.Add(SelectedAggregationType == AggregationType.Sum
                        ? GetDataPointForSum(bin)
                        : GetDataPointForAverage(bin));
                    if (ShowEventCount)
                    {
                        DataPoint eventCountDataPoint = GetDataPointForY2AxisValue(bin);
                        eventCountSeries.Points.Add(eventCountDataPoint);
                    }
                }
                chart.Series.Add(series);
            }
            if (ShowEventCount)
            {
                chart.Series.Add(eventCountSeries);
            }

        }


        protected virtual void GetTimeOfDayCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
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
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        protected void GetTimeOfDayXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            var binsContainers = GetBinsContainersByRoute(signals);
            var series = CreateSeries(0, "Route");
            chart.Series.Add(GetTimeAggregateSeries(series, binsContainers));
            if (ShowEventCount)
            {
                chart.Series.Add(GetTimeAggregateEventCountSeries(CreateEventCountSeries(), binsContainers));
            }
        }

        protected void GetTimeOfDayXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            var seriesList = new ConcurrentBag<Series>();
            var binsContainers = new ConcurrentBag<BinsContainer>();
            Parallel.For(0, signals.Count, i => // foreach (var signal in signals)
            {
                var signalBinsContainers = GetBinsContainersBySignal(signals[i]);
                binsContainers.Add(signalBinsContainers.FirstOrDefault());
                var series = CreateSeries(i, signals[i].SignalDescription);
                seriesList.Add(GetTimeAggregateSeries(series, signalBinsContainers));
            });
            int colorIndex = 1;
            foreach (var signal in signals)
            {
                var series = seriesList.FirstOrDefault(s => s.Name == signal.SignalDescription);
                series.Color = GetSeriesColorByNumber(colorIndex);
                chart.Series.Add(series);
                colorIndex++;
            }
            if (ShowEventCount)
            {
                chart.Series.Add(GetTimeAggregateEventCountSeries(CreateEventCountSeries(), binsContainers.ToList()));
            }
        }

        protected void SetTimeOfDayXAxisMinimum(Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
        }

        protected void SetTimeXAxisAxisMinimum(Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    this.StartDate
                        .AddHours(-1).ToOADate();
        }

        protected void GetTimeXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            SetTimeXAxisAxisMinimum(chart);
            var i = 1;
            List<BinsContainer> binContainersForEventCountSeries = new List<BinsContainer>();
            foreach (var signal in signals)
            {
                var series = CreateSeries(i, signal.SignalDescription);
                var binsContainers = GetBinsContainersBySignal(signal);
                binContainersForEventCountSeries.Add(binsContainers.FirstOrDefault());
                foreach (var container in binsContainers)
                {
                    foreach (var bin in container.Bins)
                    {
                        DataPoint dataPoint;
                        if (bin != null)
                        {
                            if (this.SelectedAggregationType == AggregationType.Sum)
                            {
                                dataPoint = GetDataPointForSum(bin);
                            }
                            else
                            {
                                dataPoint = GetDataPointForAverage(bin);
                            }
                            series.Points.Add(dataPoint);
                            i++;
                        }
                    }
                }
                chart.Series.Add(series);
            }
            if (ShowEventCount)
            {
                Series eventCountSeries = new Series();
                if (ShowEventCount)
                {
                    eventCountSeries = CreateEventCountSeries();
                }
                for (int index = 0; index < binContainersForEventCountSeries[0].Bins.Count; index++)
                {
                    int eventCountSum = 0;
                    foreach (var binsContainer in binContainersForEventCountSeries)
                    {
                        eventCountSum += binsContainer.Bins[index].Y2Axis;
                    }
                    eventCountSeries.Points.Add(new DataPoint(binContainersForEventCountSeries[0].Bins[index].Start.ToOADate(), binContainersForEventCountSeries[0].Bins[index].Y2Axis));
                }
                chart.Series.Add(eventCountSeries);
            }
        }


        private void RemoveApproachesByFilter(FilterSignal filterSignal, Models.Signal signal)
        {
            RemoveApproachesFromSignalByDirection(signal);
            RemoveDetectorsFromSignalByMovement(signal);
            var approachRepository = ApproachRepositoryFactory.Create();
            var excludedApproachIds =
                filterSignal.FilterApproaches.Where(f => f.Exclude).Select(f => f.ApproachId).ToList();
            var excludedApproaches = approachRepository.GetApproachesByIds(excludedApproachIds);
            foreach (var excludedApproach in excludedApproaches)
            {
                var approachesToExclude = signal.Approaches.Where(a =>
                        a.DirectionTypeID == excludedApproach.DirectionTypeID
                        && a.ProtectedPhaseNumber == excludedApproach.ProtectedPhaseNumber
                        && a.PermissivePhaseNumber == excludedApproach.PermissivePhaseNumber
                        && a.IsPermissivePhaseOverlap ==
                        excludedApproach.IsPermissivePhaseOverlap
                        && a.IsProtectedPhaseOverlap ==
                        excludedApproach.IsProtectedPhaseOverlap)
                    .ToList();
                foreach (var approachToExclude in approachesToExclude)
                    signal.Approaches.Remove(approachToExclude);
                foreach (var approach in signal.Approaches)
                foreach (var filterApproach in filterSignal.FilterApproaches.Where(f => !f.Exclude))
                    RemoveDetectorsFromApproachByFilter(filterApproach, approach);
            }
        }

        private void RemoveApproachesFromSignalByDirection(Models.Signal signal)
        {
            var approachesToRemove = new List<Approach>();
            foreach (var approach in signal.Approaches)
                if (FilterDirections.Where(f => !f.Include).Select(f => f.DirectionTypeId).ToList()
                    .Contains(approach.DirectionTypeID))
                    approachesToRemove.Add(approach);
            foreach (var approach in approachesToRemove)
                signal.Approaches.Remove(approach);
        }

        private void RemoveDetectorsFromSignalByMovement(Models.Signal signal)
        {
            var detectorsToRemove = new List<Models.Detector>();
            foreach (var approach in signal.Approaches)
            {
                foreach (var detector in approach.Detectors)
                    if (FilterMovements.Where(f => !f.Include).Select(f => f.MovementTypeId).ToList()
                        .Contains(detector.MovementTypeID ?? -1))
                        detectorsToRemove.Add(detector);
                foreach (var detectorToRemove in detectorsToRemove)
                    approach.Detectors.Remove(detectorToRemove);
            }
        }

        private static void RemoveDetectorsFromApproachByFilter(FilterApproach filterApproach, Approach approach)
        {
            var detectorRepository = DetectorRepositoryFactory.Create();
            var excludedDetectorIds =
                filterApproach.FilterDetectors.Where(f => f.Exclude).Select(f => f.Id).ToList();
            var excludedDetectors = detectorRepository.GetDetectorsByIds(excludedDetectorIds);
            foreach (var excludedDetector in excludedDetectors)
            {
                var detectorsToExclude = approach.Detectors.Where(d =>
                    d.LaneNumber == excludedDetector.LaneNumber
                    && d.LaneTypeID == excludedDetector.LaneTypeID
                    && d.MovementTypeID == excludedDetector.MovementTypeID
                    && d.DetectionHardwareID == excludedDetector.DetectionHardwareID
                    && d.DetChannel == excludedDetector.DetChannel
                ).ToList();
                foreach (var detectorToExclude in detectorsToExclude)
                    approach.Detectors.Remove(detectorToExclude);
            }
        }

        protected DataPoint GetDataPointForSum(Bin bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Sum);
            //dataPoint.Label = bin.Start.Month.ToString();
            return dataPoint;
        }

        protected DataPoint GetDataPointForAverage(Bin bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Average);
            return dataPoint;
        }

        protected DataPoint GetDataPointForY2AxisValue(Bin bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Y2Axis);
            
            return dataPoint;
        }

        protected DataPoint GetContainerDataPointForSum(BinsContainer bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.SumValue);
            return dataPoint;
        }

        protected DataPoint GetContainerDataPointForY2Axis(BinsContainer bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Y2AxisValue);
            return dataPoint;
        }

        protected DataPoint GetContainerDataPointForAverage(BinsContainer bin)
        {
            var dataPoint = new DataPoint(bin.Start.ToOADate(), bin.AverageValue);
            return dataPoint;
        }

        private void SetDataPointLabel(BinsContainer bin, DataPoint dataPoint)
        {
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
        }

        protected Series GetTimeAggregateSeries(Series series, List<BinsContainer> binsContainers)
        {
            SetEndTimeAndMinutes(out var endTime, out var minutes);
            SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime, minutes);
            return series;
        }

        protected Series GetTimeAggregateEventCountSeries(Series series, List<BinsContainer> binsContainers)
        {
            SetEndTimeAndMinutes(out var endTime, out var minutes);
            SetDataPointsForTimeAggregationEventCountSeries(binsContainers, series, endTime, minutes);
            return series;
        }

        private void SetEndTimeAndMinutes(out DateTime endTime, out int minutes)
        {
            endTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayEndHour ?? 0, TimeOptions.TimeOfDayEndMinute ?? 0, 0);
            switch (TimeOptions.SelectedBinSize)
            {
                case BinFactoryOptions.BinSize.FifteenMinute:
                    minutes = 15;
                    break;
                case BinFactoryOptions.BinSize.ThirtyMinute:
                    minutes = 30;
                    break;
                case BinFactoryOptions.BinSize.Hour:
                    minutes = 60;
                    break;
                default:
                    throw new InvalidBinSizeException(TimeOptions.SelectedBinSize.ToString() + " is an invalid bin size for time period aggregation");
            }
        }

        private void SetDataPointsForTimeAggregationSeries(List<BinsContainer> binsContainers, Series series,
            DateTime endTime, int minutes)
        {
            for (var startTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                    TimeOptions.TimeOfDayStartHour ?? 0, TimeOptions.TimeOfDayStartMinute ?? 0, 0);
                startTime < endTime;
                startTime = startTime.AddMinutes(minutes))
            {
                if (SelectedAggregationType == AggregationType.Sum)
                {
                    var sumValue = binsContainers.FirstOrDefault().Bins.Where(b =>
                        b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute).Sum(b => b.Sum);
                    series.Points.AddXY(startTime, sumValue);
                }
                else
                {
                    double averageValue = 0;
                    if (binsContainers.FirstOrDefault().Bins.Any(b =>
                        b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute))
                        averageValue = binsContainers.FirstOrDefault().Bins.Where(b =>
                            b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute).Average(b => b.Sum);
                    series.Points.AddXY(startTime, Convert.ToInt32(Math.Round(averageValue)));
                }
            }
        }

        private void SetDataPointsForTimeAggregationEventCountSeries(List<BinsContainer> binsContainers, Series eventCountSeries,
            DateTime endTime, int minutes)
        {
            for (var startTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                    TimeOptions.TimeOfDayStartHour ?? 0, TimeOptions.TimeOfDayStartMinute ?? 0, 0);
                startTime < endTime;
                startTime = startTime.AddMinutes(minutes))
            {
                var sumValue = binsContainers.FirstOrDefault().Bins.Where(b =>
                        b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute).Sum(b => b.Y2Axis);
                    eventCountSeries.Points.AddXY(startTime, sumValue);
            }
        }

        protected Series CreateSeries(int seriesColorNumber, string seriesName)
        {
            var series = new Series();
            series.BorderWidth = SeriesWidth;
            series.Color = GetSeriesColorByNumber(seriesColorNumber);
            series.Name = seriesName;
            series.ChartArea = "ChartArea1";
            series.ChartType = SelectedChartType;
            return series;
        }

        protected Series CreateEventCountSeries()
        {
            var series = new Series();
            series.BorderWidth = SeriesWidth;
            series.Color = GetSeriesColorByNumber(-1);
            series.Name = "Event Count";
            series.ChartArea = "ChartArea1";
            series.ChartType = SeriesChartType.Line;
            series.YAxisType = AxisType.Secondary;
            return series;
        }

        protected Color GetSeriesColorByNumber(int colorNumber)
        {
            while (colorNumber > 10)
                colorNumber = colorNumber - 10;

            switch (colorNumber)
            {
                case -1:
                    return Color.Black;
                case 0:
                    return Color.FromArgb(33, 119, 242);
                case 1:
                    return Color.FromArgb(178, 4, 0);

                case 2:
                    return Color.FromArgb(235, 126, 110);

                case 3:
                    return Color.FromArgb(239, 160, 43);

                case 4:
                    return Color.FromArgb(253, 208, 125);

                case 5:
                    return Color.FromArgb(185, 204, 18);

                case 6:
                    return Color.FromArgb(95, 147, 23);

                case 7:
                    return Color.FromArgb(44, 92, 18);

                case 8:
                    return Color.FromArgb(101, 114, 148);

                case 9:
                    return Color.FromArgb(58, 61, 115);

                case 10:
                    return Color.FromArgb(25, 17, 64);

                default:
                    return Color.Black;
            }
        }


        protected static void PopulateBinsForRoute(List<Models.Signal> signals, List<BinsContainer> binsContainers, AggregationBySignal aggregationBySignal)
        {
            for (var i = 0; i < binsContainers.Count; i++)
            {
                for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = binsContainers[i].Bins[binIndex];
                    bin.Sum += aggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = Convert.ToInt32(Math.Round((double)(bin.Sum / signals.Count)));
                    bin.Y2Axis += aggregationBySignal.BinsContainers[i].Bins[binIndex].Y2Axis;
                }
            }
        }

        protected abstract List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal);
        protected abstract List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals);
    }

    public class InvalidBinSizeException : Exception
    {
        public InvalidBinSizeException()
        {
        }

        public InvalidBinSizeException(string message)
            : base(message)
        {
        }

        public InvalidBinSizeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using System.Runtime.Serialization;
using MOE.Common.Business.Bins;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public abstract class AggregationMetricOptions : MetricOptions
    {
        public enum AggregationType
        {
            Sum,
            Average
        };

        public enum SeriesType
        {
            Signal,
            PhaseNumber,
            Direction,
            Route
        }

        public enum XAxisType
        {
            Time,
            TimeOfDay,
            Direction,
            Phase,
            Signal,
        }

        public enum Dimension
        {
            TwoDimensional,
            ThreeDimensional
        }

        [DataMember]
        public int SeriesWidth { get; set; }
        [DataMember]
        public Business.Bins.BinFactoryOptions TimeOptions { get; set; }
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
        public List<string> SignalIds { get; set; } = new List<string>();
        public List<Models.Signal> Signals { get; set; } = new List<Models.Signal>();
        public List<Models.Approach> Approaches { get; set; } = new List<Models.Approach>();
        public List<Models.Detector> Detectors { get; set; } = new List<Models.Detector>();
        public string ChartTitle
        {
            get
            {
                string chartTitle;
                chartTitle = "AggregationChart\n";
                chartTitle += TimeOptions.Start.ToString();
                if(TimeOptions.End > TimeOptions.Start)
                    chartTitle += " to " + TimeOptions.End.ToString() +"\n";
                if (TimeOptions.DaysOfWeek != null)
                {
                    foreach (var dayOfWeek in TimeOptions.DaysOfWeek)
                    {
                        chartTitle += dayOfWeek.ToString() + " ";
                    }
                }
                if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute != null &&
                    TimeOptions.TimeOfDayEndHour != null && TimeOptions.TimeOfDayEndMinute != null)
                {
                    chartTitle += "Limited to: " + new TimeSpan(0, TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .ToString() + " to " + new TimeSpan(0, TimeOptions.TimeOfDayEndHour.Value,
                        TimeOptions.TimeOfDayEndMinute.Value, 0).ToString() +"\n";
                }
                chartTitle += TimeOptions.SelectedBinSize.ToString() + " bins ";
                chartTitle += SelectedXAxisType.ToString() + " Aggregation ";
                chartTitle += SelectedAggregationType.ToString();
                return chartTitle;
            }
        }

        public override List<string> CreateMetric()
        {
            SetMetricType();
            base.CreateMetric();
            GetSignalObjects();
            if (SelectedXAxisType == XAxisType.TimeOfDay && TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
            {
                TimeOptions.TimeOption = BinFactoryOptions.TimeOptions.TimePeriod;
                TimeOptions.TimeOfDayStartHour = 0;
                TimeOptions.TimeOfDayStartMinute = 0;
                TimeOptions.TimeOfDayEndHour = 23;
                TimeOptions.TimeOfDayEndMinute = 59;
                if (TimeOptions.DaysOfWeek == null)
                {
                    TimeOptions.DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday};
                }
            }
            GetChartByXAxisAggregation();
            return ReturnList;
        }

        private void GetChartByXAxisAggregation()
        {
            switch (SelectedXAxisType)
            {
                case XAxisType.Time:
                    GetTimeCharts();
                    break;
                case XAxisType.TimeOfDay:
                    GetTimeOfDayCharts();
                    break;
                case XAxisType.Phase:
                    GetApproachCharts();
                    break;
                case XAxisType.Direction:
                    GetDirectionCharts();
                    break;
                case XAxisType.Signal:
                    GetSignalCharts();
                    break;
                default:
                    throw new Exception("Invalid X-Axis");
            }
        }

        public void GetSignalObjects()
        {
            if (Signals == null)
            {
                Signals = new List<Models.Signal>();
            }
            if (Signals.Count == 0)
            {
                var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                foreach (string signalId in SignalIds)
                {
                    Signals.AddRange(signalRepository.GetSignalsBetweenDates(signalId, StartDate, EndDate));
                }
            }
        }

        protected void SetMetricType()
        {
            var metricTypeRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            MetricType = metricTypeRepository.GetMetricsByID(MetricTypeID);
        }

        protected void SaveChartImage(Chart chart)
        {
            string chartName = CreateFileName(MetricType.Abbreviation);
            MetricFileLocation = ConfigurationManager.AppSettings["ImageLocation"];
            MetricWebPath = ConfigurationManager.AppSettings["ImageWebLocation"];
            chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
        }

        protected string CreateFileName(string MetricAbbreviation)
        {
            string fileName = MetricAbbreviation +
                              "-" +
                              StartDate.Year.ToString() +
                              StartDate.ToString("MM") +
                              StartDate.ToString("dd") +
                              StartDate.ToString("HH") +
                              StartDate.ToString("mm") +
                              "-" +
                              EndDate.Year.ToString() +
                              EndDate.ToString("MM") +
                              EndDate.ToString("dd") +
                              EndDate.ToString("HH") +
                              EndDate.ToString("mm-");
            Random r = new Random();
            fileName += r.Next().ToString();
            fileName += ".jpg";
            return fileName;
        }

       
        
        private void GetDirectionCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.Direction:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateStringXIntYChart(this);
                        GetDirectionXAxisDirectionSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetDirectionXAxisDirectionSeriesChart(Models.Signal signal, Chart chart)
        {
            var direcitonRepository = Models.Repositories.DirectionTypeRepositoryFactory.Create();
            var directionsList = direcitonRepository.GetAllDirections();
            int columnCounter = 1;
            var colorCount = 1;
            Series series = CreateSeries(0, signal.SignalDescription);
            foreach (var direction in directionsList)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = columnCounter;
                if (SelectedAggregationType == AggregationType.Sum)
                {
                    dataPoint.SetValueY(GetSumByDirection(signal, direction));
                }
                else
                {
                    dataPoint.SetValueY(GetAverageByDirection(signal, direction));
                }
                dataPoint.AxisLabel = direction.Description;
                dataPoint.Color = GetSeriesColorByNumber(colorCount);
                series.Points.Add(dataPoint);
                colorCount++;
                columnCounter++;
            }
            chart.Series.Add(series);
        }


        private void GetApproachCharts()
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
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetTimeCharts()
        {
            Chart chart;
                switch (SelectedSeries)
                {
                    case SeriesType.PhaseNumber:
                        foreach (var signal in Signals)
                        {
                            chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal>{signal});
                            GetTimeXAxisApproachSeriesChart(signal, chart);
                            SaveChartImage(chart);
                        }
                    break;
                    case SeriesType.Direction:
                        foreach (var signal in Signals)
                        {
                            chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> { signal });
                            GetTimeXAxisDirectionSeriesChart(signal, chart);
                            SaveChartImage(chart);
                        };
                    break;
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

        private void GetSignalCharts()
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

        private void GetSignalsXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            int signalXAxisNumber = 1;
            string seriesName = "Signals";
            Series series = CreateSeries(0, seriesName);
            foreach (var signal in signals)
            {
                List<BinsContainer> binsContainers = GetBinsContainersBySignal(signal);
                DataPoint dataPoint = new DataPoint();
                dataPoint.SetValueY(SelectedAggregationType == AggregationType.Sum
                    ? binsContainers.Sum(b => b.SumValue)
                    : Convert.ToInt32(Math.Round(binsContainers.Sum(b => b.SumValue) / (double)signals.Count)));
                dataPoint.AxisLabel = signal.SignalDescription;
                series.Points.Add(dataPoint);
                signalXAxisNumber++;
            }
            chart.Series.Add(series);
        }

        private void GetSignalsXAxisDirectionSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            List<DirectionType> availableDirections = new List<DirectionType>();
            foreach (var signal in signals)
            {
                availableDirections.AddRange(signal.GetAvailableDirections());
            }
            availableDirections = availableDirections.Distinct().ToList();
            int colorCode = 1;
            foreach (var directionType in availableDirections)
            {
                string seriesName = directionType.Description;
                Series series = CreateSeries(colorCode, seriesName);
                foreach (var signal in signals)
                {
                    List<BinsContainer> binsContainers = GetBinsContainersByDirection(directionType, signal);
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.SetValueY(SelectedAggregationType == AggregationType.Sum
                        ? binsContainers.Sum(b => b.SumValue)
                        : Convert.ToInt32(Math.Round(binsContainers.Sum(b => b.SumValue)/(double) availableDirections.Count)));
                    dataPoint.AxisLabel = signal.SignalDescription;
                    series.Points.Add(dataPoint);
                }
                colorCode++;
                chart.Series.Add(series);
            }
        }

        private void GetTimeXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
        {

            Series series = CreateSeries(0, "Route");
            List<BinsContainer> binsContainers =  GetBinsContainersByRoute(signals);
            foreach (var binsContainer in binsContainers)
            {
                foreach (var bin in binsContainer.Bins)
                {
                    series.Points.Add(SelectedAggregationType == AggregationType.Sum ? GetDataPointForSum(bin) : GetDataPointForAverage(bin));
                }
            }
            chart.Series.Add(series);

        }

        private void GetTimeXAxisDirectionSeriesChart(Models.Signal signal, Chart chart)
        {
            int i = 1;
            foreach (var directionType in signal.GetAvailableDirections())
            {
                GetDirectionSeries(chart, i, directionType, signal);
                i++;
            }
        }

        private void GetDirectionSeries(Chart chart, int colorCode, DirectionType directionType, Models.Signal signal)
        {
            Series series = CreateSeries(colorCode, directionType.Description);
            List<BinsContainer> binsContainers = GetBinsContainersByDirection(directionType, signal);
            foreach (var binsContainer in binsContainers)
            {
                foreach (var bin in binsContainer.Bins)
                {
                    DataPoint dataPoint = SelectedAggregationType == AggregationType.Sum
                            ? GetDataPointForSum(bin)
                            : GetDataPointForAverage(bin);
                            series.Points.Add(dataPoint);
                }
            }
            chart.Series.Add(series);
        }



        private void GetTimeOfDayCharts()
        {
            Chart chart;
            switch (SelectedSeries)
            {
                case SeriesType.PhaseNumber:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> { signal });
                        GetTimeOfDayXAxisApproachSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    }
                    break;
                case SeriesType.Direction:
                    foreach (var signal in Signals)
                    {
                        chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal> { signal });
                        GetTimeOfDayXAxisDirectionSeriesChart(signal, chart);
                        SaveChartImage(chart);
                    };
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
                default:
                    throw new Exception("Invalid X-Axis Series Combination");
            }
        }

        private void GetTimeOfDayXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            List<BinsContainer> binsContainers = GetBinsContainersByRoute(signals);
            Series series = CreateSeries(0, "Route");
            SetTimeAggregateSeries(chart, series, binsContainers);
        }

        private void GetTimeOfDayXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            int colorCode = 1;
            foreach (var signal in signals)
            {
                List<BinsContainer> binsContainers = GetBinsContainersBySignal(signal);
                Series series = CreateSeries(colorCode, signal.SignalDescription);
                SetTimeAggregateSeries(chart, series, binsContainers);
                colorCode++;
            }
        }

        private void GetTimeOfDayXAxisDirectionSeriesChart(Models.Signal signal, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            int colorCode = 1;
            foreach (var direction in signal.GetAvailableDirections())
            {
                List<BinsContainer> binsContainers = GetBinsContainersByDirection(direction, signal);
                Series series = CreateSeries(colorCode, direction.Description);
                SetTimeAggregateSeries(chart, series, binsContainers);
                colorCode++;
            }
        }

        private void SetTimeOfDayXAxisMinimum(Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
            {
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
            }
        }

        private void GetTimeOfDayXAxisApproachSeriesChart(Models.Signal signal, Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
            {
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
            }
            int colorCode = 1;
            foreach (var approach in signal.Approaches)
            {
                string phaseDescription = GetPhaseDescription(approach, true);
                List<BinsContainer> binsContainers = GetBinsContainersByApproach(approach, true);
                Series series = CreateSeries(colorCode, approach.Description + phaseDescription);
                SetTimeAggregateSeries(chart, series, binsContainers);
                colorCode++;
                if (approach.PermissivePhaseNumber != null)
                {
                    string permissivePhaseDescription = GetPhaseDescription(approach, true);
                    List<BinsContainer> permissiveBinsContainers = GetBinsContainersByApproach(approach, true);
                    Series permissiveSeries = CreateSeries(colorCode, approach.Description + permissivePhaseDescription);
                    SetTimeAggregateSeries(chart, permissiveSeries, permissiveBinsContainers);
                    colorCode++;
                }
            }
        }


        protected void GetSignalsXAxisPhaseNumberSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            List<int> availablePhaseNumbers = new List<int>();
            foreach (var signal in signals)
            {
                availablePhaseNumbers.AddRange(signal.GetPhasesForSignal());
            }
            availablePhaseNumbers = availablePhaseNumbers.Distinct().ToList();
            int colorCode = 1;
            foreach (var phaseNumber in availablePhaseNumbers)
            {
                string seriesName = "Phase " + phaseNumber;
                Series series = CreateSeries(colorCode, seriesName);
                foreach (var signal in signals)
                {
                    List<BinsContainer> binsContainers = GetBinsContainersByPhaseNumber(signal, phaseNumber);
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.SetValueY(SelectedAggregationType == AggregationType.Sum
                        ? binsContainers.Sum(b => b.SumValue)
                        : binsContainers.Average(b => b.SumValue));
                    dataPoint.AxisLabel = signal.SignalDescription;
                    series.Points.Add(dataPoint);
                }
                colorCode++;
                chart.Series.Add(series);
            }
        }



        protected void GetApproachXAxisChart(Models.Signal signal, Chart chart)
        {
            Series series = CreateSeries(0, signal.SignalDescription);
            int i = 1;
            foreach (var approach in signal.Approaches)
            {
                List<BinsContainer> binsContainers = GetBinsContainersByApproach(approach, true);
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                dataPoint.Color = GetSeriesColorByNumber(i);
                if (SelectedAggregationType == AggregationType.Sum)
                {
                    dataPoint.SetValueY(binsContainers.FirstOrDefault().SumValue);
                }
                else
                {
                    dataPoint.SetValueY(binsContainers.FirstOrDefault().AverageValue);
                }
                dataPoint.AxisLabel = approach.Description;
                series.Points.Add(dataPoint);
                i++;
                if (approach.PermissivePhaseNumber != null)
                {
                    List<BinsContainer> binsContainers2 = GetBinsContainersByApproach(approach, false);
                    DataPoint dataPoint2 = new DataPoint();
                    dataPoint2.XValue = i;
                    dataPoint2.Color = GetSeriesColorByNumber(i);
                    if (SelectedAggregationType == AggregationType.Sum)
                    {
                        dataPoint2.SetValueY(binsContainers2.FirstOrDefault().SumValue);
                    }
                    else
                    {
                        dataPoint2.SetValueY(binsContainers2.FirstOrDefault().AverageValue);
                    }
                    dataPoint2.AxisLabel = approach.Description;
                    series.Points.Add(dataPoint2);
                    i++;
                }
            }
            chart.Series.Add(series);
        }

        protected void GetPhaseXAxisChart(Models.Signal signal, Chart chart)
        {
            Series series = CreateSeries(0, signal.SignalDescription);
            chart.Series.Add(series);
            int i = 1;
            List<int> phaseNumbers = signal.GetPhasesForSignal();
            foreach (var phaseNumber in phaseNumbers)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                if (SelectedAggregationType == AggregationType.Sum)
                {
                    dataPoint.SetValueY(GetSumByPhaseNumber(signal, phaseNumber));
                }
                else
                {
                    dataPoint.SetValueY(GetAverageByPhaseNumber(signal, phaseNumber));
                }
                dataPoint.AxisLabel = "Phase " + phaseNumber;
                dataPoint.Color = GetSeriesColorByNumber(i);
                series.Points.Add(dataPoint);
                i++;
            }
        }
        
        protected void GetTimeXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            int i = 1;
            foreach (var signal in signals)
            {
                Series series = CreateSeries(i, signal.SignalDescription);
                List<BinsContainer> binsContainers = GetBinsContainersBySignal(signal);
                BinsContainer container = binsContainers.FirstOrDefault();
                if (container != null)
                {
                    foreach (var bin in container.Bins)
                    {
                        DataPoint dataPoint = GetDataPointForSum(bin);
                        series.Points.Add(dataPoint);
                    }
                    chart.Series.Add(series);
                    i++;
                }
            }
        }

        private DataPoint GetDataPointForSum(Bin bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Sum);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = bin.Start.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        private DataPoint GetDataPointForAverage(Bin bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Average);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        private DataPoint GetContainerDataPointForSum(BinsContainer bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.SumValue);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        private DataPoint GetContainerDataPointForAverage(BinsContainer bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.AverageValue);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }


        protected void GetTimeXAxisApproachSeriesChart(Models.Signal signal, Chart chart)
        {
            int i = 1;
            foreach (var approach in signal.Approaches)
            {
                GetApproachTimeSeriesByProtectedPermissive(chart, i, approach, true);
                 i++;
                if (approach.PermissivePhaseNumber != null)
                {
                    GetApproachTimeSeriesByProtectedPermissive(chart, i, approach, false);
                    i++;
                }
            }
        }


        private void SetTimeAggregateSeries(Chart chart, Series series, List<BinsContainer> binsContainers)
        {
            DateTime endTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                TimeOptions.TimeOfDayEndHour ?? 0, TimeOptions.TimeOfDayEndMinute ?? 0, 0);

            int minutes;
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
                    throw new Exception("Invalid bin size for time aggregation");
            }
            SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime, minutes);
            chart.Series.Add(series);
        }

        private static string GetPhaseDescription(Approach approach, bool getProtectedPhase)
        {
            return getProtectedPhase ? " Phase " + approach.ProtectedPhaseNumber : " Phase " + approach.PermissivePhaseNumber;
        }

        private void SetDataPointsForTimeAggregationSeries(List<BinsContainer> binsContainers, Series series, DateTime endTime, int minutes)
        {
            for (DateTime startTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, TimeOptions.TimeOfDayStartHour ?? 0, TimeOptions.TimeOfDayStartMinute ?? 0, 0);
                                                startTime < endTime;
                                                startTime = startTime.AddMinutes(minutes))
            {
                if (SelectedAggregationType == AggregationType.Sum)
                {
                    int sumValue = binsContainers.FirstOrDefault().Bins.Where(b =>
                        b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute).Sum(b => b.Sum);
                    series.Points.AddXY(startTime, sumValue);
                }
                else
                {
                    double averageValue = 0;
                    if (binsContainers.FirstOrDefault().Bins.Any(b =>
                        b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute))
                    {
                        averageValue = binsContainers.FirstOrDefault().Bins.Where(b =>
                            b.Start.Hour == startTime.Hour && b.Start.Minute == startTime.Minute).Average(b => b.Sum);
                    }
                    series.Points.AddXY(startTime, Convert.ToInt32(Math.Round(averageValue)));
                }
            }
        }

        
        private void GetApproachTimeSeriesByProtectedPermissive(Chart chart, int i, Approach approach, bool getProtectedPhase)
        {
            string phaseDescription = GetPhaseDescription(approach, getProtectedPhase);
            List<BinsContainer> binsContainers = GetBinsContainersByApproach(approach, getProtectedPhase);
            Series series = CreateSeries(i, approach.Description + phaseDescription);
            if ((TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Month || TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Year) &&
                TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
            {
                foreach (var binsContainer in binsContainers)
                {
                    var dataPoint = SelectedAggregationType == AggregationType.Sum ? GetContainerDataPointForSum(binsContainer) : GetContainerDataPointForAverage(binsContainer);
                    series.Points.Add(dataPoint);
                }
            }
            else
            {
                foreach (var bin in binsContainers.FirstOrDefault()?.Bins)
                {
                    var dataPoint = SelectedAggregationType == AggregationType.Sum ? GetDataPointForSum(bin) : GetDataPointForAverage(bin);
                    series.Points.Add(dataPoint);
                }
            }
            chart.Series.Add(series);
        }

        private Series CreateSeries(int seriesColorNumber, string seriesName)
        {
            Series series = new Series(); series.BorderWidth = SeriesWidth;
            series.Color = GetSeriesColorByNumber(seriesColorNumber);
            series.Name = seriesName;
            series.ChartArea = "ChartArea1";
            series.ChartType = SelectedChartType;
            return series;
        }

        protected Color GetSeriesColorByNumber(int colorNumber)
        {
            while (colorNumber > 10)
            {
                colorNumber = colorNumber - 10;
            }

            switch (colorNumber)
            {
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
        

        protected abstract List<BinsContainer> GetBinsContainersByApproach(Approach approach, bool getprotectedPhase);
        protected abstract int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetAverageByDirection(Models.Signal signal, DirectionType direction);
        protected abstract int GetSumByDirection(Models.Signal signal, DirectionType direction);
        protected abstract List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal);
        protected abstract List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType, Models.Signal signal);
        protected abstract List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals);
        protected abstract List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber);

    }

}
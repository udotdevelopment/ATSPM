using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using NuGet;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public abstract class AggregationMetricOptions : MetricOptions
    {
        
        //public enum ChartTypes
        //{
        //    Column,
        //    StackedColumn,
        //    Line,
        //    StackedLine,
        //    Pie
        //};

        public enum AggregationOperations
        {
            Sum,
            Average
        };

        public enum SeriesType
        {
            Signal,
            Approach,
            Direction,
            Route
        }

        public enum XAxisAggregationSeriesOptions
        {
            Time,
            TimeOfDay,
            Direction,
            Approach,
            Signal,
            //SignalByDirection,
            //Route,
            //SignalByPhase,
            //RouteBySignal
        }
        [DataMember]
        public int SeriesWidth { get; set; }
        [DataMember]
        public Business.Bins.BinFactoryOptions TimeOptions { get; set; }
        [DataMember]
        public SeriesChartType ChartType { get; set; }
        [DataMember]
        public AggregationOperations AggregationOperation { get; set; }
        [DataMember]
        public XAxisAggregationSeriesOptions XAxisAggregationSeriesOption { get; set; }
        [DataMember]
        public SeriesType SelectedSeries { get; set; }
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
                chartTitle += XAxisAggregationSeriesOption.ToString() + " Aggregation ";
                chartTitle += AggregationOperation.ToString();
                return chartTitle;
            }
        }

        public override List<string> CreateMetric()
        {
            SetMetricType();
            base.CreateMetric();
            GetSignalObjects();
            if (XAxisAggregationSeriesOption == XAxisAggregationSeriesOptions.TimeOfDay && TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
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

        private void GetChartByXAxisAggregation()
        {
            switch (XAxisAggregationSeriesOption)
            {
                case XAxisAggregationSeriesOptions.Time:
                    GetTimeCharts();
                    break;
                case XAxisAggregationSeriesOptions.TimeOfDay:
                    GetTimeOfDayCharts();
                    break;
                case XAxisAggregationSeriesOptions.Approach:
                    GetApproachCharts();
                    break;
                case XAxisAggregationSeriesOptions.Direction:
                    GetDirectionCharts();
                    break;
                //case XAxisAggregationSeriesOptions.Route:
                //    GetRouteCharts();
                //    break;
                //case XAxisAggregationSeriesOptions.RouteBySignal:
                //    GetRouteBySignalCharts();
                //    break;
                case XAxisAggregationSeriesOptions.Signal:
                    GetSignalCharts();
                    break;
                //case XAxisAggregationSeriesOptions.SignalByDirection:
                //    GetSignalByDirectionCharts();
                //    break;
                //case XAxisAggregationSeriesOptions.SignalByPhase:
                //    GetSignalByPhaseCharts();
                //    break;
                default:
                    GetTimeCharts();
                    break;
            }
        }


        private void GetSignalByPhaseCharts()
        {
            foreach (var signal in Signals)
            {
                Chart chart = ChartFactory.CreateStringXIntYChart(this);
                GetPhaseXAxisChart(signal, chart);
                SaveChartImage(chart);
            }
        }


        private void GetSignalCharts()
        {
            Chart chart = ChartFactory.CreateStringXIntYChart(this);
            GetSignalsXAxisSeriesChart(Signals, chart);
            SaveChartImage(chart);
        }

        private void GetSignalByDirectionCharts()
        {
            foreach (var signal in Signals)
            {
                Chart chart = ChartFactory.CreateStringXIntYChart(this);
                GetSignalByDirectionAggregateChart(signal, chart);
                SaveChartImage(chart);
            }
        }


        private  void GetRouteCharts()
        {
            Chart chart = ChartFactory.CreateTimeXIntYChart(this, null);
            GetTimeXAxisRouteChart(Signals, chart);
            SaveChartImage(chart);
        }


        private void GetRouteBySignalCharts()
        {
            Chart chart = ChartFactory.CreateTimeXIntYChart(this,null);
            GetTimeXAxisSignalSeriesChart(Signals, chart);
            SaveChartImage(chart);
        }


        private void GetDirectionCharts()
        {
            Chart chart;
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateStringXIntYChart(this);
                GetDirectionXAxisChart(signal, chart);
                SaveChartImage(chart);
            }
        }

        private void GetDirectionXAxisChart(Models.Signal signal, Chart chart)
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
                if (AggregationOperation == AggregationOperations.Sum)
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
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateStringXIntYChart(this);
                GetApproachXAxisChart(signal, chart);
                SaveChartImage(chart);
            }

        }

        private void GetTimeCharts()
        {
            Chart chart;
                switch (SelectedSeries)
                {
                    case SeriesType.Approach:
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
                }
            
        }

        private void GetTimeXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            List<BinsContainer> binsContainers =  SetSumBinsContainersByRoute(signals, binsContainers);
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
            //    if (TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
            //    {
            //        DataPoint dataPoint = AggregationOperation == AggregationOperations.Sum
            //                ? GetContainerDataPointForSum(binsContainer)
            //                : GetContainerDataPointForAverage(binsContainer);
            //            series.Points.Add(dataPoint);
            //}
            //    else
            //    {
                    foreach (var bin in binsContainer.Bins)
                    {
                        DataPoint dataPoint = AggregationOperation == AggregationOperations.Sum
                                ? GetDataPointForSum(bin)
                                : GetDataPointForAverage(bin);
                                series.Points.Add(dataPoint);
                    }
                //}
            }
            chart.Series.Add(series);
        }

        protected abstract List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType, Models.Signal signal);


        private void GetTimeOfDayCharts()
        {
            Chart chart;
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateTimeXIntYChart(this, new List<Models.Signal>{signal});

                if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
                {
                    chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                                TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                            .AddHours(-1).ToOADate();
                }
                GetTimeOfDayAggregateXAxisApproachSeriesChart(signal, chart);
                SaveChartImage(chart);
            }
        }
        

        protected void GetSignalsXAxisSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            int i = 1;
            string seriesName = string.Empty;
            foreach (var signal in signals)
            {
                seriesName += signal.SignalDescription + " ";
            }
            Series series = CreateSeries(0, seriesName);
            foreach (var signal in signals)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.Color = GetSeriesColorByNumber(i);
                dataPoint.XValue = i;
                if (AggregationOperation == AggregationOperations.Sum)
                {
                    dataPoint.SetValueY(GetSignalSumDataPoint(signal));
                }
                else
                {
                    dataPoint.SetValueY(GetSignalAverageDataPoint(signal));
                }
                dataPoint.AxisLabel = signal.SignalID;
                series.Points.Add(dataPoint);
                i++;
            }
            chart.Series.Add(series);
        }


        protected void GetApproachXAxisChart(Models.Signal signal, Chart chart)
        {
            Series series = CreateSeries(0, signal.SignalDescription);
            int i = 1;
            foreach (var approach in signal.Approaches)
            {
                List<BinsContainer> binsContainers = SetBinsContainersByApproach(approach, true);
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                dataPoint.Color = GetSeriesColorByNumber(i);
                if (AggregationOperation == AggregationOperations.Sum)
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
                    List<BinsContainer> binsContainers2 = SetBinsContainersByApproach(approach, false);
                    DataPoint dataPoint2 = new DataPoint();
                    dataPoint2.XValue = i;
                    dataPoint2.Color = GetSeriesColorByNumber(i);
                    if (AggregationOperation == AggregationOperations.Sum)
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
                if (AggregationOperation == AggregationOperations.Sum)
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


        private void GetTimeXAxisRouteChart(List<Models.Signal> signals, Chart chart)
        {
            List<BinsContainer> binsContainers = BinFactory.GetBins(TimeOptions);
            SetSumBinsContainersByRoute(signals, binsContainers);
            if (
                TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod &&
                new List<BinFactoryOptions.BinSize>{ BinFactoryOptions.BinSize.Month, BinFactoryOptions.BinSize.Year}.Contains(TimeOptions.SelectedBinSize))
            {
                Series series = CreateSeries(0, binsContainers.FirstOrDefault().Start.ToShortDateString() + "-" +
                                                binsContainers.FirstOrDefault().End.ToShortDateString());
                foreach (var binContainer in binsContainers)
                {
                    if (AggregationOperation == AggregationOperations.Sum)
                    {
                        series.Points.Add(GetContainerDataPointForSum(binContainer));
                    }
                    else
                    {
                        series.Points.Add(GetContainerDataPointForAverage(binContainer));
                    }
                }
                chart.Series.Add(series);
            }
            else
            {
                for (var index = 0; index < binsContainers.Count; index++)
                {
                    var binsContainer = binsContainers[index];
                    Series series = CreateSeries(index,
                        binsContainer.Start.ToShortDateString() + "-" + binsContainer.End.ToShortDateString()); 
                    foreach (var bin in binsContainer.Bins)
                    {
                        series.Points.Add(AggregationOperation == AggregationOperations.Sum? GetDataPointForSum(bin): GetDataPointForAverage(bin));
                    }
                    chart.Series.Add(series);
                }
            }
        }

        protected abstract void SetSumBinsContainersByRoute(List<Models.Signal> signals, List<BinsContainer> binsContainers);

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

        private void GetTimeOfDayAggregateXAxisApproachSeriesChart(Models.Signal signal, Chart chart)
        {
            int i = 1;
            foreach (var approach in signal.Approaches)
            {
                GetApproachTimeAggregateSeriesByProtectedPermissive(chart, i, approach, true);
                i++;
                if (approach.PermissivePhaseNumber != null)
                {
                    GetApproachTimeAggregateSeriesByProtectedPermissive(chart, i, approach, false);
                    i++;
                }
            }
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


        private void GetApproachTimeAggregateSeriesByProtectedPermissive(Chart chart, int seriesColor, Approach approach, bool getProtectedPhase)
        {
            string phaseDescription = GetPhaseDescription(approach, getProtectedPhase);
            List<BinsContainer> binsContainers = SetBinsContainersByApproach(approach, getProtectedPhase);
            Series series = CreateSeries(seriesColor, approach.Description + phaseDescription);
            DateTime endTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                TimeOptions.TimeOfDayEndHour ?? 0, TimeOptions.TimeOfDayEndMinute ?? 0, 0);

            switch (TimeOptions.SelectedBinSize)
            {
                case BinFactoryOptions.BinSize.FifteenMinute:
                    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime);
                    break;
                case BinFactoryOptions.BinSize.ThirtyMinute:
                    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime);
                    break;
                case BinFactoryOptions.BinSize.Hour:
                    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime);
                    break;
                //case BinFactoryOptions.BinSize.Day:
                //    return GetDayBinsContainersForRange(timeOptions);
                //    break;
                //case BinFactoryOptions.BinSize.Week:
                //    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime, 60 * 24 * 7);
                //    break;
                //case BinFactoryOptions.BinSize.Month:
                //    return GetMonthBinsForRange(timeOptions);
                //    break;
                //case BinFactoryOptions.BinSize.Year:
                //    return GetYearBinsForRange(timeOptions);
                //    break;
                default:
                    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime);
                    break;
            }
            switch (TimeOptions.SelectedBinSize)
            {
                case BinFactoryOptions.BinSize.FifteenMinute:
                    SetDataPointsForTimeAggregationSeries(binsContainers, series, endTime);
                    break;
            }
            chart.Series.Add(series);
        }

        private static string GetPhaseDescription(Approach approach, bool getProtectedPhase)
        {
            return getProtectedPhase ? " Phase " + approach.ProtectedPhaseNumber : " Phase " + approach.PermissivePhaseNumber;
        }

        private void SetDataPointsForTimeAggregationSeries(List<BinsContainer> binsContainers, Series series, DateTime endTime)
        {
            for (DateTime startTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, TimeOptions.TimeOfDayStartHour ?? 0, TimeOptions.TimeOfDayStartMinute ?? 0, 0);
                                                startTime < endTime;
                                                startTime = startTime.AddMinutes(15))
            {
                if (AggregationOperation == AggregationOperations.Sum)
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
            List<BinsContainer> binsContainers = SetBinsContainersByApproach(approach, getProtectedPhase);
            Series series = CreateSeries(i, approach.Description + phaseDescription);
            if ((TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Month || TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Year) &&
                TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
            {
                foreach (var binsContainer in binsContainers)
                {
                    var dataPoint = AggregationOperation == AggregationOperations.Sum ? GetContainerDataPointForSum(binsContainer) : GetContainerDataPointForAverage(binsContainer);
                    series.Points.Add(dataPoint);
                }
            }
            else
            {
                foreach (var bin in binsContainers.FirstOrDefault()?.Bins)
                {
                    var dataPoint = AggregationOperation == AggregationOperations.Sum ? GetDataPointForSum(bin) : GetDataPointForAverage(bin);
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
            series.ChartType = ChartType;
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

        //protected void SetSeriestype(Series series)
        //{
        //    switch (ChartType)
        //    {
        //        case ChartTypes.StackedColumn:
        //            series.ChartType = SeriesChartType.StackedColumn;
        //            series.BorderWidth = 2;
        //            break;
        //        case ChartTypes.Line:
        //            series.ChartType = SeriesChartType.Line;
        //            series.BorderWidth = 3;
        //            break;
        //        case ChartTypes.Column:
        //            series.ChartType = SeriesChartType.Column;
        //            series.BorderWidth = 2;
        //            break;
        //        case ChartTypes.StackedLine:
        //            series.ChartType = SeriesChartType.StackedArea;
        //            series.BorderWidth = 2;
        //            break;
        //        case ChartTypes.Pie:
        //            series.ChartType = SeriesChartType.Pie;
        //            series.BorderWidth = 2;
        //            break;
        //    }
        //}

        protected abstract List<BinsContainer> SetBinsContainersByApproach(Approach approach, bool getprotectedPhase);
        protected abstract int GetSignalAverageDataPoint(Models.Signal signal);
        public abstract int GetSignalSumDataPoint(Models.Signal signal);
        protected abstract int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetAverageByDirection(Models.Signal signal, DirectionType direction);
        protected abstract int GetSumByDirection(Models.Signal signal, DirectionType direction);
        protected abstract List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal);

        protected void GetSignalByDirectionAggregateChart(Models.Signal signal, Chart chart)
        {
            int columnCounter = 1;
            var colorCount = 1;
            Series series = CreateSeries(0, signal.SignalDescription);
            chart.Series.Add(series);
            List<DirectionType> distinctDirectionTypesAvailable = signal.GetAvailableDirections();
            foreach (var direction in distinctDirectionTypesAvailable)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = columnCounter;
                dataPoint.SetValueY(AggregationOperation == AggregationOperations.Sum? GetSumByDirection(signal, direction)
                    : GetAverageByDirection(signal, direction));
                dataPoint.Color = GetSeriesColorByNumber(colorCount);
                dataPoint.AxisLabel = direction.Description;
                series.Points.Add(dataPoint);
                columnCounter++;
                colorCount++;
            }
        }
            

        private List<DirectionType> GetDistinctDirectionTypesAvailable(List<Models.Signal> signals)
        {
            List<DirectionType> directionTypes = new List<DirectionType>();
            foreach (var signal in signals)
            {
                directionTypes.AddRange(signal.GetAvailableDirections());
            }
            return directionTypes.Distinct().ToList();
        }
    }

}
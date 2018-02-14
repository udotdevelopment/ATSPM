using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
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

    [DataContract]
    public abstract class SignalAggregationMetricOptions : MetricOptions
    {
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
        public List<FilterExtensions.FilterSignal> FilterSignals { get; set; } = new List<FilterSignal>();
        public List<Models.Signal> Signals { get; set; } = new List<Models.Signal>();
        public virtual string ChartTitle
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

        virtual protected void GetChartByXAxisAggregation()
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

        virtual protected void GetSignalObjects()
        {
            try
            {

            if (Signals == null)
            {
                Signals = new List<Models.Signal>();
            }
            if (Signals.Count == 0)
            {
                var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                foreach (FilterSignal filterSignal in FilterSignals)
                {
                    if (!filterSignal.Exclude)
                    {
                        var signals = signalRepository.GetSignalsBetweenDates(filterSignal.SignalId, StartDate, EndDate);
                        foreach (var signal in signals)
                        {
                            signal.Approaches = signal.Approaches.OrderBy(a => a.ProtectedPhaseNumber).ToList();
                        }
                        Signals.AddRange(signals);
                    }
                }
            }

            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("Unable to apply signal filter");
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

       

        virtual protected void GetTimeCharts()
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

        virtual protected void GetSignalCharts()
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


        protected void GetTimeXAxisRouteSeriesChart(List<Models.Signal> signals, Chart chart)
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
        

        virtual protected void GetTimeOfDayCharts()
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
            List<BinsContainer> binsContainers = GetBinsContainersByRoute(signals);
            Series series = CreateSeries(0, "Route");
            SetTimeAggregateSeries(chart, series, binsContainers);
        }

        protected void GetTimeOfDayXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
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

        protected void SetTimeOfDayXAxisMinimum(Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
            {
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
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

        protected DataPoint GetDataPointForSum(Bin bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Sum);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = bin.Start.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        protected DataPoint GetDataPointForAverage(Bin bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.Average);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        protected DataPoint GetContainerDataPointForSum(BinsContainer bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.SumValue);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }

        protected DataPoint GetContainerDataPointForAverage(BinsContainer bin)
        {
            DataPoint dataPoint = new DataPoint(bin.Start.ToOADate(), bin.AverageValue);
            if (bin.Start.Hour == 0 && bin.Start.Minute == 0)
                dataPoint.AxisLabel = StartDate.ToString("MM/dd/yyyy HH:mm");
            return dataPoint;
        }


        protected void SetTimeAggregateSeries(Chart chart, Series series, List<BinsContainer> binsContainers)
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

        protected Series CreateSeries(int seriesColorNumber, string seriesName)
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
        

       
        protected abstract List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal);
        protected abstract List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals);
        public abstract string YAxisTitle { get; }


    }

}
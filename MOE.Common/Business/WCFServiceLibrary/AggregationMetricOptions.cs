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
using System.IO;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using NuGet;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public abstract class AggregationMetricOptions : MetricOptions
    {
        
        public enum ChartTypes
        {
            Column,
            StackedColumn,
            Line,
            StackedLine,
            Pie
        };

        public enum AggregationOperations
        {
            Sum,
            Average
        };

        public enum XAxisAggregationSeriesOptions
        {
            Time,
            Direction,
            Approach,
            Signal,
            SignalByDirection,
            Route,
            SignalByPhase,
            RouteBySignal
        }

        //wouldn't be used because the AggMetrics are incorporated into [MetricTypes]
        public enum AggregationMetrics
        {
            LaneByLaneCounts,
            AdvancedCounts,
            ArrivalonGreen,
            PlatoonRatio,
            SplitFail,
            PedestrianActuations,
            Preemption,
            TSP,
            DataQuality
        }
        public enum AggregationGroups
        {
            Hour,
            Day,
            Month,
            Year,
            None,
            Signal
        }

        public AggregationMetricOptions()
        {
        }

        public Business.Bins.BinFactoryOptions TimeOptions { get; set; }
        public ChartTypes ChartType { get; set; }
        public AggregationOperations AggregationOperation { get; set; }
        public XAxisAggregationSeriesOptions XAxisAggregationSeriesOption { get; set; }
        public List<string> SignalIds { get; set; } = new List<string>();
        public List<Models.Signal> Signals { get; set; } = new List<Models.Signal>();
        public List<Models.Approach> Approaches { get; set; } = new List<Models.Approach>();
        public List<Models.Detector> Detectors { get; set; } = new List<Models.Detector>();
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        public string ChartTitle
        {
            get
            {
                string chartTitle;
                chartTitle = "AggregationChart\n";
                foreach (var signal in Signals)
                {
                    chartTitle += signal.SignalDescription + " ";
                }
                chartTitle += "\n";
                chartTitle += TimeOptions.Start.ToShortDateString() + " to " + TimeOptions.End.ToShortDateString() +"\n";
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
                    chartTitle += new TimeSpan(0, TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .ToString() + " to " + new TimeSpan(0, TimeOptions.TimeOfDayEndHour.Value,
                        TimeOptions.TimeOfDayEndMinute.Value, 0).ToString() +"\n";
                }
                chartTitle += TimeOptions.BinSize.ToString() + " bins ";
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
            SetBins();
            GetChartByXAxisAggregation();
            return ReturnList;
        }

        private void SetBins()
        {
            BinsContainers = BinFactory.GetBins(TimeOptions);
        }

        public void GetSignalObjects()
        {
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
                case XAxisAggregationSeriesOptions.Approach:
                    GetApproachCharts();
                    break;
                case XAxisAggregationSeriesOptions.Direction:
                    GetDirectionCharts();
                    break;
                case XAxisAggregationSeriesOptions.Route:
                    GetRouteCharts();
                    break;
                case XAxisAggregationSeriesOptions.RouteBySignal:
                    GetRouteBySignalCharts();
                    break;
                case XAxisAggregationSeriesOptions.Signal:
                    GetSignalCharts();
                    break;
                case XAxisAggregationSeriesOptions.SignalByDirection:
                    GetSignalByDirectionCharts();
                    break;
                case XAxisAggregationSeriesOptions.SignalByPhase:
                    GetSignalByPhaseCharts();
                    break;
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
            Chart chart = ChartFactory.CreateStringXIntYChart(this);
            GetSignalByDirectionAggregateChart(Signals, chart);
            SaveChartImage(chart);
        }


        private  void GetRouteCharts()
        {
            Chart chart = ChartFactory.CreateTimeXIntYChart(this);
            GetTimeXAxisRouteChart(Signals, chart);
            SaveChartImage(chart);
        }


        private void GetRouteBySignalCharts()
        {
            Chart chart = ChartFactory.CreateTimeXIntYChart(this);
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
            Series series = new Series();
                series.Name = signal.SignalDescription;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
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
            foreach (var signal in Signals)
            {
                chart = ChartFactory.CreateTimeXIntYChart(this);
                GetTimeXAxisApproachSeriesChart(signal, chart);
                SaveChartImage(chart);
            }
        }
        

        protected void GetSignalsXAxisSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            int i = 1;
            Series series = new Series();
            foreach (var signal in signals)
            {
                series.Name += signal.SignalDescription + " ";
            }
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
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
            Series series = new Series();
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
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
            Series series = new Series();
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
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
            SetSumBinsContainersByRoute(signals);
            foreach (var binsContainer in BinsContainers)
            {
                Series series = new Series();
                series.Name = binsContainer.Start.ToShortDateString() +"-"+ binsContainer.End.ToShortDateString();
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                foreach (var bin in binsContainer.Bins)
                {
                    if (AggregationOperation == AggregationOperations.Sum)
                    {
                        series.Points.AddXY(bin.Start, bin.Sum);
                    }
                    else
                    {
                        series.Points.AddXY(bin.Start, bin.Average);
                    }
                }
                chart.Series.Add(series);
            }
        }

        protected abstract void SetSumBinsContainersByRoute(List<Models.Signal> signals);

        protected void GetTimeXAxisSignalSeriesChart(List<Models.Signal> signals, Chart chart)
        {
            int i = 1;
            foreach (var signal in signals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = signal.SignalDescription;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                List<BinsContainer> binsContainers = SetBinsContainersBySignal(signal);
                BinsContainer container = binsContainers.FirstOrDefault();
                if (container != null)
                {
                    foreach (var bin in container.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Sum);
                    }
                    chart.Series.Add(series);
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

        private void GetApproachTimeSeriesByProtectedPermissive(Chart chart, int i, Approach approach, bool getPermissivePhase)
        {
            List<BinsContainer> binsContainers = SetBinsContainersByApproach(approach, getPermissivePhase);
            Series series = new Series();
            series.Color = GetSeriesColorByNumber(i);
            series.Name = approach.Description;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
            if ((TimeOptions.BinSize == BinFactoryOptions.BinSizes.Month || TimeOptions.BinSize == BinFactoryOptions.BinSizes.Year) &&
                TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
            {
                foreach (var binsContainer in binsContainers)
                {
                    if (AggregationOperation == AggregationOperations.Sum)
                    {
                        series.Points.AddXY(binsContainer.Start, binsContainer.SumValue);
                    }
                    else
                    {
                        series.Points.AddXY(binsContainer.Start, binsContainer.AverageValue);
                    }
                }
            }
            else
            {
                foreach (var bin in binsContainers.FirstOrDefault()?.Bins)
                {
                    if (AggregationOperation == AggregationOperations.Sum)
                    {
                        series.Points.AddXY(bin.Start, bin.Sum);
                    }
                    else
                    {
                        series.Points.AddXY(bin.Start, bin.Average);
                    }
                }
            }
            chart.Series.Add(series);
        }

        protected Color GetSeriesColorByNumber(int colorNumber)
        {
            while (colorNumber > 10)
            {
                colorNumber = colorNumber - 10;
            }

            switch (colorNumber)
            {
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

        protected void SetSeriestype(Series series)
        {
            switch (ChartType)
            {
                case ChartTypes.StackedColumn:
                    series.ChartType = SeriesChartType.StackedColumn;
                    series.BorderWidth = 2;
                    break;
                case ChartTypes.Line:
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 3;
                    break;
                case ChartTypes.Column:
                    series.ChartType = SeriesChartType.Column;
                    series.BorderWidth = 2;
                    break;
                case ChartTypes.StackedLine:
                    series.ChartType = SeriesChartType.StackedArea;
                    series.BorderWidth = 2;
                    break;
                case ChartTypes.Pie:
                    series.ChartType = SeriesChartType.Pie;
                    series.BorderWidth = 2;
                    break;
            }
        }

        protected abstract List<BinsContainer> SetBinsContainersByApproach(Approach approach, bool getprotectedPhase);
        protected abstract int GetSignalAverageDataPoint(Models.Signal signal);
        protected abstract int GetSignalSumDataPoint(Models.Signal signal);
        protected abstract int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetAverageByDirection(Models.Signal signal, DirectionType direction);
        protected abstract int GetSumByDirection(Models.Signal signal, DirectionType direction);
        protected abstract List<BinsContainer> SetBinsContainersBySignal(Models.Signal signal);
        protected abstract void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart);
    }

}
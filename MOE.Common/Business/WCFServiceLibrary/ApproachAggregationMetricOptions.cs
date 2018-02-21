using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public abstract class ApproachAggregationMetricOptions : SignalAggregationMetricOptions
    {

        public override string YAxisTitle { get; }

        public override List<string> CreateMetric()
        {
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


        protected void GetDirectionCharts()
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
            List<DirectionType> directionsList = GetFilteredDirections();
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

        private List<DirectionType> GetFilteredDirections()
        {
            var direcitonRepository = Models.Repositories.DirectionTypeRepositoryFactory.Create();
            var includedDirections = FilterDirections.Where(f => f.Include).Select(f => f.DirectionTypeId).ToList();
            var directionsList = direcitonRepository.GetDirectionsByIDs(includedDirections);
            return directionsList;
        }

        protected void GetApproachCharts()
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

        protected override void GetTimeCharts()
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


        protected void GetSignalsXAxisDirectionSeriesChart(List<Models.Signal> signals, Chart chart)
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


        protected void GetTimeXAxisDirectionSeriesChart(Models.Signal signal, Chart chart)
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


        protected override void GetTimeOfDayCharts()
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


        protected void GetTimeOfDayXAxisDirectionSeriesChart(Models.Signal signal, Chart chart)
        {
            SetTimeOfDayXAxisMinimum(chart);
            var availableDirections = signal.GetAvailableDirections();
            ConcurrentBag<Series> seriesList = new ConcurrentBag<Series>();
            Parallel.For(0, availableDirections.Count, i => // foreach (var signal in signals)
            {
                List<BinsContainer> binsContainers = GetBinsContainersByDirection(availableDirections[i], signal);
                Series series = CreateSeries(i, availableDirections[i].Description);
                seriesList.Add(GetTimeAggregateSeries(series, binsContainers));
            });
            foreach (var direction in availableDirections)
            {
                chart.Series.Add(seriesList.FirstOrDefault(s => s.Name == direction.Description));
            }
        }


        protected void GetTimeOfDayXAxisApproachSeriesChart(Models.Signal signal, Chart chart)
        {
            if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute.Value != null)
            {
                chart.ChartAreas.FirstOrDefault().AxisX.Minimum =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                            TimeOptions.TimeOfDayStartHour.Value, TimeOptions.TimeOfDayStartMinute.Value, 0)
                        .AddHours(-1).ToOADate();
            }
            ConcurrentBag<Series> seriesList = new ConcurrentBag<Series>();
            List<Approach> approaches = signal.Approaches.ToList();
            Parallel.For(0, approaches.Count, i => 
            {
                string phaseDescription = GetPhaseDescription(approaches[i], true);
                List<BinsContainer> binsContainers = GetBinsContainersByApproach(approaches[i], true);
                Series series = CreateSeries(i, approaches[i].Description + phaseDescription);
                seriesList.Add(GetTimeAggregateSeries(series, binsContainers));
                if (approaches[i].PermissivePhaseNumber != null)
                {
                    string permissivePhaseDescription = GetPhaseDescription(approaches[i], false);
                    List<BinsContainer> permissiveBinsContainers = GetBinsContainersByApproach(approaches[i], false);
                    Series permissiveSeries = CreateSeries(i, approaches[i].Description + permissivePhaseDescription);
                    seriesList.Add(GetTimeAggregateSeries(permissiveSeries, permissiveBinsContainers));
                    i++;
                }
            });
            List<Series> orderedSeries = seriesList.OrderBy(s => s.Name).ToList();
            foreach (var series in orderedSeries)
            {
                chart.Series.Add(series);
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
        
        private static string GetPhaseDescription(Approach approach, bool getProtectedPhase)
        {
            return getProtectedPhase ? " Phase " + approach.ProtectedPhaseNumber : " Phase " + approach.PermissivePhaseNumber;
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
        
        

        protected abstract List<BinsContainer> GetBinsContainersByApproach(Approach approach, bool getprotectedPhase);
        protected abstract int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber);
        protected abstract int GetAverageByDirection(Models.Signal signal, DirectionType direction);
        protected abstract int GetSumByDirection(Models.Signal signal, DirectionType direction);
        protected abstract List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType, Models.Signal signal);
        protected abstract List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber);

    }

}
using System;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using static MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions;

namespace MOE.Common.Business.WCFServiceLibrary
{

    [DataContract]
    public class ApproachSplitFailAggregationOptions: AggregationMetricOptions
    {
        public  ApproachSplitFailAggregationOptions()
        {
            MetricTypeID = 20;
        }

        

        public override int GetSignalSumDataPoint(Models.Signal signal)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }

        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }

        protected override int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, direction);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }

        protected override int GetSumByDirection(Models.Signal signal, DirectionType direction)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, direction);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }
        
        protected override int GetSignalAverageDataPoint(Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }


        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal = new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType, Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, directionType);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetSumBinsContainersByRoute(List<Models.Signal> signals)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var signal in signals)
            {
                SplitFailAggregationBySignal splitFail = new SplitFailAggregationBySignal(this, signal);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                    {
                        var bin = binsContainers[i].Bins[binIndex];
                        bin.Sum += splitFail.BinsContainers[i].Bins[binIndex].Sum;
                        bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                    }
                }
            }
            return binsContainers;
        }
        
        protected override List<BinsContainer> SetBinsContainersByApproach(Models.Approach approach, bool getprotectedPhase)
        {
            ApproachSplitFailAggregationContainer approachSplitFailAggregationContainer = new ApproachSplitFailAggregationContainer(approach, TimeOptions, StartDate, EndDate,
                getprotectedPhase);
            return approachSplitFailAggregationContainer.BinsContainers;
        }

        //protected override void GetTimeAggregateChart(Models.Signal signal, Chart chart)
        //{
        //    SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal,  BinsContainers);
        //    int i = 1;
        //    foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
        //    {
        //        Series series = new Series();
        //        SeriesCount++;
        //        series.Color = GetSeriesColorByNumber(i);
        //        series.Name = approachSplitFails.Approach.Description;
        //        series.ChartArea = "ChartArea1";
        //        series.ChartType = ChartType;
        //        if ((TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Month || TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Year) &&
        //            TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
        //        {
        //            foreach (var binsContainer in approachSplitFails.BinsContainers)
        //            {
        //                if (AggregationOperation == AggregationOperations.Sum)
        //                {
        //                    series.Points.AddXY(binsContainer.Start, binsContainer.SumValue);
        //                    DataPointCount++;
        //                }
        //                else
        //                {
        //                    series.Points.AddXY(binsContainer.Start, binsContainer.AverageValue);
        //                    DataPointCount++;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (var bin in approachSplitFails.BinsContainers.FirstOrDefault().Bins)
        //            {
        //                if (AggregationOperation == AggregationOperations.Sum)
        //                {
        //                    series.Points.AddXY(bin.Start, bin.Sum);
        //                    DataPointCount++;
        //                }
        //                else
        //                {
        //                    series.Points.AddXY(bin.Start, bin.Average);
        //                    DataPointCount++;
        //                }
        //            }
        //        }
        //        chart.Series.Add(series);
        //        i++;
        //    }
        //}

        //protected override void GetApproachAggregateChart(Models.Signal signal, Chart chart)
        //{
        //    SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
        //    Series series = new Series();
        //    SeriesCount++;
        //    series.Name = signal.SignalDescription;
        //    series.ChartArea = "ChartArea1";
        //    series.ChartType = ChartType;
        //    int i = 1;
        //    foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
        //    {
                
        //        DataPoint dataPoint = new DataPoint();
        //        DataPointCount++;
        //        dataPoint.XValue = i;
        //        dataPoint.Color = GetSeriesColorByNumber(i);
        //        if (AggregationOperation == AggregationOperations.Sum)
        //        {
        //            dataPoint.SetValueY(approachSplitFails.BinsContainers.FirstOrDefault().SumValue);
        //        }
        //        else
        //        {
        //            dataPoint.SetValueY(approachSplitFails.BinsContainers.FirstOrDefault().AverageValue);
        //        }
        //        dataPoint.AxisLabel = approachSplitFails.Approach.Description;
        //        series.Points.Add(dataPoint);
        //        i++;
        //    }
        //        chart.Series.Add(series);
        //}

        

        //protected override void GetSignalByPhaseAggregateCharts(Models.Signal signal, Chart chart)
        //{
        //    SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
        //    Series series = new Series();
        //    SeriesCount++;
        //    series.Color = GetSeriesColorByNumber(1);
        //    series.Name = signal.SignalDescription;
        //    series.ChartArea = "ChartArea1";
        //    series.ChartType = ChartType;
        //    chart.Series.Add(series);
        //    int i = 1;
        //    List<int> phaseNumbers = signal.GetPhasesForSignal();
        //    foreach (var phaseNumber in phaseNumbers)
        //    {
        //        DataPoint dataPoint = new DataPoint();
        //        DataPointCount++;
        //        dataPoint.XValue = i;
        //        if (AggregationOperation == AggregationOperations.Sum)
        //        {
        //            dataPoint.SetValueY(spliFailAggregationBySignal.GetSplitFailsByPhaseNumber(phaseNumber));
        //        }
        //        else
        //        {
        //            dataPoint.SetValueY(spliFailAggregationBySignal.GetAverageSplitFailsByPhaseNumber(phaseNumber));
        //        }
        //        dataPoint.AxisLabel = "Phase " + phaseNumber;
        //        series.Points.Add(dataPoint);
        //        i++;
        //    }
        //}

        //protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        //{
        //    List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>();
        //    foreach (var signal in signals)
        //    {
        //        spliFailAggregationBySignals.Add(new SpliFailAggregationBySignal(this, signal, BinsContainers));
        //    }
        //    int i = 1;
        //    foreach (var spliFailAggregationBySignal in spliFailAggregationBySignals)
        //    {
        //        Series series = new Series();
        //        SeriesCount++;
        //        series.Color = GetSeriesColorByNumber(i);
        //        series.Name = spliFailAggregationBySignal.Signal.SignalDescription;
        //        series.ChartArea = "ChartArea1";
        //        series.ChartType = ChartType;
        //        spliFailAggregationBySignal.GetSumSplitFailuresByBin(BinsContainers);
        //        var container = BinsContainers.FirstOrDefault();
        //        if (container != null)
        //        {
        //            foreach (var bin in container.Bins)
        //            {
        //                series.Points.AddXY(bin.Start, bin.Sum);
        //                DataPointCount++;
        //            }
        //            chart.Series.Add(series);
        //            i++;
        //        }
        //    }
        //}

        //protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        //{
        //    int i = 1;
        //    Series series = new Series();
        //    SeriesCount++;
        //    foreach (var signal in signals)
        //    {
        //        series.Name += signal.SignalDescription + " ";
        //    }
        //    series.ChartArea = "ChartArea1";
        //    series.ChartType = ChartType;
        //    foreach (var signal in signals)
        //    {
        //        List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>
        //        {
        //            new SpliFailAggregationBySignal(this, signal, BinsContainers)
        //        };
        //        foreach (var spliFailAggregationBySignal in spliFailAggregationBySignals)
        //        {
        //            DataPoint dataPoint = new DataPoint();
        //            dataPoint.Color = GetSeriesColorByNumber(i);
        //            DataPointCount++;
        //            dataPoint.XValue = i;
        //            if (AggregationOperation == AggregationOperations.Sum)
        //            {
        //                dataPoint.SetValueY(spliFailAggregationBySignal.TotalSplitFailures);
        //            }
        //            else
        //            {
        //                dataPoint.SetValueY(spliFailAggregationBySignal.AverageSplitFailures);
        //            }
        //            dataPoint.AxisLabel = spliFailAggregationBySignal.Signal.SignalID;
        //            series.Points.Add(dataPoint);
        //        }
        //        i++;
        //    }
        //    chart.Series.Add(series);
        //}

        //protected override void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart)
        //{
        //    var direcitonRepository = Models.Repositories.DirectionTypeRepositoryFactory.Create();
        //    var directionsList = direcitonRepository.GetAllDirections();
        //    int columnCounter = 1;
        //    var colorCount = 1;
        //    foreach (var direction in directionsList)
        //    {
        //        Series series = new Series();
        //        series.Color = GetSeriesColorByNumber(colorCount);
        //        series.Name = direction.Description;
        //        series.ChartArea = "ChartArea1";
        //        series.ChartType = ChartType;
        //        chart.Series.Add(series);
        //        colorCount++;
        //    }
        //    foreach (var signal in signals)
        //    {
        //        SplitFailAggregationBySignal splitFailAggregationBySignal 
        //            = new SplitFailAggregationBySignal(this, signal);
        //        foreach (var direction in directionsList)
        //        {
        //            DataPoint dataPoint = new DataPoint();
        //            dataPoint.XValue = columnCounter;
        //            if (AggregationOperation == AggregationOperations.Sum)
        //            {
        //                dataPoint.SetValueY(splitFailAggregationBySignal.GetSplitFailsByDirection(direction));
        //            }
        //            else
        //            {
        //                dataPoint.SetValueY(splitFailAggregationBySignal.GetAverageSplitFailsByDirection(direction));
        //            }
        //            dataPoint.AxisLabel = signal.SignalID;
        //            chart.Series[direction.Description].Points.Add(dataPoint);
        //        }
        //        columnCounter++;
        //    }
        //}

        //protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        //{
        //    SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
        //    Series series = new Series();
        //    SeriesCount++;
        //    series.Color = GetSeriesColorByNumber(1);
        //    series.Name = signal.SignalDescription;
        //    series.ChartArea = "ChartArea1";
        //    series.ChartType = ChartType;
        //    chart.Series.Add(series);

        //    int i = 1;
        //    List<DirectionType> directions = signal.GetAvailableDirections();
        //    foreach (var direction in directions)
        //    {
        //        DataPoint dataPoint = new DataPoint();
        //        DataPointCount++;
        //        dataPoint.XValue = i;
        //        if (AggregationOperation == AggregationOperations.Sum)
        //        {
        //            dataPoint.SetValueY(spliFailAggregationBySignal.GetSplitFailsByDirection(direction));
        //        }
        //        else
        //        {
        //            dataPoint.SetValueY(spliFailAggregationBySignal.GetAverageSplitFailsByDirection(direction));
        //        }
        //        dataPoint.AxisLabel = direction.Description;
        //        series.Points.Add(dataPoint);
        //        i++;
        //    }
        //}
    }
}

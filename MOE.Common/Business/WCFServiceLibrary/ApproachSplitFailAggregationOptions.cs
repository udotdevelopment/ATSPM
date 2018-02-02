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

        protected override int GetSignalSumDataPoint(Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }

        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers, phaseNumber);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }

        protected override int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers, phaseNumber);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers, direction);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }

        protected override int GetSumByDirection(Models.Signal signal, DirectionType direction)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers, direction);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }
        
        protected override int GetSignalAverageDataPoint(Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, BinsContainers);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }


        protected override List<BinsContainer> SetBinsContainersBySignal(Models.Signal signal)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal = new SplitFailAggregationBySignal(this, signal, BinsContainers);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override void SetSumBinsContainersByRoute(List<Models.Signal> signals)
        {
            foreach (var signal in signals)
            {
                SplitFailAggregationBySignal splitFail = new SplitFailAggregationBySignal(this, signal, BinsContainers);
                for (int i = 0; i < BinsContainers.Count; i++)
                {
                    for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
                    {
                        var bin = BinsContainers[i].Bins[binIndex];
                        bin.Sum += splitFail.BinsContainers[i].Bins[binIndex].Sum;
                        bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                    }
                }

            }
        }

        
        protected override List<BinsContainer> SetBinsContainersByApproach(Models.Approach approach, bool getprotectedPhase)
        {
            ApproachSplitFailAggregationContainer approachSplitFailAggregationContainer = new ApproachSplitFailAggregationContainer(approach, BinsContainers, StartDate,
                EndDate, getprotectedPhase);
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
        //        SetSeriestype(series);
        //        if ((TimeOptions.BinSize == BinFactoryOptions.BinSizes.Month || TimeOptions.BinSize == BinFactoryOptions.BinSizes.Year) &&
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
        //    SetSeriestype(series);
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
        //    SetSeriestype(series);
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
        //        SetSeriestype(series);
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
        //    SetSeriestype(series);
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

        protected override void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            var direcitonRepository = Models.Repositories.DirectionTypeRepositoryFactory.Create();
            var directionsList = direcitonRepository.GetAllDirections();
            int columnCounter = 1;
            var colorCount = 1;
            foreach (var direction in directionsList)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(colorCount);
                series.Name = direction.Description;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                chart.Series.Add(series);
                colorCount++;
            }
            foreach (var signal in signals)
            {
                SplitFailAggregationBySignal splitFailAggregationBySignal 
                    = new SplitFailAggregationBySignal(this, signal, BinsContainers);
                foreach (var direction in directionsList)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = columnCounter;
                    if (AggregationOperation == AggregationOperations.Sum)
                    {
                        dataPoint.SetValueY(splitFailAggregationBySignal.GetSplitFailsByDirection(direction));
                    }
                    else
                    {
                        dataPoint.SetValueY(splitFailAggregationBySignal.GetAverageSplitFailsByDirection(direction));
                    }
                    dataPoint.AxisLabel = signal.SignalID;
                    chart.Series[direction.Description].Points.Add(dataPoint);
                }
                columnCounter++;
            }
        }

        //protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        //{
        //    SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
        //    Series series = new Series();
        //    SeriesCount++;
        //    series.Color = GetSeriesColorByNumber(1);
        //    series.Name = signal.SignalDescription;
        //    series.ChartArea = "ChartArea1";
        //    SetSeriestype(series);
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

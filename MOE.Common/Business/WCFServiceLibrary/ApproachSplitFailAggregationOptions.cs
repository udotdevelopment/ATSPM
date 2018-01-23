using System;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using static MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class ApproachSplitFailAggregationOptions: AggregationMetricOptions
    {
        public  ApproachSplitFailAggregationOptions()
        {
            MetricTypeID = 20;
        }

        protected override void GetTimeAggregateChart(Models.Signal signal, Chart chart)
        {
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal,  BinsContainers);
            int i = 1;
            foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = approachSplitFails.Approach.Description;
                series.ChartArea = "ChartArea1";
                series.BorderWidth = 2;
                SetSeriestype(series);
                if (AggregationOpperation == AggregationOpperations.Sum)
                {
                    if ((TimeOptions.BinSize == BinFactoryOptions.BinSizes.Month || TimeOptions.BinSize == BinFactoryOptions.BinSizes.Year) &&
                        TimeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod)
                    {
                        foreach (var binsContainer in approachSplitFails.BinsContainers)
                        {
                            if (AggregationOpperation == AggregationOpperations.Sum)
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
                        foreach (var bin in approachSplitFails.BinsContainers.FirstOrDefault().Bins)
                        {
                            series.Points.AddXY(bin.Start, bin.Value);
                        }
                    }
                }
                //else
                //{
                //    foreach (var splitFail in approachSplitFails.AverageSplitFails)
                //    {
                //        series.Points.AddXY(splitFail.Key, splitFail.Value);
                //    }
                //}
                chart.Series.Add(series);
                i++;
            }
        }

        protected override void GetApproachAggregateChart(Models.Signal signal, Chart chart)
        {
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
            Series series = new Series();
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
            int i = 1;
            foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
            {
                
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                dataPoint.Color = GetSeriesColorByNumber(i);
                if (AggregationOpperation == AggregationOpperations.Sum)
                {
                    dataPoint.SetValueY(approachSplitFails.BinsContainers.FirstOrDefault().SumValue);
                }
                //else
                //{
                //    dataPoint.SetValueY(approachSplitFails.AveragePreemptsServiced);
                //}
                dataPoint.AxisLabel = approachSplitFails.Approach.Description;
                series.Points.Add(dataPoint);
                i++;
            }
                chart.Series.Add(series);
        }

        protected override void GetSignalByPhaseAggregateCharts(List<Models.Signal> signals, Chart chart)
        {
            throw new NotImplementedException();
        }

        protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            //XAxisTimeType = XAxisTimeTypes.Hour;
            List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>();
            foreach (var signal in signals)
            {
                spliFailAggregationBySignals.Add(new SpliFailAggregationBySignal(this, signal, BinsContainers));
            }
            int i = 1;
            foreach (var spliFailAggregationBySignal in spliFailAggregationBySignals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = spliFailAggregationBySignal.Signal.SignalDescription;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                spliFailAggregationBySignal.GetSplitFailuresByBin(BinsContainers);
                var container = BinsContainers.FirstOrDefault();
                if (container != null)
                {
                    foreach (var bin in container.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Value);
                    }
                    chart.Series.Add(series);
                    i++;
                }
            }
        }

        protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            int i = 1;
            foreach (var signal in signals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = signal.SignalID;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                chart.Series.Add(series);

                List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>
                {
                    new SpliFailAggregationBySignal(this, signal, BinsContainers)
                };

          
                foreach (var spliFailAggregationBySignal in spliFailAggregationBySignals)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i;
                    if (AggregationOpperation == AggregationOpperations.Sum)
                    {
                        dataPoint.SetValueY(spliFailAggregationBySignal.TotalSplitFailures);
                    }
                    else
                    {
                        dataPoint.SetValueY(spliFailAggregationBySignal.TotalSplitFailures);
                    }
                    dataPoint.AxisLabel = spliFailAggregationBySignal.Signal.SignalID;
                    series.Points.Add(dataPoint);
                    
                }
                
                i++;
                
            }

        }

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
                SpliFailAggregationBySignal spliFailAggregationBySignal 
                    = new SpliFailAggregationBySignal(this, signal, BinsContainers);


                foreach (var direction in directionsList)
                {


                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = columnCounter;
                    if (AggregationOpperation == AggregationOpperations.Sum)
                    {
                        dataPoint.SetValueY(spliFailAggregationBySignal.GetSplitFailsByDirection(direction));
                    }
                    else
                    {
                        dataPoint.SetValueY(spliFailAggregationBySignal.GetAverageSplitFailsByDirection(direction));
                    }
                    dataPoint.AxisLabel = signal.SignalID;
                    chart.Series[direction.Description].Points.Add(dataPoint);
                    
                }

                columnCounter++;
            }
        }

        protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        {
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainers);
            Series series = new Series();
            series.Color = GetSeriesColorByNumber(1);
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
            chart.Series.Add(series);

            int i = 1;
            List<DirectionType> directions = signal.GetAvailableDirections();
            foreach (var direction in directions)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                if (AggregationOpperation == AggregationOpperations.Sum)
                {
                    dataPoint.SetValueY(spliFailAggregationBySignal.GetSplitFailsByDirection(direction));
                }
                else
                {
                    dataPoint.SetValueY(spliFailAggregationBySignal.GetAverageSplitFailsByDirection(direction));
                }
                dataPoint.AxisLabel = direction.Description;
                series.Points.Add(dataPoint);
                i++;
            }
            
        }

    }

    


}

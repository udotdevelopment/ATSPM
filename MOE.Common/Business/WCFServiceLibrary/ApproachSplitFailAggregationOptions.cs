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
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainer);
            int i = 1;
            foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
            {
                if (i > 10)
                {
                    i = 1;
                }
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = approachSplitFails.Approach.Description;
                series.ChartArea = "ChartArea1";
                series.BorderWidth = 2;
                SetSeriestype(series);
                if (AggregationOpperation == AggregationOpperations.Sum)
                {
                    foreach (var bin in approachSplitFails.BinsContainer.Bins)
                    {
                        series.Points.AddXY(bin.Start, bin.Value);
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
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainer);
            Series series = new Series();
            series.Color = GetSeriesColorByNumber(1);
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
            int i = 1;
            foreach (var approachSplitFails in spliFailAggregationBySignal.ApproachSplitFailures)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i;
                if (AggregationOpperation == AggregationOpperations.Sum)
                {
                    dataPoint.SetValueY(approachSplitFails.BinsContainer.SumValue);
                }
                //else
                //{
                //    dataPoint.SetValueY(approachSplitFails.AverageSplitFailures);
                //}
                dataPoint.AxisLabel = approachSplitFails.Approach.Description;
                series.Points.Add(dataPoint);
                i++;
            }
            chart.Series.Add(series);
        }

        protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            //XAxisTimeType = XAxisTimeTypes.Hour;
            List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>();
            foreach (var signal in signals)
            {
                spliFailAggregationBySignals.Add(new SpliFailAggregationBySignal(this, signal, BinsContainer));
            }
            int i = 1;
            foreach (var spliFailAggregationBySignal in spliFailAggregationBySignals)
            {
                Series series = new Series();
                series.Color = GetSeriesColorByNumber(i);
                series.Name = spliFailAggregationBySignal.Signal.SignalDescription;
                series.ChartArea = "ChartArea1";
                SetSeriestype(series);
                spliFailAggregationBySignal.GetSplitFailuresByBin(BinsContainer);
                foreach (var bin in BinsContainer.Bins)
                {
                    series.Points.AddXY(bin.Start, bin.Value);
                }
                chart.Series.Add(series);
                i++;
            }
        }

        protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            List<SpliFailAggregationBySignal> spliFailAggregationBySignals = new List<SpliFailAggregationBySignal>();
            foreach (var signal in signals)
            {
                spliFailAggregationBySignals.Add(new SpliFailAggregationBySignal(this, signal, BinsContainer));
            }
            Series series = new Series();
            series.Color = GetSeriesColorByNumber(1);
            series.Name = "Signals";
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
            int i = 1;
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
                dataPoint.AxisLabel = spliFailAggregationBySignal.Signal.SignalDescription;
                series.Points.Add(dataPoint);
                i++;
            }
            chart.Series.Add(series);
        }

        protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        {
            SpliFailAggregationBySignal spliFailAggregationBySignal = new SpliFailAggregationBySignal(this, signal, BinsContainer);
            Series series = new Series();
            series.Color = GetSeriesColorByNumber(1);
            series.Name = signal.SignalDescription;
            series.ChartArea = "ChartArea1";
            SetSeriestype(series);
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
            chart.Series.Add(series);
        }

    }


}

﻿using MOE.Common.Business.DataAggregation;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Models;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class ApproachSpeedAggregationOptions : AggregationMetricOptions
    {
        public ApproachSpeedAggregationOptions()
        {
            MetricTypeID = 21;
        }

        protected override void GetSignalByPhaseAggregateCharts(Models.Signal signal, Chart chart)
        {




        }

        protected override void GetRouteAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetDirectionAggregateChart(Models.Signal signal, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetSignalAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetSignalByDirectionAggregateChart(List<Models.Signal> signals, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetApproachAggregateChart(Models.Signal signal, Chart chart)
        {
            throw new System.NotImplementedException();
        }

        protected override List<BinsContainer> SetBinsContainersBySignal(Models.Signal signal)
        {
            throw new System.NotImplementedException();
        }

        protected override List<BinsContainer> SetBinsContainersByApproach(Approach approach)
        {
            throw new System.NotImplementedException();
        }

        //protected override void GetTimeAggregateChart(Models.Signal signal, Chart chart)
        //{
        //    ApproachSpeedAggregationBySignal approachSpeedAggregatBySignal =
        //        new ApproachSpeedAggregationBySignal(this, signal, BinsContainers);
        //    int i = 1;
        //    foreach (var approachSplitFails in approachSpeedAggregatBySignal.ApproachSpeeds)
        //    {
        //        if (i > 10)
        //        {
        //            i = 1;
        //        }
        //        Series series = new Series();
        //        series.Color = GetSeriesColorByNumber(i);
        //        series.Name = approachSplitFails.Approach.Description;
        //        series.ChartArea = "ChartArea1";
        //        series.BorderWidth = 2;
        //        SetSeriestype(series);
        //        if (AggregationOperation == AggregationOperations.Sum)
        //        {
        //            foreach (var bin in approachSplitFails.BinsContainer.Bins)
        //            {
        //                series.Points.AddXY(bin.Start, bin.Sum);
        //            }
        //        }

        //        chart.Series.Add(series);
        //        i++;
        //    }
        //}
    }
}
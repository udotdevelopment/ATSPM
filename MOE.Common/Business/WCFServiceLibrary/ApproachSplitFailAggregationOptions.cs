using System;
using MOE.Common.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Business.FilterExtensions;
using System.Text.RegularExpressions;

namespace MOE.Common.Business.WCFServiceLibrary
{

    [DataContract]
    public class ApproachSplitFailAggregationOptions: ApproachAggregationMetricOptions
    {
        
        public enum AggregatedDataTypes { SplitFails, GapOuts, ForceOffs, MaxOuts }
        [DataMember]
        public AggregatedDataTypes SelectedAggregatedDataType { get; set; }

        public override string ChartTitle
        {
            get
            {
                string chartTitle;
                chartTitle = "AggregationChart\n";
                chartTitle += TimeOptions.Start.ToString();
                if (TimeOptions.End > TimeOptions.Start)
                    chartTitle += " to " + TimeOptions.End.ToString() + "\n";
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
                                      TimeOptions.TimeOfDayEndMinute.Value, 0).ToString() + "\n";
                }
                chartTitle += TimeOptions.SelectedBinSize.ToString() + " bins ";
                chartTitle += SelectedXAxisType.ToString() + " Aggregation ";
                chartTitle += SelectedAggregationType.ToString();
                return chartTitle;
            }
        }
        


        public  ApproachSplitFailAggregationOptions()
        {
            MetricTypeID = 20;
        }



        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.AverageSplitFailures;
        }

        protected override int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.TotalSplitFailures;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
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

        protected override List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            SplitFailAggregationBySignal splitFailAggregationBySignal =
                new SplitFailAggregationBySignal(this, signal, phaseNumber);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
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

        public override string YAxisTitle
        {
            get
            {
                return SelectedAggregationType.ToString() + " of Split Fail " + Regex.Replace(SelectedAggregatedDataType.ToString(),
                               @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").ToString() + " " + TimeOptions.SelectedBinSize.ToString() + " bins";
            }
        }

        protected override List<BinsContainer> GetBinsContainersByApproach(Models.Approach approach, bool getprotectedPhase)
        {
            ApproachSplitFailAggregationContainer approachSplitFailAggregationContainer = new ApproachSplitFailAggregationContainer(approach, TimeOptions, StartDate, EndDate,
                getprotectedPhase, SelectedAggregatedDataType);
            return approachSplitFailAggregationContainer.BinsContainers;
        }
        
    }
}

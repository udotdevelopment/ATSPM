using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PhaseLeftTurnGapAggregationOptions : ApproachAggregationMetricOptions
    {
        public const string GAP_COUNT_1 = "GapCount1";
        public const string GAP_COUNT_2 = "GapCount2";
        public const string GAP_COUNT_3 = "GapCount3";
        public const string GAP_COUNT_4 = "GapCount4";
        public const string GAP_COUNT_5 = "GapCount5";
        public const string GAP_COUNT_6 = "GapCount6";
        public const string GAP_COUNT_7 = "GapCount7";
        public const string GAP_COUNT_8 = "GapCount8";
        public const string GAP_COUNT_9 = "GapCount9";
        public const string GAP_COUNT_10 = "GapCount10";
        public const string GAP_COUNT_11 = "GapCount11";
        public const string SUM_GAP_DURATION_1 = "SumGapDuration1";
        public const string SUM_GAP_DURATION_2 = "SumGapDuration2";
        public const string SUM_GAP_DURATION_3 = "SumGapDuration3";
        public const string SUM_GREEN_TIME = "SumGreenTime";
        public PhaseLeftTurnGapAggregationOptions()
        {
            MetricTypeID = 20;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 0, DataName = GAP_COUNT_1 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 1, DataName = GAP_COUNT_2 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 2, DataName = GAP_COUNT_3 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 3, DataName = GAP_COUNT_4 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 4, DataName = GAP_COUNT_5 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 5, DataName = GAP_COUNT_6 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 6, DataName = GAP_COUNT_7 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 7, DataName = GAP_COUNT_8 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 8, DataName = GAP_COUNT_9 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 9, DataName = GAP_COUNT_10 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 10, DataName = GAP_COUNT_11 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 11, DataName = SUM_GAP_DURATION_1 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 12, DataName = SUM_GAP_DURATION_2 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 13, DataName = SUM_GAP_DURATION_3 });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 14, DataName = SUM_GREEN_TIME });
        }

        public override string ChartTitle
        {
            get
            {
                string chartTitle;
                chartTitle = "AggregationChart\n";
                chartTitle += TimeOptions.Start.ToString();
                if (TimeOptions.End > TimeOptions.Start)
                    chartTitle += " to " + TimeOptions.End + "\n";
                if (TimeOptions.DaysOfWeek != null)
                    foreach (var dayOfWeek in TimeOptions.DaysOfWeek)
                        chartTitle += dayOfWeek + " ";
                if (TimeOptions.TimeOfDayStartHour != null && TimeOptions.TimeOfDayStartMinute != null &&
                    TimeOptions.TimeOfDayEndHour != null && TimeOptions.TimeOfDayEndMinute != null)
                    chartTitle += " Limited to: " +
                                  new TimeSpan(0, TimeOptions.TimeOfDayStartHour.Value,
                                      TimeOptions.TimeOfDayStartMinute.Value, 0) + " to " + new TimeSpan(0,
                                      TimeOptions.TimeOfDayEndHour.Value,
                                      TimeOptions.TimeOfDayEndMinute.Value, 0) + "\n";
                chartTitle += TimeOptions.SelectedBinSize + " bins ";
                chartTitle += SelectedXAxisType + " Aggregation ";
                chartTitle += SelectedAggregationType.ToString();
                return chartTitle;
            }
        }

        public override string YAxisTitle => Regex.Replace(SelectedAggregationType + " of " +
                                                 SelectedAggregatedDataType.DataName.ToString(),
                                                 @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1") + " " +
                                             TimeOptions.SelectedBinSize + " bins";


        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var phaseCycleAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal);
            return phaseCycleAggregationBySignal.Average;
        }

        protected override double GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var phaseCycleAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal);
            return phaseCycleAggregationBySignal.Average;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
            var phaseCycleAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal, direction);
            return phaseCycleAggregationBySignal.Average;
        }

        protected override double GetSumByDirection(Signal signal, DirectionType direction)
        {
            var phaseCycleAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal, direction);
            return phaseCycleAggregationBySignal.Average;
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var phaseCycleAggregationBySignal = new PhaseLeftTurnGapAggregationBySignal(this, signal);
            return phaseCycleAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType,
            Models.Signal signal)
        {
            var phaseCycleAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal, directionType);
            return phaseCycleAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new PhaseLeftTurnGapAggregationBySignal(this, signal, phaseNumber);
            return splitFailAggregationBySignal.BinsContainers;
        }

        public override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var aggregations = new ConcurrentBag<PhaseLeftTurnGapAggregationBySignal>();
            Parallel.ForEach(signals, signal => { aggregations.Add(new PhaseLeftTurnGapAggregationBySignal(this, signal)); });
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var splitFailAggregationBySignal in aggregations)
                for (var i = 0; i < binsContainers.Count; i++)
                    for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                    {
                        var bin = binsContainers[i].Bins[binIndex];
                        bin.Sum += splitFailAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                        bin.Average = Convert.ToInt32(Math.Round((double)(bin.Sum / signals.Count)));
                    }
            return binsContainers;
        }


        protected override List<BinsContainer> GetBinsContainersByApproach(Approach approach, bool getprotectedPhase)
        {
            var approachCycleAggregationContainer = new PhaseLeftTurnGapAggregationByApproach(approach, this, StartDate,
                EndDate,
                getprotectedPhase, SelectedAggregatedDataType);
            return approachCycleAggregationContainer.BinsContainers;
        }
    }
}
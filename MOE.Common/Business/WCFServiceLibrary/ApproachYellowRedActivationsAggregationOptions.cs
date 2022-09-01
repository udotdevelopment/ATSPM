using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Models;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachYellowRedActivationsAggregationOptions : ApproachAggregationMetricOptions
    {
        public ApproachYellowRedActivationsAggregationOptions()
        {
            MetricTypeID = 20;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 0, DataName = "SevereRedLightViolations" });
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 1, DataName = "TotalRedLightViolations" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 2, DataName = "YellowActivations" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 3, DataName = "ViolationTime" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 4, DataName = "Cycles" });
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
                    chartTitle += "Limited to: " +
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

        public override string YAxisTitle => SelectedAggregationType + " of " + Regex.Replace(
                                                 SelectedAggregatedDataType.DataName,
                                                 @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1") + " " +
                                             TimeOptions.SelectedBinSize + " bins";


        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.Average;
        }

        protected override double GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.Average;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
            var splitFailAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal, direction);
            return splitFailAggregationBySignal.Average;
        }

        protected override double GetSumByDirection(Models.Signal signal, DirectionType direction)
        {
            var yellowRedActivationsAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal, direction);
            return yellowRedActivationsAggregationBySignal.Average;
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var yellowRedActivationsAggregationBySignal = new YellowRedActivationsAggregationBySignal(this, signal);
            return yellowRedActivationsAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType,
            Models.Signal signal)
        {
            var splitFailAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal, directionType);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new YellowRedActivationsAggregationBySignal(this, signal, phaseNumber);
            return splitFailAggregationBySignal.BinsContainers;
        }

        public override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var aggregations = new ConcurrentBag<YellowRedActivationsAggregationBySignal>();
            Parallel.ForEach(signals,
                signal => { aggregations.Add(new YellowRedActivationsAggregationBySignal(this, signal)); });
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var splitFailAggregationBySignal in aggregations)
                for (var i = 0; i < binsContainers.Count; i++)
                for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = binsContainers[i].Bins[binIndex];
                    bin.Sum += splitFailAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                }
            return binsContainers;
        }


        protected override List<BinsContainer> GetBinsContainersByApproach(Approach approach, bool getprotectedPhase)
        {
            var approachYellowRedActivationsAggregationContainer = new YellowRedActivationsAggregationByApproach(
                approach, this, StartDate, EndDate,
                getprotectedPhase, SelectedAggregatedDataType);
            return approachYellowRedActivationsAggregationContainer.BinsContainers;
        }
    }
}
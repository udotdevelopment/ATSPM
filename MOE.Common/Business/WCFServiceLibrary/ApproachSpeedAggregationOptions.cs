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
    public class ApproachSpeedAggregationOptions : ApproachAggregationMetricOptions
    {
        public ApproachSpeedAggregationOptions()
        {
            MetricTypeID = 25;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 0, DataName = "AverageSpeed" });
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 1, DataName = "SpeedVolume" });
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 2, DataName = "Speed85Th" });
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 3, DataName = "Speed15Th" });
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

        public override string YAxisTitle => SelectedAggregationType + " of " +  Regex.Replace(
                                                 SelectedAggregatedDataType.DataName,
                                                 @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1") + " " +
                                             TimeOptions.SelectedBinSize + " bins";


        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal);
            return speedAggregationBySignal.Average;
        }

        protected override double GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal);
            return speedAggregationBySignal.Average;
        }

        protected override int GetAverageByDirection(Models.Signal signal, DirectionType direction)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal, direction);
            return speedAggregationBySignal.Average;
        }

        protected override double GetSumByDirection(Models.Signal signal, DirectionType direction)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal, direction);
            return speedAggregationBySignal.Average;
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var speedAggregationBySignal = new SpeedAggregationBySignal(this, signal);
            return speedAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByDirection(DirectionType directionType,
            Models.Signal signal)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal, directionType);
            return speedAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var speedAggregationBySignal =
                new SpeedAggregationBySignal(this, signal, phaseNumber);
            return speedAggregationBySignal.BinsContainers;
        }

        public override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var aggregations = new ConcurrentBag<SpeedAggregationBySignal>();
            Parallel.ForEach(signals, signal => { aggregations.Add(new SpeedAggregationBySignal(this, signal)); });
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var speedAggregationBySignal in aggregations)
                for (var i = 0; i < binsContainers.Count; i++)
                for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = binsContainers[i].Bins[binIndex];
                    bin.Sum += speedAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                }
            return binsContainers;
        }


        protected override List<BinsContainer> GetBinsContainersByApproach(Approach approach, bool getprotectedPhase)
        {
            var approachSpeedAggregationContainer = new SpeedAggregationByApproach(approach, this,
                StartDate, EndDate,
                getprotectedPhase, SelectedAggregatedDataType);
            return approachSpeedAggregationContainer.BinsContainers;
        }
    }
}
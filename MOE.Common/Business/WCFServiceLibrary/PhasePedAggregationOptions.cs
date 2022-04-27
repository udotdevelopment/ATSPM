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
    public class PhasePedAggregationOptions : PhaseAggregationMetricOptions
    {
        public PhasePedAggregationOptions()
        {
            MetricTypeID = 29;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 0, DataName = PhasePedAggregationByPhase.PED_CYCLES });
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 1, DataName = PhasePedAggregationByPhase.PED_DELAY_SUM });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 2, DataName = PhasePedAggregationByPhase.MIN_PED_DELAY });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 3, DataName = PhasePedAggregationByPhase.MAX_PED_DELAY });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 4, DataName = PhasePedAggregationByPhase.PED_ACTUATIONS });
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


        protected override List<int> GetAvailablePhaseNumbers(Models.Signal signal)
        {
            var phaseTerminationRepository = Models.Repositories.PhasePedAggregationRepositoryFactory.Create();
            return phaseTerminationRepository.GetAvailablePhaseNumbers(signal, this.StartDate, this.EndDate);
        }

        protected override int GetAverageByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new PhasePedAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.Average;
        }

        protected override int GetSumByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var splitFailAggregationBySignal =
                new PhasePedAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.Average;
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var phaseTerminationAggregationBySignal = new PhasePedAggregationBySignal(this, signal);
            return phaseTerminationAggregationBySignal.BinsContainers;
        }
        

        protected override List<BinsContainer> GetBinsContainersByPhaseNumber(Models.Signal signal, int phaseNumber)
        {
            var phaseTerminationAggregationBySignal =
                new PhasePedAggregationBySignal(this, signal, phaseNumber);
            return phaseTerminationAggregationBySignal.BinsContainers;
        }

        public override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var aggregations = new ConcurrentBag<PhasePedAggregationBySignal>();
            Parallel.ForEach(signals, signal => { aggregations.Add(new PhasePedAggregationBySignal(this, signal)); });
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var phasePedAggregationBySignal in aggregations)
                for (var i = 0; i < binsContainers.Count; i++)
                for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = binsContainers[i].Bins[binIndex];
                    bin.Sum += phasePedAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                }
            return binsContainers;
        }
        

        
    }
}
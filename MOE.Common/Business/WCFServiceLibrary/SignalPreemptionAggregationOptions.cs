using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class SignalPreemptionAggregationOptions : SignalAggregationMetricOptions
    {
        public SignalPreemptionAggregationOptions()
        {
            MetricTypeID = 22;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 0, DataName = "PreemptNumber"});
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 1, DataName = "PreemptRequests"});
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 2, DataName = "PreemptServices"});
        }

        public override string YAxisTitle => SelectedAggregationType + " of Preemption " + Regex.Replace(
                                                 SelectedAggregatedDataType.ToString(),
                                                 @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1") + " " +
                                             TimeOptions.SelectedBinSize + " bins";

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var aggregationBySignal = new PreemptionAggregationBySignal(this, signal);
            return aggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var signal in signals)
            {
                var preemptionAggregationBySignal = new PreemptionAggregationBySignal(this, signal);
                for (var i = 0; i < binsContainers.Count; i++)
                for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = binsContainers[i].Bins[binIndex];
                    bin.Sum += preemptionAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                }
            }
            return binsContainers;
        }
    }
}
using System;
using MOE.Common.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Business.FilterExtensions;

namespace MOE.Common.Business.WCFServiceLibrary
{

    [DataContract]
    public class SignalPreemptionAggregationOptions: SignalAggregationMetricOptions
    {
        public enum PreemptionData {PreemptNumber, PreemptRequests, PreemptServices }
        [DataMember]
        public PreemptionData SelectedPreemptionData { get; set; }
        public SignalPreemptionAggregationOptions()
        {
            MetricTypeID = 22;
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            PreemptionAggregationBySignal splitFailAggregationBySignal = new PreemptionAggregationBySignal(this, signal);
            return splitFailAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var signal in signals)
            {
                PreemptionAggregationBySignal preemptionAggregationBySignal = new PreemptionAggregationBySignal(this, signal);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                    {

                        var bin = binsContainers[i].Bins[binIndex];
                        bin.Sum += preemptionAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                        bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                    }
                }
            }
            return binsContainers;
        }
        
        
    }
}

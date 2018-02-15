using System;
using MOE.Common.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Business.FilterExtensions;

namespace MOE.Common.Business.WCFServiceLibrary
{

    [DataContract]
    public class SignalPreemptionAggregationOptions: SignalAggregationMetricOptions
    {
        public SignalPreemptionAggregationOptions()
        {
            MetricTypeID = 22;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType{ Id = 0, DataName = "PreemptNumber"});
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 1, DataName = "PreemptRequests" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 2, DataName = "PreemptServices" });
        }

        public override string YAxisTitle
        {
            get
            {
                return SelectedAggregationType.ToString() + " of Preemption " + Regex.Replace(SelectedAggregatedDataType.ToString(),
                           @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").ToString() + " " + TimeOptions.SelectedBinSize.ToString() + " bins";
            }
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

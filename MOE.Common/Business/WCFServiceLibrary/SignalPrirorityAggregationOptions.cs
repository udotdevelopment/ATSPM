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
    public class SignalPriorityAggregationOptions: SignalAggregationMetricOptions
    {
        public SignalPriorityAggregationOptions()
        {
            MetricTypeID = 24;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 0, DataName = "TotalCycles" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 1, DataName = "PriorityRequests" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 2, DataName = "PriorityServiceEarlyGreen" });
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 3, DataName = "PriorityServiceExtendedGreen" });
        }

        public override string YAxisTitle
        {
            get
            {
                return SelectedAggregationType.ToString() + " of Priority " + Regex.Replace(SelectedAggregatedDataType.ToString(),
                           @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").ToString() + " " + TimeOptions.SelectedBinSize.ToString() + " bins";
            }
        }

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            PriorityAggregationBySignal priorityAggregationBySignal = new PriorityAggregationBySignal(this, signal);
            return priorityAggregationBySignal.BinsContainers;
        }

        protected override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var signal in signals)
            {
                PriorityAggregationBySignal priorityAggregationBySignal = new PriorityAggregationBySignal(this, signal);
                for (int i = 0; i < binsContainers.Count; i++)
                {
                    for (var binIndex = 0; binIndex < binsContainers[i].Bins.Count; binIndex++)
                    {
                        var bin = binsContainers[i].Bins[binIndex];
                        bin.Sum += priorityAggregationBySignal.BinsContainers[i].Bins[binIndex].Sum;
                        bin.Average = Convert.ToInt32(Math.Round((double) (bin.Sum / signals.Count)));
                    }
                }
            }
            return binsContainers;
        }
        
        
    }
}

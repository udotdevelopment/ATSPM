using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MOE.Common.Business.Bins;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class SignalEventCountAggregationOptions : SignalAggregationMetricOptions
    {
        public SignalEventCountAggregationOptions()
        {
            MetricTypeID = 27;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType {Id = 0, DataName = "EventCount"});
            this.Y2AxisTitle = "Event Count";

        }

        public SignalEventCountAggregationOptions(SignalAggregationMetricOptions options)
        {
            MetricTypeID = 27;
            AggregatedDataTypes = new List<AggregatedDataType>();
            AggregatedDataTypes.Add(new AggregatedDataType { Id = 0, DataName = "EventCount" });
            CopySignalAggregationBaseValues(options);
            this.Y2AxisTitle = "Event Count";
           
            
        }

        public override string YAxisTitle => SelectedAggregationType + " of " + Regex.Replace(
                                                 SelectedAggregatedDataType.DataName,
                                                 @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1") + " " +
                                             TimeOptions.SelectedBinSize + " bins";

        

        

        protected override List<BinsContainer> GetBinsContainersBySignal(Models.Signal signal)
        {
            var aggregationBySignal = new SignalEventCountAggregationBySignal(this, signal);
            return aggregationBySignal.BinsContainers;
        }

        public override List<BinsContainer> GetBinsContainersByRoute(List<Models.Signal> signals)
        {
            var binsContainers = BinFactory.GetBins(TimeOptions);
            foreach (var signal in signals)
            {
                var eventCountAggregationBySignal = new SignalEventCountAggregationBySignal(this, signal);
                PopulateBinsForRoute(signals, binsContainers, eventCountAggregationBySignal);
            }
            return binsContainers;
        }

    }
}
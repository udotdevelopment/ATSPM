using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChartGenerator" in both code and config file together.
    [ServiceContract()]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PCDOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.TMCOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.AoROptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachDelayOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.MetricOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseTerminationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptDetailOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptServiceMetricOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptServiceRequestOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.YellowAndRedOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachSpeedOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SplitFailOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SplitMonitorOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SignalAggregationMetricOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachAggregationMetricOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachSplitFailAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PreemptionAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PriorityAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachPcdAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachYellowRedActivationsAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.DetectorAggregationMetricOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.DetectorVolumeAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.ApproachSpeedAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseCycleAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.SignalEventCountAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseTerminationAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhasePedAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.TimingAndActuationsOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseLeftTurnGapAggregationOptions))]
    [ServiceKnownType(typeof(MOE.Common.Business.WCFServiceLibrary.PhaseSplitMonitorAggregationOptions))]


    public interface IMetricGenerator
    {
        [OperationContract]
        List<String> CreateMetric(MOE.Common.Business.WCFServiceLibrary.MetricOptions options);
        [OperationContract]
        List<Tuple<string, string>> GetChartAndXmlFileLocations(MOE.Common.Business.WCFServiceLibrary.MetricOptions options);
        [OperationContract]
        List<MOE.Common.Business.ApproachVolume.MetricInfo> CreateMetricWithDataTable(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions options);
        [OperationContract]
        MOE.Common.Business.TMC.TMCInfo CreateTMCChart(MOE.Common.Business.WCFServiceLibrary.TMCOptions options);
    }
}

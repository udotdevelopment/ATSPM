using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
   
    public interface IMetricGenerator
    {
        [OperationContract]
        List<String> CreateMetric(MOE.Common.Business.WCFServiceLibrary.MetricOptions options);
        [OperationContract]
        List<MOE.Common.Business.ApproachVolume.MetricInfo> CreateMetricWithDataTable(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions options);
        [OperationContract]
        MOE.Common.Business.TMC.TMCInfo CreateTMCChart(MOE.Common.Business.WCFServiceLibrary.TMCOptions options);
    }
}

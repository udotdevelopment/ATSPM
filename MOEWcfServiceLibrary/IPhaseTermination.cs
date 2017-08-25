using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPhaseTermination" in both code and config file together.
    [ServiceContract]
    public interface IPhaseTermination
    {
        [OperationContract]
        string CreateChart(DateTime startDate,
            DateTime endDate,
            string signalId,
            bool showPedWalkStartTime,
            int consecutiveCount,
            string location,
            bool showPlanStripes);
    }
}

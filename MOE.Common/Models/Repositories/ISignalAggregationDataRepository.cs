using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ISignalAggregationDataRepository
    {
        int GetTotalCycles(string signalID, DateTime startTime, DateTime endTime);
        int GetAddCycles(string signalID, DateTime startTime, DateTime endTime);
        int GetSubtractCycles(string signalId, DateTime startTime, DateTime endTime);
        int GetDwellCycles(string signalID, DateTime startTime, DateTime endTime);
        void SaveSignalData(SignalAggregation signalAggregation);
    }
}

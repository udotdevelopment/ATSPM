using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftTurnAnalysisReportAPI.Models
{
    public interface IDataCheckResult
    {
        public bool? LeftTurnVolumeOk { get;  }
        public bool? GapOutOk { get;  }
        public bool? PedCycleOk { get; }
        void  SetLeftTurnVolumesResult(string signalId);
        void  SetGapOutResult(string signalId);
        void  SetPedCyclesResult(string signalId);

    }
}

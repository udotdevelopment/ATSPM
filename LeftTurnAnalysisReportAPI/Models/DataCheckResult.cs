using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftTurnAnalysisReportAPI.Models
{
    public class DataCheckResult : IDataCheckResult
    {
        public bool? LeftTurnVolumeOk { get; private set; }
        public bool? GapOutOk { get; private set; }
        public bool? PedCycleOk { get; private set; }

        //public bool? LeftTurnVolumesOk { get; set; }
        //public bool? GapOutOk { get; set; }
        //public bool? PedCyclesOk { get; set; }

        public DataCheckResult(string signalId)
        {
             SetLeftTurnVolumesResult(signalId);
             SetGapOutResult(signalId);
             SetPedCyclesResult(signalId);
            
        }

        public void SetLeftTurnVolumesResult(string signalId)
        {
            LeftTurnVolumeOk = true;
        }

        public void SetGapOutResult(string signalId)
        {
            GapOutOk = true;
        }

        public void SetPedCyclesResult(string signalId)
        {
            PedCycleOk = true;
        }
    }
}

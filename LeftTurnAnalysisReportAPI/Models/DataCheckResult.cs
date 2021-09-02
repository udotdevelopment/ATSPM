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

        public DataCheckResult(bool checkLeftTurnVolumes, bool checkGapOut, bool checkPedCycles)
        {
            if (checkLeftTurnVolumes)
            {
                SetLeftTurnVolumesResult();
            }
            if(checkGapOut)
            {
                SetGapOutResult();
            }
            if(checkPedCycles)
            {
                SetPedCyclesResult();
            }
        }

        public void SetLeftTurnVolumesResult()
        {
            LeftTurnVolumeOk = true;
        }

        public void SetGapOutResult()
        {
            GapOutOk = true;
        }

        public void SetPedCyclesResult()
        {
            PedCycleOk = true;
        }
    }
}

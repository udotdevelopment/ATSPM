using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftTurnAnalysisReportAPI.Models
{
    public class DataCheckResult 
    {
        public bool LeftTurnVolumeOk { get;  set; }
        public bool GapOutOk { get;  set; }
        public bool PedCycleOk { get;  set; }
        
    }
}

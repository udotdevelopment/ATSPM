using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public interface IDataCheckResult
    {
        public bool LeftTurnVolumeOk { get; }
        public bool GapOutOk { get; }
        public bool PedCycleOk { get; }

    }
}

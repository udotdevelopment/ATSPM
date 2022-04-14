using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftTurnReport.Models
{
    public interface IDataCheckResult
    {
        public bool LeftTurnVolumeOk { get;  }
        public bool GapOutOk { get;  }
        public bool PedCycleOk { get; }

    }
}

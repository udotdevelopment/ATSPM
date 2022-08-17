using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class ReportResult
    {
        public double GapDuation { get; set; }
        public double SplitFails { get; set; }
        public double PedActuations { get; set; }
        public LeftTurnVolumeValue Volume { get; set; }
    }
}

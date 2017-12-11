using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ApproachVolume
{
    public class ApproachVolumeViewModel
    {
        public IEnumerable<MOE.Common.Business.ApproachVolume.MetricInfo> InfoList { get; set; }
        public string ShowMetricUrlJavascript { get; set; } = string.Empty;
    }
}

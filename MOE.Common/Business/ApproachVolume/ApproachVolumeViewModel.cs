using System.Collections.Generic;

namespace MOE.Common.Business.ApproachVolume
{
    public class ApproachVolumeViewModel
    {
        public IEnumerable<MetricInfo> InfoList { get; set; }
        public string ShowMetricUrlJavascript { get; set; } = string.Empty;
    }
}
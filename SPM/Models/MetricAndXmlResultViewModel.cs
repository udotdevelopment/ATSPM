using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class MetricAndXmlResultViewModel
    {
        public string ShowMetricUrlJavascript { get; set; } = string.Empty;
        public List<Tuple<string, string>> ChartPaths { get; set; }
    }
}
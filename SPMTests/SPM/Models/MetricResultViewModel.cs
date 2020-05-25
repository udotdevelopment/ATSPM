using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class MetricResultViewModel
    {
        public string ShowMetricUrlJavascript { get; set; } = string.Empty;
        public IEnumerable<String> ChartPaths { get; set; }

    }
}
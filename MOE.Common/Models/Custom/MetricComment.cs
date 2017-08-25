using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public partial class MetricComment
    {
        [NotMapped]
        public List<MetricType> AllMetricTypes { get; set; }
        [NotMapped]
        public int[] MetricIDs { get; set; }
        

    }
}

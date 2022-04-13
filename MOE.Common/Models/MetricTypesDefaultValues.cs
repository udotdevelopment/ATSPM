using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class MetricTypesDefaultValues
    {
        [Key, Column(Order = 0)]
        public string Chart { get; set; }
        [Key, Column(Order = 1)]
        public string Option { get; set; }
        public string Value { get; set; }
    }
}

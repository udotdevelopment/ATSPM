using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models.ViewModel.MetricUsage
{
    public class MetricUsageViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public List<MetricType> MetricTypes { get; set; }
    }
}
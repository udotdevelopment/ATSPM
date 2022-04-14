using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATSPM.Models
{
    public partial class ActionLog
    {
        [Key]
        public int ActionLogID { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public int AgencyID { get; set; }

        public Agency Agency { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [StringLength(10)]
        [Required]
        public string SignalID { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        //[Required]
        //public virtual List<int> ActionIDs { get; set; }

        public ICollection<Action> Actions { get; set; }

        //[Required]
        //public virtual List<int> MetricTypeIDs { get; set; }

        public ICollection<MetricType> MetricTypes { get; set; }
    }
}
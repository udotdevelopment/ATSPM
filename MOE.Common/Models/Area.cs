using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Display(Name = "Signal")]
        [DataMember]
        public virtual ICollection<Signal> Signals { get; set; }

        [DataMember]
        public List<int> SignalIds { get; set; }
    }
}

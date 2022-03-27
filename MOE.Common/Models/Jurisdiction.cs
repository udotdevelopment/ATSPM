using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    public class Jurisdiction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string JurisdictionName { get; set; }

        [StringLength(50)]
        public string MPO { get; set; }

        [StringLength(50)]
        public string CountyParish { get; set; }

        [StringLength(50)]
        public string OtherPartners { get; set; }

        public virtual List<Signal> Signals { get; set; }
    }
}
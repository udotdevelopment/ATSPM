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
        [Display(Name = "Agency Name")]
        public string JurisdictionName { get; set; }

        [StringLength(50)]
        public string MPO { get; set; }

        [StringLength(50)]
        [Display(Name = "County/Parish Name")]
        public string CountyParish { get; set; }

        [StringLength(50)]
        [Display(Name = "Other Partners")]
        public string OtherPartners { get; set; }


        [Display(Name = "Signal")]
        [DataMember]
        public virtual ICollection<Signal> Signals { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual List<JurisdictionSignal> JurisdictionSignals { get; set; }
    }
}
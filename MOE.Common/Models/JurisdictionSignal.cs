using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class JurisdictionSignal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Jurisdiction")]
        public int JurisdictionId { get; set; }

        public virtual Jurisdiction Jurisdiction { get; set; }

        [Required]
        [Display(Name = "Signal Order")]
        public int Order { get; set; }

        [Required]

        [Display(Name = "Signal")]
        [StringLength(10)]
        public string SignalId { get; set; }

        [NotMapped]
        public Signal Signal { get; set; }

    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class AgencySignal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Agency")]
        public int RouteId { get; set; }

        public virtual Route Route { get; set; }

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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public partial class Approach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Approach()
        {
            //LaneGroups = new HashSet<LaneGroup>();
        }

        [Key]
        public int ApproachID { get; set; }

        [Required]
        public string SignalID { get; set; }
        public virtual Signal Signal { get; set; }

        [Required]
        [Display(Name="Direction")]
        public int DirectionTypeID { get; set; }
        public virtual DirectionType DirectionType { get; set; }

        public string Description { get; set; }

        [Display(Name = "MPH (Advanced Count, Advanced Speed)")]
        public int? MPH { get; set; }

        [Required]
        [Display(Name = "Protected Phase")]
        public int ProtectedPhaseNumber { get; set; }

        [Display(Name = "Overlap")]
        public bool IsProtectedPhaseOverlap { get; set; }

        [Display(Name = "Permissive Phase")]
        public int? PermissivePhaseNumber { get; set; }

        public virtual ICollection<Detector> Detectors { get; set; }

        public virtual ICollection<ApproachRouteDetail> ApproachRouteDetails { get; set; }

    }
}

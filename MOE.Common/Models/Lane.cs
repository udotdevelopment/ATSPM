using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public partial class Lane
    {
        
        public Lane()
        {
            //Detectors = new HashSet<Graph_Detectors>();
        }

        [Key]
        public int LaneID { get; set; }

        public string Description { get; set; }

        [Required]
        public int ApproachID { get; set; }
        public virtual Approach Approach { get; set; }

        [Required]
        [Display(Name = "Lane Number")]
        public int LaneNumber { get; set; }

        [Required]
        [Display(Name="Movement Type")]
        public int MovementTypeID { get; set; }
        public virtual MovementType MovementType { get; set; }

        [Required]
        [Display(Name = "Lane Group Type")]
        public int LaneGroupTypeID { get; set; }
        public virtual LaneGroupType LaneGroupType { get; set; }     

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detector> Detectors { get; set; }
    }
}

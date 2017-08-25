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
    public partial class Phase
    {
       
        public Phase()
        {

            LaneGroups = new HashSet<LaneGroup>();
        }
         [Required]
        public bool IsOverlap { get; set; }

        [Key]
        public int PhaseID { get; set; }

         [Required]
        public int PhaseNumber { get; set; }
       
       
         public virtual ICollection<LaneGroup> LaneGroups{ get; set; }


        //
    }
}

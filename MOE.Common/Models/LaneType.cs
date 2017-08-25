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
    public partial class LaneType
    {
        public LaneType()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int LaneTypeID { get; set; }

         [Required]
        [StringLength(30)]
        public string Description { get; set; }

         [Required]
        [StringLength(5)]
        public string Abbreviation { get; set; }

     

    }
}

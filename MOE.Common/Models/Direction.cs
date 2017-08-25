using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public partial class Direction
    {
        [Key]
        public int DirectionID { get; set; }

        [Required]
        public int DirectionTypeID { get; set; }

    [StringLength(100)]
        public String Description { get; set; }

        public Direction()
         {
             Approaches = new List<Approach>();

        }

        public string SignalID { get; set; }

        public virtual Signal Signal { get; set; }
        public virtual DirectionType DirectionType { get; set; }
        public virtual ICollection<Approach> Approaches { get; set; }





    }
}

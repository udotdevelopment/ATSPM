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
    public partial class MovementType
    {
        public MovementType()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int MovementTypeID { get; set; }
        [Required]
        [StringLength(30)]
        public string Description { get; set; }
        [Required]
        [StringLength(5)]
        public string Abbreviation { get; set; }
        public int DisplayOrder { get; set; }
    }
}

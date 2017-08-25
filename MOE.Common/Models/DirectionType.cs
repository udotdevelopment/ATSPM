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
    public partial class DirectionType
    {
        public DirectionType()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DirectionTypeID { get; set; }

        [StringLength(30)]
        public string Description { get; set; }

        [StringLength(5)]
        public string Abbreviation { get; set; }

        public int DisplayOrder { get; set; }
        
    }
}

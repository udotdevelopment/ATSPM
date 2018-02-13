using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    [DataContract]
    public partial class LaneType
    {
        public LaneType()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int LaneTypeID { get; set; }

         [Required]
        [StringLength(30)]
         [DataMember]
        public string Description { get; set; }

         [Required]
        [StringLength(5)]
         [DataMember]
        public string Abbreviation { get; set; }

     

    }
}

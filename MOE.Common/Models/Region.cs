using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Region")]
    [DataContract]
    public partial class Region
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int ID { get; set; }

        [StringLength(50)]
        [DataMember]
        public string Description { get; set; }
    }
}

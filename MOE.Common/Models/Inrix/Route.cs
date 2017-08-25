namespace MOE.Common.Models.Inrix
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Route
    {
        [Key]
        public int Route_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Route_Name { get; set; }

        [Column(TypeName = "text")]
        public string Route_Description { get; set; }
    }
}

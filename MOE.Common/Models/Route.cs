namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Route")]
    public partial class Route
    {
        public int RouteID { get; set; }

        [Required]
        public string Description { get; set; }

        public int Region { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

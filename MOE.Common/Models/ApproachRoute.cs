namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApproachRoute")]
    public partial class ApproachRoute
    {

        [Required]
        public int ApproachRouteId { get; set; }

        [Required]
        public string RouteName { get; set; }

        public virtual ICollection<ApproachRouteDetail> ApproachRouteDetails { get; set; }
    }
}

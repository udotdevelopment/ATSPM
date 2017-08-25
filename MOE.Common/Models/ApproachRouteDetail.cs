namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApproachRouteDetail")]
    public partial class ApproachRouteDetail
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RouteDetailID { get; set; }
        [Required]
        [Display(Name="Approach Route")]
        public int ApproachRouteId { get; set; }
        public virtual ApproachRoute ApproachRoute { get; set; }
        [Required]
        [Display(Name = "Approach Order")]
        public int ApproachOrder { get; set; }
        [Required]
        [Display(Name = "Approach")]
        public int ApproachID { get; set; }
        public virtual Approach Approach { get; set; }
    }
}

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RouteSignal
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RouteDetailId { get; set; }

        [Required]
        [Display(Name="Route")]
        public int ApproachRouteId { get; set; }
        public virtual Route ApproachRoute { get; set; }

        [Required]
        [Display(Name = "Signal Order")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Signal")]     
        public string SignalId { get; set; }
        public List<RoutePhaseDirection> PhaseDirections { get; set; }

    }
}

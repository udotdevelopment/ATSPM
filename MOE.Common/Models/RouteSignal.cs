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
        public int Id { get; set; }

        [Required]
        [Display(Name="Route")]
        public int RouteId { get; set; }
        public virtual Route Route { get; set; }

        [Required]
        [Display(Name = "Signal Order")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Signal")]     
        public string SignalId { get; set; }
        public List<RoutePhaseDirection> PhaseDirections { get; set; }

    }
}

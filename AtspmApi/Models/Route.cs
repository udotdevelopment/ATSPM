using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AtspmApi.Models;

namespace AtspmApi.Models
{ 
    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string RouteName { get; set; }

        public virtual List<RouteSignal> RouteSignals { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public class RoutePhaseDirection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ApproachRouteDetailId { get; set; }
        public RouteSignal ApproachRouteDetail { get; set; }
        public int Phase { get; set; }
        public virtual DirectionType Direction { get; set; }
        public bool IsPhaseDirection1Overlap { get; set; }
    }
}

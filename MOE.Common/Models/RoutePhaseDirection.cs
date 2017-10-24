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
        public int RouteSignalId { get; set; }
        public RouteSignal RouteSignal { get; set; }
        public int Phase { get; set; }
        public int DirectionTypeId { get; set; }
        public virtual DirectionType Direction { get; set; }
        public bool IsOverlap { get; set; }
        public bool IsPrimaryApproach { get; set; }
    }
}

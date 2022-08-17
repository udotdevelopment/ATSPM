using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class RoutePhaseDirection
    {
        public int Id { get; set; }
        public int RouteSignalId { get; set; }
        public int Phase { get; set; }
        public int DirectionTypeId { get; set; }
        public bool IsOverlap { get; set; }
        public bool IsPrimaryApproach { get; set; }

        public virtual DirectionType DirectionType { get; set; }
        public virtual RouteSignal RouteSignal { get; set; }
    }
}

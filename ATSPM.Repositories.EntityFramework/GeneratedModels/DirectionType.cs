using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class DirectionType
    {
        public DirectionType()
        {
            Approaches = new HashSet<Approach>();
            RoutePhaseDirections = new HashSet<RoutePhaseDirection>();
        }

        public int DirectionTypeId { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<Approach> Approaches { get; set; }
        public virtual ICollection<RoutePhaseDirection> RoutePhaseDirections { get; set; }
    }
}

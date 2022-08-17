using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class MovementType
    {
        public MovementType()
        {
            Detectors = new HashSet<Detector>();
        }

        public int MovementTypeId { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<Detector> Detectors { get; set; }
    }
}

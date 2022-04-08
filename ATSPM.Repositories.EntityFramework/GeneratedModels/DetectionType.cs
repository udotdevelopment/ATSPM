using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class DetectionType
    {
        public DetectionType()
        {
            DetectionTypeDetectors = new HashSet<DetectionTypeDetector>();
            DetectionTypeMetricTypes = new HashSet<DetectionTypeMetricType>();
        }

        public int DetectionTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<DetectionTypeDetector> DetectionTypeDetectors { get; set; }
        public virtual ICollection<DetectionTypeMetricType> DetectionTypeMetricTypes { get; set; }
    }
}

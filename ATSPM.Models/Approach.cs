using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Approach
    {
        public Approach()
        {
            Detectors = new HashSet<Detector>();
        }

        public int ApproachId { get; set; }
        public string SignalId { get; set; }
        public Signal Signal { get; set; }
        public int DirectionTypeId { get; set; }
        public string Description { get; set; }
        public int? Mph { get; set; }
        public int ProtectedPhaseNumber { get; set; }
        public bool IsProtectedPhaseOverlap { get; set; }
        public int? PermissivePhaseNumber { get; set; }
        public int VersionId { get; set; }
        public bool IsPermissivePhaseOverlap { get; set; }
        public int? PedestrianPhaseNumber { get; set; }
        public bool IsPedestrianPhaseOverlap { get; set; }
        public string PedestrianDetectors { get; set; }

        public virtual DirectionType DirectionType { get; set; }
        public virtual ICollection<Detector> Detectors { get; set; }
    }
}

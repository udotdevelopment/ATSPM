using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class LaneType
    {
        public LaneType()
        {
            Detectors = new HashSet<Detector>();
        }

        public int LaneTypeId { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }

        public virtual ICollection<Detector> Detectors { get; set; }
    }
}

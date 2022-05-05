using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Signal
    {
        public string SignalId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PrimaryName { get; set; }
        public string SecondaryName { get; set; }
        public string IPAddress { get; set; }
        public int RegionId { get; set; }
        public int ControllerTypeId { get; set; }
        public bool Enabled { get; set; }
        public bool Pedsare1to1 { get; set; }
        public int VersionId { get; set; }  
        public int VersionActionId { get; set; }
        public VersionAction VersionAction { get; set; }
        public string Note { get; set; }
        public DateTime Start { get; set; }
        public ICollection<Approach> Approaches { get; set; }
        public virtual ControllerType ControllerType { get; set; }
        public virtual Region Region { get; set; }
    }
}

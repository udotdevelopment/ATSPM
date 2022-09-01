using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ControllerType
    {
        public ControllerType()
        {
            Signals = new HashSet<Signal>();
        }

        public int ControllerTypeId { get; set; }
        public string Description { get; set; }
        public long Snmpport { get; set; }
        public string Ftpdirectory { get; set; }
        public bool ActiveFtp { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Signal> Signals { get; set; }
    }
}

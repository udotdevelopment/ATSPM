using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Region
    {
        public Region()
        {
            Signals = new HashSet<Signal>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Signal> Signals { get; set; }
    }
}

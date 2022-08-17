using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Agency
    {
        public Agency()
        {
            ActionLogs = new HashSet<ActionLog>();
        }

        public int AgencyId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ActionLog> ActionLogs { get; set; }
    }
}

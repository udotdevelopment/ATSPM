using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ActionLog
    {
        public ActionLog()
        {
            ActionLogActions = new HashSet<ActionLogAction>();
            ActionLogMetricTypes = new HashSet<ActionLogMetricType>();
        }

        public int ActionLogId { get; set; }
        public DateTime Date { get; set; }
        public int AgencyId { get; set; }
        public string Comment { get; set; }
        public string SignalId { get; set; }
        public string Name { get; set; }

        public virtual Agency Agency { get; set; }
        public virtual ICollection<ActionLogAction> ActionLogActions { get; set; }
        public virtual ICollection<ActionLogMetricType> ActionLogMetricTypes { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ActionLogAction
    {
        public int ActionLogActionLogId { get; set; }
        public int ActionActionId { get; set; }

        public virtual Action ActionAction { get; set; }
        public virtual ActionLog ActionLogActionLog { get; set; }
    }
}

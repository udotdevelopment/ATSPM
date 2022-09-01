using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class Action
    {
        public Action()
        {
            ActionLogActions = new HashSet<ActionLogAction>();
        }

        public int ActionId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ActionLogAction> ActionLogActions { get; set; }
    }
}

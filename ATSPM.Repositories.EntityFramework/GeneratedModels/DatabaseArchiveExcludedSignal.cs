using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class DatabaseArchiveExcludedSignal
    {
        public int Id { get; set; }
        public string SignalId { get; set; }
    }
}

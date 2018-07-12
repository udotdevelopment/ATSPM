using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        IEnumerable<ActionLog> GetAllByDate(DateTime startDate, DateTime endDate);
    }
}
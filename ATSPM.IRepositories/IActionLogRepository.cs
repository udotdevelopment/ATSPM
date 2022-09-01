using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        IEnumerable<ActionLog> GetAllByDate(DateTime startDate, DateTime endDate);
    }
}
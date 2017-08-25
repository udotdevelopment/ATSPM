using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ICommentRepository
    {
        List<Models.Comment> GetAll();
        //List<Models.Comment> GetByEntityChartType(string signalID, int chartType, int entityType);
    }
}

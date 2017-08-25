using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IArchivedMetricsRepository
    {
        
        DateTime GetLastArchiveRunDate();
        List<Models.Signal> GetIntersections(DateTime startDate, DateTime endDate, int region);
        List<Models.Detector> GetDetectors(DateTime startDate, DateTime endDate, int region);
       List<RegionArchiveMetric> GetRegionArchiveMetrics(DateTime start, DateTime end, int startHour, int endHour, List<DayOfWeek> dayTypes, int region);
    }
    
}

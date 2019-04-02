using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IMetricFilterTypesRepository
    {
        List<MetricsFilterType> GetAllFilters();
    }
}
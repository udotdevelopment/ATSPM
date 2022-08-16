using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class MetricFilterTypesRepository : IMetricFilterTypesRepository
    {
        private readonly MOEContext db;

        public MetricFilterTypesRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<MetricsFilterType> GetAllFilters()
        {
            var results = (from r in db.MetricsFilterTypes
                           select r).ToList();
            return results;
        }
    }
}
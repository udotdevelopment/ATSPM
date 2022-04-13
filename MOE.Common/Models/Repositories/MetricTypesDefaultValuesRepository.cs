using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class MetricTypesDefaultValuesRepository : IMetricTypesDefaultValuesRepository
    {
        private readonly SPM _db;

        public MetricTypesDefaultValuesRepository()
        {
            _db = new SPM();
        }

        public MetricTypesDefaultValuesRepository(SPM context)
        {
            _db = context;
        }

        public List<MetricTypesDefaultValues> GetChartDefaults(string chart)
        {
            return (from option in _db.MetricTypesDefaultValues
                    where option.Chart == chart
                    select option).ToList();
        }
    }
}
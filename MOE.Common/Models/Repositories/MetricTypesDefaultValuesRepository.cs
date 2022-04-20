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
        public List<MetricTypesDefaultValues> GetAll()
        {
            return _db.MetricTypesDefaultValues.ToList();
        }

        public IQueryable<string> GetListOfCharts()
        {
            return _db.MetricTypesDefaultValues.Select(o => o.Chart).Distinct();
        }

        public List<MetricTypesDefaultValues> GetChartDefaults(string chart)
        {
            return (from option in _db.MetricTypesDefaultValues
                    where option.Chart == chart
                    select option).ToList();
        }

        public Dictionary<string, string> GetAllAsDictionary()
        {
            var defaults = new Dictionary<string, string>();

            var options = _db.MetricTypesDefaultValues.ToList();

            foreach (var option in options)
            {
                defaults.Add(option.Chart + option.Option, option.Value);
            }

            return defaults;
        }

        public Dictionary<string, string> GetChartDefaultsAsDictionary(string chart)
        {
            var defaults = new Dictionary<string, string>();

            var options = (from option in _db.MetricTypesDefaultValues
                where option.Chart == chart
                select option).ToList();

            foreach (var option in options)
            {
                defaults.Add(option.Option, option.Value);
            }

            return defaults;
        }

        public void Update(MetricTypesDefaultValues option)
        {
            var optiontoUpDate = _db.MetricTypesDefaultValues.Find(option.Chart, option.Option);
            _db.Entry(optiontoUpDate).CurrentValues.SetValues(option);
            _db.SaveChanges();
        }
    }
}
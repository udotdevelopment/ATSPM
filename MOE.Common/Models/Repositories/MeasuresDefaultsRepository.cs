using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class MeasuresDefaultsRepository : IMeasuresDefaultsRepository
    {
        private readonly SPM _db;

        public MeasuresDefaultsRepository()
        {
            _db = new SPM();
        }

        public MeasuresDefaultsRepository(SPM context)
        {
            _db = context;
        }
        public List<MeasuresDefaults> GetAll()
        {
            return _db.MeasuresDefaults.ToList();
        }

        public IQueryable<string> GetListOfMeasures()
        {
            return _db.MeasuresDefaults.Select(o => o.Measure).Distinct();
        }

        public List<MeasuresDefaults> GetMeasureDefaults(string measure)
        {
            return (from option in _db.MeasuresDefaults
                    where option.Measure == measure
                    select option).ToList();
        }

        public Dictionary<string, string> GetAllAsDictionary()
        {
            var defaults = new Dictionary<string, string>();

            var options = _db.MeasuresDefaults.ToList();

            foreach (var option in options)
            {
                defaults.Add(option.Measure + option.OptionName, option.Value);
            }

            return defaults;
        }

        public Dictionary<string, string> GetMeasureDefaultsAsDictionary(string measure)
        {
            var defaults = new Dictionary<string, string>();

            var options = (from option in _db.MeasuresDefaults
                where option.Measure == measure
                select option).ToList();

            foreach (var option in options)
            {
                defaults.Add(option.OptionName, option.Value);
            }

            return defaults;
        }

        public void Update(MeasuresDefaults option)
        {
            var optiontoUpDate = _db.MeasuresDefaults.Find(option.Measure, option.OptionName);
            _db.Entry(optiontoUpDate).CurrentValues.SetValues(option);
            _db.SaveChanges();
        }
    }
}
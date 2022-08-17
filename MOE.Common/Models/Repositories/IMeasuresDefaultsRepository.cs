using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public interface IMeasuresDefaultsRepository
    {
        List<MeasuresDefaults> GetAll();
        IQueryable<string> GetListOfMeasures();
        List<MeasuresDefaults> GetMeasureDefaults(string chart);
        Dictionary<string, string> GetAllAsDictionary();
        Dictionary<string, string> GetMeasureDefaultsAsDictionary(string chart);
        void Update(MeasuresDefaults option);
    }
}

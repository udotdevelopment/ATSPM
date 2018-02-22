using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly SPM db = new SPM();

        public List<Region> GetAllRegions()
        {
            var results = (from r in db.Regions
                select r).ToList();

            return results;
        }
    }
}
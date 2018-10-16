using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private SPM _db;

        public RegionsRepository()
        {
            _db = new SPM();
        }

        public RegionsRepository(SPM context)
        {
            _db = context;
        }

        public List<Region> GetAllRegions()
        {
            var results = (from r in _db.Regions
                select r).ToList();

            return results;
        }
    }
}
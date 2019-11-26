using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly Models.AtspmApi db = new Models.AtspmApi();

        public List<Region> GetAllRegions()
        {
            var results = (from r in db.Regions
                select r).ToList();

            return results;
        }
    }
}
using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly MOEContext db;

        public RegionsRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<Region> GetAllRegions()
        {
            var results = (from r in db.Regions
                           select r).ToList();

            return results;
        }
    }
}
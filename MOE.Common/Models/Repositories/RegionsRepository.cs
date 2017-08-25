using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        Models.SPM db = new SPM();

        public List<Models.Region> GetAllRegions()
        {
            List<Models.Region> results = (from r in db.Regions
                                           select r).ToList();

            return results;
        }
    }
}

using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryRegionsRepository : IRegionsRepository
    {
        public List<Region> GetAllRegions()
        {
            List < Region > regs = new List<Region>();


            var r1 = new Region { Description = "Test 1", ID = 1 };
            var r2 = new Region { Description = "Test 2", ID = 2 };
            var r3 = new Region { Description = "Test 3", ID = 3 };
           

            regs.Add(r1);
            regs.Add(r2);
            regs.Add(r3);

            return regs;
        }
    }
}
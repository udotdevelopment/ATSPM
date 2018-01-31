using System;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryRegionRepository : IRegionsRepository
    {
       

        public InMemoryMOEDatabase _db;
        
        public InMemoryRegionRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryRegionRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }


        public void Add(Region newRegion)
        {
            _db.Regions.Add(newRegion);
        }

        public void DeleteByID(int regionID)
        {
            Region region = _db.Regions.Find(r => r.ID == regionID);

            _db.Regions.Remove(region);
        }

        public List<Region> GetAllRegions()
        {
            List<Region> regions = _db.Regions;
            return regions;
        }

        public Region GetRegionByID(int regionID)
        {
            Region region = _db.Regions.Find(r => r.ID == regionID);

            return region;
        }




        //public void Update(Region region)
        //{
        //    var checkRegion = _db.Regions.Find(r => r.ID == region.ID);
        //    if (checkRegion != null)
        //    {
        //        checkRegion.RegionName = region.RegionName;
        //        if (region.RegionSignals != null)
        //        {
        //            checkRegion.RegionSignals = region.RegionSignals;
        //        }
        //    }
        //}
    }
}
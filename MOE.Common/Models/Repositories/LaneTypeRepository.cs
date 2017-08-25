using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class LaneTypeRepository : ILaneTypeRepository
    {
        Models.SPM db = new SPM();


        public List<Models.LaneType> GetAllLaneTypes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.LaneType> laneTypes = (from r in db.LaneTypes
                                         select r).ToList();
            return laneTypes;
        }

        public Models.LaneType GetLaneTypeByLaneTypeID(int laneTypeID)
        {
            var laneType = (from r in db.LaneTypes
                         where r.LaneTypeID == laneTypeID
                         select r);
            return laneType.FirstOrDefault();
        }

        public void Update(MOE.Common.Models.LaneType laneType)
        {
            MOE.Common.Models.LaneType g = (from r in db.LaneTypes
                                            where r.LaneTypeID == laneType.LaneTypeID
                                         select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(laneType);
                db.SaveChanges();
            }
            else
            {
                db.LaneTypes.Add(laneType);
                db.SaveChanges();
            }
        }

        public void Remove(MOE.Common.Models.LaneType laneType)
        {
            MOE.Common.Models.LaneType g = (from r in db.LaneTypes
                                            where r.LaneTypeID == laneType.LaneTypeID
                                         select r).FirstOrDefault();
            if (g != null)
            {
                db.LaneTypes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.LaneType laneType)
        {
            MOE.Common.Models.LaneType g = (from r in db.LaneTypes
                                            where r.LaneTypeID == laneType.LaneTypeID
                                         select r).FirstOrDefault();
            if (g == null)
            {
                db.LaneTypes.Add(g);
                db.SaveChanges();
            }
        }
    }
}

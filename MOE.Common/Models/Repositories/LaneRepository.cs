using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class LaneRepository : ILaneRepository
    {
        Models.SPM db = new SPM();


        public List<Models.Lane> GetAllLanes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Lane> lanes = (from r in db.Lanes
                                               select r).ToList();

            return lanes;
        }

        public Models.Lane GetLaneByLaneID(int laneID)
        {
            var lane = (from r in db.Lanes
                            where r.LaneID == laneID
                            select r);

            return lane.FirstOrDefault();
        }

        public void Update(MOE.Common.Models.Lane lane)
        {


            MOE.Common.Models.Lane g = (from r in db.Lanes
                                            where r.LaneID == lane.LaneID
                                            select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(lane);
                db.SaveChanges();
            }
            else
            {
                db.Lanes.Add(lane);
                db.SaveChanges();

            }


        }

        public void Remove(MOE.Common.Models.Lane lane)
        {


            MOE.Common.Models.Lane g = (from r in db.Lanes
                                            where r.LaneID == lane.LaneID
                                            select r).FirstOrDefault();
            if (g != null)
            {
                db.Lanes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            MOE.Common.Models.Lane g = db.Lanes.Find(id);
            if (g != null)
            {
                db.Lanes.Remove(g);
                db.SaveChanges();
            }
        }
        public Lane Add(MOE.Common.Models.Lane lane)
        {


            MOE.Common.Models.Lane g = (from r in db.Lanes
                                            where r.LaneID == lane.LaneID
                                            select r).FirstOrDefault();
            if (g == null)
            {
                db.Lanes.Add(lane);
                db.SaveChanges();
            }

            return lane;

        }

    }
}

using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSpeedAggregationRepository : IApproachSpeedAggregationRepository
    {
        private Models.SPM _db;


        public ApproachSpeedAggregationRepository()
        {
            _db = new SPM();
        }

        public ApproachSpeedAggregationRepository(SPM context)
        {
            _db = context;
        }

        public List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start, DateTime end)
        {
            var activationsList = (from r in this._db.ApproachSpeedAggregations
                where r.ApproachId == approachId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return activationsList;
        }

        public void Update(MOE.Common.Models.ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();

        }

        public void Remove(MOE.Common.Models.ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
        public void Add(MOE.Common.Models.ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();


        }
    }
}

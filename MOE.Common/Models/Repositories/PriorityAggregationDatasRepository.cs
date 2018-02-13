using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MOE.Common.Models.Repositories
{
    public class PriorityAggregationDatasRepository : IPriorityAggregationDatasRepository
    {
        SPM db = new SPM();


        private Models.SPM _db;


        public PriorityAggregationDatasRepository()
        {
            _db = new SPM();
        }

        public PriorityAggregationDatasRepository(SPM context)
        {
            _db = context;
        }
        public PriorityAggregation Add(PriorityAggregation priorityAggregation)
        {
            _db.PriorityAggregations.Add(priorityAggregation);
            return priorityAggregation;
        }





        public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PriorityAggregations
                           where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }


        public void Update(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }


    }
}

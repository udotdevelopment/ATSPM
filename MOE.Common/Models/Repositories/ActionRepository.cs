using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ActionRepository : IGenericRepository<Action>
    {
        private SPM db = new SPM();

        public ActionRepository()
        {
        }

        public ActionRepository(SPM context)
        {
            db = context;
        }

        public IEnumerable<Action> GetAll()
        {
            return db.Actions.ToList();
        }

        public Action GetByID(int id)
        {
            return db.Actions.Find(id);
        }

        public void Delete(Action entity)
        {
            db.ATSPM_Agencies.Remove(db.ATSPM_Agencies.Find(entity.ActionID));
        }

        public void Update(Action entity)
        {
            var agencyInDatabase = db.ATSPM_Agencies.Find(entity.ActionID);
            if (agencyInDatabase == null)
                Add(entity);
            else
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
        }

        public void Add(Action entity)
        {
            db.Actions.Add(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SetActionRepository(SPM context)
        {
            db = context;
        }

        //public List<ActionChart> GetLoggedMetricRuns(DateTime startDate, DateTime endDate)
        //{
        //    var metricTypes = db.MetricTypes.ToList();
        //    List<string> metricNames = new List<string>();
        //    foreach(MetricType m in metricTypes)
        //    {
        //        metricNames.Add(m.ChartName);
        //    }
        //    var reportsRun = db.ApplicationEvents
        //        .Where(a => a.Timestamp > startDate
        //            && a.Timestamp <= endDate
        //            && (metricNames + " Run").Contains(a.Description))
        //            .GroupBy(a => a.Description)
        //            .Select(g => new { description = g.Description, count = g.Count() });
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ActionRepository : IGenericRepository<Action>
    {
        Models.SPM db = new Models.SPM();

        public ActionRepository()
        {
        }
        public ActionRepository(Models.SPM context)
        {
            db = context;
        }
        public void SetActionRepository(Models.SPM context)
        {
            db = context;
        }
        public IEnumerable<Models.Action> GetAll()
        {
            return db.Actions.ToList();
        }
        public Models.Action GetByID(int id)
        {
            return db.Actions.Find(id);
        }
        public void Delete(Action entity)
        {
            db.Agencies.Remove(db.Agencies.Find(entity.ActionID));
        }
        public void Update(Models.Action entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.ActionID);
            if(agencyInDatabase == null)
            {
                Add(entity);
            }
            else
            {
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
            }
        }

        public void Add(Models.Action entity)
        {
            db.Actions.Add(entity);
        }

        public void Save()
        {
            db.SaveChanges();
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

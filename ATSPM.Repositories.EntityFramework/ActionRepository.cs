using ATSPM.IRepositories;
using System.Collections.Generic;
using System.Linq;
using ATSPM.Application.Models;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ActionRepository : IGenericRepository<Action>
    {
        private MOEContext db;
        

        public ActionRepository(MOEContext context)
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
            db.Agencies.Remove(db.Agencies.Find(entity.ActionId));
        }

        public void Update(Action entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.ActionId);
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

        public void SetActionRepository(MOEContext context)
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
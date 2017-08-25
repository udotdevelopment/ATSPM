using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ActionLogRepository : IActionLogRepository
    {
        Models.SPM db = new Models.SPM();

        public ActionLogRepository()
        {
        }
        public ActionLogRepository(Models.SPM context)
        {
            db = context;
        }
        public void SetActionLogRepository(Models.SPM context)
        {
            db = context;
        }
        public IEnumerable<Models.ActionLog> GetAll()
        {
            return db.ActionLogs.ToList();
        }
        public IEnumerable<Models.ActionLog> GetAllForNumberOfDays(int days)
        {
            return db.ActionLogs.Where(al => al.Date > DateTime.Today.AddDays(days * -1)).ToList();
        }
        public IEnumerable<Models.ActionLog> GetAllByDate(DateTime startDate, DateTime endDate)
        {
            return db.ActionLogs.Where(al => al.Date >= startDate && al.Date <= endDate).ToList();
        }
        public Models.ActionLog GetByID(int id)
        {
            return db.ActionLogs.Find(id);
        }
        public void Delete(ActionLog entity)
        {
            db.ActionLogs.Remove(db.ActionLogs.Find(entity.ActionLogID));
        }
        public void Update(Models.ActionLog entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.ActionLogID);
            if(agencyInDatabase == null)
            {
                Add(entity);
            }
            else
            {
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
            }
        }

        public void Add(Models.ActionLog entity)
        {
            if(entity.Actions == null)
            {
                entity.Actions = db.Actions
                    .Where(a => entity.ActionIDs.Contains(a.ActionID)).ToList();
            }
            if (entity.MetricTypes == null)
            {
                entity.MetricTypes = db.MetricTypes
                    .Where(a => entity.MetricTypeIDs.Contains(a.MetricID)).ToList();
            }
            db.ActionLogs.Add(entity);
            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }

    }
}

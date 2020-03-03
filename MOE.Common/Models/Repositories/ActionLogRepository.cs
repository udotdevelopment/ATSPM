using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ActionLogRepository : IActionLogRepository
    {
        private SPM db = new SPM();

        public ActionLogRepository()
        {
        }

        public ActionLogRepository(SPM context)
        {
            db = context;
        }

        public IEnumerable<ActionLog> GetAll()
        {
            return db.ActionLogs.ToList();
        }

        public IEnumerable<ActionLog> GetAllByDate(DateTime startDate, DateTime endDate)
        {
            return db.ActionLogs.Where(al => al.Date >= startDate && al.Date <= endDate).ToList();
        }

        public ActionLog GetByID(int id)
        {
            return db.ActionLogs.Find(id);
        }

        public void Delete(ActionLog entity)
        {
            db.ActionLogs.Remove(db.ActionLogs.Find(entity.ActionLogID));
        }

        public void Update(ActionLog entity)
        {
            var agencyInDatabase = db.ATSPM_Agencies.Find(entity.ActionLogID);
            if (agencyInDatabase == null)
                Add(entity);
            else
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
        }

        public void Add(ActionLog entity)
        {
            if (entity.Actions == null)
                entity.Actions = db.Actions
                    .Where(a => entity.ActionIDs.Contains(a.ActionID)).ToList();
            if (entity.MetricTypes == null)
                entity.MetricTypes = db.MetricTypes
                    .Where(a => entity.MetricTypeIDs.Contains(a.MetricID)).ToList();
            db.ActionLogs.Add(entity);
            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SetActionLogRepository(SPM context)
        {
            db = context;
        }

        public IEnumerable<ActionLog> GetAllForNumberOfDays(int days)
        {
            return db.ActionLogs.Where(al => al.Date > DateTime.Today.AddDays(days * -1)).ToList();
        }
    }
}
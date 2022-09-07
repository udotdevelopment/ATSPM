using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ActionLogRepository : IActionLogRepository
    {
        private MOEContext db;


        public ActionLogRepository(MOEContext context)
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
            db.ActionLogs.Remove(db.ActionLogs.Find(entity.ActionLogId));
        }

        public void Update(ActionLog entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.ActionLogId);
            if (agencyInDatabase == null)
                Add(entity);
            else
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
        }

        public void Add(ActionLog entity)
        {
            throw new NotImplementedException();
            //if (entity.Actions == null)
            //    entity.Actions = db.Actions
            //        .Where(a => entity.ActionIDs.Contains(a.ActionID)).ToList();
            //if (entity.MetricTypes == null)
            //    entity.MetricTypes = db.MetricTypes
            //        .Where(a => entity.MetricTypeIDs.Contains(a.MetricID)).ToList();
            //db.ActionLogs.Add(entity);
            //db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SetActionLogRepository(MOEContext context)
        {
            db = context;
        }

        public IEnumerable<ActionLog> GetAllForNumberOfDays(int days)
        {
            return db.ActionLogs.Where(al => al.Date > DateTime.Today.AddDays(days * -1)).ToList();
        }
    }
}
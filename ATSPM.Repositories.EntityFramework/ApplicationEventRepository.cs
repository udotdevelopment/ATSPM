using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApplicationEventRepository : IApplicationEventRepository
    {
        private readonly MOEContext db;

        public ApplicationEventRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDates(DateTime StartDate, DateTime EndDate)
        {
            var applicationEvents = (from r in db.ApplicationEvents
                                     where r.Timestamp > StartDate && r.Timestamp <= EndDate
                                     select r).ToList();

            return applicationEvents;
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesForFtpFromAllControllersEvents(DateTime StartDate, DateTime EndDate)
        {
            var applicationEvents = (from r in db.ApplicationEvents
                                     where r.Timestamp > StartDate && r.Timestamp <= EndDate
                                     && r.ApplicationName == "FTPFromAllcontrollers"
                                     select r).ToList();
            return applicationEvents;
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplication(DateTime StartDate,
            DateTime EndDate, string ApplicationName)
        {
            var applicationEvents = (from r in db.ApplicationEvents
                                     where r.Timestamp > StartDate && r.Timestamp <= EndDate && r.ApplicationName == ApplicationName
                                     select r).ToList();

            return applicationEvents;
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplicationBySeverity(DateTime StartDate,
            DateTime EndDate, string ApplicationName, ApplicationEvent.SeverityLevels Severity)
        {
            var applicationEvents = (from r in db.ApplicationEvents
                                     where r.Timestamp > StartDate && r.Timestamp <= EndDate
                                           && r.ApplicationName == ApplicationName && r.SeverityLevel == (int)Severity
                                     select r).ToList();

            return applicationEvents;
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByClass(DateTime StartDate, DateTime EndDate,
            string ApplicationName, string ClassName)
        {
            var applicationEvents = (from r in db.ApplicationEvents
                                     where r.Timestamp > StartDate && r.Timestamp <= EndDate
                                           && r.ApplicationName == ApplicationName && r.Class == ClassName
                                     select r).ToList();

            return applicationEvents;
        }

        public List<ApplicationEvent> GetAllApplicationEvents()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var applicationEvents = (from r in db.ApplicationEvents
                                     select r).ToList();

            return applicationEvents;
        }

        public ApplicationEvent GetApplicationEventByID(int applicationEventID)
        {
            var applicationEvent = from r in db.ApplicationEvents
                                   where r.Id == applicationEventID
                                   select r;

            return applicationEvent.FirstOrDefault();
        }

        public void Update(ApplicationEvent applicationEvent)
        {
            var g = (from r in db.ApplicationEvents
                     where r.Id == applicationEvent.Id
                     select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(applicationEvent);
                db.SaveChanges();
            }
            else
            {
                db.ApplicationEvents.Add(applicationEvent);
                db.SaveChanges();
            }
        }

        public void Remove(ApplicationEvent applicationEvent)
        {
            var g = (from r in db.ApplicationEvents
                     where r.Id == applicationEvent.Id
                     select r).FirstOrDefault();
            if (g != null)
            {
                db.ApplicationEvents.Remove(g);
                db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            var g = db.ApplicationEvents.Find(id);
            if (g != null)
            {
                db.ApplicationEvents.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(ApplicationEvent applicationEvent)
        {
            if (applicationEvent != null)
                try
                {
                    db.ApplicationEvents.Add(applicationEvent);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
        }

        public void QuickAdd(string applicationName, string errorClass, string errorFunction,
            ApplicationEvent.SeverityLevels severity, string description)
        {
            var e = new ApplicationEvent
            {
                ApplicationName = applicationName,
                Class = errorClass,
                Function = errorFunction,
                SeverityLevel = ((int)severity),
                Timestamp = DateTime.Now,
                Description = description
            };
            Add(e);
        }

        public List<ApplicationEvent> GetEventsByDateDescriptions(DateTime startDate, DateTime endDate,
            List<string> descriptions)
        {
            var events = db.ApplicationEvents
                .Where(ae => ae.Timestamp >= startDate
                             && ae.Timestamp <= endDate
                             && descriptions.Contains(ae.Description))
                .ToList();
            return events;
        }
    }
}
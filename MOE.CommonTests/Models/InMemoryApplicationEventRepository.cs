

using System;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    internal class InMemoryApplicationEventRepository : IApplicationEventRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApplicationEventRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemoryApplicationEventRepository()
        {
            this._db = new InMemoryMOEDatabase();
        }

        public List<ApplicationEvent> GetAllApplicationEvents()
        {
            throw new NotImplementedException();
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDates(DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplication(DateTime StartDate, DateTime EndDate, string ApplicationName)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplicationBySeverity(DateTime StartDate, DateTime EndDate,
            string ApplicationName, ApplicationEvent.SeverityLevels Severity)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationEvent> GetApplicationEventsBetweenDatesByClass(DateTime StartDate, DateTime EndDate, string ApplicationName,
            string ClassName)
        {
            throw new NotImplementedException();
        }

        public ApplicationEvent GetApplicationEventByID(int applicationEventID)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationEvent applicationEvent)
        {
            throw new NotImplementedException();
        }

        public void Add(ApplicationEvent applicationEvent)
        {
            _db.ApplicaitonEvents.Add(applicationEvent);
        }

        public void Remove(ApplicationEvent applicationEvent)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void QuickAdd(string applicationName, string errorClass, string errorFunction, ApplicationEvent.SeverityLevels severity,
            string description)
        {
            ApplicationEvent applicationEvent = new ApplicationEvent
            {
                ApplicationName = applicationName,
                Timestamp = DateTime.Now,
                Class = errorClass,
                Description = description,
                Function = errorFunction,
                SeverityLevel = severity
            };

            _db.ApplicaitonEvents.Add(applicationEvent);
        }

        public List<ApplicationEvent> GetEventsByDateDescriptions(DateTime startDate, DateTime endDate, List<string> descriptions)
        {
            throw new NotImplementedException();
        }
    }
}
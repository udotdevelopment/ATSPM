using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IApplicationEventRepository
    {
        List<ApplicationEvent> GetAllApplicationEvents();
        List<ApplicationEvent> GetApplicationEventsBetweenDates(DateTime StartDate, DateTime EndDate);

        List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplication(DateTime StartDate, DateTime EndDate,
            string ApplicationName);

        List<ApplicationEvent> GetApplicationEventsBetweenDatesByApplicationBySeverity(DateTime StartDate,
            DateTime EndDate, string ApplicationName, ApplicationEvent.SeverityLevels Severity);

        List<ApplicationEvent> GetApplicationEventsBetweenDatesByClass(DateTime StartDate, DateTime EndDate,
            string ApplicationName, string ClassName);

        ApplicationEvent GetApplicationEventByID(int applicationEventID);
        void Update(ApplicationEvent applicationEvent);
        void Add(ApplicationEvent applicationEvent);
        void Remove(ApplicationEvent applicationEvent);
        void Remove(int id);

        void QuickAdd(string applicationName, string errorClass, string errorFunction,
            ApplicationEvent.SeverityLevels severity, string description);

        List<ApplicationEvent> GetEventsByDateDescriptions(DateTime startDate, DateTime endDate,
            List<string> descriptions);
    }
}
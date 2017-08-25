using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IApplicationEventRepository
    {
        List<Models.ApplicationEvent> GetAllApplicationEvents();
        List<Models.ApplicationEvent> GetApplicationEventsBetweenDates(DateTime StartDate, DateTime EndDate);
        List<Models.ApplicationEvent> GetApplicationEventsBetweenDatesByApplication(DateTime StartDate, DateTime EndDate, string ApplicationName);
        List<Models.ApplicationEvent> GetApplicationEventsBetweenDatesByApplicationBySeverity(DateTime StartDate, DateTime EndDate, string ApplicationName, ApplicationEvent.SeverityLevels Severity);
        List<Models.ApplicationEvent> GetApplicationEventsBetweenDatesByClass(DateTime StartDate, DateTime EndDate, string ApplicationName, string ClassName);
        Models.ApplicationEvent GetApplicationEventByID(int applicationEventID);
        void Update(MOE.Common.Models.ApplicationEvent applicationEvent);
        void Add(MOE.Common.Models.ApplicationEvent applicationEvent);
        void Remove(MOE.Common.Models.ApplicationEvent applicationEvent);
        void Remove(int id);
        void QuickAdd(string applicationName, string errorClass, string errorFunction,
            ApplicationEvent.SeverityLevels severity, string description);
        List<ApplicationEvent> GetEventsByDateDescriptions(DateTime startDate, DateTime endDate,
            List<String> descriptions);

    }
}

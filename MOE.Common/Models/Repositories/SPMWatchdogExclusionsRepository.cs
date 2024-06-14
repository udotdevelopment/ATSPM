using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SPMWatchdogExclusionsRepository : ISPMWatchdogExclusionsRepository
    {
        private readonly SPM _db = new SPM();

        public SPMWatchdogExclusionsRepository()
        {
        }

        public List<SPMWatchdogExclusions> GetSPMWatchdogExclusions()
        {
            var exclusions = _db.SPMWatchdogExclusions.OrderBy(x => (int)x.TypeOfAlert).ThenBy(x => x.SignalID).ToList();
            return exclusions;
        }

        public void Add(SPMWatchdogExclusions exclusion)
        {
            try
            {
                _db.SPMWatchdogExclusions.Add(exclusion);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "Models.Repository.SPMWatchdogExclusionsRepository",
                    Function = "Add",
                    Description = ex.Message,
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Timestamp = DateTime.Now
                };
                repository.Add(error);
            }
        }

        public void Delete(int exclusion)
        {
            try
            {
                var exclusionItem = _db.SPMWatchdogExclusions.FirstOrDefault(x => x.ID == exclusion);
                if (exclusionItem != null)
                {
                    _db.SPMWatchdogExclusions.Remove(exclusionItem);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var repository =
                        ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "Models.Repository.SPMWatchdogExclusionsRepository",
                    Function = "Delete",
                    Description = ex.Message,
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Timestamp = DateTime.Now
                };
                repository.Add(error);
            }
        }
    }
}

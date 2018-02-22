using System.Data.Entity;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private SPM db = new SPM();

        public ApplicationSettingsRepository()
        {
        }

        public ApplicationSettingsRepository(SPM context)
        {
            db = context;
        }

        public WatchDogApplicationSettings GetWatchDogSettings()
        {
            return db.WatchdogApplicationSettings.First();
        }

        public GeneralSettings GetGeneralSettings()
        {
            return db.GeneralSettings.First();
        }

        public void Save(WatchDogApplicationSettings watchDogApplicationSettings)
        {
            db.Entry(watchDogApplicationSettings).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Save(GeneralSettings generalSettings)
        {
            db.Entry(generalSettings).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SetApplicationSettingsRepository(SPM context)
        {
            db = context;
        }
    }
}
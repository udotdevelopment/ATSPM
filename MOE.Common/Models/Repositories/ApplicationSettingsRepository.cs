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

        public DatabaseArchiveSettings GetDatabaseArchiveSettings()
        {
            var archiveSettings = db.DatabaseArchiveSettings.First();
            return archiveSettings;
        }

        public GeneralSettings GetGeneralSettings()
        {
            return db.GeneralSettings.First();
        }

        public int GetRawDataLimit()
        {
            GeneralSettings gs = GetGeneralSettings();
            int limit = 0;
            if (gs.RawDataCountLimit != null)
            {
                limit = (int) gs.RawDataCountLimit;
            }
            return limit;
        }

        public void Save(DatabaseArchiveSettings databaseArchiveSettings)
        {
            db.Entry(databaseArchiveSettings).State = EntityState.Modified;
            db.SaveChanges();
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
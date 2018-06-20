using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryApplicationSettingsRepository: IApplicationSettingsRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApplicationSettingsRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemoryApplicationSettingsRepository()
        {
            this._db = new InMemoryMOEDatabase();
        }

        public WatchDogApplicationSettings GetWatchDogSettings()
        {
            WatchDogApplicationSettings record = _db.ApplicationSettings.Find(r => r.ApplicationID == 1);
            return record;

        }

        public void Save(WatchDogApplicationSettings watchDogApplicationSettings)
        {
            WatchDogApplicationSettings record = _db.ApplicationSettings.Find(r => r.ApplicationID == watchDogApplicationSettings.ApplicationID);
            if (record != null)
            {
                _db.ApplicationSettings.Remove(record);
            }

            _db.ApplicationSettings.Add(watchDogApplicationSettings);
        }
    }
}
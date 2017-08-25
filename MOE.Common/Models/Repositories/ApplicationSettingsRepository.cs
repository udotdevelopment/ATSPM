using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        Models.SPM db = new Models.SPM();

        public ApplicationSettingsRepository()
        {
        }
        public ApplicationSettingsRepository(Models.SPM context)
        {
            db = context;
        }
        public void SetApplicationSettingsRepository(Models.SPM context)
        {
            db = context;
        }
        public Models.WatchDogApplicationSettings GetWatchDogSettings()
        {
            return db.WatchdogApplicationSettings.First();
        }
        public void Save(Models.WatchDogApplicationSettings watchDogApplicationSettings)
        {
            db.Entry(watchDogApplicationSettings).State = EntityState.Modified;
                db.SaveChanges();
        }
        

    }
}

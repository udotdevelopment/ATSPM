using ATSPM.IRepositories;
using ATSPM.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        //private MOEContext db;

        //public ApplicationSettingsRepository(MOEContext context)
        //{
        //    db = context;
        //}

        //public WatchDogApplicationSettings GetWatchDogSettings()
        //{
        //    var watchDogApplicationSettings = db.WatchdogApplicationSettings.First();
        //    return watchDogApplicationSettings;
        //}

        //public DatabaseArchiveSettings GetDatabaseArchiveSettings()
        //{
        //    var archiveSettings = db.DatabaseArchiveSettings.First();
        //    return archiveSettings;
        //}

        //public GeneralSettings GetGeneralSettings()
        //{
        //    return db.GeneralSettings.First();
        //}

        //public int GetRawDataLimit()
        //{
        //    GeneralSettings gs = GetGeneralSettings();
        //    int limit = 0;
        //    if (gs.RawDataCountLimit != null)
        //    {
        //        limit = (int)gs.RawDataCountLimit;
        //    }
        //    return limit;
        //}

        //public void Save(DatabaseArchiveSettings databaseArchiveSettings)
        //{
        //    db.Entry(databaseArchiveSettings).State = EntityState.Modified;
        //    db.SaveChanges();
        //}

        //public void Save(WatchDogApplicationSettings watchDogApplicationSettings)
        //{
        //    db.Entry(watchDogApplicationSettings).State = EntityState.Modified;
        //    db.SaveChanges();
        //}

        //public void Save(GeneralSettings generalSettings)
        //{
        //    db.Entry(generalSettings).State = EntityState.Modified;
        //    db.SaveChanges();
        //}

        //public void SetApplicationSettingsRepository(MOEContext context)
        //{
        //    db = context;
        //}
    }
}
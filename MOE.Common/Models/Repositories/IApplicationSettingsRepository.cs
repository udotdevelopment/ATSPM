namespace MOE.Common.Models.Repositories
{
    public interface IApplicationSettingsRepository
    {
        WatchDogApplicationSettings GetWatchDogSettings();
        GeneralSettings GetGeneralSettings();
        DatabaseArchiveSettings GetDatabaseArchiveSettings();
        void Save(WatchDogApplicationSettings watchDogApplicationSettings);
        void Save(GeneralSettings generalSettings);
        void Save(DatabaseArchiveSettings databaseArchiveSettings);
        int GetRawDataLimit();

    }
}
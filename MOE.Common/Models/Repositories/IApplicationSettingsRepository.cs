namespace MOE.Common.Models.Repositories
{
    public interface IApplicationSettingsRepository
    {
        WatchDogApplicationSettings GetWatchDogSettings();
        GeneralSettings GetGeneralSettings();
        void Save(WatchDogApplicationSettings watchDogApplicationSettings);
        void Save(GeneralSettings generalSettings);
    }
}
using AutoMapper;

namespace SPM.Models
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            var sharedProfiles = SharedViewModelMappings.GetProfiles();
            Mapper.Initialize(cfg =>
            {
                sharedProfiles.ForEach(x => cfg.AddProfile(x));
                cfg.AddProfile(new SignalProfile());
                cfg.AddProfile(new ApproachProfile());
                cfg.AddProfile(new DetectorProfile());
                cfg.AddProfile(new LookupDataProfile());
            });
        }
    }
}
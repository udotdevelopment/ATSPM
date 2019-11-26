namespace AtspmApi.Repositories
{
    public class RegionsRepositoryFactory
    {
        private static IRegionsRepository regionRepository;

        public static IRegionsRepository Create()
        {
            if (regionRepository != null)
                return regionRepository;
            return new RegionsRepository();
        }

        public static void SetArchivedMetricsRepository(IRegionsRepository newRepository)
        {
            regionRepository = newRepository;
        }
    }
}
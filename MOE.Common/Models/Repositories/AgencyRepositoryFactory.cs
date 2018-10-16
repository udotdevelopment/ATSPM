namespace MOE.Common.Models.Repositories
{
    public class AgencyRepositoryFactory
    {
        private static IGenericRepository<ATSPM_Agency> genericRepository;

        public static IGenericRepository<ATSPM_Agency> Create()
        {
            if (genericRepository != null)
                return genericRepository;
            return new AgencyRepository();
        }

        public static void SetMetricsRepository(IGenericRepository<ATSPM_Agency> newRepository)
        {
            genericRepository = newRepository;
        }
    }
}
namespace MOE.Common.Models.Repositories
{
    public class AgencyRepositoryFactory
    {
        private static IGenericRepository<Agency> genericRepository;

        public static IGenericRepository<Agency> Create()
        {
            if (genericRepository != null)
                return genericRepository;
            return new AgencyRepository();
        }

        public static void SetMetricsRepository(IGenericRepository<Agency> newRepository)
        {
            genericRepository = newRepository;
        }
    }
}
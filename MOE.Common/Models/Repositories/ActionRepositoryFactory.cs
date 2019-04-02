namespace MOE.Common.Models.Repositories
{
    public class ActionRepositoryFactory
    {
        private static IGenericRepository<Action> genericRepository;

        public static IGenericRepository<Action> Create()
        {
            if (genericRepository != null)
                return genericRepository;
            return new ActionRepository();
        }

        public static void SetMetricsRepository(IGenericRepository<Action> newRepository)
        {
            genericRepository = newRepository;
        }
    }
}
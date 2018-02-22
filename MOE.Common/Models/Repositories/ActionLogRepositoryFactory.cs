namespace MOE.Common.Models.Repositories
{
    public class ActionLogRepositoryFactory
    {
        private static IActionLogRepository genericRepository;

        public static IActionLogRepository Create()
        {
            if (genericRepository != null)
                return genericRepository;
            return new ActionLogRepository();
        }

        public static void SetMetricsRepository(IActionLogRepository newRepository)
        {
            genericRepository = newRepository;
        }
    }
}
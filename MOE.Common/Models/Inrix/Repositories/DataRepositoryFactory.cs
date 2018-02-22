namespace MOE.Common.Models.Inrix.Repositories
{
    public class DataRepositoryFactory
    {
        private static IDataRepository dataRepository;

        public static IDataRepository CreatedataRepository()
        {
            if (dataRepository != null)
                return dataRepository;
            return new DataRepository();
        }

        public static void SetArchivedMetricsRepository(IDataRepository newRepository)
        {
            dataRepository = newRepository;
        }
    }
}
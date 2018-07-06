namespace MOE.Common.Models.Repositories
{
    public class SPMMenuRepositoryFactory
    {
        private static ISPMMenuRepository spmMenuRepository;

        public static ISPMMenuRepository Create()
        {
            if (spmMenuRepository != null)
                return spmMenuRepository;
            return new SPMMenuRepository();
        }

        public static void SetArchivedMetricsRepository(ISPMMenuRepository newRepository)
        {
            spmMenuRepository = newRepository;
        }
    }
}
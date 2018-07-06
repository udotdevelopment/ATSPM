namespace MOE.Common.Models.Repositories
{
    public class ApplicationEventRepositoryFactory
    {
        private static IApplicationEventRepository applicationEventRepository;

        public static IApplicationEventRepository Create()
        {
            if (applicationEventRepository != null)
                return applicationEventRepository;
            return new ApplicationEventRepository();
        }

        public static void SetApplicationEventRepository(IApplicationEventRepository newRepository)
        {
            applicationEventRepository = newRepository;
        }
    }
}
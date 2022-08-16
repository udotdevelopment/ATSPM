namespace MOE.Common.Models.Repositories
{
    public class AreaRepositoryFactory
    {
        private static IAreaRepository AreaRepository;

        public static IAreaRepository Create()
        {
            if (AreaRepository != null)
                return AreaRepository;
            return new AreaRepository();
        }

        public static void SetAreaRepository(IAreaRepository newRepository)
        {
            AreaRepository = newRepository;
        }
    }
}

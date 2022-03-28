namespace MOE.Common.Models.Repositories
{
    public class JurisdictionRepositoryFactory
    {
        private static IJurisdictionRepository jurisdictionRepository;

        public static IJurisdictionRepository Create()
        {
            if (jurisdictionRepository != null)
                return jurisdictionRepository;
            return new JurisdictionRepository();
        }

        public static void SetJurisdictionRepository(IJurisdictionRepository newRepository)
        {
            jurisdictionRepository = newRepository;
        }
    }
}
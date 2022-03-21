namespace MOE.Common.Models.Repositories
{
    public class JurisdictionRepositoryFactory
    {
        private static IJurisdictionRepository approachJurisdictionRepository;

        public static IJurisdictionRepository Create()
        {
            if (approachJurisdictionRepository != null)
                return approachJurisdictionRepository;
            return new JurisdictionRepository();
        }

        public static void SetApproachRouteRepository(IJurisdictionRepository newRepository)
        {
            approachJurisdictionRepository = newRepository;
        }
    }
}
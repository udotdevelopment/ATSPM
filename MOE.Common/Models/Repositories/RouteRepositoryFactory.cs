namespace MOE.Common.Models.Repositories
{
    public class RouteRepositoryFactory
    {
        private static IRouteRepository approachRouteRepository;

        public static IRouteRepository Create()
        {
            if (approachRouteRepository != null)
                return approachRouteRepository;
            return new RouteRepository();
        }

        public static void SetApproachRouteRepository(IRouteRepository newRepository)
        {
            approachRouteRepository = newRepository;
        }
    }
}
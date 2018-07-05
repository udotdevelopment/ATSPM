namespace MOE.Common.Models.Repositories
{
    public class RouteSignalsRepositoryFactory
    {
        private static IRouteSignalsRepository approachRouteDetailRepository;

        public static IRouteSignalsRepository Create()
        {
            if (approachRouteDetailRepository != null)
                return approachRouteDetailRepository;
            return new RouteSignalsRepository();
        }

        public static void SetApproachRouteDetailRepository(IRouteSignalsRepository newRepository)
        {
            approachRouteDetailRepository = newRepository;
        }
    }
}
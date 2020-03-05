namespace AtspmApi.Repositories
{
    public class ApproachSpeedAggregationRepositoryFactory
    {
        private static IApproachSpeedAggregationRepository _approachSpeedAggregationRepository;

        public static IApproachSpeedAggregationRepository Create()
        {
            if (_approachSpeedAggregationRepository != null)
                return _approachSpeedAggregationRepository;
            return new ApproachSpeedAggregationRepository();
        }

        public static void SetApplicationEventRepository(IApproachSpeedAggregationRepository newRepository)
        {
            _approachSpeedAggregationRepository = newRepository;
        }
    }
}
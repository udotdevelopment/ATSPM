namespace AtspmApi.Repositories
{
    public class ApproachCycleAggregationRepositoryFactory
    {
        private static IApproachCycleAggregationRepository _approachCycleAggregationRepository;

        public static IApproachCycleAggregationRepository Create()
        {
            if (_approachCycleAggregationRepository != null)
                return _approachCycleAggregationRepository;
            return new ApproachCycleAggregationRepository();
        }

        public static void SetApplicationEventRepository(IApproachCycleAggregationRepository newRepository)
        {
            _approachCycleAggregationRepository = newRepository;
        }
    }
}
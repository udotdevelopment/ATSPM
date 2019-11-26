namespace AtspmApi.Repositories
{
    public class ApproachPcdAggregationRepositoryFactory
    {
        private static IApproachPcdAggregationRepository _approachPcdAggregationRepository;

        public static IApproachPcdAggregationRepository Create()
        {
            if (_approachPcdAggregationRepository != null)
                return _approachPcdAggregationRepository;
            return new ApproachPcdAggregationRepository();
        }

        public static void SetApplicationEventRepository(IApproachPcdAggregationRepository newRepository)
        {
            _approachPcdAggregationRepository = newRepository;
        }
    }
}
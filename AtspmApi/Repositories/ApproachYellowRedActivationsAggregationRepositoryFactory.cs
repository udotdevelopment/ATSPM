namespace AtspmApi.Repositories
{
    public class ApproachYellowRedActivationsAggregationRepositoryFactory
    {
        private static IApproachYellowRedActivationsAggregationRepository
            _approachYellowRedActivationsAggregationRepository;

        public static IApproachYellowRedActivationsAggregationRepository Create()
        {
            if (_approachYellowRedActivationsAggregationRepository != null)
                return _approachYellowRedActivationsAggregationRepository;
            return new ApproachYellowRedActivationsAggregationRepository();
        }

        public static void SetApplicationEventRepository(
            IApproachYellowRedActivationsAggregationRepository newRepository)
        {
            _approachYellowRedActivationsAggregationRepository = newRepository;
        }
    }
}
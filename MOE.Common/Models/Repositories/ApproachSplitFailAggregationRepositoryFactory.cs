namespace MOE.Common.Models.Repositories
{
    public class ApproachSplitFailAggregationRepositoryFactory
    {
        private static IApproachSplitFailAggregationRepository _approachSplitFailAggregationRepository;

        public static IApproachSplitFailAggregationRepository Create()
        {
            if (_approachSplitFailAggregationRepository != null)
                return _approachSplitFailAggregationRepository;
            return new ApproachSplitFailAggregationRepository();
        }

        public static void SetApplicationEventRepository(IApproachSplitFailAggregationRepository newRepository)
        {
            _approachSplitFailAggregationRepository = newRepository;
        }
    }
}
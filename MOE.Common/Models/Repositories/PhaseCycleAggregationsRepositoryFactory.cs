namespace MOE.Common.Models.Repositories
{
    public class PhaseCycleAggregationsRepositoryFactory
    {
        private static IPhaseCycleAggregationRepository _approachCycleAggregationRepository;

        public static IPhaseCycleAggregationRepository Create()
        {
            if (_approachCycleAggregationRepository != null)
                return _approachCycleAggregationRepository;
            return new PhaseCycleAggregationsRepository();
        }

        public static void SetApplicationEventRepository(IPhaseCycleAggregationRepository newRepository)
        {
            _approachCycleAggregationRepository = newRepository;
        }
    }
}
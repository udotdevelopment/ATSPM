namespace AtspmApi.Repositories
{
    public class SignalEventCountAggregationRepositoryFactory
    {
        private static ISignalEventCountAggregationRepository _approachCycleAggregationRepository;

        public static ISignalEventCountAggregationRepository Create()
        {
            if (_approachCycleAggregationRepository != null)
                return _approachCycleAggregationRepository;
            return new SignalEventCountAggregationRepository();
        }

        public static void SetRepository(ISignalEventCountAggregationRepository newRepository)
        {
            _approachCycleAggregationRepository = newRepository;
        }
    }
}
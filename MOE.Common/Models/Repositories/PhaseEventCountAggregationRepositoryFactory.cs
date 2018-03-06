namespace MOE.Common.Models.Repositories
{
    public class PhaseEventCountAggregationRepositoryFactory
    {
        private static IPhaseEventCountAggregationRepository _phaseCycleAggregationRepository;

        public static IPhaseEventCountAggregationRepository Create()
        {
            if (_phaseCycleAggregationRepository != null)
                return _phaseCycleAggregationRepository;
            return new PhaseEventCountAggregationRepository();
        }

        public static void SetRepository(IPhaseEventCountAggregationRepository newRepository)
        {
            _phaseCycleAggregationRepository = newRepository;
        }
    }
}
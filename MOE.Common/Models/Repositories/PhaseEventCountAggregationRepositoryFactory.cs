namespace MOE.Common.Models.Repositories
{
    public class ApproachEventCountAggregationRepositoryFactory
    {
        private static IApproachEventCountAggregationRepository _phaseCycleAggregationRepository;

        public static IApproachEventCountAggregationRepository Create()
        {
            if (_phaseCycleAggregationRepository != null)
                return _phaseCycleAggregationRepository;
            return new ApproachEventCountAggregationRepository();
        }

        public static void SetRepository(IPhaseEventCountAggregationRepository newRepository)
        {
            _phaseCycleAggregationRepository = newRepository;
        }
    }
}
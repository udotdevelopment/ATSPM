namespace MOE.Common.Models.Repositories
{
    public class PhaseSplitMonitorAggregationRepositoryFactory
    {
        private static IPhaseSplitMonitorAggregationRepository _repository;

        public static IPhaseSplitMonitorAggregationRepository Create()
        {
            if (_repository != null)
                return _repository;
            return new PhaseSplitMonitorAggregationRepository();
        }

        public static void SetApplicationEventRepository(IPhaseSplitMonitorAggregationRepository newRepository)
        {
            _repository = newRepository;
        }
    }
}
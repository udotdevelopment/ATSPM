namespace MOE.Common.Models.Repositories
{
    public class PreemptAggregationDatasRepositoryFactory
    {
        private static IPreemptAggregationDatasRepository _preemptAggregationDatasRepository;

        public static IPreemptAggregationDatasRepository Create()
        {
            if (_preemptAggregationDatasRepository != null)
                return _preemptAggregationDatasRepository;
            return new PreemptAggregationDatasRepository();
        }

        public static void SetArchivedMetricsRepository(IPreemptAggregationDatasRepository newRepository)
        {
            _preemptAggregationDatasRepository = newRepository;
        }
    }
}
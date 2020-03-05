namespace AtspmApi.Repositories
{
    public class PriorityAggregationDatasRepositoryFactory
    {
        private static IPriorityAggregationDatasRepository _priorityAggregationDatasRepository;

        public static IPriorityAggregationDatasRepository Create()
        {
            if (_priorityAggregationDatasRepository != null)
                return _priorityAggregationDatasRepository;
            return new PriorityAggregationDatasRepository();
        }

        public static void SetRepository(IPriorityAggregationDatasRepository newRepository)
        {
            _priorityAggregationDatasRepository = newRepository;
        }
    }
}
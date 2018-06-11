namespace MOE.Common.Models.Repositories
{
    public class PhasePedAggregationRepositoryFactory
    {
        private static IPhasePedAggregationRepository _repository;

        public static IPhasePedAggregationRepository Create()
        {
            if (_repository != null)
                return _repository;
            return new PhasePedAggregationRepository();
        }

        public static void SetApplicationEventRepository(IPhasePedAggregationRepository newRepository)
        {
            _repository = newRepository;
        }
    }
}
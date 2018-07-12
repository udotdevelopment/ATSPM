namespace MOE.Common.Models.Repositories
{
    public class PhaseTerminationAggregationRepositoryFactory
    {
        private static IPhaseTerminationAggregationRepository _repository;

        public static IPhaseTerminationAggregationRepository Create()
        {
            if (_repository != null)
                return _repository;
            return new PhaseTerminationAggregationRepository();
        }

        public static void SetApplicationEventRepository(IPhaseTerminationAggregationRepository newRepository)
        {
            _repository = newRepository;
        }
    }
}
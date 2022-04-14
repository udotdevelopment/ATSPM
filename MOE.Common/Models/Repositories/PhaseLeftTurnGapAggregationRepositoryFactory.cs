namespace MOE.Common.Models.Repositories
{
    public class PhaseLeftTurnGapAggregationRepositoryFactory
    {
        private static IPhaseLeftTurnGapAggregationRepository _repository;

        public static IPhaseLeftTurnGapAggregationRepository Create()
        {
            if (_repository != null)
                return _repository;
            return new PhaseLeftTurnGapAggregationRepository();
        }

        public static void SetRepository(IPhaseLeftTurnGapAggregationRepository newRepository)
        {
            _repository = newRepository;
        }
    }
}
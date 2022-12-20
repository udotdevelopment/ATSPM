namespace MOE.Common.Models.Repositories
{
    public class SignalPlanAggregationRepositoryFactory
    {
        private static ISignalPlanAggregationRepository _signalPlanAggregationRepository;

        public static ISignalPlanAggregationRepository Create()
        {
            if (_signalPlanAggregationRepository != null)
                return _signalPlanAggregationRepository;
            return new SignalPlanAggregationRepository();
        }

        public static void SetRepository(ISignalPlanAggregationRepository newRepository)
        {
            _signalPlanAggregationRepository = newRepository;
        }
    }
}
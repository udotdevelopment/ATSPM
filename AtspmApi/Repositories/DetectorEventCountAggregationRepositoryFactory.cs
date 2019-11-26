namespace AtspmApi.Repositories
{
    public class DetectorEventCountAggregationRepositoryFactory
    {
        private static IDetectorEventCountAggregationRepository _detectorAggregationRepository;

        public static IDetectorEventCountAggregationRepository Create()
        {
            if (_detectorAggregationRepository != null)
                return _detectorAggregationRepository;
            return new DetectorEventCountAggregationRepository();
        }

        public static void SetApplicationEventRepository(IDetectorEventCountAggregationRepository newRepository)
        {
            _detectorAggregationRepository = newRepository;
        }
    }
}
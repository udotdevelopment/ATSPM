namespace MOE.Common.Models.Repositories
{
    public class DetectorEventCountAggregationRepositoryFactory
    {
        private static IDetectorEventCountAggregationRepository _detectorAggregationRepository;
        private static SPM _db;

        public static IDetectorEventCountAggregationRepository Create()
        {
            if (_detectorAggregationRepository != null)
                return _detectorAggregationRepository;
            if (_db == null)
                return new DetectorEventCountAggregationRepository(_db);
            else
                return new DetectorEventCountAggregationRepository();
        }

        public static void SetApplicationEventRepository(IDetectorEventCountAggregationRepository newRepository)
        {
            _detectorAggregationRepository = newRepository;
        }

        public static void SetContext(SPM db)
        {
            _db = db;
        }
    }
}
namespace MOE.Common.Models.Repositories
{
    public class DetectorAggregationsRepositoryFactory
    {
        private static IDetectorAggregationsRepository detectorAggregationsRepository;

        public static IDetectorAggregationsRepository Create()
        {
            if (detectorAggregationsRepository != null)
            {
                return detectorAggregationsRepository;
            }
            return new DetectorAggregationsRepository();
        }

        public static void SetDetectorCommentRepository(IDetectorAggregationsRepository newRepository)
        {
            detectorAggregationsRepository = newRepository;
        }
    }
}

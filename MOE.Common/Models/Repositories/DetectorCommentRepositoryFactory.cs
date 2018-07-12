namespace MOE.Common.Models.Repositories
{
    public class DetectorCommentRepositoryFactory
    {
        private static IDetectorCommentRepository detectorCommentRepository;

        public static IDetectorCommentRepository Create()
        {
            if (detectorCommentRepository != null)
                return detectorCommentRepository;
            return new DetectorCommentRepository();
        }

        public static void SetDetectorCommentRepository(IDetectorCommentRepository newRepository)
        {
            detectorCommentRepository = newRepository;
        }
    }
}
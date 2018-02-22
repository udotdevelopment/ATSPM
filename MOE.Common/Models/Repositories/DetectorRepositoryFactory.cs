namespace MOE.Common.Models.Repositories
{
    public class DetectorRepositoryFactory
    {
        private static IDetectorRepository detectorRepository;

        public static IDetectorRepository Create()
        {
            if (detectorRepository != null)
                return detectorRepository;
            return new DetectorRepository();
        }

        public static IDetectorRepository Create(SPM context)
        {
            if (detectorRepository != null)
                return detectorRepository;
            return new DetectorRepository(context);
        }

        public static void SetDetectorRepository(IDetectorRepository newRepository)
        {
            detectorRepository = newRepository;
        }
    }
}
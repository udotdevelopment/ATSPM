namespace MOE.Common.Models.Repositories
{
    public class DetectionTypeRepositoryFactory
    {
        private static IDetectionTypeRepository detectionTypeRepository;

        public static IDetectionTypeRepository Create()
        {
            if (detectionTypeRepository != null)
                return detectionTypeRepository;
            return new DetectionTypeRepository();
        }

        public static IDetectionTypeRepository Create(SPM context)
        {
            if (detectionTypeRepository != null)
                return detectionTypeRepository;
            return new DetectionTypeRepository(context);
        }

        public static void SetDetectionTypeRepository(IDetectionTypeRepository newRepository)
        {
            detectionTypeRepository = newRepository;
        }
    }
}
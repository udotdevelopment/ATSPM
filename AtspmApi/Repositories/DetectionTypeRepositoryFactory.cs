namespace AtspmApi.Repositories
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

        public static IDetectionTypeRepository Create(Models.AtspmApi context)
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
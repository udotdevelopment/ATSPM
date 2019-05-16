namespace MOE.Common.Models.Repositories
{
    public class ControllerEventLogRepositoryFactory
    {
        private static IControllerEventLogRepository controllerEventLogRepository;

        public static IControllerEventLogRepository Create()
        {
            if (controllerEventLogRepository != null)
            {
                return controllerEventLogRepository;
            }
            else
            {
                return new ControllerEventLogRepository();
            }
        }

        public static void SetRepository(IControllerEventLogRepository newRepository)
        {
            controllerEventLogRepository = newRepository;
        }
    }
}
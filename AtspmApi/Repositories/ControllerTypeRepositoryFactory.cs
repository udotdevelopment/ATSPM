namespace AtspmApi.Repositories
{
    public class ControllerTypeRepositoryFactory
    {
        private static IControllerTypeRepository ControllerTypeRepository;

        public static IControllerTypeRepository Create()
        {
            if (ControllerTypeRepository != null)
                return ControllerTypeRepository;
            return new ControllerTypeRepository();
        }

        public static void SetControllerTypeRepository(IControllerTypeRepository newRepository)
        {
            ControllerTypeRepository = newRepository;
        }
    }
}
namespace AtspmApi.Repositories
{
    public class MovementTypeRepositoryFactory
    {
        private static IMovementTypeRepository movementTypeRepository;

        public static IMovementTypeRepository Create()
        {
            if (movementTypeRepository != null)
                return movementTypeRepository;
            return new MovementTypeRepository();
        }

        public static void SetMovementTypeRepository(IMovementTypeRepository newRepository)
        {
            movementTypeRepository = newRepository;
        }
    }
}
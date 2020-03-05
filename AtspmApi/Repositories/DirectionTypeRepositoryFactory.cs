using AtspmApi.Models;

namespace AtspmApi.Repositories

{
    public class DirectionTypeRepositoryFactory
    {
        private static IDirectionTypeRepository DirectionTypeRepository;

        public static IDirectionTypeRepository Create()
        {
            if (DirectionTypeRepository != null)
                return DirectionTypeRepository;
            return new DirectionTypeRepository();
        }

        public static void SetDirectionsRepository(IDirectionTypeRepository newRepository)
        {
            DirectionTypeRepository = newRepository;
        }
    }
}
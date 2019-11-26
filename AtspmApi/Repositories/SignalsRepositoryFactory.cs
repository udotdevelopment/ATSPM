namespace AtspmApi.Repositories
{
    public class SignalsRepositoryFactory
    {
        private static ISignalsRepository signalsRepository;

        public static ISignalsRepository Create()
        {
            if (signalsRepository != null)
                return signalsRepository;
            return new SignalsRepository();
        }

        public static ISignalsRepository Create(Models.AtspmApi context)
        {
            if (signalsRepository != null)
                return signalsRepository;
            return new SignalsRepository(context);
        }

        public static void SetSignalsRepository(ISignalsRepository newRepository)
        {
            signalsRepository = newRepository;
        }
    }
}
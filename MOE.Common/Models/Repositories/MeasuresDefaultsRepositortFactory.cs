namespace MOE.Common.Models.Repositories
{
    public class MeasuresDefaultsRepositoryFactory
    {
        private static IMeasuresDefaultsRepository measuresDefaultsRepository;

        public static IMeasuresDefaultsRepository Create()
        {
            if (measuresDefaultsRepository != null)
                return measuresDefaultsRepository;
            return new MeasuresDefaultsRepository();
        }

        public static IMeasuresDefaultsRepository Create(SPM context)
        {
            if (measuresDefaultsRepository != null)
                return measuresDefaultsRepository;
            return new MeasuresDefaultsRepository(context);
        }

        public static void SetMeasuresDefaultsRepository(IMeasuresDefaultsRepository newRepository)
        {
            measuresDefaultsRepository = newRepository;
        }
    }
}
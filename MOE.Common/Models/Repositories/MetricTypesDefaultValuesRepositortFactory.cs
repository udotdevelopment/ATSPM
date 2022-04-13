namespace MOE.Common.Models.Repositories
{
    public class MetricTypesDefaultValuesRepositoryFactory
    {
        private static IMetricTypesDefaultValuesRepository metricTypesDefaultValuesRepository;

        public static IMetricTypesDefaultValuesRepository Create()
        {
            if (metricTypesDefaultValuesRepository != null)
                return metricTypesDefaultValuesRepository;
            return new MetricTypesDefaultValuesRepository();
        }

        public static IMetricTypesDefaultValuesRepository Create(SPM context)
        {
            if (metricTypesDefaultValuesRepository != null)
                return metricTypesDefaultValuesRepository;
            return new MetricTypesDefaultValuesRepository(context);
        }

        public static void SetMetricTypesDefaultValuesRepository(IMetricTypesDefaultValuesRepository newRepository)
        {
            metricTypesDefaultValuesRepository = newRepository;
        }
    }
}
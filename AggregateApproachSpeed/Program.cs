using MOE.Common.Models.Repositories;

namespace AggregateApproachSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = ApproachSpeedAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachSpeed(args, repository);
        }
    }
}

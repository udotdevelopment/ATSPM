using MOE.Common.Models.Repositories;

namespace AggregateApproachSplitFail
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = ApproachSplitFailAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachSplitFail(args, repository);
        }
    }
}

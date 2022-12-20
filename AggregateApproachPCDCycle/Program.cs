using MOE.Common.Models.Repositories;

namespace AggregateApproachPCDCycle
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = ApproachPcdAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachSignalPhase(args, repository);
        }
    }
}

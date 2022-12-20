using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateSignalPlan
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = MOE.Common.Models.Repositories.SignalPlanAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationSignalPlan(args, repository);
        }
    }
}

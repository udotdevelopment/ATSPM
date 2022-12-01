using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateApproachCycle
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = PhaseCycleAggregationsRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachCycle(args, repository);
        }
    }
}

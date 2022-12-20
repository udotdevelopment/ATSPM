using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateYellowRedActivation
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = ApproachYellowRedActivationsAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachYellowRedActivation(args, repository);
        }
    }
}

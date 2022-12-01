using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateSignalPreemptPriority
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = MOE.Common.Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationSignalPreemptPriority(args, repository);
        }
    }
}

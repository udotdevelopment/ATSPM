using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateSignalEventData
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = SignalEventCountAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationSignalEventData(args, repository);
        }
    }
}

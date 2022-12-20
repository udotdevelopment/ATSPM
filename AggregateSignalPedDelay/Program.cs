using MOE.Common.Migrations;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateSignalPedDelay
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = PhasePedAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationSignalPedDelay(args, repository);
        }
    }
}

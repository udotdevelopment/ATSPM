using MOE.Common.Business;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateSplitMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = PhaseSplitMonitorAggregationRepositoryFactory.Create();
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationSplitMonitor(args, repository);
        }
    }
}

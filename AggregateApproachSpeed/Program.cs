using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateApproachSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregationApproachSpeed(args);
        }
    }
}

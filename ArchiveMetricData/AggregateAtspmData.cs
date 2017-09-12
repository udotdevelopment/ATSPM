using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace AggregateATSPMData
{
 


    class AggregateAtspmData
    {
        public static DateTime ToNearestQuarterHour(DateTime input)
        {
            int i = (int)(Math.Round(input.Minute / 15D) * 15);
            if (i == 60)
            {
                return new DateTime(input.Year, input.Month, input.Day, input.Hour + 1, 0, 0);
            }
            else
            {
                return new DateTime(input.Year, input.Month, input.Day, input.Hour, (int)(Math.Round(input.Minute / 15D) * 15), 0);
            }
        }
        
        static void Main(string[] args)
        {
            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartAggregation(args);
        }

        
    }
}

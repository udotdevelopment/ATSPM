using System;
using System.Configuration;

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
            dataAggregation.StartAggregationSignalEventData(args);
        }
        
    }
}

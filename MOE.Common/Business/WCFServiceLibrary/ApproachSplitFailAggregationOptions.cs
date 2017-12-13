using System;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using static MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class ApproachSplitFailAggregationOptions: AggregationMetricOptions
    {
       

        

        public  void ApproachSplitFailAggregationOption()
        {
      
            if (this.AggregationOpperation == AggregationOpperations.Sum)
            {
                foreach (var appr in Approaches)
                {


                    Series splitFailSeries = new Series();

                    DateTime reportStart = StartDate;


                    //var sum = aggregations.Sum(r => r.ForceOffs);

                    //while (reportStart < EndDate)
                    //{
                    //    var binValues = (from r in aggregations
                    //        where r.BinStartTime <= reportStart && r.BinStartTime > reportStart.AddMinutes(BinSize)
                    //        select r).ToList();

                    //    int sfSum = binValues.Sum(f => f.SplitFailures);

                    //    splitFailSeries.Points.AddXY(sfSum, reportStart);

                    //    reportStart = reportStart.AddMinutes(BinSize);
                    //}

                    

                }
            }
        }


    }


    }

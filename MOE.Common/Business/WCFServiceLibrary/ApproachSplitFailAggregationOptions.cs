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




                    

                }
            }
        }


    }


    }

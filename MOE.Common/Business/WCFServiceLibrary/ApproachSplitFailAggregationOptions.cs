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
       

        public IApproachSplitFailAggregationRepository Repo =  ApproachSplitFailAggregationRepositoryFactory.Create();

        public  void ApproachSplitFailAggregationOption()
        {
      
            if (this.AggregationOpperation == AggregationOpperations.Sum)
            {
                foreach (var appr in Approaches)
                {
                    List<ApproachSplitFailAggregation> aggregations =
                        Repo.GetApproachSplitFailAggregationByVersionIdAndDateRange(appr.VersionID,StartDate, EndDate);

                    Series s = new Series();

                    DateTime reportStart = StartDate;


                    var sum = aggregations.Sum(r => r.ForceOffs);

                   // s.Points.AddXY()


                }
            }
        }


    }


    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public abstract class AggregationByApproach
    {
        public double Total
        {
            get { return BinsContainers.Sum(c => c.SumValue); }
        }


        public Approach Approach { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        public int Average
        {
            get
            {
                if (BinsContainers.Count > 1)
                    return Convert.ToInt32(Math.Round(BinsContainers.Average(b => b.SumValue)));
                double numberOfBins = 0;
                foreach (var binsContainer in BinsContainers)
                    numberOfBins += binsContainer.Bins.Count;
                return numberOfBins > 0 ? Convert.ToInt32(Math.Round(Total / numberOfBins)) : 0;
            }
        }

        public AggregationByApproach(Approach approach, ApproachAggregationMetricOptions options, DateTime startDate,
            DateTime endDate, bool getProtectedPhase, AggregatedDataType dataType)
        {
            Approach = approach;
            BinsContainers = BinFactory.GetBins(options.TimeOptions);
        }

       
        

        protected abstract void LoadBins(Approach approach, ApproachAggregationMetricOptions options,
            bool getProtectedPhase, AggregatedDataType dataType);
        

    }
}
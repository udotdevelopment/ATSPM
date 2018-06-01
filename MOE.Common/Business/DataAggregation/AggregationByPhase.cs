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
    public abstract class AggregationByPhase
    {
        //protected List<ApproachEventCountAggregation> ApproachEventCountAggregations { get; set; }
        public double Total
        {
            get { return BinsContainers.Sum(c => c.SumValue); }
        }


        public int PhaseNumber { get; }
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

        public AggregationByPhase(Models.Signal signal, int phaseNumber, PhaseAggregationMetricOptions options, AggregatedDataType dataType)
        {
            BinsContainers = BinFactory.GetBins(options.TimeOptions);
            PhaseNumber = phaseNumber;
            //if (options.ShowEventCount)
            //{
            //    ApproachEventCountAggregations = GetApproachEventCountAggregations(options, approach, true);
            //    if (approach.PermissivePhaseNumber != null)
            //    {
            //        ApproachEventCountAggregations.AddRange(GetApproachEventCountAggregations(options, approach, false));
            //    }
            //}
            LoadBins(signal, phaseNumber, options, dataType);
        }

        //protected List<ApproachEventCountAggregation> GetApproachEventCountAggregations(ApproachAggregationMetricOptions options, Approach approach, bool getProtectedPhase)
        //{
        //    var approachEventCountAggregationRepository =
        //        MOE.Common.Models.Repositories.ApproachEventCountAggregationRepositoryFactory.Create();
        //    return
        //        approachEventCountAggregationRepository.GetPhaseEventCountAggregationByPhaseIdAndDateRange(
        //            Approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
        //}
        

        protected abstract void LoadBins(Models.Signal signal, int phaseNumber, PhaseAggregationMetricOptions options, AggregatedDataType dataType);
        

    }
}
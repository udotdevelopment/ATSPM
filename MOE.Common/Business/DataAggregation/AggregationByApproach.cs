using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{

    public abstract class AggregationByApproach
    {
        public Approach Approach { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        protected AggregationByApproach(Approach approach, BinFactoryOptions timeOptions, DateTime startDate, DateTime endDate, 
            bool getProtectedPhase, AggregatedDataType dataType)
        {
            BinsContainers = BinFactory.GetBins(timeOptions);
            Approach = approach;
            LoadBins(approach, startDate, endDate, getProtectedPhase, dataType);
        }

        protected abstract void LoadBins(Approach approach, DateTime startDate, DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType);
    }
}

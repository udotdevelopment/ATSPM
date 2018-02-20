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

    public abstract class AggregationByDetector
    {
        public Models.Detector Detector { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        protected AggregationByDetector(Models.Detector detector, BinFactoryOptions timeOptions, DateTime startDate, DateTime endDate, 
            AggregatedDataType dataType)
        {
            Detector = detector;
            BinsContainers = BinFactory.GetBins(timeOptions);
            LoadBins(detector, startDate, endDate, dataType);
        }

        protected abstract void LoadBins(Models.Detector detector, DateTime startDate, DateTime endDate,
            AggregatedDataType dataType);
    }
}

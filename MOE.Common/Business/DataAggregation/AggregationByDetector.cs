using System.Collections.Generic;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.DataAggregation
{
    public abstract class AggregationByDetector
    {
        protected AggregationByDetector(Models.Detector detector, DetectorAggregationMetricOptions options)
        {
            Detector = detector;
            BinsContainers = BinFactory.GetBins(options.TimeOptions);
            LoadBins(detector, options);
        }

        public Models.Detector Detector { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        protected abstract void LoadBins(Models.Detector detector, DetectorAggregationMetricOptions options);
    }
}
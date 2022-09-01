using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public abstract class AggregationByDetector
    {

        protected List<DetectorEventCountAggregation> DetectorEventCountAggregations { get; set; }
        protected AggregationByDetector(Models.Detector detector, DetectorAggregationMetricOptions options)
        {
            Detector = detector;
            BinsContainers = BinFactory.GetBins(options.TimeOptions);
            if (options.ShowEventCount)
            {
                DetectorEventCountAggregations = GetdetectorEventCountAggregations(options, detector);
            }
            LoadBins(detector, options);
        }

        public Models.Detector Detector { get; }
        public List<BinsContainer> BinsContainers { get; set; }

        protected List<DetectorEventCountAggregation> GetdetectorEventCountAggregations(DetectorAggregationMetricOptions options, Models.Detector detector)
        {
            var detectorEventCountAggregationRepository =
                MOE.Common.Models.Repositories.DetectorEventCountAggregationRepositoryFactory.Create();
            return
                detectorEventCountAggregationRepository.GetDetectorEventCountAggregationByDetectorIdAndDateRange(
                    Detector.ID, options.StartDate, options.EndDate);
        }

        public abstract void LoadBins(Models.Detector detector, DetectorAggregationMetricOptions options);
    }
}
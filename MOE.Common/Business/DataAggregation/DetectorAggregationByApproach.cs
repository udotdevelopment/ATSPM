using System;
using System.Collections.Generic;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class DetectorAggregationByApproach : AggregationByApproach
    {
        public DetectorAggregationByApproach(Approach approach, DetectorVolumeAggregationOptions options,
            bool getProtectedPhase) : base(approach, options.TimeOptions, options.StartDate, options.EndDate,
            getProtectedPhase, options.SelectedAggregatedDataType)
        {
            GetApproachDetectorVolumeAggregationContainersForAllDetectors(options, approach);
            LoadBins(approach, options.StartDate, options.EndDate, getProtectedPhase,
                options.SelectedAggregatedDataType);
        }

        public List<DetectorAggregationByDetector> detectorAggregationByDetectors { get; set; } =
            new List<DetectorAggregationByDetector>();

        protected override void LoadBins(Approach approach, DateTime startDate, DateTime endDate,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var detectorAggregationByDetector in detectorAggregationByDetectors)
                    bin.Sum += detectorAggregationByDetector.BinsContainers[i].Bins[binIndex].Sum;
                bin.Average = detectorAggregationByDetectors.Count > 0
                    ? bin.Sum / detectorAggregationByDetectors.Count
                    : 0;
            }
        }

        private void GetApproachDetectorVolumeAggregationContainersForAllDetectors(
            DetectorVolumeAggregationOptions options, Approach approach)
        {
            foreach (var detector in approach.Detectors)
                detectorAggregationByDetectors.Add(new DetectorAggregationByDetector(detector, options));
        }
    }
}
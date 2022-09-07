using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class DetectorAggregationByApproach : AggregationByApproach
    {
        public DetectorAggregationByApproach(Approach approach, DetectorVolumeAggregationOptions options,
            bool getProtectedPhase) : base(approach, options, options.StartDate, options.EndDate,
            getProtectedPhase, options.SelectedAggregatedDataType)
        {

            GetApproachDetectorVolumeAggregationContainersForAllDetectors(options, approach);
            LoadBins(approach, options, getProtectedPhase,
                options.SelectedAggregatedDataType);
        }

        public List<DetectorAggregationByDetector> DetectorAggregationByDetectors { get; set; } =
            new List<DetectorAggregationByDetector>();
        

        private void GetApproachDetectorVolumeAggregationContainersForAllDetectors(
            DetectorVolumeAggregationOptions options, Approach approach)
        {
            foreach (var detector in approach.Detectors)
                DetectorAggregationByDetectors.Add(new DetectorAggregationByDetector(detector, options));
        }

        protected override void LoadBins(Approach approach, ApproachAggregationMetricOptions options, bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            //var bins = DetectorAggregationByDetectors.SelectMany(b => b.BinsContainers.SelectMany(bc => bc.Bins)).ToList().GroupBy(g => g.Start, g => g.Sum, (key, a)=> new(Start = key, Sum = a.Sum().ToList();
            //var sum = bins.Sum(b => b.Sum);
            for (var i = 0; i < BinsContainers.Count; i++)
            {
                for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = BinsContainers[i].Bins[binIndex];
                    foreach (var detectorAggregationByDetector in DetectorAggregationByDetectors)
                        bin.Sum += detectorAggregationByDetector.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = DetectorAggregationByDetectors.Count > 0
                        ? bin.Sum / DetectorAggregationByDetectors.Count
                        : 0;
                }
            }
        }
    }
}
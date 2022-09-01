using System.Collections.Generic;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class DetectorAggregationBySignal : AggregationBySignal
    {
        public DetectorAggregationBySignal(DetectorVolumeAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            ApproachDetectorVolumes = new List<DetectorAggregationByApproach>();
            GetApproachDetectorVolumeAggregationContainersForAllApporaches(options, signal);
            LoadBins(null, null);
        }

        public DetectorAggregationBySignal(DetectorVolumeAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            ApproachDetectorVolumes = new List<DetectorAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachDetectorVolumes.Add(
                        new DetectorAggregationByApproach(approach, options, true));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                        ApproachDetectorVolumes.Add(new DetectorAggregationByApproach(approach, options, false));
                }
            LoadBins(null, null);
        }

        public DetectorAggregationBySignal(DetectorVolumeAggregationOptions options, Models.Signal signal,
            DirectionType direction) : base(options, signal)
        {
            ApproachDetectorVolumes = new List<DetectorAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachDetectorVolumes.Add(
                        new DetectorAggregationByApproach(approach, options, true));
                    if (approach.PermissivePhaseNumber != null)
                        ApproachDetectorVolumes.Add(
                            new DetectorAggregationByApproach(approach, options, false));
                }
            LoadBins(null, null);
        }

        public List<DetectorAggregationByApproach> ApproachDetectorVolumes { get; }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
                for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = BinsContainers[i].Bins[binIndex];
                    foreach (var detectorEventAggregationContainer in ApproachDetectorVolumes)
                    {
                        bin.Sum += detectorEventAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    }
                    bin.Average = ApproachDetectorVolumes.Count > 0 ? bin.Sum / ApproachDetectorVolumes.Count : 0;
                }
        }

        protected override void LoadBins(ApproachAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var detectorAggregationContainer in ApproachDetectorVolumes)
                {
                        if (detectorAggregationContainer.BinsContainers.Count > 0)
                        {
                            bin.Sum += detectorAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                        }
                }
                bin.Average = ApproachDetectorVolumes.Count > 0 ? bin.Sum / ApproachDetectorVolumes.Count : 0;
            }
        }

        private void GetApproachDetectorVolumeAggregationContainersForAllApporaches(
            DetectorVolumeAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachDetectorVolumes.Add(
                    new DetectorAggregationByApproach(approach, options, true));
                if (approach.PermissivePhaseNumber != null)
                    ApproachDetectorVolumes.Add(
                        new DetectorAggregationByApproach(approach, options, false));
            }
        }
    }
}
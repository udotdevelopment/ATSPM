using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.LeftTurnGapReport
{
    public static class LeftTurnVolumeAnalysis
    {
        public static LeftTurnVolumeValue GetLeftTurnVolumeStats(string signalId, int directionTypeId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime)
        {

            Dictionary<TimeSpan, int> peaks = LeftTurnReportPreCheck.GetAMPMPeakFlowRate(signalId, directionTypeId, start, end, new TimeSpan(6, 0, 0), new TimeSpan(9, 0, 0),
                new TimeSpan(15, 0, 0), new TimeSpan(18, 0, 0));
            Dictionary<TimeSpan, int> peaksToUse = new Dictionary<TimeSpan, int>();
            if (peaks.Keys.First() >= startTime && peaks.Keys.First() <= endTime)
                peaksToUse.Add(peaks.First().Key, 0);
            if (peaks.Keys.Last() >= startTime && peaks.Keys.Last() <= endTime)
                peaksToUse.Add(peaks.Last().Key, 0);
            if (peaks.Count == 0)
                throw new NotSupportedException("Peak hours must be included in the selected time range");
            LeftTurnVolumeValue leftTurnVolumeValue = new LeftTurnVolumeValue();
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(signalId, directionTypeId);
            var approach = LeftTurnReportPreCheck.GetLTPhaseNumberPhaseTypeByDirection(signalId, directionTypeId);
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            var opposingLanes = GetDetectorsByPhase(signalId, opposingPhase);
            leftTurnVolumeValue.OpposingLanes = opposingLanes.Count;
            List<Models.DetectorEventCountAggregation> leftTurnVolumeAggregation = GetDetectorVolumebyDetector(detectors, start, end, startTime, endTime);
            List<Models.DetectorEventCountAggregation> opposingVolumeAggregations = GetDetectorVolumebyDetector(opposingLanes, start, end, startTime, endTime);
            double leftTurnVolume = leftTurnVolumeAggregation.Sum(l => l.EventCount);
            double opposingVolume = opposingVolumeAggregations.Sum(o => o.EventCount);
            double crossVolumeProduct = leftTurnVolume * opposingVolume;
            SetCrossProductReview(leftTurnVolumeValue, crossVolumeProduct);
            ApproachType approachType = GetApproachType(approach);
            SetDecisionBoundariesReview(leftTurnVolumeValue, leftTurnVolume, opposingVolume, approachType);
            return leftTurnVolumeValue;
        }

        private static void SetCrossProductReview(LeftTurnVolumeValue leftTurnVolumeValue, double crossVolumeProduct)
        {
            if (leftTurnVolumeValue.OpposingLanes == 1)
            {
                leftTurnVolumeValue.CrossProductReview = crossVolumeProduct > 50000;
            }
            else
            {
                leftTurnVolumeValue.CrossProductReview = crossVolumeProduct > 100000;
            }
        }

        private static void SetDecisionBoundariesReview(LeftTurnVolumeValue leftTurnVolumeValue, double leftTurnVolume, double opposingVolume, ApproachType approachType)
        {
            switch (approachType)
            {
                case ApproachType.Permissive:
                    if (leftTurnVolumeValue.OpposingLanes == 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 9519 < leftTurnVolume * Math.Pow(opposingVolume, .706);
                    }
                    else if (leftTurnVolumeValue.OpposingLanes > 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 7974 < 2 * leftTurnVolume * Math.Pow(opposingVolume, .642);
                    }
                    break;
                case ApproachType.PermissiveProtected:
                    if (leftTurnVolumeValue.OpposingLanes == 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 4638 < leftTurnVolume * Math.Pow(opposingVolume, .500);
                    }
                    else if (leftTurnVolumeValue.OpposingLanes > 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 3782 < 2 * leftTurnVolume * Math.Pow(opposingVolume, .404);
                    }
                    break;
                case ApproachType.Protected:
                    if (leftTurnVolumeValue.OpposingLanes == 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 3693 < leftTurnVolume * Math.Pow(opposingVolume, .425);
                    }
                    else if (leftTurnVolumeValue.OpposingLanes > 1)
                    {
                        leftTurnVolumeValue.DecisionBoundariesReview = 3782 < 2 * leftTurnVolume * Math.Pow(opposingVolume, .404);
                    }
                    break;
                default:
                    leftTurnVolumeValue.DecisionBoundariesReview = false;
                    break;

            }
        }

        private static ApproachType GetApproachType(Models.Approach approach)
        {
            ApproachType approachType;
            if (approach.ProtectedPhaseNumber == 0 && approach.PermissivePhaseNumber.HasValue)
                approachType = ApproachType.Permissive;
            else if (approach.ProtectedPhaseNumber != 0 && approach.PermissivePhaseNumber.HasValue)
                approachType = ApproachType.PermissiveProtected;
            else
                approachType = ApproachType.Protected;
            return approachType;
        }

        public static List<Models.Detector> GetDetectorsByPhase(string signalId, int phase)   
        {
            var repository = Models.Repositories.DetectorRepositoryFactory.Create();
            return repository.GetDetectorsBySignalID(signalId).Where(d => d.Approach.ProtectedPhaseNumber == phase).ToList();
        }

        public static List<Models.DetectorEventCountAggregation> GetDetectorVolumebyDetector(List<Models.Detector> detectors, DateTime start,
            DateTime end, TimeSpan startTime, TimeSpan endTime)
        {
            var detectorAggregationRepository = Models.Repositories.DetectorEventCountAggregationRepositoryFactory.Create();
            var detectorAggregations = new List<Models.DetectorEventCountAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                foreach (var detector in detectors)
                {
                    detectorAggregations.AddRange(detectorAggregationRepository
                        .GetDetectorEventCountAggregationByDetectorIdAndDateRange(detector.ID, tempDate.Add(startTime), tempDate.Add(endTime)));
                }
            }
            return detectorAggregations;
        }

    }
    
    public class LeftTurnVolumeValue
    {
        public int OpposingLanes { get; set; }
        public bool CrossProductReview { get; set; }
        public bool DecisionBoundariesReview { get; set; }
    }

    enum ApproachType { Permissive, Protected, PermissiveProtected };
}



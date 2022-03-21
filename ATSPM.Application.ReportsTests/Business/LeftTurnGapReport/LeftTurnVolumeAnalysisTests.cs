using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnVolumeAnalysisTests
    {
        [Fact()]
        public void GetDemandListTest()
        {
            DateTime start = DateTime.MinValue.AddDays(1);
            DateTime end = DateTime.MinValue.AddDays(5);
            TimeSpan startTime = new TimeSpan(6, 0, 0);
            TimeSpan endTime = new TimeSpan(9, 0, 0);
            int[] daysOfWeek = new int[5] { 1, 2, 3, 4, 5 };
            List<Models.DetectorEventCountAggregation> detectorCountAggregations =
                new List<Models.DetectorEventCountAggregation>();
            for (DateTime dt = DateTime.MinValue; dt < DateTime.MinValue.AddDays(7); dt = dt.AddMinutes(15))
            {
                detectorCountAggregations.Add(new Models.DetectorEventCountAggregation { BinStartTime = dt, EventCount = 5 });
            }
            var result = LeftTurnVolumeAnalysis.GetDemandList(start, end, startTime, endTime, daysOfWeek, detectorCountAggregations);
            foreach (var p in result)
            {
                Assert.True(p.Key.TimeOfDay >= startTime && p.Key.TimeOfDay < endTime);
                Assert.Contains((int)p.Key.DayOfWeek, daysOfWeek);
                Assert.Equal(10, p.Value);
            }
        }

        [Fact()]
        public void GetApproachTypeTest()
        {
            var approachPermissive = new Models.Approach { ProtectedPhaseNumber = 0, PermissivePhaseNumber = 1 };
            var approachProtectedPermissive = new Models.Approach { ProtectedPhaseNumber = 1, PermissivePhaseNumber = 1 };
            var approachProtected = new Models.Approach { ProtectedPhaseNumber = 1, };
            Assert.Equal(ApproachType.Permissive, LeftTurnVolumeAnalysis.GetApproachType(approachPermissive));
            Assert.Equal(ApproachType.Protected, LeftTurnVolumeAnalysis.GetApproachType(approachProtected));
            Assert.Equal(ApproachType.PermissiveProtected, LeftTurnVolumeAnalysis.GetApproachType(approachProtectedPermissive));
        }

        [Fact()]
        public void SetDecisionBoundariesReviewTest()
        {
            LeftTurnVolumeValue leftTurnVolumeValue1 = new LeftTurnVolumeValue { OpposingLanes = 1 };
            LeftTurnVolumeValue leftTurnVolumeValue2 = new LeftTurnVolumeValue { OpposingLanes = 2 };

            //Permissive options
            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1873, 10, ApproachType.Permissive);
            Assert.Equal(9517.826359159299, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1874, 10, ApproachType.Permissive);
            Assert.Equal(9522.907953584903, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 909, 10, ApproachType.Permissive);
            Assert.Equal(7972.48808554924, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue2.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 910, 10, ApproachType.Permissive);
            Assert.Equal(7981.2586995047395, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue2.DecisionBoundariesReview);

            //Permissive Protected options
            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1466, 10, ApproachType.PermissiveProtected);
            Assert.Equal(4635.899049806844, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1467, 10, ApproachType.PermissiveProtected);
            Assert.Equal(4639.061327467013, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 745, 10, ApproachType.PermissiveProtected);
            Assert.Equal(3777.3416594418827, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue2.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 746, 10, ApproachType.PermissiveProtected);
            Assert.Equal(3782.4119167028784, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue2.DecisionBoundariesReview);

            //Protected options
            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1387, 10, ApproachType.Protected);
            Assert.Equal(3690.425657940949, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue1, 1388, 10, ApproachType.Protected);
            Assert.Equal(3693.086383000748, leftTurnVolumeValue1.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue1.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 745, 10, ApproachType.Protected);
            Assert.Equal(3777.3416594418827, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.False(leftTurnVolumeValue2.DecisionBoundariesReview);

            LeftTurnVolumeAnalysis.SetDecisionBoundariesReview(leftTurnVolumeValue2, 746, 10, ApproachType.Protected);
            Assert.Equal(3782.4119167028784, leftTurnVolumeValue2.CalculatedVolumeBoundary);
            Assert.True(leftTurnVolumeValue2.DecisionBoundariesReview);
        }

        [Fact()]
        public void GetCrossProductReviewTest()
        {
            Assert.False(LeftTurnVolumeAnalysis.GetCrossProductReview(50000, 1));
            Assert.True(LeftTurnVolumeAnalysis.GetCrossProductReview(50001, 1));
            Assert.False(LeftTurnVolumeAnalysis.GetCrossProductReview(100000, 2));
            Assert.True(LeftTurnVolumeAnalysis.GetCrossProductReview(100001, 2));
        }

        [Fact()]
        public void GetOpposingDetectorsTest()
        {
            var signal = new Models.Signal { Approaches = new List<Models.Approach>() };
            var approach1 = new Models.Approach
            {
                ProtectedPhaseNumber = 1,
                Detectors = new List<Models.Detector>()
            };
            approach1.Detectors.Add(new Models.Detector { MovementTypeId = 1 });
            approach1.Detectors.Add(new Models.Detector { MovementTypeId = 2 });
            approach1.Detectors.Add(new Models.Detector { MovementTypeId = 3 });

            signal.Approaches.Add(approach1);

            var approach2 = new Models.Approach
            {
                ProtectedPhaseNumber = 1,
                Detectors = new List<Models.Detector>()
            };
            approach2.Detectors.Add(new Models.Detector { MovementTypeId = 1 });
            approach2.Detectors.Add(new Models.Detector { MovementTypeId = 2 });
            approach2.Detectors.Add(new Models.Detector { MovementTypeId = 3 });

            signal.Approaches.Add(approach2);

            var detectors = LeftTurnVolumeAnalysis.GetOpposingDetectors(1, signal, new List<int> { 1, 2 });
            Assert.Equal(4, detectors.Count);
        }

        [Fact()]
        public void GetCrossProductTest()
        {
            Assert.Equal(2500.0, LeftTurnVolumeAnalysis.GetCrossProduct(100.0, 25.0));
        }
    }
}
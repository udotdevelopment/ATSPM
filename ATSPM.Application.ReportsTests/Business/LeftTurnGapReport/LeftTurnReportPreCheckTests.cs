using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSPM.IRepositories;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnReportPreCheckTests
    {

        public Dictionary<TimeSpan, double> Dictionary1 { get; }
        public Dictionary<TimeSpan, double> Dictionary2 { get; }
        public Dictionary<TimeSpan, double> Dictionary3 { get; }
        public Dictionary<TimeSpan, double> Dictionary4 { get; }

        public LeftTurnReportPreCheckTests()
        {
            Dictionary1 = new Dictionary<TimeSpan, double>
            {
                { new TimeSpan(8, 0, 0), 10 },
                { new TimeSpan(17, 0, 0), 20 }
            };
            Dictionary4 = new Dictionary<TimeSpan, double>
            {
                { new TimeSpan(8, 0, 0), 0 },
                { new TimeSpan(17, 0, 0), 0 }
            };
            Dictionary2 = new Dictionary<TimeSpan, double>
            {
                { new TimeSpan(7, 0, 0), 10 },
                { new TimeSpan(16, 0, 0), 20 }
            };
            Dictionary3 = new Dictionary<TimeSpan, double>
            {
                { new TimeSpan(8, 0, 0), 10 },
                { new TimeSpan(17, 0, 0), 20 }
            };
        }


        [Fact()]
        public void GetPercentageOfPedCyclesExceptionTest()
        {
            var nullException1 = Assert.Throws<ArgumentNullException>(
                () => LeftTurnReportPreCheck.GetPercentageOfPedCycles(null, Dictionary2));
            Assert.Equal(@"Value cannot be null. (Parameter 'averageCyles')", nullException1.Message);

            var nullException2 = Assert.Throws<ArgumentNullException>(
                () => LeftTurnReportPreCheck.GetPercentageOfPedCycles(Dictionary1, null));
            Assert.Equal(@"Value cannot be null. (Parameter 'averagePedCycles')", nullException2.Message);

            var arithmeticException = Assert.Throws<ArithmeticException>(
                () => LeftTurnReportPreCheck.GetPercentageOfPedCycles(Dictionary4, Dictionary2));
            Assert.Equal("Average Gap Out cannot be zero", arithmeticException.Message);

            var mismatchedPeaksException = Assert.Throws<IndexOutOfRangeException>(
                () => LeftTurnReportPreCheck.GetPercentageOfPedCycles(Dictionary1, Dictionary2));
            Assert.Equal("Peak hours must be the same for Cycles and Ped Cycles", mismatchedPeaksException.Message);


        }

        [Fact()]
        public void GetPercentageOfPedCyclesValidResultsTest()
        {
            var goodResult = LeftTurnReportPreCheck.GetPercentageOfPedCycles(Dictionary1, Dictionary3);
            Assert.Equal(2, goodResult.Count);

            var amGoodResult = goodResult.First();
            Assert.Equal(Dictionary1.First().Key, amGoodResult.Key);
            Assert.Equal(1, amGoodResult.Value);

            var pmGoodResult = goodResult.Last();
            Assert.Equal(Dictionary1.Last().Key, pmGoodResult.Key);
            Assert.Equal(1, pmGoodResult.Value);
        }

        [Fact()]
        public void GetOpposingPhaseTest()
        {
            var approach = new Approach { ProtectedPhaseNumber = 0, PermissivePhaseNumber = 2 };
            Assert.Equal(6, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.PermissivePhaseNumber = 4;
            Assert.Equal(8, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.PermissivePhaseNumber = 6;
            Assert.Equal(2, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.PermissivePhaseNumber = 8;
            Assert.Equal(4, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 1;
            Assert.Equal(2, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 2;
            Assert.Equal(6, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 3;
            Assert.Equal(4, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 4;
            Assert.Equal(8, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 5;
            Assert.Equal(6, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 6;
            Assert.Equal(2, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 7;
            Assert.Equal(8, LeftTurnReportPreCheck.GetOpposingPhase(approach));

            approach.ProtectedPhaseNumber = 8;
            Assert.Equal(4, LeftTurnReportPreCheck.GetOpposingPhase(approach));
        }

        [Fact()]
        public void GetPercentageOfGapOutsExceptionsTest()
        {
            var nullException1 = Assert.Throws<ArgumentNullException>(
                () => LeftTurnReportPreCheck.GetPercentageOfGapOuts(null, Dictionary2));
            Assert.Equal(@"Value cannot be null. (Parameter 'maxCycles')", nullException1.Message);

            var nullException2 = Assert.Throws<ArgumentNullException>(
                () => LeftTurnReportPreCheck.GetPercentageOfGapOuts(Dictionary1, null));
            Assert.Equal(@"Value cannot be null. (Parameter 'averageGapOuts')", nullException2.Message);

            var arithmeticException = Assert.Throws<ArithmeticException>(
                () => LeftTurnReportPreCheck.GetPercentageOfGapOuts(Dictionary4, Dictionary2));
            Assert.Equal("Max Cycles cannot be zero", arithmeticException.Message);

            var mismatchedPeaksException = Assert.Throws<IndexOutOfRangeException>(
                () => LeftTurnReportPreCheck.GetPercentageOfGapOuts(Dictionary1, Dictionary2));
            Assert.Equal("Peak hours must be the same for Average Gap Outs and Max Cycles", mismatchedPeaksException.Message);

        }

        [Fact()]
        public void GetPercentageOfGapOutsValidResultsTest()
        {
            var goodResult = LeftTurnReportPreCheck.GetPercentageOfGapOuts(Dictionary1, Dictionary3);
            Assert.Equal(2, goodResult.Count);

            var amGoodResult = goodResult.First();
            Assert.Equal(Dictionary1.First().Key, amGoodResult.Key);
            Assert.Equal(1, amGoodResult.Value);

            var pmGoodResult = goodResult.Last();
            Assert.Equal(Dictionary1.Last().Key, pmGoodResult.Key);
            Assert.Equal(1, pmGoodResult.Value);
        }

        [Fact()]
        public void LoadGapOutAveragesTest()
        {
            var aggregations = new List<double>
            {
                5,6,7
            };
            var dictionary = new Dictionary<TimeSpan, double>();
            LeftTurnReportPreCheck.LoadGapOutAverages(dictionary, new TimeSpan(8, 0, 0), aggregations);
            Assert.Equal(new TimeSpan(8, 0, 0), dictionary.First().Key);
            Assert.Equal(6, dictionary.First().Value);

        }

        [Fact()]
        public void LoadAveragesTest()
        {
            var aggregations = new List<PhaseTerminationAggregation>
            {
                {new PhaseTerminationAggregation{GapOuts=1, ForceOffs=1, MaxOuts=1, Unknown=2}},
                {new PhaseTerminationAggregation{GapOuts=1, ForceOffs=1, MaxOuts=2, Unknown=2}},
                {new PhaseTerminationAggregation{GapOuts=1, ForceOffs=2, MaxOuts=2, Unknown=2}}
            };
            var dictionary = new Dictionary<TimeSpan, double>();
            LeftTurnReportPreCheck.LoadAverages(dictionary, new TimeSpan(8, 0, 0), aggregations);
            Assert.Equal(new TimeSpan(8, 0, 0), dictionary.First().Key);
            Assert.Equal(6, dictionary.First().Value);
        }

        [Fact()]
        public void GetAmPmPeaksTest()
        {
            //what should happen if hourly flow rates are the same for 2 hours in period?
            var amStartTime = new TimeSpan(6, 0, 0);
            var amEndTime = new TimeSpan(10, 0, 0);
            var pmStartTime = new TimeSpan(16, 0, 0);
            var pmEndTime = new TimeSpan(20, 0, 0);
            var hourlyFlowRates = new Dictionary<TimeSpan, int>
            {
                { new TimeSpan(6, 0, 0), 10 },
                { new TimeSpan(7, 0, 0), 20 },
                { new TimeSpan(8, 0, 0), 30 },
                { new TimeSpan(9, 0, 0), 40 },
                { new TimeSpan(16, 0, 0), 50 },
                { new TimeSpan(17, 0, 0), 60 },
                { new TimeSpan(18, 0, 0), 70 },
                { new TimeSpan(19, 0, 0), 80 },
            };
            var result = LeftTurnReportPreCheck.GetAmPmPeaks(amStartTime,
                                                             amEndTime,
                                                             pmStartTime,
                                                             pmEndTime,
                                                             hourlyFlowRates);
            Assert.True(result.Keys.Min().Hours == 9);
            Assert.True(result.Keys.Max().Hours == 19);
        }

        [Fact()]
        public void GetHourlyFlowRatesTest()
        {
            var hourlyTimeSpans = new List<TimeSpan>();
            for (int i = 0; i < 24; i++)
            {
                hourlyTimeSpans.Add(new TimeSpan(i, 0, 0));
            }

            Dictionary<TimeSpan, int> averageByBin = new Dictionary<TimeSpan, int>();
            for (TimeSpan t = new TimeSpan(0, 0, 0); t <= new TimeSpan(23, 45, 0); t = t.Add(new TimeSpan(0, 15, 0)))
            {
                averageByBin.Add(t, 1);
            }

            var result = LeftTurnReportPreCheck.GetHourlyFlowRates(hourlyTimeSpans, averageByBin);
            foreach (var rate in result)
            {
                Assert.Equal(4, rate.Value);
            }
        }

        [Fact()]
        public void GetAveragesForBinsTest()
        {
            List<DetectorEventCountAggregation> volumeAggregations = new List<DetectorEventCountAggregation>();
            for (DateTime dt = DateTime.MinValue; dt < DateTime.MinValue.AddDays(3); dt = dt.AddMinutes(15))
            {
                volumeAggregations.Add(new DetectorEventCountAggregation { BinStartTime = dt, EventCount = 5 });
            }

            var distinctTimeSpans = new List<TimeSpan>();
            for (int i = 0; i < 24; i++)
            {
                for (int m = 0; m < 60; m += 15)
                {
                    distinctTimeSpans.Add(new TimeSpan(i, m, 0));
                }
            }
            var result = LeftTurnReportPreCheck.GetAveragesForBinsByTimeSpan(volumeAggregations, distinctTimeSpans);
            foreach (var avg in result)
            {
                Assert.Equal(5, avg.Value);
            }
        }

        [Fact()]
        public void GetAllDetectorsForSignalTest()
        {
            var testSignalRepository = new TestSignalRepository();
            var result = LeftTurnReportPreCheck.GetAllLaneByLaneDetectorsForSignal("1", DateTime.Now, testSignalRepository);
            Assert.Single(result);
        }
    }

    class TestSignalRepository : ISignalsRepository
    {
        public void AddList(List<Signal> signals)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(Signal signal)
        {
            throw new NotImplementedException();
        }

        public void AddSignalAndDetectorLists(Signal returnSignal)
        {
            throw new NotImplementedException();
        }

        public int CheckVersionWithFirstDate(string signalId)
        {
            throw new NotImplementedException();
        }

        public Signal CopySignalToNewVersion(Signal originalVersion)
        {
            throw new NotImplementedException();
        }

        public List<Signal> EagerLoadAllSignals()
        {
            throw new NotImplementedException();
        }

        public bool Exists(string signalId)
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetAllEnabledSignals()
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetAllSignals()
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalID)
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId)
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetLatestVersionOfAllSignals()
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetLatestVersionOfAllSignalsForFtp()
        {
            throw new NotImplementedException();
        }

        public Signal GetLatestVersionOfSignalBySignalID(string signalID)
        {
            throw new NotImplementedException();
        }

        public string GetSignalDescription(string signalId)
        {
            throw new NotImplementedException();
        }

        public string GetSignalLocation(string signalID)
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetSignalsBetweenDates(string signalId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Signal GetSignalVersionByVersionId(int versionId)
        {
            throw new NotImplementedException();
        }

        public Signal GetVersionOfSignalByDate(string signalId, DateTime startDate)
        {

            var detector1 = new Detector {
                DetectionTypeDetectors = new List<DetectionTypeDetector> {
                                            new DetectionTypeDetector { DetectionTypeId = 1 },
                                            new DetectionTypeDetector { DetectionTypeId = 2 },
                                            },
                DetectionHardwareId = 1
            };
            var detector2 = new Detector
            {
                DetectionTypeDetectors = new List<DetectionTypeDetector> {
                                            new DetectionTypeDetector { DetectionTypeId = 3 },
                                            new DetectionTypeDetector { DetectionTypeId = 4 },
                                            },
                DetectionHardwareId = 2
            };
            var detector3 = new Detector
            {
                DetectionTypeDetectors = new List<DetectionTypeDetector> {
                                            new DetectionTypeDetector { DetectionTypeId = 5 },
                                            new DetectionTypeDetector { DetectionTypeId = 6 }
                                            },
                DetectionHardwareId = 6
            };

            var approach1 = new Approach
            {
                Detectors = new List<Detector> { detector1, detector2, detector3 }
            };
            var signal = new Signal
            {
                Approaches = new List<Approach> { approach1 }
            };
            return signal;
        }

        public Signal GetVersionOfSignalByDateWithDetectionTypes(string signalId, DateTime startDate)
        {
            throw new NotImplementedException();
        }

        public void SetAllVersionsOfASignalToDeleted(string id)
        {
            throw new NotImplementedException();
        }

        public void SetVersionToDeleted(int versionId)
        {
            throw new NotImplementedException();
        }
    }
}
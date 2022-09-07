using System;
using Xunit;
using MOE.Common.Business.LeftTurnGapReport;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace ATSPMWebsiteTests
{
    public class LeftTurnReportPreCheckUnitTests
    {
        string BadSignal = "BadSignal";
        int BadDirectionId = -1;
        string GoodSignal = "7220";
        int GoodDirectionId = 1;
        TimeSpan AmPeakStart = new TimeSpan(6, 0, 0);
        TimeSpan AmPeakEnd = new TimeSpan(9, 0, 0);
        TimeSpan PmPeakStart = new TimeSpan(15, 0, 0);
        TimeSpan PmPeakEnd = new TimeSpan(18, 0, 0);
        DateTime StartDate = Convert.ToDateTime("1/1/2021");
        DateTime EndDate = Convert.ToDateTime("1/1/2021");
        List<DetectorEventCountAggregation> emptyDetectorEventAggregations = new List<DetectorEventCountAggregation>();
        List<Signal> emptySignals = new List<Signal>();
        List<Approach> emptyApproaches = new List<Approach>();
        List<Detector> emptyDetectors = new List<Detector>();

        List<DetectorEventCountAggregation> DetectorEventAggregations = new List<DetectorEventCountAggregation>();
        List<Signal> Signals = new List<Signal>();
        List<Approach> Approaches = new List<Approach>();
        List<Detector> Detectors = new List<Detector>();


        [Fact]
        public void LeftTurnReportPreCheck_HandleNotExistantSignalId()
        {
            var signalRepository = new TestSignalRepositoryExistsReturnsFalse();
            SignalsRepositoryFactory.SetSignalsRepository(signalRepository);
            Assert.Throws<ArgumentException>(()=>LeftTurnReportPreCheck.GetAMPMPeakFlowRate(BadSignal, BadDirectionId, StartDate, EndDate, AmPeakStart, AmPeakEnd, PmPeakStart, PmPeakEnd));
        }

        [Fact]
        public void LeftTurnReportPreCheck_HandleNoDetectorsFound()
        {
            var signalRepository = new TestSignalRepositoryGoodData();
            SignalsRepositoryFactory.SetSignalsRepository(signalRepository);
            var detectorRepository = new TestDetectorRepositoryLeftTurnGapReportDetectorsNotFound();
            DetectorRepositoryFactory.SetDetectorRepository(detectorRepository);
            Assert.Throws<NotSupportedException>(() => LeftTurnReportPreCheck.GetAMPMPeakFlowRate(GoodSignal, BadDirectionId, StartDate, EndDate, AmPeakStart, AmPeakEnd, PmPeakStart, PmPeakEnd));
        }

        [Fact]
        public void LeftTurnReportPreCheck_HandleNoAggregationData()
        {
            Mock<DbSet<DetectorEventCountAggregation>> mockSet = SetUpMock();

            var mockContext = new Mock<SPM>();
            mockContext.Setup(c => c.DetectorEventCountAggregations).Returns(mockSet.Object);

            DetectorEventCountAggregationRepositoryFactory.SetContext(mockContext.Object);

            var signalRepository = new TestSignalRepositoryGoodData();
            SignalsRepositoryFactory.SetSignalsRepository(signalRepository);
            var detectorRepository = new TestDetectorRepositoryLeftTurnGapReportDetectorsNotFound();
            DetectorRepositoryFactory.SetDetectorRepository(detectorRepository);
            Assert.Throws<NotSupportedException>(() => LeftTurnReportPreCheck.GetAMPMPeakFlowRate(GoodSignal, BadDirectionId, StartDate, EndDate, AmPeakStart, AmPeakEnd, PmPeakStart, PmPeakEnd));
        }

        private Mock<DbSet<DetectorEventCountAggregation>> SetUpMock()
        {
            var mockSet = new Mock<DbSet<DetectorEventCountAggregation>>();
            mockSet.As<IQueryable<DetectorEventCountAggregation>>().Setup(m => m.Provider).Returns(emptyDetectorEventAggregations.AsQueryable().Provider);
            mockSet.As<IQueryable<DetectorEventCountAggregation>>().Setup(m => m.Expression).Returns(emptyDetectorEventAggregations.AsQueryable().Expression);
            mockSet.As<IQueryable<DetectorEventCountAggregation>>().Setup(m => m.ElementType).Returns(emptyDetectorEventAggregations.AsQueryable().ElementType);
            mockSet.As<IQueryable<DetectorEventCountAggregation>>().Setup(m => m.GetEnumerator()).Returns(emptyDetectorEventAggregations.AsQueryable().GetEnumerator());
            return mockSet;
        }
    }

    public class TestDetectorRepositoryLeftTurnGapReportDetectorsNotFound : MOE.Common.Models.Repositories.IDetectorRepository
    {
        public Detector Add(Detector Detector)
        {
            throw new NotImplementedException();
        }

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            throw new NotImplementedException();
        }

        public bool CheckReportAvialbilityByDetector(Detector gd, int metricID)
        {
            throw new NotImplementedException();
        }

        public SPM GetContext()
        {
            throw new NotImplementedException();
        }

        public Detector GetDetectorByDetectorID(string DetectorID)
        {
            throw new NotImplementedException();
        }

        public Detector GetDetectorByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsByIds(List<int> excludedDetectorIds)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalID(string SignalID)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            throw new NotImplementedException();
        }

        public List<Detector> GetDetectorsBySignalIdMovementTypeIdDirectionTypeId(string signalId, int directionTypeId, List<int> movementTypeIds)
        {
            return new List<Detector>();
        }

        public int GetMaximumDetectorChannel(int versionId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Detector Detector)
        {
            throw new NotImplementedException();
        }

        public void Remove(int ID)
        {
            throw new NotImplementedException();
        }

        public void Update(Detector Detector)
        {
            throw new NotImplementedException();
        }
    }



    public class TestSignalRepositoryGoodData : MOE.Common.Models.Repositories.ISignalsRepository
    {
        public void AddList(List<Signal> signals)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(Signal signal)
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
            return true;
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

        public SignalFTPInfo GetSignalFTPInfoByID(string signalID)
        {
            throw new NotImplementedException();
        }

        public List<SignalFTPInfo> GetSignalFTPInfoForAllFTPSignals()
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
            throw new NotImplementedException();
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

    public class TestSignalRepositoryExistsReturnsFalse : MOE.Common.Models.Repositories.ISignalsRepository
    {
        public void AddList(List<Signal> signals)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(Signal signal)
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
            return false;
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

        public SignalFTPInfo GetSignalFTPInfoByID(string signalID)
        {
            throw new NotImplementedException();
        }

        public List<SignalFTPInfo> GetSignalFTPInfoForAllFTPSignals()
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
            throw new NotImplementedException();
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

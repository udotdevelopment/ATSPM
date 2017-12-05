using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.CommonTests.Models;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class MetricOptionsTests

    {
        private MOE.CommonTests.Models.InMemoryMOEDatabase _db;
        private InMemoryMetricTypeRepository _mtr;
        private InMemoryApplicationEventRepository _aer;
        private MOE.Common.Models.Repositories.ISignalsRepository _signalRepository;

        [TestInitialize]
        public void Initialize()
        {
            _db = new InMemoryMOEDatabase();
            _db.ClearTables();

            _mtr = new InMemoryMetricTypeRepository(_db);
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.SetMetricsRepository(_mtr);

            _aer = new InMemoryApplicationEventRepository(_db);
            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.SetApplicationEventRepository(_aer);

            _signalRepository = new InMemorySignalsRepository(_db);
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(_signalRepository);

            _db.PopulateSignal();


        }

        [TestMethod()]
        public void MetricOptionsTest()
        {
            MetricOptions options = NewOptionsForTest();

            Assert.IsTrue(options.Y2AxisMin == 0);
        }

        [TestMethod()]
        public void CreateMetricTestHasMetricTypeInChartName()
        {

            MetricOptions options = NewOptionsForTest();


            options.CreateMetric();

            var expectedMetricType = _mtr.GetMetricsByID(1);

            Models.ApplicationEvent applicationEvent = _db.ApplicaitonEvents.Take(1).FirstOrDefault();

            if (applicationEvent != null)
            {
                Assert.IsTrue(applicationEvent.Description.Contains(expectedMetricType.ChartName));
            }
            else
            {
                Assert.Fail();
            }
        }




        [TestMethod()]
        public void GetSignalLocationTest()
        {
            MetricOptions options = NewOptionsForTest();

            var signalFromDb = _signalRepository.GetLatestVersionOfSignalBySignalID(options.SignalID);

            Assert.IsTrue(options.GetSignalLocation() == signalFromDb.GetSignalLocation());


        }

        [TestMethod()]
        public void CreateFileNameReturnsStringTest()
        {
            MetricOptions options = NewOptionsForTest();


            var expectedMetricType = _mtr.GetMetricsByID(1);

            string filename = options.CreateFileName();

            Assert.IsTrue(filename.Contains(expectedMetricType.Abbreviation));
        }

        [TestMethod()]
        public void CreateFileNameCreatesImageDirectory()
        {
            string testDirPath = "C:\\Temp\\ATSPMImages";

            MetricOptions options = NewOptionsForTest();

            options.MetricFileLocation = testDirPath;

            DirectoryInfo di = new DirectoryInfo(testDirPath);
            di.Refresh();

            if (di.Exists)
            {
                Directory.Delete(testDirPath);
            }

            string filename = options.CreateFileName();
            di.Refresh();

            Assert.IsTrue(di.Exists);
        }

        [TestMethod()]
        public void DriveAvailableTest()
        {
            Assert.Fail();
        }

        private MetricOptions NewOptionsForTest()
        {
            MetricOptions options = new MetricOptions
            {
                MetricTypeID = 1,
                MetricFileLocation = "C:\\Temp\\ATSPMImages",
                SignalID = _signalRepository.GetAllSignals().Take(1).FirstOrDefault().SignalID
            };

            return options;

        }

        [TestMethod()]
        public void MetricOptionsTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateMetricTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSignalLocationTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateFileNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DriveAvailableTest1()
        {
            Assert.Fail();
        }


    }
}
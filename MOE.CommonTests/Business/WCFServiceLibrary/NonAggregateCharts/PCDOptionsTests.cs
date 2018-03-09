using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class PCDOptionsTests

    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
            db.ClearTables();
            XmlToListImporter.LoadControllerEventLog("7185Events02_01_2018.Xml", db);
            XmlToListImporter.LoadSignals("signals.xml", db);
            XmlToListImporter.LoadApproaches("approachesfor7185.xml", db);
            XmlToListImporter.LoadDetectors("detectorsFor7185.xml", db);
            XmlToListImporter.AddDetectionTypesToDetectors
                ("DetectorTypesforDetectorsFor7185.xml", db);
            XmlToListImporter.AddDetectionTypesToMetricTypes("mtdt.xml", db);
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
                new InMemorySignalsRepository(db));
            MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(db));
            ApplicationEventRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApplicationEventRepository(db));
            Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
                new InMemoryDirectionTypeRepository());
            SpeedEventRepositoryFactory.SetSignalsRepository(new InMemorySpeedEventRepository(db));
            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(db));
            ControllerEventLogRepositoryFactory.SetRepository(new InMemoryControllerEventLogRepository(db));
            DetectorRepositoryFactory.SetDetectorRepository(new InMemoryDetectorRepository(db));
            XmlToListImporter.LoadSpeedEvents("7185speed.xml", db);
        }

        [TestMethod()]
        public void PcdOptionsTest()
        {
            var start = Convert.ToDateTime("02/01/2018 00:01");
            var end = Convert.ToDateTime("02/01/2018 23:59");
            PCDOptions options = new PCDOptions("7185", start, end, 100, 5000, 15,2, true, true, 6, true );
            Assert.IsTrue(options.CreateMetric().Count > 1);
        }
    }
}
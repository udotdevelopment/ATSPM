using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachSpeedOptionsTests
    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
            db.ClearTables();
            XMLToListImporter.LoadControllerEventLogsFromMOEDB(db);
            XMLToListImporter.LoadSignals("signals.xml", db);
            XMLToListImporter.LoadApproaches("approachesfor7185.xml", db);
            XMLToListImporter.LoadDetectors("detectorsFor7185.xml", db);
            XMLToListImporter.AddDetectionTypesToDetectors
                ("DetectorTypesforDetectorsFor7185.xml", db);
            XMLToListImporter.AddDetectionTypesToMetricTypes("mtdt.xml", db);
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
            XMLToListImporter.LoadSpeedEvents("7185speed.xml", db);
        }

        [TestMethod()]
        public void CreateMetricTest()
        {
            var start = Convert.ToDateTime("02/01/2018 00:01");
            var end = Convert.ToDateTime("02/01/2018 23:59");
            ApproachSpeedOptions options = new ApproachSpeedOptions("7185",start, end, 100, 0, 15, 3, true,10 , true, true, true, true );

            Assert.IsTrue(options.CreateMetric().Count > 1);

        }
    }
}
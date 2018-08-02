using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class SplitFailOptionsTests
    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
            db.ClearTables();
            XmlToListImporter.LoadControllerEventLog("7185_10_17_2017.xml", db);
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
            ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(db));
            ControllerEventLogRepositoryFactory.SetRepository(new InMemoryControllerEventLogRepository(db));
            DetectorRepositoryFactory.SetDetectorRepository(new InMemoryDetectorRepository(db));





        }
        //[TestMethod()]
        //public void CreateMetricTest()
        //{
        //    string signalId = "7185";
        //    DateTime start = Convert.ToDateTime("10/17/2017 17:00:00");
        //    DateTime end = Convert.ToDateTime("10/17/2017 18:00:00");

        //    SplitFailOptions sfo = new SplitFailOptions(signalId, start, end,12,5,true,true,false);
        //    List<string> path = sfo.CreateMetric();

        //    Assert.IsTrue(path.Count > 0);
        //}

      

        //[TestMethod()]
        //public void SomeSpliTFailTest()
        //{
        //    InMemoryMOEDatabase _db = new InMemoryMOEDatabase();


        //}





    }
}
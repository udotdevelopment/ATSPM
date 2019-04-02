using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.SplitFail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.SplitFail.Tests
{
    [TestClass()]
    public class SplitFailPhaseTests
    {
        private InMemoryMOEDatabase inMemoryMoeDatabase = new InMemoryMOEDatabase();
        public SplitFailPhaseTests()
        {
            //inMemoryMoeDatabase.ClearTables();
            //XmlToListImporter.LoadControllerEventLog("ControllerEventLogs7185201710175-6pm.xml", inMemoryMoeDatabase);
            //XmlToListImporter.LoadSignals("signals.xml", inMemoryMoeDatabase);
            //XmlToListImporter.LoadApproaches("approachesfor7185.xml", inMemoryMoeDatabase);
            //XmlToListImporter.LoadDetectors("detectorsFor7185.xml", inMemoryMoeDatabase);
            //XmlToListImporter.AddDetectionTypesToDetectors
            //    ("DetectorTypesforDetectorsFor7185.xml", inMemoryMoeDatabase);
            //XmlToListImporter.AddDetectionTypesToMetricTypes("mtdt.xml", inMemoryMoeDatabase);
            //MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(
            //    new InMemorySignalsRepository(inMemoryMoeDatabase));
            //MetricTypeRepositoryFactory.SetMetricsRepository(new InMemoryMetricTypeRepository(inMemoryMoeDatabase));
            //ApplicationEventRepositoryFactory.SetApplicationEventRepository(
            //    new InMemoryApplicationEventRepository(inMemoryMoeDatabase));
            //Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(
            //    new InMemoryDirectionTypeRepository());
            //SpeedEventRepositoryFactory.SetSignalsRepository(new InMemorySpeedEventRepository(inMemoryMoeDatabase));
            //ApproachRepositoryFactory.SetApproachRepository(new InMemoryApproachRepository(inMemoryMoeDatabase));
            //ControllerEventLogRepositoryFactory.SetRepository(new InMemoryControllerEventLogRepository(inMemoryMoeDatabase));
            //DetectorRepositoryFactory.SetDetectorRepository(new InMemoryDetectorRepository(inMemoryMoeDatabase));
            ////XmlToListImporter.LoadSpeedEvents("7185speed.xml", inMemoryMoeDatabase);
        }
        [TestMethod()]
        public void SplitFailPhaseTest()
        {
            SplitFailOptions splitFailOptions = new SplitFailOptions{StartDate = new DateTime(2017, 10, 17, 17, 0, 0), EndDate = new DateTime(2017, 10,17, 17, 11, 1 ),  FirstSecondsOfRed = 5, SignalID = "7185", MetricTypeID = 12, ShowAvgLines = true, ShowPercentFailLines = true, ShowFailLines = true, Y2AxisMax = null, YAxisMin = 0, Y2AxisMin = 0, YAxisMax = null};
            var signalRepository = SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetLatestVersionOfSignalBySignalID("7185");
            var approach = signal.Approaches.Where(a => a.ApproachID == 5593).FirstOrDefault();
            SplitFailPhase splitFailPhase = new SplitFailPhase(approach, splitFailOptions, true);
            Assert.IsTrue(splitFailPhase.Cycles[4].StartTime == new DateTime(2017,10, 17, 17, 9, 33));
            Assert.IsTrue(splitFailPhase.Cycles[4].YellowEvent == new DateTime(2017, 10, 17, 17, 10, 09));
            Assert.IsTrue(splitFailPhase.Cycles[4].RedEvent == new DateTime(2017, 10, 17, 17, 10, 13));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen.Count == 2);
            DateTime date1 = new DateTime(2017, 10, 17, 17, 1, 59, 500);
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[0].DetectorOn == date1);
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[0].DetectorOff == new DateTime(2017, 10, 17, 17, 9, 43, 800));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[0].DurationInMilliseconds == 464300);
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[1].DetectorOn == new DateTime(2017, 10, 17, 17, 9, 55, 400));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[1].DetectorOff == new DateTime(2017, 10, 17, 17, 10, 6, 300));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringGreen[1].DurationInMilliseconds == 10900);
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringRed.Count == 1);
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringRed[0].DetectorOn == new DateTime(2017, 10, 17, 17, 10, 9, 200));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringRed[0].DetectorOff == new DateTime(2017, 10, 17, 17, 10, 13, 300));
            Assert.IsTrue(splitFailPhase.Cycles[4].ActivationsDuringRed[0].DurationInMilliseconds == 4100);
            Assert.IsTrue(splitFailPhase.Cycles[4].GreenOccupancyTimeInMilliseconds == 21700.0);
            Assert.IsTrue(splitFailPhase.Cycles[4].TotalGreenTimeMilliseconds == 36000.0);
            Assert.IsTrue(splitFailPhase.Cycles[4].RedOccupancyTimeInMilliseconds == 300.0);
            Assert.IsTrue(splitFailPhase.Cycles[4].RedOccupancyPercent == 6.0);
            Assert.IsTrue(splitFailPhase.Cycles[4].FirstSecondsOfRed == 5); 
            Assert.IsTrue(Math.Round(splitFailPhase.Cycles[4].GreenOccupancyPercent) == 60.0);
        }
        [TestMethod()]
        public void SplitFailDataAggregationTest()
        {
            var startTime = new DateTime(2014, 1, 1);
            var endTime = new DateTime(2014, 1, 1, 0, 15, 0);
            var splitFailAggregateRepository = MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            var splitFails = splitFailAggregateRepository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(4971,
                startTime, endTime, true);

            var signalRepository = SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetLatestVersionOfSignalBySignalID("5078");
            var approach = signal.Approaches.Where(s => s.ApproachID == 4971).FirstOrDefault();

            var splitFailOptions = new SplitFailOptions
            {
                FirstSecondsOfRed = 5,
                StartDate = startTime,
                EndDate = endTime,
                MetricTypeID = 12
            };
            var splitFailPhase = new SplitFailPhase(approach, splitFailOptions, true);

            Assert.IsTrue(splitFails.FirstOrDefault().SplitFailures == splitFailPhase.TotalFails);

        }
    }

    
}
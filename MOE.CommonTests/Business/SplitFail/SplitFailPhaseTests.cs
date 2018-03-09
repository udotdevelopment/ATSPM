using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.SplitFail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AddControllerEventLogsToInMemoryDb.LoadControllerEventLogs(inMemoryMoeDatabase);
            inMemoryMoeDatabase.AddTestSignalForSplitFailTest();
            XMLToListImporter
            //SignalsRepositoryFactory.SetSignalsRepository(new InMemorySignalsRepository(inMemoryMoeDatabase));
            //ControllerEventLogRepositoryFactory.SetRepository(new InMemoryControllerEventLogRepository(inMemoryMoeDatabase));
        }
        [TestMethod()]
        public void SplitFailPhaseTest()
        {
            SplitFailOptions splitFailOptions = new SplitFailOptions{StartDate = new DateTime(2017, 10, 17, 17, 0, 0), EndDate = new DateTime(2017, 10,17, 17, 11, 1 ),  FirstSecondsOfRed = 5, SignalID = "7185", MetricTypeID = 12, ShowAvgLines = true, ShowPercentFailLines = true, ShowFailLines = true, Y2AxisMax = null, YAxisMin = 0, Y2AxisMin = 0, YAxisMax = null};
            var signal = inMemoryMoeDatabase.Signals.FirstOrDefault();
            SplitFailPhase splitFailPhase = new SplitFailPhase(signal.Approaches.FirstOrDefault(), splitFailOptions, true);
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
            splitFailOptions.CreateMetric();
        }
    }
}
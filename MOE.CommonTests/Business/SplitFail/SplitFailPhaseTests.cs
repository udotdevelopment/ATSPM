using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.SplitFail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
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
        }
        [TestMethod()]
        public void SplitFailPhaseTest()
        {
            SplitFailOptions splitFailOptions = new SplitFailOptions{ EndDate = new DateTime(2017, 10,17, 17, 11, 1 ), StartDate = new DateTime(2017, 10, 17, 17, 8, 13), FirstSecondsOfRed = 1, SignalID = "7185", MetricTypeID = 12, ShowAvgLines = true, ShowPercentFailLines = true, ShowFailLines = true, Y2AxisMax = null, YAxisMin = 0, Y2AxisMin = 0, YAxisMax = null};
            var signal = inMemoryMoeDatabase.Signals.FirstOrDefault();
            SplitFailPhase splitFailPhase = new SplitFailPhase(signal.Approaches.FirstOrDefault(), splitFailOptions, true);
            Assert.IsTrue(splitFailPhase.Cycles[0].StartTime == new DateTime(2017,10, 17, 17, 9, 33));
            Assert.IsTrue(splitFailPhase.Cycles[0].YellowEvent == new DateTime(2017, 10, 17, 17, 10, 09));
            Assert.IsTrue(splitFailPhase.Cycles[0].RedEvent == new DateTime(2017, 10, 17, 17, 10, 13));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen.Count == 2);
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[0].DetectorOn == new DateTime(2017, 10, 17, 17, 1, 59, 5));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[0].DetectorOff == new DateTime(2017, 10, 17, 17, 9, 43, 8));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[0].DurationInMilliseconds == 10800);
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[1].DetectorOn == new DateTime(2017, 10, 17, 17, 1, 55, 4));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[1].DetectorOff == new DateTime(2017, 10, 17, 17, 9, 06, 3));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringGreen[1].DurationInMilliseconds == 10900);
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringRed.Count == 1);
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringRed[0].DetectorOn == new DateTime(2017, 10, 17, 17, 10, 9, 2));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringRed[0].DetectorOff == new DateTime(2017, 10, 17, 17, 10, 13, 3));
            Assert.IsTrue(splitFailPhase.Cycles[0].ActivationsDuringRed[0].DurationInMilliseconds == 300);
            Assert.IsTrue(splitFailPhase.Cycles[0].GreenOccupancyPercent == 60.0);
            Assert.IsTrue(splitFailPhase.Cycles[0].GreenOccupancyTimeInMilliseconds == 3600.0);
            Assert.IsTrue(splitFailPhase.Cycles[0].RedOccupancyPercent == 6.0);
            Assert.IsTrue(splitFailPhase.Cycles[0].RedOccupancyTimeInMilliseconds == 300.0);
        }
    }
}
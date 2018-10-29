using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Business.SignalPhase
{
    [TestClass]
    public class SignalPhase24HrsVs15MinsTest
    {
        [TestMethod]
        public void TestMetric24HrVs15Min()
        {
            //5030 has 8 phases, with 6 to 8 detectors per phase.  There is Stop Bar, Lane-by-Lane Count,
            // Lane-by-Lane with Speed Restriction, Advanced Count, Advanced Spped as the detectin types for he different 
            // detectors.  THis signals has everything and is a good one to test for everything UDOT uses for a signal.

            var signalId = "5030"; 
            int metricTypeId = 6;
            var  stringstartTime = "10/24/2018 3:30 PM";

            var startTime = DateTime.Parse(stringstartTime, System.Globalization.CultureInfo.InvariantCulture);
            var endTime24Hrs = startTime.AddDays(1);
            var endTime15Mins = startTime.AddMinutes(15);
            var signalRepository = SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetVersionOfSignalByDate(signalId, startTime);
            var metricApproaches = signal.GetApproachesForSignalThatSupportMetric(metricTypeId);
            
            if (metricApproaches.Count > 0)
                foreach (var approach in metricApproaches)
                {
                    var signalPhase24Hrs = new Common.Business.SignalPhase(startTime, endTime24Hrs, approach,true, 15, metricTypeId, false);
                    var signalPhase15Mins = new Common.Business.SignalPhase(startTime, endTime15Mins, approach, true, 15, metricTypeId, false);
                    Assert.IsTrue(signalPhase24Hrs.Cycles.Count > signalPhase15Mins.Cycles.Count);
                    for (int i = 0; i < signalPhase15Mins.Cycles.Count; i++)
                    {
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].EndTime == signalPhase15Mins.Cycles[i].EndTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].GreenEvent == signalPhase15Mins.Cycles[i].GreenEvent);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].GreenLineY == signalPhase15Mins.Cycles[i].GreenLineY);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].RedLineY == signalPhase15Mins.Cycles[i].RedLineY);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].StartTime == signalPhase15Mins.Cycles[i].StartTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalArrivalOnGreen == signalPhase15Mins.Cycles[i].TotalArrivalOnGreen);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalArrivalOnRed == signalPhase15Mins.Cycles[i].TotalArrivalOnRed);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalArrivalOnYellow == signalPhase15Mins.Cycles[i].TotalArrivalOnYellow);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalDelay == signalPhase15Mins.Cycles[i].TotalDelay);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalGreenTime == signalPhase15Mins.Cycles[i].TotalGreenTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalGreenTimeMilliseconds == signalPhase15Mins.Cycles[i].TotalGreenTimeMilliseconds);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalRedTime == signalPhase15Mins.Cycles[i].TotalRedTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalRedTimeMilliseconds == signalPhase15Mins.Cycles[i].TotalRedTimeMilliseconds);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalTime == signalPhase15Mins.Cycles[i].TotalTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalTimeMilliseconds == signalPhase15Mins.Cycles[i].TotalTimeMilliseconds);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalVolume == signalPhase15Mins.Cycles[i].TotalVolume);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalYellowTime == signalPhase15Mins.Cycles[i].TotalYellowTime);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].TotalYellowTimeMilliseconds == signalPhase15Mins.Cycles[i].TotalYellowTimeMilliseconds);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].YellowEvent == signalPhase15Mins.Cycles[i].YellowEvent);
                        Assert.IsTrue(signalPhase24Hrs.Cycles[i].YellowLineY == signalPhase15Mins.Cycles[i].YellowLineY);
                        for(int j =0; j <  signalPhase15Mins.Cycles[i].DetectorEvents.Count; j++)
                        {
                            Assert.IsTrue(signalPhase24Hrs.Cycles[i].DetectorEvents[j].ArrivalType == signalPhase15Mins.Cycles[i].DetectorEvents[j].ArrivalType);
                            Assert.IsTrue(signalPhase24Hrs.Cycles[i].DetectorEvents[j].Delay == signalPhase15Mins.Cycles[i].DetectorEvents[j].Delay);
                            Assert.IsTrue(signalPhase24Hrs.Cycles[i].DetectorEvents[j].TimeStamp == signalPhase15Mins.Cycles[i].DetectorEvents[j].TimeStamp);
                            Assert.IsTrue(signalPhase24Hrs.Cycles[i].DetectorEvents[j].YPoint == signalPhase15Mins.Cycles[i].DetectorEvents[j].YPoint);
                        }
                    }
                }
        }
    }
}

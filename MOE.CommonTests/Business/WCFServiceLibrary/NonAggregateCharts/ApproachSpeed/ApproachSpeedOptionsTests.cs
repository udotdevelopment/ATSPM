using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Business.WCFServiceLibrary.NonAggregateCharts.ApproachSpeed
{
    [TestClass()]
    public class ApproachSpeedOptionsTests
    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
            ChartTestHelper.InitializeTestDataFor7185Feb012018(db);
        }

        [TestMethod()]
        public void CreateMetricTest()
        {
            var start = Convert.ToDateTime("02/01/2018 00:01");
            var end = Convert.ToDateTime("02/01/2018 23:59");
            ApproachSpeedOptions options = new ApproachSpeedOptions("7185",start, end, 100, 0, 15, 3, true,10 , true, true, true, true );

            Assert.IsTrue(options.CreateMetric().Count > 1);

        }

        [TestMethod()]
        public void SliceTest()
        {
            var start = Convert.ToDateTime("02/01/2018 08:00");
            var end = Convert.ToDateTime("02/01/2018 09:00");
            ApproachSpeedOptions options = new ApproachSpeedOptions("7185", start, end, 100, 0, 15, 3, true, 10, true, true, true, true);

            options.CreateMetric();



            var bucket = options.SpeedDetectors[0].AvgSpeedBucketCollection.AvgSpeedBuckets[0];

            var cycle = options.SpeedDetectors[0].Cycles.Where(c => c.SpeedEvents.Count >= 5).FirstOrDefault();

            /*while I can't be sure which, if any are right, I can say that 
            this query:
                    Select* from Speed_Events
                    where DetectorID = '718518' and timestamp between '02/01/2018 08:02:47' and '02/01/2018 08:03:44'
                    and MPH > 5
            Produces 46 records when run from the produciton server, 
            and that the sum of the MPH for those events is 1797*/


           Assert.IsTrue(options.SpeedDetectors.Count > 1);

           Assert.IsTrue(options.SpeedDetectors[0].Cycles.Count == 29);

           Assert.IsTrue(cycle.SpeedEvents.Count == 46);

            Assert.IsTrue(cycle.SpeedEvents.Sum(s => s.MPH) == 1797);




        }
    }
}
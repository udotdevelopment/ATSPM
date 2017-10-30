using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavetronicsSpeedListener.Models;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class AvgSpeedBucketCollectionTests
    {
        [TestMethod()]
        public void AvgSpeedBucketCollectionTest()
        {
            List<Cycle> cycles = new List<Cycle>(); 
            AvgSpeedBucketCollection asbc = new AvgSpeedBucketCollection(DateTime.Now.AddDays(-1), DateTime.Now, cycles, 5, 5, 5);

            List<Models.Speed_Events> lse = new List<Models.Speed_Events>();

            Models.Speed_Events se1 = new Models.Speed_Events();
            se1.DetectorID = "100001";
            se1.MPH = 30;
            se1.timestamp = Convert.ToDateTime("10/26/2017 12:01");

            Models.Speed_Events se2 = new Models.Speed_Events();
            se2.DetectorID = "100001";
            se2.MPH = 30;
            se2.timestamp = Convert.ToDateTime("10/26/2017 12:02");

            Models.Speed_Events se3 = new Models.Speed_Events();
            se3.DetectorID = "100001";
            se3.MPH = 30;
            se3.timestamp = Convert.ToDateTime("10/26/2017 12:03");

            lse.Add(se1);
            lse.Add(se2);
            lse.Add(se3);


            int testAverage = asbc.GetAverageSpeed(lse);

            Assert.IsTrue(testAverage == 30);
        }

        [TestMethod()]
        public void AddItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAverageSpeedTest()
        {
            Assert.Fail();
        }
    }
}
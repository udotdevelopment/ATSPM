using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class SplitFailOptionsTests
    {
        [TestMethod()]
        public void CreateMetricTest()
        {
            string signalId = "7220";
            DateTime start = Convert.ToDateTime("10/17/2017 17:00:00");
            DateTime end = Convert.ToDateTime("10/17/2017 18:00:00");

            SplitFailOptions sfo = new SplitFailOptions(signalId, start, end,12,5,true,true,false);
            List<string> path = sfo.CreateMetric();

            Assert.IsTrue(path.Count > 0);
        }
    }
}
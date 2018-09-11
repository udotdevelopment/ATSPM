using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Business.WCFServiceLibrary.NonAggregateCharts;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachDelayOptionsTests
    {
        private InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
            ChartTestHelper.InitializeTestDataFor7185Feb012018(db);

        }
        [TestMethod()]
        public void CreateApproachDelayMetricTest()
        {
            var start = Convert.ToDateTime("02/01/2018 00:01");
            var end = Convert.ToDateTime("02/01/2018 23:59");
            ApproachDelayOptions options = new ApproachDelayOptions("7185", start, end, 15, 2000, 15, true, true);
            Assert.IsTrue(options.CreateMetric().Count > 1);
        }
    }
}
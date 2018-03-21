using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Business.WCFServiceLibrary.NonAggregateCharts;
using MOE.CommonTests.Helpers;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class PCDOptionsTests

    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        [TestInitialize()]
        public void Initialize()
        {
               ChartTestHelper.InitializeTestDataFor7185Feb012018(db);
        }

        [TestMethod()]
        public void PcdOptionsTest()
        {
            var start = Convert.ToDateTime("02/01/2018 00:01");
            var end = Convert.ToDateTime("02/01/2018 23:59");
            PCDOptions options = new PCDOptions("7185", start, end, 100, 5000, 15,2, true, true, 6, true );
            Assert.IsTrue(options.CreateMetric().Count > 1);
        }
    }
}
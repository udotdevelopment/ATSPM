using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Business.WCFServiceLibrary.NonAggregateCharts
{
    [TestClass()]
    public class AoROptionsTests
    {
        InMemoryMOEDatabase db = new InMemoryMOEDatabase();

        DateTime _start = Convert.ToDateTime("02/01/2018 00:01");
        DateTime _end = Convert.ToDateTime("02/01/2018 23:59");
        private int _metricTypeId = 9;

        [TestInitialize()]
        public void Initialize()
        {
            ChartTestHelper.InitializeTestDataFor7185Feb012018(db);
        }

        [TestMethod()]
        public void AoROptionsTest()
        {

            AoROptions options = new AoROptions("7185", _start, _end,true,15);
            options.MetricFileLocation = @"C:\SPMImages\";
            Assert.IsTrue(options.CreateMetric().Count > 1);
        }

        [TestMethod()]
        public void AoRChartsUseOnlyAdvancedCountDetectors()
        {

            var repository = SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate("7185", _start);
            var approaches = signal.GetApproachesForSignalThatSupportMetric(_metricTypeId);

            var ACapprs = new List<Approach>();

            foreach (var app in approaches)
            {
                foreach (var det in app.Detectors)
                {
                    if (det.DetectionTypeIDs.Contains(2))
                    {
                        ACapprs.Add(app);
                        break;
                    }
                }
            }

            Assert.IsTrue(ACapprs.Count == approaches.Count);
        }
    }
}
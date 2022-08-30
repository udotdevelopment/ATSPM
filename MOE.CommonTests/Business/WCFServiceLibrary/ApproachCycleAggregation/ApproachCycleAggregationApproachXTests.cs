using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Business.WCFServiceLibrary.Tests;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Business.WCFServiceLibrary.ApproachCycleAggregation
{
    [TestClass]
    public class ApproachCycleAggregationApproachOptionsTests : ApproachAggregationCreateMetricTestsBase
    {

        

        protected override void SetSpecificAggregateRepositoriesForTest()
        {
            var signals = Db.Signals;
            foreach (var signal in signals)
            {
                foreach (var approach in signal.Approaches)
                {
                    PopulateApproachData(approach);
                }
            }
            ApproachCycleAggregationRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApproachCycleAggregationRepository(Db));
        }

        protected override void PopulateApproachData(Approach approach)
        {
            Db.PopulateApproachCycleAggregationsWithRandomRecords(Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"), approach);
        }

        [TestMethod]
        public void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest()
        {

             ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions();
           
            base.CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest(options);
        }
    }
}
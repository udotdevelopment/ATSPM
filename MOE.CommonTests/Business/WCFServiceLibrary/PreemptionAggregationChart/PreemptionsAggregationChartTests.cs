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

namespace MOE.CommonTests.Business.WCFServiceLibrary.PreemptionAggregationChart
{
    [TestClass]
    public class PreemptionsAggregationChartTests: SignalAggregationCreateMetricTestsBase
    {
        protected override void SetSpecificAggregateRepositoriesForTest()
        {
            SignalEventCountAggregationRepositoryFactory.SetRepository(new InMemorySignalEventCountAggregationRepository(Db));
            PreemptAggregationDatasRepositoryFactory.SetArchivedMetricsRepository(new InMemoryPreemptAggregationDatasRepository(Db));
            foreach (var signal in Db.Signals)
            {
                PopulateSignalData(signal);
            }
        }

        protected override void PopulateSignalData(Signal signal)
        {
            Db.PopulatePreemptAggregations(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal.SignalID, signal.VersionID);
            Db.PopulateSignalEventCountwithRandomValues(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal);
        }

        [TestMethod]
        public void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest()
        {
            var options = new PreemptionAggregationOptions();
            base.CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest(options);
        }
    }
}

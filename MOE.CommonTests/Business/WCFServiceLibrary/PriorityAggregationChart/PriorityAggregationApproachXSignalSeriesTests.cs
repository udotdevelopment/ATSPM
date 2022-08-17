using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Business.WCFServiceLibrary;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class PriorityAggregationTests: SignalAggregationCreateMetricTestsBase
    {


        protected override void SetSpecificAggregateRepositoriesForTest()
        {
            var signals = Db.Signals;

            SignalEventCountAggregationRepositoryFactory.SetRepository(new InMemorySignalEventCountAggregationRepository(Db));
            PriorityAggregationDatasRepositoryFactory.SetRepository(new InMemoryPriorityAggregationDatasRepository(Db));

            foreach (var signal in signals)
            {
                PopulateSignalData(signal);
            }
        }

        protected override void PopulateSignalData(Models.Signal signal)
        {
            Db.PopulatePriorityAggregations(Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"),signal.SignalID, signal.VersionID);

            Db.PopulateSignalEventCountwithRandomValues(Convert.ToDateTime("1/1/2016"), Convert.ToDateTime("1/1/2018"), signal);
        }

        [TestMethod]
        public void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest()
        {
            var options = new PriorityAggregationOptions();
            base.CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest(options);
        }
    }
}
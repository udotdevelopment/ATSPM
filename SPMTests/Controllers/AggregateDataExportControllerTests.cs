using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.CommonTests.Models;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class AggregateDataExportControllerTests
    {
        private InMemoryMOEDatabase _db = new InMemoryMOEDatabase();
        [TestInitialize]
        public void Initialize()
        {
            _db.ClearTables();
            _db.PopulateMetricTypes();
            _db.PopulateSignal();
            _db.PopulateRoutes();
            _db.PopulateRouteWithRouteSignals();
            _db.PopulateRouteSignalsWithPhaseDirection();

            InMemoryRouteRepository routeRepo = new InMemoryRouteRepository(_db);
            MOE.Common.Models.Repositories.RouteRepositoryFactory.SetApproachRouteRepository(routeRepo);
        }
        [TestMethod()]
        public void CreateMetricTest()
        {
            int a = 0;
            Assert.IsTrue(a==0);
            int id = 20; //Aggregate Metric
            var TestController = new AggregateDataExportController();
            var TestActionResult = TestController.CreateMetric(20);
            Assert.IsNull(TestActionResult);
        }
    }
}
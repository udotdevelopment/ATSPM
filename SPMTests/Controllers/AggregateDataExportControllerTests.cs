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
            _db.PopulateMovementTypes();
            _db.PopulateDirectionTypes();
            _db.PopulateLaneTypes();
            _db.PopulateSignal();
            _db.PopulateRoutes();
            _db.PopulateRegions();

            _db.PopulateRouteWithRouteSignals();
            _db.PopulateRouteSignalsWithPhaseDirection();

            InMemoryRouteRepository routeRepo = new InMemoryRouteRepository(_db);
            MOE.Common.Models.Repositories.RouteRepositoryFactory.SetApproachRouteRepository(routeRepo);
            InMemorySignalsRepository signalRepo = new InMemorySignalsRepository(_db);
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.SetSignalsRepository(signalRepo);
            InMemoryRegionRepository regionRepo = new InMemoryRegionRepository(_db);
            MOE.Common.Models.Repositories.RegionsRepositoryFactory.SetArchivedMetricsRepository(regionRepo);

            InMemoryMetricTypeRepository metricRepo = new InMemoryMetricTypeRepository();
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.SetMetricsRepository(metricRepo);
            InMemoryMovementTypeRepository movementRepo = new InMemoryMovementTypeRepository();
            MOE.Common.Models.Repositories.MovementTypeRepositoryFactory.SetMovementTypeRepository(movementRepo);
            InMemoryDirectionTypeRepository directionTypeRepo = new InMemoryDirectionTypeRepository();
            MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.SetDirectionsRepository(directionTypeRepo);
            InMemoryLaneTypeRepository laneTypeRepo = new InMemoryLaneTypeRepository();
            MOE.Common.Models.Repositories.LaneTypeRepositoryFactory.SetLaneTypeRepository(laneTypeRepo);

        }
        [TestMethod()]
        public void CreateMetricTest()
        {
            //int a = 0;
            //Assert.IsTrue(a==0);
            int id = 1; //RouteID
            var TestController = new AggregateDataExportController();
            var TestActionResult = TestController.CreateMetric(id);
            Assert.IsNotNull(TestActionResult);
        }
    }
}
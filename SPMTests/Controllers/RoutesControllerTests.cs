using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.CommonTests.Models;

namespace SPM.Controllers.Tests
{
    [TestClass()]
    public class RoutesControllerTests
    {
        private InMemoryMOEDatabase _db = new InMemoryMOEDatabase();
        [TestInitialize]
        public void Initialize()
        {
            _db.ClearTables();
            _db.PopulateSignal();
            _db.PopulateRoutes();
            _db.PopulateRouteWithRouteSignals();
            _db.PopulateRouteSignalsWithPhaseDirection();

            InMemoryRouteRepository routeRepo = new InMemoryRouteRepository(_db);
            MOE.Common.Models.Repositories.RouteRepositoryFactory.SetApproachRouteRepository(routeRepo);
        }

        [TestMethod()]
        public void IndexTest()
        {
            var controller = new SPM.Controllers.RoutesController();
            var result = controller.Index() as ViewResult;
            var routes = result.Model as List<Route>;

            Assert.IsTrue(routes.Count > 0);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            var controller = new RoutesController();
            var result = controller.Details(1) as PartialViewResult;
            var route = result.Model as Route;

            Assert.IsTrue(route.RouteName == "Route - 1");

        }

        [TestMethod()]
        public void CreateTest()
        {
            var controller = new RoutesController();
            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void CreateTest1()
        {
            var controller = new RoutesController();
            Route route = new Route();
            route.Id = 5;
            route.RouteName = "Route - 5";

            var result = controller.Create(route);

            Assert.IsTrue(_db.Routes.Contains(route));
        }

        [TestMethod()]
        public void EditTest()
        {
            var controller = new RoutesController();
            var result = controller.Edit(1) as ViewResult;
            var route = result.Model as Route;

            Assert.IsTrue(route.RouteName == "Route - 1");

        }

        [TestMethod()]
        public void EditTest1()
        {
            var controller = new RoutesController();
            var result = controller.Edit(1) as ViewResult;
            var preEditRoute = result.Model as Route;
            preEditRoute.RouteName = "I changed this";

            controller.Edit(preEditRoute);

            var postEditRoute = _db.Routes.Find(r => r.Id == 1);

            Assert.IsTrue(postEditRoute.RouteName == "I changed this");
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var controller = new RoutesController();
            var result = controller.Delete(1) as ViewResult;
            var route = result.Model as Route;

            Assert.IsTrue(route.RouteName == "Route - 1");


        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            var routeThere = _db.Routes.Find(r => r.Id == 1);

            Assert.IsNotNull(routeThere);

            var controller = new RoutesController();
            controller.DeleteConfirmed(1);
            var routeGone = _db.Routes.Find(r => r.Id == 1);

            Assert.IsNull(routeGone);
        }
    }
}
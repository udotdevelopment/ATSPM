using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;
using MOE.Common.Models.ViewModel.RouteEdit;

namespace SPM.Controllers
{
    [Authorize(Roles = "Configuration, Admin, Technician")]
    public class RoutesController : Controller
    {
        MOE.Common.Models.Repositories.IRouteRepository routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();

        // GET: Routes
        public ActionResult Index()
        {
            return View(routeRepository.GetAllRoutes());
        }

        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var route = routeRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return HttpNotFound();
            }
            return PartialView(route);
        }

        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Create()
        {
            Route route = new Route();
            return View(route);
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteName")] Route route)
        {
            if (ModelState.IsValid)
            {
                routeRepository.Add(route);
                return RedirectToAction("Index/" + route.Id.ToString(), "RouteSignals");
            }

            return View(route);
        }

        // GET: Routes/Edit/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = routeRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteName")] Route route)
        {
            if (ModelState.IsValid)
            {
                routeRepository.Update(route);
                return RedirectToAction("Index");
            }
            return View(route);
        }

        // GET: Routes/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = routeRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            routeRepository.DeleteByID(id);
            return RedirectToAction("Index");
        }

    }
}

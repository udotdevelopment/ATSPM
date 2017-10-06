using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.RouteEdit;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApproachRoutesController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        // GET: ApproachRoutes
        public ActionResult Index()
        {
            return View(db.Routes.OrderBy(a => a.RouteName).ToList());
        }

        // GET: ApproachRoutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route approachRoute = db.Routes.Find(id);
            if (approachRoute == null)
            {
                return HttpNotFound();
            }
            return View(approachRoute);
        }

        public ActionResult RouteInfoBox(string signalID)
        {
            SignalInfoBoxViewModel viewModel = new SignalInfoBoxViewModel(signalID);
            return PartialView("RouteInfoBox", viewModel);
        }

        public ActionResult RouteMap(MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel ssvm)
        {
            return PartialView(ssvm);
        }

        // GET: ApproachRoutes/Create
        public ActionResult Create()
        {
            RouteCreateViewModel viewModel = new RouteCreateViewModel();
            return View(viewModel);
        }

        // POST: ApproachRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApproachRouteId,RouteName")] Route approachRoute)
        {
            if (ModelState.IsValid)
            {
                db.Routes.Add(approachRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(approachRoute);
        }

        // GET: ApproachRoutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route approachRoute = db.Routes.Find(id);
            if (approachRoute == null)
            {
                return HttpNotFound();
            }
            return View(approachRoute);
        }

        // POST: ApproachRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApproachRouteId,RouteName")] Route approachRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approachRoute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(approachRoute);
        }

        // GET: ApproachRoutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route approachRoute = db.Routes.Find(id);
            if (approachRoute == null)
            {
                return HttpNotFound();
            }
            return View(approachRoute);
        }

        // POST: ApproachRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Route approachRoute = db.Routes.Find(id);
            db.Routes.Remove(approachRoute);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

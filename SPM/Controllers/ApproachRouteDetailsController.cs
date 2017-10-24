using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;

namespace SPM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApproachRouteDetailsController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        // GET: RouteSignals
        public ActionResult Index(int id)
        {
            MOE.Common.Models.Repositories.IRouteRepository routeRepository =
                MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            var route = routeRepository.GetRouteByID(id);
            if (route == null)
            {
                return Content("<h1>Route Not Found");
            }
            MOE.Common.Models.ViewModel.RouteEdit.ApproachRouteDetailViewModel viewModel =
                new MOE.Common.Models.ViewModel.RouteEdit.ApproachRouteDetailViewModel();
            viewModel.RouteSignals = new List<RouteSignal>();
            viewModel.RouteID = id;
            viewModel.RouteName = route.RouteName;
            viewModel.RouteSignals = db.RouteSignals
                
                .Include(a => a.Route)
                .Where(a => a.RouteId == id)
                .OrderBy(a => a.Order)
                .ToList();
            return View(viewModel);
        }

        // GET: RouteSignals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteSignal approachRouteDetail = db.RouteSignals.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            return View(approachRouteDetail);
        }

        // GET: RouteSignals/Create
        public ActionResult Create(int id)
        {
            RouteSignal approachRouteDetail = new RouteSignal { RouteId = id };
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription");
            ViewBag.ApproachRouteId = new SelectList(db.Routes, "RouteId", "RouteName", id);
            return View(approachRouteDetail);
        }

        // POST: RouteSignals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RouteDetailID,RouteId,ApproachOrder,ApproachID")] RouteSignal approachRouteDetail)
        {
            if (ModelState.IsValid)
            {
                db.RouteSignals.Add(approachRouteDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = approachRouteDetail.RouteId });
            }
            
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.SignalId);
            ViewBag.ApproachRouteId = new SelectList(db.Routes, "RouteId", "RouteName", approachRouteDetail.RouteId);
            return View(approachRouteDetail);
        }

        // GET: RouteSignals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteSignal approachRouteDetail = db.RouteSignals.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.SignalId);
            ViewBag.ApproachRouteId = new SelectList(db.Routes, "RouteId", "RouteName", approachRouteDetail.RouteId);
            return View(approachRouteDetail);
        }

        // POST: RouteSignals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RouteDetailID,RouteId,ApproachOrder,ApproachID")] RouteSignal approachRouteDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approachRouteDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = approachRouteDetail.RouteId });
            }
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.SignalId);
            ViewBag.ApproachRouteId = new SelectList(db.Routes, "RouteId", "RouteName", approachRouteDetail.RouteId);
            return View(approachRouteDetail);
        }

        // GET: RouteSignals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteSignal approachRouteDetail = db.RouteSignals.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            return View(approachRouteDetail);
        }

        // POST: RouteSignals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteSignal approachRouteDetail = db.RouteSignals.Find(id);
            db.RouteSignals.Remove(approachRouteDetail);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = approachRouteDetail.RouteId });
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

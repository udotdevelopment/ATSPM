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

        // GET: ApproachRouteDetails
        public ActionResult Index(int id)
        {
            MOE.Common.Models.Repositories.IApproachRouteRepository routeRepository =
                MOE.Common.Models.Repositories.ApproachRouteRepositoryFactory.Create();
            var route = routeRepository.GetRouteByID(id);
            if (route == null)
            {
                return Content("<h1>Route Not Found");
            }
            else
            {

                MOE.Common.Models.ViewModel.RouteEdit.ApproachRouteDetailViewModel viewModel =
                    new MOE.Common.Models.ViewModel.RouteEdit.ApproachRouteDetailViewModel();
                viewModel.Approaches = new List<ApproachRouteDetail>();
                viewModel.RouteID = id;
                viewModel.RouteName = route.RouteName;
                viewModel.Approaches = db.ApproachRouteDetails
                    .Include(a => a.Approach)
                    .Include(a => a.ApproachRoute)
                    .Where(a => a.ApproachRouteId == id)
                    .OrderBy(a => a.ApproachOrder)
                    .ToList();
                return View(viewModel);
            }
        }

        // GET: ApproachRouteDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApproachRouteDetail approachRouteDetail = db.ApproachRouteDetails.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            return View(approachRouteDetail);
        }

        // GET: ApproachRouteDetails/Create
        public ActionResult Create(int id)
        {
            ApproachRouteDetail approachRouteDetail = new ApproachRouteDetail { ApproachRouteId = id };
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription");
            ViewBag.ApproachRouteId = new SelectList(db.ApproachRoutes, "ApproachRouteId", "RouteName", id);
            return View(approachRouteDetail);
        }

        // POST: ApproachRouteDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RouteDetailID,ApproachRouteId,ApproachOrder,ApproachID")] ApproachRouteDetail approachRouteDetail)
        {
            if (ModelState.IsValid)
            {
                db.ApproachRouteDetails.Add(approachRouteDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = approachRouteDetail.ApproachRouteId });
            }

            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.ApproachID);
            ViewBag.ApproachRouteId = new SelectList(db.ApproachRoutes, "ApproachRouteId", "RouteName", approachRouteDetail.ApproachRouteId);
            return View(approachRouteDetail);
        }

        // GET: ApproachRouteDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApproachRouteDetail approachRouteDetail = db.ApproachRouteDetails.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.ApproachID);
            ViewBag.ApproachRouteId = new SelectList(db.ApproachRoutes, "ApproachRouteId", "RouteName", approachRouteDetail.ApproachRouteId);
            return View(approachRouteDetail);
        }

        // POST: ApproachRouteDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RouteDetailID,ApproachRouteId,ApproachOrder,ApproachID")] ApproachRouteDetail approachRouteDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approachRouteDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = approachRouteDetail.ApproachRouteId });
            }
            ViewBag.ApproachID = new SelectList(db.Approaches, "ApproachID", "ApproachRouteDescription", approachRouteDetail.ApproachID);
            ViewBag.ApproachRouteId = new SelectList(db.ApproachRoutes, "ApproachRouteId", "RouteName", approachRouteDetail.ApproachRouteId);
            return View(approachRouteDetail);
        }

        // GET: ApproachRouteDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApproachRouteDetail approachRouteDetail = db.ApproachRouteDetails.Find(id);
            if (approachRouteDetail == null)
            {
                return HttpNotFound();
            }
            return View(approachRouteDetail);
        }

        // POST: ApproachRouteDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApproachRouteDetail approachRouteDetail = db.ApproachRouteDetails.Find(id);
            db.ApproachRouteDetails.Remove(approachRouteDetail);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = approachRouteDetail.ApproachRouteId });
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

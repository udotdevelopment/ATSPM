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
    public class RoutePhaseDirectionsController : Controller
    {
        //private SPM db = new SPM();

        //// GET: RoutePhaseDirections
        //public ActionResult Index()
        //{
        //    return View(db.RoutePhaseDirections.ToList());
        //}

        //// GET: RoutePhaseDirections/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Find(id);
        //    if (routePhaseDirection == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(routePhaseDirection);
        //}

        //// GET: RoutePhaseDirections/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: RoutePhaseDirections/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ApproachRouteDetailId,Phase,IsPhaseDirection1Overlap")] RoutePhaseDirection routePhaseDirection)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RoutePhaseDirections.Add(routePhaseDirection);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(routePhaseDirection);
        //}

        //// GET: RoutePhaseDirections/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Find(id);
        //    if (routePhaseDirection == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(routePhaseDirection);
        //}

        //// POST: RoutePhaseDirections/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ApproachRouteDetailId,Phase,IsPhaseDirection1Overlap")] RoutePhaseDirection routePhaseDirection)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(routePhaseDirection).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(routePhaseDirection);
        //}

        //// GET: RoutePhaseDirections/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Find(id);
        //    if (routePhaseDirection == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(routePhaseDirection);
        //}

        //// POST: RoutePhaseDirections/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Find(id);
        //    db.RoutePhaseDirections.Remove(routePhaseDirection);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

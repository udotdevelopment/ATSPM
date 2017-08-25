using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;

namespace SecurityConfiguration.Controllers
{
    public class SPMRolesController : Controller
    {
        private SPM db = new SPM();

        // GET: SPMRoles
        public ActionResult Index()
        {
            return View(db.IdentityRoles.ToList());
        }

        // GET: SPMRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMRole sPMRole = db.IdentityRoles.Find(id);
            if (sPMRole == null)
            {
                return HttpNotFound();
            }
            return View(sPMRole);
        }

        // GET: SPMRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SPMRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] SPMRole sPMRole)
        {
            if (ModelState.IsValid)
            {
                db.IdentityRoles.Add(sPMRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sPMRole);
        }

        // GET: SPMRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMRole sPMRole = db.IdentityRoles.Find(id);
            if (sPMRole == null)
            {
                return HttpNotFound();
            }
            return View(sPMRole);
        }

        // POST: SPMRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] SPMRole sPMRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sPMRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sPMRole);
        }

        // GET: SPMRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMRole sPMRole = db.IdentityRoles.Find(id);
            if (sPMRole == null)
            {
                return HttpNotFound();
            }
            return View(sPMRole);
        }

        // POST: SPMRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SPMRole sPMRole = db.IdentityRoles.Find(id);
            db.IdentityRoles.Remove(sPMRole);
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

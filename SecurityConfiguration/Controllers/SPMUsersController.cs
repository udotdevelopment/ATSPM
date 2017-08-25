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
    public class SPMUsersController : Controller
    {
        private SPM db = new SPM();

        // GET: SPMUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: SPMUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMUser sPMUser = db.Users.Find(id);
            if (sPMUser == null)
            {
                return HttpNotFound();
            }
            return View(sPMUser);
        }

        // GET: SPMUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SPMUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] SPMUser sPMUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(sPMUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sPMUser);
        }

        // GET: SPMUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMUser sPMUser = db.Users.Find(id);
            if (sPMUser == null)
            {
                return HttpNotFound();
            }
            return View(sPMUser);
        }

        // POST: SPMUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] SPMUser sPMUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sPMUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sPMUser);
        }

        // GET: SPMUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMUser sPMUser = db.Users.Find(id);
            if (sPMUser == null)
            {
                return HttpNotFound();
            }
            return View(sPMUser);
        }

        // POST: SPMUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SPMUser sPMUser = db.Users.Find(id);
            db.Users.Remove(sPMUser);
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

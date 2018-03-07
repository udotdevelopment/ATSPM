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
    public class DatabaseArchiveSettingsController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        

        // GET: DatabaseArchiveSettings/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name");
            return View();
        }

        // POST: DatabaseArchiveSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplicationID,SelectedUseArchive,SelectedTablePartition,MonthsToRemoveIndex,MonthsToRemoveData,ArchivePath,SelectedDeleteOrMove,NumberOfRows,StartTime,TimeDuration")] DatabaseArchiveSettings databaseArchiveSettings)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationSettings.Add(databaseArchiveSettings);
                db.SaveChanges();
                return View(databaseArchiveSettings);
            }

            ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name", databaseArchiveSettings.ApplicationID);
            return View(databaseArchiveSettings);
        }

        //// GET: DatabaseArchiveSettings/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DatabaseArchiveSettings databaseArchiveSettings = db.ApplicationSettings.Find(id);
        //    if (databaseArchiveSettings == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name", databaseArchiveSettings.ApplicationID);
        //    return View(databaseArchiveSettings);
        //}

        //// POST: DatabaseArchiveSettings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,ApplicationID,SelectedUseArchive,SelectedTablePartition,MonthsToRemoveIndex,MonthsToRemoveData,ArchivePath,SelectedDeleteOrMove,NumberOfRows,StartTime,TimeDuration")] DatabaseArchiveSettings databaseArchiveSettings)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(databaseArchiveSettings).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name", databaseArchiveSettings.ApplicationID);
        //    return View(databaseArchiveSettings);
        //}
        
        

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

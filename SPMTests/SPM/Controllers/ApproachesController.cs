using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using SPM.Filters;

namespace SPM.Controllers
{
    public class ApproachesController : Controller
    {
        //private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
        MOE.Common.Models.Repositories.IApproachRepository approachRepository =
                    MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
        MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IDirectionTypeRepository directionRepository =
                MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.Create();
        // GET: Signals/Copy
        [Authorize(Roles = "Admin")]
        public ActionResult Copy(int approachID)
        {
            ViewBag.DirectionType = new SelectList(directionRepository.GetAllDirections(), "DirectionTypeID", "Abbreviation");
            MOE.Common.Models.Repositories.IApproachRepository approachRepository =
                MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            MOE.Common.Models.Approach approachToCopy = approachRepository.GetApproachByApproachID(approachID);

            Approach newApproach = MOE.Common.Models.Approach.CopyApproach(approachID);
            return PartialView("Create",newApproach);
        }

        
        // GET: RouteSignals/Create
         [Authorize(Roles = "Admin")]
        public ActionResult Create(string id)
        {
            Approach approach = new Approach();
            approach.SignalID = id;
            approach.DirectionTypeID = 1;
            approach.ApproachID = 0;
            ViewBag.DirectionType = new SelectList(directionRepository.GetAllDirections(), "DirectionTypeID", "Abbreviation");            
            return PartialView(approach);
        }

        // POST: RouteSignals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SignalId,DirectionTypeID,Description,MPH,DecisionPoint,MovementDelay")] Approach approach)
        {
            
            ViewBag.DirectionType = new SelectList(directionRepository.GetAllDirections(), "DirectionTypeID", "Abbreviation");            
            if (ModelState.IsValid)
            {

                approachRepository.AddOrUpdate(approach);
                approach.Signal = signalRepository.GetLatestVersionOfSignalBySignalID(approach.SignalID);
                return PartialView("~/Views/Signals/EditorTemplates/Approach.cshtml", approach);
            }
            return PartialView(approach);
        }

        

        // GET: RouteSignals/Delete/5
         [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Approach approach = approachRepository.GetApproachByApproachID(id.Value);
            if (approach == null)
            {
                return HttpNotFound();
            }
            return View(approach);
        }

        // POST: RouteSignals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public void DeleteConfirmed(int id)
        {
            approachRepository.Remove(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}

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
    public class DetectorsController : Controller
    {
        //private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
        MOE.Common.Models.Repositories.IDetectionTypeRepository detectionTypeRepository =
            MOE.Common.Models.Repositories.DetectionTypeRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IDetectorRepository detectorRepository =
            MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IDetectionHardwareRepository detectionHardwareRepository =
            MOE.Common.Models.Repositories.DetectionHardwareRepositoryFactory.Create();

        //Use the one in SignalsController.cs!!
        //// GET: Detectors/Copy
        //[Authorize(Roles = "Admin")]
        //public ActionResult Copy(string detectorID, string mvcPath)
        //{
        //    Detector newDetector = MOE.Common.Models.Detector.CopyDetector(detectorID);
        //    return PartialView("Create", newDetector);
        //}
        
        // GET: Detectors/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {
            MOE.Common.Models.Detector detector = new Detector();
            //detector.LaneID = id; //should be approachID
            //detector.DetectionIDs = new string[]();
            detector.AllDetectionTypes = detectionTypeRepository.GetAllDetectionTypes();
            detector.DetectionTypes = new List<MOE.Common.Models.DetectionType>();

            detector.AllHardwareTypes = detectionHardwareRepository.GetAllDetectionHardwares();
            detector.DetectionHardware = new DetectionHardware();

            detector.DateAdded = DateTime.Now;

            return PartialView(detector);
        }

        // POST: Detectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateJsonAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult Create([Bind(Include = "LaneID, LaneNumber, DetChannel, DetectorID, DistanceFromStopBar,MinSpeedFilter,Enabled,DateAdded,DetectionTypeIDs, DetectionIDs")] Detector detectors)
        //{
        //    ModelState.Clear();
        //    detectors.AllDetectionTypes = detectionTypeRepository.GetAllDetectionTypesNoBasic();
        //    if (detectors.DetectionTypeIDs == null)
        //    {
        //        detectors.DetectionTypeIDs = new List<int>();                              
        //    }
        //    detectors.DetectionTypeIDs.Add(1);
        //    if (detectors.DetectionIDs != null)
        //    {
        //        foreach (string detectionTypeID in detectors.DetectionIDs)
        //        {
        //            detectors.DetectionTypeIDs.Add(Convert.ToInt32(detectionTypeID));
        //        }
        //    }
        //    else
        //    {
        //        detectors.DetectionIDs = new string[] { };
        //    }
        //    if(detectors.DetectionTypes == null)
        //    {
        //        detectors.DetectionTypes = new List<MOE.Common.Models.DetectionType>();
        //    }
        //    foreach(int id in detectors.DetectionTypeIDs)
        //    {
        //        detectors.DetectionTypes.Add(detectionTypeRepository.GetDetectionTypeByDetectionTypeID(id));
        //    }
        //    if (TryValidateModel(detectors))
        //    { 
        //        MOE.Common.Models.Repositories.ILaneRepository laneRepository =
        //            MOE.Common.Models.Repositories.LaneRepositoryFactory.Create();
        //        detectorRepository.Add(detectors);
        //        detectors.Lane = laneRepository.GetLaneByLaneID(detectors.LaneID.Value);                
        //        return PartialView("~/Views/Signals/EditorTemplates/Detector.cshtml", detectors);
        //    }
        //    return PartialView(detectors);
        //}


        // GET: Detectors/Delete/5
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detector detectors = detectorRepository.GetDetectorByID(id);
            if (detectors == null)
            {
                return HttpNotFound();
            }
            return View(detectors);
        }

        // POST: Detectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public void DeleteConfirmed(int id)
        {
            detectorRepository.Remove(id);
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

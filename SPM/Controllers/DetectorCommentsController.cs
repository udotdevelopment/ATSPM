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
    public class DetectorCommentsController : Controller
    {
        MOE.Common.Models.Repositories.IDetectorCommentRepository detectorCommentRepository =
            MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.Create();

        // GET: DetectorComments
        //public ActionResult Index()
        //{
        //    var detectorComments = detectorCommentRepository.GetAllDetectorComments();
        //    return View(detectorComments.ToList());
        //}

        // GET: DetectorComments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DetectorComment detectorComment = detectorCommentRepository.GetDetectorCommentByDetectorCommentID(id.Value);
        //    if (detectorComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(detectorComment);
        //}

        // GET: DetectorComments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {
            MOE.Common.Models.DetectorComment dc = new DetectorComment();
            dc.ID = id;
            return PartialView(dc);
        }

        // POST: DetectorComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CommentID, ID,TimeStamp,CommentText")] DetectorComment detectorComment)
        {
            if (ModelState.IsValid)
            {
                detectorComment.TimeStamp = DateTime.Now;
                detectorCommentRepository.AddOrUpdate(detectorComment);
                return PartialView("~/Views/Signals/EditorTemplates/DetectorComment.cshtml", detectorComment);                
            }

            return PartialView("~/Views/Signals/EditorTemplates/DetectorComment.cshtml", detectorComment);    
        }

        //// GET: DetectorComments/Edit/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DetectorComment detectorComment = detectorCommentRepository.GetDetectorCommentByDetectorCommentID(id.Value);
        //    if (detectorComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(detectorComment);
        //}

        //// POST: DetectorComments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult Edit([Bind(Include = "CommentID,ID,TimeStamp,CommentText")] DetectorComment detectorComment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        detectorCommentRepository.AddOrUpdate(detectorComment);
        //        return RedirectToAction("Index");
        //    }
        //    return View(detectorComment);
        //}

        //// GET: DetectorComments/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DetectorComment detectorComment = detectorCommentRepository.GetDetectorCommentByDetectorCommentID(id.Value);
        //    if (detectorComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(detectorComment);
        //}

        //// POST: DetectorComments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DetectorComment detectorComment = detectorCommentRepository.GetDetectorCommentByDetectorCommentID(id);
        //    detectorCommentRepository.Remove(detectorComment);
        //    return RedirectToAction("Index");
        //}
    }
}

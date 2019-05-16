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
    public class MetricCommentsController : Controller
    {
        //private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        MOE.Common.Models.Repositories.IMetricTypeRepository metricTyperepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IMetricCommentRepository commentRepository =
        MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();

        // GET: MetricComments/Create
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Create(string versionId)
        {
            MOE.Common.Models.Repositories.ISignalsRepository signalsRepository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Signal signal = signalsRepository.GetSignalVersionByVersionId(Convert.ToInt32(versionId));
            List<MOE.Common.Models.MetricType> allMetricTypes = metricTyperepository.GetAllToDisplayMetrics();
            MOE.Common.Models.MetricComment mc =
                new MetricComment();
            mc.Signal = signal;
            mc.AllMetricTypes = allMetricTypes;
            if (mc.MetricTypeIDs != null)
            {
                mc.MetricTypes = metricTyperepository.GetMetricTypesByMetricComment(mc);
            }
            
            return PartialView(mc);
        }

        // POST: MetricComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Create([Bind(Include = "VersionID, SignalId,CommentText, MetricIDs")] MetricComment metricComment)
        {
            metricComment.TimeStamp = DateTime.Now;
            ModelState.Clear();
            metricComment.AllMetricTypes = null;
            if (metricComment.MetricTypeIDs == null)
            {
                metricComment.MetricTypeIDs = new List<int>();
            }

            if (metricComment.MetricTypeIDs != null)
            {
                foreach (int metricID in metricComment.MetricIDs)
                {
                    metricComment.MetricTypeIDs.Add(metricID);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    commentRepository.Add(metricComment);
                }
                catch(Exception ex)
                {
                    return Content(ex.Message);
                }
                return PartialView("~/Views/Signals/EditorTemplates/MetricComment.cshtml",metricComment);
            }

            return View(metricComment);
        }

        // GET: MetricComments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetricComment metricComment = commentRepository.GetMetricCommentByMetricCommentID(id.Value);
            if (metricComment == null)
            {
                return HttpNotFound();
            }
            return View(metricComment);
        }

        // POST: MetricComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            commentRepository.Remove(commentRepository.GetMetricCommentByMetricCommentID(id));
            return RedirectToAction("Index");
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

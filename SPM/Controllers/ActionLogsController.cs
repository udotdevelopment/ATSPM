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
using Newtonsoft.Json;

namespace SPM.Controllers
{
    public class ActionLogsController : Controller
    {
        private MOE.Common.Models.Repositories.IGenericRepository<Agency> agencyRepository =
              MOE.Common.Models.Repositories.AgencyRepositoryFactory.Create();
        private MOE.Common.Models.Repositories.IGenericRepository<MOE.Common.Models.Action> actionRepository =
                MOE.Common.Models.Repositories.ActionRepositoryFactory.Create();
        private MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
        private MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IActionLogRepository actionLogRepository =
            MOE.Common.Models.Repositories.ActionLogRepositoryFactory.Create();

        // GET: ActionLogs
        public ActionResult Index()
        {
            MOE.Common.Models.Repositories.ActionLogRepository actionLogRepository =
                (MOE.Common.Models.Repositories.ActionLogRepository)MOE.Common.Models.Repositories.ActionLogRepositoryFactory.Create();
            var actionLog = actionLogRepository.GetAllForNumberOfDays(30);                
            return View(actionLog.ToList());
        }

        // GET: ActionLogs
        public ActionResult Usage()
        {
            MOE.Common.Models.ViewModel.MetricUsage.MetricUsageViewModel usageModel =
                new MOE.Common.Models.ViewModel.MetricUsage.MetricUsageViewModel();
            usageModel.StartDate = new DateTime(DateTime.Today.Year, 1, 1);
            usageModel.EndDate = DateTime.Today;
            usageModel.MetricTypes = metricTypeRepository.GetAllToDisplayMetrics();
            return View(usageModel);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ReportsRun(DateTime startDate, DateTime endDate)
        {
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData =
                GetChartViewModel("Reports Run", "ReportsRun", "# of Reports Run", "bar");
            MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            var metrics = metricTypeRepository.GetAllToDisplayMetrics();
            List<string> descriptions = new List<string>();      
            foreach (MetricType m in metrics)
            {
                descriptions.Add(m.ChartName +" Executed");
                reportData.ChartData.Add(new MOE.Common.Business.ActionLog.ChartData { Description = m.ChartName, Value = 0 });
            }
            var events = eventRepository.GetEventsByDateDescriptions(startDate, endDate, descriptions);
            foreach(ApplicationEvent ae in events)
            {
                var chartData = reportData.ChartData.Where(r => ae.Description.ToLower() == r.Description.ToLower() + " executed").FirstOrDefault();
                chartData.Value++;
            }
            SetColors(reportData);
            return PartialView("Chart", reportData);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetActionsByMetric(DateTime startDate, DateTime endDate)
        {
            var metrics = metricTypeRepository.GetAllToDisplayMetrics();
            var actionLogs = actionLogRepository.GetAllByDate(startDate, endDate);
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData = GetChartViewModel("Actions By Metric",
                "ActionsByMetric", "# of Actions", "bar");           
            List<string> descriptions = new List<string>();                
            foreach (MetricType m in metrics)
            {
                int value = 0;
                foreach(ActionLog al in actionLogs)
                {
                    foreach(MetricType mt in al.MetricTypes)
                    {
                        if(m.MetricID == mt.MetricID)
                        {
                            value += al.Actions.Count;
                        }
                    }
                }
                reportData.ChartData.Add(new MOE.Common.Business.ActionLog.ChartData { Description = m.ChartName, 
                    Value = value });
            }
            SetColors(reportData);
            return PartialView("Chart", reportData);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetMetricsUsage(DateTime startDate, DateTime endDate)
        {
            var metrics = metricTypeRepository.GetAllToDisplayMetrics();
            var actionLogs = actionLogRepository.GetAllByDate(startDate, endDate);
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData = GetChartViewModel("Chart Usage",
                "MetricsUsage", "% of Metrics Logged", "pie");
            List<string> descriptions = new List<string>();
            foreach (MetricType m in metrics)
            {
                int value = 0;
                foreach (ActionLog al in actionLogs)
                {
                    foreach (MetricType mt in al.MetricTypes)
                    {
                        if (m.MetricID == mt.MetricID)
                        {
                            value ++;
                        }
                    }
                }
                reportData.ChartData.Add(new MOE.Common.Business.ActionLog.ChartData
                {
                    Description = m.ChartName,
                    Value = value
                });
            }
            SetColors(reportData);
            return PartialView("Chart", reportData);
        }

        public ActionResult GetAgencyUsage(DateTime startDate, DateTime endDate)
        {
            var actionLogs = actionLogRepository.GetAllByDate(startDate, endDate);
            var agencies = agencyRepository.GetAll();
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData = GetChartViewModel("Agency Usage",
                "AgencyUsage", "# of Logs by Agency", "pie");
            List<string> descriptions = new List<string>();
            foreach(MOE.Common.Models.Agency a in agencies)
            {
                reportData.ChartData.Add(new MOE.Common.Business.ActionLog.ChartData
                {
                    Description = a.Description,
                    Value = actionLogs.Where(al => al.AgencyID == a.AgencyID).ToList().Count
                });
            }
            SetColors(reportData);
            return PartialView("Chart", reportData);
        }

        public ActionResult GetActionsByMetricID(DateTime startDate, DateTime endDate, int metricTypeID)
        {
            var metric = metricTypeRepository.GetMetricsByID(metricTypeID);
            var actionLogs = actionLogRepository.GetAllByDate(startDate, endDate);
            var actions = actionRepository.GetAll();
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData = GetChartViewModel(metric.ChartName,
                metric.Abbreviation, "# of Actions", "bar");
            foreach(MOE.Common.Models.Action a in actions)
            {
                reportData.ChartData.Add(new MOE.Common.Business.ActionLog.ChartData
                {
                    Description = a.Description,
                    Value = 0
                });
            }
            foreach (MOE.Common.Models.ActionLog al in actionLogs)
            {
                foreach (MOE.Common.Models.MetricType mt in al.MetricTypes)
                {
                    if (mt.MetricID == metric.MetricID)
                    {
                        foreach (MOE.Common.Models.Action a in al.Actions)
                        {
                            var chartData = reportData.ChartData
                                .Where(cd => cd.Description == a.Description)
                                .FirstOrDefault();
                            chartData.Value++;
                        }
                    }
                }
            }                   
            SetColors(reportData);
            return PartialView("Chart", reportData);
        }

        public ActionResult GetMetrics()
        {
            var metrics = metricTypeRepository.GetAllToDisplayMetrics();
            List<MetricObject> metricArray = new List<MetricObject>();
            foreach(MOE.Common.Models.MetricType m in metrics)
            {
                metricArray.Add(new MetricObject { MetricTypeID = m.MetricID, Abbreviation = m.Abbreviation });
            }
            return Json(metricArray, JsonRequestBehavior.AllowGet);
        }

        private void SetColors(MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData)
        {
            List<string> colors = reportData.GetColorList();
            for (int i = 0; i < reportData.ChartData.Count && i < colors.Count; i++)
            {
                reportData.ChartData[i].Color = colors[i];
            }
        }

        private MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel GetChartViewModel(string title, string canvasName,
            string yAxisDescription, string chartType)
        {
            MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel reportData =
                new MOE.Common.Models.ViewModel.MetricUsage.ChartViewModel();
            reportData.CanvasName = canvasName;
            reportData.ReportTitle = title;
            reportData.YAxisDescription = yAxisDescription;
            reportData.ChartType = chartType;
            reportData.ChartData = new List<MOE.Common.Business.ActionLog.ChartData>();
            return reportData;
        }

        //// GET: ActionLogs/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ActionLog actionLog = db.ActionLog.Find(id);
        //    if (actionLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(actionLog);
        //}

        // GET: ActionLogs/Create
        public ActionResult Create()
        {
            SetLists();
            ActionLog newLog = new ActionLog();
            newLog.CheckBoxListAllActions = actionRepository.GetAll().ToList();
            newLog.CheckBoxListAllMetricTypes = metricTypeRepository.GetAllToDisplayMetrics();
            newLog.Actions = new List<MOE.Common.Models.Action>();
            newLog.MetricTypes = new List<MOE.Common.Models.MetricType>();
            newLog.Date = DateTime.Now;
            return View(newLog);
        }

        // POST: ActionLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActionLogID,Date,AgencyID,Comment,SignalId,Name,CheckBoxListReturnActions,CheckBoxListReturnMetricTypes")] ActionLog actionLog)
        {
            actionLog.CheckBoxListAllActions = actionRepository.GetAll().ToList();
            actionLog.CheckBoxListAllMetricTypes = metricTypeRepository.GetAllToDisplayMetrics();
            SetLists();
            actionLog.ActionIDs = new List<int>();
            foreach (int i in actionLog.CheckBoxListReturnActions)
            {
                actionLog.ActionIDs.Add(i);
            }
            actionLog.MetricTypeIDs = new List<int>();
            foreach (int i in actionLog.CheckBoxListReturnMetricTypes)
            {
                actionLog.MetricTypeIDs.Add(i);
            }
           
            //if (TryValidateModel(actionLog))
            ////if (ModelState.IsValid)
            //{
                actionLogRepository.Add(actionLog);
                //actionLogRepository.Save();
                return Content("<h1>Action Log Created " + DateTime.Now.ToString() + "</h1>");
    //        }
    //        else
    //        {
    //            var errors = ModelState
    //.Where(x => x.Value.Errors.Count > 0)
    //.Select(x => new { x.Key, x.Value.Errors })
    //.ToArray();
    //            return Content("<h1>Unable to create Action Log " + DateTime.Now.ToString() + "</h1>");
    //        }
        }

        private void SetLists()
        {
            ViewBag.AgencyID = new SelectList(agencyRepository.GetAll(), "AgencyID", "Description");
            ViewBag.SignalID = new SelectList(signalRepository.GetLatestVersionOfAllSignalsAsQueryable(), "SignalId", "SignalDescription");
        }

        //// GET: ActionLogs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ActionLog actionLog = db.ActionLog.Find(id);
        //    if (actionLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Description", actionLog.AgencyID);
        //    ViewBag.SignalId = new SelectList(db.Signals, "SignalId", "PrimaryName", actionLog.SignalId);
        //    return View(actionLog);
        //}

        //// POST: ActionLogs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ActionLogID,Date,AgencyID,Comment,SignalId,Name")] ActionLog actionLog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(actionLog).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Description", actionLog.AgencyID);
        //    ViewBag.SignalId = new SelectList(db.Signals, "SignalId", "PrimaryName", actionLog.SignalId);
        //    return View(actionLog);
        //}

        //// GET: ActionLogs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ActionLog actionLog = db.ActionLog.Find(id);
        //    if (actionLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(actionLog);
        //}

        //// POST: ActionLogs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ActionLog actionLog = db.ActionLog.Find(id);
        //    db.ActionLog.Remove(actionLog);
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

    public class MetricObject
    {
        public string Abbreviation { get; set; }
        public int MetricTypeID { get; set; }
    }
}

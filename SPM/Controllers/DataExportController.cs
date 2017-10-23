using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SPM.Models;

namespace SPM.Controllers
{
    public class DataExportController : Controller
    {
        MOE.Common.Models.Repositories.IControllerEventLogRepository cr =
            MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DataExportViewModels
        public ActionResult RawDataExport()
        {
            return View();
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        // POST: DataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RawDataExport(DataExportViewModel vm)
        {
            if (ModelState.IsValid)
            {
                int StartHour = 0;
                int StartMinute = 0;
                int EndHour = 0;
                int EndMinute = 0;
                if (vm.StartDateHour != null)
                    StartHour = (int)vm.StartDateHour;
                if (vm.StartDateMinute != null)
                    StartMinute = (int)vm.StartDateMinute;
                if (vm.EndDateHour != null)
                    EndHour = (int)vm.EndDateHour;
                if (vm.EndDateMinute != null)
                    EndMinute = (int)vm.EndDateMinute;
                List<int> inputEventCodes = StringToIntList(vm.EventCodes);
                int inputParam = Convert.ToInt32(vm.EventParams);
                int Count = cr.GetEventCountByEventCodesParamDateTimeRange(vm.SignalId, vm.StartDateDate,
                    vm.EndDateDate, StartHour, StartMinute, EndHour, EndMinute,
                    inputEventCodes, inputParam);
                vm.Count = Count;
                return RedirectToAction("RawDataExport");
            }

            return View(vm);
        }

        // POST: DataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartDateDate,EndDateDate,StartDateHour,StartDateMinute,EndDateHour,EndDateMinute,Count")] DataExportViewModel dataExportViewModel)
        {
            if (ModelState.IsValid)
            {
                //db.DataExportViewModels.Add(dataExportViewModel);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dataExportViewModel);
        }

        //// GET: DataExportViewModels/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DataExportViewModel dataExportViewModel = db.DataExportViewModels.Find(id);
        //    if (dataExportViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dataExportViewModel);
        //}

        // POST: DataExportViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SignalId,StartDateDate,EndDateDate,StartDateHour,StartDateMinute,EndDateHour,EndDateMinute,Count")] DataExportViewModel vm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        vm.Count = cr.GetEventCountByEventCodesParamDateTimeRange(vm.SignalId, vm.StartDateDate, vm.EndDateDate,
        //            vm.StartDateHour, vm.StartDateMinute, vm.EndDateHour, vm.EndDateMinute, eventCodes, param);
        //        return RedirectToAction("Index");
        //    }
        //    return View(dataExportViewModel);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

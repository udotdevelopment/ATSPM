using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Business.Export;
using MOE.Common.Models;
using SPM.Filters;
using SPM.Models;
using System.Configuration;

namespace SPM.Controllers
{
    public class DataExportController : Controller
    {
        MOE.Common.Models.Repositories.IControllerEventLogRepository controllerEventLogRepository =
            MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DataExportViewModels
        public ActionResult RawDataExport()
        {
            DataExportViewModel viewModel = new DataExportViewModel();
            DateTime date = DateTime.Today;
            viewModel.StartDate = Convert.ToDateTime("10/17/2017");// date.AddDays(-1);
            viewModel.EndDate = Convert.ToDateTime("10/18/2017");// date;
            return View(viewModel);
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        // POST: DataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateJsonAntiForgeryToken]
        //public ActionResult RawDataExport(DataExportViewModel vm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        List<int> inputEventCodes = StringToIntList(vm.EventCodes);
        //        int inputParam = Convert.ToInt32(vm.EventParams);
        //        //int Count = controllerEventLogRepository.GetRecordCount().GetEventCountByEventCodesParamDateTimeRange(vm.SignalId, vm.StartDate,
        //        //    vm.EndDate, StartHour, StartMinute, EndHour, EndMinute,
        //        //    inputEventCodes, inputParam);
        //        //vm.Count = Count;
        //        return RedirectToAction("RawDataExport");
        //    }
        //    return View(vm);
        //}
        [HttpPost]
        public ActionResult RawDataExport(DataExportViewModel dataExportViewModel)
        {
            if (ModelState.IsValid)
            {
                List<int> eventParams = new List<int>();
                List<int> eventCodes = new List<int>();
                if (!String.IsNullOrEmpty(dataExportViewModel.EventParams))
                {
                    eventParams = CommaDashSeparated(dataExportViewModel.EventParams);
                    if (eventParams.Count == 0)
                    {
                        return Content("Unable to process Event Parameters");
                    }
                }
                if (!String.IsNullOrEmpty(dataExportViewModel.EventCodes))
                {
                    eventCodes = CommaDashSeparated(dataExportViewModel.EventCodes);
                    if (eventCodes.Count == 0)
                    { 
                        return Content("Unable to process Event Codes");
                    }
                }
                int recordCount = controllerEventLogRepository.GetRecordCountByParameterAndEvent(dataExportViewModel.SignalId,
                    dataExportViewModel.StartDate, dataExportViewModel.EndDate, eventParams, eventCodes);
                if (recordCount > dataExportViewModel.RecordCountLimit)
                {
                    return Content("The data set you have selected is too large. Your current request will generate " + recordCount.ToString() +
                        " records. Please reduces the number of records you have selected.");
                }
                else
                {
                    List<Controller_Event_Log> events = controllerEventLogRepository.GetRecordsByParameterAndEvent(dataExportViewModel.SignalId,
                        dataExportViewModel.StartDate, dataExportViewModel.EndDate, eventParams, eventCodes);
                    byte[] file = Exporter.GetCsvFile(events);
                    return File(file, "csv", "ControllerEventLogs.csv");
                }
            }
            return Content("This request cannot be processed. You may be missing parameters");
        }

        private List<int> CommaDashSeparated(string userInput)
        {
            List<int> processedCodes = new List<int>();
            string[] codes = userInput.Split(',');
            try
            {
                foreach (string code in codes)
                {
                    if (code.Contains('-'))
                    {
                        string[] withDash = code.Split('-');
                        int minNum = Convert.ToInt32(withDash[0]);
                        int maxNum = Convert.ToInt32(withDash[1]);
                        if (minNum > maxNum)
                        {
                            int temp = minNum;
                            minNum = maxNum;
                            maxNum = temp;
                        }
                        for(int ii=minNum; ii<=maxNum; ii++)
                        {
                            processedCodes.Add(ii);
                        }
                    }
                    else
                    {
                        processedCodes.Add(Convert.ToInt32(code));
                    }
                }
                processedCodes = processedCodes.Distinct().ToList();

            }
            catch (Exception e)
            {
            }
           return processedCodes;
        }

        public ActionResult GetRecordCount(DataExportViewModel dataExportViewModel)
        {
            if (ModelState.IsValid)
            {
                List<int> eventParams = new List<int>();
                List<int> eventCodes = new List<int>();
                if (!String.IsNullOrEmpty(dataExportViewModel.EventParams))
                {
                    try
                    {
                        string[] parameters = dataExportViewModel.EventParams.Split(',');
                        foreach (string parameter in parameters)
                        {
                            eventParams.Add(Convert.ToInt32(parameter));
                        }
                    }
                    catch (Exception)
                    {
                        return Content("Unable to process Event Parameters");
                    }

                }
                if (!String.IsNullOrEmpty(dataExportViewModel.EventCodes))
                {
                    try
                    {
                        string[] codes = dataExportViewModel.EventCodes.Split(',');
                        foreach (string code in codes)
                        {
                            eventCodes.Add(Convert.ToInt32(code));
                        }
                    }
                    catch (Exception)
                    {
                        return Content("Unable to process Event Codes");
                    }
                }
                int recordCount = controllerEventLogRepository.GetRecordCountByParameterAndEvent(dataExportViewModel.SignalId,
                    dataExportViewModel.StartDate, dataExportViewModel.EndDate, eventParams, eventCodes);
                dataExportViewModel.RecordCountLimit = Convert.ToInt32(ConfigurationManager.AppSettings["RawDataCountLimit"]);
                if (recordCount > dataExportViewModel.RecordCountLimit)
                {
                    return Content("The data set you have selected is too large. Your current request will generate " + recordCount.ToString() +
                        " records. Please reduces the number of records you have selected.");
                }
                else
                {
                    return Content("Your current request will generate " + recordCount.ToString() + " records.");
                }
            }
            return Content("This request cannot be processed. You may be missing parameters");
        }

        // POST: DataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,StartDateDate,EndDateDate,StartDateHour,StartDateMinute,EndDateHour,EndDateMinute,Count")] DataExportViewModel dataExportViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //db.DataExportViewModels.Add(dataExportViewModel);
        //        //db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(dataExportViewModel);
        //}

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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

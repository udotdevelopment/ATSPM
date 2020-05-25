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
using MOE.Common.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace SPM.Controllers
{
    [Authorize(Roles = "Data, Admin")]
    public class DataExportController : Controller
    {
        MOE.Common.Models.Repositories.IControllerEventLogRepository controllerEventLogRepository =
            MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DataExportViewModels
        public ActionResult RawDataExport()
        {
            DataExportViewModel viewModel = new DataExportViewModel();
            viewModel.DateTimePickerViewModel.SetDates();
            return View(viewModel);
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        [HttpPost]
        public ActionResult RawDataExport(DataExportViewModel dataExportViewModel)
        {
            string response = Request["g-recaptcha-response"];
            bool status = GetReCaptchaStatus(response);
            if (!status)
            {
                dataExportViewModel.RecaptchaMessage = "Google reCaptcha validation failed";
                return View(dataExportViewModel);
            }
            if (ModelState.IsValid)
            {
                List<int> eventParams, eventCodes;
                GetEventCodesAndEventParameters(dataExportViewModel, out eventParams, out eventCodes);
                int recordCount = controllerEventLogRepository.GetRecordCountByParameterAndEvent(dataExportViewModel.SignalId,
                    dataExportViewModel.DateTimePickerViewModel.GetStartDateTime(), dataExportViewModel.DateTimePickerViewModel.GetEndDateTimePlusOneMinute(), eventParams, eventCodes);
                if (recordCount > dataExportViewModel.RecordCountLimit)
                {
                    return Content("The data set you have selected is too large. Your current request will generate " + recordCount.ToString() +
                        " records. Please reduces the number of records you have selected.");
                }
                else
                {
                    List<Controller_Event_Log> events = controllerEventLogRepository.GetRecordsByParameterAndEvent(dataExportViewModel.SignalId,
                        dataExportViewModel.DateTimePickerViewModel.GetStartDateTime(), dataExportViewModel.DateTimePickerViewModel.GetEndDateTimePlusOneMinute(), eventParams, eventCodes);
                    byte[] file = Exporter.GetCsvFileForControllerEventLogs(events);
                    return File(file, "csv", "ControllerEventLogs.csv");
                }
            }
            return Content("This request cannot be processed. You may be missing parameters");
        }

        private void GetEventCodesAndEventParameters(DataExportViewModel dataExportViewModel, out List<int> eventParams, out List<int> eventCodes)
        {
            eventParams = new List<int>();
            eventCodes = new List<int>();
            if (!String.IsNullOrEmpty(dataExportViewModel.EventParams))
            {
                eventParams = CommaDashSeparated(dataExportViewModel.EventParams);
                if (eventParams.Count == 0)
                {
                    throw new Exception("Unable to process Event Parameters");
                }
            }
            if (!String.IsNullOrEmpty(dataExportViewModel.EventCodes))
            {
                eventCodes = CommaDashSeparated(dataExportViewModel.EventCodes);
                if (eventCodes.Count == 0)
                {
                    throw new Exception("Unable to process Event Codes");
                }
            }
        }

        private bool GetReCaptchaStatus(string response)
        {
            var generalSettingsRepository =
                MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            var settings = generalSettingsRepository.GetGeneralSettings();
            if (String.IsNullOrEmpty(settings.ReCaptchaPublicKey) && String.IsNullOrEmpty(settings.ReCaptchaSecretKey))
            {
                return true;
            }
            var userIp = Request.UserHostAddress;
#if DEBUG
            userIp = "168.178.126.48";
#endif
            string secretKey = settings.ReCaptchaSecretKey;
            var client = new WebClient();
            var result = client.DownloadString(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}&remoteip={userIp}");
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;
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
                try
                {
                    List<int> eventParams, eventCodes;
                    GetEventCodesAndEventParameters(dataExportViewModel, out eventParams, out eventCodes);
                    int recordCount = controllerEventLogRepository.GetRecordCountByParameterAndEvent(dataExportViewModel.SignalId,
                            dataExportViewModel.DateTimePickerViewModel.GetStartDateTime(), dataExportViewModel.DateTimePickerViewModel.GetEndDateTimePlusOneMinute(), eventParams, eventCodes);
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
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            return Content("This request cannot be processed. You may be missing parameters");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using SPM.Models;

namespace SPM.Controllers
{
    public class LeftTurnGapReportController : Controller
    {

        // GET: LeftTurnGapReport
        public ActionResult Index()
        {
            return View(new LeftTurnGapReportViewModel());
        }

        // GET: LeftTurnCheckBoxes
        public ActionResult GetLeftTurnCheckBoxes(string signalID)
        {
            List < CheckModel > checkModels = new List<CheckModel>{
                new CheckModel{Id=1, Name = "All Left Turns", Checked = true},
                new CheckModel{Id=1, Name = "NBL", Checked = true},
                new CheckModel{Id=1, Name = "SBL", Checked = true},
                new CheckModel{Id=1, Name = "EBL", Checked = true},
                new CheckModel{Id=1, Name = "WBL", Checked = true},
            };
            return PartialView("LeftTurnCheckBoxes",checkModels);
        }

        //public async ActionResult GetSignalDataCheckReport(SignalDataCheckReportParameters parameters)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //Passing service base url
        //        client.BaseAddress = new Uri("");
        //        client.DefaultRequestHeaders.Clear();
        //        //Define request data format
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        //        HttpResponseMessage Res = await client.GetAsync("api/DataCheck/Get");
        //        //Checking the response is successful or not which is sent using HttpClient
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            //Storing the response details recieved from web api
        //            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
        //            //Deserializing the response recieved from web api and storing into the Employee list
        //            EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
        //        }
        //        //returning the employee list to view
        //        return View(EmpInfo);
        //    }
        //    SignalDataCheckReportViewModel signalDataCheckReportViewModel = new SignalDataCheckReportViewModel(true, false, true);
        //    return PartialView("SignalDataCheckReport", signalDataCheckReportViewModel);
        //}

        public ActionResult GetFinalGapAnalysisReport(FinalGapAnalysisReportParameters parameters)
        {
            FinalGapAnalysisReportViewModel finalGapAnalysisReportViewModel = new FinalGapAnalysisReportViewModel(true, false, true, false);
            return PartialView("FinalGapAnalysisReport", finalGapAnalysisReportViewModel);
        }

       
    }

    
}

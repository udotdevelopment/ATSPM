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

        public ActionResult GetSignalDataCheckReport(string signalId)
        {
            string checkDataUrl = BuildUrlString(signalId);
            SignalDataCheckReportViewModel signalDataCheckReportViewModel = new SignalDataCheckReportViewModel(true, false, true);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(checkDataUrl);
                //HTTP GET
                var responseTask = client.GetAsync("https://localhost:44363/DataCheck?signalId=7412");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<SignalDataCheckReportViewModel>();
                    readTask.Wait();

                    signalDataCheckReportViewModel = readTask.Result;
                }
                else //web api sent error response 
                {
                    return PartialView((Object)"There was an error while trying to get the signal check data.");
                }
            }
            return PartialView("SignalDataCheckReport", signalDataCheckReportViewModel);
        }

        private static string BuildUrlString(string signalId)
        {
            string checkDataUrl = "https://localhost:44363/";            

            return checkDataUrl;
        }

        public ActionResult GetFinalGapAnalysisReport(FinalGapAnalysisReportParameters parameters)
        {
            FinalGapAnalysisReportViewModel finalGapAnalysisReportViewModel = new FinalGapAnalysisReportViewModel(true, false, true, false);
            return PartialView("FinalGapAnalysisReport", finalGapAnalysisReportViewModel);
        }

       
    }

    
}

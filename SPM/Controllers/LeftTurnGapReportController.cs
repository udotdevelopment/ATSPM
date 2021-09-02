using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public ActionResult GetSignalDataCheckReport(SignalDataCheckReportParameters parameters)
        {
            SignalDataCheckReportViewModel signalDataCheckReportViewModel = new SignalDataCheckReportViewModel(true, false, true);
            return PartialView("SignalDataCheckReport", signalDataCheckReportViewModel);
        }

        public ActionResult GetFinalGapAnalysisReport(FinalGapAnalysisReportParameters parameters)
        {
            FinalGapAnalysisReportViewModel finalGapAnalysisReportViewModel = new FinalGapAnalysisReportViewModel(true, false, true, false);
            return PartialView("FinalGapAnalysisReport", finalGapAnalysisReportViewModel);
        }

       
    }

    
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Business;
using MOE.Common.Models.Repositories;
using Newtonsoft.Json;
using RestSharp;
using Rotativa;
using SPM.Models;
using SPM.Models.LeftTurnGapReport;

namespace SPM.Controllers
{
    [Authorize(Roles = "Restricted Configuration, Admin")]
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
            var signalRepository = SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetLatestVersionOfSignalBySignalID(signalID);
            List<CheckModel> checkModels = new List<CheckModel>{
                new CheckModel{Id=0, Name = "All Left Turns", Checked = true} };
            foreach (var approach in signal.Approaches)
            {
                if(approach.Detectors.Any(d => (d.MovementTypeID == 3) && d.DetectionTypeIDs.Contains(4)))
                {
                    checkModels.Add(new CheckModel { Id = approach.ApproachID, Checked = true, Name = approach.Description});
                }
            }
            return PartialView("LeftTurnCheckBoxes",checkModels);
        }

        static async Task<IRestResponse> SendURI(Uri u, string c)
        {
            var client = new RestClient(u);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = c;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }

        [HttpPost]
        public ActionResult GetSignalDataCheckReport(string signalId, double cyclesWithPedCalls, double cyclesWithGapOuts,
            int leftTurnVolume, DateTime startDate, DateTime endDate, int[] approachIds, int[] daysOfWeek)
        {
            DataCheckPayload dataCheckPayload = new DataCheckPayload()
            {
                SignalId = signalId,
                DaysOfWeek = daysOfWeek,
                VolumePerHourThreshold = leftTurnVolume,
                PedestrianThreshold = cyclesWithPedCalls / 100,
                StartDate = startDate,
                EndDate = endDate,
                GapOutThreshold = cyclesWithGapOuts/100
            };
            var checkResults = new List<SignalDataCheckReportViewModel>();
            foreach (int approachId in approachIds)
            {
                dataCheckPayload.ApproachId = approachId;
                SignalDataCheckReportViewModel signalDataCheckReportViewModel = new SignalDataCheckReportViewModel();
                string address = BuildUrlString() + "DataCheck";
                Uri u = new Uri(address);
                var payload = JsonConvert.SerializeObject(dataCheckPayload);
                var t = Task.Run(() => SendURI(u, payload));
                t.Wait();
                var result = t.Result;
                if (result.ResponseStatus == ResponseStatus.Completed)
                {
                    signalDataCheckReportViewModel = JsonConvert.DeserializeObject<SignalDataCheckReportViewModel>(result.Content);
                    checkResults.Add(signalDataCheckReportViewModel);
                    signalDataCheckReportViewModel.VolumeThreshold = dataCheckPayload.VolumePerHourThreshold;
                    signalDataCheckReportViewModel.PedThreshold = dataCheckPayload.PedestrianThreshold;
                    signalDataCheckReportViewModel.GapOutThreshold = dataCheckPayload.GapOutThreshold;
                }                
            }
            return PartialView("SignalDataCheckReport", checkResults);
        }

        [HttpDelete]
        public void RemoveTempFile(String fileName)
        {
            //Check to make sure it is only the file name without slashes
            var file = new FileInfo(Server.MapPath("~/TempPdf/" + fileName));
            file.Delete();
        }

        private static string BuildUrlString()
        {
            string url = ConfigurationManager.AppSettings["ReportsUrl"];
            string checkDataUrl = url;           

            return checkDataUrl;
        }

        public JsonResult GetFinalGapAnalysisReport(FinalGapAnalysisReportParameters parameters)
        {
            var approachRepository = ApproachRepositoryFactory.Create();
            var finalGapAnalysisReportViewModel = new List<FinalGapAnalysisReportViewModel>();
            foreach (int approachId in parameters.ApproachIds)
            {
                var approach = approachRepository.GetApproachByApproachID(approachId);
                if (parameters.GetAMPMPeakPeriod)
                {
                    SetHoursAndMinutes(parameters, 6, 0, 9, 0);
                    var approachResultAM = GetApproachResult(parameters, approach, approachId);
                    approachResultAM.PeakPeriodDescription = "AM Peak";
                    finalGapAnalysisReportViewModel.Add(approachResultAM);

                    SetHoursAndMinutes(parameters, 15, 0, 18, 0);
                    var approachResultPM = GetApproachResult(parameters, approach, approachId);
                    approachResultPM.PeakPeriodDescription = "PM Peak";
                    finalGapAnalysisReportViewModel.Add(approachResultPM);
                }
                else if (parameters.GetAMPMPeakHour)
                {
                    LeftTurnPeakHourResultViewModel peakResult = FindPeakHours(parameters, approachId);

                    SetHoursAndMinutes(parameters, peakResult.AmStartHour, peakResult.AmStartMinute, peakResult.AmEndHour, peakResult.AmEndMinute);
                    var approachResultAM = GetApproachResult(parameters, approach, approachId);
                    approachResultAM.PeakPeriodDescription = "AM Peak";
                    finalGapAnalysisReportViewModel.Add(approachResultAM);


                    SetHoursAndMinutes(parameters, peakResult.PmStartHour, peakResult.PmStartMinute, peakResult.PmEndHour, peakResult.PmEndMinute);
                    var approachResultPM = GetApproachResult(parameters, approach, approachId);
                    approachResultPM.PeakPeriodDescription = "PM Peak";
                    finalGapAnalysisReportViewModel.Add(approachResultPM);
                }
                else if (parameters.Get24HourPeriod)
                {
                    var approachResult = GetApproachResult(parameters, approach, approachId);
                    AddCharts(approachResult, parameters);
                    approachResult.Get24HourPeriod = true;
                    finalGapAnalysisReportViewModel.Add(approachResult);
                } else
                {
                    var approachResult = GetApproachResult(parameters, approach, approachId);
                    approachResult.PeakPeriodDescription = "Custom";
                    finalGapAnalysisReportViewModel.Add(approachResult);
                }
            }
            var pdf = new PartialViewAsPdf("FinalGapAnalysisReport", finalGapAnalysisReportViewModel) { FileName = "Test.pdf" };
            PdfResult pdfResult = GetPdf(pdf);

            return Json(new { pdfResult }, JsonRequestBehavior.AllowGet);
        }

        private PdfResult GetPdf(ViewAsPdf pdf)
        {
            var settingsRepository = ApplicationSettingsRepositoryFactory.Create();
            var settings = settingsRepository.GetGeneralSettings();
            var imagelocation = settings.ImagePath;
            var pdfResult = new PdfResult();
            byte[] pdfData = pdf.BuildFile(ControllerContext);
            var tempFileName = Guid.NewGuid().ToString() + ".pdf";
            var tempFilePath = imagelocation + tempFileName;

            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            }
            pdfResult.HTML = "<iframe src=\"" + settings.ImageUrl + tempFileName +
                "\"style=\"width:100%; height:1000px;\" frameborder=\"0\"></iframe>";
            pdfResult.FileName = tempFileName;

            return pdfResult;
        }

        private static void SetHoursAndMinutes(FinalGapAnalysisReportParameters parameters, int startHour, int startMinute, int endHour, int endMinute)
        {
            parameters.StartHour = startHour;
            parameters.StartMinute = startMinute;
            parameters.EndHour = endHour;
            parameters.EndMinute = endMinute;
        }

        private static LeftTurnPeakHourResultViewModel FindPeakHours(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            LeftTurnPeakHourResultViewModel peakResult = new LeftTurnPeakHourResultViewModel();
            PeakHourParameters toSendParameters = new PeakHourParameters
            {
                ApproachId = approachId,
                DaysOfWeek = parameters.DaysOfWeek,
                EndDate = parameters.EndDate,
                SignalId = parameters.SignalId,
                StartDate = parameters.StartDate
            };
            string address = BuildUrlString() + "PeakHours";
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            var result = t.Result;
            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                peakResult = JsonConvert.DeserializeObject<LeftTurnPeakHourResultViewModel>(result.Content);
            }
            else
            {
                throw result.ErrorException;
            }
            if (peakResult == null)
            {
                throw new NullReferenceException("Unable to get peak hours");
            }

            return peakResult;
        }

        private static void AddCharts(FinalGapAnalysisReportViewModel approachResult, FinalGapAnalysisReportParameters parameters)
        {
            double gapVsDemandChartHeight = Math.Max(approachResult.AcceptableGapList.Values.Max(), approachResult.DemandList.Values.Max());
            gapVsDemandChartHeight = (double)(Math.Ceiling(gapVsDemandChartHeight / 10.0) * 10) + 10;

            var gapVsDemandChart = new GapVsDemandOptions
                (parameters.SignalId,
                parameters.StartDate,
                parameters.EndDate.AddDays(1),
                gapVsDemandChartHeight,
                0,
                31,
                approachResult.AcceptableGapList,
                approachResult.DemandList);
            approachResult.GapDemandChartImg = gapVsDemandChart.CreateMetric().FirstOrDefault();
            var pedsVsFailuresOptions = new PedsVsFailuresOptions
                (parameters.SignalId,
                parameters.StartDate,
                parameters.EndDate.AddDays(1),
                0,
                0,
                31,
                approachResult.AcceptableGapList,
                approachResult.DemandList);
            approachResult.PedSplitFailChartImg = pedsVsFailuresOptions.CreateMetric().FirstOrDefault();
        }

        private FinalGapAnalysisReportViewModel GetApproachResult(FinalGapAnalysisReportParameters parameters, Approach approach, int approachId)
        {
            FinalGapAnalysisReportViewModel approachResult = new FinalGapAnalysisReportViewModel
            {
                SignalId = parameters.SignalId,
                StartDate = parameters.StartDate,
                EndDate = parameters.EndDate,
                StartTime = new TimeSpan(parameters.StartHour ?? 0, parameters.StartMinute ?? 0, 0),
                EndTime = new TimeSpan(parameters.EndHour ?? 0, parameters.StartMinute ?? 0, 0),
                ApproachDescription = approach.Description,
                SpeedLimit = approach.MPH,
                Location = approach.Signal.PrimaryName + " & " + approach.Signal.SecondaryName,
                PhaseType = approach.GetPhaseType().GetDescription(),
                SignalType = approach.GetSignalHeadType().GetDescription()
            };

            if (parameters.GetGapReport)
            {
                var gapResult = GetGapResult(parameters, approachId);

                approachResult.GapDurationConsiderForStudy = gapResult.ConsiderForStudy;
                approachResult.Capacity = gapResult.Capacity;
                approachResult.Demand = gapResult.Demand;
                approachResult.GapOutPercent = gapResult.GapDurationPercent;
                approachResult.AcceptableGapList = gapResult.AcceptableGaps;
            }
            if (parameters.GetSplitFail)
            {
                var splitFailResult = GetSplitFailResult(parameters, approachId);

                approachResult.SplitFailsConsiderForStudy = splitFailResult.ConsiderForStudy;
                approachResult.CyclesWithSplitFailNum = splitFailResult.CyclesWithSplitFails;
                approachResult.CyclesWithSplitFailPercent = splitFailResult.SplitFailPercent;
                approachResult.PercentCyclesWithSplitFailList = splitFailResult.PercentCyclesWithSplitFailList;
                approachResult.Direction = splitFailResult.Direction;
            }
            if (parameters.GetPedestrianCall)
            {
                var PedResult = GetPedActuationResult(parameters, approachId);

                approachResult.CyclesWithPedCallNum = PedResult.CyclesWithPedCalls;
                approachResult.CyclesWithPedCallPercent = PedResult.PedActuationPercent;
                approachResult.PedActuationsConsiderForStudy = PedResult.ConsiderForStudy;
                approachResult.PercentCyclesWithPedsList = PedResult.PercentCyclesWithPedsList;
                approachResult.Direction = PedResult.Direction;
                approachResult.OpposingDirection = PedResult.OpposingDirection;
            }
            if (parameters.GetConflictingVolume || parameters.GetGapReport)
            {
                var volumeResult = GetVolumeResult(parameters, approachId);

                if (parameters.GetConflictingVolume)
                {
                    approachResult.CrossProductConsiderForStudy = volumeResult.ConsiderForStudy;
                    approachResult.VolumesConsiderForStudy = volumeResult.ConsiderForStudy;
                }

                approachResult.OpposingLanes = volumeResult.OpposingLanes;
                approachResult.CrossProductReview = volumeResult.CrossProductReview;
                approachResult.DecisionBoundariesReview = volumeResult.DecisionBoundariesReview;
                approachResult.LeftTurnVolume = volumeResult.LeftTurnVolume;
                approachResult.OpposingThroughVolume = volumeResult.OpposingThroughVolume;
                approachResult.CrossProductValue = volumeResult.CrossProductValue;
                approachResult.CalculatedVolumeBoundary = volumeResult.CalculatedVolumeBoundary;
                approachResult.DemandList = volumeResult.DemandList;
                approachResult.Direction = volumeResult.Direction;
                approachResult.OpposingDirection = volumeResult.OpposingDirection;
            }
            return approachResult;
        }

        private LeftTurnVolumeValueResultViewModel GetVolumeResult(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            string url = "Volume";
            var result = GetResult(parameters, approachId, url);
            LeftTurnVolumeValueResultViewModel volumeResult = JsonConvert.DeserializeObject<LeftTurnVolumeValueResultViewModel>(result.Content);
            volumeResult.ConsiderForStudy = volumeResult.CrossProductReview || volumeResult.DecisionBoundariesReview;
            return volumeResult;
        }

        private PedActuationResultViewModel GetPedActuationResult(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            string url = "PedActuation";
            var result = GetResult(parameters, approachId, url);
            PedActuationResultViewModel pedActuationResult = JsonConvert.DeserializeObject<PedActuationResultViewModel>(result.Content);
            pedActuationResult.ConsiderForStudy = pedActuationResult.PedActuationPercent > 0.3d;
            return pedActuationResult;
        }

        private SplitFailResultViewModel GetSplitFailResult(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            string url = "SplitFail";
            var result = GetResult(parameters, approachId, url);
            SplitFailResultViewModel splitFailResult = JsonConvert.DeserializeObject<SplitFailResultViewModel>(result.Content);
            splitFailResult.ConsiderForStudy = splitFailResult.SplitFailPercent > parameters.AcceptableSplitFailPercentage;
            return splitFailResult;
        }

        private LeftTurnGapDurationViewModel GetGapResult(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            string url = "GapDuration";
            var result = GetResult(parameters, approachId, url);
            LeftTurnGapDurationViewModel gapOutResult = JsonConvert.DeserializeObject<LeftTurnGapDurationViewModel>(result.Content);
            gapOutResult.ConsiderForStudy = gapOutResult.GapDurationPercent > parameters.AcceptableGapPercentage;
            return gapOutResult;
        }

        private IRestResponse GetResult(FinalGapAnalysisReportParameters parameters, int approachId, string url)
        {
            ReportParameters toSendParameters = GetToSendParameters(parameters, approachId);
            string address = BuildUrlString() + url;
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            if (t.Result.ResponseStatus == ResponseStatus.Completed)
                return t.Result;
            else
                throw t.Result.ErrorException;
        }

        private ReportParameters GetToSendParameters(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            ReportParameters toSendParameters = new ReportParameters
            {
                ApproachId = approachId,
                DaysOfWeek = parameters.DaysOfWeek,
                EndDate = parameters.EndDate,
                EndHour = parameters.EndHour ?? 0,
                EndMinute = parameters.EndMinute ?? 0,
                SignalId = parameters.SignalId,
                StartDate = parameters.StartDate,
                StartHour = parameters.StartHour ?? 0,
                StartMinute = parameters.StartMinute ?? 0
            };
            return toSendParameters;
        }

        private static string GetUrl(FinalGapAnalysisReportParameters parameters, string url)
        {
            url += "?signalId=" + parameters.SignalId;
            url += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            url += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            url += "&startHour=" + parameters.StartHour;
            url += "&startMinute=" + parameters.StartMinute;
            url += "&endHour=" + parameters.EndHour;
            url += "&endMinute=" + parameters.EndMinute;
            url += "&approachId={0}";
            return url;
        }

        private static string GetPeakUrl(FinalGapAnalysisReportParameters parameters)
        {
            var PeakUrl = "PeakHours?signalId=" + parameters.SignalId;
            PeakUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            PeakUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            PeakUrl += "&approachId={0}";
            return PeakUrl;
        }
    }
}

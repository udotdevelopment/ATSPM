using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using RestSharp;
using Rotativa;
using SPM.Models;
using SPM.Models.LeftTurnGapReport;

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
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = u;                
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Add("Authorization", "Bearer [Access_Token]");
            //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    var result = await client.PostAsJsonAsync("DataCheck", c);
            //    //HttpRequestMessage request = new HttpRequestMessage
            //    //{
            //    //    Method = HttpMethod.Post,
            //    //    RequestUri = u,
            //    //    Content = c
            //    //};

            //    //HttpResponseMessage result = await client.SendAsync(request);
            //    return result;
            //}
        }

        [HttpPost]
        public ActionResult GetSignalDataCheckReport(string signalId, double cyclesWithPedCalls, double cyclesWithGapOuts,
            int leftTurnVolume, DateTime startDate, DateTime endDate, int[] approachIds, int[] daysOfWeek)
        {
            Models.LeftTurnGapReport.DataCheckPayload dataCheckPayload = new Models.LeftTurnGapReport.DataCheckPayload()
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
            String url = ConfigurationManager.AppSettings["ReportsUrl"];
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
                var approachResult = new FinalGapAnalysisReportViewModel();
                if (parameters.GetAMPMPeakPeriod.HasValue && parameters.GetAMPMPeakPeriod == true)
                {
                    parameters.StartHour = 6;
                    parameters.StartMinute = 0;
                    parameters.EndHour = 9;
                    parameters.EndMinute = 0;
                    var approachResultAm = GetApproachResult(parameters, approachId);

                    parameters.StartHour = 15;
                    parameters.StartMinute = 0;
                    parameters.EndHour = 18;
                    parameters.EndMinute = 0;
                    var approachResultPm = GetApproachResult(parameters, approachId);

                    GetCombinedResult(approachResult, approachResultAm, approachResultPm, parameters);
                }
                else if (parameters.GetAMPMPeakHour.HasValue && parameters.GetAMPMPeakHour == true)
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

                    parameters.StartHour = peakResult.AmStartHour;
                    parameters.StartMinute = peakResult.AmStartMinute;
                    parameters.EndHour = peakResult.AmEndHour;
                    parameters.EndMinute = peakResult.AmEndMinute;
                    var approachResultAm = GetApproachResult(parameters, approachId);


                    parameters.StartHour = peakResult.PmStartHour;
                    parameters.StartMinute = peakResult.PmStartMinute;
                    parameters.EndHour = peakResult.PmEndHour;
                    parameters.EndMinute = peakResult.PmEndMinute;
                    var approachResultPm = GetApproachResult(parameters, approachId);

                    GetCombinedResult(approachResult, approachResultAm, approachResultPm, parameters);
                }
                else
                {
                    approachResult = GetApproachResult(parameters, approachId);
                }

                approachResult.ApproachDescription = approach.Description;
                var gapVsDemandChart = new GapVsDemandOptions
                    (parameters.SignalId,
                    parameters.StartDate,
                    parameters.EndDate.AddDays(1),
                    0, 
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
                finalGapAnalysisReportViewModel.Add(approachResult);
            }
            var pdf = new ViewAsPdf("FinalGapAnalysisReport", finalGapAnalysisReportViewModel) { FileName = "Test.pdf" };
            PdfResult pdfResult = GetPdf(pdf);
            return Json(new { pdfResult }, JsonRequestBehavior.AllowGet);

            //return Content(tempFileName, "text/html");

            //return Content("<iframe src=\"/TempPdf/" + tempFileName +
            //    "\"style=\"width:600px; height:500px;\" frameborder=\"0\"></iframe>", "text/html");
            //var pdf = new ActionAsPdf("FinalGapAnalysisReport") { FileName = "Test.pdf" };
            //byte[] byteArray = pdf.BuildFile(ControllerContext);
            //string imageBase64Data = Convert.ToBase64String(byteArray);
            //string imageDataURL = string.Format("data:application/pdf;base64,{0}", imageBase64Data);
            //return PartialView("ResultAsPdf", tempFileName);
            //return  File(byteArray, "application/pdf");
            //using (MemoryStream pdfStream = new MemoryStream())
            //{
            //    pdfStream.Write(byteArray, 0, byteArray.Length);
            //    pdfStream.Position = 0;
            //    return new FileStreamResult(pdfStream, "application/pdf");
            //}
            //return pdf;
            //return PartialView("FinalGapAnalysisReport", finalGapAnalysisReportViewModel);
        }

        private PdfResult GetPdf(ViewAsPdf pdf)
        {
            var settingsRepository = ApplicationSettingsRepositoryFactory.Create();
            var settings = settingsRepository.GetGeneralSettings();
            var imagelocation = settings.ImagePath;
            var pdfResult = new PdfResult();
            byte[] pdfData = pdf.BuildFile(ControllerContext);
            //var tempFilePath = Path.GetTempPath();
            var tempFileName = Guid.NewGuid().ToString() + ".pdf";
            //var tempFilePath = Path.Combine(Server.MapPath("~/TempPdf/"), tempFileName);
            var tempFilePath = imagelocation + tempFileName;

            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            }
            pdfResult.HTML = "<iframe src=\"" + settings.ImageUrl + tempFileName +
                "\"style=\"width:100%; height:500px;\" frameborder=\"0\"></iframe>";
            pdfResult.FileName = tempFileName;
            return pdfResult;
        }

        private static void GetCombinedResult(FinalGapAnalysisReportViewModel approachResult, FinalGapAnalysisReportViewModel approachResultAm, FinalGapAnalysisReportViewModel approachResultPm, FinalGapAnalysisReportParameters parameters)
        {
            if(parameters.GetGapReport??false)
                approachResult.GapDurationConsiderForStudy = approachResultAm.GapDurationConsiderForStudy.Value || approachResultPm.GapDurationConsiderForStudy.Value;
            if(parameters.GetPedestrianCall??false)
                approachResult.PedActuationsConsiderForStudy = approachResultAm.PedActuationsConsiderForStudy.Value || approachResultPm.PedActuationsConsiderForStudy.Value;
            if(parameters.GetSplitFail??false)
                approachResult.SplitFailsConsiderForStudy = approachResultAm.SplitFailsConsiderForStudy.Value || approachResultPm.SplitFailsConsiderForStudy.Value;
            if(parameters.GetConflictingVolume??false)
                approachResult.VolumesConsiderForStudy = approachResultAm.VolumesConsiderForStudy.Value || approachResultPm.VolumesConsiderForStudy.Value;
        }

        private FinalGapAnalysisReportViewModel GetApproachResult(FinalGapAnalysisReportParameters parameters, int approachId)
        {
            FinalGapAnalysisReportViewModel approachResult = new FinalGapAnalysisReportViewModel();
            if (parameters.GetGapReport ?? false)
            {
                var gapResult = GetGapResult(parameters, approachId);
                approachResult.GapDurationConsiderForStudy = gapResult.ConsiderForStudy;
                approachResult.Capacity = gapResult.Capacity;
                approachResult.Demand = gapResult.Demand;
                approachResult.GapOutPercent = gapResult.GapOutPercent;
                approachResult.AcceptableGapList = gapResult.AcceptableGaps;
            }

            if (parameters.GetSplitFail ?? false)
            {
                var splitFailResult = GetSplitFailResult(parameters, approachId);
                approachResult.SplitFailsConsiderForStudy = splitFailResult.ConsiderForStudy;
                approachResult.CyclesWithSplitFailNum = splitFailResult.CyclesWithSplitFails;
                approachResult.CyclesWithSplitFailPercent = splitFailResult.SplitFailPercent;
                approachResult.PercentCyclesWithSplitFailList = splitFailResult.PercentCyclesWithSplitFailList;
            }
            if (parameters.GetPedestrianCall ?? false)
            {
                var PedResult = GetPedActuationResult(parameters, approachId);
                approachResult.CyclesWithPedCallNum = PedResult.CyclesWithPedCalls;
                approachResult.CyclesWithPedCallPercent = PedResult.PedActuationPercent;
                approachResult.PedActuationsConsiderForStudy = PedResult.ConsiderForStudy;
                approachResult.PercentCyclesWithPedsList = PedResult.PercentCyclesWithPedsList;
            }
            if (parameters.GetConflictingVolume ?? false) 
            {
                var volumeResult = GetVolumeResult(parameters, approachId);
                approachResult.VolumesConsiderForStudy = volumeResult.ConsiderForStudy;
                approachResult.OpposingLanes = volumeResult.OpposingLanes;
                approachResult.CrossProductReview = volumeResult.CrossProductReview;
                approachResult.DecisionBoundariesReview = volumeResult.DecisionBoundariesReview;
                approachResult.LeftTurnVolume = volumeResult.LeftTurnVolume;
                approachResult.OpposingThroughVolume = volumeResult.OpposingThroughVolume;
                approachResult.CrossProductValue = volumeResult.CrossProductValue;
                approachResult.CalculatedVolumeBoundary = volumeResult.CalculatedVolumeBoundary;
                approachResult.ConsiderForStudy = volumeResult.ConsiderForStudy;
                approachResult.DemandList = volumeResult.DemandList;
            }
            return approachResult;
        }

        private LeftTurnVolumeValueResultViewModel GetVolumeResult(FinalGapAnalysisReportParameters parameters, int approachId)
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
            string address = BuildUrlString() + "Volume";
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            var result = t.Result;
            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                LeftTurnVolumeValueResultViewModel volumeResult = JsonConvert.DeserializeObject<LeftTurnVolumeValueResultViewModel>(result.Content);
                volumeResult.ConsiderForStudy = volumeResult.CrossProductReview || volumeResult.DecisionBoundariesReview;
                return volumeResult;
            }
            else
            {
                throw result.ErrorException;
            }

           
        }

        private PedActuationResultViewModel GetPedActuationResult(FinalGapAnalysisReportParameters parameters, int approachId)
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
            string address = BuildUrlString() + "PedActuation";
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            var result = t.Result;
            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                PedActuationResultViewModel pedActuationResult = JsonConvert.DeserializeObject<PedActuationResultViewModel>(result.Content);
                pedActuationResult.ConsiderForStudy = pedActuationResult.PedActuationPercent > 0.3d;
                return pedActuationResult;
            }
            else
            {
                throw result.ErrorException;
            }
           
        }

        private SplitFailResultViewModel GetSplitFailResult(FinalGapAnalysisReportParameters parameters, int approachId)
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
            string address = BuildUrlString() + "SplitFail";
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            var result = t.Result;
            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                SplitFailResultViewModel splitFailResult = JsonConvert.DeserializeObject<SplitFailResultViewModel>(result.Content);
                splitFailResult.ConsiderForStudy = splitFailResult.SplitFailPercent > parameters.AcceptableSplitFailPercentage;
                return splitFailResult;
            }
            else
            {
                throw result.ErrorException;
            }

        }

        private LeftTurnGapOutViewModel GetGapResult(FinalGapAnalysisReportParameters parameters, int approachId)
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
            string address = BuildUrlString()+"GapOut";
            Uri u = new Uri(address);
            var payload = JsonConvert.SerializeObject(toSendParameters);
            var t = Task.Run(() => SendURI(u, payload));
            t.Wait();
            var result = t.Result;
            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                LeftTurnGapOutViewModel gapOutResult = JsonConvert.DeserializeObject<LeftTurnGapOutViewModel>(result.Content);
                gapOutResult.ConsiderForStudy = gapOutResult.GapOutPercent > parameters.AcceptableGapPercentage;
                return gapOutResult;
            }
            else
            {
                throw result.ErrorException;
            }

            //string address = String.Format(GetGapUrl(parameters), approachId);
            //client.BaseAddress = new Uri(BuildUrlString());

            ////HTTP GET
            //var responseTask = client.GetAsync(address);
            //responseTask.Wait();

            //var result = responseTask.Result;
            //if (result.IsSuccessStatusCode)
            //{
            //    var readTask = result.Content.ReadAsAsync<LeftTurnGapOutViewModel>();
            //    readTask.Wait();

            //    LeftTurnGapOutViewModel gapOutResult = readTask.Result;
            //    return gapOutResult.GapOutPercent > parameters.AcceptableGapPercentage;
            //}
            //else //web api sent error response 
            //{
            //    throw new Exception(result.ReasonPhrase);
            //}
        }

        private static string GetGapUrl(FinalGapAnalysisReportParameters parameters)
        {
            var GapUrl = "GapOut?signalId=" + parameters.SignalId;
            GapUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            GapUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            GapUrl += "&startHour=" + parameters.StartHour;
            GapUrl += "&startMinute=" + parameters.StartMinute;
            GapUrl += "&endHour=" + parameters.EndHour;
            GapUrl += "&endMinute=" + parameters.EndMinute;
            GapUrl += "&approachId={0}";
            return GapUrl;
        }

        private static string GetSplitFailUrl(FinalGapAnalysisReportParameters parameters)
        {
            var GapUrl = "SplitFail?signalId=" + parameters.SignalId;
            GapUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            GapUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            GapUrl += "&startHour=" + parameters.StartHour;
            GapUrl += "&startMinute=" + parameters.StartMinute;
            GapUrl += "&endHour=" + parameters.EndHour;
            GapUrl += "&endMinute=" + parameters.EndMinute;
            GapUrl += "&approachId={0}";
            return GapUrl;
        }

        private static string GetPedUrl(FinalGapAnalysisReportParameters parameters)
        {
            var GapUrl = "PedActuation?signalId=" + parameters.SignalId;
            GapUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            GapUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            GapUrl += "&startHour=" + parameters.StartHour;
            GapUrl += "&startMinute=" + parameters.StartMinute;
            GapUrl += "&endHour=" + parameters.EndHour;
            GapUrl += "&endMinute=" + parameters.EndMinute;
            GapUrl += "&approachId={0}";
            return GapUrl;
        }

        private static string GetVolumeUrl(FinalGapAnalysisReportParameters parameters)
        {
            var GapUrl = "Volume?signalId=" + parameters.SignalId;
            GapUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            GapUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            GapUrl += "&startHour=" + parameters.StartHour;
            GapUrl += "&startMinute=" + parameters.StartMinute;
            GapUrl += "&endHour=" + parameters.EndHour;
            GapUrl += "&endMinute=" + parameters.EndMinute;
            GapUrl += "&approachId={0}";
            return GapUrl;
        }

        private static string GetPeakUrl(FinalGapAnalysisReportParameters parameters)
        {
            var GapUrl = "PeakHours?signalId=" + parameters.SignalId;
            GapUrl += "&startDate=" + parameters.StartDate.ToString("M-d-yyyy");
            GapUrl += "&endDate=" + parameters.EndDate.ToString("M-d-yyyy");
            GapUrl += "&approachId={0}";
            return GapUrl;
        }

    }

    
}

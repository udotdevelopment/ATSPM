using AutoMapper;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using SPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Udot.Utils.Extensions;
using Udot.Utils.HelperFunctions;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace SPM.Controllers
{

    [Authorize(Roles = "Technician, Admin, Configuration")]
    public class EnhancedConfigurationController : Controller
    {

      

        public List<Signal> Signals { get; set; }

        // GET: EnhancedConfiguration
        public ActionResult Index()
        {

            return View();
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetAllFromQuery([FromBody] QueryDetails query)
        {
            SignalsRepository sr = new SignalsRepository();
            var queryResp = GetAllSignals(query);
            var signalsVM = queryResp.Signals.Select(Mapper.Map<Signal, SignalViewModel>).ToList();
            ControllerTypeRepository ctr = new ControllerTypeRepository();
            DetectionTypeRepository dtr = new DetectionTypeRepository();
            MovementTypeRepository mtr = new MovementTypeRepository();
            LaneTypeRepository ltr = new LaneTypeRepository();
            DirectionTypeRepository dirtr = new DirectionTypeRepository();

            var slvm = new SignalsListViewModel
            {
                TotalCount = queryResp.TotalCount,
                TotalCountInQuery = queryResp.TotalCountInQuery,
                Signals = signalsVM,
                Lookups =
                {
                    ControllerTypes =
                        Mapper.Map<List<ControllerType>, List<LookupTypeViewModel>>(
                            ctr.GetControllerTypes()),
                    DetectionTypes =
                        Mapper.Map<List<DetectionType>, List<LookupTypeViewModel>>(
                            dtr.GetAllDetectionTypes()),
                    MovementTypes =
                        Mapper.Map<List<MovementType>, List<LookupTypeViewModel>>(
                            mtr.GetAllMovementTypes()),
                    LaneTypes = Mapper.Map<List<LaneType>, List<LookupTypeViewModel>>(
                        ltr.GetAllLaneTypes()),
                    DirectionTypes =
                        Mapper.Map<List<DirectionType>, List<LookupTypeViewModel>>(
                            dirtr.GetAllDirections())
                }
            };


            return Json(slvm, JsonRequestBehavior.AllowGet);
        }



        [System.Web.Http.HttpPost]
        public JsonResult GetAllFromQueryApproaches([FromBody] QueryDetails query)
        {
            ApproachesViewModel res = new ApproachesViewModel();
            var approaches = GetApproaches(query, query.Id);
            if (approaches != null && approaches.Any())
            {
                res.Approaches = Mapper.Map<List<Approach>, List<ApproachViewModel>>(approaches);
            }
            res.SignalID = query.Id;
            res.TotalCount = GetTotalCount(query.Id);


            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public JsonResult GetAllFromQueryDetectors([FromBody] QueryDetails query)
        {
            DetectorsViewModel dvm = new DetectorsViewModel();
            DetectorRepository dr = new DetectorRepository();
            var detectorsInApproach = dr.GetDetectorsByApproachID(int.Parse(query.Id));
            var detectors = GetDetectors(query);
            foreach (var det in detectors)
            {
                dvm.Detectors.Add(Mapper.Map<Detector, DetectorViewModel>(det));
            }
            dvm.TotalCount = detectorsInApproach.Count;

            return Json(dvm, JsonRequestBehavior.AllowGet);
        }

        public List<Detector> GetDetectors(QueryDetails query)
        {
            List<Detector> dets = new List<Detector>();
            Approach appr = null;
            ApproachRepository ar = new ApproachRepository();
            appr = ar.GetApproachByApproachID(int.Parse(query.Id));

            if (appr == null)
                return dets;

            if (!string.IsNullOrEmpty(query.Filter))
                dets = appr.Detectors.AsQueryable()
                    .Where(y => y.Description.ToLower().Contains(query.Filter.ToLower()) || y.DetectorID.ToString().Contains(query.Filter.ToLower()))
                    .OrderByWithDirection(HelperFunctions.GetExpression<Detector>(query.OrderBy), query.DecesendingOrder)
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize).ToList();
            else
                dets = appr.Detectors.AsQueryable()
                    .OrderByWithDirection(HelperFunctions.GetExpression<Detector>(query.OrderBy), query.DecesendingOrder)
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize).ToList();
            return dets;
        }


        public int GetTotalCount(string signalId)
        {
            int res = 0;
            SignalsRepository sr = new SignalsRepository();
            var signals = sr.GetAllSignals();

            var signal = signals.Find(x => x.SignalID == signalId);
            if (signal != null && signal.Approaches != null)
                res = signal.Approaches.Count;

            return res;
        }
        public List<Approach> GetApproaches(QueryDetails query, string signalId)
        {

            List<Approach> apprs = new List<Approach>();
            SignalsRepository sr = new SignalsRepository();

            var signalVersions = sr.GetAllVersionsOfSignalBySignalID(signalId);
            var signal = signalVersions.Find(x => x.VersionID == query.VersionId);

            if (signal != null && signal.Approaches != null && signal.Approaches.Any())
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    var filter = query.Filter.ToLower();

                    apprs = signal.Approaches.AsQueryable()
                        .Where(y => y.Description.ToLower().Contains(filter) || y.ApproachID.ToString().Contains(filter))
                        .OrderByWithDirection(HelperFunctions.GetExpression<Approach>(query.OrderBy), query.DecesendingOrder)
                        .Skip((query.PageIndex - 1) * query.PageSize)
                        .Take(query.PageSize).ToList();
                }
                else
                {
                    apprs = signal.Approaches.AsQueryable().Where(x => x.SignalID == signalId)
                        .OrderByWithDirection(HelperFunctions.GetExpression<Approach>(query.OrderBy), query.DecesendingOrder)
                        .Skip((query.PageIndex - 1) * query.PageSize)
                        .Take(query.PageSize).ToList();
                }

            }

            return apprs;
        }

        public SignalsQueryResults GetAllSignals(QueryDetails query)
        {
            var res = new SignalsQueryResults();
            SignalsRepository sr = new SignalsRepository();
            var signals = sr.GetAllSignals();
            res.TotalCount = signals.Count;
            if (!string.IsNullOrEmpty(query.Filter))
            {
                var filter = query.Filter.ToLower();
                var words = filter.Split(new[] { ',', '-', ' ', '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    signals = signals.AsQueryable().Where(x => x.PrimaryName.ToLower().Contains(word) ||
                                                                x.SecondaryName.ToLower().Contains(word) ||
                                                                x.SignalDescription.ToLower().Contains(word) ||
                                                                x.SignalID.ToLower().Contains(word))
                                    .OrderByWithDirection(HelperFunctions.GetExpression<Signal>(query.OrderBy), query.DecesendingOrder).ToList();

                }
            }
            else
            {
                signals = signals
                                 .AsQueryable()
                                 .OrderByWithDirection(HelperFunctions.GetExpression<Signal>(query.OrderBy), query.DecesendingOrder).ToList();

            }

            res.TotalCountInQuery = signals.Count;
            if (query.PageSize > 0)
            {
                signals = signals.AsQueryable()
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize).ToList();
            }

            res.Signals = signals;
            return res;
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode CreateUpdateSignal(Signal signal)
        {
            SignalsRepository sr = new SignalsRepository();
            var existingSignal = sr.GetLatestVersionOfSignalBySignalID(signal.SignalID);
            if (existingSignal == null)
            {
                signal.RegionID = 2;
                signal.Start = DateTime.Today;
                signal.Note = "Create New";
                signal.VersionList = new List<Signal>();
                signal.VersionActionId = 1;

                try
                {
                    sr.AddOrUpdate(signal);
                    return HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    return HttpStatusCode.BadRequest;
                }
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }

            try
            {
                sr.AddOrUpdate(signal);
                return HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode CreateSignalVersion(Signal signal)
        {
            SignalsRepository sr = new SignalsRepository();
            Signal copyVersion = new Signal();
            Signal originalSignalVersion = sr.GetLatestVersionOfSignalBySignalID(signal.SignalID);
            copyVersion = Signal.CopyVersion(originalSignalVersion);
            copyVersion.VersionActionId = 4;
            copyVersion.Start = signal.Start;
            copyVersion.IPAddress = signal.IPAddress;
            copyVersion.Note = signal.Note;
            copyVersion.PrimaryName = signal.PrimaryName;
            copyVersion.SecondaryName = signal.SecondaryName;
            copyVersion.ControllerTypeID = signal.ControllerTypeID;
            copyVersion.Latitude = signal.Latitude;
            copyVersion.Longitude = signal.Longitude;
            copyVersion.Enabled = signal.Enabled;
            //copyVersion.Approaches = new List<Approach>();

            try
            {
                sr.AddOrUpdate(copyVersion);
                var commentRepository = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();
                MOE.Common.Models.Repositories.IMetricTypeRepository metricTyperepository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
                List<MOE.Common.Models.MetricType> allMetricTypes = metricTyperepository.GetAllToDisplayMetrics();
                foreach (var origVersionComment in signal.Comments)
                {
                    MetricComment metricComment = new MetricComment
                    {
                        CommentText = origVersionComment.CommentText,
                        VersionID = copyVersion.VersionID,
                        SignalID = signal.SignalID,
                        TimeStamp = DateTime.Now,
                        MetricTypeIDs = new List<int> { 1, 2 },
                        MetricIDs = new int[] { 1, 2 },
                    };

                    metricComment.MetricTypes = origVersionComment.MetricTypes;
                    commentRepository.Add(metricComment);
                    copyVersion.Comments.Add(metricComment);
                }
                //sr.AddOrUpdate(copyVersion);
                copyVersion = sr.GetLatestVersionOfSignalBySignalID(copyVersion.SignalID);
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
            
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode UpdateSignalVersion(Signal signal)
        {
            SignalsRepository sr = new SignalsRepository();
            var existingSignal = sr.GetLatestVersionOfSignalBySignalID(signal.SignalID);
            var commentRepository = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();
            MOE.Common.Models.Repositories.IMetricTypeRepository metricTyperepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            if (existingSignal != null)
            {
                //let's give values for some properties
                signal.VersionList = existingSignal.VersionList;
                signal.VersionActionId = 2;
            }          
            try
            {
                sr.AddOrUpdate(signal);
                if (signal.Comments != null && signal.Comments.Count > 0)
                {
                    foreach (var origVersionComment in signal.Comments)
                    {
                        MetricComment metricComment = new MetricComment
                        {
                            CommentText = origVersionComment.CommentText,
                            VersionID = signal.VersionID,
                            SignalID = signal.SignalID,
                            TimeStamp = DateTime.Now,
                            MetricTypeIDs = new List<int> { 1, 2 },
                            MetricIDs = new int[] { 1, 2 },
                        };

                        metricComment.MetricTypes = origVersionComment.MetricTypes;
                        commentRepository.Add(metricComment);
                    }
                }
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }


        [System.Web.Http.HttpGet]
        public JsonResult GetAllVersions(string signalID)
        {
            SignalsRepository sr = new SignalsRepository();
            var res = sr.GetAllVersionsOfSignalBySignalID(signalID);
            var signalsVM = res.Select(Mapper.Map<Signal, SignalViewModel>).ToList();

            return Json(signalsVM, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Http.HttpPost]
        public HttpStatusCode CreateUpdateApproach(Approach approach)
        {
            ApproachRepository ar = new ApproachRepository();
            SignalsRepository sr = new SignalsRepository();
            var existingSignal = sr.GetLatestVersionOfSignalBySignalID(approach.SignalID);

            try
            {
                approach.Index = GetApproachIndex(existingSignal);
                approach.Detectors = new List<Detector>();
                approach.VersionID = existingSignal.VersionID;
                ar.AddOrUpdate(approach);
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private string GetApproachIndex(Signal signal)
        {
            if (signal.Approaches != null)
            {
                return "Approaches[" + signal.Approaches.Count.ToString() + "].";
            }
            signal.Approaches = new List<Approach>();
            return "Approaches[0]";
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetSignalBySignalId(string signalID)
        {
            SignalsRepository sr = new SignalsRepository();
            SignalViewModel res = new SignalViewModel();

            var existingSignal = sr.GetLatestVersionOfSignalBySignalID(signalID);
            res = Mapper.Map<Signal, SignalViewModel>(existingSignal);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode DeleteSignal(String id)
        {
            SignalsRepository sr = new SignalsRepository();

            if (id == null)
            {
                return HttpStatusCode.BadRequest;
            }

            try
            {
                sr.SetAllVersionsOfASignalToDeleted(id);
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode DeleteVersion(String id)
        {
            SignalsRepository sr = new SignalsRepository();
            int _vid = Convert.ToInt32(id);
            Signal signal = sr.GetSignalVersionByVersionId(_vid);

            if (signal == null)
            {
                return HttpStatusCode.BadRequest;
            }

            string sigId = signal.SignalID;

            try
            {
                sr.SetVersionToDeleted(_vid);
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode DeleteApproach(String id)
        {
            ApproachRepository ar = new ApproachRepository();

            if (id == null)
            {
                return HttpStatusCode.BadRequest;
            }
            Approach approach = ar.GetApproachByApproachID(int.Parse(id));

            if (approach == null)
            {
                return HttpStatusCode.NotFound;
            }

            try
            {
                ar.Remove(int.Parse(id));
                return HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode DeleteDetector(String id)
        {
            DetectorRepository dr = new DetectorRepository();

            if (id == null)
            {
                return HttpStatusCode.BadRequest;
            }
            Detector det = dr.GetDetectorByID(int.Parse(id));

            if (det == null)
            {
                return HttpStatusCode.NotFound;
            }

            try
            {
                dr.Remove(int.Parse(id));
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCode AddUpdateDetector(Detector det)
        {
            SignalsRepository _signalsRepository = new SignalsRepository();
            ApproachRepository _approachRepository = new ApproachRepository();
            DetectorRepository _detectorRepository = new DetectorRepository();
            DetectionTypeRepository detectionTypeRepository = new DetectionTypeRepository();
            DetectionHardwareRepository detectionHardwareRepository = new DetectionHardwareRepository();
            var approach = _approachRepository.GetApproachByApproachID(det.ApproachID);
            Signal signal = _signalsRepository.GetSignalVersionByVersionId(approach.VersionID);



            //det.AllHardwareTypes = detectionHardwareRepository.GetAllDetectionHardwares();
            //det.DetectionHardware = new DetectionHardware();
            det.ApproachID = approach.ApproachID;
            det.AllDetectionTypes = detectionTypeRepository.GetAllDetectionTypesNoBasic();
            //det.DetectionTypeIDs = new List<int>();
            det.DetectionTypes = new List<DetectionType>();
            det.Index = det.ApproachID + "Detectors[" + approach.Detectors.Count.ToString() + "].";
            det.DetectorComments = new List<DetectorComment>();
            det.DateAdded = DateTime.Now;
            //det.DetChannel = _detectorRepository.GetMaximumDetectorChannel(approach.VersionID) + 1;
            det.DetectorID = signal.SignalID + det.DetChannel.ToString("D2");

            if (signal.Approaches.Count == 0)
            {
                signal.Approaches = _approachRepository.GetAllApproaches().Where(a => a.VersionID == signal.VersionID).ToList();
            }

            if(approach.Detectors.Where(d => det.ID == d.ID).FirstOrDefault() == null)
            {
                try
                {
                    _detectorRepository.Add(det);
                    return HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    return HttpStatusCode.BadRequest;
                }
            }
            else
            {
                _detectorRepository.Update(det);
                return HttpStatusCode.OK;
            }


        }

        public SignalsQueryResults GetAllDetector(QueryDetails query)
        {
            var res = new SignalsQueryResults();
            SignalsRepository sr = new SignalsRepository();
            var signals = sr.GetAllSignals();
            res.TotalCount = signals.Count;
            if (!string.IsNullOrEmpty(query.Filter))
            {
                var filter = query.Filter.ToLower();
                var words = filter.Split(new[] { ',', '-', ' ', '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    signals = signals.AsQueryable().Where(x => x.PrimaryName.ToLower().Contains(word) ||
                                                                x.SecondaryName.ToLower().Contains(word) ||
                                                                x.SignalDescription.ToLower().Contains(word) ||
                                                                x.SignalID.ToLower().Contains(word)).ToList();
                    //.OrderByWithDirection(HelperFunctions.GetExpression<Signal>(query.OrderBy), query.DecesendingOrder).ToList();

                }
            }
            else
            {
                signals = signals
                                 .AsQueryable().ToList();
                //.OrderByWithDirection(HelperFunctions.GetExpression<Signal>(query.OrderBy), query.DecesendingOrder).ToList();

            }

            res.TotalCountInQuery = signals.Count;
            if (query.PageSize > 0)
            {
                signals = signals.AsQueryable()
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize).ToList();
            }

            res.Signals = signals;
            return res;
        }

    }
    public class SignalsQueryResults
    {
        public List<Signal> Signals { get; set; }
        public int TotalCount { get; set; }
        public int TotalCountInQuery { get; set; }
    }
}

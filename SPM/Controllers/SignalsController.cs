using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using SPM.Filters;
using System.Windows.Forms;
using OfficeOpenXml;

namespace SPM.Controllers
{

    [Authorize(Roles = "Technician, Admin, Configuration")]
    public class SignalsController : Controller
    {
        private MOE.Common.Models.Repositories.IControllerTypeRepository _controllerTypeRepository;
        private MOE.Common.Models.Repositories.IRegionsRepository _regionRepository;
        private MOE.Common.Models.Repositories.IAreaRepository _areaRepository;
        private MOE.Common.Models.Repositories.IDirectionTypeRepository _directionTypeRepository;
        private MOE.Common.Models.Repositories.IMovementTypeRepository _movementTypeRepository;
        private MOE.Common.Models.Repositories.ILaneTypeRepository _laneTypeRepository;
        private MOE.Common.Models.Repositories.IDetectionHardwareRepository _detectionHardwareRepository;
        private MOE.Common.Models.Repositories.IJurisdictionRepository _jurisdictionRepository;
        private MOE.Common.Models.Repositories.ISignalsRepository _signalsRepository;
        private MOE.Common.Models.Repositories.IDetectorRepository _detectorRepository;
        private MOE.Common.Models.Repositories.IDetectionTypeRepository _detectionTypeRepository;
        private MOE.Common.Models.Repositories.IApproachRepository _approachRepository;
        private MOE.Common.Models.Repositories.IMetricTypeRepository _metricTypeRepository;
        private IControllerTypeRepository controllerTypeRepository;
        private IRegionsRepository regionRepository;
        private IDirectionTypeRepository directionTypeRepository;
        private IMovementTypeRepository movementTypeRepository;
        private ILaneTypeRepository laneTypeRepository;
        private IDetectionHardwareRepository detectionHardwareRepository;
        private ISignalsRepository signalsRepository;
        private IDetectorRepository detectorRepository;
        private IDetectionTypeRepository detectionTypeRepository;
        private IApproachRepository approachRepository;
        private IMetricTypeRepository metricTypeRepository;

        public SignalsController()
        {

            _signalsRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            _detectorRepository = MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            _detectionTypeRepository = MOE.Common.Models.Repositories.DetectionTypeRepositoryFactory.Create();
            _approachRepository = MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            _metricTypeRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            _controllerTypeRepository = MOE.Common.Models.Repositories.ControllerTypeRepositoryFactory.Create();
            _regionRepository = MOE.Common.Models.Repositories.RegionsRepositoryFactory.Create();
            _areaRepository = MOE.Common.Models.Repositories.AreaRepositoryFactory.Create();
            _directionTypeRepository = MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.Create();
            _movementTypeRepository = MOE.Common.Models.Repositories.MovementTypeRepositoryFactory.Create();
            _laneTypeRepository = MOE.Common.Models.Repositories.LaneTypeRepositoryFactory.Create();
            _detectionHardwareRepository = MOE.Common.Models.Repositories.DetectionHardwareRepositoryFactory.Create();
            _jurisdictionRepository = MOE.Common.Models.Repositories.JurisdictionRepositoryFactory.Create();
        }

        public SignalsController(
         MOE.Common.Models.Repositories.IControllerTypeRepository controllerTypeRepository,
         MOE.Common.Models.Repositories.IRegionsRepository regionRepository,
         MOE.Common.Models.Repositories.IAreaRepository areaRepository,
         MOE.Common.Models.Repositories.IDirectionTypeRepository directionTypeRepository,
         MOE.Common.Models.Repositories.IMovementTypeRepository movementTypeRepository,
         MOE.Common.Models.Repositories.ILaneTypeRepository laneTypeRepository,
         MOE.Common.Models.Repositories.IDetectionHardwareRepository detectionHardwareRepository,
         MOE.Common.Models.Repositories.ISignalsRepository signalsRepository,
         MOE.Common.Models.Repositories.IDetectorRepository detectorRepository,
         MOE.Common.Models.Repositories.IDetectionTypeRepository detectionTypeRepository,
         MOE.Common.Models.Repositories.IApproachRepository approachRepository,
         MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository,
         MOE.Common.Models.Repositories.IJurisdictionRepository jurisdictionRepository)
        {
            _signalsRepository = signalsRepository;
            _detectorRepository = detectorRepository;
            _detectionTypeRepository = detectionTypeRepository;
            _approachRepository = approachRepository;
            _controllerTypeRepository = controllerTypeRepository;
            _regionRepository = regionRepository;
            _areaRepository = areaRepository;
            _directionTypeRepository = directionTypeRepository;
            _movementTypeRepository = movementTypeRepository;
            _laneTypeRepository = laneTypeRepository;
            _detectionHardwareRepository = detectionHardwareRepository;
            _metricTypeRepository = metricTypeRepository;
            _jurisdictionRepository = jurisdictionRepository;
        }

        public SignalsController(IControllerTypeRepository controllerTypeRepository, IRegionsRepository regionRepository, IDirectionTypeRepository directionTypeRepository, IMovementTypeRepository movementTypeRepository, ILaneTypeRepository laneTypeRepository, IDetectionHardwareRepository detectionHardwareRepository, ISignalsRepository signalsRepository, IDetectorRepository detectorRepository, IDetectionTypeRepository detectionTypeRepository, IApproachRepository approachRepository, IMetricTypeRepository metricTypeRepository)
        {
            this.controllerTypeRepository = controllerTypeRepository;
            this.regionRepository = regionRepository;
            this.directionTypeRepository = directionTypeRepository;
            this.movementTypeRepository = movementTypeRepository;
            this.laneTypeRepository = laneTypeRepository;
            this.detectionHardwareRepository = detectionHardwareRepository;
            this.signalsRepository = signalsRepository;
            this.detectorRepository = detectorRepository;
            this.detectionTypeRepository = detectionTypeRepository;
            this.approachRepository = approachRepository;
            this.metricTypeRepository = metricTypeRepository;
        }

        public ActionResult Index()
        {
            MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel wctv =
                new MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel(_regionRepository, _metricTypeRepository, _jurisdictionRepository, _areaRepository);

            return View(wctv);
        }

        // GET: Signals
        [AllowAnonymous]
        public ActionResult SignalDetail()
        {
            MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel wctv =
                new MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel(_regionRepository, _metricTypeRepository, _jurisdictionRepository, _areaRepository);
            return View(wctv);
        }

        [Authorize(Roles = "Admin, Configuration")]
        public int AddNewVersion(string id)
        {
            var existingSignal = _signalsRepository.GetLatestVersionOfSignalBySignalID(id);
            //if (existingSignal == null)
            //{
            //    return Content("<h1>" +"No Signal Matches this SignalID" + "</h1>");
            //}

            Signal signal = _signalsRepository.CopySignalToNewVersion(existingSignal);
            signal.VersionList = _signalsRepository.GetAllVersionsOfSignalBySignalID(signal.SignalID);
            try
            {
                _signalsRepository.AddOrUpdate(signal);
                var commentRepository = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();
                foreach (var origVersionComment in existingSignal.Comments)
                {
                    MetricComment metricComment = new MetricComment
                    {
                        CommentText = origVersionComment.CommentText,
                        VersionID = signal.VersionID,
                        SignalID = existingSignal.SignalID,
                        TimeStamp = origVersionComment.TimeStamp,
                    };
                    if (origVersionComment.MetricTypes != null)
                    {
                        metricComment.MetricTypeIDs = new List<int>();
                        foreach (var metricType in origVersionComment.MetricTypes)
                        {
                            metricComment.MetricTypeIDs.Add(metricType.MetricID);
                        }
                    }
                    commentRepository.AddOrUpdate(metricComment);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                AddSelectListsToViewBag(signal);
            }
            return signal.VersionID;
        }



        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult AddApproach(string versionId)
        {
            int id = Convert.ToInt32(versionId);
            var signal = _signalsRepository.GetSignalVersionByVersionId(id);
            Approach approach = GetNewApproach(signal);
            _approachRepository.AddOrUpdate(approach);
            AddSelectListsToViewBag(signal);
            return PartialView(approach);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult CopyApproach(int versionId, int approachID)
        {
            var signal = _signalsRepository.GetSignalVersionByVersionId(versionId);
            //Approach approachFromDatabase = signal.Approaches.Where(a => a.ApproachID == approachID).First();
            AddSelectListsToViewBag(signal);
            try
            {
                Approach newApproach = Approach.CopyApproach(approachID);
                _approachRepository.AddOrUpdate(newApproach);
                return Content("<h1>Copy Successful!</h1>");
            }
            catch (Exception ex)
            {
                return Content("<h1>" + ex.Message + "</h1>");
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

        private Approach GetNewApproach(Signal signal)
        {
            Approach approach = new Approach();
            approach.Detectors = new List<Detector>();
            approach.ApproachID = 0;
            approach.SignalID = signal.SignalID;
            approach.Description = "New Phase/Direction";
            approach.Index = GetApproachIndex(signal);
            approach.DirectionTypeID = 1;
            approach.VersionID = signal.VersionID;
            approach.IsProtectedPhaseOverlap = false;

            return approach;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult AddDetector(int versionId, int approachID, string approachIndex)
        {
            Signal signal = _signalsRepository.GetSignalVersionByVersionId(versionId);
            if (signal.Approaches.Count == 0)
            {
                signal.Approaches = _approachRepository.GetAllApproaches().Where(a => a.VersionID == signal.VersionID).ToList();
            }
            var approach = signal.Approaches.Where(s => s.ApproachID == approachID).First();
            Detector detector = CreateNewDetector(approach, approachIndex, signal.SignalID);
            AddSelectListsToViewBag(signal);
            return PartialView(detector);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult CopyDetector(int ID, int versionId, int approachID, string approachIndex)
        {
            Detector newDetector = Detector.CopyDetector(ID, true); //need to increase DetChannel if not copying the whole signal.
            Signal signal = _signalsRepository.GetSignalVersionByVersionId(versionId);
            Approach approach = signal.Approaches.Where(s => s.ApproachID == approachID).First();
            newDetector.ApproachID = approach.ApproachID;
            newDetector.Index = approachIndex + "Detectors[" + approach.Detectors.Count.ToString() + "].";
            MOE.Common.Models.Repositories.IDetectorRepository detectorRepository =
                MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            detectorRepository.Add(newDetector);  //Do the Repository Add FOR detectors AT the detector leve.
            newDetector.Approach = approach;  //????do not associate up!!! Add from top down!
            //approach.Detectors.Add(newDetector);
            AddSelectListsToViewBag(signal);
            return PartialView("AddDetector", newDetector);
        }


        [Authorize(Roles = "Admin, Configuration")]
        private Detector CreateNewDetector(Approach approach, string approachIndex, string signalID)
        {
            Detector detector = new Detector();
            detector.ApproachID = approach.ApproachID;
            detector.AllDetectionTypes = _detectionTypeRepository.GetAllDetectionTypesNoBasic();
            detector.DetectionTypeIDs = new List<int>();
            detector.DetectionTypes = new List<DetectionType>();
            detector.Index = approachIndex + "Detectors[" + approach.Detectors.Count.ToString() + "].";
            detector.DetectorComments = new List<DetectorComment>();
            detector.DateAdded = DateTime.Now;
            detector.DetChannel = _detectorRepository.GetMaximumDetectorChannel(approach.VersionID) + 1;
            detector.DetectorID = signalID + detector.DetChannel.ToString("D2");
            detector = _detectorRepository.Add(detector);
            detector.Approach = approach;
            return detector;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Import()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var file = HttpContext.Request.Files["file"];
                if (file == null) return Content("Invalid file uploaded");
                var package = new ExcelPackage(file.InputStream);
                var signalInformation = package.Workbook.Worksheets["Signal Information"];

                var signalId = signalInformation.GetValue<string>(4, 2);
                var existingSignal = _signalsRepository.GetLatestVersionOfSignalBySignalID(signalId);
                if (existingSignal == null)
                {
                    var signal = CreateNewSignal(signalId);
                    signal.Note += " (Import)";
                    ImportSignal(signalInformation, signal);
                    ImportApproachesAndDetectors(package, signal);

                    // Load signal including approaches and detectors
                    signal = _signalsRepository.GetSignalVersionByVersionId(signal.VersionID);

                    AddSelectListsToViewBag(signal);
                    return PartialView("Edit", signal);
                }

                // Add new version
                var versionId = AddNewVersion(signalId);
                // Load new version (copy of the original)
                var newSignal = _signalsRepository.GetSignalVersionByVersionId(versionId);
                
                // Update signal information
                ImportSignal(signalInformation, newSignal);

                // remove existing approaches (they are copies from the original)
                var existingApproaches = newSignal.Approaches;
                foreach (var existingApproach in existingApproaches)
                {
                    _approachRepository.Remove(existingApproach);
                }
                newSignal.Approaches.Clear();

                // Add approaches and detectors from spreadsheet
                ImportApproachesAndDetectors(package, newSignal);

                // Load signal including new approaches, detectors, and version list
                newSignal = _signalsRepository.GetSignalVersionByVersionId(versionId);
                newSignal.VersionList = _signalsRepository.GetAllVersionsOfSignalBySignalID(newSignal.SignalID);

                AddSelectListsToViewBag(newSignal);
                return PartialView("Edit", newSignal);
            }
            catch (Exception ex)
            {
                return Content("<h1>" + "Error importing signal, validate spreadsheet." + "</h1> " +
                               "<br/>" + ex.Message);
            }

        }

        private void ImportSignal(ExcelWorksheet signalInformation, Signal signal)
        {
            var controllerType =
                    _controllerTypeRepository.GetControllerTypeByDesc(signalInformation.GetValue<string>(4, 8));
            signal.PrimaryName = signalInformation.GetValue<string>(4, 4);
            signal.SecondaryName = signalInformation.GetValue<string>(4, 6);
            if (controllerType != null)
                signal.ControllerTypeID = controllerType.ControllerTypeID;
            signal.Latitude = signalInformation.GetValue<string>(5, 2);
            signal.Longitude = signalInformation.GetValue<string>(5, 4);
            signal.IPAddress = signalInformation.GetValue<string>(5, 6);
            signal.VersionList = new List<Signal> { signal };
            _signalsRepository.AddOrUpdate(signal);
        }

        private void ImportApproachesAndDetectors(ExcelPackage package, Signal signal)
        {
            for (var i = 1; i < 9; i++)
            {
                var sheet = package.Workbook.Worksheets[$"Approach {i}"];
                if (sheet != null)
                {
                    var approach = GetNewApproach(signal);
                    var directionType = _directionTypeRepository.GetByAbbreviation(sheet.GetValue<string>(4, 2));
                    if (directionType == null)
                        continue;

                    approach.DirectionTypeID = directionType.DirectionTypeID;
                    approach.Description = sheet.GetValue<string>(5, 2);
                    approach.ProtectedPhaseNumber = sheet.GetValue<int>(4, 4);
                    approach.PermissivePhaseNumber = sheet.GetValue<int>(5, 4);
                    approach.IsProtectedPhaseOverlap = sheet.GetValue<bool>(4, 6);
                    approach.IsPermissivePhaseOverlap = sheet.GetValue<bool>(5, 6);
                    _approachRepository.AddOrUpdate(approach);

                    var row = 10;
                    var detChannel = sheet.GetValue<int>(row, 1);
                    while (detChannel != 0)
                    {
                        var detector = new Detector
                        {
                            ApproachID = approach.ApproachID,
                            AllDetectionTypes = _detectionTypeRepository.GetAllDetectionTypesNoBasic(),
                            DetectionTypeIDs = new List<int>(),
                            DetectionTypes = new List<DetectionType>(),
                            Index = approach.Index + "Detectors[" + approach.Detectors.Count + "].",
                            DetectorComments = new List<DetectorComment>(),
                            DateAdded = DateTime.Now,
                            DetChannel = detChannel
                        };
                        detector.DetectorID = signal.SignalID + detector.DetChannel.ToString("D2");

                        var hardware =
                            _detectionHardwareRepository.GetDetectionHardwareByName(
                                sheet.GetValue<string>(row, 8));
                        if (hardware != null)
                        {
                            detector.DetectionHardwareID = hardware.ID;
                        }

                        detector.LatencyCorrection = sheet.GetValue<double>(row, 9);
                        detector.LaneNumber = sheet.GetValue<int>(row, 10);
                        var mvmtType =
                            _movementTypeRepository.GetMovementTypeByDesc(sheet.GetValue<string>(row, 11));
                        if (mvmtType != null)
                        {
                            detector.MovementTypeID = mvmtType.MovementTypeID;
                        }

                        var laneType =
                            _laneTypeRepository.GetLaneTypeByLaneDesc(sheet.GetValue<string>(row, 12));
                        if (laneType != null)
                        {
                            detector.LaneTypeID = laneType.LaneTypeID;
                        }
                        // Populate detection types
                        var advCount = sheet.GetValue<bool>(row, 2);
                        if (advCount)
                        {
                            detector.DetectionTypeIDs.Add(2);
                        }

                        var advSpeed = sheet.GetValue<bool>(row, 3);
                        if (advSpeed)
                        {
                            detector.DetectionTypeIDs.Add(3);
                        }

                        var lblCount = sheet.GetValue<bool>(row, 4);
                        if (lblCount)
                        {
                            detector.DetectionTypeIDs.Add(4);
                        }

                        var lblSpdRestr = sheet.GetValue<bool>(row, 5);
                        if (lblSpdRestr)
                        {
                            detector.DetectionTypeIDs.Add(5);
                        }

                        var stopBarPres = sheet.GetValue<bool>(row, 6);
                        if (stopBarPres)
                        {
                            detector.DetectionTypeIDs.Add(6);
                        }

                        var advPres = sheet.GetValue<bool>(row, 7);
                        if (advPres)
                        {
                            detector.DetectionTypeIDs.Add(7);
                        }

                        detector.DistanceFromStopBar = sheet.GetValue<int>(row, 14);
                        detector.MinSpeedFilter = sheet.GetValue<int>(row, 15);
                        detector.DecisionPoint = sheet.GetValue<int>(row, 16);
                        detector.MovementDelay = sheet.GetValue<int>(row, 17);
                        _detectorRepository.Add(detector);
                        row++;
                        detChannel = sheet.GetValue<int>(row, 1);
                    }
                }
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Create(string id)
        {
            var existingSignal = _signalsRepository.GetLatestVersionOfSignalBySignalID(id);
            if (existingSignal == null)
            {
                Signal signal = CreateNewSignal(id);
                try
                {
                    _signalsRepository.AddOrUpdate(signal);
                    signal.VersionList = new List<Signal> { signal };
                }
                catch (Exception ex)
                {
                    return Content("<h1>" + ex.Message + "</h1>");
                }
                finally
                {
                    AddSelectListsToViewBag(signal);
                }
                return PartialView("Edit", signal);
            }
            return Content("<h1>Signal Already Exists</h1>");
        }


        [Authorize(Roles = "Admin, Configuration")]
        private Signal CreateNewSignal(string id)
        {
            Signal signal = new Signal();
            signal.SignalID = id;
            signal.PrimaryName = "ChangeMe";
            signal.SecondaryName = "ChangeMe";
            signal.IPAddress = "10.10.10.10";
            signal.Latitude = "0";
            signal.Longitude = "0";
            signal.RegionID = 2;
            signal.ControllerTypeID = 1;
            signal.Start = DateTime.Today;
            signal.Note = "Create New";
            signal.Enabled = true;
            signal.Pedsare1to1 = true;
            signal.VersionList = new List<Signal>();
            signal.VersionActionId = 1;
            signal.JurisdictionId = 1;
            return signal;
        }

        // POST: Signals/Copy
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Copy(string id, string newId)
        {
            Signal newSignal = new Signal();
            if (id == null)
            {
                return Content("<h1>A signal ID is required</h1>");
            }
            Signal signal = _signalsRepository.GetLatestVersionOfSignalBySignalID(id);
            if (signal != null)
            {
                newSignal = Signal.CopySignal(signal, newId);
                newSignal.VersionActionId = 1;
                newSignal.Start = DateTime.Now;
                newSignal.Note = "Copy of Signal " + id;
                newSignal.VersionList = new List<Signal> { newSignal };
            }
            try
            {
                _signalsRepository.AddOrUpdate(newSignal);
            }
            catch (Exception ex)
            {
                return Content("<h1>" + ex.Message + "</h1>");
            }
            finally
            {
                AddSelectListsToViewBag(newSignal);
            }
            return PartialView("Edit", newSignal);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult CopyVersion(Signal signal)
        {

            //originalSignalVersion = SetDetectionTypes(originalSignalVersion);
            //MOE.Common.Models.Repositories.ISignalsRepository repository =
            //    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //repository.AddOrUpdate(originalSignalVersion);
            Signal copyVersion = new Signal();

            Signal originalSignalVersion = _signalsRepository.GetLatestVersionOfSignalBySignalID(signal.SignalID);
            if (originalSignalVersion != null)
            {
                copyVersion = Signal.CopyVersion(originalSignalVersion);
                copyVersion.VersionActionId = 4;
                copyVersion.Start = DateTime.Now;
                copyVersion.IPAddress = originalSignalVersion.IPAddress;
                copyVersion.Note = "Copy of Version " + originalSignalVersion.Note;
            }
            try
            {
                _signalsRepository.AddOrUpdate(copyVersion);
                var commentRepository = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();
                foreach (var origVersionComment in originalSignalVersion.Comments)
                {
                    MetricComment metricComment = new MetricComment
                    {
                        CommentText = origVersionComment.CommentText,
                        VersionID = copyVersion.VersionID,
                        SignalID = originalSignalVersion.SignalID,
                        TimeStamp = origVersionComment.TimeStamp,
                    };
                    if (origVersionComment.MetricTypes != null)
                    {
                        metricComment.MetricTypeIDs = new List<int>();
                        foreach (var metricType in origVersionComment.MetricTypes)
                        {
                            metricComment.MetricTypeIDs.Add(metricType.MetricID);
                        }
                    }
                    if (origVersionComment.MetricIDs != null)
                        metricComment.MetricIDs = (int[])origVersionComment.MetricIDs.Clone();
                    metricComment.MetricTypes = origVersionComment.MetricTypes;
                    //commentRepository.Add(metricComment);
                    copyVersion.Comments.Add(metricComment);
                }
                _signalsRepository.AddOrUpdate(copyVersion);
                copyVersion = _signalsRepository.GetLatestVersionOfSignalBySignalID(copyVersion.SignalID);
            }
            catch (Exception ex)
            {
                return Content("<h1>" + ex.Message + "</h1>");
            }
            finally
            {
                AddSelectListsToViewBag(copyVersion);
            }
            return PartialView("Edit", copyVersion);
        }

        // GET: Signals/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return Content("<h1>A signal ID is required</h1>");
            }
            Signal signal = _signalsRepository.GetLatestVersionOfSignalBySignalID(id);
            signal.Approaches = signal.Approaches.OrderBy(a => a.ProtectedPhaseNumber).ThenBy(a => a.DirectionType.Description).ToList();
            signal.Areas = signal.Areas.OrderBy(a => a.AreaName).ToList();

            if (signal.Approaches == null)
            {
                signal.Approaches = new List<Approach>();
            }


            foreach (Approach approach in signal.Approaches)
            {
                approach.Detectors = approach.Detectors.OrderBy(d => d.DetectorID).ToList();
            }

            if (signal != null)
            {
                if (signal.Note == null)
                {
                    signal.Note = "";
                }

                List<DetectionType> allDetectionTypes = _detectionTypeRepository.GetAllDetectionTypesNoBasic();
                foreach (Approach a in signal.Approaches)
                {
                    foreach (Detector gd in a.Detectors)
                    {
                        gd.Index = a.Index + "Detector[" + a.Detectors.ToList().FindIndex(d => d.DetectorID == gd.DetectorID).ToString() + "].";
                        gd.AllDetectionTypes = allDetectionTypes;
                        gd.DetectionTypeIDs = new List<int>();
                        gd.DetectorComments = gd.DetectorComments.OrderByDescending(x => x.TimeStamp).ToList();
                        foreach (DetectionType dt in gd.DetectionTypes)
                        {
                            gd.DetectionTypeIDs.Add(dt.DetectionTypeID);
                        }
                    }
                    a.Index = "Approaches[" + signal.Approaches.ToList().FindIndex(app => app.ApproachID == a.ApproachID).ToString() + "].";
                }
                if (signal == null)
                {
                    return HttpNotFound();
                }

                signal.Comments = signal.Comments.OrderByDescending(s => s.TimeStamp).ToList();
                AddSelectListsToViewBag(signal);
                //foreach (MOE.Common.Models.MetricComment c in signal.Comments)
                //{
                //    c.MetricTypes = _metricTypeRepository.GetMetricTypesByMetricComment(c);
                //}
            }
            return PartialView(signal);
        }

        // GET: Signals/Edit/5
        [Authorize(Roles = "Admin, Configuration, Technician")]
        public ActionResult EditVersion(string Id)
        {
            if (Id == null)
            {
                return Content("<h1>A Version ID is required</h1>");
            }
            Signal signal = _signalsRepository.GetSignalVersionByVersionId(Convert.ToInt32(Id));
            signal.Approaches = signal.Approaches.OrderBy(a => a.ProtectedPhaseNumber).ThenBy(a => a.DirectionType.Description).ToList();
            if (signal.Approaches == null)
            {
                signal.Approaches = new List<Approach>();
            }
            foreach (Approach approach in signal.Approaches)
            {
                approach.Detectors = approach.Detectors.OrderBy(d => d.DetectorID).ToList();
            }
            if (signal != null)
            {
                if (signal.Note == null)
                {
                    signal.Note = "";
                }
                List<DetectionType> allDetectionTypes = _detectionTypeRepository.GetAllDetectionTypesNoBasic();
                foreach (Approach a in signal.Approaches)
                {
                    foreach (Detector gd in a.Detectors)
                    {
                        gd.Index = a.Index + "Detector[" + a.Detectors.ToList().FindIndex(d => d.DetectorID == gd.DetectorID).ToString() + "].";
                        gd.AllDetectionTypes = allDetectionTypes;
                        gd.DetectionTypeIDs = new List<int>();
                        gd.DetectorComments = gd.DetectorComments.OrderByDescending(x => x.TimeStamp).ToList();
                        foreach (DetectionType dt in gd.DetectionTypes)
                        {
                            gd.DetectionTypeIDs.Add(dt.DetectionTypeID);
                        }
                    }
                    a.Index = "Approaches[" + signal.Approaches.ToList().FindIndex(app => app.ApproachID == a.ApproachID).ToString() + "].";
                }
                signal.Comments = signal.Comments.OrderByDescending(s => s.TimeStamp).ToList();
                signal.VersionList = _signalsRepository.GetAllVersionsOfSignalBySignalID(signal.SignalID);
                AddSelectListsToViewBag(signal);
            }
            return PartialView("Edit", signal);
        }

        [AllowAnonymous]
        public ActionResult SignalDetailResult(string id)
        {
            if (id == null)
            {
                return Content("<h1>A signal ID is required</h1>");
            }
            Signal signal = _signalsRepository.GetLatestVersionOfSignalBySignalID(id);
            signal.Approaches = signal.Approaches.OrderBy(a => a.ProtectedPhaseNumber).ThenBy(a => a.DirectionType.Description).ToList();
            foreach (Approach approach in signal.Approaches)
            {
                approach.Detectors = approach.Detectors.OrderBy(d => d.DetectorID).ToList();
            }
            if (signal != null)
            {
                List<DetectionType> allDetectionTypes = _detectionTypeRepository.GetAllDetectionTypesNoBasic();
                foreach (Approach a in signal.Approaches)
                {
                    foreach (Detector gd in a.Detectors)
                    {
                        gd.Index = a.Index + "Detector[" + a.Detectors.ToList().FindIndex(d => d.DetectorID == gd.DetectorID).ToString() + "].";
                        gd.AllDetectionTypes = allDetectionTypes;
                        gd.DetectionTypeIDs = new List<int>();
                        gd.DetectorComments = gd.DetectorComments.OrderByDescending(x => x.TimeStamp).ToList();
                        foreach (DetectionType dt in gd.DetectionTypes)
                        {
                            gd.DetectionTypeIDs.Add(dt.DetectionTypeID);
                        }
                    }
                    a.Index = "Approaches[" + signal.Approaches.ToList().FindIndex(app => app.ApproachID == a.ApproachID).ToString() + "].";
                }
                if (signal == null)
                {
                    return HttpNotFound();
                }

                signal.Comments = signal.Comments.OrderByDescending(s => s.TimeStamp).ToList();
                AddSelectListsToViewBag(signal);
                //foreach (MOE.Common.Models.MetricComment c in signal.Comments)
                //{
                //    c.MetricTypes = _metricTypeRepository.GetMetricTypesByMetricComment(c);
                //}
            }
            return PartialView(signal);
        }

        // POST: Signals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Edit(Signal signal)
        {
            try
            {
                ModelState.Clear();
                signal = SetDetectionTypes(signal);

                //var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);

                if (TryValidateModel(signal))
                {
                    MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                        MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                    signalRepository.AddOrUpdate(signal);
                    AddSelectListsToViewBag(signal);

                    foreach (var approach in signal.Approaches)
                    {
                        if (TryValidateModel(approach))
                        {
                            MOE.Common.Models.Repositories.IApproachRepository approachRepository =
                                MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
                            approachRepository.AddOrUpdate(approach);
                        }
                    }
                    return Content("Save Successful! " + DateTime.Now.ToString());
                }
                return Content("There was a validation error.");
            }

            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private Signal SetDetectionTypes(Signal signal)
        {
            if (signal.Approaches != null)
            {
                foreach (Approach a in signal.Approaches)
                {
                    if (a.Detectors != null)
                    {
                        foreach (Detector gd in a.Detectors)
                        {
                            gd.DetectorID = a.SignalID + gd.DetChannel.ToString("D2");
                            if (gd.DetectionTypeIDs == null)
                            {
                                gd.DetectionTypeIDs = new List<int>();
                            }
                            if (gd.DetectionIDs != null)
                            {
                                foreach (string detectionTypeID in gd.DetectionIDs)
                                {
                                    gd.DetectionTypeIDs.Add(Convert.ToInt32(detectionTypeID));
                                }
                            }
                        }
                    }
                }
            }
            return signal;
        }

        private void AddSelectListsToViewBag(Signal signal)
        {
            var ids = new List<int>();
            if (signal.Areas != null && signal.Areas.FirstOrDefault() != null)
            {
                foreach (var a in signal.Areas)
                {
                    ids.Add(a.Id);
                }
            }
            ViewBag.AreaIds = ids;
            ViewBag.ControllerType = new SelectList(_controllerTypeRepository.GetControllerTypes(), "ControllerTypeID", "Description", signal.ControllerTypeID);
            ViewBag.Region = new SelectList(_regionRepository.GetAllRegions(), "ID", "Description", signal.RegionID);
            ViewBag.Areas = new MultiSelectList(_areaRepository.GetAllAreas(), "Id", "AreaName", _areaRepository.GetListOfAreasForSignal(signal.SignalID));
            ViewBag.DirectionType = new SelectList(_directionTypeRepository.GetAllDirections(), "DirectionTypeID", "Abbreviation");
            ViewBag.MovementType = new SelectList(_movementTypeRepository.GetAllMovementTypes(), "MovementTypeID", "Description");
            ViewBag.LaneType = new SelectList(_laneTypeRepository.GetAllLaneTypes(), "LaneTypeID", "Description");
            ViewBag.DetectionHardware = new SelectList(_detectionHardwareRepository.GetAllDetectionHardwares(), "ID", "Name");
            ViewBag.Jurisdictions = new SelectList(_jurisdictionRepository.GetAllJurisdictions(), "Id", "JurisdictionName");
        }

        // GET: Signals/Delete/5
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _signalsRepository.SetAllVersionsOfASignalToDeleted(id);


            MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel wctv =
                new MOE.Common.Models.ViewModel.WebConfigTool.WebConfigToolViewModel(_regionRepository, _metricTypeRepository, _jurisdictionRepository, _areaRepository);

            return null;//View(wctv);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        [HttpPost, ActionName("DeleteVersion")]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Admin, Configuration")]
        public ActionResult DeleteVersion(string versionId)
        {
            int _vid = Convert.ToInt32(versionId);
            Signal signal = _signalsRepository.GetSignalVersionByVersionId(_vid);

            if (signal == null)
            {
                return Content("<h1>" + "No Version with this ID can be found " + "</h1>");
            }

            string sigId = signal.SignalID;

            Signal mostRecentVersion;

            try
            {
                _signalsRepository.SetVersionToDeleted(_vid);

            }
            catch (Exception ex)
            {
                return Content("<h1>" + ex.Message + "</h1>");
            }
            finally
            {
                mostRecentVersion = _signalsRepository.GetLatestVersionOfSignalBySignalID(sigId);
                AddSelectListsToViewBag(mostRecentVersion);
            }
            return PartialView("Edit", mostRecentVersion);

        }
    }
}

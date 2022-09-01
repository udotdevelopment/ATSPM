using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business;
using MOE.Common.Business.CustomReport;
using NuGet;

namespace MOE.Common.Models.Repositories
{
    public class SignalsRepository : ISignalsRepository
    {
        private readonly SPM _db;


        public SignalsRepository()
        {
            _db = new SPM();
        }

        public SignalsRepository(SPM context)
        {
            _db = context;
        }


        public List<Signal> GetAllSignals()
        {
            return GetLatestVersionOfAllSignals();
        }

        public Signal GetVersionOfSignalByDate(string signalId, DateTime startDate)
        {
            var signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.Start <= startDate)
                .Where(signal => signal.VersionActionId != 3)
                .ToList();

            Signal versionSignal;
            if (signals.Count > 1)
            {
                var orderedSignals = signals.OrderByDescending(signal => signal.Start);
                versionSignal = orderedSignals.First();
            }
            else
            {
                versionSignal = signals.FirstOrDefault();
            }

            if (versionSignal != null)
            {
                AddSignalAndDetectorLists(versionSignal);
            }
            return versionSignal;
        }

        public Signal GetVersionOfSignalByDateWithDetectionTypes(string signalId, DateTime startDate)
        {
            var signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.Start <= startDate)
                .Where(signal => signal.VersionActionId != 3)
                .ToList();

            if (signals.Count > 1)
            {
                var orderedSignals = signals.OrderByDescending(signal => signal.Start);
                return orderedSignals.First();
            }
            else
            {
                return signals.FirstOrDefault();
            }
        }

        public Signal GetSignalVersionByVersionId(int versionId)
        {
            var version = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .FirstOrDefault(signal => signal.VersionID == versionId);
            if (version != null)
            {
                AddSignalAndDetectorLists(version);
            }
            return version;
        }

        public void SetVersionToDeleted(int versionId)
        {
            var signal = (from r in _db.Signals where r.VersionID == versionId select r).FirstOrDefault();
            if (signal != null)
                signal.VersionActionId = 3;
            _db.SaveChanges();
        }

        public void SetAllVersionsOfASignalToDeleted(string signalId)
        {
            var signals = from r in _db.Signals
                where r.SignalID == signalId
                select r;

            foreach (var s in signals)
                s.VersionActionId = 3;

            _db.SaveChanges();
        }

        public List<Signal> GetSignalsBetweenDates(string signalId, DateTime startDate, DateTime endDate)
        {
            var signals = new List<Signal>();
            var signalBeforeStart = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId
                                 && signal.Start <= startDate
                                 && signal.VersionActionId != 3).OrderByDescending(s => s.Start)
                .Take(1)
                .FirstOrDefault();
            if (signalBeforeStart != null)
                signals.Add(signalBeforeStart);
            if (_db.Signals.Any(signal => signal.SignalID == signalId
                                          && signal.Start > startDate
                                          && signal.Start < endDate
                                          && signal.VersionActionId != 3))
                signals.AddRange(_db.Signals
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                    .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                    .Where(signal => signal.SignalID == signalId
                                     && signal.Start > startDate
                                     && signal.Start < endDate
                                     && signal.VersionActionId != 3).ToList());
            return signals;
        }

        public bool Exists(string signalId)
        {
            return _db.DatabaseArchiveExcludedSignals.Any(s => s.SignalId == signalId);
        }

        public Signal CopySignalToNewVersion(Signal originalVersion)
        {
            var newVersion = new Signal();

            //originalVersion.VersionAction = (from r in _db.VersionActions
            //    where r.ID == 4
            //    select r).FirstOrDefault();

            newVersion.VersionAction = (from r in _db.VersionActions
                where r.ID == 4
                select r).FirstOrDefault();


            newVersion.SignalID = originalVersion.SignalID;
            newVersion.Start = DateTime.Today;
            newVersion.Note = "Copy of " + originalVersion.Note;
            newVersion.PrimaryName = originalVersion.PrimaryName;
            newVersion.SecondaryName = originalVersion.SecondaryName;
            newVersion.IPAddress = originalVersion.IPAddress;
            newVersion.ControllerTypeID = originalVersion.ControllerTypeID;
            newVersion.RegionID = originalVersion.RegionID;
            newVersion.Enabled = originalVersion.Enabled;
            newVersion.Pedsare1to1 = originalVersion.Pedsare1to1;
            newVersion.Latitude = originalVersion.Latitude;
            newVersion.Longitude = originalVersion.Longitude;
            newVersion.JurisdictionId = originalVersion.JurisdictionId;
            _db.Signals.Add(newVersion);
            _db.SaveChanges();

            CopyApproaches(originalVersion, newVersion);

            return newVersion;
        }

        public List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId)
        {
            var signals = GetLatestVersionOfAllSignals();

            var filteredSignals = (from r in signals
                where r.ControllerTypeID == controllerTypeId
                select r).ToList();

            return filteredSignals;
        }

        public List<Signal> EagerLoadAllSignals()
        {
            var signals = _db.Signals.Where(r => r.VersionActionId != 3)
                .GroupBy(r => r.SignalID)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .ToList();

            return signals;
        }

        public List<Signal> GetAllEnabledSignals()
        {
            _db.Configuration.LazyLoadingEnabled = false;

            var signals = GetLatestVersionOfAllSignals().ToList();
            return signals;
        }

        public string GetSignalLocation(string signalId)
        {
            var signal = GetLatestVersionOfSignalBySignalID(signalId);
            var location = string.Empty;
            if (signal != null)
                location = signal.PrimaryName + " @ " + signal.SecondaryName;

            return location;
        }

        public string GetSignalDescription(string signalId)
        {
            var signal = (from r in _db.Signals
                where r.SignalID == signalId
                select r).FirstOrDefault();
            var location = string.Empty;
            if (signal != null)
                location = signal.SignalDescription;

            return location;
        }

        

        public void AddOrUpdate(Signal signal)
        {
            var g = (from r in _db.Signals
                where r.VersionID == signal.VersionID
                select r).FirstOrDefault();
            if (g == null)
            {
                if (signal.Approaches != null)
                {
                    foreach (var appr in signal.Approaches)
                    {
                        if (appr.Detectors != null)
                        {
                            foreach (var det in appr.Detectors)
                            {
                                AddDetectiontypestoDetector(det);
                            }
                        }
                    }
                }
                _db.Signals.Add(signal);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent
                    {
                        ApplicationName = "MOE.Common",
                        Class = "Models.Repository.SignalRepository",
                        Function = "AddOrUpdate",
                        Description = ex.Message,
                        SeverityLevel = ApplicationEvent.SeverityLevels.High,
                        Timestamp = DateTime.Now
                    };
                    repository.Add(error);
                    throw;
                }
            }
            else
            {
                Update(signal);
                //throw new Exception("Signal already exists in the database");
            }
        }

        private void AddDetectiontypestoDetector(Detector detector)
        {
            try
            {
                var g = (from r in _db.Detectors
                    where r.ID == detector.ID
                    select r).FirstOrDefault();
                if (g == null)
                {
                    detector.DetectionTypes = new List<DetectionType>();
                    detector.DetectionTypes = _db.DetectionTypes
                        .Where(dt => detector.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                }
            }

            catch (Exception ex)
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.SignalRepository";
                error.Function = "Add";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }

        public void AddList(List<Signal> signals)
        {
            foreach (var s in signals)
                try
                {
                    AddOrUpdate(s);
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
        }

        public List<SignalFTPInfo> GetSignalFTPInfoForAllFTPSignals()
        {
            var signallist = (from r in GetLatestVersionOfAllSignals()
                join ftp in _db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
                where r.ControllerTypeID != 4
                select new SignalFTPInfo
                {
                    SignalID = r.SignalID,
                    PrimaryName = r.PrimaryName,
                    Secondary_Name = r.SecondaryName,
                    User_Name = ftp.UserName,
                    Password = ftp.Password,
                    FTP_Directory = ftp.FTPDirectory,
                    IP_Address = r.IPAddress,
                    SNMPPort = ftp.SNMPPort,
                    ActiveFTP = ftp.ActiveFTP,
                    ControllerType = r.ControllerTypeID
                }
            ).ToList();

            return signallist;
        }


        public SignalFTPInfo GetSignalFTPInfoByID(string signalId)
        {
            var signal = from r in GetLatestVersionOfAllSignals()
                join ftp in _db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
                where r.SignalID == signalId
                select new SignalFTPInfo
                {
                    SignalID = r.SignalID,
                    PrimaryName = r.PrimaryName,
                    Secondary_Name = r.SecondaryName,
                    User_Name = ftp.UserName,
                    Password = ftp.Password,
                    FTP_Directory = ftp.FTPDirectory,
                    IP_Address = r.IPAddress,
                    SNMPPort = ftp.SNMPPort,
                    ActiveFTP = ftp.ActiveFTP,
                    ControllerType = r.ControllerTypeID
                };
            return signal as SignalFTPInfo;
        }


        public Signal GetLatestVersionOfSignalBySignalID(string signalId)
        {
            var returnSignal = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal => signal.Areas)
                .Include(signal => signal.Jurisdiction)
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypes.Select(dt => dt.MetricTypes))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.VersionActionId != 3)
                .OrderByDescending(signal => signal.Start)
                .FirstOrDefault();
            if (returnSignal != null)
            {
                returnSignal.VersionList = GetAllVersionsOfSignalBySignalID(returnSignal.SignalID);
                AddSignalAndDetectorLists(returnSignal);
            }
            return returnSignal;
        }

        private static void AddSignalAndDetectorLists(Signal returnSignal)
        {
            var detectionTypesRepository = MOE.Common.Models.Repositories.DetectionTypeRepositoryFactory.Create();
            var detectionTypes = detectionTypesRepository.GetAllDetectionTypes();
            var hardwareTypesRepository =
                MOE.Common.Models.Repositories.DetectionHardwareRepositoryFactory.Create();
            var hardwareTypes = hardwareTypesRepository.GetAllDetectionHardwares();
            foreach (var approach in returnSignal.Approaches)
            {
                foreach (var detector in approach.Detectors)
                {
                    detector.AllDetectionTypes = detectionTypes;
                    detector.AllHardwareTypes = hardwareTypes;
                    detector.DetectionTypeIDs = new List<int>();
                    foreach (var detectionType in detector.DetectionTypes)
                    {
                        detector.DetectionTypeIDs.Add(detectionType.DetectionTypeID);
                    }
                }
            }
        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalId)
        {
            var signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypes.Select(dt => dt.MetricTypes))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.VersionActionId != 3)
                .OrderByDescending(signal => signal.Start)
                .ToList();

            if (signals.Count > 0)
                return signals;
            return null;
        }

        public List<Signal> GetLatestVersionOfAllSignals()
        {
            var activeSignals = _db.Signals.Where(r => r.VersionActionId != 3)
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal => signal.Areas)
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypes.Select(dt => dt.MetricTypes))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType)).ToList();

            activeSignals
                .GroupBy(r => r.SignalID)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault()).ToList();
            return activeSignals;

        }

        public IQueryable<Signal> GetLatestVersionOfAllSignalsAsQueryable()
        {
            var activeSignals = _db.Signals.Where(r => r.VersionActionId != 3)
                    .GroupBy(r => r.SignalID)
                    .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault());

            return activeSignals;

        }

        public List<Signal> GetLatestVersionOfAllSignalsForFtp()
        {
            List<int> controllerTypes = new List<int>{4,5};
            var activeSignals = _db.Signals.Where(r => r.VersionActionId != 3)
                .Include(s => s.ControllerType)
                //.Where(s => !controllerTypes.Contains(s.ControllerTypeID))
                .ToList();
            activeSignals = activeSignals
                .GroupBy(r => r.SignalID)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault()).ToList();
            activeSignals = activeSignals.Where(s => !controllerTypes.Contains(s.ControllerTypeID)).ToList();

            return activeSignals;
        }

        public int CheckVersionWithFirstDate(string signalId)
        {
            var signals = GetAllVersionsOfSignalBySignalID(signalId);

            var sigs = signals.Where(r => r.Start == r.FirstDate).ToList();

            switch (sigs.Count)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return 2;
            }
        }

        private void CopyApproaches(Signal signalFromDb, Signal newSignal)
        {
            var approaches = (from r in _db.Approaches
                where r.VersionID == signalFromDb.VersionID
                select r).ToList();

            foreach (var apprFromDb in approaches)
            {
                var newApp = new Approach();

                newApp.SignalID = newSignal.SignalID;
                newApp.Description = apprFromDb.Description;
                newApp.DirectionTypeID = apprFromDb.DirectionTypeID;
                newApp.ProtectedPhaseNumber = apprFromDb.ProtectedPhaseNumber;
                newApp.DirectionTypeID = apprFromDb.DirectionTypeID;
                newApp.IsProtectedPhaseOverlap = apprFromDb.IsProtectedPhaseOverlap;
                newApp.IsPermissivePhaseOverlap = apprFromDb.IsPermissivePhaseOverlap;
                newApp.MPH = apprFromDb.MPH;
                newApp.PermissivePhaseNumber = apprFromDb.PermissivePhaseNumber;
                newApp.VersionID = newSignal.VersionID;

                _db.Approaches.Add(newApp);
                _db.SaveChanges();

                CopyDetectors(apprFromDb, newApp);
            }
        }

        private void CopyDetectors(Approach apprFromDb, Approach newApp)
        {
            var detectorsFromDb = (from r in _db.Detectors
                where r.ApproachID == apprFromDb.ApproachID
                select r).ToList();

            foreach (var detFromDb in detectorsFromDb)
            {
                var newDetector = new Detector();

                newDetector.DecisionPoint = detFromDb.DecisionPoint;
                newDetector.LatencyCorrection = detFromDb.LatencyCorrection;
                newDetector.ApproachID = newApp.ApproachID;
                newDetector.DateAdded = DateTime.Today;
                newDetector.DetChannel = detFromDb.DetChannel;
                newDetector.DetectionHardwareID = detFromDb.DetectionHardwareID;
                newDetector.DetectorID = detFromDb.DetectorID;
                newDetector.LaneNumber = detFromDb.LaneNumber;
                if(detFromDb.DetectorCommentIDs != null)
                    newDetector.DetectorCommentIDs.AddRange(detFromDb.DetectorCommentIDs);
                newDetector.MovementTypeID = detFromDb.MovementTypeID;
                newDetector.MinSpeedFilter = detFromDb.MinSpeedFilter;
                newDetector.DistanceFromStopBar = detFromDb.DistanceFromStopBar;
                if(newDetector.DetectionTypes == null)
                    newDetector.DetectionTypes = new List<DetectionType>();
                newDetector.DetectionTypes.AddRange(detFromDb.DetectionTypes);
                _db.Detectors.Add(newDetector);
                _db.SaveChanges();
            }
        }


        public bool CheckReportAvialabilityForSignal(string signalId, int metricTypeId)
        {
            var signal = GetLatestVersionOfAllSignals().Find(s => s.SignalID == signalId);
            return signal.CheckReportAvailabilityForSignal(metricTypeId);
        }


        public void Update(Signal incomingSignal)
        {
            var signalFromDatabase = (from r in _db.Signals
                where r.VersionID == incomingSignal.VersionID
                select r).FirstOrDefault();
            if (signalFromDatabase != null)
            {
                foreach (var area in signalFromDatabase.Areas.ToList())
                {
                    signalFromDatabase.Areas.Remove(area);
                }
                if (incomingSignal.AreaIds != null && incomingSignal.AreaIds.Count > 0)
                {
                    foreach (var id in incomingSignal.AreaIds)
                    {
                        var area = _db.Areas.Where(a => a.Id == id).FirstOrDefault();
                        signalFromDatabase.Areas.Add(area);
                    }
                }
                if (incomingSignal.VersionActionId == 0)
                    incomingSignal.VersionActionId = signalFromDatabase.VersionActionId;
                _db.Entry(signalFromDatabase).CurrentValues.SetValues(incomingSignal);
                if (incomingSignal.Approaches != null)
                    foreach (var a in incomingSignal.Approaches)
                    {
                        var approach =
                            signalFromDatabase.Approaches.FirstOrDefault(app => app.ApproachID == a.ApproachID);
                        if (approach != null)
                        {
                            if (!a.Equals(approach))
                                _db.Entry(approach).CurrentValues.SetValues(a);
                        }
                        else
                        {
                            signalFromDatabase.Approaches.Add(a);
                        }
                        if (a.Detectors != null)
                            foreach (var newDetector in a.Detectors)
                            {
                                var detectorFromDatabase = signalFromDatabase.GetDetectorsForSignal()
                                    .FirstOrDefault(d => d.ID == newDetector.ID);
                                if (newDetector.DetectionTypes == null)
                                    newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
                                        newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                if (detectorFromDatabase != null)
                                {
                                    if (!newDetector.Equals(detectorFromDatabase))
                                    {
                                        if (detectorFromDatabase.DetectionTypes == null)
                                            detectorFromDatabase.DetectionTypes = new List<DetectionType>();
                                        var deletedDetectionTypes = detectorFromDatabase.DetectionTypes
                                            .Except(newDetector.DetectionTypes).ToList();
                                        var addedDetectionTypes = newDetector.DetectionTypes
                                            .Except(detectorFromDatabase.DetectionTypes).ToList();

                                        deletedDetectionTypes.ForEach(delDet =>
                                            detectorFromDatabase.DetectionTypes.Remove(delDet));
                                        foreach (var n in addedDetectionTypes)
                                        {
                                            if (_db.Entry(n).State == EntityState.Detached)
                                                _db.DetectionTypes.Attach(n);
                                            detectorFromDatabase.DetectionTypes.Add(n);
                                        }

                                        //var detectionTypes = _db.DetectionTypes.Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                        //graphDetector.DetectionTypes = detectionTypes;
                                        //graphDetector.DetectionTypeIDs = gd.DetectionTypeIDs;

                                        _db.Entry(detectorFromDatabase).CurrentValues.SetValues(newDetector);
                                    }
                                }
                                else
                                {
                                    if (newDetector.DetectionTypes == null)
                                        newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
                                            newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                    approach.Detectors.Add(newDetector);
                                }
                            }
                    }
            }
            else
            {
                foreach (var a in incomingSignal.Approaches)
                foreach (var gd in a.Detectors)
                    gd.DetectionTypes = _db.DetectionTypes
                        .Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                _db.Signals.Add(incomingSignal);
            }
            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        private VersionAction GetVersionActionByVersionAction_ID(int id)
        {
            var va = (from r in _db.VersionActions
                where r.ID == id
                select r).FirstOrDefault();

            return va;
        }
    }
}
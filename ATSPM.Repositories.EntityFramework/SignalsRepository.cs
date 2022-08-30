using ATSPM.IRepositories;
using ATSPM.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class SignalsRepository : ISignalsRepository
    {
        private readonly MOEContext _db;


        public SignalsRepository(MOEContext context)
        {
            _db = context;
        }


        public List<Signal> GetAllSignals()
        {
            return GetLatestVersionOfAllSignals();
        }

        public Signal GetVersionOfSignalByDate(string SignalId, DateTime startDate)
        {
            var signals = _db.Signals
                .Include(signal => signal.Approaches)//.Select(a => a.Detectors.Select(d => d.MovementType)))
                    .ThenInclude(a => a.Detectors)
                        .ThenInclude(d => d.MovementType)
                .Include(signal => signal.Approaches)//.Select(a => a.DirectionType))
                .ThenInclude(a => a.DirectionType)
                .Where(signal => signal.SignalId == SignalId)
                .Where(signal => signal.Start <= startDate)
                .Where(signal => signal.VersionActionId != 3).ToList();

            Signal signal;
            if (signals.Count > 1)
            {
                var orderedSignals = signals.OrderByDescending(signal => signal.Start);
                signal = orderedSignals.First();
            }
            else
            {
                signal =  signals.FirstOrDefault();
            }
            var detectors = signal.Approaches.SelectMany(s => s.Detectors);
            foreach(var detector in detectors)
            {
                detector.DetectionTypeDetectors = _db.DetectionTypeDetectors.Where(d => d.Id == detector.Id).Include(d => d.DetectionType).ToList();
            }
            return signal;
        }

        public Signal GetVersionOfSignalByDateWithDetectionTypes(string SignalId, DateTime startDate)
        {
            var signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d=> d.DetectionType).ToList())))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalId == SignalId)
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

        public Signal GetSignalVersionByVersionId(int VersionId)
        {
            var version = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType).ToList())))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .FirstOrDefault(signal => signal.VersionId == VersionId);
            if (version != null)
            {
                AddSignalAndDetectorLists(version);
            }
            return version;
        }

        public void SetVersionToDeleted(int VersionId)
        {
            var signal = (from r in _db.Signals where r.VersionId == VersionId select r).FirstOrDefault();
            if (signal != null)
                signal.VersionActionId = 3;
            _db.SaveChanges();
        }

        public void SetAllVersionsOfASignalToDeleted(string SignalId)
        {
            var signals = from r in _db.Signals
                          where r.SignalId == SignalId
                          select r;

            foreach (var s in signals)
                s.VersionActionId = 3;

            _db.SaveChanges();
        }

        public List<Signal> GetSignalsBetweenDates(string SignalId, DateTime startDate, DateTime endDate)
        {
            var signals = new List<Signal>();
            var signalBeforeStart = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalId == SignalId
                                 && signal.Start <= startDate
                                 && signal.VersionActionId != 3).OrderByDescending(s => s.Start)
                .Take(1)
                .FirstOrDefault();
            if (signalBeforeStart != null)
                signals.Add(signalBeforeStart);
            if (_db.Signals.Any(signal => signal.SignalId == SignalId
                                          && signal.Start > startDate
                                          && signal.Start < endDate
                                          && signal.VersionActionId != 3))
                signals.AddRange(_db.Signals
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.MovementType)))
                    .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                    .Where(signal => signal.SignalId == SignalId
                                     && signal.Start > startDate
                                     && signal.Start < endDate
                                     && signal.VersionActionId != 3).ToList());
            return signals;
        }

        public bool Exists(string SignalId)
        {
            return _db.Signals.Any(s => s.SignalId == SignalId);
        }

        public Signal CopySignalToNewVersion(Signal originalVersion)
        {
            var newVersion = new Signal();

            //originalVersion.VersionAction = (from r in _db.VersionActions
            //    where r.ID == 4
            //    select r).FirstOrDefault();

            newVersion.VersionAction = (from r in _db.VersionActions
                                        where r.Id == 4
                                        select r).FirstOrDefault();


            newVersion.SignalId = originalVersion.SignalId;
            newVersion.Start = DateTime.Today;
            newVersion.Note = "Copy of " + originalVersion.Note;
            newVersion.PrimaryName = originalVersion.PrimaryName;
            newVersion.SecondaryName = originalVersion.SecondaryName;
            newVersion.IPAddress = originalVersion.IPAddress;
            newVersion.ControllerTypeId = originalVersion.ControllerTypeId;
            newVersion.RegionId = originalVersion.RegionId;
            newVersion.Enabled = originalVersion.Enabled;
            newVersion.Pedsare1to1 = originalVersion.Pedsare1to1;
            newVersion.Latitude = originalVersion.Latitude;
            newVersion.Longitude = originalVersion.Longitude;
            _db.Signals.Add(newVersion);
            _db.SaveChanges();

            CopyApproaches(originalVersion, newVersion);

            return newVersion;
        }

        public List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId)
        {
            var signals = GetLatestVersionOfAllSignals();

            var filteredSignals = (from r in signals
                                   where r.ControllerTypeId == controllerTypeId
                                   select r).ToList();

            return filteredSignals;
        }

        public List<Signal> EagerLoadAllSignals()
        {
            var signals = _db.Signals.Where(r => r.VersionActionId != 3)
                .GroupBy(r => r.SignalId)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .ToList();

            return signals;
        }

        public List<Signal> GetAllEnabledSignals()
        {
            //_db.Configuration.LazyLoadingEnabled = false;

            var signals = GetLatestVersionOfAllSignals().ToList();
            return signals;
        }

        public string GetSignalLocation(string SignalId)
        {
            var signal = GetLatestVersionOfSignalBySignalId(SignalId);
            var location = string.Empty;
            if (signal != null)
                location = signal.PrimaryName + " @ " + signal.SecondaryName;

            return location;
        }

        public string GetSignalDescription(string SignalId)
        {
            var signal = (from r in _db.Signals
                          where r.SignalId == SignalId
                          select r).FirstOrDefault();
            var location = string.Empty;
            if (signal != null)
                location = signal.SignalDescription;

            return location;
        }



        public void AddOrUpdate(Signal signal)
        {
            var g = (from r in _db.Signals
                     where r.VersionId == signal.VersionId
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

                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent
                    {
                        ApplicationName = "MOE.Common",
                        Class = "Models.Repository.SignalRepository",
                        Function = "AddOrUpdate",
                        Description = ex.Message,
                        SeverityLevel = (int)ApplicationEvent.SeverityLevels.High,
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

            throw new NotImplementedException();
            //try
            //{
            //    var g = (from r in _db.Detectors
            //             where r.ID == detector.ID
            //             select r).FirstOrDefault();
            //    if (g == null)
            //    {
            //        detector.DetectionTypes = new List<DetectionType>();
            //        detector.DetectionTypes = _db.DetectionTypes
            //            .Where(dt => detector.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
            //    }
            //}

            //catch (Exception ex)
            //{

            //    var repository = new ApplicationEventRepository(_db);
            //    var error = new ApplicationEvent();
            //    error.ApplicationName = "MOE.Common";
            //    error.Class = "Models.Repository.SignalRepository";
            //    error.Function = "Add";
            //    error.Description = ex.Message;
            //    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
            //    error.Timestamp = DateTime.Now;
            //    repository.Add(error);
            //    throw;
            //}
        }

        public void AddList(List<Signal> signals)
        {
            foreach (var s in signals)
                try
                {
                    AddOrUpdate(s);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        //public List<SignalFTPInfo> GetSignalFTPInfoForAllFTPSignals()
        //{
        //    var signallist = (from r in GetLatestVersionOfAllSignals()
        //                      join ftp in _db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
        //                      where r.ControllerTypeID != 4
        //                      select new SignalFTPInfo
        //                      {
        //                          SignalId = r.SignalId,
        //                          PrimaryName = r.PrimaryName,
        //                          Secondary_Name = r.SecondaryName,
        //                          User_Name = ftp.UserName,
        //                          Password = ftp.Password,
        //                          FTP_Directory = ftp.FTPDirectory,
        //                          IP_Address = r.IPAddress,
        //                          SNMPPort = ftp.SNMPPort,
        //                          ActiveFTP = ftp.ActiveFTP,
        //                          ControllerType = r.ControllerTypeID
        //                      }
        //    ).ToList();

        //    return signallist;
        //}


        //public SignalFTPInfo GetSignalFTPInfoByID(string SignalId)
        //{
        //    var signal = from r in GetLatestVersionOfAllSignals()
        //                 join ftp in _db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
        //                 where r.SignalId == SignalId
        //                 select new SignalFTPInfo
        //                 {
        //                     SignalId = r.SignalId,
        //                     PrimaryName = r.PrimaryName,
        //                     Secondary_Name = r.SecondaryName,
        //                     User_Name = ftp.UserName,
        //                     Password = ftp.Password,
        //                     FTP_Directory = ftp.FTPDirectory,
        //                     IP_Address = r.IPAddress,
        //                     SNMPPort = ftp.SNMPPort,
        //                     ActiveFTP = ftp.ActiveFTP,
        //                     ControllerType = r.ControllerTypeID
        //                 };
        //    return signal as SignalFTPInfo;
        //}


        public Signal GetLatestVersionOfSignalBySignalId(string SignalId)
        {
            var returnSignal = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d=> d.DetectionType))))
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType.DetectionTypeMetricTypes.Select(m => m.MetricTypeMetric)))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalId == SignalId)
                .Where(signal => signal.VersionActionId != 3)
                .OrderByDescending(signal => signal.Start)
                .FirstOrDefault();
            if (returnSignal != null)
            {
                returnSignal.VersionList = GetAllVersionsOfSignalBySignalId(returnSignal.SignalId);
                AddSignalAndDetectorLists(returnSignal);
            }
            return returnSignal;
        }

        public void AddSignalAndDetectorLists(Signal returnSignal)
        {

            throw new NotImplementedException();
            //var detectionHardwareRepository = new DetectionHardwareRepository(_db);
            //var detectionTypeRepository = new DetectionTypeRepository(_db);
            //var detectionTypes = detectionTypeRepository.GetAllDetectionTypes();
            //var hardwareTypes = detectionHardwareRepository.GetAllDetectionHardwares();
            //foreach (var approach in returnSignal.Approaches)
            //{
            //    foreach (var detector in approach.Detectors)
            //    {
            //        detector.AllDetectionTypes = detectionTypes;
            //        detector.AllHardwareTypes = hardwareTypes;
            //        detector.DetectionTypeIDs = new List<int>();
            //        foreach (var detectionType in detector.DetectionTypes)
            //        {
            //            detector.DetectionTypeIDs.Add(detectionType.DetectionTypeID);
            //        }
            //    }
            //}
        }

        public List<Signal> GetAllVersionsOfSignalBySignalId(string SignalId)
        {
            var signals = _db.Signals
               .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType))))
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType.DetectionTypeMetricTypes.Select(m => m.MetricTypeMetric)))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalId == SignalId)
                .Where(signal => signal.VersionActionId != 3)
                .OrderByDescending(signal => signal.Start)
                .ToList();

            if (signals.Any())
                return signals;
            return null;
        }

        public List<Signal> GetLatestVersionOfAllSignals()
        {
            var activeSignals = _db.Signals.Where(r => r.VersionActionId != 3)
               .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType))))
                .Include(signal =>
                    signal.Approaches.Select(
                        a => a.Detectors.Select(d => d.DetectionTypeDetectors.Select(d => d.DetectionType.DetectionTypeMetricTypes.Select(m => m.MetricTypeMetric)))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .GroupBy(r => r.SignalId)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault()).ToList();
            return activeSignals;
        }

        public List<Signal> GetLatestVersionOfAllSignalsForFtp()
        {
            List<int> controllerTypes = new List<int> { 4, 5 };
            var activeSignals = _db.Signals.Where(r => r.VersionActionId != 3)
                .Include(s => s.ControllerType)
                //.Where(s => !controllerTypes.Contains(s.ControllerTypeID))
                .ToList();
            activeSignals = activeSignals
                .GroupBy(r => r.SignalId)
                .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault()).ToList();
            activeSignals = activeSignals.Where(s => !controllerTypes.Contains(s.ControllerTypeId)).ToList();

            return activeSignals;
        }

        public int CheckVersionWithFirstDate(string SignalId)
        {
            var signals = GetAllVersionsOfSignalBySignalId(SignalId);

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
                              where r.VersionId == signalFromDb.VersionId
                              select r).ToList();

            foreach (var apprFromDb in approaches)
            {
                var newApp = new Approach();

                newApp.SignalId = newSignal.SignalId;
                newApp.Description = apprFromDb.Description;
                newApp.DirectionTypeId = apprFromDb.DirectionTypeId;
                newApp.ProtectedPhaseNumber = apprFromDb.ProtectedPhaseNumber;
                newApp.DirectionTypeId = apprFromDb.DirectionTypeId;
                newApp.IsProtectedPhaseOverlap = apprFromDb.IsProtectedPhaseOverlap;
                newApp.IsPermissivePhaseOverlap = apprFromDb.IsPermissivePhaseOverlap;
                newApp.Mph = apprFromDb.Mph;
                newApp.PermissivePhaseNumber = apprFromDb.PermissivePhaseNumber;
                newApp.VersionId = newSignal.VersionId;

                _db.Approaches.Add(newApp);
                _db.SaveChanges();

                CopyDetectors(apprFromDb, newApp);
            }
        }

        private void CopyDetectors(Approach apprFromDb, Approach newApp)
        {

            throw new NotImplementedException();
            //var detectorsFromDb = (from r in _db.Detectors
            //                       where r.ApproachID == apprFromDb.ApproachID
            //                       select r).ToList();

            //foreach (var detFromDb in detectorsFromDb)
            //{
            //    var newDetector = new Detector();

            //    newDetector.DecisionPoint = detFromDb.DecisionPoint;
            //    newDetector.LatencyCorrection = detFromDb.LatencyCorrection;
            //    newDetector.ApproachID = newApp.ApproachID;
            //    newDetector.DateAdded = DateTime.Today;
            //    newDetector.DetChannel = detFromDb.DetChannel;
            //    newDetector.DetectionHardwareID = detFromDb.DetectionHardwareID;
            //    newDetector.DetectorID = detFromDb.DetectorID;
            //    newDetector.LaneNumber = detFromDb.LaneNumber;
            //    if (detFromDb.DetectorCommentIDs != null)
            //        newDetector.DetectorCommentIDs.AddRange(detFromDb.DetectorCommentIDs);
            //    newDetector.MovementTypeID = detFromDb.MovementTypeID;
            //    newDetector.MinSpeedFilter = detFromDb.MinSpeedFilter;
            //    newDetector.DistanceFromStopBar = detFromDb.DistanceFromStopBar;
            //    if (newDetector.DetectionTypes == null)
            //        newDetector.DetectionTypes = new List<DetectionType>();
            //    newDetector.DetectionTypes.ToList().AddRange(detFromDb.DetectionTypes);
            //    _db.Detectors.Add(newDetector);
            //    _db.SaveChanges();
            //}
        }


        //public bool CheckReportAvialabilityForSignal(string SignalId, int metricTypeId)
        //{
        //    var signal = GetLatestVersionOfAllSignals().Find(s => s.SignalId == SignalId);
        //    return signal.CheckReportAvailabilityForSignal(metricTypeId);
        //}


        public void Update(Signal incomingSignal)
        {

            throw new NotImplementedException();
            //var signalFromDatabase = (from r in _db.Signals
            //                           where r.VersionId == incomingSignal.VersionId
            //                           select r).FirstOrDefault();
            // if (signalFromDatabase != null)
            // {
            //     if (incomingSignal.VersionActionId == 0)
            //         incomingSignal.VersionActionId = signalFromDatabase.VersionActionId;
            //     _db.Entry(signalFromDatabase).CurrentValues.SetValues(incomingSignal);
            //     if (incomingSignal.Approaches != null)
            //         foreach (var a in incomingSignal.Approaches)
            //         {
            //             var approach =
            //                 signalFromDatabase.Approaches.FirstOrDefault(app => app.ApproachID == a.ApproachID);
            //             if (approach != null)
            //             {
            //                 if (!a.Equals(approach))
            //                     _db.Entry(approach).CurrentValues.SetValues(a);
            //             }
            //             else
            //             {
            //                 signalFromDatabase.Approaches.Add(a);
            //             }
            //             if (a.Detectors != null)
            //                 foreach (var newDetector in a.Detectors)
            //                 {
            //                     var detectorFromDatabase = signalFromDatabase.GetDetectorsForSignal()
            //                         .FirstOrDefault(d => d.ID == newDetector.ID);
            //                     if (newDetector.DetectionTypes == null)
            //                         newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
            //                             newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
            //                     if (detectorFromDatabase != null)
            //                     {
            //                         if (!newDetector.Equals(detectorFromDatabase))
            //                         {
            //                             if (detectorFromDatabase.DetectionTypes == null)
            //                                 detectorFromDatabase.DetectionTypes = new List<DetectionType>();
            //                             var deletedDetectionTypes = detectorFromDatabase.DetectionTypes
            //                                 .Except(newDetector.DetectionTypes).ToList();
            //                             var addedDetectionTypes = newDetector.DetectionTypes
            //                                 .Except(detectorFromDatabase.DetectionTypes).ToList();

            //                             deletedDetectionTypes.ForEach(delDet =>
            //                                 detectorFromDatabase.DetectionTypes.Remove(delDet));
            //                             foreach (var n in addedDetectionTypes)
            //                             {
            //                                 if (_db.Entry(n).State == EntityState.Detached)
            //                                     _db.DetectionTypes.Attach(n);
            //                                 detectorFromDatabase.DetectionTypes.Add(n);
            //                             }

            //                             //var detectionTypes = _db.DetectionTypes.Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
            //                             //graphDetector.DetectionTypes = detectionTypes;
            //                             //graphDetector.DetectionTypeIDs = gd.DetectionTypeIDs;

            //                             _db.Entry(detectorFromDatabase).CurrentValues.SetValues(newDetector);
            //                         }
            //                     }
            //                     else
            //                     {
            //                         if (newDetector.DetectionTypes == null)
            //                             newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
            //                                 newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
            //                         approach.Detectors.Add(newDetector);
            //                     }
            //                 }
            //         }
            // }
            // else
            // {
            //     foreach (var a in incomingSignal.Approaches)
            //         foreach (var gd in a.Detectors)
            //             gd.DetectionTypes = _db.DetectionTypes
            //                 .Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
            //     _db.Signals.Add(incomingSignal);
            // }
            // try
            // {
            //     _db.SaveChanges();
            // }
            // catch (Exception e)
            // {
            //     //foreach (var eve in e.EntityValidationErrors)
            //     //{
            //     //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //     //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //     //    foreach (var ve in eve.ValidationErrors)
            //     //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //     //            ve.PropertyName, ve.ErrorMessage);
            //     //}
            //     throw;
            // }
        }

        private VersionAction GetVersionActionByVersionAction_ID(int id)
        {
            var va = (from r in _db.VersionActions
                      where r.Id == id
                      select r).FirstOrDefault();

            return va;
        }

        public Signal GetLatestVersionOfSignalBySignalID(string signalID)
        {
            throw new NotImplementedException();
        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalID)
        {
            throw new NotImplementedException();
        }
    }
}
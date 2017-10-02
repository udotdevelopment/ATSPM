using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using NuGet;

namespace MOE.Common.Models.Repositories
{
    public class SignalsRepository : ISignalsRepository
    {
        private Models.SPM _db;
        public DateTime LastDate = Convert.ToDateTime("01/01/9999");

        public SignalsRepository()
        {
          _db  = new SPM();
        }

        public SignalsRepository(SPM context)
        {
            _db = context;
        }




        public List<Models.Signal> GetAllSignals()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in _db.Signals
                where r.End > DateTime.Now
                select r).ToList();
            return signals;
        }

        public Signal GetVersionOfSignalByDate(string signalId, DateTime startDate)
        {
            List<Models.Signal> signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.End > startDate)
                .ToList();

            var orderedSignals = signals.OrderBy(signal => signal.End);


            return orderedSignals.First();
        }

        public Signal GetSignalVersionByVersionId(int versionId)
        {
            throw new NotImplementedException();
        }

        public void SetVersionToDeleted(int versionId)
        {
            throw new NotImplementedException();
        }

        public void SetAllVersionsOfASignalToDeleted(string signalId)
        {
            var signals = from r in _db.Signals
                where r.SignalID == signalId
                select r;

            foreach (var s in signals)
            {
                s.VersionActionId = 3;
            }

            _db.SaveChanges();
        }

        public Signal CopySignalToNewVersion(Signal originalVersion)
        {


            Common.Models.Signal newVersion = new Signal();

            originalVersion.VersionAction = (from r in _db.VersionActions
                where r.ID == 4
                select r).FirstOrDefault();

            newVersion.VersionAction = (from r in _db.VersionActions
                where r.ID == 5
                select r).FirstOrDefault();

            

            newVersion.SignalID = originalVersion.SignalID;
            originalVersion.End = DateTime.Today;
            newVersion.End = LastDate;
            newVersion.Note = originalVersion.Note;
            newVersion.PrimaryName = originalVersion.PrimaryName;
            newVersion.SecondaryName = originalVersion.SecondaryName;
            newVersion.IPAddress = originalVersion.IPAddress;
            newVersion.ControllerTypeID = originalVersion.ControllerTypeID;
            newVersion.RegionID = originalVersion.RegionID;
            newVersion.Enabled = originalVersion.Enabled;
            newVersion.Latitude = originalVersion.Latitude;
            newVersion.Longitude = originalVersion.Longitude;

            _db.Signals.Add(newVersion);
            _db.SaveChanges();

            CopyApproaches(originalVersion, newVersion);

            return newVersion;

        }

        private void CopyApproaches(Signal signalFromDb, Signal newSignal)
        {
            List<Approach> approaches = (from r in _db.Approaches
                where r.VersionID == signalFromDb.VersionID
                select r).ToList();

            foreach (var apprFromDb in approaches)
            {
                Approach newApp = new Approach();

                newApp.SignalID = newSignal.SignalID;
                newApp.Description = apprFromDb.Description;
                newApp.DirectionTypeID = apprFromDb.DirectionTypeID;
                newApp.ProtectedPhaseNumber = apprFromDb.ProtectedPhaseNumber;
                newApp.DirectionTypeID = apprFromDb.DirectionTypeID;
                newApp.IsProtectedPhaseOverlap = apprFromDb.IsProtectedPhaseOverlap;
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
                newDetector.ApproachID = newApp.ApproachID;
                newDetector.DateAdded = DateTime.Today;
                newDetector.DetChannel = detFromDb.DetChannel;
                newDetector.DetectionHardwareID = detFromDb.DetectionHardwareID;
                newDetector.DetectorID = detFromDb.DetectorID;
                newDetector.LaneNumber = detFromDb.LaneNumber;
                newDetector.DetectorCommentIDs.AddRange(detFromDb.DetectorCommentIDs);
                newDetector.MovementTypeID = detFromDb.MovementTypeID;
                newDetector.MinSpeedFilter = detFromDb.MinSpeedFilter;
                newDetector.DistanceFromStopBar = detFromDb.DistanceFromStopBar;

                _db.Detectors.Add(newDetector);
                _db.SaveChanges();
            }
        }

        public List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId)
        {
            var signals = GetLatestVerionOfAllSignals();

            var filteredSignals = (from r in signals where r.ControllerTypeID == controllerTypeId
                                   select r).ToList();

            return filteredSignals;
        }
        public List<Models.Signal> EagerLoadAllSignals()
        {

            List<Models.Signal> signals = _db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))

                .Where(signal => signal.End > DateTime.Now)
                .ToList();
            return signals;
        }

        public List<Models.Signal> GetAllEnabledSignals()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in _db.Signals
                    where r.Enabled && r.End > DateTime.Now
                                           select r)

                .Where(signal => signal.End > DateTime.Now)
                .ToList();
            return signals;
        }

        public string GetSignalLocation(string SignalID)
        {
            Models.Signal signal = (from r in _db.Signals
                where r.SignalID == SignalID
                select r).FirstOrDefault();
            string location = string.Empty;
            if (signal != null)
            {
                location = signal.PrimaryName + " @ " + signal.SecondaryName;
            }

            return location;
        }

        public List<Models.Signal> GetAllWithGraphDetectors()
        {
            List<Models.Signal> signals = (from s in _db.Signals
                where s.End > DateTime.Today &&
                s.GetDetectorsForSignal().Count > 0
                select s).ToList();
            return signals;
        }

        public bool CheckReportAvialabilityForSignal(string signalID, int metricTypeID)
        {
            var signal = _db.Signals.Find(signalID);
            return signal.CheckReportAvailabilityForSignal(metricTypeID);
        }

        public Models.Signal GetSignalBySignalID(string signalId)
        {
            return _db.Signals.Where(s => s.SignalID == signalId).FirstOrDefault(s => s.End > DateTime.Today);
        }

        public bool DoesSignalHaveDetection(string signalId)
        {
            var signal = _db.Signals.Where(s => s.SignalID == signalId).FirstOrDefault(s => s.End > DateTime.Today);
            if (signal != null && signal.GetDetectorsForSignal().Count > 0)
            {
                return true;
            }
            return false;
        }

        public void Update(MOE.Common.Models.Signal incomingSignal)
        {
            MOE.Common.Models.Signal signalFromDatabase = (from r in _db.Signals
                where r.SignalID == incomingSignal.SignalID
                select r).FirstOrDefault();
            if (signalFromDatabase != null)
            {
                _db.Entry(signalFromDatabase).CurrentValues.SetValues(incomingSignal);
                if (incomingSignal.Approaches != null)
                {
                    foreach (MOE.Common.Models.Approach a in incomingSignal.Approaches)
                    {
                        var approach =
                            signalFromDatabase.Approaches.FirstOrDefault(app => app.ApproachID == a.ApproachID);
                        if (approach != null)
                        {
                            if (!a.Equals(approach))
                            {
                                _db.Entry(approach).CurrentValues.SetValues(a);
                            }
                        }
                        else
                        {
                            signalFromDatabase.Approaches.Add(a);
                        }
                        if (a.Detectors != null)
                        {
                            foreach (MOE.Common.Models.Detector newDetector in a.Detectors)
                            {
                                var detectorFromDatabase = signalFromDatabase.GetDetectorsForSignal()
                                    .FirstOrDefault(d => d.ID == newDetector.ID);
                                if (newDetector.DetectionTypes == null)
                                {
                                    newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
                                        newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                }
                                if (detectorFromDatabase != null)
                                {
                                    if (!newDetector.Equals(detectorFromDatabase))
                                    {
                                        if (detectorFromDatabase.DetectionTypes == null)
                                        {
                                            detectorFromDatabase.DetectionTypes = new List<DetectionType>();
                                        }
                                        var deletedDetectionTypes = detectorFromDatabase.DetectionTypes
                                            .Except(newDetector.DetectionTypes).ToList<DetectionType>();
                                        var addedDetectionTypes = newDetector.DetectionTypes
                                            .Except(detectorFromDatabase.DetectionTypes).ToList<DetectionType>();

                                        deletedDetectionTypes.ForEach(delDet =>
                                            detectorFromDatabase.DetectionTypes.Remove(delDet));
                                        foreach (DetectionType n in addedDetectionTypes)
                                        {
                                            if (_db.Entry(n).State == EntityState.Detached)
                                            {
                                                _db.DetectionTypes.Attach(n);
                                            }
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
                                    {
                                        newDetector.DetectionTypes = _db.DetectionTypes.Where(x =>
                                            newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                    }
                                    approach.Detectors.Add(newDetector);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                foreach (Models.Approach a in incomingSignal.Approaches)
                {
                    foreach (Models.Detector gd in a.Detectors)
                    {
                        gd.DetectionTypes = _db.DetectionTypes
                            .Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                    }
                }
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
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public List<MOE.Common.Business.Pin> GetPinInfo()
        {
            List<MOE.Common.Business.Pin> pins = new List<Business.Pin>();
            foreach (var signal in _db.Signals.Where(s => s.Enabled == true).Where(s => s.End > DateTime.Today).ToList())
            {
                MOE.Common.Business.Pin pin = new MOE.Common.Business.Pin(signal.SignalID, signal.Latitude,
                    signal.Longitude,
                    signal.PrimaryName + " " + signal.SecondaryName, signal.RegionID.ToString());
                pin.MetricTypes = signal.GetMetricTypesString();
                pins.Add(pin);
                Console.WriteLine(pin.SignalID);
            }
            return (pins);
        }

        public void AddOrUpdate(MOE.Common.Models.Signal signal)
        {
            MOE.Common.Models.Signal g = (from r in _db.Signals
                where r.SignalID == signal.SignalID
                select r).FirstOrDefault();
            if (g == null)
            {
                _db.Signals.Add(signal);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent
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

        public void AddList(List<MOE.Common.Models.Signal> signals)
        {
            foreach (var s in signals)
            {


                try
                {
                    AddOrUpdate(s);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    throw ex;
                }
            }

        }

        public void Remove(string id)
        {
            //MOE.Common.Models.Signal g = _db.Signals.Find(id);
            //if (g != null)
            //{
            //    _db.Signals.Remove(g);
            //    _db.SaveChanges();
            //}
        }

        public void Remove(MOE.Common.Models.Signal signal)
        {
            //MOE.Common.Models.Signal g = (from r in _db.Signals
            //    where r.SignalID == originalVersion.SignalID
            //    select r).FirstOrDefault();
            //if (g != null)
            //{
            //    _db.Signals.Remove(g);
            //    _db.SaveChanges();
            //}
        }

        public SignalFTPInfo GetSignalFTPInfoByID(string signalId)
        {
            var signal = (from r in _db.Signals
                join ftp in _db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
                where r.SignalID == signalId && r.End > DateTime.Today
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
            );
            return signal as SignalFTPInfo;
        }


        public Common.Models.Signal GetLatestVersionOfSignalBySignalID(string signalID)
        {
            var signal = (from r in _db.Signals
                    where r.SignalID == signalID
                    select r)
                .Include(r => r.Approaches.Select(a => a.Detectors))
                .Include(r => r.Approaches.Select(a => a.DirectionType))
                .OrderByDescending(x => x.End)
                .Take(1).FirstOrDefault();



            return signal;
        }

        private VersionAction GetVersionActionByVersionActionId(int id)
        {
            VersionAction va = (from r in _db.VersionActions
                where r.ID == id
                select r).FirstOrDefault();

            return va;
        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalID)
        {
            var signals = (from r in _db.Signals
                    where r.SignalID == signalID
                    select r)
                .Include(r => r.Approaches.Select(a => a.Detectors))
                .Include(r => r.Approaches.Select(a => a.DirectionType))
                .OrderByDescending(x => x.End)
                .ToList();

            if (signals.Count < 0)
            {

                return signals;
            }
            return null;
        }

        public List<Signal> GetLatestVerionOfAllSignals()
        {
            var signals = (from r in _db.Signals
                where r.End == LastDate
                select r).ToList();

            return signals;
        }

        public int CheckVersionWithLastDate(string signalId)
        {
            var signals = GetAllVersionsOfSignalBySignalID(signalId);

            var sigs = signals.Where(r => r.End == LastDate).ToList();

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
    }
}
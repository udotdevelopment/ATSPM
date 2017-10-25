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
        Models.SPM db = new SPM();

        public DateTime LastDate = Convert.ToDateTime("01/01/9999");

        public List<Models.Signal> GetAllSignals()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in db.Signals
                where r.End > DateTime.Now
                select r).ToList();
            return signals;
        }

        public Signal GetVersionOfSignalByDate(string signalId, DateTime startDate)
        {
            List<Models.Signal> signals = db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .Where(signal => signal.SignalID == signalId)
                .Where(signal => signal.End > startDate)
                .ToList();

            var orderedSignals = signals.OrderBy(signal => signal.End);


            return orderedSignals.First();
        }

        public Signal CopySignalVersion(Signal signal)
        {
            Common.Models.Signal ns = new Signal();

            ns.SignalID = signal.SignalID;
            signal.End = DateTime.Today;
            ns.End = LastDate;
            ns.Note = signal.Note;
            ns.PrimaryName = signal.PrimaryName;
            ns.SecondaryName = signal.SecondaryName;
            ns.IPAddress = signal.IPAddress;
            ns.ControllerTypeID = signal.ControllerTypeID;
            ns.RegionID = signal.RegionID;
            ns.Enabled = signal.Enabled;
            ns.Latitude = signal.Latitude;
            ns.Longitude = signal.Longitude;

            db.Signals.Add(ns);
            db.SaveChanges();

            CopyApproaches(signal, ns);



            return ns;

            


        }

        private void CopyApproaches(Signal signalFromDB, Signal newSignal)
        {
            List<Approach> approaches = (from r in db.Approaches
                where r.VersionID == signalFromDB.VersionID
                select r).ToList();

            foreach (var appr_fromDB in approaches)
            {
                Approach newApp = new Approach();

                newApp.SignalID = newSignal.SignalID;
                newApp.Description = appr_fromDB.Description;
                newApp.DirectionTypeID = appr_fromDB.DirectionTypeID;
                newApp.ProtectedPhaseNumber = appr_fromDB.ProtectedPhaseNumber;
                newApp.DirectionTypeID = appr_fromDB.DirectionTypeID;
                newApp.IsProtectedPhaseOverlap = appr_fromDB.IsProtectedPhaseOverlap;
                newApp.MPH = appr_fromDB.MPH;
                newApp.PermissivePhaseNumber = appr_fromDB.PermissivePhaseNumber;
                newApp.VersionID = newSignal.VersionID;

                db.Approaches.Add(newApp);
                db.SaveChanges();

                CopyDetectors(appr_fromDB, newApp);
            }

           


        }

        private void CopyDetectors(Approach apprFromDb, Approach newApp)
        {
            var detectorsFromDb = (from r in db.Detectors
                where r.ApproachID == apprFromDb.ApproachID
                select r).ToList();
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

            List<Models.Signal> signals = db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))

                .Where(signal => signal.End > DateTime.Now)
                .ToList();
            return signals;
        }

        public List<Models.Signal> GetAllEnabledSignals()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in db.Signals
                    where r.Enabled && r.End > DateTime.Now
                                           select r)

                .Where(signal => signal.End > DateTime.Now)
                .ToList();
            return signals;
        }

        public string GetSignalLocation(string SignalID)
        {
            Models.Signal signal = (from r in db.Signals
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
            List<Models.Signal> signals = (from s in db.Signals
                where s.End > DateTime.Today &&
                s.GetDetectorsForSignal().Count > 0
                select s).ToList();
            return signals;
        }

        public bool CheckReportAvialabilityForSignal(string signalID, int metricTypeID)
        {
            var signal = db.Signals.Find(signalID);
            return signal.CheckReportAvailabilityForSignal(metricTypeID);
        }

        public Models.Signal GetSignalBySignalID(string signalId)
        {
            return db.Signals.Where(s => s.SignalID == signalId).FirstOrDefault(s => s.End > DateTime.Today);
        }

        public bool DoesSignalHaveDetection(string signalId)
        {
            var signal = db.Signals.Where(s => s.SignalID == signalId).FirstOrDefault(s => s.End > DateTime.Today);
            if (signal != null && signal.GetDetectorsForSignal().Count > 0)
            {
                return true;
            }
            return false;
        }

        public void Update(MOE.Common.Models.Signal incomingSignal)
        {
            MOE.Common.Models.Signal signalFromDatabase = (from r in db.Signals
                where r.SignalID == incomingSignal.SignalID
                select r).FirstOrDefault();
            if (signalFromDatabase != null)
            {
                db.Entry(signalFromDatabase).CurrentValues.SetValues(incomingSignal);
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
                                db.Entry(approach).CurrentValues.SetValues(a);
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
                                    newDetector.DetectionTypes = db.DetectionTypes.Where(x =>
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
                                            if (db.Entry(n).State == EntityState.Detached)
                                            {
                                                db.DetectionTypes.Attach(n);
                                            }
                                            detectorFromDatabase.DetectionTypes.Add(n);
                                        }

                                        //var detectionTypes = db.DetectionTypes.Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                        //graphDetector.DetectionTypes = detectionTypes;
                                        //graphDetector.DetectionTypeIDs = gd.DetectionTypeIDs;

                                        db.Entry(detectorFromDatabase).CurrentValues.SetValues(newDetector);
                                    }
                                }
                                else
                                {
                                    if (newDetector.DetectionTypes == null)
                                    {
                                        newDetector.DetectionTypes = db.DetectionTypes.Where(x =>
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
                        gd.DetectionTypes = db.DetectionTypes
                            .Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                    }
                }
                db.Signals.Add(incomingSignal);
            }
            try
            {
                db.SaveChanges();
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
            foreach (var signal in db.Signals.Where(s => s.Enabled == true).Where(s => s.End > DateTime.Today).ToList())
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
            MOE.Common.Models.Signal g = (from r in db.Signals
                where r.SignalID == signal.SignalID
                select r).FirstOrDefault();
            if (g == null)
            {
                db.Signals.Add(signal);
                try
                {
                    db.SaveChanges();
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
            //MOE.Common.Models.Signal g = db.Signals.Find(id);
            //if (g != null)
            //{
            //    db.Signals.Remove(g);
            //    db.SaveChanges();
            //}
        }

        public void Remove(MOE.Common.Models.Signal signal)
        {
            //MOE.Common.Models.Signal g = (from r in db.Signals
            //    where r.SignalID == signal.SignalID
            //    select r).FirstOrDefault();
            //if (g != null)
            //{
            //    db.Signals.Remove(g);
            //    db.SaveChanges();
            //}
        }

        public SignalFTPInfo GetSignalFTPInfoByID(string signalId)
        {
            var signal = (from r in db.Signals
                join ftp in db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
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

        public void UpdateWithNewVersion(Signal incomingSignal)
        {
            var oldSignalConfig = GetLatestVersionOfSignalBySignalID(incomingSignal.SignalID);
            oldSignalConfig.End = DateTime.Now;
            oldSignalConfig.VersionAction = GetVersionActionByVersionActionId(4);




            try
            {
                db.SaveChanges();
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

            AddOrUpdate(incomingSignal);
        }

        public Common.Models.Signal GetLatestVersionOfSignalBySignalID(string signalID)
        {
            var signal = (from r in db.Signals
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
            VersionAction va = (from r in db.VersionActions
                where r.ID == id
                select r).FirstOrDefault();

            return va;
        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalID)
        {
            var signals = (from r in db.Signals
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
            var signals = (from r in db.Signals
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
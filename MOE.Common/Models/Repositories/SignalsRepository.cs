using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace MOE.Common.Models.Repositories
{
    public class SignalsRepository : ISignalsRepository
    {
        Models.SPM db = new SPM();

        public List<Models.Signal> GetAllSignals()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in db.Signals
                                           select r).ToList();
            return signals;
        }

        public List<Models.Signal> EagerLoadAllSignals()
        {

            List<Models.Signal> signals = db.Signals
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))               
                .ToList();
            return signals;
        }

        public List<Models.Signal> GetAllEnabledSignals()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Signal> signals = (from r in db.Signals
                                           where r.Enabled == true
                                           select r).ToList();
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
                                          where s.GetDetectorsForSignal().Count > 0
                                          select s).ToList();
            return signals;
        }

        public  bool CheckReportAvialabilityForSignal(string signalID, int metricTypeID)
        {
            var signal = db.Signals.Find(signalID);
            return signal.CheckReportAvailabilityForSignal(metricTypeID);
        }

        public Models.Signal GetSignalBySignalID(string signalID)
        {            
            return db.Signals.Find(signalID);
        }

        public bool DoesSignalHaveDetection(string signalID)
        {
            var signal = db.Signals.Find(signalID);
            if (signal.GetDetectorsForSignal().Count > 0)
            {
                return true;
            }
            else { return false; }
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
                        var approach = signalFromDatabase.Approaches.Where(app => app.ApproachID == a.ApproachID).FirstOrDefault();
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
                                var detectorFromDatabase = signalFromDatabase.GetDetectorsForSignal().Where(d => d.ID == newDetector.ID).FirstOrDefault();
                                if (newDetector.DetectionTypes == null)
                                {
                                    newDetector.DetectionTypes = db.DetectionTypes.Where(x => newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
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

                                        deletedDetectionTypes.ForEach(delDet => detectorFromDatabase.DetectionTypes.Remove(delDet));
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
                                        newDetector.DetectionTypes = db.DetectionTypes.Where(x => newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
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
                        gd.DetectionTypes = db.DetectionTypes.Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
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
            foreach(var signal in db.Signals.Where(s => s.Enabled == true).ToList())
            {
                MOE.Common.Business.Pin pin = new MOE.Common.Business.Pin(signal.SignalID, signal.Latitude, signal.Longitude,
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
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.SignalRepository";
                    error.Function = "AddOrUpdate";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
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
            db.Signals.AddRange(signals);
            try
            {
                db.SaveChanges();
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                throw ex;
            }

        }
        public void Remove(string id)
        {
            MOE.Common.Models.Signal g = db.Signals.Find(id);
            if (g != null)
            {
                db.Signals.Remove(g);
                db.SaveChanges();
            }
        }

        public void Remove(MOE.Common.Models.Signal signal)
        {
            MOE.Common.Models.Signal g = (from r in db.Signals
                                          where r.SignalID == signal.SignalID
                                          select r).FirstOrDefault();
            if (g != null)
            {
                db.Signals.Remove(g);
                db.SaveChanges();
            }
        }

        public SignalFTPInfo GetSignalFTPInfoByID(string signalID)
        {
            var signal = (from r in db.Signals
                          join ftp in db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
                          where r.SignalID == signalID
                          select new SignalFTPInfo
                          { SignalID = r.SignalID,
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
    }    
}

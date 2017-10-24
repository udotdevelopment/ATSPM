using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Signal = MOE.Common.Models.Signal;

namespace MOE.CommonTests.Models
{
    public class InMemorySignalsRepository : ISignalsRepository
    {
        public DateTime LastDate = Convert.ToDateTime("01/01/9999");

        private InMemoryMOEDatabase _moeDB = new InMemoryMOEDatabase();

 

        public void AddList(List<Common.Models.Signal> signals)
        {
            foreach (var s in signals)
            {
                _moeDB.Signals.Add(s);
            }
        }

        public void AddOrUpdate(Common.Models.Signal signal)
        {
            MOE.Common.Models.Signal g = (from r in _moeDB.Signals
                                          where r.SignalID == signal.SignalID 
                                          select r).FirstOrDefault();
            if (g == null)
            {
               

               _moeDB.Signals.Add(signal);
                
            }
            else
            {
                Update(signal);
               
            }
        }

        

        public void UpdateWithNewVersion(Common.Models.Signal incomingSignal)
        {



        }

        public List<Signal> GetAllVersionsOfSignalBySignalID(string signalID)
        {
            var signals = (from r in _moeDB.Signals
                           where r.SignalID == signalID
                           select r).ToList();

            signals.OrderBy(d => d.End);

            return signals;
        }

        private VersionAction GetVersionActionByVersionActionId(int Id)
        {
            VersionAction va = (from r in _moeDB.VersionActions
                               where r.ID == Id
                               select r).FirstOrDefault();

            return va;
        }

        public void Update(MOE.Common.Models.Signal incomingSignal)
        {
            MOE.Common.Models.Signal signalFromDatabase = (from r in _moeDB.Signals
                                                           where r.SignalID == incomingSignal.SignalID
                                                           select r).FirstOrDefault();
            if (signalFromDatabase != null)
            {
                signalFromDatabase.VersionAction = (from r in _moeDB.VersionActions
                                                    where r.ID == 3
                                                            select r).FirstOrDefault();



                if (incomingSignal.Approaches != null)
                {
                    foreach (MOE.Common.Models.Approach a in incomingSignal.Approaches)
                    {
                        var approach = signalFromDatabase.Approaches.FirstOrDefault(app => app.ApproachID == a.ApproachID);
                        if (approach != null)
                        {
                            if (!a.Equals(approach))
                            {
                                signalFromDatabase.Approaches.Remove(approach);
                                signalFromDatabase.Approaches.Add(a);
                                
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
                                var detectorFromDatabase = _moeDB.Detectors.Where(d => d.ID == newDetector.ID).FirstOrDefault();
                                if (newDetector.DetectionTypes == null)
                                {
                                    newDetector.DetectionTypes = _moeDB.DetectionTypes.Where(x => newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
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

                                            detectorFromDatabase.DetectionTypes.Add(n);
                                        }

                                        a.Detectors.Remove(newDetector);
                                        a.Detectors.Add(detectorFromDatabase);
                                        
                                    }
                                }
                                else
                                {
                                    if (newDetector.DetectionTypes == null)
                                    {
                                        newDetector.DetectionTypes = _moeDB.DetectionTypes.Where(x => newDetector.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                                    }
                                    if (approach != null) approach.Detectors.Add(newDetector);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                foreach (Common.Models.Approach a in incomingSignal.Approaches)
                {
                    foreach (Common.Models.Detector gd in a.Detectors)
                    {
                        gd.DetectionTypes = _moeDB.DetectionTypes.Where(x => gd.DetectionTypeIDs.Contains(x.DetectionTypeID)).ToList();
                    }
                }
                _moeDB.Signals.Add(incomingSignal);
            }
        }

        public List<Common.Models.Signal> EagerLoadAllSignals()
        {
            return _moeDB.Signals;
        }

        public List<Common.Models.Signal> GetAllEnabledSignals()
        {
            return _moeDB.Signals;
        }

        public List<Common.Models.Signal> GetAllSignals()
        {
            return _moeDB.Signals;
        }

        public List<Common.Models.Signal> GetAllWithGraphDetectors()
        {
            List<Common.Models.Signal> sigsWithDetectors = new List<Common.Models.Signal>();

            foreach (var s in _moeDB.Signals)
            {
                List<Common.Models.Detector> dets = s.GetDetectorsForSignal();

                if (dets.Count > 0)
                {
                    sigsWithDetectors.Add(s);
                }

            }

            return sigsWithDetectors;
        }

        public List<Pin> GetPinInfo()
        {
            throw new NotImplementedException();
        }

        public Common.Models.Signal GetSignalBySignalID(string signalID)
        {
            var signal = GetLatestVersionOfSignalBySignalID(signalID);

            return signal;
        }

        public Common.Models.Signal GetLatestVersionOfSignalBySignalID(string signalID)
        {
            var signal = (from r in _moeDB.Signals
                          where r.SignalID == signalID
                          select r).OrderByDescending(x => x.End).Take(1).FirstOrDefault();



            return signal;
        }



        public SignalFTPInfo GetSignalFTPInfoByID(string signalID)
        {
            throw new NotImplementedException();
        }

        public string GetSignalLocation(string signalID)
        {
            var signal = GetSignalBySignalID(signalID);

            string location = string.Empty;
            if (signal != null)
            {
                location = signal.PrimaryName + " @ " + signal.SecondaryName;
            }

            return location;
        }

        public void Remove(Common.Models.Signal signal)
        {
            _moeDB.Signals.Remove(signal);
        }

        public void Remove(string id)
        {
            var signal = GetSignalBySignalID(id);

            if(signal != null)
            {
                _moeDB.Signals.Remove(signal);

            }
        }

        public List<Signal> GetLatestVerionOfAllSignals()
        {
            var signals = (from r in _moeDB.Signals
                          where r.End ==  LastDate
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

        public List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId)
        {
            throw new NotImplementedException();
        }

        public Signal GetVersionOfSignalByDate(string signalId, DateTime startDate)
        {
            throw new NotImplementedException();
        }

        public List<Signal> EagerLoadAllEnabledSignals()
        {
            throw new NotImplementedException();
        }
    }
}

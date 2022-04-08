//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.Entity;

//namespace MOE.Common.Models.Repositories
//{
//    public class SignalsRepositoryTest : ISignalsRepository
//    {
//        Models.MOEContext db = new MOEContext();
//        List<Models.MetricType> metrics = new List<MetricType>();

//        public void AddListAndSaveToDatabase(List<MOE.Common.Models.Signal> signals)
//        {

//        }

//        public List<Models.Signal> GetAllSignals()
//        {
//            List<Models.Signal> signals = new List<Signal>();
//            signals.Add(new Signal { SignalId = "1", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "2", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "3", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "4", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "5", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "6", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "7", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "8", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "9", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "10", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "11", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "12", PrimaryName = "Test", SecondaryName = "Test" });

//            return signals;
//        }

//        public List<Models.Signal> GetAllEnabledSignals()
//        {
//            List<Models.Signal> signals = new List<Signal>();
//            signals.Add(new Signal { SignalId = "1", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "2", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "3", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "4", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "5", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "6", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "7", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "8", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "9", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "10", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "11", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "12", PrimaryName = "Test", SecondaryName = "Test" });

//            return signals;
//        }

//        public List<Models.Signal> EagerLoadAllSignals()
//        {
//            List<Models.Signal> signals = new List<Signal>();
//            signals.Add(new Signal { SignalId = "1", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "2", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "3", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "4", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "5", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "6", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "7", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "8", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "9", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "10", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "11", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "12", PrimaryName = "Test", SecondaryName = "Test" });

//            return signals;
//        }

//        public List<Models.Signal> GetAllSignalsWithChildrenObjects()
//        {
//            List<Models.Signal> signals = new List<Signal>();
//            signals.Add(new Signal { SignalId = "1", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "2", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "3", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "4", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "5", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "6", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "7", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "8", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "9", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "10", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "11", PrimaryName = "Test", SecondaryName = "Test" });
//            signals.Add(new Signal { SignalId = "12", PrimaryName = "Test", SecondaryName = "Test" });

//            return signals;
//        }

//        public string GetSignalLocation(string SignalId)
//        {
//            Models.Signal signal = (from r in db.Signals
//                                     where r.SignalId == SignalId
//                                     select r).FirstOrDefault();
//            string location = string.Empty;
//            if (signal != null)
//            {
//                location = signal.PrimaryName + " " + signal.SecondaryName;
//            }

//            return location;
//        }

//        public List<Models.Signal> GetAllWithGraphDetectors()
//        {
//            List<Models.Signal> signals = (from s in db.Signals
//                                          where s.GetDetectorsForSignal().Count > 0
//                                          select s).ToList();
//            return signals;
//        }

//        public  bool CheckReportAvialabilityForSignal(string signalID, int metricTypeID)
//        {
//            MOE.Common.Models.Repositories.ISignalsRepository repository =
//                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
//            var signal = repository.GetSignalBySignalID(signalID);
//            return signal.CheckReportAvailabilityForSignal(metricTypeID);
//        }

//        public Models.Signal GetSignalBySignalID(string signalID)
//        {
//            //var signal = db.Signals
//            //    .Include(s =>
//            //        s.Directions
//            //            .Select(dir =>
//            //                dir.RouteSignals
//            //                .Select(a => a.LaneGroups
//            //                .Select(lg2 => lg2.Lanes
//            //                    .Select(l => l.Detectors.
//            //                        Select(d => d.DetectionType.MetricTypes)))))).Where(s => s.SignalId == signalID);
//            var signal = (from r in db.Signals
//                          where r.SignalId == signalID
//                          select r);

//            return signal.FirstOrDefault();
//        }

//        public void AddOrUpdate(MOE.Common.Models.Signal signal)
//    {


//        MOE.Common.Models.Signal g = (from r in db.Signals
//                                      where r.SignalId == signal.SignalId
//                                    select r).FirstOrDefault();
//            if (g != null)
//            {
//                db.Entry(g).CurrentValues.SetValues(signal);
//                db.SaveChanges();
//            }
//            else
//            {
//                db.Signals.Add(signal);
//                db.SaveChanges();

//            }


//    }
//        public List<MOE.Common.Business.Pin> GetPinInfo()
//        {
//            metrics = db.MetricTypes.Select(s => s).ToList();

//            //var sigs = (from s in db.Signals
//            //            join gd in db.Detectors on s.SignalId equals gd.SignalId
//            //            join dd in db.DetectionTypeDetectors on gd.DetectorID equals dd.Detectors_DetectorID
//            //            join dt in db.DetectionTypes on dd.DetectionType_DetectionTypeID equals dt.DetectionTypeID
//            //            select new { s.SignalId, s.PrimaryName, s.Secondary_Name, s.Latitude, s.Longitude, s.Region, dt.DetectionTypeID }).ToList();

//            List<MOE.Common.Models.Custom.SignalWithDetection> sigs = db.SignalsWithDetection.SqlQuery(@"select s.SignalId, s.PrimaryName, s.Secondary_Name, s.Latitude, s.Longitude, s.Region, dt.DetectionTypeID
//from Signals s
//join Detector gd on s.SignalId = gd.SignalId
//join DetectionTypeDetectors dd on gd.ID = dd.Detectors_DetectorID
//join DetectionTypes dt on dd.DetectionType_DetectionTypeID = dd.DetectionType_DetectionTypeID").ToList();


//            var distinctsignalinfo = (from s in sigs
//                                      select new MOE.Common.Business.Pin(s.SignalId, s.Latitude, s.Longitude, s.PrimaryName + " " + s.Secondary_Name, s.Region)).Distinct().ToList();


//            foreach (MOE.Common.Business.Pin p in distinctsignalinfo)
//            {
//                var detections = (from s in sigs
//                                  where p.SignalId == s.SignalId
//                                  select s.DetectionTypeID).Distinct().ToList();

//                foreach (int i in detections)
//                {
//                    p.MetricTypes += GetAvailableMetricIDs(i);
//                }


//            }


//            return (distinctsignalinfo);
//           // return new List<Business.Pin>();
//        }

//        private string GetAvailableMetricIDs(int DetectionTypeID)
//        {
//            List<int> ids = (from m in metrics
//                            where m.DetectionTypeID == DetectionTypeID
//                            select m.MetricID).ToList();
//            string result = string.Empty;
//            foreach(int i in ids)
//            {
//                result += i.ToString()+",";
//            }
//            return result;
//        }
//        public void Add(MOE.Common.Models.Signal signal)
//        {


//            MOE.Common.Models.Signal g = (from r in db.Signals
//                                          where r.SignalId == signal.SignalId
//                                          select r).FirstOrDefault();
//            if (g == null)
//            {
//                db.Signals.Add(g);
//                db.SaveChanges();
//            }

//        }

//        public void Remove(MOE.Common.Models.Signal signal)
//        {


//            MOE.Common.Models.Signal g = (from r in db.Signals
//                                          where r.SignalId == signal.SignalId
//                                          select r).FirstOrDefault();
//            if (g != null)
//            {
//                db.Signals.Remove(g);
//                db.SaveChanges();
//            }
//        }

//        public SignalFTPInfo GetSignalFTPInfoByID(string signalID)
//        {
//            var signal = (from r in db.Signals
//                          join ftp in db.ControllerType on r.ControllerTypeID equals ftp.ControllerTypeID
//                          where r.SignalId == signalID
//                          select new SignalFTPInfo
//                          { SignalId = r.SignalId,
//                            PrimaryName = r.PrimaryName,
//                            Secondary_Name = r.SecondaryName,
//                            User_Name = ftp.UserName,
//                            Password = ftp.Password,
//                            FTP_Directory = ftp.FTPDirectory,
//                            IP_Address = r.IPAddress,
//                            SNMPPort = ftp.SNMPPort,
//                            ActiveFTP = ftp.ActiveFTP,
//                            ControllerType = r.ControllerTypeID
//                                }
//                          );

//            return signal as SignalFTPInfo;
//        }

//        public void Remove(string id)
//        {
//            MOE.Common.Models.Signal g = db.Signals.Find(id);
//            if (g != null)
//            {
//                db.Signals.Remove(g);
//                db.SaveChanges();
//            }
//        }
//    }


//}


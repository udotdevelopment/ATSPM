using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectorRepository: IDetectorRepository
    {
        MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        //This method probably doesn't really belong here anymore.
        //public List<MOE.Common.Models.Detectors> GetDetectorsBySignalID(string SignalID)
        //{
        //    MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper(SignalID);

        //    List<MOE.Common.Models.Detectors> detectors = smh.GetDetectorsForSignal();

        //    return detectors;
        //}
        //This method probably doesn't really belong here anymore.
        //public List<MOE.Common.Models.Detectors> GetDetectorsBySignalIDAndPhase(string SignalID, int PhaseNumber)
        //{
        //    MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper(SignalID);

        //    List<int> phases = smh.GetPhasesForSignal();

        //    List<MOE.Common.Models.Detectors> detectors = new List<Detectors>();
            
        //    foreach(int p in phases)
        //    {
        //        foreach(MOE.Common.Models.Lane l in p.LaneGroups.FirstOrDefault().Lanes)
        //        {
        //            detectors.AddRange(l.Detectors);
        //        }
        //    }


        //    return detectors;
        //}

        public MOE.Common.Models.Detector GetDetectorByDetectorID(string DetectorID)
        {
            var det = (from r in db.Detectors
                      where r.DetectorID == DetectorID 
                      select r).FirstOrDefault();

            return det;
        }

        public MOE.Common.Models.Detector GetDetectorByID(int ID)
        {
            var det = db.Detectors.Find(ID);
            return det;
        }

        public List<MOE.Common.Models.Detector> GetDetectorsBySignalID(string SignalID)
        {
            return db.Signals.Find(SignalID).GetDetectorsForSignal();
        }

        public int GetMaximumDetectorChannel(string signalID)
        {
            int max = 0;
            var signal = db.Signals.Find(signalID);
            if (signal != null)
            {
                var detectors = signal.GetDetectorsForSignal();
                if (detectors.Count() > 0)
                {
                    max = detectors.Max(g => g.DetChannel);
                }
            }
            return max;
        }

        public Detector Add(Models.Detector detector)
        {
            Models.Detector g = (from r in db.Detectors
                                        where r.ID == detector.ID
                                        select r).FirstOrDefault();
            if (g == null)
            {
                detector.DetectionTypes = new List<DetectionType>();
                detector.DetectionTypes = db.DetectionTypes.Where(dt => detector.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList(); ;
                try
                {
                    db.Detectors.Add(detector);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Add";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
                
            }
            return detector;
        }
        public void Update(Models.Detector detector)
        {
            Models.Detector g = (from r in db.Detectors
                                        where r.ID == detector.ID
                                          select r).FirstOrDefault();
            if (g != null)
            {
                foreach(int i in detector.DetectionTypeIDs)
                {
                    Models.DetectionType t = (from r in db.DetectionTypes
                                              where r.DetectionTypeID == i
                                              select r).FirstOrDefault();

                    detector.DetectionTypes.Add(t);
                }
                try
                {
                    db.Entry(g).CurrentValues.SetValues(detector);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Update";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
            }
        }

        public void Remove(Models.Detector detector)
        {
            Models.Detector g = (from r in db.Detectors
                                        where r.ID == detector.ID
                                        select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    db.Detectors.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Remove";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
            }
        }


        public void Remove(int ID)
        {
            Models.Detector g = (from r in db.Detectors
                                        where r.ID == ID
                                        select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    db.Detectors.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Remove";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
            }
        }

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            MOE.Common.Models.Detector gd = db.Detectors
                .Where(g => g.DetectorID == detectorID)                
                .FirstOrDefault();    
            bool result = false;
            if(gd != null)
            {
                foreach (DetectionType dt in gd.DetectionTypes)
                {
                    foreach (Models.MetricType m in dt.MetricTypes)
                    {
                        if (m.MetricID == metricID)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        

        public List<MOE.Common.Models.Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            List<MOE.Common.Models.Detector> detectors = new List<Detector>();
            List<MOE.Common.Models.Detector> dets = db.Signals.Find(SignalID).GetDetectorsForSignal();
            foreach(MOE.Common.Models.Detector d in dets)
            {
                if(CheckReportAvialbility(d.DetectorID, MetricID))
                {
                    detectors.Add(d);
                }
            }
            return (detectors);
        }

    }

}

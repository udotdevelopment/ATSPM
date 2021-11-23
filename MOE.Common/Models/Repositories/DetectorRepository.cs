using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class DetectorRepository : IDetectorRepository
    {

        private SPM _db;





        public DetectorRepository()
        {
            _db = new SPM();

        }

        public DetectorRepository(SPM context)
        {
            _db = context;

        }


        SPM IDetectorRepository.GetContext()
        {

            return (_db);
        }

   

    //This method probably doesn't really belong here anymore.
        //public List<MOE.Common.Models.Detectors> GetDetectorsBySignalID(string SignalID)
        //{
        //    MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper(SignalId);

        //    List<MOE.Common.Models.Detectors> detectors = smh.GetDetectorsForSignal();

        //    return detectors;
        //}
        //This method probably doesn't really belong here anymore.
        //public List<MOE.Common.Models.Detectors> GetDetectorsBySignalIDAndPhase(string SignalId, int PhaseNumber)
        //{
        //    MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper(SignalId);

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

        public Detector GetDetectorByDetectorID(string DetectorID)
        {
            var det = (from r in _db.Detectors
                where r.DetectorID == DetectorID
                orderby r.DateAdded descending
                select r).FirstOrDefault();

            return det;
        }

        public List<Detector> GetDetectorsByApproachID(int approachID)
        {
            var detectors = new List<Detector>();
            detectors = _db.Detectors.Where(d => d.ApproachID == approachID).ToList(); 
            return detectors.OrderBy(d => d.DetectorID).ToList();
        }

        public Detector GetDetectorByID(int ID)
        {
            var det = _db.Detectors.Find(ID);
            return det;
        }

        public List<Detector> GetDetectorsBySignalID(string SignalID)
        {
            return _db.Signals.Find(SignalID).GetDetectorsForSignal();
        }

        public int GetMaximumDetectorChannel(int versionId)
        {
            var max = 0;
            var signal = _db.Signals.Find(versionId);
            if (signal != null)
            {
                var detectors = signal.GetDetectorsForSignal();
                if (detectors.Count() > 0)
                    max = detectors.Max(g => g.DetChannel);
            }
            return max;
        }

        public List<Detector> GetDetectorsByIds(List<int> excludedDetectorIds)
        {
            return _db.Detectors.Where(a => excludedDetectorIds.Contains(a.ID)).ToList();
        }

        public Detector Add(Detector detector)
        {
            var g = (from r in _db.Detectors
                where r.ID == detector.ID
                select r).FirstOrDefault();
            if (g == null)
            {
                detector.DetectionTypes = new List<DetectionType>();
                detector.DetectionTypes = _db.DetectionTypes
                    .Where(dt => detector.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                try
                {
                    _db.Detectors.Add(detector);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
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

        public void Update(Detector detector)
        {
            var g = (from r in _db.Detectors
                where r.ID == detector.ID
                select r).FirstOrDefault();
            if (g != null)
            {
                foreach (var i in detector.DetectionTypeIDs)
                {
                    var t = (from r in _db.DetectionTypes
                        where r.DetectionTypeID == i
                        select r).FirstOrDefault();

                    detector.DetectionTypes.Add(t);
                }
                try
                {
                    _db.Entry(g).CurrentValues.SetValues(detector);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
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

        public void Remove(Detector detector)
        {
            var g = (from r in _db.Detectors
                where r.ID == detector.ID
                select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Detectors.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
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


        public void Remove(int ID)
        {
            var g = (from r in _db.Detectors
                where r.ID == ID
                select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Detectors.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
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

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            var gd = _db.Detectors
                .GroupBy(g => g.DetectorID == detectorID)
                .Select(r => r.OrderByDescending(g => g.DateAdded).FirstOrDefault())
                .ToList();

            var result = false;
            if (gd != null)
                foreach (var firstGd in gd)
                    foreach (var dt in firstGd.DetectionTypes)
                        foreach (var m in dt.MetricTypes)
                            if (m.MetricID == metricID)
                                return true;
            return result;
        }

        public bool CheckReportAvialbilityByDetector(Detector gd, int metricID)
        {
            var result = false;
            if (gd != null)
                if (gd.DetectionTypes != null)
                {
                    foreach (var dt in gd.DetectionTypes)
                    {
                        foreach (var m in dt.MetricTypes)
                            if (m.MetricID == metricID)
                                return true;
                    }
                }
            return result;
        }


        public List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            var detectors = new List<Detector>();
            var dets = _db.Signals.Find(SignalID).GetDetectorsForSignal();
            foreach (var d in dets)
                if (CheckReportAvialbility(d.DetectorID, MetricID))
                    detectors.Add(d);
            return detectors;
        }

 
    }
}
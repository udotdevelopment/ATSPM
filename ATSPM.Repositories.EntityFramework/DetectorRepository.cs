using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class DetectorRepository : IDetectorRepository
    {

        private MOEContext _db;


        public DetectorRepository(MOEContext context)
        {
            _db = context;

        }


        //MOEContext IDetectorRepository.GetContext()
        //{
        //    return _db;
        //}



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
                       where r.DetectorId == DetectorID
                       orderby r.DateAdded descending
                       select r).FirstOrDefault();

            return det;
        }

        public Detector GetDetectorByID(int Id)
        {
            var det = _db.Detectors.Find(Id);
            return det;
        }

        public List<Detector> GetDetectorsBySignalID(string SignalID)
        {
            var signal = _db.Signals.Where(s => s.SignalId == SignalID).FirstOrDefault();
            var detectors = signal.GetDetectorsForSignal();
            return detectors;
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
            return _db.Detectors.Where(a => excludedDetectorIds.Contains(a.Id)).ToList();
        }

        public Detector Add(Detector detector)
        {

            throw new NotImplementedException();
            //var g = (from r in _db.Detectors
            //         where r.Id == detector.Id
            //         select r).FirstOrDefault();
            //if (g == null)
            //{
            //    detector.DetectionTypes = new List<DetectionType>();
            //    detector.DetectionTypes = _db.DetectionTypes
            //        .Where(dt => detector.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
            //    ;
            //    try
            //    {
            //        _db.Detectors.Add(detector);
            //        _db.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        var repository = new ApplicationEventRepository(_db);
            //        var error = new ApplicationEvent();
            //        error.ApplicationName = "MOE.Common";
            //        error.Class = "Models.Repository.DetectorRepository";
            //        error.Function = "Add";
            //        error.Description = ex.Message;
            //        error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
            //        error.Timestamp = DateTime.Now;
            //        repository.Add(error);
            //        throw;
            //    }
            //}
            //return detector;
        }

        public void Update(Detector detector)
        {
            throw new NotImplementedException();
            //var g = (from r in _db.Detectors
            //         where r.Id == detector.Id
            //         select r).FirstOrDefault();
            //if (g != null)
            //{
            //    foreach (var i in detector.DetectionTypeIDs)
            //    {
            //        var t = (from r in _db.DetectionTypes
            //                 where r.DetectionTypeID == i
            //                 select r).FirstOrDefault();

            //        detector.DetectionTypes.Add(t);
            //    }
            //    try
            //    {
            //        _db.Entry(g).CurrentValues.SetValues(detector);
            //        _db.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {

            //        var repository = new ApplicationEventRepository(_db);
            //        var error = new ApplicationEvent();
            //        error.ApplicationName = "MOE.Common";
            //        error.Class = "Models.Repository.DetectorRepository";
            //        error.Function = "Update";
            //        error.Description = ex.Message;
            //        error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
            //        error.Timestamp = DateTime.Now;
            //        repository.Add(error);
            //        throw;
            //    }
            //}
        }

        public void Remove(Detector detector)
        {
            var g = (from r in _db.Detectors
                     where r.Id == detector.Id
                     select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Detectors.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Remove";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
        }


        public void Remove(int Id)
        {
            var g = (from r in _db.Detectors
                     where r.Id == Id
                     select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Detectors.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorRepository";
                    error.Function = "Remove";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
        }

        public bool CheckReportAvialbility(string detectorID, int metricID)
        {
            var gd = _db.Detectors
                .GroupBy(g => g.DetectorId == detectorID)
                .Select(r => r.OrderByDescending(g => g.DateAdded).FirstOrDefault())
                .ToList();
            
            var result = false;
            if (gd != null)
                foreach (var firstGd in gd)
                    foreach (var dt in firstGd.DetectionTypeDetectors)                        
                        foreach (var m in dt.DetectionType.DetectionTypeMetricTypes)
                            if (m.MetricTypeMetricId == metricID)
                                return true;
            return result;
        }

        public bool CheckReportAvialbilityByDetector(Detector gd, int metricID)
        {
            var result = false;
            if (gd != null)
                if (gd.DetectionTypeDetectors != null)
                    foreach (var dt in gd.DetectionTypeDetectors)
                        foreach (var m in dt.DetectionType.DetectionTypeMetricTypes)
                            if (m.MetricTypeMetricId == metricID)
                                return true;
            return result;
        }


        public List<Detector> GetDetectorsBySignalIDAndMetricType(string SignalID, int MetricID)
        {
            var detectors = new List<Detector>();
            var dets = _db.Signals.Find(SignalID).GetDetectorsForSignal();
            foreach (var d in dets)
                if (CheckReportAvialbility(d.DetectorId, MetricID))
                    detectors.Add(d);
            return detectors;
        }

        public List<Detector> GetDetectorsBySignalIdMovementTypeIdDirectionTypeId(string signalId, int directionTypeId, List<int> movementTypeIds)
        {            
            var detectors = _db.Approaches
                .Where(a => a.SignalId == signalId)
                .SelectMany(a => a.Detectors).Where(d=> movementTypeIds.Contains(d.MovementTypeId??-1));
            return detectors.ToList();
        }
    }
}
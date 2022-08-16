using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApproachRepository : IApproachRepository
    {
        private readonly SPM _db;

        public ApproachRepository()
        {
            _db = new SPM();
        }

        public ApproachRepository(SPM context)
        {
            _db = context;
        }

        public List<Approach> GetAllApproaches()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            var approaches = (from r in _db.Approaches
                select r).ToList();

            if (approaches.Count == 0)
            {
                var ex = new Exception("There were no records in this Query");
                throw ex;
            }
            return approaches;
        }

        public Approach GetApproachByApproachID(int approachId)
        {
            var approach = (from r in _db.Approaches
                where r.ApproachID == approachId
                select r).FirstOrDefault();
            if (approach != null)
                return approach;
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "Models.Repository.ApproachRepository",
                    Function = "GetApproachByApproachID",
                    Description = "No approach for ID.  Attempted ID# = " + approachId,
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Timestamp = DateTime.Now
                };
                repository.Add(error);
                throw new Exception("There is no Approach for this ID");
            }
        }

        public void AddOrUpdate(Approach approach)
        {
            var g = (from r in _db.Approaches
                where r.ApproachID == approach.ApproachID
                select r).FirstOrDefault();
            if (approach.Detectors != null)
            {
                foreach (var det in approach.Detectors)
                {
                    AddDetectiontypestoDetector(det);
                }
            }
            if (g != null)
                try
                {
                    _db.Entry(g).CurrentValues.SetValues(approach);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent
                    {
                        ApplicationName = "MOE.Common",
                        Class = "Models.Repository.ApproachRepository",
                        Function = "Update",
                        Description = ex.Message,
                        SeverityLevel = ApplicationEvent.SeverityLevels.High,
                        Timestamp = DateTime.Now
                    };
                    repository.Add(error);
                    throw;
                }
            else
                try
                {
                    foreach (var d in approach.Detectors)
                        if (d.DetectionTypes == null && d.DetectionTypeIDs != null)
                            d.DetectionTypes = _db.DetectionTypes
                                .Where(dt => d.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                    _db.Approaches.Add(approach);
                    _db.SaveChanges();
                }

                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent
                    {
                        ApplicationName = "MOE.Common",
                        Class = "Models.Repository.ApproachRepository",
                        Function = "Add",
                        Description = ex.Message,
                        SeverityLevel = ApplicationEvent.SeverityLevels.High,
                        Timestamp = DateTime.Now
                    };
                    repository.Add(error);
                    throw;
                }
        }

        public void Remove(Approach approach)
        {
            var g = (from r in _db.Approaches
                where r.ApproachID == approach.ApproachID
                select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Approaches.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    {
                        var repository =
                            ApplicationEventRepositoryFactory.Create();
                        var error = new ApplicationEvent
                        {
                            ApplicationName = "MOE.Common",
                            Class = "Models.Repository.ApproachRepository",
                            Function = "Remove",
                            Description = ex.Message,
                            SeverityLevel = ApplicationEvent.SeverityLevels.High,
                            Timestamp = DateTime.Now
                        };
                        repository.Add(error);
                        throw ex;
                    }
                }
        }

        public Approach FindAppoachByVersionIdPhaseOverlapAndDirection(int versionId, int phaseNumber, bool isOverlap,
            int directionTypeId)
        {
            var g = (from r in _db.Approaches
                where r.VersionID == versionId
                      && r.ProtectedPhaseNumber == phaseNumber
                      && r.IsProtectedPhaseOverlap == isOverlap
                      && r.DirectionTypeID == directionTypeId
                select r).FirstOrDefault();

            return g;
        }

        public void Remove(int approachId)
        {
            var approach = _db.Approaches.Find(approachId);
            if (approach != null)
                try
                {
                    _db.Approaches.Remove(approach);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error =
                        new ApplicationEvent
                        {
                            ApplicationName = "MOE.Common",
                            Class = "Models.Repository.ApproachRepository",
                            Function = "Remove",
                            Description = ex.Message,
                            SeverityLevel = ApplicationEvent.SeverityLevels.High,
                            Timestamp = DateTime.Now
                        };
                    repository.Add(error);
                    throw ex;
                }
        }

        public List<Approach> GetApproachesByIds(List<int> excludedApproachIds)
        {
            return _db.Approaches.Where(a => excludedApproachIds.Contains(a.ApproachID)).ToList();
        }

        public Approach Add(Approach approach)
        {
            var g = (from r in _db.Approaches
                where r.ApproachID == approach.ApproachID
                select r).FirstOrDefault();
            foreach (var det in approach.Detectors)
            {
                AddDetectiontypestoDetector(det);
            }

            if (g == null)
            {
                approach = _db.Approaches.Add(approach);
                _db.SaveChanges();
            }
            else
            {
                AddOrUpdate(approach);
                _db.SaveChanges();
            }
            return approach;
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

        ICollection<Approach> IApproachRepository.GetApproachesForSignal(int versionID)
        {
            return _db.Approaches.Where(a => a.VersionID == versionID).ToList();
        }
    }
}
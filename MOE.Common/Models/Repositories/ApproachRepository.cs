using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MOE.Common.Models.Repositories
{
    public class ApproachRepository : IApproachRepository
    {
        Models.SPM _db;
        public ApproachRepository()
        {
            _db = new SPM();
        }
        public ApproachRepository(SPM context)
        {
            _db = context;
        }

        public List<Models.Approach> GetAllApproaches()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            List<Models.Approach> approaches = (from r in _db.Approaches
                                                 select r).ToList();

            if (approaches.Count == 0)
            {
                Exception ex = new Exception("There were no records in this Query");
                throw(ex);
            }
            return approaches;
        }

        public Models.Approach GetApproachByApproachID(int approachId)
        {
            var approach = (from r in _db.Approaches
                             where r.ApproachID == approachId
                             select r).FirstOrDefault(); ;

            if (approach != null)
            {
                return approach;
            }
            {
                Repositories.IApplicationEventRepository repository =
                    ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "Models.Repository.ApproachRepository",
                    Function = "GetApproachByApproachID",
                    Description = "No approach for ID.  Attempted ID# = " + approachId.ToString(),
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Timestamp = DateTime.Now
                };
                repository.Add(error);
                throw(new Exception("There is no Approach for this ID"));
            }
        }

        public void AddOrUpdate(MOE.Common.Models.Approach approach)
        {


            MOE.Common.Models.Approach g = (from r in _db.Approaches
                                             where r.ApproachID == approach.ApproachID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    _db.Entry(g).CurrentValues.SetValues(approach);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent
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
            }
            else
            {
                try
                {
                    foreach(Detector d in approach.Detectors)
                    {
                        if(d.DetectionTypes == null && d.DetectionTypeIDs != null)
                        {
                            d.DetectionTypes = _db.DetectionTypes.Where(dt => d.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                        }
                    }
                    _db.Approaches.Add(approach);
                    _db.SaveChanges();
                }
                
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent
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


        }

        public void Remove(MOE.Common.Models.Approach approach)
        {            
            MOE.Common.Models.Approach g = (from r in _db.Approaches
                                             where r.ApproachID == approach.ApproachID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    _db.Approaches.Remove(g);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {

                    {
                        MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                        MOE.Common.Models.ApplicationEvent error = new ApplicationEvent
                        {
                            ApplicationName = "MOE.Common",
                            Class = "Models.Repository.ApproachRepository",
                            Function = "Remove",
                            Description = ex.Message,
                            SeverityLevel = ApplicationEvent.SeverityLevels.High,
                            Timestamp = DateTime.Now
                        };
                        repository.Add(error);
                        throw (ex);
                    }
                }
            }
     
        }

        public Approach FindAppoachByVersionIdPhaseOverlapAndDirection(int versionId, int phaseNumber, bool isOverlap, int directionTypeId)
        {
            MOE.Common.Models.Approach g = (from r in _db.Approaches
            where r.VersionID== versionId
            && r.ProtectedPhaseNumber == phaseNumber
            && r.IsProtectedPhaseOverlap == isOverlap
            && r.DirectionTypeID == directionTypeId
            select r).FirstOrDefault();

            return g;
        }

        public void Remove(int approachId)
        {
            Approach approach = _db.Approaches.Find(approachId);
            if(approach != null) 
            {
                try
                {
                    _db.Approaches.Remove(approach);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {

                    
                        MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                        MOE.Common.Models.ApplicationEvent error =
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
                        throw (ex);
                    
                }
            }
        }

        public Approach Add(MOE.Common.Models.Approach approach)
        {
            MOE.Common.Models.Approach g = (from r in _db.Approaches
                                            where r.ApproachID == approach.ApproachID
                                            select r).FirstOrDefault();
            if (g == null)
            {
                approach = _db.Approaches.Add(approach);
                _db.SaveChanges();
            }
            else
            {
                this.AddOrUpdate(approach);
                _db.SaveChanges();
            }
            return approach;
        }

        
    }
}

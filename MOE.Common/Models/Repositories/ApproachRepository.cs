using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachRepository : IApproachRepository
    {
        Models.SPM db = new SPM();


        public List<Models.Approach> GetAllApproaches()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Approach> approaches = (from r in db.Approaches
                                                 select r).ToList();

            if (approaches.Count == 0)
            {
                Exception ex = new Exception("There were no records in this Query");
                throw(ex);
            }
            else
            {
                return approaches;
            }
        }

        public Models.Approach GetApproachByApproachID(int approachID)
        {
            var approach = (from r in db.Approaches
                             where r.ApproachID == approachID
                             select r);

            if (approach != null)
            {
                return approach.FirstOrDefault();
            }
            else
            {
                
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRepository";
                    error.Function = "GetApproachByApproachID";
                    error.Description = "No approach for ID.  Attempted ID# = " + approachID.ToString();
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw(new Exception("There is no Approach for this ID"));
                }
            }
        }

        public void AddOrUpdate(MOE.Common.Models.Approach approach)
        {


            MOE.Common.Models.Approach g = (from r in db.Approaches
                                             where r.ApproachID == approach.ApproachID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    db.Entry(g).CurrentValues.SetValues(approach);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRepository";
                    error.Function = "Update";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
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
                            d.DetectionTypes = db.DetectionTypes.Where(dt => d.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                        }
                    }
                    db.Approaches.Add(approach);
                    db.SaveChanges();
                }
                
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRepository";
                    error.Function = "Add";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }

            }


        }

        public void Remove(MOE.Common.Models.Approach approach)
        {            
            MOE.Common.Models.Approach g = (from r in db.Approaches
                                             where r.ApproachID == approach.ApproachID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    db.Approaches.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    {
                        MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                        MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                        error.ApplicationName = "MOE.Common";
                        error.Class = "Models.Repository.ApproachRepository";
                        error.Function = "Remove";
                        error.Description = ex.Message;
                        error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                        error.Timestamp = DateTime.Now;
                        repository.Add(error);
                        throw (ex);
                    }
                }
            }
     
        }
        public void Remove(int approachID)
        {
            Approach approach = db.Approaches.Find(approachID);
            if(approach != null)
            {
                try
                {
                    db.Approaches.Remove(approach);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    {
                        MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                        MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                        error.ApplicationName = "MOE.Common";
                        error.Class = "Models.Repository.ApproachRepository";
                        error.Function = "Remove";
                        error.Description = ex.Message;
                        error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                        error.Timestamp = DateTime.Now;
                        repository.Add(error);
                        throw (ex);
                    }
                }
            }
        }

        public Approach Add(MOE.Common.Models.Approach approach)
        {
            MOE.Common.Models.Approach g = (from r in db.Approaches
                                            where r.ApproachID == approach.ApproachID
                                            select r).FirstOrDefault();
            if (g == null)
            {
                approach = db.Approaches.Add(approach);
                db.SaveChanges();
            }
            else
            {
                this.AddOrUpdate(approach);
                db.SaveChanges();
            }
            return approach;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Validation;
using System.Web.UI.WebControls;



namespace MOE.Common.Models.Repositories
{
    public class JurisdictionRepository : IJurisdictionRepository
    {
        private readonly SPM db = new SPM();

        public List<Jurisdiction> GetAllJurisdictions()
        {
            var jurisdictions = (from r in db.Jurisdictions
                          orderby r.JurisdictionName
                          select r).ToList();
            return jurisdictions;
        }

        public Jurisdiction GetJurisdictionByID(int jurisdictionId)
        {
            var jurisdiction = (from r in db.Jurisdictions
                                where r.Id == jurisdictionId
                            select r).FirstOrDefault();
            if (jurisdiction != null)
                return jurisdiction;
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "Models.Repository.JurisdictionRepository",
                    Function = "GetApproachByApproachID",
                    Description = "No jurisdiction for ID.  Attempted ID# = " + jurisdictionId,
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Timestamp = DateTime.Now
                };
                repository.Add(error);
                throw new Exception("There is no Jurisdiction for this ID");
            }
        }

        public Jurisdiction GetJurisdictionByName(string jurisdictionName)
        {
            var jurisdiction = (from r in db.Jurisdictions
                         where r.JurisdictionName == jurisdictionName
                         select r).FirstOrDefault();
            return jurisdiction;
        }

        public void DeleteByID(int jurisdictionId)
        {
            var jurisdiction = (from r in db.Jurisdictions
                         where r.Id == jurisdictionId
                                select r).FirstOrDefault();

            db.Jurisdictions.Remove(jurisdiction);
            db.SaveChanges();
        }

        public void Remove(Jurisdiction jurisdiction)
        {
            var g = (from r in db.Jurisdictions
                     where r.Id == jurisdiction.Id
                     select r).FirstOrDefault();
            if (g != null)
                try
                {
                    db.Jurisdictions.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    {
                        var repository =
                            ApplicationEventRepositoryFactory.Create();
                        var error = new ApplicationEvent
                        {
                            ApplicationName = "MOE.Common",
                            Class = "Models.Repository.JurisdictionRepository",
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

        public void Update(Jurisdiction newJurisdiction)
        {
            var jurisdiction = GetJurisdictionByID(newJurisdiction.Id);
            if (jurisdiction != null)
            {
                db.Entry(jurisdiction).CurrentValues.SetValues(newJurisdiction);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Jurisdiction Not Found");
            }
        }

        public void Add(Jurisdiction newJurisdiction)
        {
            try
            {
                db.Jurisdictions.Add(newJurisdiction);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachJurisdictionRepository";
                error.Function = "Add";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }
    }
}
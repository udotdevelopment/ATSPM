using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            var jurisdiction = db.Jurisdictions
                .Include(r => r.JurisdictionSignals.Select(s => s.Signal)).FirstOrDefault(r => r.Id == jurisdictionId);
            jurisdiction.JurisdictionSignals = jurisdiction.JurisdictionSignals.OrderBy(s => s.Order).ToList();
            var signalRepository = Repositories.SignalsRepositoryFactory.Create();
            foreach (var jurisdictionSignal in jurisdiction.JurisdictionSignals)
            {
                jurisdictionSignal.Signal = signalRepository.GetLatestVersionOfSignalBySignalID(jurisdictionSignal.SignalId);
            }
            if (jurisdiction != null)
                return jurisdiction;
            var repository =
                ApplicationEventRepositoryFactory.Create();
            var error = new ApplicationEvent();
            error.ApplicationName = "MOE.Common";
            error.Class = "Models.Repository.ApproachJurisdictionRepository";
            error.Function = "GetByJurisdictionID";
            error.Description = "No Jurisdiction for ID.  Attempted ID# = " + jurisdictionId;
            error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
            error.Timestamp = DateTime.Now;
            repository.Add(error);
            throw new Exception("There is no Jurisdiction for this ID");
        }

        public Jurisdiction GetJurisdictionByIDAndDate(int jurisdictionId, DateTime startDate)
        {
            var jurisdiction = db.Jurisdictions
                .Include(r => r.JurisdictionSignals.Select(s => s.Signal)).FirstOrDefault(r => r.Id == jurisdictionId);
            jurisdiction.JurisdictionSignals = jurisdiction.JurisdictionSignals.OrderBy(s => s.Order).ToList();
            var signalRepository = Repositories.SignalsRepositoryFactory.Create();
            foreach (var jurisdictionSignal in jurisdiction.JurisdictionSignals)
            {
                jurisdictionSignal.Signal = signalRepository.GetVersionOfSignalByDate(jurisdictionSignal.SignalId, startDate);
            }
            if (jurisdiction != null)
                return jurisdiction;
            return jurisdiction;
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

        public void Update(Jurisdiction newJurisdiction)
        {
            var jurisdiction = (from r in db.Jurisdictions
                         where r.Id == newJurisdiction.Id
                                select r).FirstOrDefault();

            if (jurisdiction != null)
            {
                var newjurisdiction = new Jurisdiction();

                newjurisdiction.Id = jurisdiction.Id;
                newjurisdiction.JurisdictionName = newjurisdiction.JurisdictionName;
                try
                {
                    db.Entry(jurisdiction).CurrentValues.SetValues(newjurisdiction);
                    db.SaveChanges();
                }

                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachJurisdictionRepository";
                    error.Function = "UpdateByID";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
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
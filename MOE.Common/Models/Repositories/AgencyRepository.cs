using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class AgencyRepository : IGenericRepository<ATSPM_Agency>
    {
        private SPM db = new SPM();

        public AgencyRepository()
        {
        }

        public AgencyRepository(SPM context)
        {
            db = context;
        }

        public IEnumerable<ATSPM_Agency> GetAll()
        {
            return db.ATSPM_Agencies.ToList();
        }

        public ATSPM_Agency GetByID(int id)
        {
            return db.ATSPM_Agencies.Find(id);
        }

        public void Delete(ATSPM_Agency entity)
        {
            db.ATSPM_Agencies.Remove(db.ATSPM_Agencies.Find(entity.AgencyID));
        }

        public void Update(ATSPM_Agency entity)
        {
            var agencyInDatabase = db.ATSPM_Agencies.Find(entity.AgencyID);
            if (agencyInDatabase == null)
                Add(entity);
            else
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
        }

        public void Add(ATSPM_Agency entity)
        {
            db.ATSPM_Agencies.Add(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SetAgencyRepository(SPM context)
        {
            db = context;
        }

        public void dosomething()
        {
        }
    }
}
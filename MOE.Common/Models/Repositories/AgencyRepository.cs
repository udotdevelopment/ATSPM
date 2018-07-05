using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class AgencyRepository : IGenericRepository<Agency>
    {
        private SPM db = new SPM();

        public AgencyRepository()
        {
        }

        public AgencyRepository(SPM context)
        {
            db = context;
        }

        public IEnumerable<Agency> GetAll()
        {
            return db.Agencies.ToList();
        }

        public Agency GetByID(int id)
        {
            return db.Agencies.Find(id);
        }

        public void Delete(Agency entity)
        {
            db.Agencies.Remove(db.Agencies.Find(entity.AgencyID));
        }

        public void Update(Agency entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.AgencyID);
            if (agencyInDatabase == null)
                Add(entity);
            else
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
        }

        public void Add(Agency entity)
        {
            db.Agencies.Add(entity);
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
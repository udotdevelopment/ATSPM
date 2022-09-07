using ATSPM.IRepositories;
using System.Collections.Generic;
using System.Linq;
using ATSPM.Application.Models;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class AgencyRepository : IGenericRepository<Agency>
    {
        private MOEContext db;

        public AgencyRepository(MOEContext context)
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
            db.Agencies.Remove(db.Agencies.Find(entity.AgencyId));
        }

        public void Update(Agency entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.AgencyId);
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

        public void SetAgencyRepository(MOEContext context)
        {
            db = context;
        }

        public void dosomething()
        {
        }
    }
}
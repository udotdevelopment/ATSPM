using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class AgencyRepository : IGenericRepository<Agency>
    {
        Models.SPM db = new Models.SPM();

        public AgencyRepository()
        {
        }
        public AgencyRepository(Models.SPM context)
        {
            db = context;
        }
        public void SetAgencyRepository(Models.SPM context)
        {
            db = context;
        }
        public IEnumerable<Models.Agency> GetAll()
        {
            return db.Agencies.ToList();
        }
        public Models.Agency GetByID(int id)
        {
            return db.Agencies.Find(id);
        }
        public void Delete(Agency entity)
        {
            db.Agencies.Remove(db.Agencies.Find(entity.AgencyID));
        }
        public void Update(Models.Agency entity)
        {
            var agencyInDatabase = db.Agencies.Find(entity.AgencyID);
            if(agencyInDatabase == null)
            {
                Add(entity);
            }
            else
            {
                db.Entry(agencyInDatabase).CurrentValues.SetValues(entity);
            }
        }

        public void Add(Models.Agency entity)
        {
            db.Agencies.Add(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void dosomething()
        {

        }
    }
}

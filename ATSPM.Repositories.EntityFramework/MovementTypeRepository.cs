using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class MovementTypeRepository : IMovementTypeRepository
    {
        private readonly MOEContext db;

        public MovementTypeRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<MovementType> GetAllMovementTypes()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var movementTypes = (from r in db.MovementTypes
                                 orderby r.DisplayOrder
                                 select r).ToList();

            return movementTypes;
        }

        public MovementType GetMovementTypeByMovementTypeID(int movementTypeID)
        {
            var movementType = from r in db.MovementTypes
                               where r.MovementTypeId == movementTypeID
                               select r;

            return movementType.FirstOrDefault();
        }

        public void Update(MovementType movementType)
        {
            var g = (from r in db.MovementTypes
                     where r.MovementTypeId == movementType.MovementTypeId
                     select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(movementType);
                db.SaveChanges();
            }
            else
            {
                db.MovementTypes.Add(movementType);
                db.SaveChanges();
            }
        }

        public void Remove(MovementType movementType)
        {
            var g = (from r in db.MovementTypes
                     where r.MovementTypeId == movementType.MovementTypeId
                     select r).FirstOrDefault();
            if (g != null)
            {
                db.MovementTypes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MovementType movementType)
        {
            var g = (from r in db.MovementTypes
                     where r.MovementTypeId == movementType.MovementTypeId
                     select r).FirstOrDefault();
            if (g == null)
            {
                db.MovementTypes.Add(g);
                db.SaveChanges();
            }
        }
    }
}
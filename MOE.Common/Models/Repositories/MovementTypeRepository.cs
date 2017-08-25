using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MovementTypeRepository : IMovementTypeRepository
    {
        Models.SPM db = new SPM();


        public List<Models.MovementType> GetAllMovementTypes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.MovementType> movementTypes = (from r in db.MovementTypes
                                                       orderby r.DisplayOrder
                                         select r).ToList();

            return movementTypes;
        }

        public Models.MovementType GetMovementTypeByMovementTypeID(int movementTypeID)
        {
            var movementType = (from r in db.MovementTypes
                         where r.MovementTypeID == movementTypeID
                         select r);

            return movementType.FirstOrDefault();
        }

        public void Update(MOE.Common.Models.MovementType movementType)
        {


            MOE.Common.Models.MovementType g = (from r in db.MovementTypes
                                         where r.MovementTypeID == movementType.MovementTypeID
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

        public void Remove(MOE.Common.Models.MovementType movementType)
        {


            MOE.Common.Models.MovementType g = (from r in db.MovementTypes
                                         where r.MovementTypeID == movementType.MovementTypeID
                                         select r).FirstOrDefault();
            if (g != null)
            {
                db.MovementTypes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.MovementType movementType)
        {


            MOE.Common.Models.MovementType g = (from r in db.MovementTypes
                                         where r.MovementTypeID == movementType.MovementTypeID
                                         select r).FirstOrDefault();
            if (g == null)
            {
                db.MovementTypes.Add(g);
                db.SaveChanges();
            }

        }

    }
}

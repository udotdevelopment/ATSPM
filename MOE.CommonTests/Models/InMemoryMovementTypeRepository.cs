using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryMovementTypeRepository : IMovementTypeRepository
    {
        private InMemoryMOEDatabase _MOE;
        public InMemoryMovementTypeRepository()
        {
            _MOE = new InMemoryMOEDatabase();

        }

        public InMemoryMovementTypeRepository(InMemoryMOEDatabase MOE)
        {
            _MOE = MOE;

        }

        public void Add(MovementType movementType)
        {
            throw new System.NotImplementedException();
        }

        public List<MovementType> GetAllMovementTypes()
        {
          
            List<MovementType> movementTypes = (from r in _MOE.MovementTypes
                orderby r.DisplayOrder
                select r).ToList();

            return movementTypes;
        }

        public MovementType GetMovementTypeByMovementTypeID(int movementTypeID)
        {
            var movementType = (from r in _MOE.MovementTypes
                where r.MovementTypeID == movementTypeID
                select r);

            return movementType.FirstOrDefault();
        }

        public MovementType GetMovementTypeByDesc(string desc)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(MovementType movementType)
        {
            throw new System.NotImplementedException();
        }

        public void Update(MovementType movementType)
        {
            throw new System.NotImplementedException();
        }
    }
}
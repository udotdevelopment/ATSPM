using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IMovementTypeRepository
    {
        List<MovementType> GetAllMovementTypes();
        MovementType GetMovementTypeByMovementTypeID(int movementTypeID);
        MovementType GetMovementTypeByDesc(string desc);
        void Update(MovementType movementType);
        void Add(MovementType movementType);
        void Remove(MovementType movementType);
    }
}
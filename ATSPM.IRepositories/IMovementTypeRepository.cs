using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IMovementTypeRepository
    {
        List<MovementType> GetAllMovementTypes();
        MovementType GetMovementTypeByMovementTypeID(int movementTypeID);
        void Update(MovementType movementType);
        void Add(MovementType movementType);
        void Remove(MovementType movementType);
    }
}
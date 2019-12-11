using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
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
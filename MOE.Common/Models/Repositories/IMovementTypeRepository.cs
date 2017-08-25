using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IMovementTypeRepository
    {
        List<Models.MovementType> GetAllMovementTypes();
        Models.MovementType GetMovementTypeByMovementTypeID(int movementTypeID);
        void Update(MOE.Common.Models.MovementType movementType);
        void Add(MOE.Common.Models.MovementType movementType);
        void Remove(MOE.Common.Models.MovementType movementType);
    }
}

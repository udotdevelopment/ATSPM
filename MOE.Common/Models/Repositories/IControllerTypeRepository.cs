using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IControllerTypeRepository
    {
        List<ControllerType> GetControllerTypes();
    }
}
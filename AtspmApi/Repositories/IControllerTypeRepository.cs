using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IControllerTypeRepository
    {
        List<ControllerType> GetControllerTypes();
    }
}
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryControllerTypeRepository : IControllerTypeRepository
    {
        private InMemoryMOEDatabase _MOE = new InMemoryMOEDatabase();
        public List<ControllerType> GetControllerTypes()
        {
            var controllerTypes = (from r in _MOE.ControllerTypes
                select r).ToList();

            return controllerTypes;
        }
    }
}
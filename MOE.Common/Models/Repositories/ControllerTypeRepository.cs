using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ControllerTypeRepository : IControllerTypeRepository
    {
        private readonly SPM db = new SPM();

        public List<ControllerType> GetControllerTypes()
        {
            var list = (from r in db.ControllerType
                select r).ToList();

            return list;
        }
    }
}
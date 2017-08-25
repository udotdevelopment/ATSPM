using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ControllerTypeRepository : IControllerTypeRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<MOE.Common.Models.ControllerType> GetControllerTypes()
        {
            List<MOE.Common.Models.ControllerType> list = (from r in db.ControllerType
                                                            select r).ToList();

            return list;
        }
    }
}

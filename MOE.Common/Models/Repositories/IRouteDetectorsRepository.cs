using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IRouteDetectorsRepository
    {
        List<Models.Route_Detectors> SelectByRegion(int regionID);
    }
}

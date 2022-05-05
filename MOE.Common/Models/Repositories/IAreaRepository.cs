using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IAreaRepository
    {
        List<Area> GetAllAreas();
        Area GetAreaByID(int areaId);
        Area GetAreaByName(string AreaName);
        List<Area> GetListOfAreasForSignal(string signalId);
        void DeleteByID(int areaId);
        void Remove(Area Area);
        void Update(Area newArea);
        void Add(Area newArea);
    }
}

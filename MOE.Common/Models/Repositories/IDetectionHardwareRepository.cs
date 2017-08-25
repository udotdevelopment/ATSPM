using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectionHardwareRepository
    {
        List<Models.DetectionHardware> GetAllDetectionHardwares();
        List<Models.DetectionHardware> GetAllDetectionHardwaresNoBasic();
        Models.DetectionHardware GetDetectionHardwareByID(int ID);
        void Update(MOE.Common.Models.DetectionHardware DetectionHardware);
        void Add(MOE.Common.Models.DetectionHardware DetectionHardware);
        void Remove(MOE.Common.Models.DetectionHardware DetectionHardware);
    }
}

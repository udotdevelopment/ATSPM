using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectionHardwareRepository
    {
        List<DetectionHardware> GetAllDetectionHardwares();
        List<DetectionHardware> GetAllDetectionHardwaresNoBasic();
        DetectionHardware GetDetectionHardwareByID(int ID);
        DetectionHardware GetDetectionHardwareByName(string name);
        void Update(DetectionHardware DetectionHardware);
        void Add(DetectionHardware DetectionHardware);
        void Remove(DetectionHardware DetectionHardware);
    }
}
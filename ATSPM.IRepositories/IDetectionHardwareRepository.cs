using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IDetectionHardwareRepository
    {
        List<DetectionHardware> GetAllDetectionHardwares();
        List<DetectionHardware> GetAllDetectionHardwaresNoBasic();
        DetectionHardware GetDetectionHardwareByID(int ID);
        void Update(DetectionHardware DetectionHardware);
        void Add(DetectionHardware DetectionHardware);
        void Remove(DetectionHardware DetectionHardware);
    }
}
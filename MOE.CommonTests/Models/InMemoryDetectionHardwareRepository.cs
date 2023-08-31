using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryDetectionHardwareRepository : IDetectionHardwareRepository
    {
        private InMemoryMOEDatabase _MOE;

        public InMemoryDetectionHardwareRepository()
        {
            _MOE = new InMemoryMOEDatabase();

        }

        public InMemoryDetectionHardwareRepository(InMemoryMOEDatabase MOE)
        {
            _MOE = MOE;

        }
        public void Add(DetectionHardware DetectionHardware)
        {
            throw new System.NotImplementedException();
        }

        public List<DetectionHardware> GetAllDetectionHardwares()
        {
            
            List<DetectionHardware> DetectionHardwares = (from r in _MOE.DetectionHardwares
                select r).ToList();

            return DetectionHardwares;
        }

        public List<DetectionHardware> GetAllDetectionHardwaresNoBasic()
        {
            List<DetectionHardware> DetectionHardwares = (from r in _MOE.DetectionHardwares
                where r.Name != "Basic"
                select r).ToList();

            return DetectionHardwares;
        }

        public DetectionHardware GetDetectionHardwareByID(int ID)
        {
            var DetectionHardware = (from r in _MOE.DetectionHardwares
                where r.ID == ID
                select r);

            return DetectionHardware.FirstOrDefault();
        }

        public DetectionHardware GetDetectionHardwareByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public List<DetectionHardware> GetDetectionHardwareByIDs(List<int> IDs)
        {
            return (from r in _MOE.DetectionHardwares
                where IDs.Contains(r.ID)
                select r).ToList();
        }

        public void Remove(DetectionHardware DetectionHardware)
        {
            throw new System.NotImplementedException();
        }

        public void Update(DetectionHardware DetectionHardware)
        {
            throw new System.NotImplementedException();
        }
    }
}
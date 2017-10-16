using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Linq;

namespace MOE.CommonTests.Models
{
    public class InMemoryDetectionTypeRepository : IDetectionTypeRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryDetectionTypeRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemoryDetectionTypeRepository()
        {
            this._db = new InMemoryMOEDatabase();
        }

        public List<DetectionType> GetAllDetectionTypes()
        {
            throw new System.NotImplementedException();
        }

        public List<DetectionType> GetAllDetectionTypesNoBasic()
        {
           var types = (from r in _db.DetectionTypes
                       where r.DetectionTypeID != 1
                       select r).ToList();

            return types;
        }

        public DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID)
        {
            throw new System.NotImplementedException();
        }

        public void Update(DetectionType detectionType)
        {
            throw new System.NotImplementedException();
        }

        public void Add(DetectionType detectionType)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(DetectionType detectionType)
        {
            throw new System.NotImplementedException();
        }
    }
}
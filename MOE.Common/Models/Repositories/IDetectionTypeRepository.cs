using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectionTypeRepository
    {
        List<Models.DetectionType> GetAllDetectionTypes();
        List<Models.DetectionType> GetAllDetectionTypesNoBasic();
        //List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> GetAllDetectionTypesWithSupportedMetricAbbreviations();
        Models.DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID);
        void Update(MOE.Common.Models.DetectionType detectionType);
        void Add(MOE.Common.Models.DetectionType detectionType);
        void Remove(MOE.Common.Models.DetectionType detectionType);
    }
}

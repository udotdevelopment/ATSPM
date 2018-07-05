using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectionTypeRepository
    {
        List<DetectionType> GetAllDetectionTypes();

        List<DetectionType> GetAllDetectionTypesNoBasic();

        //List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> GetAllDetectionTypesWithSupportedMetricAbbreviations();
        DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID);

        void Update(DetectionType detectionType);
        void Add(DetectionType detectionType);
        void Remove(DetectionType detectionType);
    }
}
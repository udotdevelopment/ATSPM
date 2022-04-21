using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorCommentRepository
    {
        List<DetectorComment> GetAllDetectorComments();
        DetectorComment GetDetectorCommentByDetectorCommentID(int detectorCommentID);
        DetectorComment GetMostRecentDetectorCommentByDetectorID(int ID);
        void AddOrUpdate(DetectorComment detectorComment);
        void Add(DetectorComment detectorComment);
        void Remove(DetectorComment detectorComment);
        List<DetectorComment> GetDetectorsCommentsByDetectorID(int ID);
    }
}
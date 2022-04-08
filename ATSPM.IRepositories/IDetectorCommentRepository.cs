using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IDetectorCommentRepository
    {
        List<DetectorComment> GetAllDetectorComments();
        DetectorComment GetDetectorCommentByDetectorCommentID(int detectorCommentID);
        DetectorComment GetMostRecentDetectorCommentByDetectorID(int ID);
        void AddOrUpdate(DetectorComment detectorComment);
        void Add(DetectorComment detectorComment);
        void Remove(DetectorComment detectorComment);
    }
}
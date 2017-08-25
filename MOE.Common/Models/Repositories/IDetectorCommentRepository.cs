using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorCommentRepository
    {
        List<Models.DetectorComment> GetAllDetectorComments();
        Models.DetectorComment GetDetectorCommentByDetectorCommentID(int detectorCommentID);
        Models.DetectorComment GetMostRecentDetectorCommentByDetectorID(int ID);
        void AddOrUpdate(MOE.Common.Models.DetectorComment detectorComment);
        void Add(MOE.Common.Models.DetectorComment detectorComment);
        void Remove(MOE.Common.Models.DetectorComment detectorComment);
    }
}

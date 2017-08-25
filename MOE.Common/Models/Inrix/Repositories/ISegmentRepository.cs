using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ISegmentRepository
    {
        List<Models.Inrix.Segment> GetAll();
        void Add(Models.Inrix.Segment segment);
        void Update(int segmentID, string newSegmentName, string newSegmentDescription);
        void Remove(Models.Inrix.Segment segment);
        void RemoveByID(int segmentID);

        Models.Inrix.Segment SelectByID(int segmentID);

        Models.Inrix.Segment SelectSegmentByName(string name);
    }
}

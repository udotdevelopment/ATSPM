using System.Collections.Generic;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ISegmentRepository
    {
        List<Segment> GetAll();
        void Add(Segment segment);
        void Update(int segmentID, string newSegmentName, string newSegmentDescription);
        void Remove(Segment segment);
        void RemoveByID(int segmentID);

        Segment SelectByID(int segmentID);

        Segment SelectSegmentByName(string name);
    }
}
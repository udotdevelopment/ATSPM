using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class SegmentRepository : ISegmentRepository
    {
        private readonly Inrix db = new Inrix();


        public List<Segment> GetAll()
        {
            var slist = (from r in db.Segments
                select r).ToList();

            return slist;
        }

        public Segment SelectSegmentByName(string name)
        {
            var g = (from r in db.Segments
                where r.Segment_Name == name
                select r).FirstOrDefault();

            return g;
        }

        public void Remove(Segment segment)
        {
            db.Segments.Remove(segment);
            db.SaveChanges();
        }

        public Segment SelectByID(int segmentID)
        {
            var g = (from r in db.Segments
                where r.Segment_ID == segmentID
                select r).FirstOrDefault();

            return g;
        }

        public void RemoveByID(int segmentID)
        {
            var g = SelectByID(segmentID);

            db.Segments.Remove(g);
            db.SaveChanges();
        }

        public void Add(Segment segment)
        {
            db.Segments.Add(segment);
            db.SaveChanges();
        }

        public void Update(int segmentID, string newSegmentName, string newSegmentDescription)
        {
            var g = (from r in db.Segments
                where r.Segment_ID == segmentID
                select r).FirstOrDefault();


            if (g != null)
            {
                var newSegment = new Segment();
                newSegment.Segment_ID = g.Segment_ID;
                newSegment.Segment_Name = newSegmentName;
                newSegment.Segment_Description = newSegmentDescription;

                db.Entry(g).CurrentValues.SetValues(newSegment);
                db.SaveChanges();
            }
        }
    }
}
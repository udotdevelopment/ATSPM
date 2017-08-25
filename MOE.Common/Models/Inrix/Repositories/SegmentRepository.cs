using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class SegmentRepository: ISegmentRepository
    {
        private Models.Inrix.Inrix db = new Inrix();


        public List<Models.Inrix.Segment> GetAll()
        {
            List<Models.Inrix.Segment> slist = (from r in db.Segments
                                              select r).ToList();

            return slist;
        }

        public Models.Inrix.Segment SelectSegmentByName(string name)
        {
            var g = (from r in db.Segments
                                    where r.Segment_Name == name
                                    select r).FirstOrDefault();

            return g;
        }

        public void Remove(Models.Inrix.Segment segment)
        {
            db.Segments.Remove(segment);
            db.SaveChanges();
        }

        public Models.Inrix.Segment SelectByID(int segmentID)
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

        public void Add(Models.Inrix.Segment segment)
        {
            db.Segments.Add(segment);
            db.SaveChanges();
        }

        public void Update(int segmentID, string newSegmentName, string newSegmentDescription)
        {
            Models.Inrix.Segment g = (from r in db.Segments
                                    where r.Segment_ID == segmentID
                                    select r).FirstOrDefault();

            

            
            if (g != null)
            {
                Models.Inrix.Segment newSegment = new Segment();
                newSegment.Segment_ID = g.Segment_ID;
                newSegment.Segment_Name = newSegmentName;
                newSegment.Segment_Description = newSegmentDescription;

                db.Entry(g).CurrentValues.SetValues(newSegment);
                db.SaveChanges();
            }

            
        }

        
    }
}

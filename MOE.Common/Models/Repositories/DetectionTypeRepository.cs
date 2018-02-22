using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class DetectionTypeRepository : IDetectionTypeRepository
    {
        private readonly SPM db = new SPM();


        public List<DetectionType> GetAllDetectionTypes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var detectionTypes = (from r in db.DetectionTypes
                select r).ToList();

            return detectionTypes;
        }

        public List<DetectionType> GetAllDetectionTypesNoBasic()
        {
            var detectionTypes = (from r in db.DetectionTypes
                where r.Description != "Basic"
                select r).ToList();

            return detectionTypes;
        }

        //public List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> GetAllDetectionTypesWithSupportedMetricAbbreviations()
        //{

        //    db.Configuration.LazyLoadingEnabled = false;
        //    List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> detectionTypes = (from d in db.DetectionTypes
        //                          join m in db.MetricTypes on d.DetectionTypeID equals m.DetectionTypeID into a
        //                          select new DetectetorWithMetricAbbreviation
        //                          {
        //                             DetectionTypeID = d.DetectionTypeID,
        //                             Description = d.Description,
        //                             Abreviaiton = a.Select(x => x.Abbreviation).ToList()

        //                          }).ToList()

        //                          ;

        //    return detectionTypes;
        //}


        public DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID)
        {
            var detectionType = from r in db.DetectionTypes
                where r.DetectionTypeID == detectionTypeID
                select r;

            return detectionType.FirstOrDefault();
        }

        public void Update(DetectionType detectionType)
        {
            var g = (from r in db.DetectionTypes
                where r.DetectionTypeID == detectionType.DetectionTypeID
                select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(detectionType);
                db.SaveChanges();
            }
            else
            {
                db.DetectionTypes.Add(detectionType);
                db.SaveChanges();
            }
        }

        public void Remove(DetectionType detectionType)
        {
            var g = (from r in db.DetectionTypes
                where r.DetectionTypeID == detectionType.DetectionTypeID
                select r).FirstOrDefault();
            if (g != null)
            {
                db.DetectionTypes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(DetectionType detectionType)
        {
            var g = (from r in db.DetectionTypes
                where r.DetectionTypeID == detectionType.DetectionTypeID
                select r).FirstOrDefault();
            if (g == null)
            {
                db.DetectionTypes.Add(g);
                db.SaveChanges();
            }
        }

        public List<DetectionType> GetDetectionTypeByDetectionTypeIDs(List<int> detectionTypeIDs)
        {
            return (from r in db.DetectionTypes
                where detectionTypeIDs.Contains(r.DetectionTypeID)
                select r).ToList();
        }

        public class DetectetorWithMetricAbbreviation
        {
            public int DetectionTypeID { get; set; }
            public string Description { get; set; }
            public List<string> Abreviaiton { get; set; }
        }
    }
}
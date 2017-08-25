using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectionTypeRepository : IDetectionTypeRepository
    {
        Models.SPM db = new SPM();


        public List<Models.DetectionType> GetAllDetectionTypes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.DetectionType> detectionTypes = (from r in db.DetectionTypes
                                         select r).ToList();

            return detectionTypes;
        }

        public List<Models.DetectionType> GetAllDetectionTypesNoBasic()
        {
            List<Models.DetectionType> detectionTypes = (from r in db.DetectionTypes
                                                         where r.Description != "Basic"
                                                         select r).ToList();

            return detectionTypes;
        }

        public class DetectetorWithMetricAbbreviation
        {
            public int DetectionTypeID { get; set; }
            public string Description { get; set; }
            public List<String> Abreviaiton { get; set; }

            
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



        public Models.DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID)
        {
            var detectionType = (from r in db.DetectionTypes
                         where r.DetectionTypeID == detectionTypeID
                         select r);

            return detectionType.FirstOrDefault();
        }

        public List<Models.DetectionType> GetDetectionTypeByDetectionTypeIDs(List<int> detectionTypeIDs)
        {
           return (from r in db.DetectionTypes
                                 where detectionTypeIDs.Contains(r.DetectionTypeID)
                                 select r).ToList();
        }

        public void Update(MOE.Common.Models.DetectionType detectionType)
        {


            MOE.Common.Models.DetectionType g = (from r in db.DetectionTypes
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

        public void Remove(MOE.Common.Models.DetectionType detectionType)
        {


            MOE.Common.Models.DetectionType g = (from r in db.DetectionTypes
                                         where r.DetectionTypeID == detectionType.DetectionTypeID
                                         select r).FirstOrDefault();
            if (g != null)
            {
                db.DetectionTypes.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.DetectionType detectionType)
        {


            MOE.Common.Models.DetectionType g = (from r in db.DetectionTypes
                                         where r.DetectionTypeID == detectionType.DetectionTypeID
                                         select r).FirstOrDefault();
            if (g == null)
            {
                db.DetectionTypes.Add(g);
                db.SaveChanges();
            }

        }

    }
}

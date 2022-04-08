using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class DetectionTypeRepository : IDetectionTypeRepository
    {
        private MOEContext _db;

        public DetectionTypeRepository(MOEContext context)
        {
            _db = context;
        }

        public List<DetectionType> GetAllDetectionTypes()
        {
            //_db.Configuration.LazyLoadingEnabled = false;
            var detectionTypes = (from r in _db.DetectionTypes
                                  select r).ToList();

            return detectionTypes;
        }

        public List<DetectionType> GetAllDetectionTypesNoBasic()
        {
            var detectionTypes = (from r in _db.DetectionTypes
                                  where r.Description != "Basic"
                                  select r).ToList();

            return detectionTypes;
        }

        //public List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> GetAllDetectionTypesWithSupportedMetricAbbreviations()
        //{

        //    _db.Configuration.LazyLoadingEnabled = false;
        //    List<Models.Repositories.DetectionTypeRepository.DetectetorWithMetricAbbreviation> detectionTypes = (from d in _db.DetectionTypes
        //                          join m in _db.MetricTypes on d.DetectionTypeId equals m.DetectionTypeId into a
        //                          select new DetectetorWithMetricAbbreviation
        //                          {
        //                             DetectionTypeId = d.DetectionTypeId,
        //                             Description = d.Description,
        //                             Abreviaiton = a.Select(x => x.Abbreviation).ToList()

        //                          }).ToList()

        //                          ;

        //    return detectionTypes;
        //}


        public DetectionType GetDetectionTypeByDetectionTypeID(int detectionTypeID)
        {
            var detectionType = from r in _db.DetectionTypes
                                where r.DetectionTypeId == detectionTypeID
                                select r;

            return detectionType.FirstOrDefault();
        }

        public void Update(DetectionType detectionType)
        {
            var g = (from r in _db.DetectionTypes
                     where r.DetectionTypeId == detectionType.DetectionTypeId
                     select r).FirstOrDefault();
            if (g != null)
            {
                _db.Entry(g).CurrentValues.SetValues(detectionType);
                _db.SaveChanges();
            }
            else
            {
                _db.DetectionTypes.Add(detectionType);
                _db.SaveChanges();
            }
        }

        public void Remove(DetectionType detectionType)
        {
            var g = (from r in _db.DetectionTypes
                     where r.DetectionTypeId == detectionType.DetectionTypeId
                     select r).FirstOrDefault();
            if (g != null)
            {
                _db.DetectionTypes.Remove(g);
                _db.SaveChanges();
            }
        }

        public void Add(DetectionType detectionType)
        {
            var g = (from r in _db.DetectionTypes
                     where r.DetectionTypeId == detectionType.DetectionTypeId
                     select r).FirstOrDefault();
            if (g == null)
            {
                _db.DetectionTypes.Add(g);
                _db.SaveChanges();
            }
        }

        public List<DetectionType> GetDetectionTypeByDetectionTypeIDs(List<int> detectionTypeIDs)
        {
            return (from r in _db.DetectionTypes
                    where detectionTypeIDs.Contains(r.DetectionTypeId)
                    select r).ToList();
        }

        public class DetectetorWithMetricAbbreviation
        {
            public int DetectionTypeId { get; set; }
            public string Description { get; set; }
            public List<string> Abreviaiton { get; set; }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryApproachRepository : IApproachRepository
    {
        public InMemoryMOEDatabase _db = new InMemoryMOEDatabase();


        public void AddOrUpdate(Approach approach)
        {

            MOE.Common.Models.Approach g = (from r in _db.Approaches
                where r.ApproachID == approach.ApproachID
                select r).FirstOrDefault();
            if (g != null)
            {

                _db.Approaches.Remove(g);
                _db.Approaches.Add(approach);

            }
            else
            {

                //foreach (Detector d in approach.Detectors)
                //{
                //    if (d.DetectionTypes == null && d.DetectionTypeIDs != null)
                //    {
                //        d.DetectionTypes = _db.DetectionTypes
                //            .Where(dt => d.DetectionTypeIDs.Contains(dt.DetectionTypeID)).ToList();
                //    }
                //}
                _db.Approaches.Add(approach);

            }
        }

        public Approach FindAppoachByVersionIdPhaseOverlapAndDirection(int versionId, int phaseNumber, bool isOverlap, int directionTypeId)
        {
            throw new NotImplementedException();
        }

        public List<Approach> GetAllApproaches()
        {
            
            List<Common.Models.Approach> approaches = (from r in _db.Approaches
                select r).ToList();

            if (approaches.Count == 0)
            {
                Exception ex = new Exception("There were no records in this Query");
                throw (ex);
            }
            return approaches;
        }

        public Approach GetApproachByApproachID(int approachId)
        {
            var approach = (from r in _db.Approaches
                where r.ApproachID == approachId
                select r).FirstOrDefault(); ;

            if (approach != null)
            {
                return approach;
            }
            return null;
        }

        public void Remove(Approach approach)
        {
            throw new NotImplementedException();
        }

        public void Remove(int approachID)
        {
            throw new NotImplementedException();
        }
    }
}

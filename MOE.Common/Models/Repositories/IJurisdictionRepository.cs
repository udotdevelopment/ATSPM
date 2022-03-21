using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IJurisdictionRepository
    {
        List<Jurisdiction> GetAllJurisdictions();
        Jurisdiction GetJurisdictionByID(int jurisdictionID);
        Jurisdiction GetJurisdictionByName(string jurisdictionName);
        void DeleteByID(int jurisdictionID);
        void Update(Jurisdiction jurisdiction);
        void Add(Jurisdiction newJurisdiction);
        Jurisdiction GetJurisdictionByIDAndDate(int jurisdictionId, DateTime startDate);
    }
}
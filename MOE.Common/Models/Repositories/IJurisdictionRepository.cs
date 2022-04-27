using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IJurisdictionRepository
    {
        List<Jurisdiction> GetAllJurisdictions();
        Jurisdiction GetJurisdictionByID(int jurisdictionId);
        Jurisdiction GetJurisdictionByName(string jurisdictionName);
        void DeleteByID(int jurisdictionId);
        void Update(Jurisdiction newJurisdiction);
        void Add(Jurisdiction newJurisdiction);
    }
}
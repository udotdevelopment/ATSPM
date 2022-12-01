using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IAggregationRepositoryBase 
    {
        DateTime? GetLastAggregationDate();
    }
}
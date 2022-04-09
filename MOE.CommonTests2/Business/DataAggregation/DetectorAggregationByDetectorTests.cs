using Xunit;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Models.Repositories;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation.Tests
{
    public class DetectorAggregationByDetectorTests

    {
        
    }
}

public class DetectorEventCountAggregationRepositoryTest : IDetectorEventCountAggregationRepository
{
    public List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(int detectorId, DateTime start, DateTime end)
    {
        var random = new Random();
        List<DetectorEventCountAggregation> fake = new List<DetectorEventCountAggregation>();
        for(DateTime tempStart = start; tempStart <= end; tempStart = tempStart.AddMinutes(15))
        {
            fake.Add(new DetectorEventCountAggregation { ApproachId = 1, BinStartTime = tempStart, DetectorPrimaryId = detectorId, SignalId = "4028", EventCount = random.Next(0, 500) });
        }
        return fake;
    }

    public int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }
}
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IApproachSplitFailAggregationRepository
    {
        int GetApproachSplitFailCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        ApproachSplitFailAggregation Add(ApproachSplitFailAggregation priorityAggregation);
        void Update(ApproachSplitFailAggregation priorityAggregation);
        void Remove(ApproachSplitFailAggregation priorityAggregation);

        List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationBySignalIdPhaseDateRange(string signalId, int approachID,
            int phase, DateTime startDate, DateTime endDate);
        bool Exists(string signalId, int phaseNumber, DateTime dateTime1, DateTime dateTime2);
    }
}
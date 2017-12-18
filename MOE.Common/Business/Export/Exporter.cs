using System.Collections;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using MOE.Common.Models;

namespace MOE.Common.Business.Export
{
    public class Exporter
    {
        public Exporter(IEnumerable records, string filePath)
        {
            using (var csv = new CsvWriter(new StreamWriter(filePath)))
            {
                
                csv.Configuration.RegisterClassMap<ApproachCycleAggregation.ApproachCycleAggregationClassMap>();
                csv.Configuration.RegisterClassMap<ApproachPcdAggregation.ApproachPcdAggregationClassMap>();
                csv.Configuration.RegisterClassMap<ApproachSpeedAggregation.ApproachSpeedAggregationClassMap>();
                csv.Configuration.RegisterClassMap<ApproachSplitFailAggregation.ApproachSplitFailAggregationClassMap>();
                csv.Configuration.RegisterClassMap<ApproachYellowRedActivationAggregation.ApproachYellowRedActivationAggregationClassMap>();
                csv.Configuration.RegisterClassMap<DetectorAggregation.DetectorAggregationClassMap>();
                csv.Configuration.RegisterClassMap<PreemptionAggregation.PreemptionAggregationClassMap>();
                csv.Configuration.RegisterClassMap<PriorityAggregation.PriorityAggregationClassMap>();





                csv.WriteRecords(records);
                
            }
        }


    }
}
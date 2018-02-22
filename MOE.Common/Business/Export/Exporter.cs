using System.Collections;
using System.IO;
using CsvHelper;
using MOE.Common.Models;

namespace MOE.Common.Business.Export
{
    public class Exporter
    {
        public static byte[] GetCsvFile(IEnumerable records)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(streamWriter))
                {
                    //using (var csv = new CsvWriter(new StreamWriter(filePath)))
                    //{

                    csv.Configuration.RegisterClassMap<ApproachCycleAggregation.ApproachCycleAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachPcdAggregation.ApproachPcdAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachSpeedAggregation.ApproachSpeedAggregationClassMap>();
                    csv.Configuration
                        .RegisterClassMap<ApproachSplitFailAggregation.ApproachSplitFailAggregationClassMap>();
                    csv.Configuration
                        .RegisterClassMap<ApproachYellowRedActivationAggregation.
                            ApproachYellowRedActivationAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<DetectorAggregation.DetectorAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PreemptionAggregation.PreemptionAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PriorityAggregation.PriorityAggregationClassMap>();
                    csv.WriteRecords(records);
                }
                return memoryStream.ToArray();
            }
        }
    }
}
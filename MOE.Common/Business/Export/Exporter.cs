using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

                    csv.Configuration.RegisterClassMap<PhaseCycleAggregation.ApproachCycleAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachPcdAggregation.ApproachPcdAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachSpeedAggregation.ApproachSpeedAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachSplitFailAggregation.ApproachSplitFailAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachYellowRedActivationAggregation.ApproachYellowRedActivationAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<DetectorEventCountAggregation.DetectorEventCountAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PreemptionAggregation.PreemptionAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PriorityAggregation.PriorityAggregationClassMap>();
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] GetCsvFileForControllerEventLogs(List<Controller_Event_Log> records)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(streamWriter))
                {
                    csv.Configuration.RegisterClassMap<Controller_Event_Log.ControllerEventLogClassMap>();
                    csv.WriteHeader<Controller_Event_Log>();
                    csv.NextRecord();
                    foreach (var record in records)
                    {
                        csv.WriteField(record.SignalID);
                        csv.WriteField(record.Timestamp.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                        csv.WriteField(record.EventCode);
                        csv.WriteField(record.EventParam);
                        csv.NextRecord();
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
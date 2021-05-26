using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Parquet;

namespace MOE.Common.Business.Parquet
{
    public static class ParquetArchive
    {
        public static List<Controller_Event_Log> GetDataFromArchive(string localPath, string signalId,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localPath)) return new List<Controller_Event_Log>();
                var dateRange = startTime.Date == endTime.Date
                    ? new List<DateTime> {startTime.Date}
                    : GetDateRange(startTime, endTime);

                var events = new List<Controller_Event_Log>();
                foreach (var date in dateRange)
                {
                    if (File.Exists(
                        $"{localPath}\\date={date.Date:yyyy-MM-dd}\\{signalId}_{date.Date:yyyy-MM-dd}.parquet"))
                    {
                        using (var stream =
                            File.OpenRead(
                                $"{localPath}\\date={date.Date:yyyy-MM-dd}\\{signalId}_{date.Date:yyyy-MM-dd}.parquet"))
                        {
                            var newEvents = ParquetConvert.Deserialize<ParquetEventLog>(stream);
                            foreach (var parquetEvent in newEvents)
                            {
                                events.Add(new Controller_Event_Log
                                {
                                    SignalID = parquetEvent.SignalID,
                                    Timestamp = date.Date.AddMilliseconds(parquetEvent.TimestampMs),
                                    EventCode = parquetEvent.EventCode,
                                    EventParam = parquetEvent.EventParam
                                });
                            }
                        }
                    }
                    else
                    {
                        var logRepository = ApplicationEventRepositoryFactory.Create();
                        var e = new ApplicationEvent
                        {
                            ApplicationName = "MOE.Common",
                            Class = "ParquetArchive",
                            Function = "GetDataFromArchive",
                            SeverityLevel = ApplicationEvent.SeverityLevels.High,
                            Description =
                                $"File {localPath}\\date={date.Date:yyyy-MM-dd}\\{signalId}_{date.Date:yyyy-MM-dd}.parquet does not exist",
                            Timestamp = DateTime.Now
                        };
                        logRepository.Add(e);
                        return new List<Controller_Event_Log>();
                    }

                    return events.Where(x => x.Timestamp >= startTime && x.Timestamp < endTime).ToList();
                }

                return events.Where(x => x.Timestamp >= startTime && x.Timestamp < endTime).ToList();
            }
            catch (Exception ex)
            {
                var logRepository = ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent
                {
                    ApplicationName = "MOE.Common",
                    Class = "ParquetArchive",
                    Function = "GetDataFromArchive",
                    SeverityLevel = ApplicationEvent.SeverityLevels.High,
                    Description = ex.Message,
                    Timestamp = DateTime.Now
                };
                logRepository.Add(e);
                return new List<Controller_Event_Log>();
            }
        }

        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("EndDate must be greater than or equal to StartDate");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

        public static string GetSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}

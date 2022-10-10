using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Azure.Storage.Blobs;
using Google.Cloud.Storage.V1;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Parquet;

namespace MOE.Common.Business.Parquet
{
    public static class ParquetArchive
    {
        private static readonly string Destination = ConfigurationManager.AppSettings["StorageLocation"];
        private static readonly string GoogleBucketName = ConfigurationManager.AppSettings["BucketName"];
        private static readonly string FolderName = ConfigurationManager.AppSettings["FolderName"];
        private static readonly string AwsBucketName = ConfigurationManager.AppSettings["S3_BUCKETNAME"];
        private static readonly string AwsAccessKey = ConfigurationManager.AppSettings["S3_ACCESSKEY"];
        private static readonly string AwsSecretKey = ConfigurationManager.AppSettings["S3_SECRETKEY"];
        private static readonly string Container = ConfigurationManager.AppSettings["AZURE_CONTAINER"];

        public static List<Controller_Event_Log> GetDataFromArchive(string localPath, string signalId,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localPath)) return new List<Controller_Event_Log>();
                var dateRange = startTime.Date == endTime.Date
                    ? new List<DateTime> { startTime.Date }
                    : GetDateRange(startTime, endTime);

                var events = new List<Controller_Event_Log>();

                switch (Destination)
                {
                    case "0":
                        events = GetEventsFromLocalFile(localPath, signalId, dateRange);
                        break;
                    case "1":
                        events = GetEventsFromGoogleCloud(signalId, dateRange);
                        break;
                    case "2":
                        events = GetEventsFromAws(signalId, dateRange);
                        break;
                    case "3":
                        events = GetEventsFromAzure(signalId, dateRange);
                        break;
                    default:
                        return events;
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

        private static List<Controller_Event_Log> GetEventsFromAzure(string signalId, IEnumerable<DateTime> dateRange)
        {
            var events = new List<Controller_Event_Log>();


            var blobServiceClient = new BlobServiceClient(ConfigurationManager.AppSettings["AZURE_CONN_STRING"]);
            var container = blobServiceClient.GetBlobContainerClient(Container);

            foreach (var date in dateRange)
            {
                var fileName = $"{FolderName}date={date:yyyy-MM-dd}/{signalId}_{date:yyyy-MM-dd}.parquet";
                var blobClient = container.GetBlobClient(fileName);
                if (blobClient.Exists())
                {
                    using (var ms = new MemoryStream())
                    {
                        blobClient.DownloadTo(ms);
                        var newEvents = ParquetConvert.Deserialize<ParquetEventLog>(ms);
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
            }

            return events;
        }

        private static List<Controller_Event_Log> GetEventsFromAws(string signalId, IEnumerable<DateTime> dateRange)
        {
            var bucketRegion = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["S3_REGION"]);
            var events = new List<Controller_Event_Log>();
            using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretKey, bucketRegion))
            {
                foreach (var date in dateRange)
                {
                    var fileName = $"{FolderName}date={date:yyyy-MM-dd}/{signalId}_{date:yyyy-MM-dd}.parquet";

                    var info = new S3FileInfo(client, AwsBucketName, fileName);
                    if (info.Exists)
                    {
                        var request = new GetObjectRequest
                        {
                            BucketName = AwsBucketName,
                            Key = fileName
                        };
                        var response = client.GetObject(request);

                        using (var ms = new MemoryStream())
                        {
                            response.ResponseStream.CopyTo(ms);
                            var newEvents = ParquetConvert.Deserialize<ParquetEventLog>(ms);
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
                }
            }

            return events;
        }

        private static List<Controller_Event_Log> GetEventsFromGoogleCloud(string signalId, IEnumerable<DateTime> dateRange)
        {
            var events = new List<Controller_Event_Log>();
            var storage = StorageClient.Create();
            foreach (var date in dateRange)
            {
                var objName = $"{FolderName}date={date:yyyy-MM-dd}/{signalId}_{date:yyyy-MM-dd}.parquet";
                var obj = storage.ListObjects(GoogleBucketName, objName);
                if (obj.Any())
                {
                    using (var ms = new MemoryStream())
                    {
                        storage.DownloadObject(GoogleBucketName, objName, ms);
                        var newEvents = ParquetConvert.Deserialize<ParquetEventLog>(ms);
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
            }

            return events;
        }

        private static List<Controller_Event_Log> GetEventsFromLocalFile(string localPath, string signalId, IEnumerable<DateTime> dateRange)
        {
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
                }
            }

            return events;
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

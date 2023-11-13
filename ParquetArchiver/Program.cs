using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Azure.Storage.Blobs;
using MOE.Common.Business.Parquet;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Parquet;
using Parquet.Data;
using MailMessage = System.Net.Mail.MailMessage;
using Google.Cloud.Storage.V1;

namespace ParquetArchiver
{
    class Program
    {
        private static readonly string FolderName = ConfigurationManager.AppSettings["FolderName"];
        private static readonly string GoogleBucketName = ConfigurationManager.AppSettings["BucketName"];
        private static readonly string Container = ConfigurationManager.AppSettings["AZURE_CONTAINER"];
        private static readonly string FilePathPrefix = ConfigurationManager.AppSettings["FILE_PATH_PREFIX"];

        private static readonly string AccessKey = ConfigurationManager.AppSettings["S3_ACCESSKEY"];
        private static readonly string SecretKey = ConfigurationManager.AppSettings["S3_SECRETKEY"];
        private static readonly string BucketName = ConfigurationManager.AppSettings["S3_BUCKETNAME"];
        private static readonly string StorageLocation = ConfigurationManager.AppSettings["StorageLocation"];

        private const string MAX_DEGREES_OF_PARALLELISM = "MaxDegreesOfParallelism";
        private const string START_DATE = "StartDate";
        private const string END_DATE = "EndDate";
        private const string LOCAL_ARCHIVE_DIRECTORY = "LocalArchiveDirectory";
        private const string USE_START_AND_END_DATES = "UseStartAndEndDates";
        private const string DAYS_AGO_TO_RUN = "DaysAgoToRun";
        private const string SEND_COMPLETION_EMAIL = "SendCompletionEmail";
        private const string EMAIL_LIST = "EmailList";
        private const string SMTP_SERVER = "SmtpServer";
        private const string FROM_EMAIL = "FromEmail";
        private const string FROM_EMAIL_PASSWORD = "FromEmailPassword";
        private const string USE_SSL = "UseSSL";
        private const string DAYS_BACK_TO_CHECK = "DaysBackToCheck";

        private static string _missingDaysStr = "Days missing from archive to be retried:\n";
        private static bool _wereDaysMissing = false;

        static async Task Main(string[] args)
        {
            if (StorageLocation == "-1")
                return;

            //var db = new SPM();

            //var signalsRepository = SignalsRepositoryFactory.Create(db);
            //var signals = signalsRepository.GetLatestVersionOfAllSignals();
            DateTime start;
            DateTime end;
            bool useStartAndEndDates;

            try
            {
                useStartAndEndDates = int.Parse(ParquetArchive.GetSetting(USE_START_AND_END_DATES)) == 1;
            }
            catch (Exception)
            {
                WriteToLog("UseStartAndEndDates was not a valid value. Values are 0 or 1.");
                return;
            }

            if (useStartAndEndDates)
            {
                try
                {
                    start = DateTime.Parse(ParquetArchive.GetSetting(START_DATE));
                }
                catch (Exception)
                {
                    WriteToLog("StartDate not in a valid format. Format must be mm/dd/yyyy");
                    return;
                }

                try
                {
                    end = DateTime.Parse(ParquetArchive.GetSetting(END_DATE));
                }
                catch (Exception)
                {
                    WriteToLog("EndDate not in a valid format. Format must be mm/dd/yyyy");
                    return;
                }
            }
            else
            {
                try
                {
                    var daysToKeep = int.Parse(ParquetArchive.GetSetting(DAYS_AGO_TO_RUN));
                    start = DateTime.Today.AddDays(-daysToKeep);
                    end = start;

                }
                catch (Exception)
                {
                    WriteToLog("DaysToKeep was not a whole number");
                    return;
                }

            }

            var totalWatch = new Stopwatch();
            totalWatch.Start();

            var dateList = GetDateRange(start, end).ToList();
            if (!useStartAndEndDates)
            {
                var missingDaysToAdd = CheckForMissingDays(start);
                dateList.AddRange(missingDaysToAdd);
            }

            await Archive(dateList, StorageLocation);

            totalWatch.Stop();
            WriteToLog($"All data converted in {totalWatch.ElapsedMilliseconds / 1000} seconds");

            try
            {
                SendEmail(dateList);
            }
            catch (Exception ex)
            {
                WriteToLog("Error sending email");
                WriteToLog(ex.Message);
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }

            if (Environment.UserInteractive)
                Console.ReadLine();

        }

        private static async Task Archive(List<DateTime> dateList, string destination)
        {
            var semaphore = new SemaphoreSlim(Convert.ToInt32(ParquetArchive.GetSetting(MAX_DEGREES_OF_PARALLELISM)));
            foreach (var date in dateList)
            {
                try
                {
                    WriteToLog("Getting distinct signals in event log table.");
                    var eventLogRepo = ControllerEventLogRepositoryFactory.Create();
                    var signalsToArchive = eventLogRepo.GetSignalIdsInControllerEventLog(date, date + TimeSpan.FromDays(1));
                    WriteToLog($"{signalsToArchive.Count} signals found on {date.ToLongDateString()}.");
                    
                    var tasks = signalsToArchive.Select(async signal =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            var watch = new Stopwatch();
                            watch.Start();
                            Console.WriteLine(
                                $"Started Writing {signal}: for {date.ToShortDateString()}");

                            var eventLogRepository = ControllerEventLogRepositoryFactory.Create();
                            var events = eventLogRepository.GetSignalEventsBetweenDates(signal, date, date.AddDays(1));
                            if (events.Any())
                            {
                                switch (destination)
                                {
                                    case "0":
                                        SaveToLocalStorage(signal, events, date);
                                        break;
                                    case "1":
                                        await SaveToGoogleCloud(signal, events, date);
                                        break;
                                    case "2":
                                        await SaveToAws(signal, events, date);
                                        break;
                                    case "3":
                                        await SaveToAzure(signal, events, date);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid storage location specified, returning");
                                        return;
                                }
                            }

                            watch.Stop();
                            Console.WriteLine(
                                $"Finished writing {signal}: for {date.ToShortDateString()} in {watch.ElapsedMilliseconds / 1000} seconds");
                        }
                        catch (Exception ex)
                        {
                            WriteToLog($"Error archiving {signal} on {date.ToLongDateString()}: {ex.Message}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    await Task.WhenAll(tasks);

                    WriteToLog($"Data conversion complete for {date.ToLongDateString()}");
                }
                catch (Exception ex)
                {
                    WriteToLog($"Error archiving {date.ToLongDateString()}: {ex.Message}");
                }
            }
        }

        private static void SaveToLocalStorage(string signal, List<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                var localPath = ParquetArchive.GetSetting(LOCAL_ARCHIVE_DIRECTORY);

                if (!Directory.Exists($"{localPath}\\{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}"))
                    Directory.CreateDirectory(
                        $"{localPath}\\{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}");

                using (var stream =
                       File.Create(
                           $"{localPath}\\{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}\\{signal}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet")
                      )
                {
                    var parquetEvents = ConvertToParquetEventLogList(events);

                    var signalIdColumn = new DataColumn(new DataField<string>("SignalID"),
                        parquetEvents.Select(x => x.SignalID).ToArray());
                    var dateColumn = new DataColumn(new DataField<string>("Date"),
                        parquetEvents.Select(x => x.Date).ToArray());
                    var timestampColumn = new DataColumn(new DataField<double>("TimestampMs"),
                        parquetEvents.Select(x => x.TimestampMs).ToArray());

                    var eventCodeColumn = new DataColumn(new DataField<int>("EventCode"),
                        parquetEvents.Select(x => x.EventCode).ToArray());
                    var eventParamColumn = new DataColumn(new DataField<int>("EventParam"),
                        parquetEvents.Select(x => x.EventParam).ToArray());

                    var schema = new Schema(signalIdColumn.Field, dateColumn.Field, timestampColumn.Field,
                        eventCodeColumn.Field, eventParamColumn.Field);
                    using (var parquetWriter = new ParquetWriter(schema, stream))
                    {
                        try
                        {
                            //create a new row group in the file
                            using (var groupWriter = parquetWriter.CreateRowGroup())
                            {
                                groupWriter.WriteColumn(signalIdColumn);
                                groupWriter.WriteColumn(dateColumn);
                                groupWriter.WriteColumn(timestampColumn);
                                groupWriter.WriteColumn(eventCodeColumn);
                                groupWriter.WriteColumn(eventParamColumn);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Sleep and retry
                            WriteToLog(
                                $"Initial save: Error saving {signal} on {date.ToShortDateString()}");
                            WriteToLog($"{ex.Message}");
                            if (ex.InnerException?.Message != null)
                            {
                                WriteToLog(ex.InnerException?.Message);
                            }

                            Thread.Sleep(10000);

                            try
                            {
                                // create a new row group in the file
                                using (ParquetRowGroupWriter groupWriter2 = parquetWriter.CreateRowGroup())
                                {
                                    groupWriter2.WriteColumn(signalIdColumn);
                                    groupWriter2.WriteColumn(dateColumn);
                                    groupWriter2.WriteColumn(timestampColumn);
                                    groupWriter2.WriteColumn(eventCodeColumn);
                                    groupWriter2.WriteColumn(eventParamColumn);
                                }
                            }
                            catch (Exception ex2)
                            {
                                WriteToLog(
                                    $"Retry: Error saving {signal} on {date.ToShortDateString()}");
                                WriteToLog($"{ex2.Message}");
                                if (ex2.InnerException?.Message != null)
                                {
                                    WriteToLog(ex2.InnerException?.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex.Message + "\n" + ex.InnerException?.Message);
            }

        }

        private static async Task SaveToGoogleCloud(string signal, List<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                // Needed if running on a server without google cloud sdk installed
                // Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ConfigurationManager.AppSettings["GoogleAppCredentialsLocation"]);
                var storage = await StorageClient.CreateAsync();

                var parquetEvents = ConvertToParquetEventLogList(events);

                using (var ms = new MemoryStream())
                {
                    ParquetConvert.Serialize(parquetEvents, ms);
                    ms.Position = 0;
                    var fileName = $"{FolderName}/{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}/{signal}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
                    await storage.UploadObjectAsync(GoogleBucketName, fileName, null, ms);
                }
            }
            catch (Exception ex)
            {
                WriteToLog($"Error saving {signal} on {date.ToShortDateString()}");
                WriteToLog($"{ex.Message}");
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }
        }

        private static async Task SaveToAws(string signal, IReadOnlyCollection<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var bucketRegion = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["S3_REGION"]);

                    var parquetEvents = ConvertToParquetEventLogList(events);
                    ParquetConvert.Serialize(parquetEvents, ms);
                    ms.Position = 0;
                    var fileName = $"{FolderName}{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}/{signal}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
                    using (var client = new AmazonS3Client(AccessKey, SecretKey, bucketRegion))
                    {
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = ms,
                            Key = fileName,
                            BucketName = BucketName
                        };
                        var fileTransferUtility = new TransferUtility(client);
                        await fileTransferUtility.UploadAsync(uploadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(
                $"Retry: Error saving {signal} on {date.ToShortDateString()}");
                WriteToLog($"{ex.Message}");
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }
        }

        private static async Task SaveToAzure(string signal, List<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var parquetEvents = ConvertToParquetEventLogList(events);
                    ParquetConvert.Serialize(parquetEvents, ms);
                    ms.Position = 0;
                    var fileName = $"{FolderName}{FilePathPrefix}={events.First().Timestamp.Date:yyyy-MM-dd}/{signal}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
                    var blobServiceClient = new BlobServiceClient(ConfigurationManager.AppSettings["AZURE_CONN_STRING"]);
                    var container = blobServiceClient.GetBlobContainerClient(Container);
                    var blobClient = container.GetBlobClient(fileName);
                    await blobClient.UploadAsync(ms, true);
                }
            }
            catch (Exception ex)
            {
                WriteToLog(
                $"Retry: Error saving {signal} on {date.ToShortDateString()}");
                WriteToLog($"{ex.Message}");
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }
        }

        private static IEnumerable<DateTime> CheckForMissingDays(DateTime start)
        {
            var daysBackToCheck = int.Parse(ParquetArchive.GetSetting(DAYS_BACK_TO_CHECK));
            if (daysBackToCheck == 0)
                return new List<DateTime>();
            WriteToLog($"Going back {daysBackToCheck} for missing days");
            var dateRange = GetDateRange(start.AddDays(-daysBackToCheck), start.AddDays(-1));
            switch (StorageLocation)
            {
                case "0":
                    return CheckLocalMissingFiles(dateRange);
                case "1":
                    return CheckGoogleMissingFiles(dateRange);
                case "2":
                    WriteToLog("Missing days for AWS Not implemented yet");
                    break;
                case "3":
                    WriteToLog("Missing days for Azure Not implemented yet");
                    break;
            }
            return new List<DateTime>();
        }

        private static List<DateTime> CheckLocalMissingFiles(IEnumerable<DateTime> dateRange)
        {
            WriteToLog("Checking local storage for missing days.");
            var localPath = ParquetArchive.GetSetting(LOCAL_ARCHIVE_DIRECTORY);
            var retVal = new List<DateTime>();

            foreach (var date in dateRange)
            {
                if (!Directory.Exists($"{localPath}\\{FilePathPrefix}={date.Date:yyyy-MM-dd}"))
                {
                    retVal.Add(date);
                    _missingDaysStr += $"{date.ToLongDateString()}\n";
                    _wereDaysMissing = true;
                    WriteToLog($"{date.ToLongDateString()} missing, adding to current run");
                }
            }

            return retVal;
        }

        private static List<DateTime> CheckGoogleMissingFiles(IEnumerable<DateTime> dateRange)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ConfigurationManager.AppSettings["GoogleAppCredentialsLocation"]);
            WriteToLog("Checking Google Cloud storage for missing days.");
            var storage = StorageClient.Create();
            var retVal = new List<DateTime>();
            foreach (var date in dateRange)
            {
                var storageObject = storage.ListObjects(GoogleBucketName).Where(x => x.Id.StartsWith($"{GoogleBucketName}/{FolderName}/{FilePathPrefix}={date.Date:yyyy-MM-dd}")).ToList();
                if (!storageObject.Any())
                {
                    retVal.Add(date);
                    _missingDaysStr += $"{date.ToLongDateString()}\n";
                    _wereDaysMissing = true;
                    WriteToLog($"{date.ToLongDateString()} missing, adding to current run");
                }
            }
            return retVal;
        }

        private static List<ParquetEventLog> ConvertToParquetEventLogList(IEnumerable<Controller_Event_Log> events)
        {
            return events.Select(tempEvent => new ParquetEventLog
            {
                SignalID = tempEvent.SignalID,
                Date = tempEvent.Timestamp.ToShortDateString(),
                TimestampMs = tempEvent.Timestamp.TimeOfDay.TotalMilliseconds,
                EventCode = tempEvent.EventCode,
                EventParam = tempEvent.EventParam
            })
                .ToList();
        }

        private static void SendEmail(IEnumerable<DateTime> datesRun)
        {
            var sendCompletionEmail = int.Parse(ParquetArchive.GetSetting(SEND_COMPLETION_EMAIL)) == 1;
            if (!sendCompletionEmail) return;

            var emailList = ParquetArchive.GetSetting(EMAIL_LIST);
            if (!string.IsNullOrWhiteSpace(emailList))
            {
                var smtpServer = ParquetArchive.GetSetting(SMTP_SERVER);
                var useSsl = int.Parse(ParquetArchive.GetSetting(USE_SSL)) == 1;
                var fromEmail = ParquetArchive.GetSetting(FROM_EMAIL);

                var smtp = new SmtpClient(smtpServer)
                {
                    EnableSsl = useSsl
                };

                var password = ParquetArchive.GetSetting(FROM_EMAIL_PASSWORD);
                if (!string.IsNullOrWhiteSpace(password))
                    smtp.Credentials = new NetworkCredential(fromEmail, password);

                var mailMessage = new MailMessage();
                mailMessage.To.Add(emailList);
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.Subject = "Parquet Archive Run Completion";
                if (_wereDaysMissing)
                    mailMessage.Body = _missingDaysStr +
                                       $"\nThe Parquet Archiver has finished running for the following days:";
                else
                    mailMessage.Body = "The Parquet Archiver has finished running for the following days:";
                foreach (var row in datesRun)
                {
                    mailMessage.Body += $"\n{row.ToLongDateString()}";
                }

                smtp.Send(mailMessage);
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

        private static void WriteToLog(string message)
        {
            var localPath = ParquetArchive.GetSetting(LOCAL_ARCHIVE_DIRECTORY);
            using (var writer = File.AppendText($"{localPath}\\log.txt"))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
            Console.WriteLine(message);
        }
    }
}
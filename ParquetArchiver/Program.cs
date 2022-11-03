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
        private static readonly string Container = ConfigurationManager.AppSettings["AZURE_CONTAINER"];

        private static readonly string AccessKey = ConfigurationManager.AppSettings["S3_ACCESSKEY"];
        private static readonly string SecretKey = ConfigurationManager.AppSettings["S3_SECRETKEY"];
        private static readonly string BucketName = ConfigurationManager.AppSettings["S3_BUCKETNAME"];

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

        static void Main(string[] args)
        {
            var storageLocation = ConfigurationManager.AppSettings["StorageLocation"];
            if (storageLocation == "-1")
                return;

            var db = new SPM();

            var signalsRepository = SignalsRepositoryFactory.Create(db);
            var signals = signalsRepository.GetLatestVersionOfAllSignals();
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            bool useStartAndEndDates;

            try
            {
                useStartAndEndDates = int.Parse(ParquetArchive.GetSetting(USE_START_AND_END_DATES)) == 1;
            }
            catch (Exception)
            {
                Console.WriteLine("UseStartAndEndDates was not a valid value. Values are 0 or 1.");
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
                    Console.WriteLine("StartDate not in a valid format. Format must be mm/dd/yyyy");
                    return;
                }

                try
                {
                    end = DateTime.Parse(ParquetArchive.GetSetting(END_DATE));
                }
                catch (Exception)
                {
                    Console.WriteLine("EndDate not in a valid format. Format must be mm/dd/yyyy");
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
                    Console.WriteLine("DaysToKeep was not a whole number");
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

            var options = new ParallelOptions
            { MaxDegreeOfParallelism = Convert.ToInt32(ParquetArchive.GetSetting(MAX_DEGREES_OF_PARALLELISM)) };

            Archive(dateList, signals, options, storageLocation);

            totalWatch.Stop();
            Console.WriteLine($"All data converted in {totalWatch.ElapsedMilliseconds / 1000} seconds");

            try
            {
                SendEmail(dateList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email. Check the log for more information.");
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

        private static void Archive(List<DateTime> dateList, List<Signal> signals, ParallelOptions options, string destination)
        {
            foreach (var date in dateList)
            {
                WriteToLog($"Starting data conversion for {date.ToLongDateString()}");
                Console.WriteLine($"Starting data conversion for {date.ToLongDateString()}");
                Parallel.ForEach(signals, options, async signal =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    Console.WriteLine(
                        $"Started Writing {signal.SignalID}: {signal.PrimaryName} {signal.SecondaryName} for {date.ToShortDateString()}");

                    var eventLogRepository = ControllerEventLogRepositoryFactory.Create();
                    var events = eventLogRepository.GetSignalEventsBetweenDates(signal.SignalID, date, date.AddDays(1));
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
                        $"Finished writing {signal.SignalID}: {signal.PrimaryName} {signal.SecondaryName} for {date.ToShortDateString()} in {watch.ElapsedMilliseconds / 1000} seconds");
                });
                Console.WriteLine($"Data conversion complete for {date.ToLongDateString()}");
                WriteToLog($"Data conversion complete for {date.ToLongDateString()}");
            }
        }

        private static void SaveToLocalStorage(Signal signal, List<Controller_Event_Log> events, DateTime date)
        {
            var localPath = ParquetArchive.GetSetting(LOCAL_ARCHIVE_DIRECTORY);
            Console.WriteLine($"Data acquired for Signal {signal.SignalID}");

            if (!Directory.Exists($"{localPath}\\date={events.First().Timestamp.Date:yyyy-MM-dd}"))
                Directory.CreateDirectory(
                    $"{localPath}\\date={events.First().Timestamp.Date:yyyy-MM-dd}");

            using (var stream =
                   File.Create(
                       $"{localPath}\\date={events.First().Timestamp.Date:yyyy-MM-dd}\\{signal.SignalID}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet")
                  )
            {
                var parquetEvents = ConvertToParquetEventLogList(events);


                Console.WriteLine($"Data converted to ParquetEventLog for Signal {signal.SignalID}");

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
                        Console.WriteLine(ex.Message);
                        WriteToLog(
                            $"Initial save: Error saving {signal.SignalID} on {date.ToShortDateString()}");
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
                            Console.WriteLine(ex2.Message);
                            WriteToLog(
                                $"Retry: Error saving {signal.SignalID} on {date.ToShortDateString()}");
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

        private static async Task SaveToGoogleCloud(Signal signal, List<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ConfigurationManager.AppSettings["GoogleAppCredentialsLocation"]);
                var storage = StorageClient.Create();
                var bucketName = ConfigurationManager.AppSettings["BucketName"];
                
                Console.WriteLine($"Data acquired for Signal {signal.SignalID}");
                var parquetEvents = ConvertToParquetEventLogList(events);
                Console.WriteLine($"Data converted to ParquetEventLog for Signal {signal.SignalID}");
                using (var ms = new MemoryStream())
                {
                    ParquetConvert.Serialize(parquetEvents, ms);
                    ms.Position = 0;
                    var fileName = $"{FolderName}date={events.First().Timestamp.Date:yyyy-MM-dd}/{signal.SignalID}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
                    await storage.UploadObjectAsync(bucketName, fileName, null, ms);
                    Console.WriteLine($"{fileName} uploaded.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                WriteToLog(
                                $"Retry: Error saving {signal.SignalID} on {date.ToShortDateString()}");
                WriteToLog($"{ex.Message}");
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }
        }

        private static async Task SaveToAws(Signal signal, IReadOnlyCollection<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var bucketRegion = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["S3_REGION"]);
                    Console.WriteLine($"Data acquired for Signal {signal.SignalID}");
                    var parquetEvents = ConvertToParquetEventLogList(events);
                    Console.WriteLine($"Data converted to ParquetEventLog for Signal {signal.SignalID}");
                    ParquetConvert.Serialize(parquetEvents, ms);
                    Console.WriteLine("Uploading parquet file to S3 bucket.");
                    ms.Position = 0;
                    var fileName = $"{FolderName}date={events.First().Timestamp.Date:yyyy-MM-dd}/{signal.SignalID}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
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
                        Console.WriteLine($"Upload complete: {uploadRequest.Key}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                WriteToLog(
                $"Retry: Error saving {signal.SignalID} on {date.ToShortDateString()}");
                WriteToLog($"{ex.Message}");
                if (ex.InnerException?.Message != null)
                {
                    WriteToLog(ex.InnerException?.Message);
                }
            }
        }

        private static async Task SaveToAzure(Signal signal, List<Controller_Event_Log> events, DateTime date)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    Console.WriteLine($"Data acquired for Signal {signal.SignalID}");
                    var parquetEvents = ConvertToParquetEventLogList(events);
                    Console.WriteLine($"Data converted to ParquetEventLog for Signal {signal.SignalID}");
                    ParquetConvert.Serialize(parquetEvents, ms);
                    Console.WriteLine("Uploading parquet file to Azure.");
                    ms.Position = 0;
                    var fileName = $"{FolderName}date={events.First().Timestamp.Date:yyyy-MM-dd}/{signal.SignalID}_{events.First().Timestamp.Date:yyyy-MM-dd}.parquet";
                    var blobServiceClient = new BlobServiceClient(ConfigurationManager.AppSettings["AZURE_CONN_STRING"]);
                    var container = blobServiceClient.GetBlobContainerClient(Container);
                    var blobClient = container.GetBlobClient(fileName);
                    await blobClient.UploadAsync(ms, true);
                    Console.WriteLine($"Upload complete: {blobClient.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                WriteToLog(
                $"Retry: Error saving {signal.SignalID} on {date.ToShortDateString()}");
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
            var localPath = ParquetArchive.GetSetting(LOCAL_ARCHIVE_DIRECTORY);

            var dateRange = GetDateRange(start.AddDays(-daysBackToCheck), start.AddDays(-1));
            var retVal = new List<DateTime>();

            foreach (var date in dateRange)
            {
                if (!Directory.Exists($"{localPath}\\date={date.Date:yyyy-MM-dd}"))
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
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MOE.Common.Business.Parquet;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using Parquet;
using Parquet.Data;
using MailMessage = System.Net.Mail.MailMessage;

namespace ParquetArchiver
{
    class Program
    {
        private const string MaxDegreesOfParallelism = "MaxDegreesOfParallelism";
        private const string StartDate = "StartDate";
        private const string EndDate = "EndDate";
        private const string LocalArchiveDirectory = "LocalArchiveDirectory";
        private const string UseStartAndEndDates = "UseStartAndEndDates";
        private const string DaysAgoToRun = "DaysAgoToRun";
        private const string SendCompletionEmail = "SendCompletionEmail";
        private const string EmailList = "EmailList";
        private const string SmtpServer = "SmtpServer";
        private const string FromEmail = "FromEmail";
        private const string FromEmailPassword = "FromEmailPassword";
        private const string UseSSL = "UseSSL";
        private const string DaysBackToCheck = "DaysBackToCheck";

        private static string _missingDaysStr = "Days missing from archive to be retried:\n";
        private static bool _wereDaysMissing = false;

        static void Main(string[] args)
        {
            var db = new SPM();

            var signalsRepository = SignalsRepositoryFactory.Create(db);
            var signals = signalsRepository.GetLatestVersionOfAllSignals();
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            bool useStartAndEndDates;

            try
            {
                useStartAndEndDates = int.Parse(ParquetArchive.GetSetting(UseStartAndEndDates)) == 1;
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
                    start = DateTime.Parse(ParquetArchive.GetSetting(StartDate));
                }
                catch (Exception)
                {
                    Console.WriteLine("StartDate not in a valid format. Format must be mm/dd/yyyy");
                    return;
                }

                try
                {
                    end = DateTime.Parse(ParquetArchive.GetSetting(EndDate));
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
                    var daysToKeep = int.Parse(ParquetArchive.GetSetting(DaysAgoToRun));
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
                {MaxDegreeOfParallelism = Convert.ToInt32(ParquetArchive.GetSetting(MaxDegreesOfParallelism))};

            var localPath = ParquetArchive.GetSetting(LocalArchiveDirectory);

            foreach (var date in dateList)
            {
                WriteToLog($"Starting data conversion for {date.ToLongDateString()}");
                
                Parallel.ForEach(signals, options, signal =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    Console.WriteLine(
                        $"Started Writing {signal.SignalID}: {signal.PrimaryName} {signal.SecondaryName} for {date.ToShortDateString()}");

                    var eventLogRepository = ControllerEventLogRepositoryFactory.Create();
                    var events = eventLogRepository.GetSignalEventsBetweenDates(signal.SignalID, date, date.AddDays(1));
                    if (events.Any())
                    {
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

                    watch.Stop();
                    Console.WriteLine(
                        $"Finished writing {signal.SignalID}: {signal.PrimaryName} {signal.SecondaryName} for {date.ToShortDateString()} in {watch.ElapsedMilliseconds / 1000} seconds");
                });

                WriteToLog($"Data conversion complete for {date.ToLongDateString()}");
            }

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

        private static IEnumerable<DateTime> CheckForMissingDays(DateTime start)
        {
            var daysBackToCheck = int.Parse(ParquetArchive.GetSetting(DaysBackToCheck));
            if (daysBackToCheck == 0)
                return new List<DateTime>();
            WriteToLog($"Going back {daysBackToCheck} for missing days");
            var localPath = ParquetArchive.GetSetting(LocalArchiveDirectory);

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
            var sendCompletionEmail = int.Parse(ParquetArchive.GetSetting(SendCompletionEmail)) == 1;
            if (!sendCompletionEmail) return;

            var emailList = ParquetArchive.GetSetting(EmailList);
            if (!string.IsNullOrWhiteSpace(emailList))
            {
                var smtpServer = ParquetArchive.GetSetting(SmtpServer);
                var useSSL = int.Parse(ParquetArchive.GetSetting(UseSSL)) == 1;
                var fromEmail = ParquetArchive.GetSetting(FromEmail);

                var smtp = new SmtpClient(smtpServer)
                {
                    EnableSsl = useSSL
                };

                var password = ParquetArchive.GetSetting(FromEmailPassword);
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
            var localPath = ParquetArchive.GetSetting(LocalArchiveDirectory);
            using (var writer = File.AppendText($"{localPath}\\log.txt"))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
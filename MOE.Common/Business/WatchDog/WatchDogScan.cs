using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WatchDog
{
    public class WatchDogScan
    {
        private readonly ConcurrentBag<SPMWatchDogErrorEvent> ForceOffErrors = new ConcurrentBag<SPMWatchDogErrorEvent>();
        //private readonly object eventRepository;
        public ConcurrentBag<SPMWatchDogErrorEvent> LowHitCountErrors = new ConcurrentBag<SPMWatchDogErrorEvent>();
        public ConcurrentBag<SPMWatchDogErrorEvent> MaxOutErrors = new ConcurrentBag<SPMWatchDogErrorEvent>();
        public ConcurrentBag<SPMWatchDogErrorEvent> MissingRecords = new ConcurrentBag<SPMWatchDogErrorEvent>();
        public ConcurrentBag<SPMWatchDogErrorEvent> CannotFtpFiles = new ConcurrentBag<SPMWatchDogErrorEvent>();
        public List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();
        public ConcurrentBag<Models.Signal> SignalsNoRecords = new ConcurrentBag<Models.Signal>();
        public ConcurrentBag<Models.Signal> SignalsWithRecords = new ConcurrentBag<Models.Signal>();
        public ConcurrentBag<SPMWatchDogErrorEvent> StuckPedErrors = new ConcurrentBag<SPMWatchDogErrorEvent>();

        public WatchDogScan(DateTime scanDate)
        {
            ScanDate = scanDate;
            var settingsRepository = ApplicationSettingsRepositoryFactory.Create();
            Settings = settingsRepository.GetWatchDogSettings();
        }

        public WatchDogApplicationSettings Settings { get; set; }
        public DateTime ScanDate { get; set; }
        public int ErrorCount { get; set; }
        //public List<SPMWatchDogErrorEvent> ErrorMessages = new List<SPMWatchDogErrorEvent>();
        public void StartScan()
        {
            if (!Settings.WeekdayOnly || Settings.WeekdayOnly && ScanDate.DayOfWeek != DayOfWeek.Saturday &&
               ScanDate.DayOfWeek != DayOfWeek.Sunday)
            {
               var watchDogErrorEventRepository = SPMWatchDogErrorEventRepositoryFactory.Create();
               var signalRepository = SignalsRepositoryFactory.Create();
               var signals = signalRepository.EagerLoadAllSignals();
                CheckForRecords(signals);
                CheckAllSignals(signals);
                CheckSignalsWithData();
                CheckApplicationEvents(signals);
                CreateAndSendEmail();
            }
        }

        private void CheckApplicationEvents(List<Models.Signal> signals)
        {
            CheckFtpFromAllControllers(signals);
        }

        private void CheckFtpFromAllControllers(List<Models.Signal> signals)
        {
            var startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            var endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            var analysisStart = ScanDate.Date + startHour;
            var analysisEnd = ScanDate.Date + endHour;
            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;  
           MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            var events = eventRepository.GetApplicationEventsBetweenDatesByApplication(analysisStart, analysisEnd, "FTPFromAllcontrollers");
            Parallel.ForEach(signals, options, signal =>
            //foreach(var signal in signals)
            {
                 ErrorCount = (events.Where(e => e.Description.Contains(signal.SignalID)).Count());
                 if (ErrorCount > 0)
                 {
                     MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                     Console.WriteLine("Signal " + signal.SignalID + " can not download from this signal during " + ErrorCount.ToString() + " atempts.  ");
                     error.SignalID = signal.SignalID;
                     error.DetectorID = "0";
                     error.Phase = 0;
                     error.Direction = "";
                     error.TimeStamp = ScanDate;
                     error.Message = "FTPFromAllControllers could not download from Controller.  Number of attempts were: " + ErrorCount.ToString();
                     error.ErrorCode = 6;
                     CannotFtpFiles.Add(error);
                 }
            //}
            });
        }

        private void CheckAllSignals(List<Models.Signal> signals)
        {
            var startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            var endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            var AnalysisStart = ScanDate.Date + startHour;
            var AnalysisEnd = ScanDate.Date + endHour;
            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;

            //Parallel.ForEach(signals, options, signal =>
                foreach(var signal in signals)
                {
                    AnalysisPhaseCollection APcollection = null;
                try
                {
                     APcollection =
                        new AnalysisPhaseCollection(signal.SignalID,
                            AnalysisStart, AnalysisEnd, Settings.ConsecutiveCount);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to get analysis phase for signal " + signal.SignalID);
                }

                    if (APcollection != null)
                    {
                        foreach (var phase in APcollection.Items)
                            //Parallel.ForEach(APcollection.Items, options,phase =>
                        {
                            try
                            {
                                CheckForMaxOut(phase, signal);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(phase.SignalID + " " + phase.PhaseNumber + " - Max Out Error " +
                                                  e.Message);
                            }

                            try
                            {

                                CheckForForceOff(phase, signal);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(phase.SignalID + " " + phase.PhaseNumber + " - Force Off Error " +
                                                  e.Message);
                            }

                            try
                            {
                                CheckForStuckPed(phase, signal);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(phase.SignalID + " " + phase.PhaseNumber + " - Stuck Ped Error " +
                                                  e.Message);
                            }
                        }
                    }

                    //);
            }//);
        }

        private void CheckSignalsWithData()
        {
            var startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            var endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            var AnalysisStart = ScanDate.Date + startHour;
            var AnalysisEnd = ScanDate.Date + endHour;
            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;

            Parallel.ForEach(SignalsWithRecords, options, signal => { CheckForLowDetectorHits(signal); }
            );
        }

        private void CheckForRecords(List<Models.Signal> signals)
        {
            var options = new ParallelOptions();
            //options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;
           // Parallel.ForEach(signals, options, signal =>
            foreach (var signal in signals)
            {
                try
                {
                    GetRecordCountForWeekDayAndWeekend(signal);
                }
                catch (Exception e)
                {
                    Console.WriteLine(signal.SignalID + " - " + e.Message);
                }
            }//);
        }

        private void GetRecordCountForWeekDayAndWeekend(Signal signal)
        {
            if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                CheckSignalRecordCount(ScanDate.AddDays(-2), signal);
            else
                CheckSignalRecordCount(ScanDate, signal);
        }

        private void CheckSignalRecordCount(DateTime dateToCheck, Models.Signal signal)
        {
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            if (controllerEventLogRepository.GetRecordCount(signal.SignalID, dateToCheck.AddDays(-1), dateToCheck) > Settings.MinimumRecords)
            {
                Console.WriteLine("Signal " + signal.SignalID + " Has Current records");
                SignalsWithRecords.Add(signal);
            }
            else
            {
                Console.WriteLine("Signal " + signal.SignalID + " Does Not Have Current records");
                SignalsNoRecords.Add(signal);
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.DetectorID = "0";
                error.Phase = 0;
                error.Direction = "";
                error.TimeStamp = ScanDate;
                error.Message = "Missing Records - IP: " + signal.IPAddress;
                error.ErrorCode = 1;
                MissingRecords.Add(error);
            }
        }

        private void CreateAndSendEmail()
        {
            var message = new MailMessage();
            var db = new SPM();
            var userStore = new UserStore<SPMUser>(db);
            var userManager = new UserManager<SPMUser>(userStore);
            var users = (from u in userManager.Users
                where u.ReceiveAlerts
                select u).ToList();
            foreach (var user in users)
                message.To.Add(user.Email);
            message.To.Add(Settings.DefaultEmailAddress);
            message.Subject = "ATSPM Alerts for " + ScanDate.ToShortDateString();
            message.From = new MailAddress(Settings.FromEmailAddress);
            var missingErrors = SortAndAddToMessage(MissingRecords);
            var forceErrors = SortAndAddToMessage(ForceOffErrors);
            var maxErrors = SortAndAddToMessage(MaxOutErrors);
            var countErrors = SortAndAddToMessage(LowHitCountErrors);
            var stuckpedErrors = SortAndAddToMessage(StuckPedErrors);
            var ftpErrors = SortAndAddToMessage(CannotFtpFiles);
            if (MissingRecords.Count > 0 && missingErrors != "")
            {
                message.Body += " \n --The following signals had too few records in the database on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": \n";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": \n";
                message.Body += missingErrors;
            }
            else
            {
                message.Body += "\n --No new missing record errors were found on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": \n";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": \n";
            }

            if (ForceOffErrors.Count > 0 && forceErrors != "")
            {
                message.Body += " \n --The following signals had too many force off occurrences between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
                message.Body += forceErrors;
            }
            else
            {
                message.Body += "\n --No new force off errors were found between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
            }

            if (MaxOutErrors.Count > 0 && maxErrors != "")
            {
                message.Body += " \n --The following signals had too many max out occurrences between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
                message.Body += maxErrors;
            }
            else
            {
                message.Body += "\n --No new max out errors were found between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
            }

            if (LowHitCountErrors.Count > 0 && countErrors != "")
            {
                message.Body += " \n --The following signals had unusually low advanced detection counts on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                else
                    message.Body += ScanDate.AddDays(-1).ToShortDateString() + " between ";
                message.Body += Settings.PreviousDayPMPeakStart + ":00 and " +
                                Settings.PreviousDayPMPeakEnd + ":00: \n";
                message.Body += countErrors;
            }
            else
            {
                message.Body += "\n --No new low advanced detection count errors on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                else
                    message.Body += ScanDate.AddDays(-1).ToShortDateString() + " between ";
                message.Body += Settings.PreviousDayPMPeakStart + ":00 and " +
                                Settings.PreviousDayPMPeakEnd + ":00: \n";
            }
            if (StuckPedErrors.Count > 0 && stuckpedErrors != "")
            {
                message.Body += " \n --The following signals have high pedestrian activation occurrences between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
                message.Body += stuckpedErrors;
            }
            else
            {
                message.Body += "\n --No new high pedestrian activation errors between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
            }
            if (CannotFtpFiles.Count > 0 && ftpErrors != "")
            {
                message.Body += " \n --The following signals have had FTP problems.  central was not able to delete the file on the controller between  " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
                message.Body += ftpErrors;
            }
            else
            {
                message.Body += "\n --No new controllers had problems FTPing files from the controller between " +
                                Settings.ScanDayStartHour + ":00 and " +
                                Settings.ScanDayEndHour + ":00: \n";
            }
            SendMessage(message);
        }

        private void 
            CheckForLowDetectorHits(Models.Signal signal)
        {
            var detectors = signal.GetDetectorsForSignalThatSupportAMetric(6);
            //Parallel.ForEach(detectors, options, detector =>
            foreach (var detector in detectors)
                try
                {
                    if(detector.DetectionTypes != null && detector.DetectionTypes.Any(d => d.DetectionTypeID == 2))
                    {
                        var channel = detector.DetChannel;
                        var direction = detector.Approach.DirectionType.Description;
                        var start = new DateTime();
                        var end = new DateTime();
                        if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            start = ScanDate.AddDays(-3).Date.AddHours(Settings.PreviousDayPMPeakStart);
                            end = ScanDate.AddDays(-3).Date.AddHours(Settings.PreviousDayPMPeakEnd);
                        }
                        else
                        {
                            start = ScanDate.AddDays(-1).Date.AddHours(Settings.PreviousDayPMPeakStart);
                            end = ScanDate.AddDays(-1).Date.AddHours(Settings.PreviousDayPMPeakEnd);
                        }

                        var currentVolume = detector.GetVolumeForPeriod(start, end);
                        //Compare collected hits to low hit threshold, 
                        if (currentVolume < Convert.ToInt32(Settings.LowHitThreshold))
                        {
                            var error = new SPMWatchDogErrorEvent();
                            error.SignalID = signal.SignalID;
                            error.DetectorID = detector.DetectorID;
                            error.Phase = detector.Approach.ProtectedPhaseNumber;
                            error.TimeStamp = ScanDate;
                            error.Direction = detector.Approach.DirectionType.Description;
                            error.Message = "CH: " + channel.ToString() + " - Count: " + currentVolume.ToString();
                            error.ErrorCode = 2;
                            if (!LowHitCountErrors.Contains(error))
                                LowHitCountErrors.Add(error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var er =
                        ApplicationEventRepositoryFactory.Create();
                    er.QuickAdd("SPMWatchDog", "Program", "CheckForLowDetectorHits",
                        ApplicationEvent.SeverityLevels.Medium, detector.DetectorID + "-" + ex.Message);
                }
            //);
        }

        private void CheckForStuckPed(AnalysisPhase phase, Models.Signal signal)
        {
            if (phase.PedestrianEvents.Count > Settings.MaximumPedestrianEvents)
            {
                var error = new SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction ?? "";
                error.Message = phase.PedestrianEvents.Count +
                                " Pedestrian Activations";
                error.ErrorCode = 3;
                if (!StuckPedErrors.Contains(error))
                {
                    Console.WriteLine("Signal " + signal.SignalID + phase.PedestrianEvents.Count +
                                      " Pedestrian Activations");
                    StuckPedErrors.Add(error);
                }
            }
        }

        private void CheckForForceOff(AnalysisPhase phase, Models.Signal signal)
        {
            if (phase.PercentForceOffs > Settings.PercentThreshold &&
                phase.TerminationEvents.Where(t => t.EventCode != 7).Count() > Settings.MinPhaseTerminations)
            {
                var error = new SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction ?? "";
                error.Message = "Force Offs " + Math.Round(phase.PercentForceOffs * 100, 1) + "%";
                error.ErrorCode = 4;
                if (!ForceOffErrors.Contains(error))
                    ForceOffErrors.Add(error);
            }
        }

        private void CheckForMaxOut(AnalysisPhase phase,
            Models.Signal signal)
        {
            if (phase.PercentMaxOuts > Settings.PercentThreshold &&
                phase.TotalPhaseTerminations > Settings.MinPhaseTerminations)
            {
                var error = new SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction ?? "";
                error.Message = "Max Outs " + Math.Round(phase.PercentMaxOuts * 100, 1) + "%";
                error.ErrorCode = 5;
                if (MaxOutErrors.Count == 0 || !MaxOutErrors.Contains(error))
                {
                    Console.WriteLine("Signal " + signal.SignalID + " Has MaxOut Errors");
                    MaxOutErrors.Add(error);
                }
            }
        }

        private void SendMessage(MailMessage message)
        {
            var er =
                ApplicationEventRepositoryFactory.Create();
            var smtp = new SmtpClient(Settings.EmailServer);
            try
            {
                Console.WriteLine("Sent message to: " + message.To + "\nMessage text: " + message.Body + "\n");
                smtp.Send(message);
                Thread.Sleep(5000);
                er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                    ApplicationEvent.SeverityLevels.Information,
                    "Email Sent Successfully to: " + message.To);
            }
            catch (Exception ex)
            {
                er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                    ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }
        }

        private string SortAndAddToMessage(ConcurrentBag<SPMWatchDogErrorEvent> errors)
        {
            var watchDogErrorEventRepository = SPMWatchDogErrorEventRepositoryFactory.Create();
            var SortedErrors = errors.OrderBy(x => x.SignalID).ThenBy(x => x.Phase).ToList();
            var ErrorMessage = "";
            foreach (var error in SortedErrors)
            {
                if (!Settings.EmailAllErrors)
                {
                    //List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();
                    //compare to error log to see if this was failing yesterday
                    if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                        RecordsFromTheDayBefore =
                            watchDogErrorEventRepository.GetSPMWatchDogErrorEventsBetweenDates(ScanDate.AddDays(-3),
                                ScanDate.AddDays(-2).AddMinutes(-1));
                    else
                        RecordsFromTheDayBefore =
                            watchDogErrorEventRepository.GetSPMWatchDogErrorEventsBetweenDates(ScanDate.AddDays(-1),
                                ScanDate.AddMinutes(-1));
                }
                if (Settings.EmailAllErrors || FindMatchingErrorInErrorTable(error) == false)
                {
                    var signalRepository = SignalsRepositoryFactory.Create();
                    var signal = signalRepository.GetLatestVersionOfSignalBySignalID(error.SignalID);
                    //   Add to email if it was not failing yesterday
                    ErrorMessage += error.SignalID;
                    ErrorMessage += " - ";
                    ErrorMessage += signal.PrimaryName;
                    ErrorMessage += " & ";
                    ErrorMessage += signal.SecondaryName;
                    if (error.Phase > 0)
                    {
                        ErrorMessage += " - Phase ";
                        ErrorMessage += error.Phase;
                    }
                    ErrorMessage += " (" + error.Message + ")";
                    ErrorMessage += "\n";
                }
            }
            try
            {
                watchDogErrorEventRepository.AddListAndSaveToDatabase(errors.ToList());
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                foreach (var validationError in entityValidationErrors.ValidationErrors)
                    Console.WriteLine("Property: " + validationError.PropertyName + " Error: " +
                                      validationError.ErrorMessage);
            }
            return ErrorMessage;
        }

        private static int FindChannel(string SignalID, int Phase)
        {
            var smh = SignalsRepositoryFactory.Create();
            var sig = smh.GetLatestVersionOfSignalBySignalID(SignalID);
            var dets = sig.GetDetectorsForSignalByPhaseNumber(Phase);
            if (dets.Count() > 0)
                return dets.FirstOrDefault().DetChannel;
            return 0;
        }

        private bool FindMatchingErrorInErrorTable(SPMWatchDogErrorEvent error)
        {
            var MatchingRecord = (from r in RecordsFromTheDayBefore
                where error.SignalID == r.SignalID
                      && error.DetectorID == r.DetectorID
                      && error.ErrorCode == r.ErrorCode
                      && error.Phase == r.Phase
                select r).FirstOrDefault();
            if (MatchingRecord != null)
                return true;
            return false;
        }

        private static string FindDetector(Models.Signal Signal, int Channel)
        {
            try
            {
                var gd = Signal.GetDetectorForSignalByChannel(Channel);
                if (gd != null)
                    return gd.DetectorID;
                return "0";
            }
            catch
            {
                return "0";
            }
        }
    }
}
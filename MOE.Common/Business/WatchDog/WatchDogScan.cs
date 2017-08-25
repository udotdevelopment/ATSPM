using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.WatchDog
{
    public class WatchDogScan
    {
        public MOE.Common.Models.WatchDogApplicationSettings Settings { get; set; }
        public DateTime ScanDate { get; set; }
        public ConcurrentBag<MOE.Common.Models.Signal> signalsWithRecords =
            new ConcurrentBag<MOE.Common.Models.Signal>();
        public ConcurrentBag<MOE.Common.Models.Signal> signalsNoRecords =
            new ConcurrentBag<MOE.Common.Models.Signal>();
        ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> ForceOffErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> MaxOutErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> LowHitCountErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> MissingRecords = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> StuckPedErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        public List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();

        public WatchDogScan(DateTime scanDate)
        {
            this.ScanDate = scanDate;
            MOE.Common.Models.Repositories.IApplicationSettingsRepository settingsRepository =
                MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            this.Settings = settingsRepository.GetWatchDogSettings();
        }

        public void StartScan()
        {
            if (!Settings.WeekdayOnly || (Settings.WeekdayOnly && ScanDate.DayOfWeek != DayOfWeek.Saturday && ScanDate.DayOfWeek != DayOfWeek.Sunday))
            {
                var watchDogErrorEventRepository = MOE.Common.Models.Repositories.SPMWatchDogErrorEventRepositoryFactory.Create();
                MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                var signals = signalRepository.EagerLoadAllSignals();
                CheckForRecords(signals);
                CheckAllSignals(signals);
                CheckSignalsWithData();
                CreateAndSendEmail();
            }
        }

        private void CheckAllSignals(List<Models.Signal> signals)
        {
            TimeSpan startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            TimeSpan endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            DateTime AnalysisStart = ScanDate.Date + startHour;
            DateTime AnalysisEnd = ScanDate.Date + endHour;
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;

           Parallel.ForEach(signals, options, signal =>
                //foreach(var signal in signals)
            {
                MOE.Common.Business.AnalysisPhaseCollection APcollection =
                    new MOE.Common.Business.AnalysisPhaseCollection(signal.SignalID,
                        AnalysisStart, AnalysisEnd, Settings.ConsecutiveCount);

                foreach (MOE.Common.Business.AnalysisPhase phase in APcollection.Items)
                //Parallel.ForEach(APcollection.Items, options,phase =>
                {
                    CheckForMaxOut(phase, signal);
                    CheckForForceOff(phase, signal);
                    CheckForStuckPed(phase, signal);
                }
                // );
            });
        }

        private void CheckSignalsWithData()
        {
            TimeSpan startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            TimeSpan endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            DateTime AnalysisStart = ScanDate.Date + startHour;
            DateTime AnalysisEnd = ScanDate.Date + endHour;
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;
            
            Parallel.ForEach(signalsWithRecords, options, signal =>
            {                
                CheckForLowDetectorHits(signal);
            }
            );

        }

        private void CheckForRecords(List<MOE.Common.Models.Signal> signals)
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism;
            Parallel.ForEach(signals, options, signal =>
            {                
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                {
                    CheckSignalRecordCount(ScanDate.AddDays(-2), signal);
                }
                else
                {
                    CheckSignalRecordCount(ScanDate, signal);
                }
            });
        }

        private void CheckSignalRecordCount(DateTime dateToCheck, Models.Signal signal)
        {
            MOE.Common.Models.Repositories.IControllerEventLogRepository controllerEventLogRepository =
                    MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            if (controllerEventLogRepository.GetRecordCount(signal.SignalID, dateToCheck.AddDays(-1), dateToCheck) > Settings.MinimumRecords)
            {
                Console.WriteLine("Signal " + signal.SignalID + "Has Current records");
                signalsWithRecords.Add(signal);
            }
            else
            {
                Console.WriteLine("Signal " + signal.SignalID + "Does Not Have Current records");
                signalsNoRecords.Add(signal);
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.DetectorID = "0";
                error.Phase = 0;
                error.Direction = "";
                error.TimeStamp = ScanDate;
                error.Message = "Missing Records";
                error.ErrorCode = 1;
                MissingRecords.Add(error);
            }
        }
        private void CreateAndSendEmail()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            var userStore = new UserStore<SPMUser>(db);
            var userManager = new UserManager<SPMUser>(userStore);
            List<SPMUser> users = (from u in userManager.Users
                                   where u.ReceiveAlerts == true
                                   select u).ToList();
            foreach (SPMUser user in users)
            {
                message.To.Add(user.Email);
            }
            message.To.Add(Settings.DefaultEmailAddress);    
            message.Subject = "ATSPM Alerts for " + ScanDate.ToShortDateString();
            message.From = new System.Net.Mail.MailAddress(Settings.FromEmailAddress);
            string missingErrors = SortAndAddToMessage(MissingRecords);
            string forceErrors = SortAndAddToMessage(ForceOffErrors);
            string maxErrors = SortAndAddToMessage(MaxOutErrors);
            string countErrors = SortAndAddToMessage(LowHitCountErrors);
            string stuckpedErrors = SortAndAddToMessage(StuckPedErrors);
            if (MissingRecords.Count > 0 && missingErrors != "")
            {
                message.Body += " \n --The following signals had too few records in the database on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                {
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": \n";
                }
                else
                {
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": \n";
                }
                message.Body += missingErrors;
            }
            else
            {
                message.Body += "\n --No new missing record errors were found on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                {
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": \n";
                }
                else
                {
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": \n";
                }
            }

            if (ForceOffErrors.Count > 0 && forceErrors != "")
            {
                message.Body += " \n --The following signals had too many force off occurrences between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
                message.Body += forceErrors;
            }
            else
            {

                message.Body += "\n --No new force off errors were found between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
            }

            if (MaxOutErrors.Count > 0 && maxErrors != "")
            {
                message.Body += " \n --The following signals had too many max out occurrences between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
                message.Body += maxErrors;
            }
            else
            {

                message.Body += "\n --No new max out errors were found between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
            }

            if (LowHitCountErrors.Count > 0 && countErrors != "")
            {
                message.Body += " \n --The following signals had unusually low advanced detection counts on ";
                     if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                {

                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                }
                else
                {
                    message.Body += ScanDate.AddDays(-1).ToShortDateString() + " between ";
                }
                message.Body += Settings.PreviousDayPMPeakStart.ToString() + ":00 and " +
                Settings.PreviousDayPMPeakEnd.ToString() + ":00: \n";
                message.Body += countErrors;
            }
            else
            {
                message.Body += "\n --No new low advanced detection count errors on ";
                if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                {

                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                }
                else
                {
                    message.Body += ScanDate.AddDays(-1).ToShortDateString() + " between ";
                }
                message.Body += Settings.PreviousDayPMPeakStart.ToString() + ":00 and " +
                Settings.PreviousDayPMPeakEnd.ToString() + ":00: \n";
            }
            if (StuckPedErrors.Count > 0 && stuckpedErrors != "")
            {
                message.Body += " \n --The following signals have high pedestrian activation occurrences between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
                message.Body += stuckpedErrors;
            }
            else
            {

                message.Body += "\n --No new high pedestrian activation errors between " +
                Settings.ScanDayStartHour.ToString() + ":00 and " +
                Settings.ScanDayEndHour.ToString() + ":00: \n";
            }

            SendMessage(message);
        }

        private void CheckForLowDetectorHits(MOE.Common.Models.Signal signal)
        {
            List<MOE.Common.Models.Detector> detectors = signal.GetDetectorsForSignalThatSupportAMetric(6);

            //Parallel.ForEach(detectors, options, detector =>
            foreach (MOE.Common.Models.Detector detector in detectors)
            {
                try
                {
                    int channel = detector.DetChannel;
                    string direction = detector.Approach.DirectionType.Description;
                    DateTime start = new DateTime();
                    DateTime end = new DateTime();
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
                    int currentVolume = detector.GetVolumeForPeriod(start, end);
                    //Compare collected hits to low hit threshold, 
                    if (currentVolume < Convert.ToInt32(Settings.LowHitThreshold))
                    {
                        MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                        error.SignalID = signal.SignalID;
                        error.DetectorID = detector.DetectorID;
                        error.Phase = detector.Approach.ProtectedPhaseNumber;
                        error.TimeStamp = ScanDate;
                        error.Direction = detector.Approach.DirectionType.Description;
                        error.Message = "Count: "+ currentVolume.ToString();
                        error.ErrorCode = 2;
                        if (!LowHitCountErrors.Contains(error))
                        {
                            LowHitCountErrors.Add(error);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository er =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

                    er.QuickAdd("SPMWatchDog", "Program", "CheckForLowDetectorHits", 
                        MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, detector.DetectorID + "-" + ex.Message);
                }
            }
            //);
        }

        private void CheckForStuckPed(MOE.Common.Business.AnalysisPhase phase,  MOE.Common.Models.Signal signal)
        {
            if (phase.PedestrianEvents.Count > Settings.MaximumPedestrianEvents)
            {
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction??"";
                error.Message = phase.PedestrianEvents.Count.ToString() +
                        " Pedestrian Activations";
                error.ErrorCode = 3;
                if (!StuckPedErrors.Contains(error))
                {
                    Console.WriteLine("Signal " + signal.SignalID + phase.PedestrianEvents.Count.ToString() + 
                        " Pedestrian Activations");
                    StuckPedErrors.Add(error);
                }
            }
        }

        private void CheckForForceOff(MOE.Common.Business.AnalysisPhase phase, MOE.Common.Models.Signal signal)
        {
            if (phase.PercentForceOffs > Settings.PercentThreshold && phase.TerminationEvents.Where(t=>t.EventCode !=7).Count() > Settings.MinPhaseTerminations)
            {
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction??"";
                error.Message = "Force Offs " + Math.Round(phase.PercentForceOffs * 100, 1).ToString() + "%";
                error.ErrorCode = 4;
                if (!ForceOffErrors.Contains(error))
                {
                    ForceOffErrors.Add(error);
                }
            }
        }

        private void CheckForMaxOut(MOE.Common.Business.AnalysisPhase phase,
             MOE.Common.Models.Signal signal)
        {
            if (phase.PercentMaxOuts > Settings.PercentThreshold && phase.TotalPhaseTerminations > Settings.MinPhaseTerminations)
            {
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = ScanDate;
                error.Direction = phase.Direction??"";
                error.Message = "Max Outs " + Math.Round(phase.PercentMaxOuts * 100, 1).ToString() + "%";
                error.ErrorCode = 5;
               
                    if (MaxOutErrors.Count == 0 || !MaxOutErrors.Contains(error))
                    {
                        Console.WriteLine("Signal " + signal.SignalID + "Has MaxOut Errors");
                        MaxOutErrors.Add(error);
                    }
                

            }
        }

        private void SendMessage( System.Net.Mail.MailMessage message)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository er =
                                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Settings.EmailServer);
            try
            {
            Console.WriteLine("Sent message to: " + message.To.ToString() + "\nMessage text: " + message.Body + "\n");
            smtp.Send(message);
            System.Threading.Thread.Sleep(5000);
            er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                MOE.Common.Models.ApplicationEvent.SeverityLevels.Information,
                "Email Sent Successfully to: " + message.To.ToString());
            }
            catch(Exception ex)
            {                            
                er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }
        }

        private string SortAndAddToMessage(ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> errors)
        {
            MOE.Common.Models.Repositories.ISPMWatchDogErrorEventRepository watchDogErrorEventRepository =
                MOE.Common.Models.Repositories.SPMWatchDogErrorEventRepositoryFactory.Create();
            List<MOE.Common.Models.SPMWatchDogErrorEvent> SortedErrors = 
                errors.OrderBy(x => x.SignalID).ThenBy(x => x.Phase).ToList();

                string ErrorMessage = "";

                foreach (MOE.Common.Models.SPMWatchDogErrorEvent error in SortedErrors)
                {
                    //List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();
                    //compare to error log to see if this was failing yesterday
                    if (Settings.WeekdayOnly && ScanDate.DayOfWeek == DayOfWeek.Monday)
                    {
                        RecordsFromTheDayBefore =
                            watchDogErrorEventRepository.GetSPMWatchDogErrorEventsBetweenDates(ScanDate.AddDays(-3), ScanDate.AddDays(-2).AddMinutes(-1));
                    }
                    else
                    {
                        RecordsFromTheDayBefore =
                            watchDogErrorEventRepository.GetSPMWatchDogErrorEventsBetweenDates(ScanDate.AddDays(-1), ScanDate.AddMinutes(-1));
                    }

                    if (FindMatchingErrorInErrorTable(error) == false)
                    {

                        MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                        var signal = signalRepository.GetSignalBySignalID(error.SignalID);
                        //   Add to email if it was not failing yesterday
                        ErrorMessage += error.SignalID.ToString();
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
                        //}

                    }
                }
                    try
                    {
                        watchDogErrorEventRepository.AddList(errors.ToList());

                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }

                return ErrorMessage;
                    
            }
    
        static private int FindChannel(string SignalID, int Phase)
        {
            
            MOE.Common.Models.Repositories.ISignalsRepository smh = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            MOE.Common.Models.Signal sig = smh.GetSignalBySignalID(SignalID);

            var dets = sig.GetDetectorsForSignalByPhaseNumber(Phase);

            if (dets.Count() > 0)
            {
                return dets.FirstOrDefault().DetChannel;
            }
            else
            {
                return 0;
            }

        }



        private bool FindMatchingErrorInErrorTable(SPMWatchDogErrorEvent error)
        {
            var MatchingRecord = (from r in RecordsFromTheDayBefore
                                 where error.SignalID == r.SignalID
                                 && error.DetectorID == r.DetectorID
                                 && error.ErrorCode == r.ErrorCode
                                 && error.Phase == r.Phase
                                 select r).FirstOrDefault();

            if(MatchingRecord != null)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        static private string FindDetector(MOE.Common.Models.Signal Signal, int Channel)
        {


            try
            {
                MOE.Common.Models.Detector gd = Signal.GetDetectorForSignalByChannel(Channel);

                if (gd != null)
                {
                    return gd.DetectorID;
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        
    }
}

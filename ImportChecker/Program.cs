using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common;
using System.Data.Entity;
using MOE.Common.Models;
using System.Configuration;
using System.ServiceProcess;

namespace ImportChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ImportChecker CheckPlease = new ImportChecker();
        }
    }
    public class ImportChecker
    {
        public MOE.Common.Models.SPM _db;
        public MOE.Common.Models.Repositories.IControllerEventLogRepository CELRepository;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        public bool ThereIsAProblem { get; set; }
        public ImportChecker()
        {
            StartTime = DateTime.Now.AddMinutes(-30);
            EndTime = DateTime.Now;
            ThereIsAProblem = false;
            try
            {
                CELRepository = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                _db = new SPM();
                var testSignals = _db.Signals.Take(1).FirstOrDefault();
            }
            catch
            {
                ThereIsAProblem = true;
                message.Body = "The import checker could not find the database.";
            }
            if (_db != null && CELRepository != null && !ThereIsAProblem)
            {
                List<MOE.Common.Models.ControllerType> TypesInUse = FindControllerTypesInUse();
                foreach (var t in TypesInUse)
                {
                    CheckControllerEventLogByControllerType(t);
                }
                CheckSpeedRecords();
                CheckSpeedService();
            }
            if(ThereIsAProblem)
            {
                CreateAndSendEmail();
            }
        }
        private void CheckSpeedService()
        {
            try
            {
                ServiceController sc = new ServiceController(ConfigurationManager.AppSettings["ListenerServiceName"]);
                sc.MachineName = ConfigurationManager.AppSettings["ListenerServer"];
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    ThereIsAProblem = true;
                    message.Body += "The Speed Listener Server is not running!\n";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Could not find or manage the speed listener service on " + ConfigurationManager.AppSettings["ListenerServer"]);
            }
        }
        private void CheckSpeedRecords()
        {
            var speedRecords = (from r in _db.Speed_Events
                               where r.timestamp > StartTime && r.timestamp <= EndTime
                               select r).Take(20);
            if (speedRecords.Count() < 10)
            {
                ThereIsAProblem = true;
                message.Body += "There are no new speed records recently!\n";
            }
        }
        public List<MOE.Common.Models.ControllerType> FindControllerTypesInUse()
        {
            List<MOE.Common.Models.ControllerType> TypesInUse = new List<ControllerType>();
            List<MOE.Common.Models.ControllerType> AllTypes = (from r in _db.ControllerType
                                                            select r).ToList();
            foreach(var t in AllTypes)
            {
                var s = (from r in _db.Signals 
                         where r.ControllerTypeID == t.ControllerTypeID
                         select r).FirstOrDefault();
                if(s!=null)
                {
                    TypesInUse.Add(t);
                }
            }
            return TypesInUse;
        }
        public void CheckControllerEventLogByControllerType(MOE.Common.Models.ControllerType ControllerType)
        {
            List<MOE.Common.Models.Signal> signals = FindAFewSignalsByType(ControllerType.ControllerTypeID);
            if (signals.Count > 0)
            {
                List<string> FailedSignals = new List<string>();
                int i = 0;
                foreach (var s in signals)
                {
                    List<MOE.Common.Models.Controller_Event_Log> recentevents = CELRepository.GetTopNumberOfSignalEventsBetweenDates(s.SignalID, 200, StartTime, EndTime);
                    Console.WriteLine("Signal " + s.SignalID);
                    if (recentevents.Count > 50)
                    {
                        Console.WriteLine("...Has Records!");
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("!!! Is Missing Records !!!");
                        FailedSignals.Add(s.SignalID);
                    }
                }
                if (i <= (signals.Count/2))
                {
                    ThereIsAProblem = true;
                    message.Body += "The importer for controller type " + ControllerType.Description + " is not working for the folowing controllers:\n";
                    foreach(string s in FailedSignals)
                    {
                        message.Body += s + "\n";
                    }
                }
            }
        }
        public List<MOE.Common.Models.Signal> FindAFewSignalsByType(int ControllerTypeID)
        {
            List<MOE.Common.Models.Signal>  signals = (from r in _db.Signals 
                                                  where r.ControllerTypeID == ControllerTypeID && r.Enabled == true
                                                  && r.Start < DateTime.Today
                                                  select r).OrderBy(t => Guid.NewGuid()).Take(10).ToList();
        return signals;
        }
        private  void CreateAndSendEmail()
        {
            message.Subject = "There is a problem with the ATSPM data collection proccess!";
            message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["FromAddress"]);
            string[] to = ConfigurationManager.AppSettings["ToAddress"].Split(';');
            foreach (string s in to)
            {
                message.To.Add(s);
            }
            SendMessage(message);
        }
         private void SendMessage(System.Net.Mail.MailMessage message)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository er =
                                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["EmailServer"]);
            try
            {
                Console.WriteLine("Sent message to: " + message.To.ToString() + "\nMessage text: " + message.Body + "\n");
                smtp.Send(message);
                System.Threading.Thread.Sleep(500);
                er.QuickAdd("ImportChecker", "Program", "SendMessage",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Information,
                    "Email Sent Successfully to: " + message.To.ToString());
            }
            catch (Exception ex)
            {
                try
                {
                    er.QuickAdd("ImportChecker", "Program", "SendMessage",
                        MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}


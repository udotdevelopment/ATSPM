using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib.Messaging;
using System.Threading.Tasks;
using System.Configuration;
using System.Security;
using MOE.Common.Business;
using MOE.Common.Data;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace FTPfromAllControllers
{
    public class FTPfromAllControllers
    {
        static void Main(string[] args)
        {
            IApplicationEventRepository errorRepository = ApplicationEventRepositoryFactory.Create();
            //while (true) 
            //{ 
            try
            {
                SignalFtpOptions signalFtpOptions = new SignalFtpOptions(
                    Convert.ToInt32(ConfigurationManager.AppSettings["SNMPTimeout"]),
                    Convert.ToInt32(ConfigurationManager.AppSettings["SNMPRetry"]),
                    Convert.ToInt32(ConfigurationManager.AppSettings["SNMPPort"]),
                    Convert.ToBoolean(ConfigurationManager.AppSettings["DeleteFilesAfterFTP"]),
                    ConfigurationManager.AppSettings["LocalDirectory"],
                    Convert.ToInt32(ConfigurationManager.AppSettings["FTPConnectionTimeoutInSeconds"]),
                    Convert.ToInt32(ConfigurationManager.AppSettings["FTPReadTimeoutInSeconds"]),
                    Convert.ToBoolean(ConfigurationManager.AppSettings["skipCurrentLog"]),
                    Convert.ToBoolean(ConfigurationManager.AppSettings["RenameDuplicateFiles"]),
                    Convert.ToInt32(ConfigurationManager.AppSettings["waitBetweenFileDownloadMilliseconds"]),
                    Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfFilesTransferAtOneTime"])
                );

                SPM db = new MOE.Common.Models.SPM();
                ISignalsRepository signalsRepository = SignalsRepositoryFactory.Create(db);
                List<Signal> signals = signalsRepository.GetLatestVersionOfAllSignalsForFtp().ToList();


                int maxThreads = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreads"]);
                int minutesToWait = Convert.ToInt32(ConfigurationManager.AppSettings["MinutesToWait"]);
                
                    //.Where(s =>s.SignalID =="7060");
                    // EOS Signal at Bangerter and 3500 South
                var options = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
                Parallel.ForEach(signals.AsEnumerable(), options, signal =>
                //foreach (var signal in signals) 
                {
                    try
                    {
                        MOE.Common.Business.SignalFtp signalFtp =
                            new MOE.Common.Business.SignalFtp(signal, signalFtpOptions);
                        if (!Directory.Exists(signalFtpOptions.LocalDirectory + signal.SignalID))
                        {
                            Directory.CreateDirectory(signalFtpOptions.LocalDirectory + signal.SignalID);
                        }
                        if (CheckIfIPAddressIsValid(signal))
                        {
                            try
                            {
                                signalFtp.GetCurrentRecords();
                            }
                            catch (AggregateException ex)
                            {
                                Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                                errorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop",
                                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium,
                                    "Error At Highest Level for signal " + signal.SignalID);
                            }
                        }
                    }
                    catch (AggregateException ex)
                    {
                        Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                        errorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop",
                            MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium,
                            "Error At Highest Level for signal " + signal.SignalID);
                    }
                    //} 
                });
                //string timeNow = DateTime.Now.ToString("t"); 
                //Console.WriteLine("At {0}, it is time to take a nap. Program will wait for {1} minutes.", timeNow, minutesToWait); 
                //System.Threading.Thread.Sleep(minutesToWait * 60 * 1000); 
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Error At Highest Level for Main (FTPfromAllControllers) " + ex.Message);
                errorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium,
                    "Error At Highest Level for Main (FTPfromAllControllers) at " + DateTime.Now.ToString("g"));
            }
            //} 
        }

        public static bool CheckIfIPAddressIsValid(MOE.Common.Models.Signal signal)
        {
            bool hasValidIP = false;
            IPAddress ip;
            if (signal.IPAddress == "0")
            {
                return false;
            }
            if (signal.IPAddress == "0.0.0.0")
            {
                return false;
            }

            //test to see if the address is reachable 
            if (IPAddress.TryParse(signal.IPAddress, out ip))
            {
                Ping pingSender = new Ping();
                PingOptions pingOptions = new PingOptions();

                // Use the default Ttl value which is 128,  
                // but change the fragmentation behavior. 
                pingOptions.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.  
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                try
                {
                    PingReply reply = pingSender.Send(signal.IPAddress, timeout, buffer, pingOptions);
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        hasValidIP = true;
                    }
                }
                catch
                {
                    hasValidIP = false;
                }
            }
            return hasValidIP;
        }
    }
}

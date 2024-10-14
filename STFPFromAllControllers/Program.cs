using MOE.Common.Business;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace STFPFromAllControllers
{
    class Program
    {
        static void Main(string[] args)
        {
            IApplicationEventRepository errorRepository = ApplicationEventRepositoryFactory.Create();
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
                Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfFilesTransferAtOneTime"]),
                Convert.ToBoolean(ConfigurationManager.AppSettings["RequiresPPK"]),
                ConfigurationManager.AppSettings["PPKLocation"],
                Convert.ToInt32(ConfigurationManager.AppSettings["RegionalControllerType"]),
                ConfigurationManager.AppSettings["SshFingerprint"]
            );
            int maxThreads = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreads"]);


            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            ISignalsRepository signalsRepository = SignalsRepositoryFactory.Create(db);

            var options = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
            if (signalFtpOptions.RequiresPpk)
            {
                var signals =
                    signalsRepository.GetLatestVersionOfAllSignalsForSftp(signalFtpOptions.RegionControllerType);
                //Parallel.ForEach(signals.AsEnumerable(), options, signal =>
                foreach (var signal in signals)
                {
                    try
                    {
                        MOE.Common.Business.SignalFtp signalFtp =
                            new MOE.Common.Business.SignalFtp(signal, signalFtpOptions);

                        if (!Directory.Exists(signalFtpOptions.LocalDirectory + signal.SignalID))
                        {
                            Directory.CreateDirectory(signalFtpOptions.LocalDirectory + signal.SignalID);
                        }

                        //Get the records over FTP
                        if (CheckIfIPAddressIsValid(signal))
                        {
                            try
                            {
                                signalFtp.GetCubicFilesAsyncPpk(signalFtpOptions.PpkLocation,
                                    signalFtpOptions.SshFingerprint);
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
                }

                ;
            }
            else
            {
                var signals = signalsRepository.GetLatestVersionOfAllSignalsForSftp();
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

                        //Get the records over FTP
                        if (CheckIfIPAddressIsValid(signal))
                        {
                            try
                            {
                                signalFtp.GetCubicFilesAsync(
                                    ConfigurationManager.AppSettings["SFTP_CREDENTIALS_FILE_PATH"]);
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
            }
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

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib.Messaging;
using System.Threading.Tasks;
using System.Configuration;
using MOE.Common.Data;
using MOE.Common.Models.Repositories;

namespace FTPfromAllControllers
{
    class FTPfromAllControllers
    {

        static void Main(string[] args)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository ErrorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            int MaxThreads = Properties.Settings.Default.MaxThreads;
            int SNMPTimeout = Properties.Settings.Default.SNMPTimeout;
            int SNMPRetry = Properties.Settings.Default.SNMPRetry;
            int SNMPPort = Properties.Settings.Default.SNMPPort;
            bool DeleteAfterFTP = Properties.Settings.Default.DeleteFilesAfterFTP;
            bool ImportAfterFTP = Properties.Settings.Default.ImportAfterFTP;
            int WaitBetweenFiles = Properties.Settings.Default.WaitBetweenFiles;
            var connection = ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(connection, Properties.Settings.Default.DestinationTableName,
                               Properties.Settings.Default.WriteToConsole, Properties.Settings.Default.forceNonParallel, Properties.Settings.Default.MaxThreads, Properties.Settings.Default.DeleteFiles,
                               Properties.Settings.Default.EarliestAcceptableDate, Properties.Settings.Default.BulkCopyBatchSize, Properties.Settings.Default.BulkCopyTimeOut);




            MOE.Common.Models.SPM _db = new MOE.Common.Models.SPM();

            MOE.Common.Models.Repositories.ISignalsRepository _sr = SignalsRepositoryFactory.Create(_db);


            var _signals = _sr.GetSignalFTPInfoForAllFTPSignals();



            var options = new ParallelOptions { MaxDegreeOfParallelism = Properties.Settings.Default.MaxThreads };

            Parallel.ForEach(_signals.AsEnumerable(), options, row =>
            //foreach (var row in SignalsDT)
            {
                try
                {
                    MOE.Common.Business.Signal signal = new MOE.Common.Business.Signal();

                    //Initialize the signal, because I didn't make a proper constructor

                    signal.PrimaryName = row.PrimaryName.ToString();
                    signal.SecondaryName = row.Secondary_Name.ToString();
                    signal.IpAddress = row.IP_Address.ToString();
                    signal.SignalID = row.SignalID.ToString();



                    string Username = row.User_Name;
                    string Password = row.Password;
                    string LocalDir = Properties.Settings.Default.HostDir + signal.SignalID + "\\";
                    string RemoteDir = row.FTP_Directory;
                    bool ActiveMode = row.ActiveFTP;


                    if (!Directory.Exists(LocalDir))
                    {
                        Directory.CreateDirectory(LocalDir);
                    }


                    //Get the records over FTP
                    if (CheckIfIPAddressIsValid(signal))
                    {
                        try
                        {

                            MOE.Common.Business.Signal.GetCurrentRecords(signal.IpAddress, signal.SignalID, Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
                                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, 0, Options, Properties.Settings.Default.FTPTimeout);

                        }
                        catch (AggregateException ex)
                        {
                            Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                            ErrorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalID);

                        }


                    }



                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                    ErrorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalID);

                }
            }

    );

         }

        public static bool CheckIfIPAddressIsValid(MOE.Common.Business.Signal signal)
        {
            bool hasValidIP = false;

            IPAddress ip;

            hasValidIP = IPAddress.TryParse(signal.IpAddress, out ip);



            if (signal.IpAddress == "0")
            {
                hasValidIP = false;
            }

            //test to see if the address is reachable
            if (hasValidIP)
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
                PingReply reply = pingSender.Send(signal.IpAddress, timeout, buffer, pingOptions);
                if (reply.Status != IPStatus.Success)
                {
                    hasValidIP = false;
                }
            }

            return hasValidIP;
        }

    }



}



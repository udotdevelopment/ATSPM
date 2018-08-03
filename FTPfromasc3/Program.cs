using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib.Messaging;
using System.Threading.Tasks;
using System.Configuration;
using System.Security;
using MOE.Common.Data;
using MOE.Common.Models.Repositories;

namespace FTPfromAllControllers
{
    public class FTPfromAllControllers
    {

        static void Main(string[] args)
        {
            IApplicationEventRepository errorRepository = ApplicationEventRepositoryFactory.Create();
            int snmpTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SNMPTimeout"]);
            int snmpRetry = Convert.ToInt32(ConfigurationManager.AppSettings["SNMPRetry"]);
            int snmpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SNMPPort"]);
            bool deleteAfterFtp = Convert.ToBoolean(ConfigurationManager.AppSettings["DeleteFilesAfterFTP"]); 
            bool importAfterFtp = Convert.ToBoolean(ConfigurationManager.AppSettings["ImportAfterFTP"]);
            string destinationTableName = ConfigurationManager.AppSettings["DestinationTableName"];
            bool writeToConsole = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteToConsole"]);
            bool forceNonParallel = Convert.ToBoolean(ConfigurationManager.AppSettings["forceNonParallel"]);
            int maxThreads = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreads"]);
            bool deleteFiles = Convert.ToBoolean(ConfigurationManager.AppSettings["DeleteFiles"]);
            DateTime earliestAcceptableDate = Convert.ToDateTime(ConfigurationManager.AppSettings["EarliestAcceptableDate"]);
            int bulkCopyBatchSize = Convert.ToInt32(ConfigurationManager.AppSettings["BulkCopyBatchSize"]);
            int bulkCopyTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["BulkCopyTimeOut"]);
            string hostDir = ConfigurationManager.AppSettings["HostDir"];
            int fTpTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["FTPTimeout"]);
            var connection = ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            MOE.Common.Business.BulkCopyOptions bulkCopyOptions = new MOE.Common.Business.BulkCopyOptions(connection, destinationTableName,
                               writeToConsole, forceNonParallel, maxThreads, deleteFiles,
                               earliestAcceptableDate, bulkCopyBatchSize, bulkCopyTimeOut);


            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            ISignalsRepository signalsRepository = SignalsRepositoryFactory.Create(db);
            var signals = signalsRepository.GetSignalFTPInfoForAllFTPSignals();
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
            //Parallel.ForEach(signals.AsEnumerable(), options, row =>
            foreach (var row in signals)
            {
                try
                {
                    MOE.Common.Business.Signal signal = new MOE.Common.Business.Signal();

                    //Initialize the signal, because I didn't make a proper constructor
                    signal.PrimaryName = row.PrimaryName.ToString();
                    signal.SecondaryName = row.Secondary_Name.ToString();
                    signal.IpAddress = row.IP_Address.ToString();
                    signal.SignalId = row.SignalID.ToString();

                    string username = row.User_Name;
                    //SecureString password = new SecureString();
                    //foreach (char c in row.Password)
                    //    password.AppendChar(c);
                    string password = row.Password;
                    string localDir = hostDir + signal.SignalId + "\\";
                    string remoteDir = row.FTP_Directory;
                    bool activeMode = row.ActiveFTP;

                    if (!Directory.Exists(localDir))
                    {
                        Directory.CreateDirectory(localDir);
                    }

                    //Get the records over FTP
                    if (CheckIfIPAddressIsValid(signal))
                    {
                        try
                        {

                            MOE.Common.Business.Signal.GetCurrentRecords(signal.IpAddress, signal.SignalId, username, password, localDir, remoteDir, deleteAfterFtp,
                                snmpRetry, snmpTimeout, snmpPort, activeMode, 0, fTpTimeout);

                        }
                        catch (AggregateException ex)
                        {
                            Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                            errorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalID);

                        }
                    }
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                    errorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalID);

                }
            }
            //);

         }

        public static bool CheckIfIPAddressIsValid(MOE.Common.Business.Signal signal)
        {
            bool hasValidIP = false;
            IPAddress ip;
            if (signal.IpAddress == "0")
            {
                return false;
            }
            if (signal.IpAddress == "0.0.0.0")
            {
                return false;
            }

            //test to see if the address is reachable
            if (IPAddress.TryParse(signal.IpAddress, out ip))
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
                else
                {
                    hasValidIP = true;
                }
            }
            return hasValidIP;
        }
    }
}



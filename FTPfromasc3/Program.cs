using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using MOE.Common;
using System.Configuration;







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


            //MOE.Common.Data.Signals.SignalFTPparamsDataTable SignalsDT = new MOE.Common.Data.Signals.SignalFTPparamsDataTable();
            //MOE.Common.Data.SignalsTableAdapters.SignalFTPparamsTableAdapter SignalsTA = new MOE.Common.Data.SignalsTableAdapters.SignalFTPparamsTableAdapter();

            //SignalsTA.Fill(SignalsDT);

            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

            var SignalsDT = from r in db.Signals
                            join f in db.ControllerType on r.ControllerTypeID equals f.ControllerTypeID
                            where r.ControllerTypeID != 4
                            select new
                            {
                                SignalId = r.SignalID,
                                PrimaryName = r.PrimaryName,
                                Secondary_Name = r.SecondaryName,
                                Region = r.Region,
                                IP_Address = r.IPAddress,
                                UserName = f.UserName,
                                Password = f.Password,
                                FTPDirectory = f.FTPDirectory,
                                ActiveFTP = f.ActiveFTP

        };


               
            var options = new ParallelOptions { MaxDegreeOfParallelism = Properties.Settings.Default.MaxThreads};

            Parallel.ForEach(SignalsDT.AsEnumerable(), options, row =>
            //foreach (var row in SignalsDT)
            {
                try
                {
                    MOE.Common.Business.Signal signal = new MOE.Common.Business.Signal();

                    //Initialize the signal, because I didn't make a proper constructor

                    signal.PrimaryName = row.PrimaryName.ToString();
                    signal.SecondaryName = row.Secondary_Name.ToString();
                    signal.Region = row.Region.ToString();
                    signal.IpAddress = row.IP_Address.ToString();
                    signal.SignalID = row.SignalId.ToString();



                    string Username = row.UserName;
                    string Password = row.Password;
                    string LocalDir = Properties.Settings.Default.HostDir + signal.SignalID + "\\";
                    string RemoteDir = row.FTPDirectory;
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
                            //var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                            //var token = tokenSource.Token;

                            //Task task = Task.Factory.StartNew(() => MOE.Common.Business.Signal.GetCurrentRecords(signal.IpAddress, signal.SignalID, Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
                            //    SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, 0, Options, Properties.Settings.Default.FTPTimeout, token), token);

                            //task.Wait();

                            //if (token.IsCancellationRequested)
                            //    token.ThrowIfCancellationRequested();

                            MOE.Common.Business.Signal.GetCurrentRecords(signal.IpAddress, signal.SignalID, Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
                                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, 0, Options, Properties.Settings.Default.FTPTimeout);

                        }
                        catch (AggregateException ex)
                        {
                            Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                            ErrorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalId);

                        }


                    }



                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("Error At Highest Level for signal " + ex.Message);
                    ErrorRepository.QuickAdd("FTPFromAllControllers", "Main", "Main Loop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, "Error At Highest Level for signal " + row.SignalId);

                }
            }

    );
    
        
            


                    //if (Properties.Settings.Default.DealWithMoab)
                    //{
                    //    try
                    //    {
                    //        DealWithMoab(Options);
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}


            
   
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
        public static void DealWithMoab(MOE.Common.Business.BulkCopyOptions Options)
        {
            //int configuredRegion = 4;
            //int SNMPTimeout = Properties.Settings.Default.SNMPTimeout;
            //int SNMPRetry = Properties.Settings.Default.SNMPRetry;
            //int SNMPPort = Properties.Settings.Default.SNMPPort;
            //bool DeleteAfterFTP = Properties.Settings.Default.DeleteFilesAfterFTP;
            //bool ImportAfterFTP = Properties.Settings.Default.ImportAfterFTP;
            //int WaitBetweenFiles = Properties.Settings.Default.WaitBetweenFiles;
            //int MoabTimeout = 100;
            //string Username = "econolite";
            //string Password = "ecpi2ecpi";

            //string RemoteDir = "\\Set1";
            //bool ActiveMode = Properties.Settings.Default.FTPActiveMode;

            //string LocalDir = Properties.Settings.Default.HostDir + "8301" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.21", "8301", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);

            //LocalDir = Properties.Settings.Default.HostDir + "8302" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.27", "8302", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);

            //LocalDir = Properties.Settings.Default.HostDir + "8303" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.33", "8303", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);

            //LocalDir = Properties.Settings.Default.HostDir + "8304" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.45", "8304", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);

            //LocalDir = Properties.Settings.Default.HostDir + "8305" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.51", "8305", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);

            //LocalDir = Properties.Settings.Default.HostDir + "8306" + "\\";
            //MOE.Common.Business.Signal.GetCurrentRecords("10.135.5.57", "8306", Username, Password, LocalDir, RemoteDir, DeleteAfterFTP,
            //                SNMPRetry, SNMPTimeout, SNMPPort, ImportAfterFTP, ActiveMode, WaitBetweenFiles, Options, MoabTimeout);


        }
    }



}



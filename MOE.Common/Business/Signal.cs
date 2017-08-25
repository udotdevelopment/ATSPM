//extern alias SharpSNMP;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using AlexPilotti;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Timers;


namespace MOE.Common.Business
{
    public class Signal
    {
        private string primaryName;
        public string PrimaryName
        {
            get
            {
                return primaryName;
            }
            set
            {
                primaryName = value;
            }
        }


        private string secondaryName;
        public string SecondaryName
        {
            get
            {
                return secondaryName;
            }
            set
            {
                secondaryName = value;
            }
        }

        private string ipAddress;
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        private string region;
        public string Region
        {
            get
            {
                return region;
            }
            set
            {
                region = value;
            }
        }

        private string signalId;
        public string SignalID
        {
            get
            {
                return signalId;
            }
            set
            {
                signalId = value;
            }
        }

        private string longitude;
        public string Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        private string latitude;
        public string Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        private int controllerType;
        public int ControllerType
        {
            get
            {
                return controllerType;
            }
            set
            {
                controllerType = value;
            }
        }

        private string ftpPath;
        public string FTPPath
        {
            get
            {
                return ftpPath;
            }
            set
            {
                ftpPath = value;
            }
        }

        private int snmpPort;
        public int SNMPPort
        {
            get
            {
                return snmpPort;
            }
            set
            {
                snmpPort = value;
            }
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Signal y = (Signal)obj;
            return this != null && y != null && this.SignalID == y.SignalID;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : (this.SignalID.GetHashCode());
        }


                public override string ToString()
        {

            string signalName = this.signalId + " : " + this.primaryName + " @ " + this.secondaryName;

            return signalName;
                    //return base.ToString();
        }

                public static void TransferFiles(FTPSClient FTP, String FTPFile, String LocalDir, string RemoteDir, string Server)
                {
                    FTP.SetCurrentDirectory("..");
                    FTP.SetCurrentDirectory(RemoteDir);
                    Console.WriteLine("Transfering " + FTPFile + " from " + RemoteDir + " on " + Server + " to " + LocalDir);
                    //chek to see if the local dir exists.
                    //if not, make it.
                    if (Directory.Exists(LocalDir))
                    {
                    }
                    else
                    {
                        Directory.CreateDirectory(LocalDir);
                    }
                    FTP.GetFile(FTPFile, LocalDir + FTPFile);
                    
                }



                public static bool GetCurrentRecords(string Server, string SignalId, string User, string Password, string LocalDir, string RemoteDir, bool DeleteFilesAfterFTP, int SNMPRetry, int SNMPTimeout, int SNMPPort, bool ImportAfterFTP, bool activemode, int waitbetweenrecords, BulkCopyOptions Options, int FTPTimeout)//, CancellationToken Token)
                {
                    bool recordsComplete = false;
                    
                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();


                    //Initialize the FTP object
                    List<string> RetryFiles = new List<string>();
                    FTPSClient FTP = new FTPSClient();     
                    NetworkCredential Cred = new NetworkCredential(User, Password);
                    ESSLSupportMode SSLMode = ESSLSupportMode.ClearText;
                    EDataConnectionMode DM = EDataConnectionMode.Passive;
                    if (activemode)
                    {
                        DM = EDataConnectionMode.Active;
                    }
                    

                    bool connected = false;
                    string FilePattern = ".dat";


                    try
                    {


                        try
                        {
                            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                            var token2 = tokenSource.Token;

                            Task task = Task.Factory.StartNew(() => FTP.Connect(Server, 21, Cred, SSLMode, null, null, 0, 0, 0, FTPTimeout, true, DM)
                                , token2);

                            task.Wait(token2);

                            if (token2.IsCancellationRequested)
                            {
                                token2.ThrowIfCancellationRequested();
                            }


                        }
                        catch (AggregateException)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, "The connection task timed out for signal " + Server);
                        }
                            { Console.WriteLine("Connection Failure"); }

                        connected = true;
                        
                    }
                    //If there is an error, Print the error and go on to the next file.
                    catch (FTPException ex)
                    {
                        errorRepository.QuickAdd("FTPFromAllcontrollers","Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium,  ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                    catch (AggregateException)
                    { Console.WriteLine("Connection Failure"); }

                    catch (SocketException ex)
                    {
                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                    catch (IOException ex)
                    {
                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                        Console.WriteLine(ex.Message);
                    }

                    if (connected)
                    {
                        try
                        {
                            //if (Token.IsCancellationRequested)
                            //{
                            //    Token.ThrowIfCancellationRequested();
                            //}

                            FTP.SetCurrentDirectory("..");
                            
                           
                            FTP.SetCurrentDirectory(RemoteDir);
                            

                        }
                        catch (AggregateException)
                        { Console.WriteLine("Connection Failure"); }

                        catch (Exception ex)
                        {
                           
                            errorRepository.QuickAdd("FTPFromAllcontrollers","Signal", "GetCurrentRecords", Models.ApplicationEvent.SeverityLevels.Medium,  ex.Message);
                            Console.WriteLine(ex.Message);
                        }




                      

                       
                        try
                        {
                            IList < AlexPilotti.FTPS.Common.DirectoryListItem > RemoteFiles = null;
                            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                            var token = tokenSource.Token;

                            Task task = Task.Factory.StartNew(() => RemoteFiles = FTP.GetDirectoryList(RemoteDir)
                                , token);

                            task.Wait(token);

                            if (token.IsCancellationRequested)
                            {
                                token.ThrowIfCancellationRequested();
                            }
                       
                        List<String> RetrievedFiles = new List<String>();

                           
          
                           //errorRepository.QuickAdd("FTPFromAllcontrollers","Signal", "GetFTPFileList", Models.ApplicationEvent.SeverityLevels.Information, "Retrevied File list from " + Server);
                        if (RemoteFiles != null)
                        {
                            foreach (AlexPilotti.FTPS.Common.DirectoryListItem FTPFile in RemoteFiles)
                            {
                                if (!FTPFile.IsDirectory && FTPFile.Name.Contains(FilePattern))
                                {
                                    try
                                    {
                                        //if (Token.IsCancellationRequested)
                                        //{
                                        //    Token.ThrowIfCancellationRequested();
                                        //}

                                        //If there are no errors, get the file, and add the filename to the retrieved files array for deletion later


                                        var token2 = tokenSource.Token;

                                        Task task2 = Task.Factory.StartNew(() => TransferFiles(FTP, FTPFile.Name, LocalDir, RemoteDir, Server)
                                            , token2);

                                        task2.Wait(token2);

                                        if (token2.IsCancellationRequested)
                                        {
                                            token2.ThrowIfCancellationRequested();
                                        }
                                        else
                                        {


                                            RetrievedFiles.Add(FTPFile.Name);

                                            recordsComplete = true;

                                        }
                                    }
                                    //If there is an error, Print the error and try the file again.
                                    catch (AggregateException)
                                    {
                                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "TransferFiles", Models.ApplicationEvent.SeverityLevels.Medium, "Transfer Task Timed Out");
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorMessage = "Exception:" + ex.Message + " While Transfering file: " + FTPFile + " from " + RemoteDir + " on " + Server + " to " + LocalDir;
                                        Console.WriteLine(errorMessage);
                                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "TransferFiles", Models.ApplicationEvent.SeverityLevels.Medium, errorMessage);
                                        RetryFiles.Add(FTPFile.Name);


                                    }
                                    Thread.Sleep(waitbetweenrecords);
                                }
                            }
                        }

                            //Delete the files we downloaded.  We don't want ot have to deal with the file more than once.  If we delete the file form the controller once we capture it, it will reduce redundancy.

                           
                            if (DeleteFilesAfterFTP)
                            {
                                DeleteFilesFromFTPServer(FTP, RetrievedFiles, waitbetweenrecords, RemoteDir, Server);//, Token);

                            }
                        }

                        catch (Exception ex)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "RetrieveFiles", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                            Console.WriteLine(ex.Message);
                        }




                        FTP.Close();
                        FTP.Dispose();

                        //**********************************************************
                        //I don't think this is doing much good. -SJ 11-22-2016

                        //Try to get the missing files again with a new FTP connection

                        //if (RetryFiles.Count > 1) 
                        //{
                            
                        //    foreach (string FTPFile in RetryFiles)
                        //    {
                        //        try
                        //        {

                        //            Thread.Sleep(waitbetweenrecords);

                        //            FTPSClient _FTP = new FTPSClient();  
                        //            _FTP.Connect(Server, 21, Cred, SSLMode, null, null, 0, 0, 0, 6000, false, DM);

                        //            TransferFiles(_FTP, FTPFile, LocalDir, RemoteDir, Server);
                                    

                        //            if (DeleteFilesAfterFTP)
                        //            {
                        //                try
                        //                {
                        //                    _FTP.DeleteFile(FTPFile);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    Console.WriteLine("Exception:" + ex.Message + " While Deleting file: " + FTPFile + " from " + RemoteDir + " on " + Server + " to " + LocalDir);

                        //                }
                        //            }
                        //            _FTP.Close();
                        //            _FTP.Dispose();
                                    
                                    
                        //        }
                        //        //If there is an error, Print the error and move on.
                        //        catch (Exception ex1)
                        //        {
                        //            Console.WriteLine("Exception:" + ex1.Message + " While Transfering file: " + FTPFile + " from " + RemoteDir + " on " + Server + " to " + LocalDir);
                        //        }

                        //    }

                        // }



                       // RetryFiles.Clear();
                        //**************************************

                        //Turn Logging off.
                        //The ASC3 controller stoploggin if the current file is removed.  to make sure logging comtinues, we must turn the loggin feature off on the 
                        //controller, then turn it back on.
                        try
                        {


                            TurnOffASC3LoggingOverSNMP(SNMPRetry, SNMPPort, SNMPTimeout, Server);

                            Thread.Sleep(SNMPTimeout);


   
                            TurnOnASC3LoggingOverSNMP(SNMPRetry, SNMPPort, SNMPTimeout, Server);
                        }
                        catch
                        {

                        }


                    }
                        return recordsComplete;

                    }

        static public void DeleteFilesFromFTPServer(FTPSClient FTP, List<String>FilesToDelete, int WaitBetweenRecords, string RemoteDirectory, string Server)//, CancellationToken Token)
                {

                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

                    foreach (string FTPFile in FilesToDelete)
                    {
                        try
                        {
  


                            FTP.DeleteFile(FTPFile);
                        }
                        catch (FTPException ex)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                            Console.WriteLine(ex.Message);
                        }
                        catch (AggregateException)
                        { Console.WriteLine("Connection Failure"); }

                        catch (SocketException ex)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                            Console.WriteLine(ex.Message);
                        }
                        catch (IOException ex)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                            Console.WriteLine("Exception:" + ex.Message + " While Deleting file: " + FTPFile + " from " + RemoteDirectory + " on " + Server);

                        }
                        Thread.Sleep(WaitBetweenRecords);
                    }
            
                }

        static public void TurnOffASC3LoggingOverSNMP(int SNMPRetry, int SNMPPort, int SNMPTimeout, String ServerIPAddress)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

            for (int counter = 0; counter < SNMPRetry; counter++)
            {

                try
                {
                    SmnpSet(ServerIPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i", SNMPPort);
                }
                catch (SnmpException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOffASC3LoggingOverSNMP", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                    Console.WriteLine(ex);
                }
                int SNMPState = 10;
                try
                {

                    SNMPState = SnmpGet(ServerIPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i");
                }
                catch (SnmpException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOffASC3LoggingOverSNMP", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                    Console.WriteLine(ex);
                }
                if (SNMPState == 0)
                {
                    break;
                }
                Thread.Sleep(SNMPTimeout);
            }
        }

        static public void TurnOnASC3LoggingOverSNMP(int SNMPRetry, int SNMPPort, int SNMPTimeout, String ServerIPAddress)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

            for (int counter = 0; counter < SNMPRetry; counter++)
            {
                try
                {
                    SmnpSet(ServerIPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i", SNMPPort);
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOnASC3LoggingOverSNMP", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                    Console.WriteLine(ex);
                }

                int SNMPState = 10;
                try
                {
                    SNMPState = SnmpGet(ServerIPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i");
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOnASC3LoggingOverSNMP", Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
                    Console.WriteLine(ex);
                }
                if (SNMPState == 1)
                {
                    break;
                }
                Thread.Sleep(SNMPTimeout);
            }
        }

                static public bool DecodeASC3File(string FileName, string signalId, BulkCopyOptions Options)
                {

                    System.Text.Encoding encoding = System.Text.Encoding.ASCII;
                    try
                    {
                        using (BinaryReader BR = new BinaryReader(File.Open(FileName, FileMode.Open), encoding))
                        {


                            DataTable elTable = new DataTable();
                            UniqueConstraint custUnique =
                            new UniqueConstraint(new DataColumn[] { elTable.Columns["SignalID"],
                                                    elTable.Columns["Timestamp"], 
                                                    elTable.Columns["EventCode"],
                                                    elTable.Columns["EventParam"]
                                            });

                            elTable.Constraints.Add(custUnique);

                            if (BR.BaseStream.Position + 21 < BR.BaseStream.Length)
                            {
                                //Find the start Date
                                String dateString = "";
                                for (int i = 1; i < 21; i++)
                                {

                                    Char c = BR.ReadChar();
                                    dateString += c;
                                }

                                //Console.WriteLine(dateString);
                                DateTime StartTime = new DateTime();
                                if (DateTime.TryParse(dateString, out StartTime) && (BR.BaseStream.Position < BR.BaseStream.Length))
                                {

                                    //find  line feed characters, that should take us to the end of the header.
                                    // First line break is after Version
                                    // Second LF is after FileName
                                    // Third LF is after Interseciton number, which isn't used as far as I can tell
                                    // Fourth LF is after IP address
                                    // Fifth is after MAC Address
                                    // Sixth is after "Controller data log beginning:," and then the date
                                    // Seven is after "Phases in use:," and then the list of phases, seperated by commas

                                    int i = 0;

                                    while (i < 7 && (BR.BaseStream.Position < BR.BaseStream.Length))
                                    {
                                        Char c = BR.ReadChar();
                                        //Console.WriteLine(c.ToString());
                                        if (c == '\n')
                                        {
                                            i++;

                                        }

                                    }

                                    //The first record alwasy seems to be missing a timestamp.  Econolite assumes the first even occures at the same time
                                    // the second event occurs.  I am going to tag the first event with secondevet.timestamp - 1/10 second
                                    int firstEventCode = new Int32();
                                    int firstEventParam = new Int32();


                                    if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                                    {
                                        firstEventCode = Convert.ToInt32(BR.ReadChar());
                                    }

                                    if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                                    {
                                        firstEventParam = Convert.ToInt32(BR.ReadChar());
                                    }

                                    bool firstEventEntered = false;
                                    //MOE.Common.Business.ControllerEvent firstEvent = new MOE.Common.Business.ControllerEvent(SignalID, StartTime, firstEventCode, firstEventParam);


                                    //After that, we can probably start reading
                                    while ((BR.BaseStream.Position + (sizeof(byte) * 4)) <= BR.BaseStream.Length)  //we need ot make sure we are more that 4 characters from the end
                                    {
                                        DateTime EventTime = new DateTime();
                                        int EventCode = new Int32();
                                        int EventParam = new Int32();

                                        //MOE.Common.Business.ControllerEvent controllerEvent = null;
                                        for (int EventPart = 1; EventPart < 4; EventPart++)
                                        {

                                            //getting the time offset
                                            if (EventPart == 1)
                                            {
                                                byte[] rawoffset = new byte[2];
                                                //char[] offset = new char[2];
                                                rawoffset = BR.ReadBytes(2);
                                                Array.Reverse(rawoffset);
                                                int offset = BitConverter.ToInt16(rawoffset, 0);

                                                double tenths = Convert.ToDouble(offset) / 10;

                                                EventTime = StartTime.AddSeconds(tenths);
                                            }

                                            //getting the EventCode
                                            if (EventPart == 2)
                                            {
                                                //Char EventCodeChar = BR.ReadChar();
                                                //EventCode = Convert.ToInt32(BR.ReadChar());
                                                EventCode = Convert.ToInt32(BR.ReadByte());
                                            }

                                            if (EventPart == 3)
                                            {
                                                //Char EventParamChar = BR.ReadChar();
                                                //EventParam = Convert.ToInt32(BR.ReadChar());
                                                EventParam = Convert.ToInt32(BR.ReadByte());
                                            }


                                        }

                                        //controllerEvent = new MOE.Common.Business.ControllerEvent(SignalID, EventTime, EventCode, EventParam);

                                        if ((EventTime) <= DateTime.Now && (EventTime > Properties.Settings.Default.EarliestAcceptableDate))
                                        {
                                            if (!firstEventEntered)
                                            {
                                                try
                                                {
                                                    elTable.Rows.Add(signalId, EventTime.AddMilliseconds(-1), firstEventCode, firstEventParam);
                                                }
                                                catch
                                                {
                                                }
                                                try
                                                {
                                                    elTable.Rows.Add(signalId, EventTime, EventCode, EventParam);
                                                }
                                                catch
                                                {
                                                }
                                                firstEventEntered = true;
                                            }
                                            else
                                            {

                                                try
                                                {
                                                    elTable.Rows.Add(signalId, EventTime, EventCode, EventParam);
                                                }
                                                catch
                                                {
                                                }
                                            }


                                        }
                                    }


                                }
                                //this is what we do when the datestring doesn't parse
                                else
                                {
                                    return (TestNameAndLength(FileName));
                                }



                            }


                            else
                            {
                                return (TestNameAndLength(FileName));
                            }




                            if (BulktoDB(elTable, Options))
                            {
                                return true;

                            }
                            else
                            {
                                return SplitBulkToDB(elTable, Options);

                            }

                        }
                    }
                    catch
                    {

                        return false;
                    }

                }

        //If the file is tiny, it is likely empty, and we want to delte it (return true)
        //If the file has INT in the name, it is likely the old file version, and we want to delete it (return true)
                  static public bool TestNameAndLength(string FilePath)
                {
                    try
                    {
                        FileInfo f = new FileInfo(FilePath);
                        if (f.Name.Contains("INT") || f.Length < 367)
                        {
                            return true;
                        }
                      else
                        {
                            return false;
                        }
                    }
                    catch 
                    {
                        return false;
                    }
                }

                static public bool SaveAsCSV(DataTable table, string Path)
                {
                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                          Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in table.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }

                    File.WriteAllText(Path, sb.ToString());
                    return true;
                }


               
                public static bool BulktoDB(DataTable elTable, BulkCopyOptions Options)
               
                {
                    using (Options.Connection)
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Options.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                    {
                        for (int i = 1; 1 < 4; i++)
                        {
                            
                            try
                            {
                                Options.Connection.Open();
                            }
                            catch
                            {
                                Thread.Sleep(Properties.Settings.Default.SleepTime);

                            }
                            if (Options.Connection.State == ConnectionState.Open)
                            {
                                if (Properties.Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine(" DB connection established");
                                }

                                break;
                            }
                        }
                        String sigID = "";
                        if (elTable.Rows.Count > 0)
                        {
                            DataRow row = elTable.Rows[0];
                           sigID = row[0].ToString();
                        }

                        if (Options.Connection.State == ConnectionState.Open)
                        {
                            bulkCopy.BulkCopyTimeout = Properties.Settings.Default.BulkCopyTimeout;

                            bulkCopy.BatchSize = Properties.Settings.Default.BulkCopyBatchSize;

                            if (Properties.Settings.Default.WriteToConsole)
                            {
                                bulkCopy.SqlRowsCopied +=
                                    new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                                bulkCopy.NotifyAfter = Properties.Settings.Default.BulkCopyBatchSize;
                            }
                            string tablename = Properties.Settings.Default.EventLogTableName;
                            bulkCopy.DestinationTableName = tablename;
                            
                            if (elTable.Rows.Count > 0)
                            {
                                try
                                {

                                    bulkCopy.WriteToServer(elTable);
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        
                                        Console.WriteLine("                   !!!!!!!!!!!!!!!!!!!!!!!!          ");
                                        Console.WriteLine("                   The bulk insert executed          ");
                                        Console.WriteLine("                   !!For Signal "+sigID+" !!!!!!     ");
                                        Console.WriteLine("                   !!!!!!!!!!!!!!!!!!!!!!!!          ");
                                        Console.WriteLine("                   !!!!!!!!!!!!!!!!!!!!!!!!          ");
                                    }
                                    Options.Connection.Close();
                                    return true;
                                }
                                catch (SqlException ex)
                                {
                                    if (ex.Number == 2601)
                                    {
                                        if (Properties.Settings.Default.WriteToConsole)
                                        {
                                            Console.WriteLine("****There is a permission error!*****");
                                        }
                                    }
                                    else
                                    {
                                        if (Properties.Settings.Default.WriteToConsole)
                                        {
                                            //Console.WriteLine("****DATABASE ERROR*****");
                                            Console.WriteLine(ex);
                                        }
                                    }
                                    Options.Connection.Close();
                                    return false;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    return false;
                                }
                            }
                            else
                            {
                                Options.Connection.Close();
                                return false;
                            }





                        }
                        else
                        { return false;}
                    }



                }

            private static void OnSqlRowsCopied(
            object sender, SqlRowsCopiedEventArgs e)
                {
                    Console.WriteLine("Copied {0} so far...", e.RowsCopied);
                }

                public static bool  SplitBulkToDB(DataTable elTable, BulkCopyOptions Options)
                {

                    if (elTable.Rows.Count > 0)
                    {
                        DataTable topDT = new DataTable();
                        DataTable bottomDT = new DataTable();

                        int top = Convert.ToInt32((elTable.Rows.Count * .1));

                        int bottom = (elTable.Rows.Count - top);


       
                            DataTable dtTop = new DataTable();
                            try
                            {
                                dtTop = elTable.Rows.Cast<DataRow>().Take(elTable.Rows.Count / 2).CopyToDataTable();
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }

                            if (dtTop.Rows.Count > 0)
                            {
                                topDT.Merge(dtTop);
                                if (dtTop.Rows.Count > 0)
                                {
                                    if (BulktoDB(topDT, Options))
                                    {
                                     
                                    }
                                    else
                                    {
                                        DataTable elTable2 = new DataTable();
                                        elTable2.Merge(topDT.Copy());
                                        LineByLineWriteToDB(elTable2);

                                    }
                                }
                                topDT.Clear();
                                topDT.Dispose();
                                dtTop.Clear();
                                dtTop.Dispose();
                            }


                            DataTable dtBottom = new DataTable();
                            try
                            {
                                dtBottom = elTable.Rows.Cast<DataRow>().Take(elTable.Rows.Count / 2).CopyToDataTable();
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                            if (dtBottom.Rows.Count > 0)
                            {
                                bottomDT.Merge(dtBottom);

                                if (bottomDT.Rows.Count > 0)
                                {
                                    if (BulktoDB(bottomDT, Options))
                                    {
                                        
                                    }
                                    else
                                    {
                                        DataTable elTable2 = new DataTable();
                                        elTable2.Merge(bottomDT.Copy());
                                        LineByLineWriteToDB(elTable2);

                                    }
                                }
                                bottomDT.Clear();
                                bottomDT.Dispose();
                                dtBottom.Clear();
                                dtBottom.Dispose();
                                
                            }
                  
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public static bool LineByLineWriteToDB(DataTable elTable)
                {
                    //MOE.Common.Data.MOETableAdapters.QueriesTableAdapter moeTA = new MOE.Common.Data.MOETableAdapters.QueriesTableAdapter();
                    using (MOE.Common.Models.SPM db = new Models.SPM())
                    {

                        foreach (DataRow row in elTable.Rows)
                        //Parallel.ForEach(elTable.AsEnumerable(), row =>
                        {
                            try
                            {
                                Models.Controller_Event_Log r = new Models.Controller_Event_Log();
                                r.SignalID = row[0].ToString();
                                r.Timestamp = Convert.ToDateTime(row[1]);
                                r.EventCode = Convert.ToInt32(row[2]);
                                r.EventParam = Convert.ToInt32(row[3]);


                                if (Properties.Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine("---Inserting line for ControllerType {0} at {1}---", row[0], row[1]);
                                }

                                db.Controller_Event_Log.Add(r);
                                db.SaveChangesAsync();

                            }
                            catch (SqlException sqlex)
                            {
                                if (sqlex.Number == 2627)
                                {
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        Console.WriteLine("Duplicate line for signal {0} at {1}", row[0], row[1]);
                                    }
                                    //duplicateLineCount++;
                                }

                                else
                                {
                                    //insertErrorCount++;
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        Console.WriteLine("Exeption {0} \n While Inserting a line for controller {1} on timestamp {2}", sqlex, row[0], row[1]);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (Properties.Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine("Exeption {0} \n While Inserting a line for controller {1} on timestamp {2}", ex, row[0], row[1]);
                                }
                            }

                        }
                        //);

                        return true;

                    }
                }


        static int SnmpGet(string ControllerAddress, string OID, string Value, string Type)
        {
            
            IPAddress IPControllerAddress = IPAddress.Parse(ControllerAddress);
            string community = "public";
            int timeout = 1000;
            VersionCode version = VersionCode.V1;
            IPEndPoint receiver = new IPEndPoint(IPControllerAddress, 161);
            ObjectIdentifier oid = new ObjectIdentifier(OID);
            List<Variable> vList = new List<Variable>();
            ISnmpData data = new Integer32(int.Parse(Value));
            Variable oiddata = new Variable(oid, data);
            vList.Add(new Variable(oid));
            
            int retrievedValue = 0;

            try
            {
               Variable variable = Messenger.Get(version, receiver, new OctetString(community), vList, timeout).FirstOrDefault();

                Console.WriteLine("Check state = {0}", variable.Data.ToString());
                retrievedValue = Int32.Parse(variable.Data.ToString());
            }
            catch (SnmpException SNMPex)
            {
                Console.Write(SNMPex.ToString());
            }
            catch (SocketException Socketex)
            {
                Console.Write(Socketex.ToString());
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            //foreach (Variable variable in
            //    Messenger.Get(version, receiver, new OctetString(community), vList, timeout))
            //{
            //    Console.WriteLine("Check state = {0}", variable.Data.ToString());
            //    retrievedValue = Int32.Parse(variable.Data.ToString());
            //}


            return retrievedValue;
        }


        static void SmnpSet(string ControllerAddress, string OID, string Value, string Type, int SNMPPort)
        {
            IPAddress IPControllerAddress = IPAddress.Parse(ControllerAddress);
            string community = "public";
            int timeout = 1000;
            VersionCode version = VersionCode.V1;
            IPEndPoint receiver = new IPEndPoint(IPControllerAddress, SNMPPort);
            ObjectIdentifier oid = new ObjectIdentifier(OID);
            List<Variable> vList = new List<Variable>();
            ISnmpData data = new Integer32(int.Parse(Value));
            Variable oiddata = new Variable(oid, data);

            vList.Add(oiddata);

            try
            {
                Messenger.Set(version, receiver, new OctetString(community), vList, timeout);
                Console.WriteLine(vList.FirstOrDefault());
            }
            catch (SnmpException SNMPex)
            {
                Console.Write(SNMPex.ToString());
            }
            catch (SocketException Socketex)
            {
                Console.Write(Socketex.ToString());
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            //foreach (Variable variable in  Messenger.Set(version, receiver, new OctetString(community), vList, timeout))
            //{
            //    Console.WriteLine(variable);
            //}



        }

        public static void EnableLogging(String IPAddress, int SNMPRetry, int SNMPtimeout, int SNMPPort)
        {

            for (int counter = 0; counter < SNMPRetry; counter++)
            {

                try
                {
                    SmnpSet(IPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i", SNMPPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                int SNMPState = 10;
                try
                {
                    SNMPState = SnmpGet(IPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (SNMPState == 0)
                {
                    break;
                }
                Thread.Sleep(SNMPtimeout);
            }

            Thread.Sleep(SNMPtimeout);


            //turn logging on
            for (int counter = 0; counter < SNMPRetry; counter++)
            {
                try
                {
                    SmnpSet(IPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i", SNMPPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                int SNMPState = 10;
                try
                {
                    SNMPState = SnmpGet(IPAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (SNMPState == 1)
                {
                    break;
                }
                Thread.Sleep(SNMPtimeout);
            }
        }

    }
}

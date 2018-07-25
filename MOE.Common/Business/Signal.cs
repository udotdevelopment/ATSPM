//extern alias SharpSNMP;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.Common.Properties;

namespace MOE.Common.Business
{
    public class Signal
    {
        public string PrimaryName { get; set; }

        public string SecondaryName { get; set; }

        public string IpAddress { get; set; }

        public string Region { get; set; }

        public string SignalId { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int ControllerType { get; set; }

        public string FtpPath { get; set; }

        public int SnmpPort { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var y = (Signal) obj;
            return this != null && y != null && SignalId == y.SignalId;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : SignalId.GetHashCode();
        }


        public override string ToString()
        {
            var signalName = SignalId + " : " + PrimaryName + " @ " + SecondaryName;

            return signalName;
            //return base.ToString();
        }

        public static void TransferFiles(FTPSClient ftp, string ftpFile, string localDir, string remoteDir,
            string server)
        {
            try
            {
                ftp.SetCurrentDirectory("..");
                ftp.SetCurrentDirectory(remoteDir);
                Console.WriteLine(@"Transfering " + ftpFile + @" from " + remoteDir + @" on " + server + @" to " + localDir);
                //chek to see if the local dir exists.
                //if not, make it.
                if (!Directory.Exists(localDir))
                {
                    Directory.CreateDirectory(localDir);
                }
                ftp.GetFile(ftpFile, localDir + ftpFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    "MOE.Common.Business.Signal", e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, remoteDir + " @ " + server + " - " + e.Message);
            }
        }


        public static bool GetCurrentRecords(string server, string signalId, string user, string password,
            string localDir, string remoteDir, bool deleteFilesAfterFtp, int snmpRetry, int snmpTimeout, int snmpPort, bool activemode, int waitbetweenrecords,
            int ftpTimeout) //, CancellationToken Token)
        {
            var recordsComplete = false;
            var errorRepository = ApplicationEventRepositoryFactory.Create();
            bool skipCurrentLog = Convert.ToBoolean(ConfigurationManager.AppSettings["skipCurrentLog"]);

            //Initialize the FTP object
            var retryFiles = new List<string>();
            var ftp = new FTPSClient();
            var cred = new NetworkCredential(user, password);
            var sslMode = ESSLSupportMode.ClearText;
            var dm = EDataConnectionMode.Passive;
            if (activemode)
                dm = EDataConnectionMode.Active;
            var connected = false;
            var filePattern = ".dat";
            try
            {
                try
                {
                    var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    var token2 = tokenSource.Token;
                    Task task = Task.Factory.StartNew(
                        () => ftp.Connect(server, 21, cred, sslMode, null, null, 0, 0, 0, ftpTimeout, true, dm)
                        , token2);
                    task.Wait(token2);
                    if (token2.IsCancellationRequested)
                        token2.ThrowIfCancellationRequested();
                }
                catch (AggregateException)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + "The connection task timed out");
                    Console.WriteLine(signalId + " @ " + server + " - " + "The connection task timed out");
                }
                connected = true;
            }
            //If there is an error, Print the error and go on to the next file.
            catch (FTPException ex)
            {
                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
            }
            catch (AggregateException)
            {
                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + "Connection Failure - One or more errors occured");
                Console.WriteLine(signalId + " @ " + server + " - " + "Connection Failure - One or more errors occured before connection established");
            }
            catch (SocketException ex)
            {
                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
            }
            catch (IOException ex)
            {
                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
            }
            catch (Exception ex)
            {
                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_ConnectToController", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
            }

            if (connected)
            {
                try
                {
                    ftp.SetCurrentDirectory("..");
                    ftp.SetCurrentDirectory(remoteDir);
                }
                catch (AggregateException)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + "Connection Failure - One or more errors occured after connection established");
                    Console.WriteLine(signalId + " @ " + server + " - " + "Connection Failure - One or more errors occured after connection established");
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
                }
                try
                {
                    IList<DirectoryListItem> remoteFiles = null;
                    var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3000));
                    var token = tokenSource.Token;
                    Task task = Task.Factory.StartNew(() => remoteFiles = ftp.GetDirectoryList(remoteDir), token);
                    task.Wait(token);
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                    var retrievedFiles = new List<string>();
                    if (remoteFiles != null)
                    {
                        DateTime localDate = DateTime.Now;
                        if (skipCurrentLog)
                        {
                            localDate = localDate.AddMinutes(-16);
                        }
                        else
                        {
                            localDate = localDate.AddMinutes(120);
                        }
                        foreach (var ftpFile in remoteFiles)
                        {
                            if (!ftpFile.IsDirectory && ftpFile.Name.Contains(filePattern) && ftpFile.CreationTime < localDate)
                            {
                                try
                                {
                                    var token2 = tokenSource.Token;
                                    var task2 = Task.Factory.StartNew(() => TransferFiles(ftp, ftpFile.Name, localDir, remoteDir, server), token2);
                                    task2.Wait(token2);
                                    if (token2.IsCancellationRequested)
                                    {
                                        token2.ThrowIfCancellationRequested();
                                    }
                                    else
                                    {
                                        retrievedFiles.Add(ftpFile.Name);
                                        recordsComplete = true;
                                    }
                                }
                                //If there is an error, Print the error and try the file again.
                                catch (AggregateException)
                                {
                                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_TransferFiles", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + "Transfer Task Timed Out");
                                }
                                catch (Exception ex)
                                {
                                    string errorMessage = "Exception:" + ex.Message + " While Transfering file: " + ftpFile + " from signal" + signalId + " @ " + remoteDir + " on " + server + " to " + localDir;
                                    Console.WriteLine(errorMessage);
                                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_TransferFiles", Models.ApplicationEvent.SeverityLevels.Medium, errorMessage);
                                    retryFiles.Add(ftpFile.Name);
                                }
                                Thread.Sleep(waitbetweenrecords);
                            }
                        }

                        //Delete the files we downloaded.  We don't want ot have to deal with the file more than once.  If we delete the file form the controller once we capture it, it will reduce redundancy.
                        if (deleteFilesAfterFtp)
                            DeleteFilesFromFtpServer(ftp, retrievedFiles, waitbetweenrecords, remoteDir, server, signalId);//, Token);

                    }
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "GetCurrentRecords_RetrieveFiles", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
                }
                ftp.Close();
                ftp.Dispose();

                //Turn Logging off.
                //The ASC3 controller stoploggin if the current file is removed.  to make sure logging continues, we must turn the loggin feature off on the 
                //controller, then turn it back on.
                try
                {
                    if (skipCurrentLog && CheckAsc3LoggingOverSnmp(snmpRetry, snmpPort, snmpTimeout, server, signalId))
                    {
                        //Do Nothing
                    }
                    else
                    {
                        try
                        {
                            TurnOffAsc3LoggingOverSnmp(snmpRetry, snmpPort, snmpTimeout, server, signalId);
                            Thread.Sleep(snmpTimeout);
                            TurnOnAsc3LoggingOverSnmp(snmpRetry, snmpPort, snmpTimeout, server, signalId);
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {
                }
            }
            return recordsComplete;
        }

        static public void DeleteFilesFromFtpServer(FTPSClient ftp, List<String> filesToDelete, int waitBetweenRecords, string remoteDirectory, string server, string signalId)//, CancellationToken Token)
        {
            var errorRepository = ApplicationEventRepositoryFactory.Create();

            foreach (var ftpFile in filesToDelete)
            {
                try
                {
                    ftp.DeleteFile(ftpFile);
                }
                catch (FTPException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + server + " - " + ex.Message);
                }
                catch (AggregateException)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + "Connection Failure one or more errors occured deleting the file via FTP");
                    Console.WriteLine(signalId + " @ " + server + " - " + "Connection Failure one or more errors occured deleting the file via FTP");
                }
                catch (SocketException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine(ex.Message);
                }
                catch (IOException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "DeleteFilesFromFTPServer", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + server + " - " + ex.Message);
                    Console.WriteLine("Exception:" + ex.Message + " While Deleting file: " + ftpFile + " from " + remoteDirectory + " on " + server);
                }
                Thread.Sleep(waitBetweenRecords);
            }
        }

        static public void TurnOffAsc3LoggingOverSnmp(int snmpRetry, int snmpPort, int snmpTimeout, String serverIpAddress, string signalId)
        {
            var errorRepository = ApplicationEventRepositoryFactory.Create();

            for (var counter = 0; counter < snmpRetry; counter++)
            {
                try
                {
                    SmnpSet(serverIpAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i", snmpPort);
                }
                catch (SnmpException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOffASC3LoggingOverSNMP",
                        ApplicationEvent.SeverityLevels.Medium, serverIpAddress + " " + ex.Message);
                    Console.WriteLine(ex);
                }
                var snmpState = 10;
                try
                {
                    snmpState = SnmpGet(serverIpAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i");
                }
                catch (SnmpException ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "TurnOffASC3LoggingOverSNMP_Get", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + serverIpAddress + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + serverIpAddress + " - " + ex);
                }
                if (snmpState == 0)
                    break;
                Thread.Sleep(snmpTimeout);
            }
        }

        public static void TurnOnAsc3LoggingOverSnmp(int snmpRetry, int snmpPort, int snmpTimeout, String serverIpAddress, string signalId)
        {
            var errorRepository = ApplicationEventRepositoryFactory.Create();

            for (var counter = 0; counter < snmpRetry; counter++)
            {
                try
                {
                    SmnpSet(serverIpAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i", snmpPort);
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOnASC3LoggingOverSNMP_Set", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + serverIpAddress + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + serverIpAddress + " - " + ex);
                }
                var snmpState = 10;
                try
                {
                    snmpState = SnmpGet(serverIpAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i");
                }
                catch (Exception ex)
                {
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " TurnOnASC3LoggingOverSNMP_Get", Models.ApplicationEvent.SeverityLevels.Medium, signalId + " @ " + serverIpAddress + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + serverIpAddress + " - " + ex);
                }
                if (snmpState == 1)
                    break;
                Thread.Sleep(snmpTimeout);
            }
        }

        static public bool CheckAsc3LoggingOverSnmp(int snmpRetry, int snmpPort, int snmpTimeout,
            String serverIpAddress, string signalId)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository =
                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            bool success = false;
            for (int counter = 0; counter < snmpRetry; counter++)
            {
                int snmpState = 10;
                try
                {
                    snmpState = SnmpGet(serverIpAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i");
                }
                catch (Exception ex)
                {

                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", " CheckASC3LoggingOverSNMP_Get",
                        Models.ApplicationEvent.SeverityLevels.Medium,
                        signalId + " @ " + serverIpAddress + " - " + ex.Message);
                    Console.WriteLine(signalId + " @ " + serverIpAddress + " - " + ex);
                }
                if (snmpState == 1)
                {
                    success = true;
                    break;
                }
                Thread.Sleep(snmpTimeout);
            }
            return success;
        }

        public static bool DecodeAsc3File(string fileName, string signalId, BulkCopyOptions options)
        {
            var encoding = Encoding.ASCII;
            try
            {
                using (var br = new BinaryReader(File.Open(fileName, FileMode.Open), encoding))
                {
                    var elTable = new DataTable();
                    var custUnique =
                        new UniqueConstraint(new[]
                        {
                            elTable.Columns["SignalID"],
                            elTable.Columns["Timestamp"],
                            elTable.Columns["EventCode"],
                            elTable.Columns["EventParam"]
                        });

                    elTable.Constraints.Add(custUnique);

                    if (br.BaseStream.Position + 21 < br.BaseStream.Length)
                    {
                        //Find the start Date
                        var dateString = "";
                        for (var i = 1; i < 21; i++)
                        {
                            var c = br.ReadChar();
                            dateString += c;
                        }

                        //Console.WriteLine(dateString);
                        var startTime = new DateTime();
                        if (DateTime.TryParse(dateString, out startTime) &&
                            br.BaseStream.Position < br.BaseStream.Length)
                        {
                            //find  line feed characters, that should take us to the end of the header.
                            // First line break is after Version
                            // Second LF is after FileName
                            // Third LF is after Interseciton number, which isn't used as far as I can tell
                            // Fourth LF is after IP address
                            // Fifth is after MAC Address
                            // Sixth is after "Controller data log beginning:," and then the date
                            // Seven is after "Phases in use:," and then the list of phases, seperated by commas

                            var i = 0;

                            while (i < 7 && br.BaseStream.Position < br.BaseStream.Length)
                            {
                                var c = br.ReadChar();
                                //Console.WriteLine(c.ToString());
                                if (c == '\n')
                                    i++;
                            }

                            //The first record alwasy seems to be missing a timestamp.  Econolite assumes the first even occures at the same time
                            // the second event occurs.  I am going to tag the first event with secondevet.timestamp - 1/10 second
                            var firstEventCode = new int();
                            var firstEventParam = new int();


                            if (br.BaseStream.Position + sizeof(char) < br.BaseStream.Length)
                                firstEventCode = Convert.ToInt32(br.ReadChar());

                            if (br.BaseStream.Position + sizeof(char) < br.BaseStream.Length)
                                firstEventParam = Convert.ToInt32(br.ReadChar());

                            var firstEventEntered = false;
                            //MOE.Common.Business.ControllerEvent firstEvent = new MOE.Common.Business.ControllerEvent(SignalID, StartTime, firstEventCode, firstEventParam);


                            //After that, we can probably start reading
                            while (br.BaseStream.Position + sizeof(byte) * 4 <= br.BaseStream.Length
                            ) //we need ot make sure we are more that 4 characters from the end
                            {
                                var eventTime = new DateTime();
                                var eventCode = new int();
                                var eventParam = new int();

                                //MOE.Common.Business.ControllerEvent controllerEvent = null;
                                for (var eventPart = 1; eventPart < 4; eventPart++)
                                {
                                    //getting the time offset
                                    if (eventPart == 1)
                                    {
                                        var rawoffset = new byte[2];
                                        //char[] offset = new char[2];
                                        rawoffset = br.ReadBytes(2);
                                        Array.Reverse(rawoffset);
                                        int offset = BitConverter.ToInt16(rawoffset, 0);

                                        var tenths = Convert.ToDouble(offset) / 10;

                                        eventTime = startTime.AddSeconds(tenths);
                                    }

                                    //getting the EventCode
                                    if (eventPart == 2)
                                        eventCode = Convert.ToInt32(br.ReadByte());

                                    if (eventPart == 3)
                                        eventParam = Convert.ToInt32(br.ReadByte());
                                }

                                //controllerEvent = new MOE.Common.Business.ControllerEvent(SignalID, EventTime, EventCode, EventParam);

                                if (eventTime <= DateTime.Now && eventTime > Settings.Default.EarliestAcceptableDate)
                                    if (!firstEventEntered)
                                    {
                                        try
                                        {
                                            elTable.Rows.Add(signalId, eventTime.AddMilliseconds(-1), firstEventCode,
                                                firstEventParam);
                                        }
                                        catch
                                        {
                                        }
                                        try
                                        {
                                            elTable.Rows.Add(signalId, eventTime, eventCode, eventParam);
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
                                            elTable.Rows.Add(signalId, eventTime, eventCode, eventParam);
                                        }
                                        catch
                                        {
                                        }
                                    }
                            }
                        }
                        //this is what we do when the datestring doesn't parse
                        else
                        {
                            return TestNameAndLength(fileName);
                        }
                    }


                    else
                    {
                        return TestNameAndLength(fileName);
                    }


                    if (BulktoDb(elTable, options))
                        return true;
                    return SplitBulkToDb(elTable, options);
                }
            }
            catch
            {
                return false;
            }
        }

        //If the file is tiny, it is likely empty, and we want to delte it (return true)
        //If the file has INT in the name, it is likely the old file version, and we want to delete it (return true)
        public static bool TestNameAndLength(string filePath)
        {
            try
            {
                var f = new FileInfo(filePath);
                if (f.Name.Contains("INT") || f.Length < 367)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool SaveAsCsv(DataTable table, string path)
        {
            var sb = new StringBuilder();

            var columnNames = table.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in table.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(path, sb.ToString());
            return true;
        }


        public static bool BulktoDb(DataTable elTable, BulkCopyOptions options)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            using (options.Connection)
            {
                using (var bulkCopy =
                    new SqlBulkCopy(options.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    for (var i = 1;; i++)
                    {
                        try
                        {
                            options.Connection.Open();
                        }
                        catch
                        {
                            Thread.Sleep(Settings.Default.SleepTime);
                        }
                        if (options.Connection.State == ConnectionState.Open)
                        {
                            if (Settings.Default.WriteToConsole)
                                Console.WriteLine("DB connection established");

                            break;
                        }
                    }
                    var sigId = "";
                    if (elTable.Rows.Count > 0)
                    {
                        var row = elTable.Rows[0];
                        sigId = row[0].ToString();
                    }

                    if (options.Connection.State == ConnectionState.Open)
                    {
                        bulkCopy.BulkCopyTimeout = Settings.Default.BulkCopyTimeout;

                        bulkCopy.BatchSize = Settings.Default.BulkCopyBatchSize;

                        if (Settings.Default.WriteToConsole)
                        {
                            bulkCopy.SqlRowsCopied +=
                                OnSqlRowsCopied;
                            bulkCopy.NotifyAfter = Settings.Default.BulkCopyBatchSize;
                        }
                        var tablename = Settings.Default.EventLogTableName;
                        bulkCopy.DestinationTableName = tablename;

                        if (elTable.Rows.Count > 0)
                        {
                            try
                            {
                                bulkCopy.WriteToServer(elTable);
                                if (Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine("!!!!!!!!! The bulk insert executed for Signal " + sigId +
                                                      " !!!!!!!!!");
                                }
                                options.Connection.Close();
                                return true;
                            }
                            catch (SqlException ex)
                            {
                                if (ex.Number == 2601)
                                {
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "BulktoDB_SQL.ex",
                                            Models.ApplicationEvent.SeverityLevels.Medium,
                                            "There is a permission error - " + sigId + " - " + ex.Message);
                                        Console.WriteLine("**** There is a permission error - " + sigId + " *****");
                                    }
                                }
                                else
                                {
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        //Console.WriteLine("****DATABASE ERROR*****");
                                        errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "BulktoDB_SQL.ex",
                                            Models.ApplicationEvent.SeverityLevels.Medium,
                                            "General Error - " + sigId + " - " + ex.Message);
                                        Console.WriteLine("DATABASE ERROR - " + sigId + " - " + ex.Message);
                                    }
                                }
                                options.Connection.Close();
                                return false;
                            }
                            catch (Exception ex)
                            {
                                errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "BulktoDB_Reg.ex",
                                    Models.ApplicationEvent.SeverityLevels.Medium,
                                    "General Error - " + sigId + " - " + ex.Message);
                                Console.WriteLine(ex);
                                return false;
                            }
                        }
                        else
                        {
                            options.Connection.Close();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private static void OnSqlRowsCopied(
            object sender, SqlRowsCopiedEventArgs e)
        {
            Console.WriteLine("Copied {0} so far...", e.RowsCopied);
        }

        public static bool SplitBulkToDb(DataTable elTable, BulkCopyOptions options)
        {
            if (elTable.Rows.Count > 0)
            {
                var topDt = new DataTable();
                var bottomDt = new DataTable();

                var top = Convert.ToInt32(elTable.Rows.Count * .1);

                var bottom = elTable.Rows.Count - top;


                var dtTop = new DataTable();
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
                    topDt.Merge(dtTop);
                    if (dtTop.Rows.Count > 0)
                        if (BulktoDb(topDt, options))
                        {
                        }
                        else
                        {
                            var elTable2 = new DataTable();
                            elTable2.Merge(topDt.Copy());
                            LineByLineWriteToDb(elTable2);
                        }
                    topDt.Clear();
                    topDt.Dispose();
                    dtTop.Clear();
                    dtTop.Dispose();
                }


                var dtBottom = new DataTable();
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
                    bottomDt.Merge(dtBottom);

                    if (bottomDt.Rows.Count > 0)
                        if (BulktoDb(bottomDt, options))
                        {
                        }
                        else
                        {
                            var elTable2 = new DataTable();
                            elTable2.Merge(bottomDt.Copy());
                            LineByLineWriteToDb(elTable2);
                        }
                    bottomDt.Clear();
                    bottomDt.Dispose();
                    dtBottom.Clear();
                    dtBottom.Dispose();
                }

                return true;
            }
            return false;
        }

        public static bool LineByLineWriteToDb(DataTable elTable)
        {
            //MOE.Common.Data.MOETableAdapters.QueriesTableAdapter moeTA = new MOE.Common.Data.MOETableAdapters.QueriesTableAdapter();
            using (var db = new SPM())
            {
                foreach (DataRow row in elTable.Rows)
                    //Parallel.ForEach(elTable.AsEnumerable(), row =>
                    try
                    {
                        var r = new Controller_Event_Log();
                        r.SignalID = row[0].ToString();
                        r.Timestamp = Convert.ToDateTime(row[1]);
                        r.EventCode = Convert.ToInt32(row[2]);
                        r.EventParam = Convert.ToInt32(row[3]);


                        if (Settings.Default.WriteToConsole)
                            Console.WriteLine("---Inserting line for ControllerType {0} at {1}---", row[0], row[1]);

                        db.Controller_Event_Log.Add(r);
                        db.SaveChangesAsync();
                    }
                    catch (SqlException sqlex)
                    {
                        if (sqlex.Number == 2627)
                        {
                            if (Settings.Default.WriteToConsole)
                                Console.WriteLine("Duplicate line for signal {0} at {1}", row[0], row[1]);
                            //duplicateLineCount++;
                        }

                        else
                        {
                            //insertErrorCount++;
                            if (Settings.Default.WriteToConsole)
                                Console.WriteLine(
                                    "Exeption {0} \n While Inserting a line for controller {1} on timestamp {2}", sqlex,
                                    row[0], row[1]);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Settings.Default.WriteToConsole)
                            Console.WriteLine(
                                "Exeption {0} \n While Inserting a line for controller {1} on timestamp {2}", ex,
                                row[0], row[1]);
                    }
                //);

                return true;
            }
        }


        private static int SnmpGet(string controllerAddress, string objectIdentifier, string value, string type)
        {
            var ipControllerAddress = IPAddress.Parse(controllerAddress);
            var community = "public";
            var timeout = 1000;
            var version = VersionCode.V1;
            var receiver = new IPEndPoint(ipControllerAddress, 161);
            var oid = new ObjectIdentifier(objectIdentifier);
            var vList = new List<Variable>();
            ISnmpData data = new Integer32(int.Parse(value));
            var oiddata = new Variable(oid, data);
            vList.Add(new Variable(oid));
            var retrievedValue = 0;
            try
            {
                var variable = Messenger.Get(version, receiver, new OctetString(community), vList, timeout)
                    .FirstOrDefault();

                Console.WriteLine(controllerAddress + " - Check state = {0}", variable.Data.ToString());
                retrievedValue = int.Parse(variable.Data.ToString());
            }
            catch (SnmpException snmPex)
            {
                Console.WriteLine(controllerAddress + " - " + snmPex.ToString());
            }
            catch (SocketException socketex)
            {
                Console.WriteLine(controllerAddress + " - " + socketex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(controllerAddress + " - " + ex.ToString());
            }
            return retrievedValue;
        }


        private static void SmnpSet(string controllerAddress, string objectIdentifier, string value, string type, int snmpPort)
        {
            var ipControllerAddress = IPAddress.Parse(controllerAddress);
            var community = "public";
            var timeout = 1000;
            var version = VersionCode.V1;
            var receiver = new IPEndPoint(ipControllerAddress, snmpPort);
            var oid = new ObjectIdentifier(objectIdentifier);
            var vList = new List<Variable>();
            ISnmpData data = new Integer32(int.Parse(value));
            var oiddata = new Variable(oid, data);
            vList.Add(oiddata);
            try
            {
                Messenger.Set(version, receiver, new OctetString(community), vList, timeout);
                Console.WriteLine(vList.FirstOrDefault());
            }
            catch (SnmpException snmPex)
            {
                Console.WriteLine(controllerAddress + " - " + snmPex.ToString());
            }
            catch (SocketException socketex)
            {
                Console.WriteLine(controllerAddress + " - " + socketex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(controllerAddress + " - " + ex.ToString());
            }
        }

        public static void EnableLogging(string ipAddress, int snmpRetry, int snmPtimeout, int snmpPort)
        {
            for (var counter = 0; counter < snmpRetry; counter++)
            {
                try
                {
                    SmnpSet(ipAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i", snmpPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ipAddress + " - " + ex);
                }
                var snmpState = 10;
                try
                {
                    snmpState = SnmpGet(ipAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "0", "i");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ipAddress + " - " + ex);
                }
                if (snmpState == 0)
                    break;
                Thread.Sleep(snmPtimeout);
            }
            Thread.Sleep(snmPtimeout);


            //turn logging on
            for (var counter = 0; counter < snmpRetry; counter++)
            {
                try
                {
                    SmnpSet(ipAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i", snmpPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ipAddress + " - " + ex);
                }

                var snmpState = 10;
                try
                {
                    snmpState = SnmpGet(ipAddress, "1.3.6.1.4.1.1206.3.5.2.9.17.1.0", "1", "i");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ipAddress + " - " + ex);
                }
                if (snmpState == 1)
                    break;
                Thread.Sleep(snmPtimeout);
            }
        }
    }
}
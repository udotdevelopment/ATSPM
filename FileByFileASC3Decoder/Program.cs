using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Timers;
using MOE.Common;
using System;
using System.IO;
using System.IO.Compression;

namespace FileByFileASC3Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            string cwd = appSettings["ASC3LogsPath"];
            bool writeToConsole = Convert.ToBoolean(appSettings["WriteToConsole"]);
            bool deleteFile = Convert.ToBoolean(appSettings["DeleteFile"]);
            bool writeToCsv = Convert.ToBoolean(appSettings["WriteToCsv"]);
            bool isGzipAgency = Convert.ToBoolean(appSettings["IsGzipAgency"]);


            foreach (string s in Directory.GetDirectories(cwd))
            {
                dirList.Add(s);
            }

            foreach (string dir in dirList)
            {
                if (writeToConsole)
                {
                    Console.WriteLine("-----------------------------Starting Signal " + dir);
                }

                if (isGzipAgency)
                {
                    //get the name of the directory and casting it to an string
                    //This is the only way the program knows the signal number of the controller.
                    string[] strsplit = dir.Split(new char[] { '\\' });
                    string dirname = strsplit.Last();
                    string sigid = dirname;
                    var files = (Directory.GetFiles(dir, "*.gz"));

                    foreach (string s in files)
                    {
                        try
                        {
                            FileInfo f = new FileInfo(s);
                            if (f.Name.Contains("INT") || f.Name.Contains("_1970_"))
                            {
                                try
                                {
                                    File.Delete(s);
                                }
                                catch
                                {
                                }

                                continue;
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            var mergedEventsTable =
                                new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();
                            MOE.Common.Business.LogDecoder.Asc3Decoder.DecodeAsc3GzipFile(s,cwd, sigid, mergedEventsTable,
                                Convert.ToDateTime(appSettings["EarliestAcceptableDate"]));
                            using (MOE.Common.Data.MOE.Controller_Event_LogDataTable eventsTable =
                                   new MOE.Common.Data.MOE.Controller_Event_LogDataTable())
                            {
                                mergedEventsTable.CopyToDataTable(eventsTable, LoadOption.PreserveChanges);
                                mergedEventsTable.Dispose();
                                string connectionString =
                                    ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
                                string destTable = appSettings["DestinationTableNAme"];
                                MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(
                                    connectionString, destTable,
                                    writeToConsole, true, 0, deleteFile,
                                    Convert.ToDateTime(appSettings["EarliestAcceptableDate"]), 5000, 30);
                                if (eventsTable.Count > 0)
                                {
                                    if (MOE.Common.Business.SignalFtp.BulktoDb(eventsTable, Options, destTable) &&
                                        deleteFile)
                                    {
                                        try
                                        {
                                            File.Delete(s);
                                        }
                                        catch
                                        {

                                        }
                                    }

                                }

                                if (writeToCsv)
                                {
                                    FileInfo fileInfo = new FileInfo(s);
                                    StringBuilder sb = new StringBuilder();
                                    IEnumerable<string> columnNames = eventsTable.Columns.Cast<DataColumn>()
                                        .Select(column => column.ColumnName);
                                    sb.AppendLine(string.Join(",", columnNames));

                                    foreach (MOE.Common.Data.MOE.Controller_Event_LogRow row in eventsTable.Rows)
                                    {
                                        List<string> fields = new List<string>();
                                        fields.Add(row.SignalID);
                                        fields.Add(row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                        fields.Add(row.EventCode.ToString());
                                        fields.Add(row.EventParam.ToString());
                                        sb.AppendLine(string.Join(",", fields));
                                    }

                                    var csvFilePath = fileInfo.FullName.TrimEnd(new char[] { '.', 'd', 'a', 't' }) +
                                                      ".csv";
                                    File.WriteAllText(csvFilePath, sb.ToString());
                                }
                            }
                        }
                        catch
                        {
                        }
                    }



                }
                else
                {
                    //get the name of the directory and casting it to an string
                    //This is the only way the program knows the signal number of the controller.
                    string[] strsplit = dir.Split(new char[] { '\\' });
                    string dirname = strsplit.Last();
                    string sigid = dirname;
                    var files = (Directory.GetFiles(dir, "*.dat"));

                    foreach (string s in files)
                    {
                        try
                        {
                            FileInfo f = new FileInfo(s);
                            if (f.Name.Contains("INT") || f.Name.Contains("_1970_"))
                            {
                                try
                                {
                                    File.Delete(s);
                                }
                                catch
                                {
                                }

                                continue;
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            var mergedEventsTable =
                                new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();
                            MOE.Common.Business.LogDecoder.Asc3Decoder.DecodeAsc3File(s, sigid, mergedEventsTable,
                                Convert.ToDateTime(appSettings["EarliestAcceptableDate"]));
                            using (MOE.Common.Data.MOE.Controller_Event_LogDataTable eventsTable =
                                   new MOE.Common.Data.MOE.Controller_Event_LogDataTable())
                            {
                                mergedEventsTable.CopyToDataTable(eventsTable, LoadOption.PreserveChanges);
                                mergedEventsTable.Dispose();
                                string connectionString =
                                    ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
                                string destTable = appSettings["DestinationTableNAme"];
                                MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(
                                    connectionString, destTable,
                                    writeToConsole, true, 0, deleteFile,
                                    Convert.ToDateTime(appSettings["EarliestAcceptableDate"]), 5000, 30);
                                if (eventsTable.Count > 0)
                                {
                                    if (MOE.Common.Business.SignalFtp.BulktoDb(eventsTable, Options, destTable) &&
                                        deleteFile)
                                    {
                                        try
                                        {
                                            File.Delete(s);
                                        }
                                        catch
                                        {

                                        }
                                    }

                                }

                                if (writeToCsv)
                                {
                                    FileInfo fileInfo = new FileInfo(s);
                                    StringBuilder sb = new StringBuilder();
                                    IEnumerable<string> columnNames = eventsTable.Columns.Cast<DataColumn>()
                                        .Select(column => column.ColumnName);
                                    sb.AppendLine(string.Join(",", columnNames));

                                    foreach (MOE.Common.Data.MOE.Controller_Event_LogRow row in eventsTable.Rows)
                                    {
                                        List<string> fields = new List<string>();
                                        fields.Add(row.SignalID);
                                        fields.Add(row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                        fields.Add(row.EventCode.ToString());
                                        fields.Add(row.EventParam.ToString());
                                        sb.AppendLine(string.Join(",", fields));
                                    }

                                    var csvFilePath = fileInfo.FullName.TrimEnd(new char[] { '.', 'd', 'a', 't' }) +
                                                      ".csv";
                                    File.WriteAllText(csvFilePath, sb.ToString());
                                }
                            }
                        }
                        catch
                        {
                        }
                    }



                }
            }

        }

    }

}









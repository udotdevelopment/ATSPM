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
using System.Dynamic;
using System.Timers;
using MOE.Common;

namespace NEWDecodeandImportASC3Logs
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            List<string> dirList = new List<string>();
            string cwd = appSettings["ASC3LogsPath"];
            int maxFilesToImportPerSignal = Convert.ToInt32(appSettings["MaxFilesPerSignalToImport"]);
            string startSignal = null;
            string endSignal = null;
            if (args.Length == 2)
            {
                startSignal = args[0];
                endSignal = args[1];
            }
            foreach (string s in Directory.GetDirectories(cwd))
            {
                dirList.Add(s);
            }
            SimplePartitioner<string> sp = new SimplePartitioner<string>(dirList);
            ParallelOptions optionsMain = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            Parallel.ForEach(sp, optionsMain, dir =>
            {
               
                string signalId;
                string[] fileNames;
                GetFileNamesAndSignalId(dir, out signalId, out fileNames);
                if ((args.Length == 2 && (String.Compare(signalId, startSignal, comparisonType:StringComparison.OrdinalIgnoreCase) > 0 ||
                                          String.Compare(signalId, startSignal, comparisonType: StringComparison.OrdinalIgnoreCase) == 0 ) &&
                    (String.Compare(signalId, endSignal, comparisonType: StringComparison.OrdinalIgnoreCase) < 0 || 
                     String.Compare(signalId, endSignal, comparisonType: StringComparison.OrdinalIgnoreCase) == 0)) || args.Length ==0)
                {
                    var toDelete = new ConcurrentBag<string>();
                    var mergedEventsTable = new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();
                    if (Convert.ToBoolean(appSettings["WriteToConsole"]))
                    {
                        Console.WriteLine("-----------------------------Starting Signal " + dir);
                    }
                    //foreach (var fileName in fileNames)
                        for(int i = 0; i < maxFilesToImportPerSignal && i < fileNames.Length; i++)
                    {
                        try
                        {
                            MOE.Common.Business.LogDecoder.Asc3Decoder.DecodeAsc3File(fileNames[i], signalId,
                                mergedEventsTable, Convert.ToDateTime(appSettings["EarliestAcceptableDate"]));
                            toDelete.Add(fileNames[i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = CreateDataTableForImport();
                    AddEventsToImportTable(mergedEventsTable, elTable);
                    mergedEventsTable.Dispose();
                    BulkImportRecordsAndDeleteFiles(appSettings, toDelete, elTable);
                }
            });
        }

        private static void GetFileNamesAndSignalId(string dir, out string signalId, out string[] fileNames)
        {
            string[] strsplit = dir.Split(new char[] { '\\' });
            signalId = strsplit.Last();
            fileNames = Directory.GetFiles(dir, "*.dat?");
        }

        private static void BulkImportRecordsAndDeleteFiles(NameValueCollection appSettings, ConcurrentBag<string> toDelete, MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            string destTable = appSettings["DestinationTableNAme"];
            MOE.Common.Business.BulkCopyOptions options = new MOE.Common.Business.BulkCopyOptions(connectionString, destTable,
                Convert.ToBoolean(appSettings["WriteToConsole"]),
                Convert.ToBoolean(appSettings["forceNonParallel"]),
                Convert.ToInt32(appSettings["MaxThreads"]),
                Convert.ToBoolean(appSettings["DeleteFile"]),
                Convert.ToDateTime(appSettings["EarliestAcceptableDate"]),
                Convert.ToInt32(appSettings["BulkCopyBatchSize"]),
                Convert.ToInt32(appSettings["BulkCopyTimeOut"]));
            if (elTable.Count > 0)
            {
                if (MOE.Common.Business.SignalFtp.BulktoDb(elTable, options, destTable) && Convert.ToBoolean(appSettings["DeleteFile"]))
                {
                    DeleteFiles(toDelete);
                }
            }
            else
            {
                ConcurrentBag<String> td = new ConcurrentBag<String>();
                foreach (string s in toDelete)
                {
                    if (s.Contains("1970_01_01"))
                    {
                        td.Add(s);
                    }
                }
                if (td.Count > 0)
                {
                    DeleteFiles(td);
                }
            }
        }

        private static void AddEventsToImportTable(BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow> mergedEventsTable, MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable)
        {
            foreach (var r in mergedEventsTable)
            {
                try
                {
                    elTable.AddController_Event_LogRow(r.SignalID, r.Timestamp, r.EventCode,
                        r.EventParam);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static MOE.Common.Data.MOE.Controller_Event_LogDataTable CreateDataTableForImport()
        {
            MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable();
            //UniqueConstraint custUnique =
            //new UniqueConstraint(new DataColumn[] { elTable.Columns["SignalId"],
            //                            elTable.Columns["Timestamp"],
            //                            elTable.Columns["EventCode"],
            //                            elTable.Columns["EventParam"]
            //                });

            //elTable.Constraints.Add(custUnique);
            return elTable;
        }

        static public bool SaveAsCsv(DataTable datatable, string path)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = datatable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in datatable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }
            File.WriteAllText(path, sb.ToString());
            return true;
        }

        public static void DeleteFiles(ConcurrentBag<string> files)
        {
            foreach (string f in files)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        File.Delete(f);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}

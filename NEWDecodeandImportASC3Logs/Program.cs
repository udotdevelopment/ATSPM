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

namespace NEWDecodeandImportASC3Logs
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            string CWD = Properties.Settings.Default.ASC3LogsPath;
            string CSV = Properties.Settings.Default.CSVOutPAth;

            //var tableCollection = new BlockingCollection<DataTable>();

            //DataTable mergedEventsTable = new DataTable();
            ParallelOptions options;

            if (!Properties.Settings.Default.forceNonParallel)
            {
                options = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            }
            else
            {
                if (Properties.Settings.Default.MaxThreads < 2)
                {
                    options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
                }
                else
                {
                    options = new ParallelOptions { MaxDegreeOfParallelism = Properties.Settings.Default.MaxThreads };
                }
            }

            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }

            
            SimplePartitioner<string> sp = new SimplePartitioner<string>(dirList);
            //foreach (string dir in dirList)

            ParallelOptions optionsMain = new ParallelOptions { MaxDegreeOfParallelism = Properties.Settings.Default.MaxThreadsMain};
            Parallel.ForEach(sp, optionsMain, dir =>
                      {
                          var ToDelete = new ConcurrentBag<string>();

                          if (Properties.Settings.Default.WriteToConsole)
                          {
                              Console.WriteLine("-----------------------------Starting Signal " + dir);
                          }


                          //get the name of the directory and casting it to an int
                          //This is the only way the program knows the signal number of the controller.
                          string[] strsplit = dir.Split(new char[] { '\\' });
                          string dirname = strsplit.Last();
                          string sigid = dirname;
                          var mergedEventsTable = new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();

                          SimplePartitioner<string> sp2 = new SimplePartitioner<string>(Directory.GetFiles(dir, "*.dat"));
                          Parallel.ForEach(sp2, options, s =>
                                {



                                    try
                                    {
                                        MOE.Common.Business.LogDecoder.ASC3Decoder.DecodeASC3File(s, sigid, mergedEventsTable);

                                        ToDelete.Add(s);

                                    }
                                   
                                    catch { }


                                }
                                 );



                        
                          using (MOE.Common.Data.MOE.Controller_Event_LogDataTable EventsTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable())
                          {
                               
                              
                              mergedEventsTable.CopyToDataTable(EventsTable, LoadOption.PreserveChanges);


                              mergedEventsTable.Dispose();

                              string connectionString = Properties.Settings.Default.SPMConnectionString;
                              string destTable = Properties.Settings.Default.DestinationTableNAme;
                              

                              MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(connectionString, destTable,
                                  Properties.Settings.Default.WriteToConsole,Properties.Settings.Default.forceNonParallel, Properties.Settings.Default.MaxThreads, Properties.Settings.Default.DeleteFile, 
                                  Properties.Settings.Default.EarliestAcceptableDate, Properties.Settings.Default.BulkCopyBatchSize, Properties.Settings.Default.BulkCopyTimeOut);
                                  
                                  

                              if (EventsTable.Count > 0)
                              {


                                  if (MOE.Common.Business.Signal.BulktoDB(EventsTable, Options) && Properties.Settings.Default.DeleteFile)
                                  {


                                      DeleteFiles(ToDelete);
                                  }
                                  //string filename = sigid.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Month.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Day.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Year.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Hour.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Minute.ToString();
                                  //filename += "_";
                                  //filename += DateTime.Now.Second.ToString();
                                  //filename += ".csv";

                                  //SaveAsCSV(EventsTable, Path.Combine(CSV, filename));
                                  //if (Properties.Settings.Default.DeleteFile)
                                  //{
                                  //    DeleteFiles(ToDelete);
                                  //}
                              }

                              else
                              {
                                  
                                  ConcurrentBag<String> td = new ConcurrentBag<String>();

                                  foreach (string s in ToDelete)
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
                          


                      }

                          );



        }



        static public bool SaveAsCSV(DataTable Datatable, string Path)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = Datatable.Columns.Cast<DataColumn>().
                                  Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in Datatable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(Path, sb.ToString());
            return true;
        }

        public static void DeleteFiles(ConcurrentBag<string> Files)
        {
            foreach (string f in Files)
            {
                File.Delete(f);
            }
        }
    }
}

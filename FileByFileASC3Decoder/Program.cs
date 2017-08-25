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

namespace FileByFileASC3Decoder
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            string CWD = FileByFileASC3Decoder.Properties.Settings.Default.ASC3LogsPath;


            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }



            foreach (string dir in dirList)
            {
                

                if (FileByFileASC3Decoder.Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("-----------------------------Starting Signal " + dir);
                }


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
                        if (f.Name.Contains("INT") || f.Name.Contains("_1970_") || f.Length < 367)
                        {
                            try
                            {
                                File.Delete(s);
                            }
                            catch { }
                            continue;
                        }
                    }
                    catch { }



                    try
                    {
                        var mergedEventsTable = new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();

                        MOE.Common.Business.LogDecoder.ASC3Decoder.DecodeASC3File(s, sigid, mergedEventsTable);


                        using (MOE.Common.Data.MOE.Controller_Event_LogDataTable EventsTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable())
                        {


                            mergedEventsTable.CopyToDataTable(EventsTable, LoadOption.PreserveChanges);


                            mergedEventsTable.Dispose();

                            string connectionString = FileByFileASC3Decoder.Properties.Settings.Default.SPMConnectionString;
                            string destTable = FileByFileASC3Decoder.Properties.Settings.Default.DestinationTableNAme;


                            MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(connectionString, destTable,
                                FileByFileASC3Decoder.Properties.Settings.Default.WriteToConsole, true, 0, FileByFileASC3Decoder.Properties.Settings.Default.DeleteFile,
                                FileByFileASC3Decoder.Properties.Settings.Default.EarliestAcceptableDate, 5000, 30);



                            if (EventsTable.Count > 0)
                            {


                                if (MOE.Common.Business.Signal.BulktoDB(EventsTable, Options) && FileByFileASC3Decoder.Properties.Settings.Default.DeleteFile)
                                {

                                    try
                                    {
                                        File.Delete(s);
                                    }
                                    catch { }

                                }

                            }

                        }


                    }
                    catch { }

                }
            }
        }
    }
}









using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Mail;
using MOE.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using MOE.Common.Models.Repositories;


namespace DecodeTrafficwareLogs
{
    class Program
    {
        private IApplicationEventRepository errorRepository = ApplicationEventRepositoryFactory.Create();
        private const int retryMax = 3;
        private const int retryTime = 2000;

        static void Main(string[] args)
        {
            new Program().FindFiles();
            new Program().SaveEvents();
        }

        private void FindFiles()
        {
            string CWD = Properties.Settings.Default.TWLogsPath;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();

            // MOEDataSetTableAdapters.QueriesTableAdapter MoeTA = new MOEDataSetTableAdapters.QueriesTableAdapter();



            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }

            if (dirList.Count > 0)
            {
                foreach (string dir in dirList)

                //DO NOT MAKE THIS A PARALLEL OPPERATION!
                //Doing so results in the files beign put in the wrong directory, were they are read as being part of the 
                //Wrong signal
                //var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(4) };
                //Parallel.ForEach(dirList.AsEnumerable(), options , dir =>
                {

                    foreach (string file in Directory.GetFiles(dir, "*.dat"))
                    {
                        DecodeTW(dir, file);
                        if (Properties.Settings.Default.WriteToConsole)
                        {
                            Console.WriteLine("Done decoding file {0}", file);
                        }
                    }


                    //foreach (string file in Directory.GetFiles(dir, "*.csv"))
                    //{
                    //    WritetoDB(dir, file, MoeTA);
                    //    Console.WriteLine("Done writing file {0} to database", file);
                    //}


                }//);


            }
        }

        private void DecodeTW(string dir, string file)
        {

            //path to the decoder program
            string decoder = Properties.Settings.Default.DecoderPath;
            //time in MS to wait for the decoder ot fail
            int timeOut = 500;
            bool fileDecoded = true;

            try
            {
                //Set the current directory.  One of the quirks of the decoder is that
                //it requires the target file to be in the current workig directory
                Directory.SetCurrentDirectory(dir);
            }
            catch (DirectoryNotFoundException ex)
            {
                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("The specified directory does not exist. {0}", ex);
                }
            }

            try
            {
                string csvfile = file.Replace(".dat", ".csv");
                string arguments = file + " " + csvfile;
                Process p = Process.Start(decoder, arguments);

                //Wait for window to finish loading.

                //Wait for the process to exit or time out.
                p.WaitForExit(timeOut);
                //Check to see if the process is still running.
                if (p.HasExited == false)
                {
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (p.Responding)
                    {
                        //Process was responding; close the main window.
                        p.CloseMainWindow();
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        p.Kill();
                    }
                }
                p.Dispose();
            }
            catch (Exception ex)
            {
                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("Exception {0} while decoding file {1}", ex, file);
                }
                fileDecoded = false;
            }

            //If the Delete flag is checking in settings, and the file has been decoded, then delte the file.
            if (Properties.Settings.Default.DeleteFiles && fileDecoded)
            {
                for (int i = 1; i <= retryMax; i++)
                {
                    try
                    {
                        File.Delete(file);
                        break;
                    }
                    catch (Exception ex)
                    {
                        errorRepository.QuickAdd("DecodeTrafficwareLogs", "Signal", "DecodeTW - Delete",
                            MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, file + " - " + ex.Message);
                        if (Properties.Settings.Default.WriteToConsole)
                        {
                            Console.WriteLine("Exception {0} while deleting file {1}", ex, file);
                        }

                        Thread.Sleep(retryTime);
                    }
                }
            }
        }

//subroutine to write the decoded log to the database.
        //this is where most of the work is done.

        //The only way we match signalid to the collected logs is by the directory name.
        //static void WritetoDB(string dir, string file, MOEDataSetTableAdapters.QueriesTableAdapter MoeTA)
        private void SaveEvents()
        {



            int insertErrorCount = 0;
            //int duplicateLineCount = 0;
            int insertedLinecount = 0;
            double errorRatio = 0;
            bool fileHasBeenRead = false;
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            TimeSpan elapsedTime = new TimeSpan();
            string CWD = Properties.Settings.Default.TWLogsPath;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            ConcurrentQueue<string> FilesToDelete = new ConcurrentQueue<string>();


            // MOEDataSetTableAdapters.QueriesTableAdapter MoeTA = new MOEDataSetTableAdapters.QueriesTableAdapter();



            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }



            var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
            Parallel.ForEach(dirList.AsEnumerable(), options, dir =>
            //Parallel.ForEach(dirList.AsEnumerable(), dir =>
            //foreach (string dir in dirList)
            {

                //get the name of the directory and casting it to an int
                //This is the only way the program knows the signal number of the controller.
                string[] strsplit = dir.Split(new char[] { '\\' });
                string dirname = strsplit.Last();
                string sigid = dirname;

                Console.WriteLine("Starting signal " + dirname);


                var options1 = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
                Parallel.ForEach(Directory.GetFiles(dir, "*.csv"), options1, file =>
                //Parallel.ForEach(Directory.GetFiles(dir, "*.csv"), file =>
                //foreach (string file in Directory.GetFiles(dir, "*.csv"))
                {
                    DataTable elTable = new DataTable();

                    elTable.Columns.Add("SignalId", typeof(String));
                    elTable.Columns.Add("Timestamp", typeof(DateTime));
                    elTable.Columns.Add("EventCode", typeof(Int32));
                    elTable.Columns.Add("EventParam", typeof(Int32));

                    UniqueConstraint custUnique =
new UniqueConstraint(new DataColumn[] { elTable.Columns[0],
                                        elTable.Columns[1], 
                                           elTable.Columns[2],
                                            elTable.Columns[3]
                                            });

                    elTable.Constraints.Add(custUnique);



                    startTime = DateTime.Now;




                    foreach (string line in File.ReadAllLines(file))
                    {

                        //Every other line is blank.  We only care about the lines that have data, and 
                        //every data line has a comma
                        if (line.Contains(','))
                        {
                            //the first five lines or so are header information.  They need to be filtered out.
                            if (line.Contains(",T"))
                            {
                                //even if there is nothing but header in the file, we want it marked as being read
                                //so we can delete is later.
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",Intersection#"))
                            {
                                fileHasBeenRead = true;
                            }
 
                            else if (line.Contains(",IP Address:"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",MAC Address"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",ControllerType Data Log Beginning"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",Phases in use"))
                            {
                                fileHasBeenRead = true;
                            }
                            else
                            {
                                //split the line on commas and assign each split to a var
                                string[] lineSplit = line.Split(new char[] { ',' });
                                DateTime timeStamp = new DateTime();
                                int eventCode = 0;
                                int eventParam = 0;
                                bool lineError = false;
                                //it might happen that the character on the line are not quite right.
                                //the Try/catch stuff is an attempt to deal with that.
                                try
                                {
                                    timeStamp = Convert.ToDateTime(lineSplit[0]);
                                }
                                catch 
                                {

                                    Console.Write("Error converting {0} to Datetime.  Skipping line", lineSplit[0]);
                                    lineError = true;
                                }
                                try
                                {
                                    eventCode = Convert.ToInt32(lineSplit[1]);
                                }
                                catch 
                                {
                                    Console.Write("Error converting {0} to eventCode Interger.  Skipping line", lineSplit[0]);
                                    lineError = true;
                                }
                                try
                                {
                                    eventParam = Convert.ToInt32(lineSplit[2]);
                                }
                                catch 
                                {
                                    Console.Write("Error converting {0} to eventParam Interger.  Skipping line", lineSplit[0]);
                                    lineError = true;
                                }
                                //If there were no errors on the line, then put the line into the bulk queue
                                if (!lineError)
                                {
                                    try
                                    {
                                        elTable.Rows.Add(sigid, timeStamp, eventCode, eventParam);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }


                                }

                                //If it gets this far, the file has been opened
                                fileHasBeenRead = true;



                            }


                        }

                        if (Properties.Settings.Default.WriteToConsole)
                        {
                            Console.WriteLine("NEXT LINE");

                        }
                    }

                    // Array.Clear(lines, 0, lines.Count());




                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("$$$ Entire file has been read $$$");

                    }


                    //Do the Math to find out if the error ratio is intolerably high before deleting the file
                    if (insertErrorCount > 0)
                    {
                        errorRatio = Convert.ToDouble(insertErrorCount) / Convert.ToDouble((insertedLinecount + insertErrorCount));
                    }
                    else
                    {
                        errorRatio = 0;
                    }

                    if (file.Length == 0)
                    {
                        fileHasBeenRead = true;
                    }


                    endTime = DateTime.Now;

                    string connectionString = Properties.Settings.Default.SPM.ToString();


                    MOE.Common.Business.BulkCopyOptions Options = new MOE.Common.Business.BulkCopyOptions(connectionString,  Properties.Settings.Default.DestinationTableName,
                        Properties.Settings.Default.WriteToConsole, Properties.Settings.Default.forceNonParallel, Properties.Settings.Default.MaxThreads, Properties.Settings.Default.DeleteFile,
                        Properties.Settings.Default.EarliestAcceptableDate, Properties.Settings.Default.BulkCopyBatchSize, Properties.Settings.Default.BulkCopyTimeOut);
                                  

                    //the Signal class has a static methos to insert the tableinto the DB.  We are using that.
                    MOE.Common.Business.SignalFtp.BulktoDb(elTable, Options, Properties.Settings.Default.DestinationTableName);

                    elapsedTime = endTime - startTime;

                    if (Properties.Settings.Default.DeleteFiles)
                    {
                        try
                        {
                            //if ((errorRatio < Properties.Settings.Default.ErrorRatio) && (fileHasBeenRead))
                            //{
                                try
                                {
                                    File.Delete(file);
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        Console.WriteLine("{0} Deleted", file);
                                    }
                                }
                                catch (SystemException sysex)
                                {
                                    if (Properties.Settings.Default.WriteToConsole)
                                    {
                                        Console.WriteLine("{0} while Deleting {1}, waiting 100 ms before trying again", sysex, file);
                                    }
                                    Thread.Sleep(100);
                                    try
                                    {
                                        File.Delete(file);
                                    }
                                    catch 
                                    {
                                        

                                    }

                                }


                                try
                                {
                                    // MoeTA.sp_ProgramMessageInsert("Low", "ProcessASC3Logs", DBMessage);
                                }
                                catch (SqlException ex)
                                {
                                    Console.WriteLine(ex);
                                }


                           // }
                            //else
                            //{
                            //    if (Properties.Settings.Default.WriteToConsole)
                            //    {
                            //        Console.WriteLine("Too many insertion errors to delete {0}", file);
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            if (Properties.Settings.Default.WriteToConsole)
                            {
                                Console.WriteLine("{0} while deleting file {1}", ex, file);
                            }

                            FilesToDelete.Enqueue(file);
                        }
                    }

                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("%%%Start of file Loop%%%");
                        Thread.Sleep(100);
                    }
                }
            );

                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("%%%Start of DIRECTORY  Loop%%%");
                    Thread.Sleep(100);
                }
                //CleanUpFiles(FilesToDelete);
                //});

                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("###Start of Queue Build Hit###");

                }

               
            }
             );
        }
    }
}

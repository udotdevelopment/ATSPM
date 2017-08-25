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

namespace DecodePeekLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().FindFiles();
            new Program().SaveEvents();
        }

        private void FindFiles()
        {
            string CWD = Properties.Settings.Default.PeekDatPath;
            List<string> dirList = new List<string>();
          

            

            //We have to use a gzip,exe to get everything form .gz to .dat



            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }

            if (dirList.Count > 0)
            {
                string toolsdir = Properties.Settings.Default.ToolsDirectory;
                Directory.SetCurrentDirectory(toolsdir);
                foreach (string dir in dirList)
                {
                    string decoder = Properties.Settings.Default.gzipExePath;
                    string arguments =   dir + "\\*.* -d";
                    if(Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine(decoder);
                    }
                    try
                    {
                        
                        Process p = Process.Start(decoder, arguments);
                        //Process p = Process.Start(@gzipCMD);

                        //Wait for window to finish loading.

                        //Wait for the process to exit or time out.
                        p.WaitForExit(1000);
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
                            Console.WriteLine("Exception {0} while decoding", ex);
                        }
                        
                    }


                    decoder = Properties.Settings.Default.decoderExePAth;
                        arguments = dir + " e";
                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine(decoder);
                    }
                    try
                    {

                        Process p2 = Process.Start(decoder, arguments);
                        //Process p2 = Process.Start(@DecodeCMD);

                        //Wait for window to finish loading.

                        //Wait for the process to exit or time out.
                        p2.WaitForExit(1000);
                        //Check to see if the process is still running.
                        if (p2.HasExited == false)
                        {
                            //Process is still running.
                            //Test to see if the process is hung up.
                            if (p2.Responding)
                            {
                                //Process was responding; close the main window.
                                p2.CloseMainWindow();
                            }
                            else
                            {
                                //Process was not responding; force the process to close.
                                p2.Kill();
                            }
                        }
                        p2.Dispose();
                    }

                    catch (Exception ex)
                    {
                        if (Properties.Settings.Default.WriteToConsole)
                        {
                            Console.WriteLine("Exception {0} while decoding", ex);
                        }

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
            //bool fileHasBeenRead = false;
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            TimeSpan elapsedTime = new TimeSpan();
            string CWD = Properties.Settings.Default.PeekDatPath;
            List<string> dirList = new List<string>();
            ConcurrentQueue<string> FilesToDelete = new ConcurrentQueue<string>();



            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s+"\\"+Properties.Settings.Default.PeekCSVPAth);
                if(Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine(s);
                }
            }



            var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
            Parallel.ForEach(dirList.AsEnumerable(), options, dir =>
            //Parallel.ForEach(dirList.AsEnumerable(), dir =>
            //foreach (string dir in dirList)
            {
                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine(dir);
                }

                //get the name of the directory and casting it to an int
                //This is the only way the program knows the signal number of the controller.
                string[] strsplit = dir.Split(new char[] { '\\' });
                string dirname = strsplit[strsplit.Count() - 2];
                string sigid = dirname;

                Console.WriteLine("Starting signal " + sigid);


                var options1 = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
                Parallel.ForEach(Directory.GetFiles(dir, "*.csv"), options1, file =>
                //Parallel.ForEach(Directory.GetFiles(dir, "*.csv"), file =>
                //foreach (string file in Directory.GetFiles(dir, "*.csv"))
                {
                    bool fileHasBeenRead = false;
                    MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable();

                    UniqueConstraint custUnique =
new UniqueConstraint(new DataColumn[] { elTable.Columns["SignalID"],
                                        elTable.Columns["Timestamp"], 
                                           elTable.Columns["EventCode"],
                                            elTable.Columns["EventParam"]
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
                            if (line.Contains(",ATC"))
                            {
                                //even if there is nothing but header in the file, we want it marked as being read
                                //so we can delete is later.
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(".dat"))
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
                            else if (line.Contains(",Data Log Beginning"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",Phases in use"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",Binary"))
                            {
                                fileHasBeenRead = true;
                            }
                            else if (line.Contains(",End Of"))
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
                                        elTable.AddController_Event_LogRow(sigid, timeStamp, eventCode, eventParam);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }


                                }
                                else
                                {
                                    //insertErrorCount++;
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

                    //the Signal class has a static method to insert the tableinto the DB.  We are using that.
                    string connectionString = Properties.Settings.Default.SPM.ToString();

                    MOE.Common.Business.BulkCopyOptions bulkOptions = new MOE.Common.Business.BulkCopyOptions(connectionString, Properties.Settings.Default.DestinationTableName,
                                  Properties.Settings.Default.WriteToConsole, Properties.Settings.Default.ForceNonParallel, Properties.Settings.Default.MaxThreads, Properties.Settings.Default.DeleteFiles,
                                  Properties.Settings.Default.EarliestAcceptableDate, Properties.Settings.Default.BulkCopyBatchSize, Properties.Settings.Default.BulkCopyTimeOut);


                    MOE.Common.Business.Signal.BulktoDB(elTable, bulkOptions);

                    elapsedTime = endTime - startTime;

                    if (Properties.Settings.Default.DeleteFiles)
                    {
                        try{
                            Directory.Delete(dir);
                            
                        }
                            catch
                        {

                            }
                        string d = Properties.Settings.Default.PeekDatPath;
                        foreach (string f in Directory.GetFiles(d, "*.dat"))
                        {
                            File.Delete(f);
                        }
                        //try
                        //{
                        //    //if ((errorRatio < Properties.Settings.Default.ErrorRatio) && (fileHasBeenRead))
                        //    //{
                        //    try
                        //    {
                        //        File.Delete(file);
                        //        if (Properties.Settings.Default.WriteToConsole)
                        //        {
                        //            Console.WriteLine("{0} Deleted", file);
                        //        }
                        //    }
                        //    catch (SystemException sysex)
                        //    {
                        //        if (Properties.Settings.Default.WriteToConsole)
                        //        {
                        //            Console.WriteLine("{0} while Deleting {1}, waiting 100 ms before trying again", sysex, file);
                        //        }
                        //        Thread.Sleep(100);
                        //        try
                        //        {
                        //            File.Delete(file);
                        //        }
                        //        catch (SystemException sysex2)
                        //        {
                        //            if (Properties.Settings.Default.WriteToConsole)
                        //            {
                        //                Console.WriteLine("{0} while Deleting {1}, waiting 100 ms before trying again", sysex2, file);
                        //            }

                        //        }

                        //    }


                        //    try
                        //    {
                        //        // MoeTA.sp_ProgramMessageInsert("Low", "ProcessASC3Logs", DBMessage);
                        //    }
                        //    catch (SqlException ex)
                        //    {
                        //        Console.WriteLine(ex);
                        //    }


                        //    // }
                        //    //else
                        //    //{
                        //    //    if (Properties.Settings.Default.WriteToConsole)
                        //    //    {
                        //    //        Console.WriteLine("Too many insertion errors to delete {0}", file);
                        //    //    }
                        //    //}
                        //}
                        //catch (Exception ex)
                        //{
                        //    if (Properties.Settings.Default.WriteToConsole)
                        //    {
                        //        Console.WriteLine("{0} while deleting file {1}", ex, file);
                        //    }

                        //    FilesToDelete.Enqueue(file);
                        //}
                    }

                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("%%%End of file Loop%%%");
                        Thread.Sleep(100);
                    }
                }
            );

                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("%%%End of DIRECTORY  Loop%%%");
                    Thread.Sleep(100);
                }
                //CleanUpFiles(FilesToDelete);
                //});

                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("###End of Queue Build Hit###");

                }


            }
             );
        }
    }
}

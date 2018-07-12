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

namespace DecodeSiemensLogs
{
    class Program
    {
        bool fileDecoded = true;

        static void Main(string[] args)
        {
            new Program().FindFiles();
            new Program().SaveEvents();
        }

        private void FindFiles()
        {
            string CWD = Properties.Settings.Default.LogPath;
            string CSV = Properties.Settings.Default.CSVOutPAth;
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

                    //foreach (string file in Directory.GetFiles(dir, "*.dat"))
                    //{
                    DecodeSiemens(dir);
                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("Done decoding Directory {0}", dir);
                    }
                    //}


                    //foreach (string file in Directory.GetFiles(dir, "*.csv"))
                    //{
                    //    WritetoDB(dir, file, MoeTA);
                    //    Console.WriteLine("Done writing file {0} to database", file);
                    //}


                }//);


            }
        }

        // returns true if the filename indicates new data
        // standard Sepac filename format such as SIEM_50.73.234.81_2017_09_11_1700
        // and 60 minutes per file
        private bool NewDataFile(string file, DateTime lastrecord)
        {
            string[] filename = file.Split('_');
            var filetime = new DateTime(Int32.Parse(filename[2]),
                                        Int32.Parse(filename[3]),
                                        Int32.Parse(filename[4]),
                                        Int32.Parse(filename[5].Substring(0, 2)),
                                        59,
                                        59,
                                        999);
            var newer = (lastrecord < filetime);
            return (newer);
        }
        private int ExistingRecords(string signal, string file, MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository)
        {
            string[] filename = file.Split('_');
            var start = new DateTime(Int32.Parse(filename[2]),
                                        Int32.Parse(filename[3]),
                                        Int32.Parse(filename[4]),
                                        Int32.Parse(filename[5].Substring(0, 2)),
                                        0,
                                        0,
                                        0);
            var end = start + new TimeSpan(0, 1, 59, 59, 999);
            return celRepository.GetRecordCount(signal, start, end);
        }

        private void DecodeSiemens(string dir)
        {

            //path to the decoder program
            string decoder = Properties.Settings.Default.DecoderPath;
            //time in MS to wait for the decoder ot fail
            int timeOut = Properties.Settings.Default.TimeOut;


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
                //string csvfile = file.Replace(".dat", ".csv");
                //string arguments = file + " " + csvfile;
                string arguments = "-i *.dat";
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
                    Console.WriteLine("Exception {0} while decoding directory {1}", ex, dir);
                }
                fileDecoded = false;
            }

            //If the Delete flag is checking in settings, and the file has been decoded, then delte the file.
            if (Properties.Settings.Default.DeleteFiles && fileDecoded)
            {
                DeleteFiles(dir);
            }
        }

        private void DeleteFiles(string dir)
        {
            foreach (string file in Directory.GetFiles(dir, "*.dat"))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("Exception {0} while deleting file {1}", ex, file);
                    }

                }
            }
        }

        private void WriteToConsole(string msg)
        {
            if (!Properties.Settings.Default.WriteToConsole)
                return;
            Console.WriteLine("{0}: {1}", DateTime.Now, msg);
        }
        //subroutine to write the decoded log to the database.
        //this is where most of the work is done.

        //The only way we match signalid to the collected logs is by the directory name.
        //static void WritetoDB(string dir, string file, MOEDataSetTableAdapters.QueriesTableAdapter MoeTA)
        private void SaveEvents()
        {
            int insertErrorCount = 0;
            int insertedLinecount = 0;
            double errorRatio = 0;
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            TimeSpan elapsedTime = new TimeSpan();
            string CWD = Properties.Settings.Default.LogPath;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            ConcurrentQueue<string> FilesToDelete = new ConcurrentQueue<string>();
            var lastrecords = new Dictionary<string, DateTime>();
            var countrecords = new Dictionary<string, int>();
            MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository =
            MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();

            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
                var signalID = s.Split(new char[] { '\\' }).Last();
                lastrecords.Add(signalID, celRepository.GetMostRecentRecordTimestamp(signalID));
                foreach (var file in Directory.GetFiles(s))
                {
                    countrecords.Add(file, ExistingRecords(signalID, file, celRepository));
                }
            }
            var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
            Parallel.ForEach(dirList.AsEnumerable(), options, dir =>
            //foreach (var dir in dirList.AsEnumerable())
            {
                //get the name of the directory and casting it to an int
                //This is the only way the program knows the signal number of the controller.
                string[] strsplit = dir.Split(new char[] { '\\' });
                string dirname = strsplit.Last();
                string sigid = dirname;
                var dstOffset = Math.Abs(DateTimeOffset.Now.Offset.Hours);
                WriteToConsole("Starting signal " + dirname);
                var options1 = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Properties.Settings.Default.MaxThreads) };
                //Parallel.ForEach(Directory.GetFiles(dir, "*.csv").OrderBy(f => f), options1, file =>
                foreach (var file in Directory.GetFiles(dir, "*.csv").OrderBy(f => f))
                {
                    if (countrecords[file] >= File.ReadAllLines(file).Length - 1)
                    {
                        var delete = Properties.Settings.Default.DeleteFiles;
                        WriteToConsole(String.Format("Skipping {0} {1}, we already imported this.", (delete ? "and deleting" : ""), file));
                        if (delete)
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception e)
                            {
                                WriteToConsole(String.Format("Unable to delete {0}: {1}", file, e.Message));
                            }
                        }
                        continue;
                        //return;
                    }
                    int skippedrecords = 0;
                    DataTable elTable = new DataTable();
                    elTable.Columns.Add("sigid", System.Type.GetType("System.String"));
                    elTable.Columns.Add("timeStamp", System.Type.GetType("System.DateTime"));
                    elTable.Columns.Add("eventCode", System.Type.GetType("System.Int32"));
                    elTable.Columns.Add("eventParam", System.Type.GetType("System.Int32"));
                    UniqueConstraint custUnique = new UniqueConstraint(new DataColumn[] {
                          elTable.Columns[0],
                          elTable.Columns[1],
                          elTable.Columns[2],
                          elTable.Columns[3] });
                    elTable.Constraints.Add(custUnique);
                    startTime = DateTime.Now;
                    //Siemens decoder makes the first line the IP address, so skip it.
                    foreach (string line in File.ReadAllLines(file).Skip(1))
                    {
                        //Every other line is blank.  We only care about the lines that have data, and 
                        //every data line has a comma
                        if (line.Contains(','))
                        {
                            //split the line on commas and assign each split to a var
                            string[] lineSplit = line.Split(new char[] { ',' });
                            DateTime timeStamp = new DateTime();
                            int eventCode = 0;
                            int eventParam = 0;
                            //it might happen that the character on the line are not quite right.
                            //the Try/catch stuff is an attempt to deal with that.
                            try
                            {
                                timeStamp = Convert.ToDateTime(lineSplit[0]);
                                //Siemens decoder is converting to local time from UTC, so convert back to local time
                                //Not perfect during DST transitions (at 2:00 AM twice per year)
                                timeStamp = timeStamp + TimeSpan.FromHours(dstOffset);
                                if (timeStamp < lastrecords[sigid])
                                {
                                    skippedrecords++;
                                    continue;
                                }
                                eventCode = Convert.ToInt32(lineSplit[1]);
                                eventParam = Convert.ToInt32(lineSplit[2]);
                            }
                            catch (Exception ex)
                            {
                                WriteToConsole(String.Format("{0} while converting {1} to event.  Skipping line", ex, lineSplit[0]));
                                continue;
                            }
                            try
                            {
                                elTable.Rows.Add(sigid, timeStamp, eventCode, eventParam);
                            }
                            catch (Exception ex)
                            {
                                WriteToConsole(String.Format("{0} while adding event to data table", ex.ToString()));
                            }

                        }
                    }
                    WriteToConsole(String.Format("{0} has been parsed. Skipped {1} old records", file, skippedrecords));

                    //Do the Math to find out if the error ratio is intolerably high before deleting the file
                    if (insertErrorCount > 0)
                    {
                        errorRatio = Convert.ToDouble(insertErrorCount) / Convert.ToDouble((insertedLinecount + insertErrorCount));
                    }
                    else
                    {
                        errorRatio = 0;
                    }

                    string connectionString = Properties.Settings.Default.SPM.ToString();
                    MOE.Common.Business.BulkCopyOptions bulkOptions = new MOE.Common.Business.BulkCopyOptions(connectionString, Properties.Settings.Default.DestinationTableName,
                                 Properties.Settings.Default.WriteToConsole, Properties.Settings.Default.forceNonParallel, Properties.Settings.Default.MaxThreads, Properties.Settings.Default.DeleteFile,
                                 Properties.Settings.Default.EarliestAcceptableDate, Properties.Settings.Default.BulkCopyBatchSize, Properties.Settings.Default.BulkCopyTimeOut);

                    endTime = DateTime.Now;

                    //the Signal class has a static methods to insert the table into the DB.  We are using that.
                    MOE.Common.Business.Signal.BulktoDB(elTable, bulkOptions);
                    elapsedTime = endTime - startTime;

                    if (Properties.Settings.Default.DeleteFiles)
                    {
                        try
                        {
                            File.Delete(file);
                            WriteToConsole(String.Format("{0} Deleted", file));
                        }
                        catch (SystemException sysex)
                        {
                            WriteToConsole(String.Format("{0} while Deleting {1}, waiting 100 ms before trying again", sysex, file));
                            Thread.Sleep(100);
                            try
                            {
                                File.Delete(file);
                            }
                            catch (SystemException sysex2)
                            {
                                WriteToConsole(String.Format("{0} while Deleting {1}, giving up", sysex2, file));
                            }
                        }

                        catch (Exception ex)
                        {
                            WriteToConsole(String.Format("{0} while deleting file {1}", ex, file));
                            FilesToDelete.Enqueue(file);
                        }
                    }
                }
                //);  
            }
            );
        }
    }
}
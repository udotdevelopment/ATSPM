using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace DecodeASC3Logs
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().FindFiles();
        }
    
    private void FindFiles()
    {
            string CWD = Properties.Settings.Default.ASC3LogsPath;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();

            // MOEDataSetTableAdapters.QueriesTableAdapter MoeTA = new MOEDataSetTableAdapters.QueriesTableAdapter();



            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }
            //if (dirList.Count > 1)
            //{
            //foreach (string dir in dirList)
            //{
            //    foreach (string file in Directory.GetFiles(dir, "*.dat"))
            //    {
            //        fileList.Add(file);
            //    }
            //}

            //foreach (string file in Directory.GetFiles(CWD, "*.dat"))
            //{
            //    fileList.Add(file);
            //}

           // if (fileList.Count > 0)
        if(dirList.Count >0)
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
                        DecodeASC3(dir, file);
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

        private void DecodeASC3(string dir, string file)
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
                Process p = Process.Start(decoder, file);

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
    }
}

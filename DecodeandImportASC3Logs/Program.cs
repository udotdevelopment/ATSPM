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

namespace DecodeandImportASC3Logs
{
    class Program
    {
        public static ParallelOptions options = null;

        
         

        static void Main(string[] args)
        {
            //check Parallelism settings
            
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


            string CWD = Properties.Settings.Default.ASC3LogsPath;
            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();
            ConcurrentQueue<string> FilesToDelete = new ConcurrentQueue<string>();

            foreach (string s in Directory.GetDirectories(CWD))
            {
                dirList.Add(s);
            }


            SimplePartitioner<string> sp = new SimplePartitioner<string>(dirList);

            Parallel.ForEach(sp, options, dir =>
            //foreach (string dir in dirList)
            {
                if (Properties.Settings.Default.WriteToConsole)
                {
                    Console.WriteLine("-----------------------------Starting Signal " + dir);
                }

                
                //get the name of the directory 
                //This is the only way the program knows the signal number of the controller.
                string[] strsplit = dir.Split(new char[] { '\\' });
                string dirname = strsplit.Last();
                string sigid = dirname;
                //fileList = Directory.GetFiles(dir, "*.dat");
                SimplePartitioner<string> sp2 = new SimplePartitioner<string>(Directory.GetFiles(dir, "*.dat"));

                Parallel.ForEach(sp2, options, file =>
                //foreach (string file in Directory.GetFiles(dir, "*.dat"))
                {
                    if (Properties.Settings.Default.WriteToConsole)
                    {
                        Console.WriteLine("Opening File " + file);
                    }
                    try
                    {
                        bool fileRead = MOE.Common.Business.Signal.DecodeASC3File(file, sigid);

                        if (Properties.Settings.Default.DeleteFiles && fileRead)
                        {
                            try
                            {
                                File.Delete(file);
                                if (Properties.Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine("-------"+ file + " Was Deleted");
                                }
                            }
                            finally
                            {
                                if (Properties.Settings.Default.WriteToConsole)
                                {
                                    Console.WriteLine("Error Deleting " + file + "-----------");
                                }
                            }
                        }
                        else
                        {

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        if (Properties.Settings.Default.WriteToConsole)
                        {
                            Console.WriteLine(ex + " while working with file " + file);
                        }
                    }
                }
                );
            }
        );
        }

        


    }
}

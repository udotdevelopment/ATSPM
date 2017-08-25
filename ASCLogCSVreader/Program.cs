using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
//using MOE.Common;


using LumenWorks.Framework.IO.Csv;


namespace ASCLogCSVreader
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvReader csv = new CsvReader(new StreamReader("c:\\ECON_172.17.160.28_2014_04_25_1632.csv"),true);
            csv.SkipEmptyLines = true;
            string signalID = "1001";
            //List<MOE.Common.Business.ControllerEvent> events = new List<MOE.Common.Business.ControllerEvent>();

         while (csv.ReadNextRecord())
        {
            
           // MOE.Common.Business.ControllerEvent event = new MOE.Common.Business.ControllerEvent(csv[0], csv[1]); 

        }
        Console.ReadLine();
        }
        }
    }


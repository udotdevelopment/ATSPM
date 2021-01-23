using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common;
namespace MemoryAggregation
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var settings = config.AppSettings.Settings;

            var dataAggregation = new MOE.Common.Business.DataAggregation.DataAggregation();
            dataAggregation.StartMemoryAggregation(args);
        }
    }
}

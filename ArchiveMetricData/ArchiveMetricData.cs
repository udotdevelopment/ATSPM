using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using MOE.Common;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MOE.Common.Models;

namespace ArchiveMetricData
{
 


    class ArchiveMetricData
    {
        public static DateTime ToNearestQuarterHour(DateTime input)
        {
            int i = (int)(Math.Round(input.Minute / 15D) * 15);
            if (i == 60)
            {
                return new DateTime(input.Year, input.Month, input.Day, input.Hour + 1, 0, 0);
            }
            else
            {
                return new DateTime(input.Year, input.Month, input.Day, input.Hour, (int)(Math.Round(input.Minute / 15D) * 15), 0);
            }
        }
        private static DateTime startDate;
        private static DateTime endDate;
        private static MOE.Common.Models.Repositories.ISignalAggregationDataRepository aggregationRepository =
            MOE.Common.Models.Repositories.SignalAggregationDataRepositoryRepositoryFactory.Create();
        static void Main(string[] args)
        {
            
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            int binSize = Convert.ToInt32(appSettings["BinSize"]);
            SetStartEndDate(args);
            while (startDate < endDate)
            {
                Console.WriteLine("Starting " + startDate.ToShortDateString());
                MOE.Common.Models.Repositories.ISignalsRepository signalsRepository =
                    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                var signals = signalsRepository.GetAllEnabledSignals();
                var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
                Parallel.ForEach(signals, options, signal =>
                {
                    for (DateTime dt = startDate; dt < startDate.AddDays(1); dt = dt.AddMinutes(binSize))
                    {
                        ProcessSignal(signal, dt, dt.AddMinutes(binSize));
                    }
                });
                startDate = startDate.AddDays(1);
            }
        }

        private static void SetStartEndDate(string[] args)
        {
            startDate = DateTime.Today;
            if (args.Length == 1)
            {
                startDate = Convert.ToDateTime(args[0]);
                endDate = DateTime.Today;
            }
            else if (args.Length == 2)
            {
                startDate = Convert.ToDateTime(args[0]);
                endDate = Convert.ToDateTime(args[1]).AddDays(1);
            }
            else
            {
                startDate = DateTime.Today.AddDays(-1);//archivedMetricsRepository.GetLastArchiveRunDate()).AddMinutes(15);
                endDate = DateTime.Today;
            }
        }

        private static void ProcessSignal(Signal signal, DateTime startTime, DateTime endTime)
        {
            SignalAggregationData signalAggregationData = new SignalAggregationData();
            signalAggregationData.TotalCycles = aggregationRepository.GetTotalCycles(signal.SignalID, startTime, endTime);
            signalAggregationData.AddCyclesInTransition = aggregationRepository.GetSubtractCycles(signal.SignalID, startTime, endTime);
            signalAggregationData.DwellCyclesInTransition = aggregationRepository.GetDwellCycles(signal.SignalID, startTime, endTime);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOE.Common;
using System.Threading;
using System.Threading.Tasks;

namespace EnableSPMLogginforASC3
{
    class Program
    {
        static void Main(string[] args)
        {
            MOE.Common.Data.Signals.SignalsInfoDataTable signalsTable = new MOE.Common.Data.Signals.SignalsInfoDataTable();
            MOE.Common.Data.SignalsTableAdapters.SignalsInfoTableAdapter signalsTA = new MOE.Common.Data.SignalsTableAdapters.SignalsInfoTableAdapter();

            signalsTA.Fill(signalsTable);
            //List<MOE.Common.Business.Signal> signals = new List<MOE.Common.Business.Signal>();

            //foreach (MOE.Common.Data.Signals.SignalsInfoRow row in signalsTable)
            //{
            //    MOE.Common.Business.Signal sig = new MOE.Common.Business.Signal();
            //    sig.IpAddress = row.IP_Address;
            //    signals.Add(sig);
            //    //MOE.Common.Business.Signal.EnableLogging(row.IP_Address, Properties.Settings.Default.SNMPRetry, Properties.Settings.Default.SNMPTimeout);
                
            //}

            Parallel.ForEach(signalsTable.AsEnumerable(), row =>
                    {
                        MOE.Common.Business.Signal.EnableLogging(row.IP_Address, Properties.Settings.Default.SNMPRetry, Properties.Settings.Default.SNMPTimeout, Properties.Settings.Default.SNMPPort);
                    });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MOE.Common.Business.PEDDelay;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PedDelayOptions : MetricOptions
    {
        public PedDelayOptions(string signalId, DateTime startDate, DateTime endDate, double? yAxisMax)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;
        }

        public PedDelayOptions()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            YAxisMax = 3;
            Y2AxisMax = 10;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var pds = new PedDelaySignal(SignalID, StartDate, EndDate);
            foreach (var p in pds.PedPhases)
                if (p.Cycles.Count > 0)
                {
                    var pdc = new PEDDelayChart(this, p);
                    var chart = pdc.Chart;
                    var chartName = CreateFileName();
                    chart.SaveImage(MetricFileLocation + chartName);
                    ReturnList.Add(MetricWebPath + chartName);
                }
            return ReturnList;
        }
    }
}
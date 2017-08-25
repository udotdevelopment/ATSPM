using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PedDelayOptions: MetricOptions
    {

        public PedDelayOptions(string signalID, DateTime startDate, DateTime endDate, double? yAxisMax)
        {
            SignalID = signalID;
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
            MOE.Common.Business.PEDDelay.PedDelaySignal pds = new MOE.Common.Business.PEDDelay.PedDelaySignal(SignalID,
     StartDate, EndDate);
            foreach (MOE.Common.Business.PEDDelay.PedPhase p in pds.PedPhases)
            {
                if (p.Cycles.Count > 0)
                {
                    MOE.Common.Business.PEDDelay.PEDDelayChart pdc = 
                        new MOE.Common.Business.PEDDelay.PEDDelayChart(this, p);
                    Chart chart = pdc.chart;
                    string chartName = CreateFileName();
                    chart.SaveImage(MetricFileLocation + chartName);
                    ReturnList.Add(MetricWebPath + chartName);
                }
            }
            return ReturnList;
        }
    }
    
}

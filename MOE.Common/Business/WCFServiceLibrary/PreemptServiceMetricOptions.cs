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

namespace MOE.Common.Business.WCFServiceLibrary

{
    [DataContract]
    public class PreemptServiceMetricOptions : MetricOptions
    {
        public PreemptServiceMetricOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            YAxisMax = yAxisMax;        
            //MetricTypeID = metricTypeID;
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnString = new List<string>();

            MOE.Common.Business.ControllerEventLogs eventsTable = new MOE.Common.Business.ControllerEventLogs();

            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);
            if (eventsTable.Events.Count > 0)
            {
                MOE.Common.Business.Preempt.PreemptServiceMetric psChart = 
                    new MOE.Common.Business.Preempt.PreemptServiceMetric(this, eventsTable);
                Chart chart = psChart.chart;
                //Create the File Name

                string chartName = CreateFileName();


                //Save an image of the chart

                chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                returnString.Add(MetricWebPath + chartName);
                    

        
            }


            return returnString;

        }
    }
}

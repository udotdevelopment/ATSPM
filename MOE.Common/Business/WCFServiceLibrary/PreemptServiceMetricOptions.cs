using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Preempt;

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
            var returnString = new List<string>();

            var eventsTable = new ControllerEventLogs();

            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);
            if (eventsTable.Events.Count > 0)
            {
                var psChart = new PreemptServiceMetric(this, eventsTable);
                var chart = psChart.chart;
                var chartName = CreateFileName();
                chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                returnString.Add(MetricWebPath + chartName);
            }
            return returnString;
        }
    }
}
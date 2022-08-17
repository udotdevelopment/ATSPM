using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachDelayOptions : MetricOptions
    {
        public ApproachDelayOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax,
            double y2AxisMax,
            int binSize, bool showPlanStatistics, bool showDelayPerVehicle)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            ShowPlanStatistics = showPlanStatistics;
            MetricTypeID = 8;
            ShowTotalDelayPerHour = ShowTotalDelayPerHour;
            ShowDelayPerVehicle = showDelayPerVehicle;
            StartDate = startDate;
            EndDate = endDate;
        }

        public ApproachDelayOptions()
        {
            BinSizeList = new List<int>() { 5, 15 };
            MetricTypeID = 8;
            SetDefaults();
        }

        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }

        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }

        [DataMember]
        [Display(Name = "Show Total Delay PerHour")]
        public bool ShowTotalDelayPerHour { get; set; }

        [DataMember]
        [Display(Name = "Show Delay Per Vehicle")]
        public bool ShowDelayPerVehicle { get; set; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalphasecollection =
                new SignalPhaseCollection(StartDate,
                    EndDate, SignalID,
                    ShowPlanStatistics, SelectedBinSize, 8);


            foreach (var signalPhase in signalphasecollection.SignalPhaseList)
            {
                var delayChart = new DelayChart(this, signalPhase);

                var chart = delayChart.Chart;

                var chartName = CreateFileName();

                var removethese = new List<Title>();

                foreach (var t in chart.Titles)
                    if (t.Text == "" || t.Text == null)
                        removethese.Add(t);
                foreach (var t in removethese)
                    chart.Titles.Remove(t);

                //Save an image of the chart
                chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

                ReturnList.Add(MetricWebPath + chartName);
            }
            return ReturnList;
        }
    }
}
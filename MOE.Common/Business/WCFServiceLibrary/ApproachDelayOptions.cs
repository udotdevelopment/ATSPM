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
            int binSize, bool showPlanStatistics, int metricTypeID, bool showDelayPerVehicle,
            bool showTotalDelayPerHour)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            ShowPlanStatistics = showPlanStatistics;
            MetricTypeID = metricTypeID;
            ShowTotalDelayPerHour = ShowTotalDelayPerHour;
            ShowDelayPerVehicle = showDelayPerVehicle;
        }

        public ApproachDelayOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
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

        public void SetDefaults()
        {
            YAxisMax = 15;
            Y2AxisMax = 20000;
            ShowPlanStatistics = true;
            ShowTotalDelayPerHour = true;
            ShowDelayPerVehicle = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var location = GetSignalLocation();

            var signalphasecollection =
                new SignalPhaseCollection(StartDate,
                    EndDate, SignalID,
                    ShowPlanStatistics, SelectedBinSize, 8);


            foreach (var signalPhase in signalphasecollection.SignalPhaseList)
            {
                var delayChart = new DelayChart(this, signalPhase);

                var chart = delayChart.chart;

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
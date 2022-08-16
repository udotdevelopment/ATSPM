using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class AoROptions : MetricOptions
    {
        public AoROptions()
        {
            BinSizeList = new List<int>() { 5, 15 };
            MetricTypeID = 9;
            SetDefaults();
        }

        public AoROptions(string signalId, DateTime start, DateTime end,  bool showPlanStatistics, int selectedBinSize)
        {
            BinSizeList = new List<int>() { 5, 15 };
            MetricTypeID = 9;
            StartDate = start;
            EndDate = end;
            SignalID = signalId;
            ShowPlanStatistics = showPlanStatistics;
            SelectedBinSize = selectedBinSize;

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

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var returnList = new List<string>();
            var signalphasecollection = new SignalPhaseCollection(this, false, SelectedBinSize);
            if (signalphasecollection.SignalPhaseList.Count > 0)
                foreach (var signalPhase in signalphasecollection.SignalPhaseList)
                {
                    var aorChart = new ArriveOnRedChart(this, signalPhase);
                    var chart = aorChart.Chart;
                    var chartName = CreateFileName();
                    var removethese = new List<Title>();
                    foreach (var t in chart.Titles)
                        if (string.IsNullOrEmpty(t.Text))
                            removethese.Add(t);
                    foreach (var t in removethese)
                        chart.Titles.Remove(t);
                    chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                    returnList.Add(MetricWebPath + chartName);
                }
            return returnList;
        }


    }
}
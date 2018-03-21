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

            SetDefaults();
        }

        public AoROptions(string signalId, DateTime start, DateTime end,  bool showPlanStatistics, int selectedBinSize)
        {
            SetDefaults();
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

        public void SetDefaults()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 9;
            YAxisMax = null;
            ShowPlanStatistics = true;
            SelectedBinSize = 15;
        }

        

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var returnList = new List<string>();
            var signalphasecollection =
                new SignalPhaseCollection(StartDate, EndDate, SignalID, false, SelectedBinSize, MetricTypeID);
            if (signalphasecollection.SignalPhaseList.Count > 0)
                foreach (var signalPhase in signalphasecollection.SignalPhaseList)
                {
                    var aorChart = new ArriveOnRedChart(this, signalPhase);
                    var chart = aorChart.chart;
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
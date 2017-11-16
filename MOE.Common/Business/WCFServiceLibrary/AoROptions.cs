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
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{

    [DataContract]
    public class AoROptions : MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }
        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }


        public AoROptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 9;
            SetDefaults();
        }
        public void SetDefaults()
        {
            YAxisMax = null;
            ShowPlanStatistics = true;
            SelectedBinSize = 15;
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();
            SignalPhaseCollection signalphasecollection = new SignalPhaseCollection(StartDate, EndDate, SignalID, false, SelectedBinSize, 9);
            if (signalphasecollection.SignalPhaseList.Count > 0)
            {
                foreach (SignalPhase signalPhase in signalphasecollection.SignalPhaseList)
                {
                    ArriveOnRedChart aorChart = new ArriveOnRedChart(this, signalPhase);
                    Chart chart = aorChart.chart;
                    string chartName = CreateFileName();
                    var removethese = new List<Title>();
                    foreach (Title t in chart.Titles)
                    {
                        if (string.IsNullOrEmpty(t.Text))
                        {
                            removethese.Add(t);
                        }
                    }
                    foreach (Title t in removethese)
                    {
                        chart.Titles.Remove(t);
                    }
                    chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                    returnList.Add(MetricWebPath + chartName);
                }
            }
            return returnList;
        }
    }
}

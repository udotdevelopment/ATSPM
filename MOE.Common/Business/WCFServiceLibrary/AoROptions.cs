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

        public AoROptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, int binSize, bool showPlanStatistics)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            ShowPlanStatistics = showPlanStatistics;
        }
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

            MOE.Common.Business.SignalPhaseCollection signalphasecollection =
              new MOE.Common.Business.SignalPhaseCollection(StartDate,
                  EndDate, SignalID,
           false, SelectedBinSize, 9);

            string location = GetSignalLocation();

            if (signalphasecollection.SignalPhaseList.Count > 0)
            {
                foreach (MOE.Common.Business.SignalPhase signalPhase in signalphasecollection.SignalPhaseList)
                {
                    MOE.Common.Business.ArriveOnRedChart AoRChart = 
                        new MOE.Common.Business.ArriveOnRedChart(this, signalPhase);
                    Chart chart = AoRChart.chart;

                    //Create the File Name

                    string chartName = CreateFileName();
                    

                    var removethese = new List<Title>();

                    foreach (Title t in chart.Titles)
                    {
                        if (t.Text == "" || t.Text == null)
                        {
                            removethese.Add(t);
                        }
                    }
                    foreach (Title t in removethese)
                    {
                        chart.Titles.Remove(t);
                    }


                    //Save an image of the chart
                    chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                    returnList.Add(MetricWebPath + chartName);

                }
            }
            return returnList;
        }
    }
}

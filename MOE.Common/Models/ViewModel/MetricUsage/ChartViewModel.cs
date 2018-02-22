using System.Collections.Generic;
using MOE.Common.Business.ActionLog;

namespace MOE.Common.Models.ViewModel.MetricUsage
{
    public class ChartViewModel
    {
        public List<ChartData> ChartData { get; set; }
        public string ReportTitle { get; set; }
        public string CanvasName { get; set; }
        public string YAxisDescription { get; set; }
        public string ChartType { get; set; }

        public List<string> GetColorList()
        {
            var colors = new List<string>();
            colors.Add("rgba(178,4,0,1)");
            colors.Add("rgba(235,126,110,1)");
            colors.Add("rgba(239,160,43,1)");
            colors.Add("rgba(253,208,125,1)");
            colors.Add("rgba(185,204,18,1)");
            colors.Add("rgba(95,147,23,1)");
            colors.Add("rgba(44,92,18,1)");
            colors.Add("rgba(101,114,148,1)");
            colors.Add("rgba(58,61,115,1)");
            colors.Add("rgba(25,17,64,1)");
            colors.Add("rgba(178,4,0,.6)");
            colors.Add("rgba(235,126,110,.6)");
            colors.Add("rgba(239,160,43,.6)");
            colors.Add("rgba(253,208,125,.6)");
            colors.Add("rgba(185,204,18,.6)");
            colors.Add("rgba(95,147,23,.6)");
            colors.Add("rgba(44,92,18,.6)");
            colors.Add("rgba(101,114,148,.6)");
            colors.Add("rgba(58,61,115,.6)");
            colors.Add("rgba(25,17,64,.6)");
            return colors;
        }
    }
}
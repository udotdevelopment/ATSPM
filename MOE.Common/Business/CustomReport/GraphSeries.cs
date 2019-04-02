using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.CustomReport
{
    public class GraphSeries
    {
        public GraphSeries(int eventCode, Color color, SeriesChartType type)
        {
            EventCode = eventCode;
            SeriesColor = color;
            SeriesType = type;
        }

        public int EventCode { get; set; }

        public Color SeriesColor { get; set; }

        public SeriesChartType SeriesType { get; set; }
    }
}
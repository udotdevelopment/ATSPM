using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business.CustomReport
{
    public class GraphSeries
    {
        private int _EventCode;

        public int EventCode
        {
            get { return _EventCode; }
            set { _EventCode = value; }
        }

        private Color _SeriesColor;

        public Color SeriesColor
        {
            get { return _SeriesColor; }
            set { _SeriesColor = value; }
        }

        private SeriesChartType _SeriesType;

        public SeriesChartType SeriesType
        {
            get { return _SeriesType; }
            set { _SeriesType = value; }
        }
        
        public GraphSeries(int eventCode, Color color, SeriesChartType type)
        {
            _EventCode = eventCode;
            _SeriesColor = color;
            _SeriesType = type;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;


namespace MOE.Common.Business.Inrix
{
    public class GraphLine
    {
        protected   Color lineColor;
        public Color LineColor
        {
            get
            {
                return lineColor;
            }

        }

        protected ChartDashStyle lineStyle;
        public ChartDashStyle LineStyle
        {
            get
            {
                return lineStyle;
            }
        }

        protected int lineWidth;
        public int LineWidth
        {
            get
            {
                return lineWidth;
            }
        }

        protected int confidenceScore;
        public int Confidence
        {
            get
            {
                return confidenceScore;
            }
        }

        protected int lineNumber;
        public int LineNumber
        {
            get
            {
                return lineNumber;
            }
            set
            {
                lineNumber = value;
            }
        }

        protected List<DayOfWeek> dayTypes;
        public List<DayOfWeek> DayTypes
        {
            get
            {
                return dayTypes;
            }
        }

        protected string dayTypesLetters;
        public string DayTypesLetters
        {
            get
            {
                return dayTypesLetters;
            }
        }

        protected string startHour;
        public string StartHour
        {
            get
            {
                return startHour;
            }
        }

        protected string endHour;
        public string EndHour
        {
            get
            {
                return endHour;
            }
        }

        protected DateTime startDay;
        public DateTime StartDay
        {
            get
            {
                return startDay;
            }
        }

        protected DateTime endDay;
        public DateTime EndDay
        {
            get
            {
                return endDay;
            }
        }



        public GraphLine(string color, string style, int width, List<DayOfWeek> daytypes,
            DateTime startday, DateTime endday, string starthour, string endhour, int confidence, int linenumber)
        {
            confidenceScore = confidence;
            lineNumber = linenumber;

            switch (color)
            {
                case "Red":
                    lineColor = Color.Red;
                break;
                case "Purple":
                lineColor = Color.Purple;
                break;
                case "Green":
                lineColor = Color.Green;
                break;
                case "Blue":
                lineColor = Color.Blue;
                break;
                case "Yellow":
                lineColor = Color.Yellow;
                break;
                case "Black":
                lineColor = Color.Black;
                break;
                case "Orange":
                lineColor = Color.Orange;
                break;
                default:
                    lineColor = Color.Black;
                    break;
            }

            switch (style)
            {
                case "Dashed":
                    lineStyle = ChartDashStyle.Dash;
                    break;
                case "Dotted":
                    lineStyle = ChartDashStyle.Dot;
                    break;
                case "Solid":
                    lineStyle = ChartDashStyle.Solid;
                    break;
                default:
                    lineStyle = ChartDashStyle.Solid;
                    break;
            }

            lineWidth = width;
            dayTypes = daytypes;

            //StringBuilder sb = new StringBuilder(daytypes);
            //sb.Replace("1", "Su");
            //sb.Replace("2", "Mo");
            //sb.Replace("3", "Tu");
            //sb.Replace("4", "We");
            //sb.Replace("5", "Th");
            //sb.Replace("6", "Fr");
            //sb.Replace("7", "Sa");

            //dayTypesLetters = sb.ToString();


            startDay = startday;
            endDay = endday;
            startHour = starthour;
            endHour = endhour;

        }
    }
}

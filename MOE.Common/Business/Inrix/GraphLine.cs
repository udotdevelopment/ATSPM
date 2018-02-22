using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.Inrix
{
    public class GraphLine
    {
        protected int confidenceScore;

        protected List<DayOfWeek> dayTypes;

        protected string dayTypesLetters;

        protected DateTime endDay;

        protected string endHour;
        protected Color lineColor;

        protected int lineNumber;

        protected ChartDashStyle lineStyle;

        protected int lineWidth;

        protected DateTime startDay;

        protected string startHour;


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

        public Color LineColor => lineColor;

        public ChartDashStyle LineStyle => lineStyle;

        public int LineWidth => lineWidth;

        public int Confidence => confidenceScore;

        public int LineNumber
        {
            get => lineNumber;
            set => lineNumber = value;
        }

        public List<DayOfWeek> DayTypes => dayTypes;

        public string DayTypesLetters => dayTypesLetters;

        public string StartHour => startHour;

        public string EndHour => endHour;

        public DateTime StartDay => startDay;

        public DateTime EndDay => endDay;
    }
}
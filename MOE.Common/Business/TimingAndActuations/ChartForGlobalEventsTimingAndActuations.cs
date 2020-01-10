using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using CsvHelper.Configuration.Attributes;
using Lextm.SharpSnmpLib;
using Microsoft.Extensions.Options;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using Exception = System.Exception;

namespace MOE.Common.Business.TimingAndActuations
{
    class ChartForGlobalEventsTimingAndActuations
    {
        public Chart Chart { get; set; }
 
        //private GlobalGetDataTimingAndActuations GlobalGetDataTimingAndActuations { get; set; }

        private double _yValue;
        private int _dotSize;
        
        public ChartForGlobalEventsTimingAndActuations(GlobalGetDataTimingAndActuations globalGetDataTimingAndActuations, TimingAndActuationsOptions options)
        {
            var GlobalDataTimingAndActuations = globalGetDataTimingAndActuations;
            var Options = options;
            var orginalEndDate = Options.EndDate;
            var reportTimespan = Options.EndDate - Options.StartDate;
            var totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 121)  // remove extra minute to timefrom defaults
            {
                Options.EndDate =Options.EndDate.AddMinutes(-1);
            }
            Chart = ChartFactory.CreateDefaultChart(Options);
            SetChartTitle(Options);
            SetGlobalEvents(GlobalDataTimingAndActuations, Options);
            SetYAxisLabels(Options);
            Options.EndDate = orginalEndDate;
        }

        private void SetGlobalEvents(GlobalGetDataTimingAndActuations globalGetDataTimingAndActuations, TimingAndActuationsOptions options)
        {
            _yValue = 0.5;
            _dotSize = 1;
            if (options.DotAndBarSize > 0) _dotSize = options.DotAndBarSize;
            foreach (var globalCustomEventElement in globalGetDataTimingAndActuations.GlobalCustomEvents)
            {
                var globalEventsSeries = new Series
                {
                    ChartType = SeriesChartType.Point,
                    XValueType = ChartValueType.DateTime
                };
                var globalCustomEvents = globalCustomEventElement.Value;
                if (globalCustomEvents.Count <= 0) continue;
                for (int i = 0; i < globalCustomEvents.Count; i++)
                {
                    var p0 = globalEventsSeries.Points.AddXY(globalCustomEvents[i].Timestamp.ToOADate(), _yValue);
                    globalEventsSeries.Points[p0].Color = Color.Transparent;
                    globalEventsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                    globalEventsSeries.Points[p0].MarkerColor = Color.Black;
                    globalEventsSeries.Points[p0].MarkerSize = _dotSize;
                }
                if (globalEventsSeries.Points.Count <= 0) return;
                globalEventsSeries.Name = globalCustomEventElement.Key;
                Chart.Series.Add(globalEventsSeries);
                _yValue += 1.0;
            }
        }
        
        private void SetYAxisLabels(TimingAndActuationsOptions options)
        {
            var yMaximum = Math.Round(_yValue += 0.8, 0);
            var height = (25.0 * yMaximum + 180.0);
            if (height < 200) height = 300;
            Chart.Height = (Unit)height;
            Chart.ChartAreas[0].AxisY.Interval = 1;
            var bottomOffset = 0;
            var topOffset = bottomOffset + 1;
            var timingAxisLabel = new CustomLabel(bottomOffset, topOffset, "  ", 1, LabelMarkStyle.None);
            foreach (var t in Chart.Series)
            {
                bottomOffset = (int) t.Points[0].YValues[0];
                topOffset = bottomOffset + 1;
                timingAxisLabel = new CustomLabel(bottomOffset, topOffset, t.Name, 0,
                    LabelMarkStyle.None);
                Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            }
            bottomOffset++;
            topOffset = bottomOffset + 1;
            var vehicleTimingAxisLabel = new CustomLabel(bottomOffset, topOffset, "Vehicle Signal Display                    .", 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(vehicleTimingAxisLabel);
        }

        private void SetChartTitle(TimingAndActuationsOptions options)
        {
            Chart.ChartAreas[0].AxisY.Title = "";
            Chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.Title = "";
            Chart.Titles.Add(ChartTitleFactory.GetChartName(options.MetricTypeID));
            Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                options.SignalID, options.StartDate,options.EndDate));
        }
    }
}


    


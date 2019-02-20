using System;
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
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using Exception = System.Exception;

namespace MOE.Common.Business.TimingAndActuations
{
    class TimingAndActuationsChartForGlobalEvents
    {
        public Chart Chart { get; set; }
 
        private TimingAndActuationsGlobalGetData TimingAndActuationsGlobalGetData { get; }

        private double _yValue;
        private int _dotSize;
        private DateTime orginalEndDate;

        public TimingAndActuationsChartForGlobalEvents(string SignalID,
            TimingAndActuationsGlobalGetData timingAndActuationsGlobalGetData)
        {
            _yValue = 0.5;
            _dotSize = 1;
            TimingAndActuationsGlobalGetData = timingAndActuationsGlobalGetData;

            if (!TimingAndActuationsGlobalGetData.Options.GlobalCustomCode1.HasValue &&
                !TimingAndActuationsGlobalGetData.Options.GlobalCustomCode2.HasValue) return;
            if (TimingAndActuationsGlobalGetData.Options.DotAndBarSize > 0)
                _dotSize = TimingAndActuationsGlobalGetData.Options.DotAndBarSize;
            orginalEndDate = TimingAndActuationsGlobalGetData.Options.EndDate;
            var reportTimespan = TimingAndActuationsGlobalGetData.Options.EndDate -
                                 TimingAndActuationsGlobalGetData.Options.StartDate;
            var totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 121)
            {
                TimingAndActuationsGlobalGetData.Options.EndDate =
                    TimingAndActuationsGlobalGetData.Options.EndDate.AddMinutes(-1);
            }
            Chart = ChartFactory.CreateDefaultChart(TimingAndActuationsGlobalGetData.Options);
            SetChartTitle();
            SetGlobalEvents();
            SetYAxisLabels();
            TimingAndActuationsGlobalGetData.Options.EndDate = orginalEndDate;
        }

        private void SetGlobalEvents()
        {
            if (TimingAndActuationsGlobalGetData.GlobalCustomEvents == null) return;
            if (!TimingAndActuationsGlobalGetData.GlobalCustomEvents.Any()) return;
            
            foreach (var globalCustomEventElement in TimingAndActuationsGlobalGetData.GlobalCustomEvents)
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
               // if (globalEventsSeries.Points.Count <= 0) return;
                globalEventsSeries.Name = globalCustomEventElement.Key;
                Chart.Series.Add(globalEventsSeries);
                _yValue += 1.0;
            }
        }
        /********************************************************************************************

        lanesProcessed = 0;
            var phaseEventsSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };
            foreach (var phaseEventElement in TimingAndActuationsForPhase.PhaseCustomEvents)
            {
                // if (!phaseEventElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    phaseEventsSeries = new Series
                    {
                        ChartType = SeriesChartType.Point,
                        XValueType = ChartValueType.DateTime
    };
}
                else
                {
                    _laneOffset = (double) ++_lanesProcessed* 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }
                var phaseCustomEvents = phaseEventElement.Value;
                var lastItem = phaseCustomEvents.Count;
                if (lastItem <= 0) continue;
                for (var i = 0; i<lastItem; i++)
                {
                    var p0 = phaseEventsSeries.Points.AddXY(phaseCustomEvents[i].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    phaseEventsSeries.Points[p0].Color = Color.Transparent;
                    phaseEventsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                    phaseEventsSeries.Points[p0].MarkerColor = Color.Black;
                    phaseEventsSeries.Points[p0].MarkerSize = _dotSize;
                }

                if (phaseEventsSeries.Points.Count <= 0) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    phaseEventsSeries.Name = phaseEventElement.Key;
                    Chart.Series.Add(phaseEventsSeries);
                    _yValue += 1.0;
                }
            }

            if (phaseEventsSeries.Points.Count <= 0) return;
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                phaseEventsSeries.Name = "Combined Lanes for Custom Phase Events";
                Chart.Series.Add(phaseEventsSeries);
                _yValue += 1.0;
            }

************************************************************************/




private void SetYAxisLabels()
        {
            var yMaximum = Math.Round(_yValue += 0.8, 0);
            Chart.ChartAreas[0].AxisY.Maximum = yMaximum;
            Chart.Height = (Unit) (25.0 * yMaximum + 180.0);
            Chart.ChartAreas[0].AxisY.Interval = 1;
            var bottomOffset = 0;
            var topOffset = bottomOffset + 1;
            var timingAxisLabel = new CustomLabel(bottomOffset, topOffset, "                                         ", 1,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
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
            var VehicleTimingAxisLabel = new CustomLabel(bottomOffset, topOffset, "Timing And Actutation Signal Vehicle Information", 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(VehicleTimingAxisLabel);
        }

        private void SetChartTitle()
        {
            Chart.ChartAreas[0].AxisY.Title = "";
            Chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.Title = "";
            Chart.Titles.Add(ChartTitleFactory.GetChartName(TimingAndActuationsGlobalGetData.Options.MetricTypeID));
            Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                TimingAndActuationsGlobalGetData.Options.SignalID, TimingAndActuationsGlobalGetData.Options.StartDate,
                TimingAndActuationsGlobalGetData.Options.EndDate));
        }
    }
}


    


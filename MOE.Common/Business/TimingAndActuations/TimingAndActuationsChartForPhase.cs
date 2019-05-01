using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    public class TimingAndActuationsChartForPhase
    {
        public Chart Chart { get; set; }
        private TimingAndActuationsForPhase TimingAndActuationsForPhase { get; set; }
        private double _yValue;
        private int _dotSize;
        private double _laneOffset;
        private int _lanesProcessed;
        private DateTime orginalEndDate;

        public TimingAndActuationsChartForPhase(TimingAndActuationsForPhase timingAndActuationsForPhase)
        {
            _laneOffset = 0.0;
            _lanesProcessed = 0;
            _yValue = 0.5;
            _dotSize = 1;
            TimingAndActuationsForPhase = timingAndActuationsForPhase;
            var getPermissivePhase = TimingAndActuationsForPhase.GetPermissivePhase;
            if (TimingAndActuationsForPhase.Options.DotAndBarSize > 0)
            {
                _dotSize = TimingAndActuationsForPhase.Options.DotAndBarSize;
            }
            orginalEndDate = TimingAndActuationsForPhase.Options.EndDate;
            var reportTimespan = TimingAndActuationsForPhase.Options.EndDate -
                                 TimingAndActuationsForPhase.Options.StartDate;
            var totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 121)
            {
                TimingAndActuationsForPhase.Options.EndDate =
                    TimingAndActuationsForPhase.Options.EndDate.AddMinutes(-1);
            }
            Chart = ChartFactory.CreateDefaultChart(TimingAndActuationsForPhase.Options);
            Chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.False;
            SetChartTitle();
            if (TimingAndActuationsForPhase.Options.ShowVehicleSignalDisplay)
            {
                SetCycleStrips();
            }
            if (TimingAndActuationsForPhase.PhaseCustomEvents != null &&
                TimingAndActuationsForPhase.PhaseCustomEvents.Any())
            {
                SetPhaseCustomEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedCount)
            {
                SetAdvanceCountEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedDilemmaZone)
            {
                SetAdvancePresenceEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowLaneByLaneCount)
            {
                SetLaneByLaneCount();
            }
            if (TimingAndActuationsForPhase.Options.ShowStopBarPresence)
            {
                SetStopBarEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianActuation && !getPermissivePhase)
            {
                SetPedestrianActuation();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianIntervals && !getPermissivePhase)
            {
                SetPedestrianInterval();
            }
            SetYAxisLabels();
            TimingAndActuationsForPhase.Options.EndDate = orginalEndDate;
        }

        private void SetPhaseCustomEvents()
        {
            _lanesProcessed = 0;
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
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }

                var phaseCustomEvents = phaseEventElement.Value;
                var lastItem = phaseCustomEvents.Count;
                if (lastItem <= 0) continue;
                for (var i = 0; i < lastItem; i++)
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
                phaseEventsSeries.Name = "All Lanes: Phase Events";
                Chart.Series.Add(phaseEventsSeries);
                _yValue += 1.0;
            }
        }

        private void SetChartTitle()
        {
            Chart.ChartAreas[0].AxisY.Title = "";
            Chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.Title = "";
            Chart.Titles.Add(ChartTitleFactory.GetChartName(TimingAndActuationsForPhase.Options.MetricTypeID));
            Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                TimingAndActuationsForPhase.Options.SignalID, TimingAndActuationsForPhase.Options.StartDate,
                TimingAndActuationsForPhase.Options.EndDate));
            Chart.Titles.Add(
                ChartTitleFactory.GetPhaseAndPhaseDescriptions(TimingAndActuationsForPhase.Approach,
                    TimingAndActuationsForPhase.GetPermissivePhase));
        }

        private void SetPedestrianInterval()
        {
            if (TimingAndActuationsForPhase == null) return;
            if (!TimingAndActuationsForPhase.PedestrianIntervals.Any()) return;
            var pedestrianIntervalsSeries = new Series
            {
                ChartType = SeriesChartType.StepLine,
                XValueType = ChartValueType.DateTime,
                BorderWidth = 20,
                Name = "Pedestrian Intervals"
            };
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                pedestrianIntervalsSeries.BorderWidth = 15;
            }

            foreach (var phase in TimingAndActuationsForPhase.PedestrianIntervals)
            {
                var pointNumber = new int();
                switch (phase.EventCode)
                {
                    case 21:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.Gray;
                        break;
                    case 22:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.DeepSkyBlue;
                        break;
                    case 23:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.LightSkyBlue;
                        break;
                }
            }

            if (pedestrianIntervalsSeries.Points.Count <= 0) return;
            Chart.Series.Add(pedestrianIntervalsSeries);
            _yValue += 1.0;
        }

        private void SetPedestrianActuation()
        {
            if (TimingAndActuationsForPhase.PedestrianEvents == null) return;
            if (!TimingAndActuationsForPhase.PedestrianEvents.Any()) return;
            var pedestrianActuation = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime,
                Name = "Pedestrian Detector Actuations"
            };
            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                pedestrianActuation.ChartType = SeriesChartType.Line;
            }
            if (TimingAndActuationsForPhase.Options.ShowRawEvents)
            {
                for (var i = 0; i < TimingAndActuationsForPhase.PedestrianEvents.Count; i++)
                {
                    var seriesPointIndex = new int();
                    if (TimingAndActuationsForPhase.PedestrianEvents[i].EventCode == 90)
                    {
                        seriesPointIndex = pedestrianActuation.Points.AddXY(
                            TimingAndActuationsForPhase.PedestrianEvents[i].Timestamp.ToOADate(), _yValue);
                        pedestrianActuation.Points[seriesPointIndex].Color = Color.Transparent;
                        pedestrianActuation.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Triangle;
                        pedestrianActuation.Points[seriesPointIndex].MarkerColor = Color.Black;
                        pedestrianActuation.Points[seriesPointIndex].MarkerSize = _dotSize;
                    }
                    else if (TimingAndActuationsForPhase.PedestrianEvents[i].EventCode == 89)
                    {
                        seriesPointIndex = pedestrianActuation.Points.AddXY(
                            TimingAndActuationsForPhase.PedestrianEvents[i].Timestamp.ToOADate(), _yValue);
                        pedestrianActuation.Points[seriesPointIndex].Color = Color.Black;
                        pedestrianActuation.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Square;
                        pedestrianActuation.Points[seriesPointIndex].MarkerColor = Color.LightSlateGray;
                        pedestrianActuation.Points[seriesPointIndex].MarkerSize = _dotSize;
                    }
                }
            }
            else 
            {
                var lastItem = TimingAndActuationsForPhase.PedestrianEvents.Count - 1;
                if (lastItem <= 0) return;
                for (var i = 0; i < lastItem; i++)
                {
                    if (TimingAndActuationsForPhase.PedestrianEvents[i].EventCode == 90 &&
                        TimingAndActuationsForPhase.PedestrianEvents[i + 1].EventCode == 89)
                    {
                        var p0 = pedestrianActuation.Points.AddXY(
                            TimingAndActuationsForPhase.PedestrianEvents[i].Timestamp.ToOADate(), _yValue);
                        pedestrianActuation.Points[p0].Color = Color.Transparent;
                        pedestrianActuation.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                        pedestrianActuation.Points[p0].MarkerColor = Color.Black;
                        pedestrianActuation.Points[p0].MarkerSize = _dotSize;
                        var p1 = pedestrianActuation.Points.AddXY(
                            TimingAndActuationsForPhase.PedestrianEvents[i + 1].Timestamp.ToOADate(), _yValue);
                        pedestrianActuation.Points[p1].Color = Color.Black;
                        pedestrianActuation.Points[p1].MarkerStyle = MarkerStyle.Square;
                        pedestrianActuation.Points[p1].MarkerColor = Color.LightSlateGray;
                        pedestrianActuation.Points[p1].MarkerSize = _dotSize;
                    }
                }
            }
            if (pedestrianActuation.Points.Count > 0)
            {
                Chart.Series.Add(pedestrianActuation);
                _yValue += 1.0;
            }
        }

        private void SetStopBarEvents()
        {
            _lanesProcessed = 0;
            if (TimingAndActuationsForPhase.StopBarEvents == null) return;
            if (!TimingAndActuationsForPhase.StopBarEvents.Any()) return;
            var stopBarSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };
            if(TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                stopBarSeries.ChartType = SeriesChartType.Line;
            }
            foreach (var stopBarEventElement in TimingAndActuationsForPhase.StopBarEvents)
            {
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
                {
                    _laneOffset = (double)++_lanesProcessed * 0.2 - 0.3;
                    _laneOffset = (_laneOffset > _yValue + 0.5) ? _yValue + 0.5 : _laneOffset;
                }
                else
                {
                    stopBarSeries = new Series
                    {
                        ChartType = SeriesChartType.Point,
                        XValueType = ChartValueType.DateTime
                    };
                    if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
                    {
                        stopBarSeries.ChartType = SeriesChartType.Line;
                    }
                }
                var stopBarEvents = stopBarEventElement.Value;
                if (TimingAndActuationsForPhase.Options.ShowRawEvents)
                {
                    for (var i = 0; i < stopBarEvents.Count; i++)
                    {
                        var seriesPointer = new int();
                        if (stopBarEvents[i].EventCode == 82)
                        {
                            seriesPointer = stopBarSeries.Points.AddXY(
                                stopBarEvents[i].Timestamp, _yValue + _laneOffset);
                            stopBarSeries.Points[seriesPointer].Color = Color.Transparent;
                            stopBarSeries.Points[seriesPointer].MarkerStyle = MarkerStyle.Triangle;
                            stopBarSeries.Points[seriesPointer].MarkerColor = Color.Black;
                            stopBarSeries.Points[seriesPointer].MarkerSize = _dotSize;
                        }
                        else if (stopBarEvents[i].EventCode == 81)
                        {
                            seriesPointer = stopBarSeries.Points.AddXY(
                                stopBarEvents[i].Timestamp.ToOADate(), _yValue + _laneOffset);
                            stopBarSeries.Points[seriesPointer].Color = Color.Black;
                            stopBarSeries.Points[seriesPointer].MarkerStyle = MarkerStyle.Square;
                            stopBarSeries.Points[seriesPointer].MarkerColor = Color.LightSlateGray;
                            stopBarSeries.Points[seriesPointer].MarkerSize = _dotSize;
                        }
                    }
                }
                else
                {
                    var lastItem = stopBarEvents.Count - 1;
                    if (lastItem > 0)
                    {
                        for (var i = 0; i < lastItem; i++)
                        {
                            if (stopBarEvents[i].EventCode == 82 && stopBarEvents[i + 1].EventCode == 81)
                            {
                                var p0 = stopBarSeries.Points.AddXY(stopBarEvents[i].Timestamp.ToOADate(),
                                    _yValue + _laneOffset);
                                stopBarSeries.Points[p0].Color = Color.Transparent;
                                stopBarSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                                stopBarSeries.Points[p0].MarkerColor = Color.Black;
                                stopBarSeries.Points[p0].MarkerSize = _dotSize;
                                var p1 = stopBarSeries.Points.AddXY(stopBarEvents[i + 1].Timestamp.ToOADate(),
                                    _yValue + _laneOffset);
                                stopBarSeries.Points[p1].Color = Color.Black;
                                stopBarSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                                stopBarSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                                stopBarSeries.Points[p1].MarkerSize = _dotSize;
                            }
                        }
                    }
                }
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false && stopBarSeries.Points.Count > 0)
                {
                    stopBarSeries.Name = stopBarEventElement.Key;
                    Chart.Series.Add(stopBarSeries);
                    _yValue += 1.0;
                }
            }
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup && stopBarSeries.Points.Count > 0)
            {
                stopBarSeries.Name = "All Lanes: Stop Bar Events";
                Chart.Series.Add(stopBarSeries);
                _yValue += 1.0;
            }
        }

        private void SetLaneByLaneCount()
        {
            if (TimingAndActuationsForPhase.LaneByLanes == null) return;
            _lanesProcessed = 0;
            var laneByLaneSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };

            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                laneByLaneSeries.ChartType = SeriesChartType.Line;
            }

            foreach (var laneByLaneElement in TimingAndActuationsForPhase.LaneByLanes)
            {
                //if (laneByLaneElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
                {
                    _laneOffset = (double)++_lanesProcessed * 0.2 - 0.3;
                    _laneOffset = (_laneOffset > _yValue + 0.5) ? _yValue + 0.5 : _laneOffset;
                }
                else
                {
                    laneByLaneSeries = new Series
                    {
                        ChartType = SeriesChartType.Point,
                        XValueType = ChartValueType.DateTime
                    };
                    if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
                    {
                        laneByLaneSeries.ChartType = SeriesChartType.Line;
                    }
                }
                var laneByLaneEvents = laneByLaneElement.Value;
                if (TimingAndActuationsForPhase.Options.ShowRawEvents)
                {

                }
                else
                {
                    var lastItem = laneByLaneEvents.Count - 1;
                    if (lastItem > 0)
                    {
                        for (var i = 0; i < lastItem; i++)
                        {
                            if (laneByLaneEvents[i].EventCode == 82 && laneByLaneEvents[i + 1].EventCode == 81)
                            {
                                var p0 = laneByLaneSeries.Points.AddXY(laneByLaneEvents[i].Timestamp.ToOADate(),
                                    _yValue + _laneOffset);
                                laneByLaneSeries.Points[p0].Color = Color.Transparent;
                                laneByLaneSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                                laneByLaneSeries.Points[p0].MarkerColor = Color.Black;
                                laneByLaneSeries.Points[p0].MarkerSize = _dotSize;
                                var p1 = laneByLaneSeries.Points.AddXY(laneByLaneEvents[i + 1].Timestamp.ToOADate(),
                                    _yValue + _laneOffset);
                                laneByLaneSeries.Points[p1].Color = Color.Black;
                                laneByLaneSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                                laneByLaneSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                                laneByLaneSeries.Points[p1].MarkerSize = _dotSize;
                            }
                        }
                    }
                }
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false && laneByLaneSeries.Points.Count > 0)
                {
                        laneByLaneSeries.Name = laneByLaneElement.Key;
                        Chart.Series.Add(laneByLaneSeries);
                        _yValue += 1.0;
                }
            }
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup && laneByLaneSeries.Points.Count > 0)
            {
                laneByLaneSeries.Name = "All Lane By Lane: Events";
                Chart.Series.Add(laneByLaneSeries);
                _yValue += 1.0;
            }
        }

        private void SetYAxisLabels()
        {
            var topLabel = new Series
            {
                ChartType = SeriesChartType.Point,
                Color = Color.Transparent,
                Name = "Vehicle Signal Display                    .",
                XValueType = ChartValueType.DateTime
            };
            topLabel.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.AddMinutes(-3).ToOADate(), _yValue);
            Chart.Series.Add(topLabel);
            //_yValue++;
            var yMaximum = Math.Round(_yValue + 0.8, 0);
            Chart.ChartAreas[0].AxisY.Maximum = yMaximum;
            var height = yMaximum > 20 ? 20 * yMaximum + 170 : 25.0 * yMaximum + 180.0;
            if (height < 200) height = 250;
            Chart.Height = (Unit) height;
            Chart.ChartAreas[0].AxisY.Interval = 1;
            var bottomLabelOffset = 0;
            var topLabelOffset = bottomLabelOffset + 1;
            var timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset,
                ".                                         .", 1,
                LabelMarkStyle.None);
            //Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            //Chart.ChartAreas[0].AxisY.TitleForeColor = Color.White;
            //Series[0] is the required black dot for the stripes to appear.
            for (var i = 1; i < Chart.Series.Count; i++)
            {
                bottomLabelOffset = (int) Chart.Series[i].Points[0].YValues[0];
                topLabelOffset = bottomLabelOffset + 1;
                var sideLabel = Chart.Series[i].Name;
                timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[i].Name, 0,
                    LabelMarkStyle.None);
                Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            }

            //bottomLabelOffset = Chart.ChartAreas[0].AxisY.CustomLabels.Count -1;
            //topLabelOffset = bottomLabelOffset + 1;
            //timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, "Vehicle Signal Display                    .", 0,
            //    LabelMarkStyle.None);
            //Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
        }

        private void SetAdvanceCountEvents()
        {
            _lanesProcessed = 0;
            var advancedOffset = 0.0;
            var darkColor = Color.Black;
            var lightColor = Color.Gray;
            var advanceCountSeries =
                new Series
                {
                    ChartType = SeriesChartType.Point,
                    XValueType = ChartValueType.DateTime
                };
            //if(TimingAndActuationsForPhase.Options.ShowLinesStartEnd  == false)
            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                advanceCountSeries.ChartType = SeriesChartType.Line;
            }
            if (TimingAndActuationsForPhase.Options.AdvancedOffset != null &&
                TimingAndActuationsForPhase.Options.AdvancedOffset != 0.0)
            {
                advancedOffset = (float) TimingAndActuationsForPhase.Options.AdvancedOffset;
                darkColor = Color.DarkViolet;
                lightColor = Color.MediumPurple;
            }
            foreach (var advanceCountEventElement in TimingAndActuationsForPhase.AdvanceCountEvents)
            {
                if (advanceCountEventElement.Value.Any())
                {
                    if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
                    {
                        _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.2;
                        _laneOffset = (_laneOffset > _yValue + 0.5) ? _yValue + 0.5 : _laneOffset;
                    }
                    else
                    {
                        advanceCountSeries = new Series
                        {
                            ChartType = SeriesChartType.Point,
                            XValueType = ChartValueType.DateTime
                        };
                        if(TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
                        
                        {
                            advanceCountSeries.ChartType = SeriesChartType.Line;
                        }
                    }
                    var advanceEvents = advanceCountEventElement.Value;
                    if (TimingAndActuationsForPhase.Options.ShowRawEvents)
                    {
                        for (var i = 0; i < advanceEvents.Count; i++)
                        {
                            var seriesPointer = new int();
                            if (advanceEvents[i].EventCode == 82)
                            {
                                seriesPointer = advanceCountSeries.Points.AddXY(advanceEvents[i].Timestamp.AddSeconds(advancedOffset).ToOADate(),
                                    _yValue + _laneOffset);
                                advanceCountSeries.Points[seriesPointer].Color = Color.Transparent;
                                advanceCountSeries.Points[seriesPointer].MarkerStyle = MarkerStyle.Triangle;
                                advanceCountSeries.Points[seriesPointer].MarkerColor = darkColor;
                                advanceCountSeries.Points[seriesPointer].MarkerSize = _dotSize;
                            }
                            else if (advanceEvents[i].EventCode == 81)
                            {
                                seriesPointer = advanceCountSeries.Points.AddXY(advanceEvents[i].Timestamp.AddSeconds(advancedOffset).ToOADate(),
                                    _yValue + _laneOffset);
                                advanceCountSeries.Points[seriesPointer].Color = Color.Black;
                                advanceCountSeries.Points[seriesPointer].MarkerStyle = MarkerStyle.Square;
                                advanceCountSeries.Points[seriesPointer].MarkerColor = lightColor;
                                advanceCountSeries.Points[seriesPointer].MarkerSize = _dotSize;
                            }
                        }
                    }
                    else
                    {
                        var lastItem = advanceEvents.Count - 1;
                        if (lastItem > 0)
                        {
                            for (var i = 0; i < lastItem; i++)
                            {
                                if (advanceEvents[i].EventCode == 82 && advanceEvents[i + 1].EventCode == 81)
                                {
                                    var p0 = advanceCountSeries.Points.AddXY(
                                        advanceEvents[i].Timestamp.AddSeconds(advancedOffset).ToOADate(),
                                        _yValue + _laneOffset);
                                    advanceCountSeries.Points[p0].Color = Color.Transparent;
                                    advanceCountSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                                    advanceCountSeries.Points[p0].MarkerColor = darkColor;
                                    advanceCountSeries.Points[p0].MarkerSize = _dotSize;
                                    var p1 = advanceCountSeries.Points.AddXY(
                                        advanceEvents[i + 1].Timestamp.AddSeconds(advancedOffset).ToOADate(),
                                        _yValue + _laneOffset);
                                    advanceCountSeries.Points[p1].Color = darkColor;
                                    advanceCountSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                                    advanceCountSeries.Points[p1].MarkerColor = lightColor;
                                    advanceCountSeries.Points[p1].MarkerSize = _dotSize;
                                }
                            }
                        }
                    }
                    if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false &&
                        advanceCountSeries.Points.Count > 0)
                    {
                        advanceCountSeries.Name = advanceCountEventElement.Key;
                        Chart.Series.Add(advanceCountSeries);
                        _yValue += 1.0;
                    }
                }
            }
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup && advanceCountSeries.Points.Count > 0)
            {
                advanceCountSeries.Name = "All Lanes: Advanced Events";
                Chart.Series.Add(advanceCountSeries);
                _yValue += 1.0;
            }
        }

    private void SetAdvancePresenceEvents()
        {
            _lanesProcessed = 0;
            var advancePresenceSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };
            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                advancePresenceSeries.ChartType = SeriesChartType.Line;
            }
            foreach (var advancePresenceElement in TimingAndActuationsForPhase.AdvancePresenceEvents)
            {
                if (!advancePresenceElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    advancePresenceSeries = new Series
                    {
                        ChartType = SeriesChartType.Point,
                        XValueType = ChartValueType.DateTime
                    };
                    if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
                    {
                        advancePresenceSeries.ChartType = SeriesChartType.Line;
                    }
                }
                else
                {
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }
                var advancePresenceEvents = advancePresenceElement.Value;
                if (TimingAndActuationsForPhase.Options.ShowRawEvents)
                {
                    for (var i = 0; i < advancePresenceEvents.Count; i++)
                    {
                        var seriesPointIndex = new int();
                        if (advancePresenceEvents[i].EventCode == 82)
                        {
                            seriesPointIndex = advancePresenceSeries.Points.AddXY(advancePresenceEvents[i].Timestamp.ToOADate(),
                                _yValue + _laneOffset);
                            advancePresenceSeries.Points[seriesPointIndex].Color = Color.Transparent;
                            advancePresenceSeries.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Triangle;
                            advancePresenceSeries.Points[seriesPointIndex].MarkerSize = _dotSize;
                            advancePresenceSeries.Points[seriesPointIndex].MarkerColor = Color.Black;
                        }
                        else if (advancePresenceEvents[i].EventCode == 81)
                            seriesPointIndex = advancePresenceSeries.Points.AddXY(advancePresenceEvents[i].Timestamp.ToOADate(),
                                _yValue + _laneOffset);
                        advancePresenceSeries.Points[seriesPointIndex].Color = Color.Black;
                        advancePresenceSeries.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Square;
                        advancePresenceSeries.Points[seriesPointIndex].MarkerSize = _dotSize;
                        advancePresenceSeries.Points[seriesPointIndex].MarkerColor = Color.LightSlateGray;
                    }
                }
                else
                {
                    var lastItem = advancePresenceEvents.Count - 1;
                    for (var i = 0; i < lastItem; i++)
                    {
                        if (advancePresenceEvents[i].EventCode == 82 && advancePresenceEvents[i + 1].EventCode == 81)
                        {
                            var p0 = advancePresenceSeries.Points.AddXY(advancePresenceEvents[i].Timestamp.ToOADate(),
                                _yValue + _laneOffset);
                            advancePresenceSeries.Points[p0].Color = Color.Transparent;
                            advancePresenceSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                            advancePresenceSeries.Points[p0].MarkerColor = Color.Black;
                            advancePresenceSeries.Points[p0].MarkerSize = _dotSize;
                            var p1 = advancePresenceSeries.Points.AddXY(
                                advancePresenceEvents[i + 1].Timestamp.ToOADate(),
                                _yValue + _laneOffset);
                            advancePresenceSeries.Points[p1].Color = Color.Black;
                            advancePresenceSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                            advancePresenceSeries.Points[p1].MarkerSize = _dotSize;
                            advancePresenceSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                        }
                    }
                }
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false && advancePresenceSeries.Points.Count > 0)
                {
                    advancePresenceSeries.Name = advancePresenceElement.Key;
                        Chart.Series.Add(advancePresenceSeries);
                        _yValue += 1.0;
                }
            }
            if (advancePresenceSeries.Points.Count > 0 && TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
            {
                advancePresenceSeries.Name = "All Lanes: Advanced Presence Events";
                Chart.Series.Add(advancePresenceSeries);
                _yValue += 1.0;
            }
        }

        private void SetCycleStrips()
        {
            var localColorDarkGreen = Color.MediumSeaGreen; //MediumSeaGreen
            var localColorGreen = Color.LightGreen; //LightGreen
            var localColorYellow = Color.Yellow;
            var localColorDarkRed = Color.Firebrick; //Firebrick
            var localColorRed = Color.LightCoral; //light coral
            var localColorGray = Color.DarkGray;
            var localColorBlack = Color.Black;
            
            var blackDot = new Series
            {
                ChartType = SeriesChartType.Point,
                Color = localColorBlack,
                Name = "Change to Black.  Do not Label!",
                XValueType = ChartValueType.DateTime
            };
            blackDot.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.Hour, 0.0);
            Chart.Series.Add(blackDot);

            foreach (var vehicleDispalyCycle in TimingAndActuationsForPhase.CycleAllEvents)
            {
                var firstStripLine = new StripLine()
                {
                    BackColor = localColorGray,
                    IntervalOffset = 0.0,
                    StripWidth = 100.0,
                    IntervalOffsetType = Chart.ChartAreas[0].AxisX.IntervalType,
                    StripWidthType = Chart.ChartAreas[0].AxisX.IntervalType,
                    IntervalType = Chart.ChartAreas[0].AxisX.IntervalType,
                    Interval = 1
            };
                Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(firstStripLine);
                var vehicleDisplayCycleValue = vehicleDispalyCycle.Value;
                if (vehicleDisplayCycleValue.Count > 1)
                {
                    for (int i = 0; i < vehicleDisplayCycleValue.Count; i++)
                    {
                        var vehicleStripeLine = new StripLine();
                        TimeSpan timeSpanStartOffset = vehicleDisplayCycleValue[i].Timestamp - 
                                                       TimingAndActuationsForPhase.Options.StartDate;
                        TimeSpan timeSpanWidth = TimingAndActuationsForPhase.Options.EndDate -
                                                        vehicleDisplayCycleValue[i].Timestamp;
                        vehicleStripeLine.IntervalOffsetType = Chart.ChartAreas[0].AxisX.IntervalType;
                        vehicleStripeLine.StripWidthType = Chart.ChartAreas[0].AxisX.IntervalType;
                        vehicleStripeLine.IntervalType = Chart.ChartAreas[0].AxisX.IntervalType;
                        vehicleStripeLine.Interval = 1;
                        switch (vehicleDisplayCycleValue[i].EventCode)
                        {
                            case 1:
                                vehicleStripeLine.BackColor = localColorDarkGreen;
                                break;
                            case 3:
                                vehicleStripeLine.BackColor = localColorGreen;
                                break;
                            case 8:
                                vehicleStripeLine.BackColor = localColorYellow;
                                break;
                            case 9:
                                vehicleStripeLine.BackColor = localColorDarkRed;
                                break;
                            case 11:
                                vehicleStripeLine.BackColor = localColorRed;
                                break;
                            case 61:
                                vehicleStripeLine.BackColor = localColorDarkGreen;
                                break;
                            case 62:
                                vehicleStripeLine.BackColor = localColorGreen;
                                break;
                            case 63:
                                vehicleStripeLine.BackColor = localColorYellow;
                                break;
                            case 64:
                                vehicleStripeLine.BackColor = localColorDarkRed;
                                break;
                            case 65:
                                vehicleStripeLine.BackColor = localColorRed;
                                break;
                            default:
                                vehicleStripeLine.BackColor = localColorGray;
                                break;
                        }
                        var stripOffest = new double();
                        var stripWidth = new double();
                        switch (Chart.ChartAreas[0].AxisX.IntervalType)
                        {
                            case DateTimeIntervalType.Seconds:
                            {
                                stripOffest = timeSpanStartOffset.TotalSeconds;
                                stripWidth = timeSpanWidth.TotalSeconds;
                                break;
                            }
                            case DateTimeIntervalType.Minutes:
                            {
                                stripOffest = timeSpanStartOffset.TotalMinutes;
                                stripWidth = timeSpanWidth.TotalMinutes;
                                break;
                            }
                            case DateTimeIntervalType.Hours:
                            {
                                stripOffest = timeSpanStartOffset.TotalHours;
                                stripWidth = timeSpanWidth.TotalHours;
                                break;
                            }
                            case DateTimeIntervalType.Days:
                            {
                                stripOffest = timeSpanStartOffset.TotalDays;
                                stripWidth = timeSpanWidth.TotalDays;
                                break;
                            }
                        }
                        vehicleStripeLine.IntervalOffset = stripOffest;
                        vehicleStripeLine.StripWidth = (stripWidth > 0.0) ? stripWidth : 0.0;
                        Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(vehicleStripeLine);
                    }
                }
            }
        }
    }
}

using System;
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
            SetChartTitle();
            SetCycleStrips();
            if (TimingAndActuationsForPhase.PhaseCustomEvents != null &&
                TimingAndActuationsForPhase.PhaseCustomEvents.Any())
            {
                SetPhaseCustomEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianActuation == true)
            {
                SetPedestrianActuation();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedCount == true)
            {
                SetAdvanceCountEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedDilemmaZone == true)
            {
                SetAdvancePresenceEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowLaneByLaneCount == true)
            {
                SetLaneByLaneCount();
            }
            if (TimingAndActuationsForPhase.Options.ShowStopBarPresence == true)
            {
                SetStopBarEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianIntervals == true)
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
                phaseEventsSeries.Name = "Combined Lanes for Custom Phase Events";
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

        private void WriteStripLines()
        {
            //    var fileName = TimingAndActuationsForPhase.Approach.Description;
            //    using (StreamWriter cycleFile = new StreamWriter(@"c:\temp\save\StripLines" + fileName + ".csv"))
            //    {
            //        cycleFile.WriteLine("Color" + ", " + "Offset" + ", " + "Width");
            //        foreach (var strip in Chart.ChartAreas[0].Axes[0].StripLines)
            //        {
            //            cycleFile.WriteLine(strip.BackColor.ToString(), strip.IntervalOffset.ToString("00.00000000"), strip.StripWidth.ToString("00.00000000"));
            //        }
            //    }
        }

        private void SetPedestrianInterval()
        {
            //if (TimingAndActuationsForPhase == null) return;
            //if (!TimingAndActuationsForPhase.PedestrianIntervals.Any()) return;
            var pedestrianIntervalsSeries = new Series
            {
                ChartType = SeriesChartType.StepLine,
                XValueType = ChartValueType.DateTime,
                BorderWidth = 22,
                Name = "Pedestrian Intervals"
            };
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
                pedestrianIntervalsSeries.BorderWidth = 17;

            var lastItem = TimingAndActuationsForPhase.PedestrianIntervals.Count;
            if (lastItem <= 0) return;
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
            //if (TimingAndActuationsForPhase.PedestrianEvents == null) return;
            //if (!TimingAndActuationsForPhase.PedestrianEvents.Any()) return;
            //var areTherePedAct = false;
            //var isThereStuckButton = false;
            //var isThereChattingButton = false;
            var pedestrianActuation = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
                Name = "Pedestrian Detector Actuations"
            };
            var lastItem = TimingAndActuationsForPhase.PedestrianEvents.Count - 1;
            if (lastItem < 0) return;
            for (var inde2 = 0; inde2 < lastItem; inde2++)
            {
                if (TimingAndActuationsForPhase.PedestrianEvents[inde2].EventCode != 90 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde2 + 1].EventCode != 89) continue;
                var p0 = pedestrianActuation.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde2].Timestamp.ToOADate(), _yValue);
                pedestrianActuation.Points[p0].Color = Color.Transparent;
                pedestrianActuation.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                pedestrianActuation.Points[p0].MarkerColor = Color.Black;
                pedestrianActuation.Points[p0].MarkerSize = _dotSize;
                var p1 = pedestrianActuation.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde2 + 1].Timestamp.ToOADate(), _yValue);
                pedestrianActuation.Points[p1].Color = Color.Black;
                pedestrianActuation.Points[p1].MarkerStyle = MarkerStyle.Square;
                pedestrianActuation.Points[p1].MarkerColor = Color.LightSlateGray;
                pedestrianActuation.Points[p1].MarkerSize = _dotSize;
            }

            if (pedestrianActuation.Points.Count <= 0) return;
            Chart.Series.Add(pedestrianActuation);
            _yValue += 1.0;

            //isThereStuckButton = SetStuckOnPedButton();
            //isThereChattingButton = SetChattingPedButton();
            //if (areTherePedAct || isThereStuckButton || isThereChattingButton)
            //_yValue += 1.0;
        }

        private bool SetChattingPedButton()
        {
            if (!TimingAndActuationsForPhase.PedestrianEvents.Any()) return false;
            var pedestrianChatter = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };
            var chattingOffset = 0.25;
            var lastItem = TimingAndActuationsForPhase.PedestrianEvents.Count - 6;
            if (lastItem < 0) return false;
            for (var inde7 = 0; inde7 < lastItem; inde7++)
            {
                if (TimingAndActuationsForPhase.PedestrianEvents[inde7].EventCode != 90 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 1].EventCode != 89 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 2].EventCode != 90 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 3].EventCode != 89 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 4].EventCode != 90 ||
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 5].EventCode != 89) continue;
                var quickPushes = TimingAndActuationsForPhase.PedestrianEvents[inde7 + 4].Timestamp
                    .Subtract(TimingAndActuationsForPhase.PedestrianEvents[inde7].Timestamp);
                if (quickPushes.Seconds >= 1) continue;
                var p0 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7].Timestamp.ToOADate(),
                    _yValue + +chattingOffset - 0.1);
                pedestrianChatter.Points[p0].Color = Color.Transparent;
                var p1 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 1].Timestamp.ToOADate(), _yValue);
                pedestrianChatter.Points[p1].Color = Color.DeepPink;
                pedestrianChatter.Points[p1].MarkerStyle = MarkerStyle.Diamond;
                pedestrianChatter.Points[p1].MarkerSize = _dotSize;
                p0 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 2].Timestamp.ToOADate(),
                    _yValue + chattingOffset - 0.1);
                pedestrianChatter.Points[p0].Color = Color.Transparent;
                p1 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 3].Timestamp.ToOADate(), _yValue);
                pedestrianChatter.Points[p1].Color = Color.DeepPink;
                pedestrianChatter.Points[p1].MarkerStyle = MarkerStyle.Triangle;
                pedestrianChatter.Points[p1].MarkerSize = _dotSize;
                p0 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 4].Timestamp.ToOADate(),
                    _yValue + chattingOffset - 0.1);
                pedestrianChatter.Points[p0].Color = Color.Transparent;
                p1 = pedestrianChatter.Points.AddXY(
                    TimingAndActuationsForPhase.PedestrianEvents[inde7 + 5].Timestamp.ToOADate(), _yValue);
                pedestrianChatter.Points[p1].Color = Color.DeepPink;
                pedestrianChatter.Points[p1].MarkerStyle = MarkerStyle.Triangle;
                pedestrianChatter.Points[p1].MarkerSize = _dotSize;
            }

            if (pedestrianChatter.Points.Count <= 0) return false;
            pedestrianChatter.Name = "chatting";
            Chart.Series.Add(pedestrianChatter);
            return true;

        }

        bool SetStuckOnPedButton()
        {
            if (!TimingAndActuationsForPhase.PedestrianEvents.Any()) return false;
            var pedestrianButton = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.DateTime
            };
            var stuckOffset = 0.1;
            var lastItem = TimingAndActuationsForPhase.PedestrianEvents.Count - 1;
            if (lastItem < 0) return false;
            for (var inde8 = 0; inde8 < lastItem; inde8++)
            {
                if (TimingAndActuationsForPhase.PedestrianEvents[inde8].EventCode != 90) continue;
                var buttonTime = TimingAndActuationsForPhase.PedestrianEvents[inde8 + 1].Timestamp
                    .Subtract(TimingAndActuationsForPhase.PedestrianEvents[inde8].Timestamp);
                if (buttonTime.Seconds >= 3)
                {
                    var p0 = pedestrianButton.Points.AddXY(
                        TimingAndActuationsForPhase.PedestrianEvents[inde8].Timestamp.ToOADate(),
                        _yValue + stuckOffset + 0.1);
                    pedestrianButton.Points[p0].Color = Color.Transparent;
                    var p1 = pedestrianButton.Points.AddXY(
                        TimingAndActuationsForPhase.PedestrianEvents[inde8 + 1].Timestamp.ToOADate(), _yValue);
                    pedestrianButton.Points[p1].Color = Color.Lime;
                    pedestrianButton.Points[p1].MarkerStyle = MarkerStyle.Star5;
                    pedestrianButton.Points[p1].MarkerSize = _dotSize;
                }
            }

            if (pedestrianButton.Points.Count <= 0) return false;
            pedestrianButton.Name = "stuck";
            Chart.Series.Add(pedestrianButton);
            return true;

        }

        private void SetStopBarEvents()
        {
            _lanesProcessed = 0;
            var stopBarSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime
            };
            if (TimingAndActuationsForPhase.StopBarEvents == null) return;
            if (!TimingAndActuationsForPhase.StopBarEvents.Any()) return;
            foreach (var stopBarEventElement in TimingAndActuationsForPhase.StopBarEvents)
            {
                //if (!stopBarEventElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    stopBarSeries = new Series
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.DateTime
                    };
                }
                else
                {
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }

                var stopBarEvents = stopBarEventElement.Value;
                var lastItem = stopBarEvents.Count - 1;
                if (lastItem <= 0) continue;
                for (var i = 0; i < lastItem; i++)
                {
                    if (stopBarEvents[i].EventCode != 82 || stopBarEvents[i + 1].EventCode != 81) continue;
                    var p0 = stopBarSeries.Points.AddXY(stopBarEvents[i].Timestamp.ToOADate(), _yValue + _laneOffset);
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

                if (stopBarSeries.Points.Count <= 0) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    stopBarSeries.Name = stopBarEventElement.Key;
                    Chart.Series.Add(stopBarSeries);
                    _yValue += 1.0;
                }
            }

            if (stopBarSeries.Points.Count <= 0) return;
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                stopBarSeries.Name = "Combined Lanes for Stop Bar Events";
                Chart.Series.Add(stopBarSeries);
                _yValue += 1.0;
            }
        }

        private void SetLaneByLaneCount()
        {
            //if (TimingAndActuationsForPhase.LaneByLanes == null) return;
            _lanesProcessed = 0;
            var laneByLaneSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime
            };

            foreach (var laneByLaneElement in TimingAndActuationsForPhase.LaneByLanes)
            {
                //if (laneByLaneElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    laneByLaneSeries = new Series
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.DateTime
                    };
                }
                else
                {
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }

                var laneByLaneEvents = laneByLaneElement.Value;
                var lastItem = laneByLaneEvents.Count - 1;
                if (lastItem < 0) continue;
                for (var i = 0; i < lastItem; i++)
                {
                    if (laneByLaneEvents[i].EventCode != 82 || laneByLaneEvents[i + 1].EventCode != 81) continue;
                    var p0 = laneByLaneSeries.Points.AddXY(laneByLaneEvents[i].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    laneByLaneSeries.Points[p0].Color = Color.Transparent;
                    laneByLaneSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                    laneByLaneSeries.Points[p0].MarkerColor = Color.Black;
                    laneByLaneSeries.Points[p0].BorderWidth = _dotSize;
                    var p1 = laneByLaneSeries.Points.AddXY(laneByLaneEvents[i + 1].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    laneByLaneSeries.Points[p1].Color = Color.Black;
                    laneByLaneSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                    laneByLaneSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                    laneByLaneSeries.Points[p1].MarkerSize = _dotSize;
                }

                if (laneByLaneSeries.Points.Count <= 0) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    laneByLaneSeries.Name = laneByLaneElement.Key;
                    Chart.Series.Add(laneByLaneSeries);
                    _yValue += 1.0;
                }
            }

            if (laneByLaneSeries.Points.Count <= 0) return;
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                laneByLaneSeries.Name = "Combined Lane By Lane Events";
                Chart.Series.Add(laneByLaneSeries);
                _yValue += 1.0;

            }
        }

        void SetYAxisLabels()
        {
            var yMaximum = Math.Round(_yValue + 0.8, 0);
            Chart.ChartAreas[0].AxisY.Maximum = yMaximum;
            Chart.Height = (Unit) (25.0 * yMaximum + 180.0);
            Chart.ChartAreas[0].AxisY.Interval = 1;
            var bottomLabelOffset = 0;
            var topLabelOffset = bottomLabelOffset + 1;
            var timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, "                                         ", 1,
                LabelMarkStyle.None);
            //Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            Chart.ChartAreas[0].AxisY.TitleForeColor = Color.White;
            //Series[0] is the required black dot for the stripes to appear.
            for (var i = 1; i < Chart.Series.Count; i++)
            {
                if (Chart.Series[i].Name == "stuck" || Chart.Series[i].Name == "chatting") continue;
                bottomLabelOffset = (int) Chart.Series[i].Points[0].YValues[0];
                topLabelOffset = bottomLabelOffset + 1;
                timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[i].Name, 0,
                    LabelMarkStyle.None);
                Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            }
            bottomLabelOffset++;
            topLabelOffset = bottomLabelOffset + 1;
            timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, "Timing And Actutation Signal Vehicle Information", 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
        }

        private void SetAdvanceCountEvents()
        {
            _lanesProcessed = 0;
            var advanceCountSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime
            };
            foreach (var advanceCountEventElement in TimingAndActuationsForPhase.AdvanceCountEvents)
            {
                //if (!advanceCountEventElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    advanceCountSeries = new Series
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.DateTime
                    };
                }
                else
                {
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }

                var advanceEvents = advanceCountEventElement.Value;
                var lastItem = advanceEvents.Count - 1;
                if (lastItem <= 0) continue;
                for (var i = 0; i < lastItem; i++)
                {
                    if (advanceEvents[i].EventCode != 82 || advanceEvents[i + 1].EventCode != 81) continue;
                    var p0 = advanceCountSeries.Points.AddXY(advanceEvents[i].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    advanceCountSeries.Points[p0].Color = Color.Transparent;
                    advanceCountSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                    advanceCountSeries.Points[p0].MarkerColor = Color.Black;
                    advanceCountSeries.Points[p0].MarkerSize = _dotSize;
                    var p1 = advanceCountSeries.Points.AddXY(advanceEvents[i + 1].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    advanceCountSeries.Points[p1].Color = Color.Black;
                    advanceCountSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                    advanceCountSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                    advanceCountSeries.Points[p1].MarkerSize = _dotSize;
                }

                if (advanceCountSeries.Points.Count <= 0) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    advanceCountSeries.Name = advanceCountEventElement.Key;
                    Chart.Series.Add(advanceCountSeries);
                    _yValue += 1.0;
                }
            }

            if (advanceCountSeries.Points.Count <= 0) return;
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                advanceCountSeries.Name = "Combined Lanes For Advanced Count Events";
                Chart.Series.Add(advanceCountSeries);
                _yValue += 1.0;
            }
        }

        private void SetAdvancePresenceEvents()
        {
            _lanesProcessed = 0;
            var advancePresenceSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime
            };
            foreach (var advancePresenceElement in TimingAndActuationsForPhase.AdvancePresenceEvents)
            {
//                if (!advancePresenceElement.Value.Any()) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    advancePresenceSeries = new Series
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.DateTime
                    };
                }
                else
                {
                    _laneOffset = (double) ++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }

                var advanceEvents = advancePresenceElement.Value;
                var lastItem = advanceEvents.Count - 1;
                if (lastItem <= 0) continue;
                for (var i = 0; i < lastItem; i++)
                {
                    if (advanceEvents[i].EventCode != 82 || advanceEvents[i + 1].EventCode != 81) continue;
                    var p0 = advancePresenceSeries.Points.AddXY(advanceEvents[i].Timestamp.ToOADate(),
                        _yValue + _laneOffset);
                    advancePresenceSeries.Points[p0].Color = Color.Transparent;
                    advancePresenceSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                    advancePresenceSeries.Points[p0].MarkerColor = Color.Black;
                    advancePresenceSeries.Points[p0].MarkerSize = _dotSize;
                    var p1 = advancePresenceSeries.Points.AddXY(advanceEvents[i + 1].Timestamp.ToOADate(), _yValue);
                    advancePresenceSeries.Points[p1].Color = Color.Black;
                    advancePresenceSeries.Points[p1].MarkerStyle = MarkerStyle.Square;
                    advancePresenceSeries.Points[p1].MarkerSize = _dotSize;
                    advancePresenceSeries.Points[p1].MarkerColor = Color.LightSlateGray;
                }

                if (advancePresenceSeries.Points.Count <= 0) continue;
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == false)
                {
                    advancePresenceSeries.Name = advancePresenceElement.Key;
                    Chart.Series.Add(advancePresenceSeries);
                    _yValue += 1.0;
                }
            }

            if (advancePresenceSeries.Points.Count <= 0) return;
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                advancePresenceSeries.Name = "Combined Lanes For Advanced Presence Events";
                Chart.Series.Add(advancePresenceSeries);
                _yValue += 1.0;
            }
        }

        private void SetCycleStrips()
        {
            if (TimingAndActuationsForPhase.Cycles.Any() &&
                TimingAndActuationsForPhase.Cycles[0].EndMinGreen == new DateTime(1900, 1, 1))
            {
                SetFourLineStrips();
            }
            else
            {
                SetFiveLineStripe();
            }
        }

        private void SetFourLineStrips()
        {
            if (!TimingAndActuationsForPhase.Cycles.Any()) return;
            var localColorDarkGreen = Color.MediumSeaGreen;
            var localColorGreen = Color.LightGreen;
            var localColorYellow = Color.Yellow;
            var localColorDarkRed = Color.Firebrick;
            var localColorRed = Color.LightCoral;
            var localColorGray = Color.DarkGray;
            var localColorBlack = Color.Black;
            var tempIntervalOffset = 0.0;
            double tempStripWidth = 0.0;
            /*******************************************************************************************************
            *  For some unknown reason, the stripes only show up when there is data in the chart!  So add a point  *
            *  at the orgin.  This makes the stipes be displayed even if there is no other data for the chart!     *
            *  This is "label 0, so start at index 1 when labeling the axes.                                       *
            *******************************************************************************************************/
            var blackDot = new Series
            {
                ChartType = SeriesChartType.Point,
                Color = localColorBlack,
                Name = "Change to Black.  Do not Label!",
                XValueType = ChartValueType.DateTime
            };
            blackDot.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.Hour, 0.0);
            Chart.Series.Add(blackDot);
            switch (Chart.ChartAreas[0].AxisX.IntervalType)
            {
                case DateTimeIntervalType.Seconds:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 5; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.StartYellow - cycle.StartGreen).TotalSeconds;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.StartRed - cycle.StartYellow).TotalSeconds;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.OverLapDark - cycle.EndRed).TotalSeconds;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorGray;
                                    tempIntervalOffset =
                                        (cycle.OverLapDark - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.OverLapDark)
                                        .TotalSeconds;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Seconds;
                            minStripLine.StripWidthType = DateTimeIntervalType.Seconds;
                            minStripLine.IntervalType = DateTimeIntervalType.Seconds;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
                case DateTimeIntervalType.Minutes:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 5; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.StartYellow - cycle.StartGreen).TotalMinutes;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.StartRed - cycle.StartYellow).TotalMinutes;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.OverLapDark - cycle.EndRed).TotalMinutes;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorGray;
                                    tempIntervalOffset =
                                        (cycle.OverLapDark - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.OverLapDark)
                                        .TotalMinutes;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Minutes;
                            minStripLine.StripWidthType = DateTimeIntervalType.Minutes;
                            minStripLine.IntervalType = DateTimeIntervalType.Minutes;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
                case DateTimeIntervalType.Hours:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 5; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.StartYellow - cycle.StartGreen).TotalHours;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.StartRed - cycle.StartYellow).TotalHours;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    tempStripWidth = (cycle.OverLapDark - cycle.EndRed).TotalHours;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorGray;
                                    tempIntervalOffset =
                                        (cycle.OverLapDark - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.OverLapDark)
                                        .TotalHours;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Hours;
                            minStripLine.StripWidthType = DateTimeIntervalType.Hours;
                            minStripLine.IntervalType = DateTimeIntervalType.Hours;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
                case DateTimeIntervalType.Days:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 5; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.StartYellow - cycle.StartGreen).TotalDays;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.StartRed - cycle.StartYellow).TotalDays;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    tempStripWidth = (cycle.OverLapDark - cycle.EndRed).TotalDays;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorGray;
                                    tempIntervalOffset =
                                        (cycle.OverLapDark - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.OverLapDark)
                                        .TotalDays; 
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Days;
                            minStripLine.StripWidthType = DateTimeIntervalType.Days;
                            minStripLine.IntervalType = DateTimeIntervalType.Days;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
            }
        }

        private void SetFiveLineStripe()
        {
            if (!TimingAndActuationsForPhase.Cycles.Any()) return;
            var localColorDarkGreen = Color.MediumSeaGreen;     //MediumSeaGreen
            var localColorGreen = Color.LightGreen;   //LightGreen
            var localColorYellow = Color.Yellow;
            var localColorDarkRed = Color.Firebrick; //Firebrick
            var localColorRed = Color.LightCoral;       //light coral
            var localColorGray = Color.DarkGray;
            var localColorBlack = Color.Black;
            /*******************************************************************************************************
            *  For some unknown reason, the stripes only show up when there is data in the chart!  So add a point  *
            *  at the orgin.  This makes the stipes be displayed even if there is no other data for the chart!     *
            *  This is "label 0, so start at index 1 when labeling the axes.                                       *
            *******************************************************************************************************/
            var blackDot = new Series
            {
                ChartType = SeriesChartType.Point,
                Color = localColorBlack,
                Name = "Change to Black.  Do not Label!",
                XValueType = ChartValueType.DateTime
            };
            blackDot.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.Hour, 0.0);
            Chart.Series.Add(blackDot);
            var phase = TimingAndActuationsForPhase.PhaseNumber;
            var count = TimingAndActuationsForPhase.Cycles.Count;
            var tempIntervalOffset = 0.0;
            var tempStripWidth = 0.0;
            double lastOffset = 0.0;
            double lastWidth = 0.0;
            var lastStripLine = new StripLine();
            switch (Chart.ChartAreas[0].AxisX.IntervalType)
            {
                case DateTimeIntervalType.Seconds:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 6; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorDarkGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalSeconds;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalSeconds;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalSeconds;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorDarkRed;
                                    tempIntervalOffset = (cycle.StartRedClearance -
                                                          TimingAndActuationsForPhase.Options.StartDate).TotalSeconds;
                                    tempStripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalSeconds;
                                    break;
                                case 5:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalSeconds;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.StartRed)
                                        .TotalSeconds;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Seconds;
                            minStripLine.StripWidthType = DateTimeIntervalType.Seconds;
                            minStripLine.IntervalType = DateTimeIntervalType.Seconds;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            lastOffset = tempIntervalOffset + tempStripWidth;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                            var stripCount = Chart.ChartAreas[0].AxisX.StripLines.Count;
                        }
                    }
                    break;
                case DateTimeIntervalType.Minutes:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 6; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorDarkGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalMinutes;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalMinutes;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalMinutes;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorDarkRed;
                                    tempIntervalOffset =
                                        (cycle.StartRedClearance -
                                         TimingAndActuationsForPhase.Options.StartDate).TotalMinutes;
                                    tempStripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalMinutes;
                                    break;
                                case 5:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalMinutes;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.StartRed)
                                        .TotalMinutes;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Minutes;
                            minStripLine.StripWidthType = DateTimeIntervalType.Minutes;
                            minStripLine.IntervalType = DateTimeIntervalType.Minutes;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = tempIntervalOffset;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
                case DateTimeIntervalType.Hours:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 6; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorDarkGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalHours;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalHours;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalHours;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorDarkRed;
                                    tempIntervalOffset =
                                        (cycle.StartRedClearance - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalHours;
                                    tempStripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalHours;
                                    break;
                                case 5:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalHours;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.StartRed)
                                        .TotalHours;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Hours;
                            minStripLine.StripWidthType = DateTimeIntervalType.Hours;
                            minStripLine.IntervalType = DateTimeIntervalType.Hours;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = (tempIntervalOffset > 0.0) ? tempIntervalOffset : 0.0;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
                case DateTimeIntervalType.Days:
                    foreach (var cycle in TimingAndActuationsForPhase.Cycles)
                    {
                        for (var cycleColor = 1; cycleColor < 6; cycleColor++)
                        {
                            var minStripLine = new StripLine();
                            switch (cycleColor)
                            {
                                case 1:
                                    minStripLine.BackColor = localColorDarkGreen;
                                    tempIntervalOffset =
                                        (cycle.StartGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.EndMinGreen - cycle.StartGreen).TotalDays;
                                    break;
                                case 2:
                                    minStripLine.BackColor = localColorGreen;
                                    tempIntervalOffset =
                                        (cycle.EndMinGreen - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.StartYellow - cycle.EndMinGreen).TotalDays;
                                    break;
                                case 3:
                                    minStripLine.BackColor = localColorYellow;
                                    tempIntervalOffset =
                                        (cycle.StartYellow - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.StartRedClearance - cycle.StartYellow).TotalDays;
                                    break;
                                case 4:
                                    minStripLine.BackColor = localColorDarkRed;
                                    tempIntervalOffset =
                                        (cycle.StartRedClearance - TimingAndActuationsForPhase.Options.StartDate)
                                        .TotalDays;
                                    tempStripWidth = (cycle.StartRed - cycle.StartRedClearance).TotalDays;
                                    break;
                                case 5:
                                    minStripLine.BackColor = localColorRed;
                                    tempIntervalOffset =
                                        (cycle.StartRed - TimingAndActuationsForPhase.Options.StartDate).TotalDays;
                                    tempStripWidth = (TimingAndActuationsForPhase.Options.EndDate - cycle.StartRed)
                                        .TotalDays;
                                    break;
                            }

                            minStripLine.IntervalOffsetType = DateTimeIntervalType.Days;
                            minStripLine.StripWidthType = DateTimeIntervalType.Days;
                            minStripLine.IntervalType = DateTimeIntervalType.Days;
                            minStripLine.Interval = 1;
                            minStripLine.IntervalOffset = (tempIntervalOffset > 0.0) ? tempIntervalOffset : 0.0;
                            minStripLine.StripWidth = (tempStripWidth > 0.0) ? tempStripWidth : 0.0;
                            Chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(minStripLine);
                        }
                    }

                    break;
            }
        }
    }
}

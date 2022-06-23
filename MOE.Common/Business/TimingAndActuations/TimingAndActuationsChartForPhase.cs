using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

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
        private int _headerDisaplay;

        public TimingAndActuationsChartForPhase(TimingAndActuationsForPhase timingAndActuationsForPhase)
        {
            _laneOffset = 0.0;
            _lanesProcessed = 0;
            _yValue = 0.5;
            TimingAndActuationsForPhase = timingAndActuationsForPhase;
            var getPermissivePhase = TimingAndActuationsForPhase.GetPermissivePhase;
            _dotSize = 1;
            if (TimingAndActuationsForPhase.Options.DotAndBarSize > 0)
            {
                _dotSize = TimingAndActuationsForPhase.Options.DotAndBarSize;
            }
            var orginalEndDate = TimingAndActuationsForPhase.Options.EndDate;
            var reportTimespan = TimingAndActuationsForPhase.Options.EndDate -
                                 TimingAndActuationsForPhase.Options.StartDate;
            var totalMinutesRounded = Math.Round(reportTimespan.TotalMinutes);
            if (totalMinutesRounded <= 121)
            {
                TimingAndActuationsForPhase.Options.EndDate =
                    TimingAndActuationsForPhase.Options.EndDate.AddMinutes(-1);
            }
            var myCount = -1;
            if (TimingAndActuationsForPhase.PedestrianIntervals != null)
            {
                myCount = TimingAndActuationsForPhase.PedestrianIntervals.Count;
            }
            if (TimingAndActuationsForPhase.Options.ShowRawEventData
                && TimingAndActuationsForPhase.CycleAllEvents.Values.Count == 0
                && myCount <= 0)
            {
                TimingAndActuationsForPhase.Options.EndDate = orginalEndDate;
                return;
            }
            Chart = ChartFactory.CreateDefaultChart(TimingAndActuationsForPhase.Options);
            Chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.False;
            SetChartTitle();
            if (TimingAndActuationsForPhase.Options.ShowVehicleSignalDisplay
                || TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                SetCycleStrips();
            }
            if (TimingAndActuationsForPhase.PhaseCustomEvents != null
                && TimingAndActuationsForPhase.PhaseCustomEvents.Any())
            {
                SetPhaseCustomEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedCount
                && !TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                SetAdvanceCountEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowAdvancedDilemmaZone
                && !TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                SetAdvancePresenceEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowLaneByLaneCount
                && !TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                SetLaneByLaneCount();
            }
            if (TimingAndActuationsForPhase.Options.ShowStopBarPresence
                && !TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                SetStopBarEvents();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianActuation
                && (!getPermissivePhase
                || TimingAndActuationsForPhase.Options.ShowRawEventData))
            {
                SetPedestrianActuation();
            }
            if (TimingAndActuationsForPhase.Options.ShowPedestrianIntervals
                && (!getPermissivePhase || TimingAndActuationsForPhase.Options.ShowRawEventData))
            {
                SetPedestrianInterval();
            }
            SetYAxisLabels();
            TimingAndActuationsForPhase.Options.EndDate = orginalEndDate;
        }

        public TimingAndActuationsChartForPhase(bool legend)
        {
            Chart = ChartFactory.CreateTAALegendChart();
            Chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY.Title = "";
            Chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.Title = "";
            var title = new Title("Legend For Timing And Actuation Charts");
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            Chart.Titles.Add(title);
            Chart.ChartAreas[0].AxisY.Minimum = 0;
            Chart.ChartAreas[0].AxisY.Maximum = 16;
            Chart.ChartAreas[0].AxisY.Interval = 1;
            Chart.ChartAreas[0].AxisX.Minimum = 0;
            Chart.ChartAreas[0].AxisX.Maximum = 1;
            Chart.ChartAreas[0].AxisX.Interval = 1;
            Chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;

            SetLegendSymbols();
            SetPedestrianColors();
            SetStopColors();
        }


        private void SetLegendSymbols()
        {
            var legendSymbolsSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.Double
            };
            var bottomLabelOffset = 0.0;
            var topLabelOffset = bottomLabelOffset + 1.0;
            var p0 = legendSymbolsSeries.Points.AddXY(0.5, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Square;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.MediumPurple;
            legendSymbolsSeries.Points[p0].MarkerSize = 10;
            legendSymbolsSeries.Name = ".          Medium Purple Square: Advanced Count with Time Offset, Detector Off, Code 81";
            Chart.Series.Add(legendSymbolsSeries);
            var countOfCustomLabels = Chart.ChartAreas[0].AxisY.CustomLabels.Count;

            var timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[countOfCustomLabels].Name, 0,
                    LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);

            legendSymbolsSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.Double,
            };
            bottomLabelOffset += 1.0;
            topLabelOffset = bottomLabelOffset + 1.0;
            p0 = legendSymbolsSeries.Points.AddXY(0.5, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.DarkViolet;
            legendSymbolsSeries.Points[p0].MarkerSize = 12;
            legendSymbolsSeries.Name = ".          Dark Purple Triangle: Advanced Count with Time Offset, Detector On, Event Code 82";
            Chart.Series.Add(legendSymbolsSeries);
            countOfCustomLabels = Chart.ChartAreas[0].AxisY.CustomLabels.Count;
            timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[countOfCustomLabels].Name, 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);

            legendSymbolsSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Double
            };
            bottomLabelOffset += 1.0;
            topLabelOffset = bottomLabelOffset + 1.0;
            p0 = legendSymbolsSeries.Points.AddXY(0.25, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerSize = 12;
            p0 = legendSymbolsSeries.Points.AddXY(0.75, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Black;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerSize = 12;
            legendSymbolsSeries.BorderWidth = 2;
            Chart.Series.Add(legendSymbolsSeries);
            legendSymbolsSeries.Name = ".          Line Connecting Detector On, (Code 82) to Detector Off (Code 81)";
            countOfCustomLabels = Chart.ChartAreas[0].AxisY.CustomLabels.Count;
            timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[countOfCustomLabels].Name, 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);

            legendSymbolsSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.Double
            };
            bottomLabelOffset += 1.0;
            topLabelOffset = bottomLabelOffset + 1.0;
            p0 = legendSymbolsSeries.Points.AddXY(0.5, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Square;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.LightSlateGray;
            legendSymbolsSeries.Points[p0].MarkerSize = 10;
            legendSymbolsSeries.Name = ".          Light Gray Square: Detector Off, Code 81";
            Chart.Series.Add(legendSymbolsSeries);
            countOfCustomLabels = Chart.ChartAreas[0].AxisY.CustomLabels.Count;
            timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[countOfCustomLabels].Name, 0,
                    LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            legendSymbolsSeries = new Series
            {
                ChartType = SeriesChartType.Point,
                XValueType = ChartValueType.Double,
            };
            bottomLabelOffset += 1.0;
            topLabelOffset = bottomLabelOffset + 1.0;
            p0 = legendSymbolsSeries.Points.AddXY(0.5, bottomLabelOffset + 0.5);
            legendSymbolsSeries.Points[p0].Color = Color.Transparent;
            legendSymbolsSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendSymbolsSeries.Points[p0].MarkerColor = Color.Black;
            legendSymbolsSeries.Points[p0].MarkerSize = 12;
            legendSymbolsSeries.Name = ".          Black Triangle: Detector On, Code 82";
            Chart.Series.Add(legendSymbolsSeries);
            countOfCustomLabels = Chart.ChartAreas[0].AxisY.CustomLabels.Count;
            timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[countOfCustomLabels].Name, 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
        }

        private void SetPedestrianColors()
        {
            var rowNumber = 5.0;
            //var rowNumber = Chart.ChartAreas[0].AxisY.CustomLabels.Count;
            var labelNameForLegend = "Gray Fill Box: Pedestrian Begin Solid Don't Walk, Event Codes 23 & 69 ";
            MakeLineColorForLegendLine(Color.Gray, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Deep Blue Fill Box: Pedestrian Begin Clearance, Event Codes 22 & 68";
            MakeLineColorForLegendLine(Color.DeepSkyBlue, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Light Blue Fill Box: Pedestrian Begin Walk, Event Codes 21 & 67";
            MakeLineColorForLegendLine(Color.LightSkyBlue, labelNameForLegend, rowNumber);
        }

        private void MakeLineColorForLegendLine(Color lineColorForLegend, string labelNameForLegend, double rowNumber)
        {
            var legendLineSeries = new Series
            {
                ChartType = SeriesChartType.StepLine,
                XValueType = ChartValueType.Double
            };
            var topOffset = rowNumber + 1;
            var p0 = legendLineSeries.Points.AddXY(0.25, rowNumber + 0.5);
            legendLineSeries.Points[p0].Color = Color.Transparent;
            legendLineSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendLineSeries.Points[p0].MarkerColor = Color.Transparent;
            legendLineSeries.Points[p0].MarkerSize = 1;

            p0 = legendLineSeries.Points.AddXY(0.75, rowNumber + 0.5);
            legendLineSeries.Points[p0].Color = lineColorForLegend;
            legendLineSeries.Points[p0].MarkerStyle = MarkerStyle.Triangle;
            legendLineSeries.Points[p0].MarkerColor = Color.Transparent;
            legendLineSeries.Points[p0].MarkerSize = 1;
            legendLineSeries.Points[p0].BorderWidth = 25;
            legendLineSeries.Name = labelNameForLegend;
            Chart.Series.Add(legendLineSeries);
            var timingAxisLabel = new CustomLabel(rowNumber, topOffset, labelNameForLegend, 0,
                LabelMarkStyle.None);
            Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
        }

        private void SetStopColors()
        {
            var localColorDarkGreen = Color.MediumSeaGreen; //MediumSeaGreen
            var localColorGreen = Color.LightGreen; //LightGreen
            var localColorYellow = Color.Yellow;
            var localColorDarkRed = Color.Firebrick; //Firebrick

            var localColorRed = Color.LightCoral; //light coral
            var localColorGray = Color.DarkGray;
            var localColorBlack = Color.Black;
            var localColor62Green = Color.LimeGreen;
            var localColor61Green = Color.LightGreen;
            var stopColorsSeries = new Series
            {
                ChartType = SeriesChartType.StepLine,
                XValueType = ChartValueType.Double
            };

            var rowNumber = 8.0;
            var labelNameForLegend = "Light Gray Fill Box: No Vehicle Display Cycles Found";
            MakeLineColorForLegendLine(Color.Gainsboro, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Light Red Fill Box: Phase End Red Clearance, Overlap off (Inactive with Red indication) Event Codes 11 & 65";
            MakeLineColorForLegendLine(Color.LightCoral, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Dark Red Fill Box: Phase End Yellow Clearance, Overlap Begin Red Clearance, Event Codes 9 & 64";
            MakeLineColorForLegendLine(Color.Firebrick, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Light Yellow Fill Box: Phase Begin Yellow Clearance, Begin Overlap Yellow, Event Codes 8 & 63";
            MakeLineColorForLegendLine(Color.Yellow, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Light Green Fill Box: Overlap Begin Trailing Green (Extension), Event Code 62";
            MakeLineColorForLegendLine(Color.LightGreen, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Lime Green Fill Box: Overlap Begin Green, Event Code 61";
            MakeLineColorForLegendLine(Color.LimeGreen, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Medium Spring Green Fill Box: Phase Min Complete, Event Code 3";
            MakeLineColorForLegendLine(Color.MediumSpringGreen, labelNameForLegend, rowNumber);
            rowNumber += 1.0;
            labelNameForLegend = "Dark Green Fill Box: Phae Begin Green, Event Code 1";
            MakeLineColorForLegendLine(Color.MediumSeaGreen, labelNameForLegend, rowNumber);

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
                    _laneOffset = (double)++_lanesProcessed * 0.2 - 0.3;
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
            TimingAndActuationsForPhase.Options.HeadTitleCounter++;
            _headerDisaplay = 0;
            if (TimingAndActuationsForPhase.Options.ShowHeaderForEachPhase)
            {
                Chart.Titles.Add(ChartTitleFactory.GetChartName(TimingAndActuationsForPhase.Options.MetricTypeID));
                Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                    TimingAndActuationsForPhase.Options.SignalID, TimingAndActuationsForPhase.Options.StartDate,
                    TimingAndActuationsForPhase.Options.EndDate));
                _headerDisaplay = 120;
            }
            else
            {
                if (TimingAndActuationsForPhase.Options.HeadTitleCounter == 1)
                {
                    Chart.Titles.Add(ChartTitleFactory.GetChartName(TimingAndActuationsForPhase.Options.MetricTypeID));
                    //Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                    //TimingAndActuationsForPhase.Options.SignalID, TimingAndActuationsForPhase.Options.StartDate,
                    //TimingAndActuationsForPhase.Options.EndDate));

                    Chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                        TimingAndActuationsForPhase.Options.SignalID, TimingAndActuationsForPhase.Options.StartDate,
                        TimingAndActuationsForPhase.Options.EndDate));
                    _headerDisaplay = 120;
                }
            }
            if (TimingAndActuationsForPhase.Options.ShowRawEventData)
            {
                Chart.Titles.Add(
                    ChartTitleFactory.GetPhaseOrOverlap(TimingAndActuationsForPhase.PhaseNumber, TimingAndActuationsForPhase.PhaseOrOverlap));
            }
            else
            {
                Chart.Titles.Add(
                    ChartTitleFactory.GetPhaseAndPhaseDescriptions(TimingAndActuationsForPhase.Approach,
                        TimingAndActuationsForPhase.GetPermissivePhase));
            }
        }

        public void SetPedestrianInterval()
        {
            if (TimingAndActuationsForPhase == null) return;
            if (!TimingAndActuationsForPhase.PedestrianIntervals.Any()) return;
            var pedestrianIntervalsSeries = new Series
            {
                ChartType = SeriesChartType.StepLine,
                XValueType = ChartValueType.DateTime,
                BorderWidth = 15,
                Name = "Pedestrian Intervals"
            };
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup == true)
            {
                pedestrianIntervalsSeries.BorderWidth = 10;
            }

            foreach (var phase in TimingAndActuationsForPhase.PedestrianIntervals)
            {
                var pointNumber = new int();
                switch (phase.EventCode)
                {
                    // This type of line, the color is to the left of the point.  So everything is shifted.
                    // To the right of code 21, the color should be LightSkyBlue; to the right of 22 DeepSkyBlue, and 23 is Gray.
                    case 21:
                    case 67:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.Gray;
                        break;
                    case 22:
                    case 68:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.LightSkyBlue;
                        break;
                    case 23:
                    case 69:
                        pointNumber = pedestrianIntervalsSeries.Points.AddXY(
                            phase.Timestamp.ToOADate(), _yValue);
                        pedestrianIntervalsSeries.Points[pointNumber].Color = Color.DeepSkyBlue;
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
            var pedPhaseOrOverLap = TimingAndActuationsForPhase.Approach.IsPedestrianPhaseOverlap ? "Ovl" : "ph";
            var pedPhase = TimingAndActuationsForPhase.Approach.PedestrianPhaseNumber ?? TimingAndActuationsForPhase.Approach.ProtectedPhaseNumber;
            foreach (var pedEventElement in TimingAndActuationsForPhase.PedestrianEvents)
            {
                if (pedEventElement.Value.Count == 0) continue;
                var pedestrianActuation = new Series
                {
                    ChartType = SeriesChartType.Point,
                    XValueType = ChartValueType.DateTime,
                    Name = $"Ped Det. Actuations Ped {pedPhaseOrOverLap} {pedPhase}, ch {pedEventElement.Key}"
                };
                var pedEvents = pedEventElement.Value;
                if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
                {
                    pedestrianActuation.ChartType = SeriesChartType.Line;
                }
                if (TimingAndActuationsForPhase.Options.ShowEventPairs)
                {
                    for (var i = 0; i < pedEvents.Count; i++)
                    {
                        var seriesPointIndex = new int();
                        if (pedEvents[i].EventCode == 90)
                        {
                            seriesPointIndex = pedestrianActuation.Points.AddXY(
                                pedEvents[i].Timestamp.ToOADate(), _yValue);
                            pedestrianActuation.Points[seriesPointIndex].Color = Color.Transparent;
                            pedestrianActuation.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Triangle;
                            pedestrianActuation.Points[seriesPointIndex].MarkerColor = Color.Black;
                            pedestrianActuation.Points[seriesPointIndex].MarkerSize = _dotSize;
                        }
                        else if (pedEvents[i].EventCode == 89)
                        {
                            seriesPointIndex = pedestrianActuation.Points.AddXY(
                                pedEvents[i].Timestamp.ToOADate(), _yValue);
                            pedestrianActuation.Points[seriesPointIndex].Color = Color.Black;
                            pedestrianActuation.Points[seriesPointIndex].MarkerStyle = MarkerStyle.Square;
                            pedestrianActuation.Points[seriesPointIndex].MarkerColor = Color.LightSlateGray;
                            pedestrianActuation.Points[seriesPointIndex].MarkerSize = _dotSize;
                        }
                    }
                }
                else
                {
                    var lastItem = pedEvents.Count - 1;
                    if (lastItem <= 0) return;
                    for (var i = 0; i < lastItem; i++)
                    {
                        if (pedEvents[i].EventCode == 90 &&
                            pedEvents[i + 1].EventCode == 89)
                        {
                            var p0 = pedestrianActuation.Points.AddXY(
                                pedEvents[i].Timestamp.ToOADate(), _yValue);
                            pedestrianActuation.Points[p0].Color = Color.Transparent;
                            pedestrianActuation.Points[p0].MarkerStyle = MarkerStyle.Triangle;
                            pedestrianActuation.Points[p0].MarkerColor = Color.Black;
                            pedestrianActuation.Points[p0].MarkerSize = _dotSize;
                            var p1 = pedestrianActuation.Points.AddXY(
                                pedEvents[i + 1].Timestamp.ToOADate(), _yValue);
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
            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                stopBarSeries.ChartType = SeriesChartType.Line;
            }
            foreach (var stopBarEventElement in TimingAndActuationsForPhase.StopBarEvents)
            {
                if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
                {
                    _laneOffset = (double)++_lanesProcessed * 0.2 - 0.45;
                    _laneOffset = (_laneOffset > _yValue + 0.5) ? _yValue + 0.45 : _laneOffset;
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
                if (TimingAndActuationsForPhase.Options.ShowEventPairs)
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
                if (TimingAndActuationsForPhase.Options.ShowEventPairs)
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
            topLabel.Points.AddXY(TimingAndActuationsForPhase.Options.StartDate.ToOADate(), _yValue);
            Chart.Series.Add(topLabel);
            //_yValue++;
            var yMaximum = Math.Round(_yValue + 0.8, 0);
            Chart.ChartAreas[0].AxisY.Maximum = yMaximum;
            var rowMultipler = 25;
            var phaseHeight = 40;
            var height = yMaximum * rowMultipler + _headerDisaplay + phaseHeight;
            if (Chart.ChartAreas[0].AxisX.IntervalType == DateTimeIntervalType.Seconds && yMaximum < 3)
            {
                height = 125;
            }
            if (_headerDisaplay > 0 && yMaximum < 3)
            {
                height = 200;
            }

            if (height < 130)
            {
                height = 150;
            }
            if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
            {
                height *= 1.5;
            }
            Chart.Height = new Unit(height);
            Chart.ChartAreas[0].AxisY.Interval = 1;
            for (var i = 1; i < Chart.Series.Count; i++)
            {
                var bottomLabelOffset = (int)Chart.Series[i].Points[0].YValues[0];
                var topLabelOffset = bottomLabelOffset + 1;
                var sideLabel = Chart.Series[i].Name;
                var timingAxisLabel = new CustomLabel(bottomLabelOffset, topLabelOffset, Chart.Series[i].Name, 0,
                    LabelMarkStyle.None);
                Chart.ChartAreas[0].AxisY.CustomLabels.Add(timingAxisLabel);
            }
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
            if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)
            {
                advanceCountSeries.ChartType = SeriesChartType.Line;
            }
            if (TimingAndActuationsForPhase.Options.AdvancedOffset != 0.0)
            {
                advancedOffset = (float)TimingAndActuationsForPhase.Options.AdvancedOffset;
                darkColor = Color.DarkViolet;
                lightColor = Color.MediumPurple;
            }
            foreach (var advanceCountEventElement in TimingAndActuationsForPhase.AdvanceCountEvents)
            {
                if (advanceCountEventElement.Value.Any())
                {
                    if (TimingAndActuationsForPhase.Options.CombineLanesForEachGroup)
                    {
                        _laneOffset = (double)++_lanesProcessed * 0.2 - 0.2;
                        _laneOffset = (_laneOffset > _yValue + 0.5) ? _yValue + 0.5 : _laneOffset;
                    }
                    else
                    {
                        advanceCountSeries = new Series
                        {
                            ChartType = SeriesChartType.Point,
                            XValueType = ChartValueType.DateTime
                        };
                        if (TimingAndActuationsForPhase.Options.ShowLinesStartEnd)

                        {
                            advanceCountSeries.ChartType = SeriesChartType.Line;
                        }
                    }
                    var advanceEvents = advanceCountEventElement.Value;
                    if (TimingAndActuationsForPhase.Options.ShowEventPairs)
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
                    _laneOffset = (double)++_lanesProcessed * 0.2 - 0.3;
                    if (_laneOffset > _yValue + 0.5) _laneOffset = _yValue + 0.5;
                }
                var advancePresenceEvents = advancePresenceElement.Value;
                if (TimingAndActuationsForPhase.Options.ShowEventPairs)
                {
                    var seriesPointIndex = new int();
                    for (var i = 0; i < advancePresenceEvents.Count; i++)
                    {
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
            var localColorDarkGreen = Color.MediumSeaGreen; //code 1 MediumSeaGreen
            var localColorGreen = Color.MediumSpringGreen; // code 3 LightGreen
            var localColorYellow = Color.Yellow;  // code 8, code 63
            var localColorDarkRed = Color.Firebrick; //code 9, code 64 Firebrick

            var localColorRed = Color.LightCoral; // code 11, code 65 light coral
            var localColorGray = Color.DarkGray;
            var localColorBlack = Color.Black;
            var localColor61Green = Color.LimeGreen;  // code 61
            var localColor62Green = Color.LightGreen; // code 62

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
                    BackColor = Color.Gainsboro,
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
                        var vehicleStripeLine = new StripLine()
                        {
                            IntervalOffsetType = Chart.ChartAreas[0].AxisX.IntervalType,
                            StripWidthType = Chart.ChartAreas[0].AxisX.IntervalType,
                            IntervalType = Chart.ChartAreas[0].AxisX.IntervalType,
                            Interval = 1
                        };
                        switch (vehicleDisplayCycleValue[i].EventCode)
                        {
                            case 1:
                                {
                                    vehicleStripeLine.BackColor = localColorDarkGreen;
                                    break;
                                }
                            case 61:
                                {
                                    vehicleStripeLine.BackColor = localColor61Green;
                                    break;
                                }
                            case 3:
                                {
                                    vehicleStripeLine.BackColor = localColorGreen;
                                    break;
                                }
                            case 62:
                                {
                                    vehicleStripeLine.BackColor = localColor62Green;
                                    break;
                                }
                            case 8:
                            case 63:
                                {
                                    vehicleStripeLine.BackColor = localColorYellow;
                                    break;
                                }
                            case 9:
                            case 64:
                                {
                                    vehicleStripeLine.BackColor = localColorDarkRed;
                                    break;
                                }
                            case 11:
                            case 65:
                                {
                                    vehicleStripeLine.BackColor = localColorRed;
                                    break;
                                }
                        }
                        //var startTime = TimingAndActuationsForPhase.Options.StartDate.AddMinutes(-TimingAndActuationsForPhase.Options.ExtendSearch);
                        //var endTime = TimingAndActuationsForPhase.Options.EndDate.AddMinutes(TimingAndActuationsForPhase.Options.ExtendSearch);
                        var startTime = TimingAndActuationsForPhase.Options.StartDate;
                        var endTime = TimingAndActuationsForPhase.Options.EndDate;
                        var timeSpanStartOffset = vehicleDisplayCycleValue[i].Timestamp - startTime;
                        var timeSpanWidth = endTime - vehicleDisplayCycleValue[i].Timestamp;
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
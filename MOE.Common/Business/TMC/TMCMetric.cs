using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.TMC
{
    public class TMCMetric
    {
        public Chart chart;

        public TMCMetric(DateTime graphStartDate, DateTime graphEndDate,
            Models.Signal signal, DirectionType direction, List<Models.Detector> detectorsByDirection,
            LaneType laneType, MovementType movementType,
            TMCOptions options, TMCInfo tmcInfo)
        {
            chart = ChartFactory.CreateDefaultChart(options);
            Options = options;
            var extendedDirection = string.Empty;
            var reportTimespan = graphEndDate - graphStartDate;
            SetChartProperties();
            SetChartTitle(laneType, direction, movementType);
            //var chartArea = new ChartArea();
            //chartArea.Name = "ChartArea1";
            //AddXAxis(chartArea, reportTimespan);
            //AddYAxis(chartArea, movementType, options.YAxisMax, options.Y2AxisMax);
            //chart.ChartAreas.Add(chartArea);
            CreateAndAddSeries(graphStartDate, graphEndDate);
            CreatAndAddLegend(chart);
            AddDataToChart(graphStartDate, graphEndDate, detectorsByDirection,
                laneType, movementType, options, tmcInfo);
        }

        public TMCOptions Options { get; set; }

        private void CreatAndAddLegend(Chart chart)
        {
            var chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chartLegend.Docking = Docking.Bottom;
            chart.Legends.Add(chartLegend);
            chart.ChartAreas[0].AxisY.Title = "Volume (VPH)";
        }

        private void AddYAxis(ChartArea chartArea, MovementType movementType, double? thruMax, double? turnMax)
        {
            chartArea.AxisY.Title = "Volume (VPH)";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            if (movementType.Description == "Thru")
            {
                if (thruMax != null && thruMax > 0)
                {
                    chartArea.AxisY.Maximum = thruMax.Value;
                    if (thruMax.Value < 2000)
                        chartArea.AxisY.Interval = 100;
                    else
                        chartArea.AxisY.Interval = 500;
                }
            }
            else
            {
                if (turnMax != null && turnMax > 0)
                {
                    chartArea.AxisY.Maximum = turnMax.Value;
                    if (turnMax.Value < 2000)
                        chartArea.AxisY.Interval = 100;
                    else
                        chartArea.AxisY.Interval = 500;
                }
            }
        }

        private void AddXAxis(ChartArea chartArea, TimeSpan reportTimespan)
        {
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            //   chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            if (reportTimespan.Days < 1)
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX2.Interval = 1;
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                    chartArea.AxisX2.LabelStyle.Format = "HH:mm";
                }
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
        }

        private void SetChartTitle(LaneType laneType, DirectionType direction, MovementType movementType)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetBoldTitle(direction.Description + " " +
                                                            movementType.Description + " " + laneType.Description +
                                                            " Lanes"));
            if (Options.Y2AxisMax != null && (movementType.MovementTypeID == 2 || movementType.MovementTypeID == 3))
                chart.ChartAreas[0].AxisY.Maximum = Options.Y2AxisMax.Value;
        }

        private void SetChartProperties()
        {
            ChartFactory.SetImageProperties(chart);

            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineColor = Color.Black;
            chart.BorderlineWidth = 2;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
        }

        private void CreateAndAddSeries(DateTime graphStartDate, DateTime graphEndDate)
        {
            var TotalVolume = new Series();
            TotalVolume.ChartType = SeriesChartType.Line;
            TotalVolume.Name = "Total Volume";
            TotalVolume.Color = Color.Black;
            TotalVolume.BorderWidth = 2;

            //Lane Series
            var L1 = new Series();
            L1.ChartType = SeriesChartType.Line;
            L1.Color = Color.DarkRed;
            L1.Name = "Lane 1";
            L1.XValueType = ChartValueType.DateTime;
            L1.MarkerStyle = MarkerStyle.None;
            L1.MarkerSize = 1;

            var L2 = new Series();
            L2.ChartType = SeriesChartType.Line;
            L2.Color = Color.DarkOrange;
            L2.Name = "Lane 2";
            L2.XValueType = ChartValueType.DateTime;


            var L3 = new Series();
            L3.ChartType = SeriesChartType.Line;
            L3.Color = Color.Blue;
            L3.Name = "Lane 3";
            L3.XValueType = ChartValueType.DateTime;
            L3.BorderDashStyle = ChartDashStyle.Dot;
            L3.BorderWidth = 2;

            var L4 = new Series();
            L4.ChartType = SeriesChartType.Line;
            L4.Color = Color.DarkRed;
            L4.Name = "Lane 4";
            L4.XValueType = ChartValueType.DateTime;
            L4.BorderDashStyle = ChartDashStyle.Dot;
            L4.BorderWidth = 2;

            var LT = new Series();
            LT.ChartType = SeriesChartType.Line;
            LT.Color = Color.DarkOrange;
            LT.Name = "Thru Left";
            LT.XValueType = ChartValueType.DateTime;
            LT.BorderDashStyle = ChartDashStyle.Dot;
            LT.BorderWidth = 2;

            var RT = new Series();
            RT.ChartType = SeriesChartType.Line;
            RT.Color = Color.Blue;
            RT.Name = "Thru Right";
            RT.XValueType = ChartValueType.DateTime;
            RT.BorderDashStyle = ChartDashStyle.Solid;
            RT.BorderWidth = 1;

            //Add the Posts series to ensure the chart is the size of the selected timespan
            var posts = new Series();
            posts.ChartType = SeriesChartType.Point;
            posts.Color = Color.White;
            posts.Name = "Posts";
            posts.IsVisibleInLegend = false;
            posts.XValueType = ChartValueType.DateTime;

            chart.Series.Add(TotalVolume);
            chart.Series.Add(L1);
            chart.Series.Add(L2);
            chart.Series.Add(L3);
            chart.Series.Add(L4);
            chart.Series.Add(LT);
            chart.Series.Add(RT);
            chart.Series.Add(posts);
            chart.Series["Posts"].Enabled = true;

            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);
        }

        private void AddDataToChart(DateTime startDate, DateTime endDate,
            List<Models.Detector> detectorsByDirection, LaneType laneType, MovementType movementType,
            TMCOptions options, TMCInfo tmcInfo)
        {
            var MovementTotals = new SortedDictionary<DateTime, int>();
            var laneTotals = new SortedDictionary<string, int>();
            var binSizeMultiplier = 60 / options.SelectedBinSize;
            var totalVolume = 0;
            var tmcDetectors = new List<Models.Detector>();
            FindLaneDetectors(tmcDetectors, movementType, detectorsByDirection, laneType);
            var laneCount = tmcDetectors.Count();
            for (var ln = 1; ln < 5; ln++)
            {
                var detector = (from r in tmcDetectors
                    where r.LaneNumber == ln
                    select r).FirstOrDefault();
                if (detector != null)
                    if (laneCount > 0 && detector.MovementTypeID != 4 && detector.MovementTypeID != 5)
                    {
                        var d = new Detector(detector, startDate, endDate,
                            options.SelectedBinSize);
                        foreach (var volume in d.Volumes.Items)
                        {
                            if (options.ShowDataTable)
                            {
                                var tmcd = new TMCData();
                                tmcd.Direction = detector.Approach.DirectionType.Description;
                                tmcd.LaneType = detector.LaneType.Description;
                                if (!options.ShowLaneVolumes)
                                    tmcd.MovementType = tmcd.Direction;
                                else
                                    tmcd.MovementType = detector.MovementType.Abbreviation;
                                //tmcd.DetectorID = detector.DetectorID;
                                tmcd.Timestamp = volume.XAxis; //.AddMinutes(options.SelectedBinSize *-1);
                                tmcd.Count = volume.YAxis / binSizeMultiplier;
                                tmcInfo.tmcData.Add(tmcd);
                            }
                            if (options.ShowLaneVolumes)
                                chart.Series["Lane " + ln].Points.AddXY(volume.XAxis, volume.YAxis);
                            //One of the calculations requires total volume by lane.  This if statment keeps a 
                            //running total of that volume and stores it in a dictonary with the lane number.
                            if (laneTotals.ContainsKey("L" + ln))
                                laneTotals["L" + ln] += volume.YAxis;
                            else
                                laneTotals.Add("L" + ln, volume.YAxis);
                            //we need ot track the total number of cars (volume) for this movement.
                            //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
                            //Then the movement total can be plotted on the graph
                            if (MovementTotals.ContainsKey(volume.XAxis))
                                MovementTotals[volume.XAxis] += volume.YAxis;
                            else
                                MovementTotals.Add(volume.XAxis, volume.YAxis);
                        }
                    }
            }

            if (movementType.MovementTypeID == 1)
            {
                var thruTurnLanes = (from r in detectorsByDirection
                    where r.MovementTypeID == 4 ||
                          r.MovementTypeID == 5
                    select r).ToList();
                foreach (var detector in thruTurnLanes)
                {
                    var d = new Detector(detector, startDate, endDate, options.SelectedBinSize);
                    foreach (var volume in d.Volumes.Items)
                    {
                        if (detector.MovementType.Abbreviation == "TL")
                        {
                            {
                                if (options.ShowLaneVolumes)
                                {
                                    if (options.ShowDataTable)
                                    {
                                        var tmcd = new TMCData();
                                        tmcd.Direction = detector.Approach.DirectionType.Description;
                                        tmcd.LaneType = detector.LaneType.Description;
                                        if (!options.ShowLaneVolumes)
                                            tmcd.MovementType = tmcd.Direction;
                                        else
                                            tmcd.MovementType = detector.MovementType.Abbreviation;
                                        //tmcd.DetectorID = detector.DetectorID;
                                        tmcd.Timestamp = volume.XAxis; //.AddMinutes(options.SelectedBinSize * -1);
                                        tmcd.Count = volume.YAxis / binSizeMultiplier;
                                        tmcInfo.tmcData.Add(tmcd);
                                    }
                                    chart.Series["Thru Left"].Points.AddXY(volume.XAxis, volume.YAxis);
                                }
                            }
                            if (laneTotals.ContainsKey("TL"))
                                laneTotals["TL"] += volume.YAxis;
                            else
                                laneTotals.Add("TL", volume.YAxis);
                        }
                        if (detector.MovementType.Abbreviation == "TR")
                            if (options.ShowLaneVolumes)
                            {
                                if (options.ShowDataTable)
                                {
                                    var tmcd = new TMCData();
                                    tmcd.Direction = detector.Approach.DirectionType.Description;
                                    tmcd.LaneType = detector.LaneType.Description;
                                    if (!options.ShowLaneVolumes)
                                        tmcd.MovementType = tmcd.Direction;
                                    else
                                        tmcd.MovementType = detector.MovementType.Abbreviation;
                                    //tmcd.DetectorID = detector.DetectorID;
                                    tmcd.Timestamp = volume.XAxis; //.AddMinutes(options.SelectedBinSize * -1);
                                    tmcd.Count = volume.YAxis / binSizeMultiplier;
                                    tmcInfo.tmcData.Add(tmcd);
                                }
                                chart.Series["Thru Right"].Points.AddXY(volume.XAxis, volume.YAxis);
                            }
                        if (laneTotals.ContainsKey("TR"))
                            laneTotals["TR"] += volume.YAxis;
                        else
                            laneTotals.Add("TR", volume.YAxis);
                        if (MovementTotals.ContainsKey(volume.XAxis))
                            MovementTotals[volume.XAxis] += volume.YAxis;
                        else
                            MovementTotals.Add(volume.XAxis, volume.YAxis);
                    }
                }
            }
            var binMultiplier = 60 / options.SelectedBinSize;
            //get the total volume for the approach
            foreach (var totals in MovementTotals)
            {
                if (options.ShowTotalVolumes)
                    chart.Series["Total Volume"].Points.AddXY(totals.Key, totals.Value);
                totalVolume += totals.Value;
            }

            var highLaneVolume = 0;
            if (laneTotals.Values.Count > 0)
                highLaneVolume = laneTotals.Values.Max();


            var peakHourValue = findPeakHour(MovementTotals, binMultiplier);
            var PHV = peakHourValue.Value;
            var peakHour = peakHourValue.Key;
            var PeakHourMAXVolume = 0;

            var fluPlaceholder = "";

            if (laneCount > 0 && highLaneVolume > 0)
            {
                var fLU = Convert.ToDouble(totalVolume) /
                          (Convert.ToDouble(laneCount) * Convert.ToDouble(highLaneVolume));
                fluPlaceholder = SetSigFigs(fLU, 2).ToString();
            }
            else
            {
                fluPlaceholder = "Not Available";
            }


            for (var i = 0; i < binMultiplier; i++)
                if (MovementTotals.ContainsKey(peakHour.AddMinutes(i * options.SelectedBinSize)))
                    if (PeakHourMAXVolume < MovementTotals[peakHour.AddMinutes(i * options.SelectedBinSize)])
                        PeakHourMAXVolume = MovementTotals[peakHour.AddMinutes(i * options.SelectedBinSize)];

            var PHFPlaceholder = FindPHF(PHV, PeakHourMAXVolume, binMultiplier);


            var peakHourString = peakHour.ToShortTimeString() + " - " + peakHour.AddHours(1).ToShortTimeString();
            var statistics = new Dictionary<string, string>();
            statistics.Add("Total Volume", (totalVolume / binMultiplier).ToString());
            statistics.Add("Peak Hour", peakHourString);
            statistics.Add("Peak Hour Volume", PHV / binMultiplier + " VPH");
            statistics.Add("PHF", PHFPlaceholder);
            statistics.Add("fLU", fluPlaceholder);
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
            SetSeriesVisibility(laneCount, options.ShowLaneVolumes);
        }

        private void FindLaneDetectors(List<Models.Detector> tmcDetectors, MovementType movementType,
            List<Models.Detector> detectorsByDirection, LaneType laneType)
        {
            foreach (var detector in detectorsByDirection)
                if (detector.LaneType.LaneTypeID == laneType.LaneTypeID)
                    if (movementType.MovementTypeID == 1)
                    {
                        if (detector.MovementType.Description == "Thru" ||
                            detector.MovementType.Description == "Thru-Right" ||
                            detector.MovementType.Description == "Thru-Left")
                            tmcDetectors.Add(detector);
                    }
                    else if (detector.MovementType.MovementTypeID == movementType.MovementTypeID)
                    {
                        tmcDetectors.Add(detector);
                    }
        }


        private void SetSeriesVisibility(int laneCount, bool showLaneVolumes)
        {
            foreach (var series in chart.Series)
            {
                if (series.Points.Count < 1)
                    series.IsVisibleInLegend = false;
                else
                    series.IsVisibleInLegend = true;

                if (series.Name == "Posts")
                    series.IsVisibleInLegend = false;
            }


            if (laneCount == 1 && showLaneVolumes)
            {
                chart.Series["Total Volume"].Enabled = false;
                chart.Series["Total Volume"].IsVisibleInLegend = false;
            }
        }

        private string FindPHF(int PHV, int PeakHourMAXVolume, int binMultiplier)
        {
            var PHFPlaceholder = "";

            if (PeakHourMAXVolume > 0)
            {
                var PHF = SetSigFigs(
                    Convert.ToDouble(PHV) / (Convert.ToDouble(PeakHourMAXVolume) * Convert.ToDouble(binMultiplier)), 2);
                PHFPlaceholder = PHF.ToString();
            }
            else
            {
                PHFPlaceholder = "Not Avialable";
            }

            return PHFPlaceholder;
        }


        private KeyValuePair<DateTime, int> findPeakHour(SortedDictionary<DateTime, int> dirVolumes, int binMultiplier)
        {
            var subTotal = 0;
            var peakHourValue = new KeyValuePair<DateTime, int>();

            var startTime = new DateTime();
            var iteratedVolumes = new SortedDictionary<DateTime, int>();

            for (var i = 0; i < dirVolumes.Count - (binMultiplier - 1); i++)
            {
                startTime = dirVolumes.ElementAt(i).Key;
                subTotal = 0;
                for (var x = 0; x < binMultiplier; x++)
                    subTotal = subTotal + dirVolumes.ElementAt(i + x).Value;
                iteratedVolumes.Add(startTime, subTotal);
            }

            //Find the highest value in the iterated Volumes dictionary.
            //This should bee the peak hour.
            foreach (var kvp in iteratedVolumes)
                if (kvp.Value > peakHourValue.Value)
                    peakHourValue = kvp;

            return peakHourValue;
        }


        private static double SetSigFigs(double d, int digits)
        {
            var scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

            return scale * Math.Round(d / scale, digits);
        }
    }
}
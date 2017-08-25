using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business.TMC
{
    public class TMCMetric
    {
        public Chart chart = new Chart();
        public WCFServiceLibrary.TMCOptions Options { get; set; }

        public TMCMetric(DateTime graphStartDate, DateTime graphEndDate,
            Models.Signal signal, Models.DirectionType direction, List<Models.Detector> detectorsByDirection,
            Models.LaneType laneType, Models.MovementType movementType,
            MOE.Common.Business.WCFServiceLibrary.TMCOptions options, MOE.Common.Business.TMC.TMCInfo tmcInfo)
        {
            Options = options;
            string extendedDirection = string.Empty;
            TimeSpan reportTimespan = graphEndDate - graphStartDate;
            SetChartProperties();
            SetChartTitle(laneType, direction,movementType);
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            AddXAxis(chartArea, reportTimespan);
            AddYAxis(chartArea, movementType, options.YAxisMax, options.Y2AxisMax);
            chart.ChartAreas.Add(chartArea);
            CreateAndAddSeries(graphStartDate, graphEndDate);
            CreatAndAddLegend(chart);
            AddDataToChart(graphStartDate, graphEndDate, direction, detectorsByDirection, 
                laneType, movementType, options, tmcInfo);
        }

        private void CreatAndAddLegend(Chart chart)
        {
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chartLegend.Docking = Docking.Bottom;
            chart.Legends.Add(chartLegend);
        }

        private void AddYAxis(ChartArea chartArea, Models.MovementType movementType, double? thruMax, double? turnMax)
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
                    {
                        chartArea.AxisY.Interval = 100;
                    }
                    else
                    {
                        chartArea.AxisY.Interval = 500;
                    }
                }

            }
            else
            {

                if (turnMax != null && turnMax > 0)
                {
                    chartArea.AxisY.Maximum = turnMax.Value;
                    if (turnMax.Value < 2000)
                    {
                        chartArea.AxisY.Interval = 100;
                    }
                    else
                    {
                        chartArea.AxisY.Interval = 500;
                    }
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
            {
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
            }
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;

        }

        private void SetChartTitle(Models.LaneType laneType, Models.DirectionType direction, Models.MovementType movementType)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(Options.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(
                Options.SignalID, Options.StartDate, Options.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetBoldTitle(direction.Description + " " +
                movementType.Description + " " + laneType.Description + " Lanes"));
        }

        private void SetChartProperties()
        {

            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 550;
            chart.Width = 750;

            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineColor = Color.Black;
            chart.BorderlineWidth = 2;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
        }

        private void CreateAndAddSeries(DateTime graphStartDate, DateTime graphEndDate)
        {

            Series TotalVolume = new Series();
            TotalVolume.ChartType = SeriesChartType.Line;
            TotalVolume.Name = "Total Volume";
            TotalVolume.Color = Color.Black;
            TotalVolume.BorderWidth = 2;

            //Lane Series
            Series L1 = new Series();
            L1.ChartType = SeriesChartType.Line;
            L1.Color = Color.DarkRed;
            L1.Name = "Lane 1";
            L1.XValueType = ChartValueType.DateTime;
            L1.MarkerStyle = MarkerStyle.None;
            L1.MarkerSize = 1;

            Series L2 = new Series();
            L2.ChartType = SeriesChartType.Line;
            L2.Color = Color.DarkOrange;
            L2.Name = "Lane 2";
            L2.XValueType = ChartValueType.DateTime;


            Series L3 = new Series();
            L3.ChartType = SeriesChartType.Line;
            L3.Color = Color.Blue;
            L3.Name = "Lane 3";
            L3.XValueType = ChartValueType.DateTime;
            L3.BorderDashStyle = ChartDashStyle.Dot;
            L3.BorderWidth = 2;

            Series L4 = new Series();
            L4.ChartType = SeriesChartType.Line;
            L4.Color = Color.DarkRed;
            L4.Name = "Lane 4";
            L4.XValueType = ChartValueType.DateTime;
            L4.BorderDashStyle = ChartDashStyle.Dot;
            L4.BorderWidth = 2;

            Series LT = new Series();
            LT.ChartType = SeriesChartType.Line;
            LT.Color = Color.DarkOrange;
            LT.Name = "Thru Left";
            LT.XValueType = ChartValueType.DateTime;
            LT.BorderDashStyle = ChartDashStyle.Dot;
            LT.BorderWidth = 2;

            Series RT = new Series();
            RT.ChartType = SeriesChartType.Line;
            RT.Color = Color.Blue;
            RT.Name = "Thru Right";
            RT.XValueType = ChartValueType.DateTime;
            RT.BorderDashStyle = ChartDashStyle.Solid;
            RT.BorderWidth = 1;

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series posts = new Series();
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

        private void AddDataToChart(DateTime startDate, DateTime endDate, Models.DirectionType direction, 
            List<Models.Detector> detectorsByDirection, Models.LaneType laneType, Models.MovementType movementType,
            MOE.Common.Business.WCFServiceLibrary.TMCOptions options, MOE.Common.Business.TMC.TMCInfo tmcInfo)
        {
            SortedDictionary<DateTime, int> MovementTotals = new SortedDictionary<DateTime, int>();
            SortedDictionary<string, int> laneTotals = new SortedDictionary<string, int>();
            int binSizeMultiplier = 60 / options.SelectedBinSize;
            int totalVolume = 0;
            List<MOE.Common.Models.Detector> tmcDetectors = new List<Models.Detector>();
            FindLaneDetectors(tmcDetectors, movementType, detectorsByDirection, laneType);            
            int laneCount = tmcDetectors.Count();
            for (int ln = 1; ln < 5; ln++)
            {
                Models.Detector detector = (from r in tmcDetectors
                                            where r.LaneNumber == ln
                                            select r).FirstOrDefault();
                if (detector != null)
                {
                    if (laneCount > 0 && detector.MovementTypeID != 4 && detector.MovementTypeID != 5)
                    {
                        MOE.Common.Business.Detector d = new MOE.Common.Business.Detector(detector, startDate, endDate, 
                            options.SelectedBinSize);
                        foreach (MOE.Common.Business.Volume volume in d.Volumes.Items)
                        {
                            if (options.ShowDataTable)
                            {
                               
                                MOE.Common.Business.TMC.TMCData tmcd = new TMC.TMCData();
                                tmcd.Direction = detector.Approach.DirectionType.Description;
                                tmcd.LaneType = detector.LaneType.Description;
                                if (!options.ShowLaneVolumes)
                                {
                                    tmcd.MovementType = tmcd.Direction;
                                }
                                else
                                {
                                    tmcd.MovementType = detector.MovementType.Abbreviation;
                                }
                                //tmcd.DetectorID = detector.DetectorID;
                                tmcd.Timestamp = volume.XAxis.AddMinutes(options.SelectedBinSize *-1);
                                tmcd.Count = volume.YAxis / binSizeMultiplier;
                                tmcInfo.tmcData.Add(tmcd);
                            }
                            if (options.ShowLaneVolumes)
                            {                                
                                chart.Series["Lane " + ln.ToString()].Points.AddXY(volume.XAxis, volume.YAxis);
                            }
                            //One of the calculations requires total volume by lane.  This if statment keeps a 
                            //running total of that volume and stores it in a dictonary with the lane number.
                            if (laneTotals.ContainsKey("L" + ln))
                            {
                                laneTotals["L" + ln] += volume.YAxis;
                            }
                            else
                            {
                                laneTotals.Add("L" + ln, volume.YAxis);
                            }
                            //we need ot track the total number of cars (volume) for this movement.
                            //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
                            //Then the movement total can be plotted on the graph
                            if (MovementTotals.ContainsKey(volume.XAxis))
                            {
                                MovementTotals[volume.XAxis] += volume.YAxis;
                            }
                            else
                            {
                                MovementTotals.Add(volume.XAxis, volume.YAxis);
                            }
                        }
                    }
                }

            }

            if (movementType.MovementTypeID == 1)
            {
                List<Models.Detector> thruTurnLanes = (from r in detectorsByDirection
                                                       where r.MovementTypeID == 4 ||
                                                       r.MovementTypeID == 5
                                                       select r).ToList();
                foreach (Models.Detector detector in thruTurnLanes)
                {
                    MOE.Common.Business.Detector d = new MOE.Common.Business.Detector(detector, startDate, endDate,options.SelectedBinSize);
                    foreach (MOE.Common.Business.Volume volume in d.Volumes.Items)
                    {
                        if (detector.MovementType.Abbreviation == "TL")
                        {
                            {
                                if (options.ShowLaneVolumes)
                                {
                                    if (options.ShowDataTable)
                                    {
                                        
                                        MOE.Common.Business.TMC.TMCData tmcd = new TMC.TMCData();
                                        tmcd.Direction = detector.Approach.DirectionType.Description;
                                        tmcd.LaneType = detector.LaneType.Description;
                                        if (!options.ShowLaneVolumes)
                                        {
                                            tmcd.MovementType = tmcd.Direction;
                                        }
                                        else
                                        {
                                            tmcd.MovementType = detector.MovementType.Abbreviation;
                                        }
                                        //tmcd.DetectorID = detector.DetectorID;
                                        tmcd.Timestamp = volume.XAxis.AddMinutes(options.SelectedBinSize * -1);
                                        tmcd.Count = volume.YAxis / binSizeMultiplier;
                                        tmcInfo.tmcData.Add(tmcd);
                                    }
                                    chart.Series["Thru Left"].Points.AddXY(volume.XAxis, volume.YAxis);
                                }
                            }
                            if (laneTotals.ContainsKey("TL"))
                            {
                                laneTotals["TL"] += volume.YAxis;
                            }
                            else
                            {
                                laneTotals.Add("TL", volume.YAxis);
                            }

                        }
                        if (detector.MovementType.Abbreviation == "TR")
                        {
                            if (options.ShowLaneVolumes)
                            {
                                if (options.ShowDataTable)
                                {
                                    
                                    MOE.Common.Business.TMC.TMCData tmcd = new TMC.TMCData();
                                    tmcd.Direction = detector.Approach.DirectionType.Description;
                                    tmcd.LaneType = detector.LaneType.Description;
                                    if (!options.ShowLaneVolumes)
                                    {
                                        tmcd.MovementType = tmcd.Direction;
                                    }
                                    else
                                    {
                                        tmcd.MovementType = detector.MovementType.Abbreviation;
                                    }
                                    //tmcd.DetectorID = detector.DetectorID;
                                    tmcd.Timestamp = volume.XAxis.AddMinutes(options.SelectedBinSize * -1);
                                    tmcd.Count = volume.YAxis / binSizeMultiplier;
                                    tmcInfo.tmcData.Add(tmcd);
                                }
                                chart.Series["Thru Right"].Points.AddXY(volume.XAxis, volume.YAxis);
                            }
                        }
                        if (laneTotals.ContainsKey("TR"))
                        {
                            laneTotals["TR"] += volume.YAxis;
                        }
                        else
                        {
                            laneTotals.Add("TR", volume.YAxis);
                        }
                        if (MovementTotals.ContainsKey(volume.XAxis))
                        {
                            MovementTotals[volume.XAxis] += volume.YAxis;
                        }
                        else
                        {
                            MovementTotals.Add(volume.XAxis, volume.YAxis);
                        }
                    }
                }
            }
            int binMultiplier = 60 / options.SelectedBinSize;
            //get the total volume for the approach
            foreach (KeyValuePair<DateTime, int> totals in MovementTotals)
            {
                if (options.ShowTotalVolumes)
                {
                    chart.Series["Total Volume"].Points.AddXY(totals.Key, totals.Value);
                }
                totalVolume += (totals.Value);
            }

            int highLaneVolume = 0;
            if (laneTotals.Values.Count > 0)
            {
                highLaneVolume = laneTotals.Values.Max();
            }


            KeyValuePair<DateTime, int> peakHourValue = findPeakHour(MovementTotals, binMultiplier);
            int PHV = peakHourValue.Value;
            DateTime peakHour = peakHourValue.Key;
            int PeakHourMAXVolume = 0;

            string fluPlaceholder = "";

            if (laneCount > 0 && highLaneVolume > 0)
            {
                double fLU = Convert.ToDouble(totalVolume) / (Convert.ToDouble(laneCount) * Convert.ToDouble(highLaneVolume));
                fluPlaceholder = SetSigFigs(fLU, 2).ToString();
            }
            else
            {
                fluPlaceholder = "Not Available";
            }



         
            for (int i = 0; i < binMultiplier; i++)
            {
                if (MovementTotals.ContainsKey(peakHour.AddMinutes(i * options.SelectedBinSize)))
                {
                    if (PeakHourMAXVolume < (MovementTotals[peakHour.AddMinutes(i * options.SelectedBinSize)]))
                    {
                        PeakHourMAXVolume = MovementTotals[peakHour.AddMinutes(i * options.SelectedBinSize)];
                    }
                }
            }

            string PHFPlaceholder = FindPHF(PHV, PeakHourMAXVolume, binMultiplier);


            string peakHourString = peakHour.ToShortTimeString() + " - " + peakHour.AddHours(1).ToShortTimeString();            
            Dictionary<string, string> statistics = new Dictionary<string, string>();
            statistics.Add("Total Volume", (totalVolume / binMultiplier).ToString());
            statistics.Add("Peak Hour", peakHourString);
            statistics.Add("Peak Hour Volume", (PHV / binMultiplier).ToString() + " VPH");
            statistics.Add("PHF", PHFPlaceholder);
            statistics.Add("fLU", fluPlaceholder);
            chart.Titles.Add(ChartTitleFactory.GetStatistics(statistics));
            SetSeriesVisibility(laneCount, options.ShowLaneVolumes);

        }

        private void FindLaneDetectors(List<Models.Detector> tmcDetectors, Models.MovementType movementType, List<Models.Detector> detectorsByDirection, Models.LaneType laneType)
        {
            foreach (MOE.Common.Models.Detector detector in detectorsByDirection)
            {
                if (detector.LaneType.LaneTypeID == laneType.LaneTypeID)
                {
                    if (movementType.MovementTypeID == 1)
                    {
                        if (detector.MovementType.Description == "Thru" || detector.MovementType.Description == "Thru-Right" || detector.MovementType.Description == "Thru-Left")
                        {
                            tmcDetectors.Add(detector);
                        }
                    }
                    else if (detector.MovementType.MovementTypeID == movementType.MovementTypeID)
                    {
                        tmcDetectors.Add(detector);
                    }
                }
            }
        }


        private void SetSeriesVisibility(int laneCount, bool showLaneVolumes)
        {
            foreach (Series series in chart.Series)
            {
                if (series.Points.Count < 1  )
                {
                    series.IsVisibleInLegend = false;
                }
                else
                {
                    series.IsVisibleInLegend = true;
                }

                if(series.Name == "Posts")
                {
                    series.IsVisibleInLegend = false;
                }
            }
            

            if (laneCount == 1 && showLaneVolumes == true)
            {
                chart.Series["Total Volume"].Enabled = false;
                chart.Series["Total Volume"].IsVisibleInLegend = false;
            }
        }

        private string FindPHF(int PHV, int PeakHourMAXVolume, int binMultiplier)
        {
            string PHFPlaceholder = "";

            if (PeakHourMAXVolume > 0)
            {
                double PHF = SetSigFigs(Convert.ToDouble(PHV) / (Convert.ToDouble(PeakHourMAXVolume) * Convert.ToDouble(binMultiplier)), 2);
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
            int subTotal = 0;
            KeyValuePair<DateTime, int> peakHourValue = new KeyValuePair<DateTime, int>();

            DateTime startTime = new DateTime();
            SortedDictionary<DateTime, int> iteratedVolumes = new SortedDictionary<DateTime, int>();

            for (int i = 0; i < (dirVolumes.Count - (binMultiplier - 1)); i++)
            {
                startTime = dirVolumes.ElementAt(i).Key;
                subTotal = 0;
                for (int x = 0; x < binMultiplier; x++)
                {
                    subTotal = subTotal + dirVolumes.ElementAt(i + x).Value;
                }
                iteratedVolumes.Add(startTime, subTotal);
            }

            //Find the highest value in the iterated Volumes dictionary.
            //This should bee the peak hour.
            foreach (KeyValuePair<DateTime, int> kvp in iteratedVolumes)
            {
                if (kvp.Value > peakHourValue.Value)
                {
                    peakHourValue = kvp;
                }
            }

            return peakHourValue;
        }



        private static double SetSigFigs(double d, int digits)
        {

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

            return scale * Math.Round(d / scale, digits);
        }
    }
}




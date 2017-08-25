using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business
{
    public class BikeLaneChart
    {
        public Chart chart = new Chart();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphStartDate"></param>
        /// <param name="graphEndDate"></param>
        /// <param name="signalId"></param>
        /// <param name="location"></param>
        /// <param name="direction"></param>
        /// <param name="movement"></param>
        /// <param name="thruMax"></param>
        /// <param name="turnMax"></param>
        /// <param name="detectors"></param>
        /// <param name="plans"></param>
        /// <param name="isThruMovement"></param>
        /// <param name="showLaneVolumes"></param>
        /// <param name="showTotalVolumes"></param>
        /// <param name="binSize"></param>
        /// 

        public BikeLaneChart(DateTime graphStartDate, DateTime graphEndDate, string signalId, string location,
            string direction, string movement, double thruMax, double turnMax, List<Detector> Thrulanes, List<Detector> RTlanes, List<Detector> LTlanes,
            MOE.Common.Business.PlanCollection plans, bool isThruMovement, bool showLaneVolumes, bool showTotalVolumes, 
            int binSize)
        {

            
            string extendedDirection = string.Empty;

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 367;
            chart.Width = 734;

            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineColor = Color.Black;
            chart.BorderlineWidth = 2;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            //Set the chart title
            chart.Titles.Add(location + "SIG#" + signalId.ToString() + "\n" + graphStartDate.ToString("f") + " - " + graphEndDate.ToString("f"));
            chart.Titles.Add("Bike Lane " + direction + " " + movement);

            chart.Titles[1].Font = new Font(chart.Titles[0].Font, FontStyle.Bold);

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";


            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
           // chartLegend.Title = direction + " " + movement;
            chartLegend.Docking = Docking.Bottom;
            chart.Legends.Add(chartLegend);
            


            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            //   chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;


            chartArea.AxisY.Title = "Volume (Bikes Per Hour)";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
           
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            if (movement.IndexOf("Thru") > -1)
            {
             

                if (thruMax > 0)
                {
                    chartArea.AxisY.Maximum = thruMax;
                }
                else
                {
                    chartArea.AxisY.Maximum = 1000;
                }

            }
            else
            {
                if (turnMax> 0)
                {
                    chartArea.AxisY.Maximum = turnMax;
                }
                else
                {
                    chartArea.AxisY.Maximum = 300;
                }
            }
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 15;
            chartArea.AxisY.Interval = 2;

            chart.ChartAreas.Add(chartArea);

            Series TotalVolume = new Series();
            TotalVolume.ChartType = SeriesChartType.Line;
            TotalVolume.Name = "Total Volume";
            TotalVolume.Color = Color.Black;
            TotalVolume.BorderWidth = 2;


            //Lane Series
            Series Thru = new Series();
            Thru.ChartType = SeriesChartType.Line;
            Thru.Color = Color.Blue;
            Thru.Name = "Thru";
            Thru.XValueType = ChartValueType.DateTime;
            Thru.MarkerStyle = MarkerStyle.None;
            Thru.MarkerSize = 1;

            Series Right = new Series();
            Right.ChartType = SeriesChartType.Line;
            Right.Color = Color.Green;
            Right.Name = "Right";
            Right.XValueType = ChartValueType.DateTime;
            Right.MarkerStyle = MarkerStyle.None;
            Right.MarkerSize = 1;

            Series Left = new Series();
            Left.ChartType = SeriesChartType.Line;
            Left.Color = Color.Purple;
            Left.Name = "Left";
            Left.XValueType = ChartValueType.DateTime;
            Left.MarkerStyle = MarkerStyle.None;
            Left.MarkerSize = 1;

            //Series SB = new Series();
            //SB.ChartType = SeriesChartType.Line;
            //SB.Color = Color.DarkOrange;
            //SB.Name = "Southbound";
            //SB.XValueType = ChartValueType.DateTime;


            //Series EB = new Series();
            //EB.ChartType = SeriesChartType.Line;
            //EB.Color = Color.Blue;
            //EB.Name = "Eastbound";
            //EB.XValueType = ChartValueType.DateTime;
            //EB.BorderDashStyle = ChartDashStyle.Dot;
            //EB.BorderWidth = 2;

            //Series WB = new Series();
            //WB.ChartType = SeriesChartType.Line;
            //WB.Color = Color.Green;
            //WB.Name = "Westbound";
            //WB.XValueType = ChartValueType.DateTime;
            //WB.BorderDashStyle = ChartDashStyle.Dot;
            //WB.BorderWidth = 2;

            

            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;

            chart.Series.Add(TotalVolume);
            chart.Series.Add(Thru);
            chart.Series.Add(Right);
            chart.Series.Add(Left);
            chart.Series.Add(testSeries);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);



            AddDataToChart(graphStartDate, graphEndDate, Thrulanes, RTlanes, LTlanes, signalId, plans, isThruMovement, showLaneVolumes, showTotalVolumes, binSize);
        
        }

         private Color GetNextColor(int colorKey)
         {

             Color color = new Color();

             switch (colorKey)
             {
                 case 1:
                     color = Color.Green;
                     break;
                 case 2:
                     color = Color.Purple;
                     break;
                 case 3:
                     color = Color.Yellow;
                     break;
                 case 4:
                     color = Color.Orange;
                     break;

             }
                






             return color;
         }

        private void AddDataToChart(DateTime startDate, DateTime endDate, List<Detector> ThruDetectors, List<Detector> RTDetectors, List<Detector> LTDetectors,
            string signalId, MOE.Common.Business.PlanCollection plans, bool isThruMovement, 
            bool showLaneVolumes, bool showTotalVolumes, int binSize)
        {

            SortedDictionary<DateTime, int> BikeTotals = new SortedDictionary<DateTime, int>();
            //SortedDictionary<int, int> laneTotals = new SortedDictionary<int, int>();
            int totalVolume = 0;
            int laneCount = 0;



            if (ThruDetectors.Count > 0)
            {

                //SortedDictionary<DateTime, int> NBTotals = new SortedDictionary<DateTime, int>();
                foreach (MOE.Common.Business.Detector detector in ThruDetectors)
                {

                    laneCount++;
                    foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
                    {
                        if (showLaneVolumes)
                        {
                            chart.Series["Thru"].Points.AddXY(volume.XAxis, volume.YAxis);
                        }

                        //we need ot track the total number of bikes (volume) for this movement.
                        //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
                        //Then the movement total can be plotted on the graph
                        if (BikeTotals.ContainsKey(volume.XAxis))
                        {
                            BikeTotals[volume.XAxis] += volume.YAxis;
                        }
                        else
                        {
                            BikeTotals.Add(volume.XAxis, volume.YAxis);
                        }

                        //if (laneTotals.ContainsKey(volume.XAxis))
                        //{
                        //    laneTotals[volume.XAxis] += volume.YAxis;
                        //}
                        //else
                        //{
                        //    laneTotals.Add(volume.XAxis, volume.YAxis);
                        //}


                    }

                }
            }


            if (RTDetectors.Count > 0)
            {

                //SortedDictionary<DateTime, int> NBTotals = new SortedDictionary<DateTime, int>();
                foreach (MOE.Common.Business.Detector detector in RTDetectors)
                {

                    laneCount++;
                    foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
                    {
                        if (showLaneVolumes)
                        {
                            chart.Series["Right"].Points.AddXY(volume.XAxis, volume.YAxis);
                        }
                        //we need ot track the total number of bikes (volume) for this movement.
                        //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
                        //Then the movement total can be plotted on the graph
                        if (BikeTotals.ContainsKey(volume.XAxis))
                        {
                            BikeTotals[volume.XAxis] += volume.YAxis;
                        }
                        else
                        {
                            BikeTotals.Add(volume.XAxis, volume.YAxis);
                        }

                        //if (NBTotals.ContainsKey(volume.XAxis))
                        //{
                        //    NBTotals[volume.XAxis] += volume.YAxis;
                        //}
                        //else
                        //{
                        //    NBTotals.Add(volume.XAxis, volume.YAxis);
                        //}


                    }

                }
            }

            if (LTDetectors.Count > 0)
            {

                //SortedDictionary<DateTime, int> NBTotals = new SortedDictionary<DateTime, int>();
                foreach (MOE.Common.Business.Detector detector in LTDetectors)
                {

                    laneCount++;
                    foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
                    {
                        if (showLaneVolumes)
                        {
                            chart.Series["Left"].Points.AddXY(volume.XAxis, volume.YAxis);
                        }
                        //we need ot track the total number of bikes (volume) for this movement.
                        //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
                        //Then the movement total can be plotted on the graph
                        if (BikeTotals.ContainsKey(volume.XAxis))
                        {
                            BikeTotals[volume.XAxis] += volume.YAxis;
                        }
                        else
                        {
                            BikeTotals.Add(volume.XAxis, volume.YAxis);
                        }

                        //if (NBTotals.ContainsKey(volume.XAxis))
                        //{
                        //    NBTotals[volume.XAxis] += volume.YAxis;
                        //}
                        //else
                        //{
                        //    NBTotals.Add(volume.XAxis, volume.YAxis);
                        //}


                    }

                }
            }
                

                if (showLaneVolumes)
                {
                    //foreach (KeyValuePair<DateTime, int> volume in NBTotals)
                    //{
                    
                    //if (chart.ChartAreas[0].AxisY.Maximum < (volume.Value + (volume.Value * .1)))
                    //{
                    //    chart.ChartAreas[0].AxisY.Maximum = (volume.Value + (volume.Value * .1));
                    //}
                    
                    //}
                }


            
                    
                

                

                int binMultiplier = 60 / binSize;

                //get the total volume for the approach
                foreach (KeyValuePair<DateTime, int> totals in BikeTotals)
                {
                    if (showTotalVolumes)
                    {
                        chart.Series["Total Volume"].Points.AddXY(totals.Key, totals.Value);
                    }
                    totalVolume += (totals.Value);
                }


                int highLaneVolume = BikeTotals.Values.Max();
                
                
                KeyValuePair<DateTime, int> peakHourValue = findPeakHour(BikeTotals, binMultiplier);
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



                string PHFPlaceholder = "";
                for (int i = 0; i < binMultiplier; i++)
                {
                    if (BikeTotals.ContainsKey(peakHour.AddMinutes(i * binSize)))
                    {
                    if (PeakHourMAXVolume < (BikeTotals[peakHour.AddMinutes(i * binSize)]))
                    {
                    PeakHourMAXVolume = BikeTotals[peakHour.AddMinutes(i * binSize)];
                    }
                    }
                }

                if (PeakHourMAXVolume > 0)
                {
                double PHF = SetSigFigs( Convert.ToDouble(PHV) / (Convert.ToDouble(PeakHourMAXVolume) * Convert.ToDouble(binMultiplier)), 2);
                    PHFPlaceholder = PHF.ToString();
                }
                else
                {
                    PHFPlaceholder = "Not Avialable";
                }

                string peakHourString = peakHour.ToShortTimeString() + " - " + peakHour.AddHours(1).ToShortTimeString();

                string titleString = " TV: " + (totalVolume/binMultiplier).ToString() + " PH: " + peakHourString + " PHV: " + (PHV / binMultiplier).ToString() + " VPH \n " +
                    " PHF: " + PHFPlaceholder + "        fLU: " + fluPlaceholder;
                chart.Titles.Add(titleString);

                foreach (Series series in chart.Series)
                {
                    if (series.Points.Count < 1)
                    {
                        series.IsVisibleInLegend = false;
                    }
                    else
                    {
                        series.IsVisibleInLegend = true;
                    }
                }
                chart.Series["Posts"].IsVisibleInLegend = false;

                if (laneCount == 1)
                {
                    chart.Series["Total Volume"].Enabled = false;
                }

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

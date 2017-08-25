using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;


namespace MOE.Common.Business.Inrix
{   

    public class InrixCumulativeFreqChart
    {
        public Chart chart = new Chart();
        public DataTable InrixReliabilityDT = new DataTable();

        public InrixCumulativeFreqChart(List<MOE.Common.Business.Inrix.GraphLine> lines, MOE.Common.Business.Inrix.TMCCollection tmcCollection, int binsize, double maxX, double minX)
        {

             CreateReliabilityTable();
            

            string extendedDirection = string.Empty;

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 450;
            chart.Width = 750;

            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineColor = Color.Black;
            chart.BorderlineWidth = 2;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            //Set the chart title
            chart.Titles.Add("Cumulative Frequency Chart");
            chart.Titles.Add("");

            chart.Titles[1].Font = new Font(chart.Titles[0].Font, FontStyle.Bold);

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";


            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);



            chartArea.AxisX.Title = "Travel Time (Minutes)";
            chartArea.AxisX.Interval = 2;
            if (maxX > 0)
            {
                chartArea.AxisX.Maximum = maxX;
            } 
            chartArea.AxisX.Minimum = 0;
            if (minX > 0)
            {
                chartArea.AxisX.Minimum = minX;
            }
            //   chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;



            chartArea.AxisY.Title = "Cumulative Frequency";
            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.Interval = 25;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.Maximum = 100;
            chartArea.AxisY.Minimum = 0;

           
            chart.ChartAreas.Add(chartArea);

            int i = 0;
            foreach (MOE.Common.Business.Inrix.GraphLine line in lines)
            {
                i++;
                Series series = new Series();
                series.IsVisibleInLegend = true;
                series.ChartType = SeriesChartType.Spline;
                series.Color = line.LineColor;
                series.BorderWidth = line.LineWidth;
                //series.MarkerSize = line.LineWidth;
                series.BorderDashStyle = line.LineStyle;
                series.Name = @"#" + line.LineNumber + ": " + line.StartDay.ToShortDateString() + " - " + line.EndDay.ToShortDateString() + " from " +
                    line.StartHour + " to " + line.EndHour;
                series.XValueType = ChartValueType.Int32;

               bool hasSeries = false;

               foreach (Series s in chart.Series)
               {
                   if (s.Name == series.Name)
                   {
                       hasSeries = true;
                   }
               }

               if (!hasSeries)
               {
                   chart.Series.Add(series);
               }
                

                AddDataToCFChart(line, tmcCollection, binsize, series.Name, line.LineNumber);
            }



        
        }



        private void AddDataToCFChart(MOE.Common.Business.Inrix.GraphLine line, MOE.Common.Business.Inrix.TMCCollection tmccollection, int binsize, string seriesname, int rangeID)
         {
             //DataPointComparer comparer = new DataPointComparer();
             //TextWriter tw = new StreamWriter(@"c:\array.txt");

             tmccollection.RankedTravelTimes.Clear();
             tmccollection.BadTMCs.Clear();
            
             tmccollection.CalculateTravelTimes(line.StartDay, line.EndDay, line.StartHour, line.EndHour, line.Confidence, line.DayTypes, binsize, ref InrixReliabilityDT, rangeID);
             
             
             if (tmccollection.BadTMCs.Count > 0 & chart.Titles.Count < 3)
             {
                 Title badTMCtitle = new Title();
                 badTMCtitle.Text = "One or more links are missing data.";
                 badTMCtitle.ForeColor = Color.Red;
                 badTMCtitle.IsDockedInsideChartArea = true;
                 badTMCtitle.Docking = Docking.Bottom;
                 chart.Titles.Add(badTMCtitle);
                

                 //chart.Titles.Add("One or more TMC have no data.");
                 //chart.Titles[2].ForeColor = Color.Red;
                 //chart.Titles[2].IsDockedInsideChartArea = true;
                 //chart.Titles[2].Docking = Docking.Bottom;
                 


             }

     

            foreach(KeyValuePair<double, double> pair in tmccollection.RankedTravelTimes)
             {

                 chart.Series[seriesname].Points.AddXY(Math.Round(pair.Value,2), pair.Key);

                 //if (pair.Value >= x)
                 //{
                 //    x = pair.Value;
                 //}
                 //else
                 //{
                 //    int w = 1;
                 //}

                 //if (pair.Key >= y)
                 //{
                 //    y = pair.Key;
                 //}
                 //else
                 //{
                 //    int w = 1;
                 //}

             }
            //foreach (DataPoint point in chart.Series[seriesname].Points)
            //{
            //    if (point.YValues.Count() > 1)
            //    {
            //        foreach (double d in point.YValues)
            //        {
            //            tw.WriteLine(point.XValue.ToString() + "," + d);
            //            tw.Flush();
            //        }
            //    }
            //}
             
         }

        private void CreateReliabilityTable()
        {
            DataColumn tmcCode = new DataColumn();
            DataColumn tmcName = new DataColumn();
            DataColumn tmcLength = new DataColumn();
            DataColumn timeRangeID = new DataColumn();
            DataColumn timeRange = new DataColumn();
            DataColumn avgTT = new DataColumn();
            DataColumn stdDev = new DataColumn();
            DataColumn percGoodBins = new DataColumn();
            DataColumn avgConfidence = new DataColumn();

            tmcCode.ColumnName = "TMC Code";
            tmcName.ColumnName = "TMC Name";
            tmcLength.ColumnName = "TMC Length";
            timeRangeID.ColumnName = "Range ID";
            timeRange.ColumnName = "Time Range";
            avgTT.ColumnName = "Avg. Travel Time";
            stdDev.ColumnName = "Std. Dev.";
            percGoodBins.ColumnName = "% Good Bins";
            avgConfidence.ColumnName = "Avg. Confidence Score";


            InrixReliabilityDT.Columns.Add(tmcCode);
            InrixReliabilityDT.Columns.Add(tmcName);
            InrixReliabilityDT.Columns.Add(timeRangeID);
            InrixReliabilityDT.Columns.Add(timeRange);
            InrixReliabilityDT.Columns.Add(tmcLength);
            InrixReliabilityDT.Columns.Add(avgTT);
            InrixReliabilityDT.Columns.Add(stdDev);
            InrixReliabilityDT.Columns.Add(percGoodBins);
            InrixReliabilityDT.Columns.Add(avgConfidence);


            


            

        }


        //I don't htink anything uses this function just yet.
        public void AddRowsToTable(string tmc_code, string range, double avg_tt, double std_dev, double perc_good_bins, double avg_confidence)
        {


            InrixReliabilityDT.Rows.Add(new Object[] { tmc_code, 
                                                       range, 
                                                       avg_tt.ToString("N0"), 
                                                       std_dev.ToString("N0"), 
                                                       perc_good_bins.ToString("N0"),
                                                       avg_confidence.ToString("N0"),
                                                       });

        }
    }
}

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PCDService" in both code and config file together.
    public class PCDService : IPCDService
    {
        public List<string> CreateChart(DateTime startDate, DateTime endDate, string signalId, bool showVolume,
            int volumeBinSize, //int region, 
            string location, double y1AxisMaximum,
            double y2AxisMaximum, int dotSize, bool showArrivalOnGreen)
        {
            return new List<string>();
        //    List<string> existsList = new List<string>();
        //    List<string> returnList = new List<string>();
        //    string chartLocation = ConfigurationManager.AppSettings["ImageLocation"];

        //    //if (endDate <= DateTime.Now)
        //    //{
        //    //    MOE.Common.Data.MOETableAdapters.DistinctSignalPhaseTableAdapter adapter =
        //    //        new MOE.Common.Data.MOETableAdapters.DistinctSignalPhaseTableAdapter();

        //    //    MOE.Common.Data.MOE.DistinctSignalPhaseDataTable table =
        //    //        adapter.GetData(signalId.ToString());

        //    //    foreach (MOE.Common.Data.MOE.DistinctSignalPhaseRow row in table.Rows)
        //    //    {
        //    //        string chartName = "PCD-" +
        //    //                    signalId +
        //    //                    "-" +
        //    //                    row.Phase +
        //    //                    "-" +
        //    //                    startDate.Year.ToString() +
        //    //                    startDate.ToString("MM") +
        //    //                    startDate.ToString("dd") +
        //    //                    startDate.ToString("HH") +
        //    //                    startDate.ToString("mm") +
        //    //                    "-" +
        //    //                    endDate.Year.ToString() +
        //    //                    endDate.ToString("MM") +
        //    //                    endDate.ToString("dd") +
        //    //                    endDate.ToString("HH") +
        //    //                    endDate.ToString("mm-") +
        //    //                    volumeBinSize.ToString() +
        //    //                    "-" +
        //    //                    y1AxisMaximum.ToString() +
        //    //                    "-" +
        //    //                    y2AxisMaximum.ToString() +
        //    //                    "-" +
        //    //                    dotSize.ToString();
        //    //        if (showVolume)
        //    //        {
        //    //            chartName += "-V";
        //    //        }
        //    //        if (showArrivalOnGreen)
        //    //        {
        //    //            chartName += "-G";
        //    //        }
        //    //        chartName += ".jpg";

        //    //        if (File.Exists(chartLocation + chartName))
        //    //        {
        //    //            returnList.Add(chartName);
        //    //        }
        //    //    }
        //    //}

        //    if (returnList.Count == 0)
        //    {
        //        Chart chart = new Chart();
        //        MOE.Common.Business.SignalPhaseCollection signalphasecollection =
        //                  new MOE.Common.Business.SignalPhaseCollection(startDate,
        //                      endDate, signalId, //region, 
        //                    showVolume, volumeBinSize, "HAS_PCD");



        //        //If there are phases in the database add the charts
        //        if (signalphasecollection.SignalPhaseList.Count > 0)
        //        {
        //            foreach (MOE.Common.Business.SignalPhase signalPhase in signalphasecollection.SignalPhaseList)
        //            {
        //                if (signalPhase.Plans.PlanList.Count > 0)
        //                {

        //                    //Display the PDC chart
        //                    chart = GetNewChart(startDate, endDate, signalId, signalPhase.Phase, signalPhase.Direction,
        //                         location, signalPhase.IsOverlap, y1AxisMaximum, y2AxisMaximum, showVolume, dotSize);

        //                    AddDataToChart(chart, signalPhase, startDate, endDate, signalId, showVolume, showArrivalOnGreen);

        //                    //Create the File Name
        //                    string chartName = "PCD-" +
        //                        signalId +
        //                        "-" +
        //                        signalPhase.Phase +
        //                        "-" +
        //                        startDate.Year.ToString() +
        //                        startDate.ToString("MM") +
        //                        startDate.ToString("dd") +
        //                        startDate.ToString("HH") +
        //                        startDate.ToString("mm") +
        //                        "-" +
        //                        endDate.Year.ToString() +
        //                        endDate.ToString("MM") +
        //                        endDate.ToString("dd") +
        //                        endDate.ToString("HH") +
        //                        endDate.ToString("mm-") +
        //                        volumeBinSize.ToString() +
        //                        "-" +
        //                        y1AxisMaximum.ToString() +
        //                        "-" +
        //                        y2AxisMaximum.ToString() +
        //                        "-" +
        //                        dotSize.ToString();
        //                    if (showVolume)
        //                    {
        //                        chartName += "-V";
        //                    }
        //                    if (showArrivalOnGreen)
        //                    {
        //                        chartName += "-G";
        //                    }
        //                    Random r = new Random();
        //                    chartName += r.Next().ToString();
        //                    chartName += ".jpg";



        //                    //Save an image of the chart
        //                    chart.SaveImage(chartLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
        //                    returnList.Add(chartName);

        //                }
        //            }

        //        }
        //    }
           

        //    return returnList;
        //}

        ///// <summary>
        ///// Gets a new chart for the pcd Diagram
        ///// </summary>
        ///// <param name="graphStartDate"></param>
        ///// <param name="graphEndDate"></param>
        ///// <param name="signalId"></param>
        ///// <param name="phase"></param>
        ///// <param name="direction"></param>
        ///// <param name="location"></param>
        ///// <returns></returns>
        //private Chart GetNewChart(DateTime graphStartDate, DateTime graphEndDate, string signalId,
        //    int phase, string direction, string location, bool isOverlap, double y1AxisMaximum,
        //    double y2AxisMaximum, bool showVolume, int dotSize)
        //{
        //    Chart chart = new Chart();
        //    string extendedDirection = string.Empty;
        //    string movementType = "Phase";
        //    if (isOverlap)
        //    {
        //        movementType = "Overlap";
        //    }


        //    //Gets direction for the title
        //    switch (direction)
        //    {
        //        case "SB":
        //            extendedDirection = "Southbound";
        //            break;
        //        case "NB":
        //            extendedDirection = "Northbound";
        //            break;
        //        default:
        //            extendedDirection = direction;
        //            break;
        //    }

        //    //Set the chart properties
        //    chart.ImageType = ChartImageType.Jpeg;
        //    chart.Height = 450;
        //    chart.Width = 1100;
        //    chart.ImageStorageMode = ImageStorageMode.UseImageLocation;

        //    //Set the chart title
        //    chart.Titles.Add(location + "Signal " + signalId.ToString() + " "
        //        + movementType + ": " + phase.ToString() +
        //        " " + extendedDirection + "\n" + graphStartDate.ToString("f") +
        //        " - " + graphEndDate.ToString("f"));

        //    //Create the chart legend
        //    Legend chartLegend = new Legend();
        //    chartLegend.Name = "MainLegend";
        //    //chartLegend.LegendStyle = LegendStyle.Table;
        //    chartLegend.Docking = Docking.Left;
        //    chartLegend.CustomItems.Add(Color.Blue, "AoG - Arrival On Green");
        //    chartLegend.CustomItems.Add(Color.Blue, "GT - Green Time");
        //    chartLegend.CustomItems.Add(Color.Maroon, "PR - Platoon Ratio");
        //    //LegendCellColumn a = new LegendCellColumn();
        //    //a.ColumnType = LegendCellColumnType.Text;
        //    //a.Text = "test";
        //    //chartLegend.CellColumns.Add(a);
        //    chart.Legends.Add(chartLegend);


        //    //Create the chart area
        //    ChartArea chartArea = new ChartArea();
        //    chartArea.Name = "ChartArea1";
        //    chartArea.AxisY.Maximum = y1AxisMaximum;           
        //    chartArea.AxisY.Minimum = 0;
        //    chartArea.AxisY.Title = "Cycle Time (Seconds) ";

        //    if (showVolume)
        //    {
        //        chartArea.AxisY2.Enabled = AxisEnabled.True;
        //        chartArea.AxisY2.MajorTickMark.Enabled = true;
        //        chartArea.AxisY2.MajorGrid.Enabled = false;
        //        chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
        //        chartArea.AxisY2.Interval = 500;
        //        chartArea.AxisY2.Maximum = y2AxisMaximum;
        //        chartArea.AxisY2.Title = "Volume Per Hour ";
        //    }

        //    chartArea.AxisX.Title = "Time (Hour of Day)";
        //    chartArea.AxisX.Interval = 1;
        //    chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
        //    chartArea.AxisX.LabelStyle.Format = "HH";
        //    //chartArea.AxisX.Minimum = 0;

        //    chartArea.AxisX2.Enabled = AxisEnabled.True;
        //    chartArea.AxisX2.MajorTickMark.Enabled = true;
        //    chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
        //    chartArea.AxisX2.LabelStyle.Format = "HH";
        //    chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
        //    chartArea.AxisX2.Interval = 1;
        //    //chartArea.AxisX.Minimum = 0;

        //    chart.ChartAreas.Add(chartArea);

        //    //Add the point series
        //    Series pointSeries = new Series();
        //    pointSeries.ChartType = SeriesChartType.Point;
        //    pointSeries.Color = Color.Black;
        //    pointSeries.Name = "Detector Activation";
        //    pointSeries.XValueType = ChartValueType.DateTime;
        //    pointSeries.MarkerSize = dotSize;
        //    chart.Series.Add(pointSeries);

        //    //Add the green series
        //    Series greenSeries = new Series();
        //    greenSeries.ChartType = SeriesChartType.Line;
        //    greenSeries.Color = Color.DarkGreen;
        //    greenSeries.Name = "Change to Green";
        //    greenSeries.XValueType = ChartValueType.DateTime;
        //    greenSeries.BorderWidth = 1;
        //    chart.Series.Add(greenSeries);

        //    //Add the yellow series
        //    Series yellowSeries = new Series();
        //    yellowSeries.ChartType = SeriesChartType.Line;
        //    yellowSeries.Color = Color.Yellow;
        //    yellowSeries.Name = "Change to Yellow";
        //    yellowSeries.XValueType = ChartValueType.DateTime;
        //    chart.Series.Add(yellowSeries);

        //    //Add the red series
        //    Series redSeries = new Series();
        //    redSeries.ChartType = SeriesChartType.Line;
        //    redSeries.Color = Color.Red;
        //    redSeries.Name = "Change to Red";
        //    redSeries.XValueType = ChartValueType.DateTime;
        //    chart.Series.Add(redSeries);

        //    //Add the red series
        //    Series volumeSeries = new Series();
        //    volumeSeries.ChartType = SeriesChartType.Line;
        //    volumeSeries.Color = Color.Black;
        //    volumeSeries.Name = "Volume Per Hour";
        //    volumeSeries.XValueType = ChartValueType.DateTime;
        //    volumeSeries.YAxisType = AxisType.Secondary;
        //    chart.Series.Add(volumeSeries);



        //    //Add points at the start and and of the x axis to ensure
        //    //the graph covers the entire period selected by the user
        //    //whether there is data or not
        //    chart.Series["Detector Activation"].Points.AddXY(graphStartDate, 0);
        //    chart.Series["Detector Activation"].Points.AddXY(graphEndDate, 0);
        //    return chart;
        //}

        ///// <summary>
        ///// Adds data points to a graph with the series GreenLine, YellowLine, Redline
        ///// and Points already added.
        ///// </summary>
        ///// <param name="chart"></param>
        ///// <param name="signalPhase"></param>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <param name="signalId"></param>
        //private void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase, DateTime startDate,
        //    DateTime endDate, string signalId, bool showVolume, bool showArrivalOnGreen)
        //{
        //    decimal totalDetectorHits = 0;
        //    decimal totalOnGreenArrivals = 0;
        //    decimal percentArrivalOnGreen = 0;

        //    foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
        //    {
        //        if (plan.PCDcollection.Count > 0)
        //        {
        //            foreach (MOE.Common.Business.Cycle pcd in plan.PCDcollection)
        //            {
        //                chart.Series["Change to Green"].Points.AddXY(
        //                    //pcd.StartTime,
        //                    pcd.GreenEvent,
        //                    pcd.GreenLineY);
        //                chart.Series["Change to Yellow"].Points.AddXY(
        //                    //pcd.StartTime,
        //                    pcd.YellowEvent,
        //                    pcd.YellowLineY);
        //                chart.Series["Change to Red"].Points.AddXY(
        //                    //pcd.StartTime, 
        //                    pcd.EndTime,
        //                    pcd.RedLineY);
        //                totalDetectorHits += pcd.DetectorCollection.Count;
        //                foreach (MOE.Common.Business.DetectorDataPoint detectorPoint in pcd.DetectorCollection)
        //                {
        //                    chart.Series["Detector Activation"].Points.AddXY(
        //                        //pcd.StartTime, 
        //                        detectorPoint.TimeStamp,
        //                        detectorPoint.YPoint);
        //                    if (detectorPoint.YPoint > pcd.GreenLineY && detectorPoint.YPoint < pcd.RedLineY)
        //                    {
        //                        totalOnGreenArrivals++;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (showVolume)
        //    {
        //        foreach (MOE.Common.Business.Volume v in signalPhase.Volume.Items)
        //        {
        //            chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
        //        }
        //    }

        //    //if arrivals on green is selected add the data to the chart
        //    if (showArrivalOnGreen)
        //    {
        //        if (totalDetectorHits > 0)
        //        {
        //            percentArrivalOnGreen = (totalOnGreenArrivals / totalDetectorHits) * 100;
        //        }
        //        else
        //        {
        //            percentArrivalOnGreen = 0;
        //        }

        //        chart.Titles.Add(Math.Round(percentArrivalOnGreen).ToString() +
        //            "% AoG");


        //        SetPlanStrips(signalPhase.Plans.PlanList, chart, startDate);
        //    }

        //    //Add Comment to chart
        //    MOE.Common.Models.Repositories.IDetectorCommentRepository dcr = MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.CreateDetectorCommentRepository();


        //    MOE.Common.Models.Detector_Comment comment = dcr.GetCommentByDetector(signalId);
        //    if (comment != null)
        //    {

        //        chart.Titles.Add(comment.Comment);
        //        chart.Titles[1].Docking = Docking.Bottom;
        //        chart.Titles[1].ForeColor = Color.Red;
        //    }

            
        //}


        ///// <summary>
        ///// Adds plan strips to the chart
        ///// </summary>
        ///// <param name="planCollection"></param>
        ///// <param name="chart"></param>
        ///// <param name="graphStartDate"></param>
        //protected void SetPlanStrips(List<MOE.Common.Business.Plan> planCollection, Chart chart, DateTime graphStartDate)
        //{
        //    int backGroundColor = 1;
        //    foreach (MOE.Common.Business.Plan plan in planCollection)
        //    {
        //        StripLine stripline = new StripLine();
        //        //Creates alternating backcolor to distinguish the plans
        //        if (backGroundColor % 2 == 0)
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightGray);
        //        }
        //        else
        //        {
        //            stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
        //        }

        //        //Set the stripline properties
        //        stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
        //        stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
        //        stripline.Interval = 1;
        //        stripline.IntervalType = DateTimeIntervalType.Days;               
        //        stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
        //        stripline.StripWidthType = DateTimeIntervalType.Hours;
        //        
        //        chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

        //        //Add a corrisponding custom label for each strip
        //        CustomLabel Plannumberlabel = new CustomLabel();
        //        Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
        //        Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
        //        switch (plan.PlanNumber)
        //        {
        //            case 254:
        //                Plannumberlabel.Text = "Free";
        //                break;
        //            case 255:
        //                Plannumberlabel.Text = "Flash";
        //                break;
        //            case 0:
        //                Plannumberlabel.Text = "Unknown";
        //                break;
        //            default:
        //                Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

        //                break;
        //        }

        //        Plannumberlabel.ForeColor = Color.Black;
        //        Plannumberlabel.RowIndex = 3;

        //        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

        //        CustomLabel aogLabel = new CustomLabel();
        //        aogLabel.FromPosition = plan.StartTime.ToOADate();
        //        aogLabel.ToPosition = plan.EndTime.ToOADate();
        //        aogLabel.Text = plan.PercentArrivalOnGreen.ToString() + "% AoG\n" +
        //            plan.PercentGreen.ToString() + "% GT";

        //        aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
        //        aogLabel.ForeColor = Color.Blue;
        //        aogLabel.RowIndex = 2;
        //        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

        //        CustomLabel statisticlabel = new CustomLabel();
        //        statisticlabel.FromPosition = plan.StartTime.ToOADate();
        //        statisticlabel.ToPosition = plan.EndTime.ToOADate();
        //        statisticlabel.Text =
        //            plan.PlatoonRatio.ToString() + " PR";
        //        statisticlabel.ForeColor = Color.Maroon;
        //        statisticlabel.RowIndex = 1;
        //        chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);

        //        //CustomLabel PlatoonRatiolabel = new CustomLabel();
        //        //PercentGreenlabel.FromPosition = plan.StartTime.ToOADate();
        //        //PercentGreenlabel.ToPosition = plan.EndTime.ToOADate();
        //        //PercentGreenlabel.Text = plan.PlatoonRatio.ToString() + " PR";
        //        //PercentGreenlabel.ForeColor = Color.Black;
        //        //PercentGreenlabel.RowIndex = 1;
        //        //chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(PercentGreenlabel);

        //        //Change the background color counter for alternating color
        //        backGroundColor++;

        //    }
        }
    }
}

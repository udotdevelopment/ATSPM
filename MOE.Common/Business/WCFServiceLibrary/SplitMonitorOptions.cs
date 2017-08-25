using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class SplitMonitorOptions : MetricOptions
    {
        [DataMember]
        [Display(Name = "Percentile Split")]
        public int? SelectedPercentileSplit { get; set; }

        [DataMember]
        [Display(Name = "Show Plans")]
        public bool ShowPlanStripes { get; set; }

        [DataMember]
        [Display(Name = "Show Ped Activity")]
        public bool ShowPedActivity { get; set; }

        [DataMember]
        [Display(Name = "Show Average Split")]
        public bool ShowAverageSplit { get; set; }

        [DataMember]
        [Display(Name = "Show % Max Out/ForceOff")]
        public bool ShowPercentMaxOutForceOff { get; set; }

        [DataMember]
        [Display(Name = "Show Percent GapOuts")]
        public bool ShowPercentGapOuts { get; set; }

        [DataMember]
        [Display(Name = "Show Percent Skip")]
        public bool ShowPercentSkip { get; set; }

        public List<SelectListItem> PercentSplitsSelectList { get; set; }

        [Display(Name = "Adjust Y Axis")]
        public bool AdjustYAxis { get; set; }

        public SplitMonitorOptions(string signalID, double? yAxisMax, int metricTypeID,
            DateTime startDate, DateTime endDate,
            int percentileSplit, bool showPlanStripes, bool showPedActivity,
            bool showAverageSplit, bool showPercentMaxOutForceOff, bool showPercentGapOuts, bool showPercentSkip)
        {
            SignalID = signalID;
            YAxisMax = yAxisMax;
            MetricTypeID = metricTypeID;
            StartDate = startDate;
            EndDate = endDate;
            SelectedPercentileSplit = percentileSplit;
            ShowPlanStripes = showPlanStripes;
            ShowPedActivity = showPedActivity;
            ShowAverageSplit = showAverageSplit;
            ShowPercentMaxOutForceOff = showPercentMaxOutForceOff;
            ShowPercentGapOuts = showPercentGapOuts;
            ShowPercentSkip = showPercentSkip;
            MetricType.ChartName = "SplitMonitor";
        }

        public SplitMonitorOptions()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            SetPercentSplitsList();
            ShowPlanStripes = true;
            ShowPedActivity = true;
            ShowAverageSplit = true;
            ShowPercentMaxOutForceOff = true;
            ShowPercentGapOuts = true;
            ShowPercentSkip = true;
        }

        private void SetPercentSplitsList()
        {
            PercentSplitsSelectList = new List<SelectListItem>();
            PercentSplitsSelectList.Add(new SelectListItem { Value = "", Text = "No Percentile Split" });
            PercentSplitsSelectList.Add(new SelectListItem { Value = "50", Text = "50" });
            PercentSplitsSelectList.Add(new SelectListItem { Value = "75", Text = "75" });
            PercentSplitsSelectList.Add(new SelectListItem { Value = "85", Text = "85", Selected = true });
            PercentSplitsSelectList.Add(new SelectListItem { Value = "90", Text = "90" });
            PercentSplitsSelectList.Add(new SelectListItem { Value = "95", Text = "95" });
        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            MOE.Common.Business.AnalysisPhaseCollection analysisPhaseCollection =
                       new MOE.Common.Business.AnalysisPhaseCollection(SignalID, StartDate,
                           EndDate);
            //If there are phases in the collection add the charts
            if (analysisPhaseCollection.Items.Count > 0)
            {
                foreach (MOE.Common.Business.Plan plan in analysisPhaseCollection.Plans.PlanList)
                {
                    plan.SetProgrammedSplits(SignalID);
                    plan.SetHighCycleCount(analysisPhaseCollection);
                }

                //If there are phases in the collection add the charts


                //dummy chart to create a legend for the entire split monitor page.
                Chart dummychart = new Chart();
                ChartArea chartarea1 = new ChartArea();
                dummychart.ImageType = ChartImageType.Jpeg;
                dummychart.Height = 200;
                dummychart.Width = 800;

                //add comments to the chart
                dummychart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                dummychart.BorderlineDashStyle = ChartDashStyle.Dot;

                Series PedActivity = new Series();
                Series GapoutSeries = new Series();
                Series MaxOutSeries = new Series();
                Series ForceOffSeries = new Series();
                Series ProgramedSplit = new Series();
                Series UnknownSeries = new Series();

                PedActivity.Name = "Ped Activity";
                GapoutSeries.Name = "Gap Out";
                MaxOutSeries.Name = "Max Out";
                ForceOffSeries.Name = "Force Off";
                ProgramedSplit.Name = "Programmed Split";
                UnknownSeries.Name = "Unknown Termination Cause";


                PedActivity.MarkerStyle = MarkerStyle.Cross;
                GapoutSeries.MarkerStyle = MarkerStyle.Circle;
                MaxOutSeries.MarkerStyle = MarkerStyle.Circle;
                ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
                ProgramedSplit.BorderDashStyle = ChartDashStyle.Solid;
                UnknownSeries.MarkerStyle = MarkerStyle.Circle;

                GapoutSeries.Color = Color.OliveDrab;
                PedActivity.Color = Color.DarkGoldenrod;
                MaxOutSeries.Color = Color.Red;
                ForceOffSeries.Color = Color.MediumBlue;
                ProgramedSplit.Color = Color.OrangeRed;
                UnknownSeries.Color = Color.Black;

                dummychart.Series.Add(ProgramedSplit);
                dummychart.Series.Add(GapoutSeries);
                dummychart.Series.Add(MaxOutSeries);
                dummychart.Series.Add(ForceOffSeries);
                dummychart.Series.Add(UnknownSeries);
                dummychart.Series.Add(PedActivity);

                dummychart.ChartAreas.Add(chartarea1);

                Legend dummychartLegend = new Legend();
                dummychartLegend.Name = "DummyLegend";

                dummychartLegend.IsDockedInsideChartArea = true;

                dummychartLegend.Title = "Chart Legend";
                dummychartLegend.Docking = Docking.Bottom;
                dummychartLegend.Alignment = StringAlignment.Center;
                dummychart.Legends.Add(dummychartLegend);
                List<Title> removethese = new List<Title>();

                foreach (Title t in dummychart.Titles)
                {
                    if (t.Text == "" || t.Text == null)
                    {
                        removethese.Add(t);
                    }
                }
                foreach (Title t in removethese)
                {
                    dummychart.Titles.Remove(t);
                }


                string dummyChartFileName = CreateFileName();
                dummychart.SaveImage(MetricFileLocation + dummyChartFileName);
                ReturnList.Add(MetricWebPath + dummyChartFileName);

                if (analysisPhaseCollection.Items.Count > 0)
                {
                    var phasesInOrder = (from r in analysisPhaseCollection.Items
                                         select r).OrderBy(r => r.PhaseNumber);
                    foreach (MOE.Common.Business.AnalysisPhase Phase in phasesInOrder)
                    {
                        Chart chart = GetNewSplitMonitorChart(StartDate, EndDate, SignalID, GetSignalLocation(), Phase.PhaseNumber);
                        AddSplitMonitorDataToChart(chart, StartDate, EndDate, Phase, SignalID, analysisPhaseCollection.Plans);
                        if (ShowPlanStripes)
                        {
                            SetSimplePlanStrips(analysisPhaseCollection.Plans, chart, StartDate);
                            SetSplitMonitorStatistics(analysisPhaseCollection.Plans, Phase, chart);
                        }
                        string chartFileName = CreateFileName();
                        removethese = new List<Title>();

                        foreach (Title t in chart.Titles)
                        {
                            if (t.Text == "" || t.Text == null)
                            {
                                removethese.Add(t);
                            }
                        }
                        foreach (Title t in removethese)
                        {
                            chart.Titles.Remove(t);
                        }
                        chart.SaveImage(MetricFileLocation + chartFileName);
                        ReturnList.Add(MetricWebPath + chartFileName);
                    }
                }
            }
            return ReturnList;
        }

        private void SetSplitMonitorStatistics(MOE.Common.Business.PlanCollection plans, MOE.Common.Business.AnalysisPhase phase, Chart chart)
        {

            //find the phase Cycles that occure during the plan.


            foreach (MOE.Common.Business.Plan plan in plans.PlanList)
            {
                var Cycles = from cycle in phase.Cycles.Items
                             where cycle.StartTime > plan.StartTime && cycle.EndTime < plan.EndTime
                             orderby cycle.Duration
                             select cycle;

                // find % Skips
                if (ShowPercentSkip)
                {
                    if (plan.CycleCount > 0)
                    {
                        double CycleCount = plan.CycleCount;
                        double SkippedPhases = (plan.CycleCount - Cycles.Count());
                        double SkipPercent = 0;
                        if (CycleCount > 0)
                        {
                            SkipPercent = SkippedPhases / CycleCount;
                        }


                        CustomLabel skipLabel = new CustomLabel();
                        skipLabel.FromPosition = plan.StartTime.ToOADate();
                        skipLabel.ToPosition = plan.EndTime.ToOADate();
                        skipLabel.Text = string.Format("{0:0.0%} Skips", SkipPercent);
                        skipLabel.LabelMark = LabelMarkStyle.LineSideMark;
                        skipLabel.ForeColor = Color.Black;
                        skipLabel.RowIndex = 1;
                        chart.ChartAreas[0].AxisX2.CustomLabels.Add(skipLabel);
                    }
                }

                // find % GapOuts
                if (ShowPercentGapOuts)
                {
                    var GapOuts = from cycle in Cycles
                                  where cycle.TerminationEvent == 4
                                  select cycle;

                    double CycleCount = plan.CycleCount;
                    double gapouts = GapOuts.Count();
                    double GapPercent = 0;
                    if (CycleCount > 0)
                    {
                        GapPercent = gapouts / CycleCount;
                    }


                    CustomLabel gapLabel = new CustomLabel();
                    gapLabel.FromPosition = plan.StartTime.ToOADate();
                    gapLabel.ToPosition = plan.EndTime.ToOADate();
                    gapLabel.Text = string.Format("{0:0.0%} GapOuts", GapPercent);
                    gapLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    gapLabel.ForeColor = Color.OliveDrab;
                    gapLabel.RowIndex = 2;
                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(gapLabel);

                }

                //Set Max Out
                if (ShowPercentMaxOutForceOff && plan.PlanNumber == 254)
                {
                    var MaxOuts = from cycle in Cycles
                                  where cycle.TerminationEvent == 5
                                  select cycle;

                    double CycleCount = plan.CycleCount;
                    double maxouts = MaxOuts.Count();
                    double MaxPercent = 0;
                    if (CycleCount > 0)
                    {
                        MaxPercent = maxouts / CycleCount;
                    }


                    CustomLabel maxLabel = new CustomLabel();
                    maxLabel.FromPosition = plan.StartTime.ToOADate();
                    maxLabel.ToPosition = plan.EndTime.ToOADate();
                    maxLabel.Text = string.Format("{0:0.0%} MaxOuts", MaxPercent);
                    maxLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    maxLabel.ForeColor = Color.Red;
                    maxLabel.RowIndex = 3;
                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(maxLabel);

                }

                // Set Force Off
                if (ShowPercentMaxOutForceOff && plan.PlanNumber != 254
                    )
                {
                    var ForceOffs = from cycle in Cycles
                                    where cycle.TerminationEvent == 6
                                    select cycle;

                    double CycleCount = plan.CycleCount;
                    double forceoffs = ForceOffs.Count();
                    double ForcePercent = 0;
                    if (CycleCount > 0)
                    {
                        ForcePercent = forceoffs / CycleCount;
                    }


                    CustomLabel forceLabel = new CustomLabel();
                    forceLabel.FromPosition = plan.StartTime.ToOADate();
                    forceLabel.ToPosition = plan.EndTime.ToOADate();
                    forceLabel.Text = string.Format("{0:0.0%} ForceOffs", ForcePercent);
                    forceLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    forceLabel.ForeColor = Color.MediumBlue;
                    forceLabel.RowIndex = 3;
                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(forceLabel);

                }

                //Average Split
                if (ShowAverageSplit)
                {
                    double runningTotal = 0;
                    double averageSplits = 0;
                    foreach (MOE.Common.Business.AnalysisPhaseCycle Cycle in Cycles)
                    {
                        runningTotal = runningTotal + Cycle.Duration.TotalSeconds;
                    }

                    if (Cycles.Count() > 0)
                    {
                        averageSplits = runningTotal / Cycles.Count();
                    }



                    CustomLabel avgLabel = new CustomLabel();
                    avgLabel.FromPosition = plan.StartTime.ToOADate();
                    avgLabel.ToPosition = plan.EndTime.ToOADate();
                    avgLabel.Text = string.Format("{0: 0.0} Avg. Split", averageSplits);
                    avgLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    avgLabel.ForeColor = Color.Black;
                    avgLabel.RowIndex = 4;
                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(avgLabel);

                    //Percentile Split
                    if (SelectedPercentileSplit != null && (Cycles.Count() > 2))
                    {
                        Double percentileResult = 0;
                        Double Percentile = (Convert.ToDouble(SelectedPercentileSplit) / 100);
                        int setCount = Cycles.Count();



                        Double PercentilIndex = Percentile * setCount;
                        if (PercentilIndex % 1 == 0)
                        {
                            percentileResult = Cycles.ElementAt((Convert.ToInt16(PercentilIndex) - 1)).Duration.TotalSeconds;

                        }
                        else
                        {
                            double indexMod = (PercentilIndex % 1);
                            //subtracting .5 leaves just the integer after the convert.
                            //There was probably another way to do that, but this is easy.
                            int indexInt = Convert.ToInt16(PercentilIndex - .5);

                            Double step1 = Cycles.ElementAt((Convert.ToInt16(indexInt) - 1)).Duration.TotalSeconds;
                            Double step2 = Cycles.ElementAt((Convert.ToInt16(indexInt))).Duration.TotalSeconds;
                            Double stepDiff = (step2 - step1);
                            Double step3 = (stepDiff * indexMod);
                            percentileResult = (step1 + step3);

                        }

                        CustomLabel percentileLabel = new CustomLabel();
                        percentileLabel.FromPosition = plan.StartTime.ToOADate();
                        percentileLabel.ToPosition = plan.EndTime.ToOADate();
                        percentileLabel.Text = string.Format("{0: 0.0} - {1} Percentile Split", percentileResult, Convert.ToDouble(SelectedPercentileSplit));
                        percentileLabel.LabelMark = LabelMarkStyle.LineSideMark;
                        percentileLabel.ForeColor = Color.Purple;
                        percentileLabel.RowIndex = 5;
                        chart.ChartAreas[0].AxisX2.CustomLabels.Add(percentileLabel);

                    }

                }
            }
        }

        private void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
            {
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        Plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        Plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        Plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                backGroundColor++;

            }
        }

        private Chart GetNewSplitMonitorChart(DateTime graphStartDate, DateTime graphEndDate, string signalId, string location, int phase)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                   MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalId);
            List<MOE.Common.Models.Detector> detectors = signal.GetDetectorsForSignalByPhaseNumber(phase);
            string detID = "";
            if (detectors.Count() > 0)
            {
                detID = detectors.First().ToString();
            }
            else
            {
                detID = "0";
            }
            Chart chart = new Chart();
            string extendedDirection = string.Empty;
            TimeSpan reportTimespan = graphEndDate - graphStartDate;

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 500;
            chart.Width = 740;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.BorderlineDashStyle = ChartDashStyle.Dot;

            SetChartTitle(chart, phase);
            

            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
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


            chartArea.AxisY.Title = "Phase Duration (Seconds)";
            if(YAxisMax != null)
            {
                chartArea.AxisY.Maximum = YAxisMax.Value;
            }
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas.Add(chartArea);

            Series PedActivity = new Series();
            PedActivity.ChartType = SeriesChartType.Point;
            PedActivity.Color = Color.Transparent;
            PedActivity.Name = "PedActivity";
            PedActivity.MarkerStyle = MarkerStyle.Circle;
            PedActivity.MarkerBorderColor = Color.Orange;
            PedActivity.MarkerBorderWidth = 1;
            PedActivity.MarkerSize = 3;

            Series GapoutSeries = new Series();
            GapoutSeries.ChartType = SeriesChartType.Point;
            GapoutSeries.Color = Color.OliveDrab;
            GapoutSeries.Name = "GapOut";
            GapoutSeries.XValueType = ChartValueType.DateTime;
            GapoutSeries.MarkerStyle = MarkerStyle.Circle;
            GapoutSeries.MarkerSize = 3;

            Series MaxOutSeries = new Series();
            MaxOutSeries.ChartType = SeriesChartType.Point;
            MaxOutSeries.Color = Color.Red;
            MaxOutSeries.Name = "MaxOut";
            MaxOutSeries.XValueType = ChartValueType.DateTime;
            MaxOutSeries.MarkerStyle = MarkerStyle.Circle;
            MaxOutSeries.MarkerSize = 3;

            Series ForceOffSeries = new Series();
            ForceOffSeries.ChartType = SeriesChartType.Point;
            ForceOffSeries.Color = Color.MediumBlue;
            ForceOffSeries.Name = "ForceOff";
            ForceOffSeries.MarkerStyle = MarkerStyle.Circle;
            ForceOffSeries.MarkerSize = 3;

            Series UnknownSeries = new Series();
            UnknownSeries.ChartType = SeriesChartType.Point;
            UnknownSeries.Color = Color.Black;
            UnknownSeries.Name = "Unknown";
            UnknownSeries.MarkerStyle = MarkerStyle.Circle;
            UnknownSeries.MarkerSize = 3;

            Series ProgramedSplit = new Series();
            ProgramedSplit.ChartType = SeriesChartType.StepLine;
            ProgramedSplit.Color = Color.OrangeRed;
            ProgramedSplit.Name = "Programed Split";
            ProgramedSplit.BorderWidth = 1;



            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series testSeries = new Series();
            testSeries.IsVisibleInLegend = false;
            testSeries.ChartType = SeriesChartType.Point;
            testSeries.Color = Color.White;
            testSeries.Name = "Posts";
            testSeries.XValueType = ChartValueType.DateTime;

            chart.Series.Add(ProgramedSplit);
            chart.Series.Add(GapoutSeries);
            chart.Series.Add(MaxOutSeries);
            chart.Series.Add(ForceOffSeries);
            chart.Series.Add(UnknownSeries);
            chart.Series.Add(PedActivity);
            chart.Series.Add(testSeries);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);


            return chart;
        }

        private void SetChartTitle(Chart chart, int phase)
        {
            chart.Titles.Add(ChartTitleFactory.GetChartName(this.MetricTypeID));
            chart.Titles.Add(ChartTitleFactory.GetSignalLocationAndDateRange(this.SignalID, this.StartDate, this.EndDate));
            chart.Titles.Add(ChartTitleFactory.GetPhase(phase));
        }

        private void AddSplitMonitorDataToChart(Chart chart, DateTime startDate,
   DateTime endDate, MOE.Common.Business.AnalysisPhase phase, string signalId, MOE.Common.Business.PlanCollection plans)
        {

            //Table 
            if (phase.Cycles.Items.Count > 0)
            {
                plans.FillMissingSplits();
                int MaxSplitLength = 0;
                foreach (MOE.Common.Business.Plan plan in plans.PlanList)
                {

                    try
                    {

                        chart.Series["Programed Split"].Points.AddXY(plan.StartTime, plan.Splits[phase.PhaseNumber]);
                        chart.Series["Programed Split"].Points.AddXY(plan.EndTime, plan.Splits[phase.PhaseNumber]);

                        if (plan.Splits[phase.PhaseNumber] > MaxSplitLength)
                        {
                            MaxSplitLength = plan.Splits[phase.PhaseNumber];
                        }




                    }
                    catch 
                    {
                        //System.Windows.MessageBox.Show(ex.ToString());
                    }
                }


                foreach (MOE.Common.Business.AnalysisPhaseCycle Cycle in phase.Cycles.Items)
                {


                    if (Cycle.TerminationEvent == 4)
                    {

                        chart.Series["GapOut"].Points.AddXY(Cycle.StartTime, Cycle.Duration.TotalSeconds);
                    }

                    if (Cycle.TerminationEvent == 5)
                    {

                        chart.Series["MaxOut"].Points.AddXY(Cycle.StartTime, Cycle.Duration.TotalSeconds);
                    }

                    if (Cycle.TerminationEvent == 6)
                    {

                        chart.Series["ForceOff"].Points.AddXY(Cycle.StartTime, Cycle.Duration.TotalSeconds);
                    }

                    if (Cycle.TerminationEvent == 0)
                    {

                        chart.Series["Unknown"].Points.AddXY(Cycle.StartTime, Cycle.Duration.TotalSeconds);
                    }

                    if (Cycle.HasPed && ShowPedActivity)
                    {
                        if (Cycle.PedDuration == 0)
                        {
                            if(Cycle.PedStartTime == DateTime.MinValue)
                            {
                                Cycle.SetPedStart(Cycle.StartTime);
                            }
                            if (Cycle.PedEndTime == DateTime.MinValue)
                            {
                                Cycle.SetPedEnd(Cycle.YellowEvent
                                    );
                            }
                        }
                        chart.Series["PedActivity"].Points.AddXY(Cycle.PedStartTime, Cycle.PedDuration);
                    }
                }
                if (MaxSplitLength > 0)
                {
                    if (MaxSplitLength >= 50)
                    {
                        chart.ChartAreas[0].AxisY.Maximum = (1.5 * MaxSplitLength);
                    }
                    else 
                    {
                        chart.ChartAreas[0].AxisY.Maximum = (2.5 * MaxSplitLength);
                    }
                }
                if (YAxisMax != null)
                {
                    chart.ChartAreas[0].AxisY.Maximum = YAxisMax.Value;
                }
            }
        }




    }
}


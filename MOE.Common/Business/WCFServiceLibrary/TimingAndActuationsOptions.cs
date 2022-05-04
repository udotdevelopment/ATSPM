using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc.Routing.Constraints;
using System.Web.UI.DataVisualization.Charting;
using AjaxControlToolkit;
using MOE.Common.Business.TimingAndActuations;
using MOE.Common.Migrations;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class TimingAndActuationsOptions : MetricOptions
    {
        [Display(Name = "Legend")]
        [Required]
        [DataMember]
        public bool ShowLegend { get; set; }

        [Display(Name = "Header For Each Phase")]
        [Required]
        [DataMember]
        public bool ShowHeaderForEachPhase { get; set; }

        [Display(Name = "Combine Lanes for Phase")]
        [Required]
        [DataMember]
        public bool CombineLanesForEachGroup { get; set; }

        [Display(Name = "Dot and Marker Size")]
        [Required]
        [DataMember]
        public int DotAndBarSize { get; set; }

        [Display(Name = "Phase Filter")]
        [DataMember]
        public string PhaseFilter { get; set; }

        [Display(Name = "Phase Event Codes")]
        [DataMember]
        public string PhaseEventCodes { get; set; }

        [Display(Name = "Global Event Codes")]
        [DataMember]
        public string GlobalCustomEventCodes { get; set; }

        [Display(Name = "Global Event Params")]
        [DataMember]
        public string GlobalCustomEventParams { get; set; }

        [Display(Name = "Extend Search (left) Minutes.decimal")]
        [Required]
        [DataMember]
        public double ExtendVsdSearch { get; set; }

        [Display(Name = "Vehicle Signal Display")]
        [Required]
        [DataMember]
        public bool ShowVehicleSignalDisplay { get; set; }

        [Display(Name = "Pedestrian Intervals")]
        [Required]
        [DataMember]
        public bool ShowPedestrianIntervals { get; set; }

        [Display(Name = "Pedestrian Actuations")]
        [Required]
        [DataMember]
        public bool ShowPedestrianActuation { get; set; }

        [Display(Name = "Extend Start/Stop Search Minutes.decimal")]
        [Required]
        [DataMember]
        public double ExtendStartStopSearch { get; set; }

        [Display(Name = "Stop Bar Presence")]
        [Required]
        [DataMember]
        public bool ShowStopBarPresence { get; set; }

        [Display(Name = "Lane-by-lane Count")]
        [ Required]
        [DataMember]
        public bool ShowLaneByLaneCount { get; set; }

        [Display(Name = "Advanced Presence")]
        [Required]
        [DataMember]
        public bool ShowAdvancedDilemmaZone { get; set; }

        [Display(Name = "Advanced Count")]
        [Required]
        [DataMember]
        public bool ShowAdvancedCount { get; set; }
        
        [Display(Name = "Advd Count Offset")]
        [DataMember]
        public double AdvancedOffset { get; set; }

        [Display(Name = "All Lanes For Each Phase")]
        [Required]
        [DataMember]
        public bool ShowAllLanesInfo { get; set; }
        
        [Display(Name = "On/Off Lines")]
        [Required]
        [DataMember]
        public bool ShowLinesStartEnd { get; set; }
        
        [Display(Name = "Event Start Stop Pairs")]
        [Required]
        [DataMember]
        public bool ShowEventPairs { get; set; }
        
        [Display(Name = "Raw Data Display")]
        [Required]
        [DataMember]
        public bool ShowRawEventData { get; set; }

        [Display(Name = "Show Permissive Phases")]
        [Required]
        [DataMember]
        public bool ShowPermissivePhases { get; set; }
        
        public List<int> GlobalEventCodesList { get; set; }
        public List<int> GlobalEventParamsList { get; set; }
        public List<int> PhaseEventCodesList { get; set; }
        public List<int> PhaseFilterList { get; set; }
        public int GlobalEventCounter { get; set; }
        public Models.Signal Signal { get; set; }
        public int HeadTitleCounter { get; set; }

        public TimingAndActuationsOptions(string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            MetricTypeID = 17;
            //ExtendSearch = 0;
        }

        public TimingAndActuationsOptions()
        {
            MetricTypeID = 17;
            //ExtendSearch = 0;
            SetDefaults();
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalRepository = SignalsRepositoryFactory.Create();
            Signal = signalRepository.GetVersionOfSignalByDateWithDetectionTypes(SignalID, StartDate);
            var chart = new Chart();
            HeadTitleCounter = 0;
            var timingAndActuationsForPhases = new List<TimingAndActuationsForPhase>();
            PhaseEventCodesList = ExtractListOfNumbers(PhaseEventCodes);
            int phaseCounter = 0;
            PhaseFilterList = ExtractListOfNumbers(PhaseFilter);
            if (ShowRawEventData)
            {
                for (int i = 1; i <= 16; i++)
                {
                    if (PhaseFilterList.Any() && PhaseFilterList.Contains(i) || PhaseFilterList.Count == 0)
                    {
                        var phaseOrOverlap = true;
                        timingAndActuationsForPhases.Add(new TimingAndActuationsForPhase(i, phaseOrOverlap, this));
                        timingAndActuationsForPhases[phaseCounter++].PhaseNumberSort = "Phase - " + i.ToString("D2");
                        phaseOrOverlap = false;
                        timingAndActuationsForPhases.Add(new TimingAndActuationsForPhase(i, phaseOrOverlap, this));
                        timingAndActuationsForPhases[phaseCounter++].PhaseNumberSort = "zOverlap - " + i.ToString("D2");
                    }
                }
            }
            else
            {
                //var approachNumber = 1;
                if (Signal.Approaches.Count > 0)
                {
                    //Parallel.ForEach(Signal.Approaches, approach =>
                    foreach (var approach in Signal.Approaches)
                    {
                        bool permissivePhase;
                        if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                        {
                            if (PhaseFilterList.Any() && PhaseFilterList.Contains((int) approach.PermissivePhaseNumber)
                                || PhaseFilterList.Count == 0)
                            {
                                var permissivePhaseNumber = (int) approach.PermissivePhaseNumber;
                                permissivePhase = true;
                                timingAndActuationsForPhases.Add(
                                    new TimingAndActuationsForPhase(approach, this, permissivePhase));
                                timingAndActuationsForPhases[phaseCounter++].PhaseNumberSort =
                                    approach.PermissivePhaseNumber.Value.ToString("D2") + "-1";
                            }
                        }

                        if (approach.ProtectedPhaseNumber > 0)
                        {
                            if (PhaseFilterList.Any() && PhaseFilterList.Contains(approach.ProtectedPhaseNumber)
                                || PhaseFilterList.Count == 0)
                            {
                                permissivePhase = false;
                                timingAndActuationsForPhases.Add(
                                    new TimingAndActuationsForPhase(approach, this, permissivePhase));
                                timingAndActuationsForPhases[phaseCounter++].PhaseNumberSort =
                                    approach.ProtectedPhaseNumber.ToString("D2") + "-2";
                            }
                        }
                    }
                    //});
                }
            }

            timingAndActuationsForPhases = timingAndActuationsForPhases.OrderBy(t => t.PhaseNumberSort).ToList();
            //  Can not add parrallelism because of contention for the same chart.
            //Parallel.ForEach(timingAndActuationsForPhases, timingAndActutionsForPhase =>
                foreach (var timingAndActutionsForPhase in timingAndActuationsForPhases)
            {
                if (timingAndActutionsForPhase.GetPermissivePhase==false ||
                    ShowPermissivePhases)
                {
                    GetChart(timingAndActutionsForPhase);
                }
           // });
            }

        GlobalEventCodesList = ExtractListOfNumbers(GlobalCustomEventCodes);
            GlobalEventParamsList = ExtractListOfNumbers(GlobalCustomEventParams);
            GlobalEventCounter = 0;
            if (GlobalEventCodesList != null && GlobalEventParamsList != null &&
                GlobalEventCodesList.Any() && GlobalEventCodesList.Count > 0 &&
                GlobalEventParamsList.Any() && GlobalEventParamsList.Count > 0)
            {
                var globalGetDataTimingAndActuations = new GlobalGetDataTimingAndActuations(SignalID, this);
                GetGlobalChart(globalGetDataTimingAndActuations, this);
            }
            if (ShowLegend)
            {
                GetChartLegend();
            }
            return ReturnList;
        }

        //private void CreateFilesForPartitions()
        //{
        //    string path = @"C:\temp\PFunction_ControllerAndSpeed.txt";
        //    //using (var PF = new StreamWriter(path, false))
            //{
            //    PF.WriteLine("USE [MOE]");
            //    PF.WriteLine("GO");
            //    PF.WriteLine();
            //    PF.WriteLine("CREATE PARTITION FUNCTION [PF_MOE_ControllerAndSpeed](datetime2(7)) AS RANGE RIGHT FOR VALUES ");
            //    PF.Write("(");
            //    var inTheBegining = this.StartDate;
            //    var inTheEnd = this.EndDate;
            //    for (int i = 0; i < 265; i++)
            //    {
            //        PF.Write("N'");
            //        PF.WriteLine(inTheBegining.ToString("s") + "',");
            //        inTheBegining = inTheBegining.AddDays(7);
            //    }
            //    PF.WriteLine();
            //    PF.WriteLine("GO");
            //    PF.Close();
            //}

            //path = @"C:\temp\PSchema_ControllerAndSpeed.txt";
            //using (var PS = new StreamWriter(path, false))
            //{
            //    var inTheBegining = this.StartDate;
            //    PS.WriteLine("USE [MOE]");
            //    PS.WriteLine("GO");
            //    PS.WriteLine();
            //    PS.WriteLine("CREATE PARTITION SCHEME [PS_MOE_ControllerAndSpeed] AS PARTITION [PF_MOE_ControllerAndSpeed] TO (");
            //    for (int i = 0; i < 265; i++)
            //    {
                    
            //        PS.Write("[MOE_");
            //        PS.WriteLine(inTheBegining.ToString("yyyy-MM-dd") + "],");
            //        inTheBegining = inTheBegining.AddDays(7);
            //    }
            //    PS.WriteLine(")");
            //    PS.WriteLine("GO");
            //    PS.Close();
            //}

            //path = @"C:\temp\Files_ControllerAndSpeed.txt";
            //using (var PF = new StreamWriter(path, false))
            //{
            //    PF.WriteLine("USE [MOE]");
            //    PF.WriteLine("GO");
            //    PF.WriteLine();
            //    var inTheBegining = this.StartDate;
            //    var inTheEnd = this.EndDate;
            //    for (int i = 0; i < 265; i++)
            //    {
            //        PF.Write("ALTER DATABASE [MOE] ADD FILE (NAME = N'MOE_ControllerAndSpeed_");
            //        PF.WriteLine(inTheBegining.ToString("yyyy-MM-dd")
            //                 + @"', FILENAME = N'E:\MSSQL\DATA\MOE_PARTITIONS\ControllerAndSpeed\ControllerAndSpeed_"
            //                 + inTheBegining.ToString("yyyy-MM-dd")
            //                 + ".NDF', SIZE = 8192MB, MAXSIZE = UNLIMITED, FILEGROWTH = 64MB) TO FILEGROUP "
            //                 + "[MOE_ControllerAndSpeed_" + inTheBegining.ToString("yyyy-MM-dd") + "]");
            //        inTheBegining = inTheBegining.AddDays(7);
            //    }
            //    PF.WriteLine();
            //    PF.WriteLine("GO");
            //    PF.Close();
            //}


            //path = @"C:\temp\FileGroup_ControllerAndSpeed.txt";
            //using (var PS = new StreamWriter(path, false))
            //{
            //    var inTheBegining = this.StartDate;
            //    PS.WriteLine("USE [MOE]");
            //    PS.WriteLine("GO");
            //    PS.WriteLine();
            //    for (int i = 0; i < 265; i++)
            //    {
            //        PS.Write("ALTER DATABASE [MOE] ADD FILEGROUP [MOE_ControllerAndSpeed_");
            //        PS.WriteLine(inTheBegining.ToString("yyyy-MM-dd") + "]");
            //        inTheBegining = inTheBegining.AddDays(7);
            //    }
            //    PS.WriteLine();
            //    PS.WriteLine("GO");
            //    PS.Close();
            //}




        //    path = @"C:\temp\PFunction_Aggregation.txt";
        //    using (var PF = new StreamWriter(path, false))
        //    {
        //        PF.WriteLine("USE [MOE]");
        //        PF.WriteLine("GO");
        //        PF.WriteLine();
        //        PF.WriteLine("CREATE PARTITION FUNCTION [PF_MOE_Aggregation](datetime2(7)) AS RANGE RIGHT FOR VALUES ");
        //        PF.Write("(");
        //        var inTheBegining = this.EndDate;
        //        for (int i = 0; i < 100; i++)
        //        {
        //            PF.Write("N'");
        //            PF.WriteLine(inTheBegining.ToString("s") + "',");
        //            inTheBegining = inTheBegining.AddMonths(1);
        //        }
        //        PF.WriteLine();
        //        PF.WriteLine("GO");
        //        PF.Close();
        //    }

        //    path = @"C:\temp\PSchema_Aggregation.txt";
        //    using (var PS = new StreamWriter(path, false))
        //    {
        //        var inTheBegining = this.EndDate;
        //        PS.WriteLine("USE [MOE]");
        //        PS.WriteLine("GO");
        //        PS.WriteLine();
        //        PS.WriteLine("CREATE PARTITION SCHEME [PS_MOE_Aggregation] AS PARTITION [PF_MOE_Aggregation] TO (");
        //        for (int i = 0; i < 100; i++)
        //        {
                    
        //            PS.Write("[MOE_Aggregation_");
        //            PS.WriteLine(inTheBegining.ToString("yyyy-MM-dd") + "],");
        //            inTheBegining = inTheBegining.AddMonths(1);
        //        }
        //        PS.WriteLine(")");
        //        PS.WriteLine("GO");
        //        PS.Close();
        //    }

        //    path = @"C:\temp\Files_Aggregation.txt";
        //    using (var PF = new StreamWriter(path, false))
        //    {
        //        PF.WriteLine("USE [MOE]");
        //        PF.WriteLine("GO");
        //        PF.WriteLine();
        //        var inTheBegining = this.EndDate;
        //        for (int i = 0; i < 100; i++)
        //        {
        //            PF.Write("ALTER DATABASE [MOE] ADD FILE (NAME = N'MOE_Aggregation_");
        //            PF.WriteLine(inTheBegining.ToString("yyyy-MM-dd")
        //                         + @"', FILENAME = N'E:\MSSQL\DATA\MOE_PARTITIONS\Aggregation\Aggregation_"
        //                         + inTheBegining.ToString("yyyy-MM-dd")
        //                         + ".NDF', SIZE = 2048MB, MAXSIZE = UNLIMITED, FILEGROWTH = 64MB) TO FILEGROUP "
        //                         + "[MOE_Aggregation_" + inTheBegining.ToString("yyyy-MM-dd") + "]");
               
        //            inTheBegining = inTheBegining.AddMonths(1);
        //        }
        //        PF.WriteLine();
        //        PF.WriteLine("GO");
        //        PF.Close();
        //    }


        //    path = @"C:\temp\FileGroup_Aggregation.txt";
        //    using (var PS = new StreamWriter(path, false))
        //    {
        //        var inTheBegining = this.EndDate;
        //        PS.WriteLine("USE [MOE]");
        //        PS.WriteLine("GO");
        //        PS.WriteLine();
        //        for (int i = 0; i < 100; i++)
        //        {
        //            PS.WriteLine("ALTER DATABASE [MOE] ADD FILEGROUP [MOE_Aggregation_" + inTheBegining.ToString("yyyy-MM-dd") + "]");
        //            inTheBegining = inTheBegining.AddMonths(1);
        //        }
        //        PS.WriteLine();
        //        PS.WriteLine("GO");
        //        PS.Close();
        //    }
        //}





        private void GetChartLegend()
        {
            if (ReturnList == null)
                ReturnList = new List<string>();
            bool showRawData = true;
            var taaLegendChart = new TimingAndActuationsChartForPhase(showRawData);
           if (taaLegendChart.Chart != null)
            {
                taaLegendChart.Chart.BackColor = Color.Goldenrod;
                var chartName = CreateFileName();
                taaLegendChart.Chart.ImageLocation = MetricFileLocation + chartName;
                taaLegendChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                ReturnList.Add(MetricWebPath + chartName);
            }
        }

        private void GetGlobalChart(GlobalGetDataTimingAndActuations globalGetDataTimingAndActuations,
            TimingAndActuationsOptions options)
        {
            if (ReturnList == null) ReturnList = new List<string>();
            var taaGlobalChart =
                new ChartForGlobalEventsTimingAndActuations(globalGetDataTimingAndActuations, options)
                {
                    Chart = {BackColor = Color.LightSkyBlue}
                };
            var chartName = CreateFileName();
            taaGlobalChart.Chart.ImageLocation = MetricFileLocation + chartName;
            taaGlobalChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
            ReturnList.Add(MetricWebPath + chartName);
        }

        private void GetChart(TimingAndActuationsForPhase timingAndActutionsForPhase)
        {
            if (ReturnList == null)
                ReturnList = new List<string>();
            var taaChart = new TimingAndActuationsChartForPhase(timingAndActutionsForPhase);
            if (taaChart.Chart != null)
            {
                if (timingAndActutionsForPhase.GetPermissivePhase)
                {
                    taaChart.Chart.BackColor = Color.LightGray;
                }
                var chartName = CreateFileName();
                taaChart.Chart.ImageLocation = MetricFileLocation + chartName;
                taaChart.Chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
                ReturnList.Add(MetricWebPath + chartName);
            }
        }

        public List<int> ExtractListOfNumbers(string comnmaDashSepartedList)
        {
            var numbers = new List<int>();
            if (String.IsNullOrEmpty(comnmaDashSepartedList))
            {
                return numbers;
            }
            numbers = CommaDashSeparated(comnmaDashSepartedList);
            return numbers;
        }

        private List<int> CommaDashSeparated(string codesList)
        {
            var processedNumbers = new List<int>();
            var codes = codesList.Split(',');
            try
            {
                foreach (string code in codes)
                {
                    if (code.Contains('-'))
                    {
                        string[] withDash = code.Split('-');
                        int minNum = Convert.ToInt32(withDash[0]);
                        int maxNum = Convert.ToInt32(withDash[1]);
                        if (minNum > maxNum)
                        {
                            int temp = minNum;
                            minNum = maxNum;
                            maxNum = temp;
                        }
                        for (int ii = minNum; ii <= maxNum; ii++)
                        {
                            processedNumbers.Add(ii);
                        }
                    }
                    else
                    {
                        processedNumbers.Add(Convert.ToInt32(code));
                    }
                }
                processedNumbers = processedNumbers.Distinct().OrderByDescending(p => p).ToList();
            }
            catch (Exception e)
            {
            }
            return processedNumbers;
        }
    }
}

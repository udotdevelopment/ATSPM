using System;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using Action = MOE.Common.Models.Action;

namespace MOE.Common.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SPM>
    {
        private readonly bool _pendingMigrations;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            var migrator = new DbMigrator(this);
            //_pendingMigrations = migrator.GetPendingMigrations().Any();
            CommandTimeout = int.MaxValue;
        }

        protected override void Seed(SPM context)
        {
            //  This method will be called after migrating to the latest version.
            context.Menus.AddOrUpdate(
                m => m.MenuId,
                new Menu
                {
                    MenuId = 1,
                    MenuName = "Measures",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 2,
                    MenuName = "Reports",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 3,
                    MenuName = "Log Action Taken",
                    Controller = "ActionLogs",
                    Action = "Create",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 30
                },
                new Menu
                {
                    MenuId = 4,
                    MenuName = "Links",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 40
                },
                new Menu
                {
                    MenuId = 5,
                    MenuName = "FAQ",
                    Controller = "FAQs",
                    Action = "Display",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 50
                },
                new Menu
                {
                    MenuId = 27,
                    MenuName = "About",
                    Controller = "Home",
                    Action = "About",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 90
                },
                new Menu
                {
                    MenuId = 11,
                    MenuName = "Admin",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 100
                },
                new Menu
                {
                    MenuId = 9,
                    MenuName = "Signal",
                    Controller = "DefaultCharts",
                    Action = "Index",
                    ParentId = 1,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 10,
                    MenuName = "Purdue Link Pivot",
                    Controller = "LinkPivot",
                    Action = "Analysis",
                    ParentId = 1,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 8,
                    MenuName = "Chart Usage",
                    Controller = "ActionLogs",
                    Action = "Usage",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 71,
                    MenuName = "Configuration",
                    Controller = "Signals",
                    Action = "SignalDetail",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 15
                },
                new Menu
                {
                    MenuId = 48,
                    MenuName = "Aggregate Data",
                    Controller = "AggregateDataExport",
                    Action = "Index",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 58,
                    MenuName = "Left Turn Gap Analysis",
                    Controller = "LeftTurnGapReport",
                    Action = "Index",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 25
                },
                new Menu
                {
                    MenuId = 12,
                    MenuName = "Signal Configuration",
                    Controller = "Signals",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 16,
                    MenuName = "Menu Configuration",
                    Controller = "Menus",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 13,
                    MenuName = "Route Configuration",
                    Controller = "Routes",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 30
                },
                new Menu
                {
                    MenuId = 66,
                    MenuName = "Area Configuration",
                    Controller = "Areas",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 40
                },
                new Menu
                {
                    MenuId = 17,
                    MenuName = "Jurisdiction Configuration",
                    Controller = "Jurisdictions",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 50
                },
                new Menu
                {
                    MenuId = 57,
                    MenuName = "General Settings",
                    Controller = "GeneralSettings",
                    Action = "Edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 60
                },
                new Menu
                {
                    MenuId = 100,
                    MenuName = "Measure Defaults Settings",
                    Controller = "MeasuresDefaults",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 70
                },
                new Menu
                {
                    MenuId = 56,
                    MenuName = "Database Archive Settings",
                    Controller = "DatabaseArchiveSettings",
                    Action = "edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 80
                },
                new Menu
                {
                    MenuId = 49,
                    MenuName = "Raw Data Export",
                    Controller = "DataExport",
                    Action = "RawDataExport",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 90
                },
                new Menu
                {
                    MenuId = 54,
                    MenuName = "Watch Dog",
                    Controller = "WatchDogApplicationSettings",
                    Action = "Edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 100
                },
                new Menu
                {
                    MenuId = 51,
                    MenuName = "Users",
                    Controller = "SPMUsers",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 110
                },
                new Menu
                {
                    MenuId = 15,
                    MenuName = "Roles",
                    Controller = "Account",
                    Action = "RoleAddToUser",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 120
                },
                new Menu
                {
                    MenuId = 52,
                    MenuName = "FAQs",
                    Controller = "FAQs",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 130
                },
                new Menu
                {
                    MenuId = 42,
                    MenuName = "GDOT ATSPM Installation Manual",
                    Controller = "Images",
                    Action = "ATSPM_Installation_Manual.pdf",
                    ParentId = 6,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 34,
                    MenuName = "GDOT ATSPM Component Details",
                    Controller = "Images",
                    Action = "ATSPM_Component_Details_20200120.pdf",
                    ParentId = 6,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 43,
                    MenuName = "GDOT ATSPM Reporting Details",
                    Controller = "Images",
                    Action = "ATSPM_Reporting_Details_20200121.pdf",
                    ParentId = 6,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 30
                },
                new Menu
                {
                    MenuId = 70,
                    MenuName = "ATSPM User Case Examples Manual",
                    Controller = "Images",
                    Action = "ATSPM_User Case Examples_Manual_20200128.pdf",
                    ParentId = 6,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 40
                },
                 new Menu
                 {
                     MenuId = 75,
                     MenuName = "Methods and Assumptions",
                     Controller = "Images",
                     Action = "ATSPMMethodsandAssumptions.pdf",
                     ParentId = 6,
                     Application = "SignalPerformanceMetrics",
                     DisplayOrder = 45
                 }
            );

            context.ExternalLinks.AddOrUpdate(
                c => c.DisplayOrder,
                new ExternalLink
                {
                    Name = "Indiana Hi Resolution Data Logger Enumerations",
                    DisplayOrder = 1,
                    Url = " https://docs.lib.purdue.edu/jtrpdata/3/"
                },
                new ExternalLink
                {
                    Name = "Florida ATSPM",
                    DisplayOrder = 2,
                    Url = "https://atspm.cflsmartroads.com/ATSPM"
                },
                new ExternalLink
                {
                    Name = "FAST (Southern Nevada)",
                    DisplayOrder = 3,
                    Url = "http://challenger.nvfast.org/spm"
                },
                new ExternalLink
                {
                    Name = "Georgia ATSPM",
                    DisplayOrder = 4,
                    Url = "https://traffic.dot.ga.gov/atspm"
                },
                new ExternalLink
                {
                    Name = "Arizona ATSPM",
                    DisplayOrder = 5,
                    Url = "http://spmapp01.mcdot-its.com/ATSPM"
                },
                 new ExternalLink
                 {
                     Name = "PennDOT ATSPM Interface",
                     DisplayOrder = 7,
                     Url = "https://www.dot.state.pa.us/public/Bureaus/BOMO/Portal/ATSPM/index.html"
                 },
                new ExternalLink
                {
                    Name = "ATSPM Workshop 2016 SLC",
                    DisplayOrder = 8,
                    Url = "http://docs.lib.purdue.edu/atspmw/2016"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 1 - Morning",
                    DisplayOrder = 9,
                    Url = "https://connectdot.connectsolutions.com/p75dwqefphk"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 1 - Afternoon",
                    DisplayOrder = 10,
                    Url = "https://connectdot.connectsolutions.com/p6l6jaoy3gj"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 2 - Morning",
                    DisplayOrder = 11,
                    Url = "https://connectdot.connectsolutions.com/p6mlkvekogo/"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 2 - Mid Morning",
                    DisplayOrder = 12,
                    Url = "https://connectdot.connectsolutions.com/p3ua8gtj09r/"
                }
            );
            context.ControllerType.AddOrUpdate(
                c => c.ControllerTypeID,
                new ControllerType
                {
                    ControllerTypeID = 1,
                    Description = "ASC3",
                    SNMPPort = 161,
                    FTPDirectory = "//Set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 2,
                    Description = "Cobalt",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 3,
                    Description = "ASC3 - 2070",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 4,
                    Description = "MaxTime",
                    SNMPPort = 161,
                    FTPDirectory = "none",
                    ActiveFTP = false,
                    UserName = "none",
                    Password = "none"
                },
                new ControllerType
                {
                    ControllerTypeID = 5,
                    Description = "Trafficware",
                    SNMPPort = 161,
                    FTPDirectory = "none",
                    ActiveFTP = true,
                    UserName = "none",
                    Password = "none"
                },
                new ControllerType
                {
                    ControllerTypeID = 6,
                    Description = "Siemens SEPAC",
                    SNMPPort = 161,
                    FTPDirectory = "/mnt/sd",
                    ActiveFTP = false,
                    UserName = "admin",
                    Password = "$adm*kon2"
                },
                new ControllerType
                {
                    ControllerTypeID = 7,
                    Description = "McCain ATC EX",
                    SNMPPort = 161,
                    FTPDirectory = " /mnt/rd/hiResData",
                    ActiveFTP = false,
                    UserName = "root",
                    Password = "root"
                },
                new ControllerType
                {
                    ControllerTypeID = 8,
                    Description = "Peek",
                    SNMPPort = 161,
                    FTPDirectory = "mnt/sram/cuLogging",
                    ActiveFTP = false,
                    UserName = "atc",
                    Password = "PeekAtc"
                },
                new ControllerType
                {
                    ControllerTypeID = 9,
                    Description = "EOS",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                }
            );

            context.DetectionTypes.AddOrUpdate(
                c => c.DetectionTypeID,
                new DetectionType { DetectionTypeID = 1, Description = "Basic" },
                new DetectionType { DetectionTypeID = 2, Description = "Advanced Count" },
                new DetectionType { DetectionTypeID = 3, Description = "Advanced Speed" },
                new DetectionType { DetectionTypeID = 4, Description = "Lane-by-lane Count" },
                new DetectionType { DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction" },
                new DetectionType { DetectionTypeID = 6, Description = "Stop Bar Presence" },
                new DetectionType { DetectionTypeID = 7, Description = "Advanced Presence" }
            );

            context.MeasuresDefaults.AddOrUpdate(
                new MeasuresDefaults { Measure = "AoR", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "AoR", OptionName = "ShowPlanStatistics", Value = "True" },
                new MeasuresDefaults { Measure = "AoR", OptionName = "YAxisMax", Value = null },

                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "ShowDelayPerVehicle", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "ShowPlanStatistics", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "ShowTotalDelayPerHour", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "Y2AxisMax", Value = "10" },
                new MeasuresDefaults { Measure = "ApproachDelay", OptionName = "YAxisMax", Value = "15" },

                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "Show15Percentile", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "Show85Percentile", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "ShowAverageSpeed", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "ShowPostedSpeed", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "ShowPlanStatistics", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "YAxisMax", Value = "60" },
                new MeasuresDefaults { Measure = "ApproachSpeed", OptionName = "YAxisMin", Value = "0" },

                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "YAxisMin", Value = "0" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "YAxisMax", Value = null },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowDirectionalSplits", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowTotalVolume", Value = "False" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowNbEbVolume", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowSbWbVolume", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowTMCDetection", Value = "True" },
                new MeasuresDefaults { Measure = "ApproachVolume", OptionName = "ShowAdvanceDetection", Value = "True" },


                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "BinSize", Value = "15" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap1Max", Value = "3.3" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap1Min", Value = "1" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap2Max", Value = "3.7" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap2Min", Value = "3.3" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap3Max", Value = "7.4" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap3Min", Value = "3.7" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "Gap4Min", Value = "7.4" },
                new MeasuresDefaults { Measure = "LeftTurnGapAnalysis", OptionName = "TrendLineGapThreshold", Value = "7.4" },

                new MeasuresDefaults { Measure = "PCD", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "SelectedDotSize", Value = "1" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "SelectedLineSize", Value = "1" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "ShowPlanStatistics", Value = "True" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "ShowVolumes", Value = "True" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "Y2AxisMax", Value = "2000" },
                new MeasuresDefaults { Measure = "PCD", OptionName = "YAxisMax", Value = "150" },

                new MeasuresDefaults { Measure = "PedDelay", OptionName = "YAxisMax", Value = "180" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "TimeBuffer", Value = "15" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "ShowPedBeginWalk", Value = "True" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "ShowCycleLength", Value = "True" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "ShowPercentDelay", Value = "True" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "ShowPedRecall", Value = "False" },
                new MeasuresDefaults { Measure = "PedDelay", OptionName = "PedRecallThreshold", Value = "75" },

                new MeasuresDefaults { Measure = "PhaseTermination", OptionName = "SelectedConsecutiveCount", Value = "3" },
                new MeasuresDefaults { Measure = "PhaseTermination", OptionName = "ShowPedActivity", Value = "True" },
                new MeasuresDefaults { Measure = "PhaseTermination", OptionName = "ShowPlanStripes", Value = "True" },
                new MeasuresDefaults { Measure = "PhaseTermination", OptionName = "YAxisMax", Value = null },

                new MeasuresDefaults { Measure = "SplitFail", OptionName = "FirstSecondsOfRed", Value = "5" },
                new MeasuresDefaults { Measure = "SplitFail", OptionName = "ShowAvgLines", Value = "True" },
                new MeasuresDefaults { Measure = "SplitFail", OptionName = "ShowFailLines", Value = "True" },
                new MeasuresDefaults { Measure = "SplitFail", OptionName = "ShowPercentFailLines", Value = "False" },

                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "SelectedPercentileSplit", Value = "85" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowAverageSplit", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowPedActivity", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowPercentGapOuts", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowPercentMaxOutForceOff", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowPercentSkip", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "ShowPlanStripes", Value = "True" },
                new MeasuresDefaults { Measure = "SplitMonitor", OptionName = "YAxisMax", Value = null },

                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "AdvancedOffset", Value = "0" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "CombineLanesForEachGroup", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "DotAndBarSize", Value = "6" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ExtendStartStopSearch", Value = "2" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ExtendVsdSearch", Value = "5" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowAdvancedCount", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowAdvancedDilemmaZone", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowAllLanesInfo", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowEventPairs", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowHeaderForEachPhase", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowLaneByLaneCount", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowLegend", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowLinesStartEnd", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowPedestrianActuation", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowPedestrianIntervals", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowPermissivePhases", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowRawEventData", Value = "False" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowStopBarPresence", Value = "True" },
                new MeasuresDefaults { Measure = "TimingAndActuations", OptionName = "ShowVehicleSignalDisplay", Value = "True" },

                new MeasuresDefaults { Measure = "TMC", OptionName = "SelectedBinSize", Value = "15" },
                new MeasuresDefaults { Measure = "TMC", OptionName = "ShowDataTable", Value = "False" },
                new MeasuresDefaults { Measure = "TMC", OptionName = "ShowLaneVolumes", Value = "True" },
                new MeasuresDefaults { Measure = "TMC", OptionName = "ShowTotalVolumes", Value = "True" },
                new MeasuresDefaults { Measure = "TMC", OptionName = "Y2AxisMax", Value = "300" },
                new MeasuresDefaults { Measure = "TMC", OptionName = "YAxisMax", Value = "1000" },

                new MeasuresDefaults { Measure = "WaitTime", OptionName = "ShowPlanStripes", Value = "True" },

                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "SevereLevelSeconds", Value = "4" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowAverageTimeRedLightViolations", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowAverageTimeYellowOccurences", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowPercentRedLightViolations", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowPercentSevereRedLightViolations", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowPercentYellowLightOccurrences", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowRedLightViolations", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowSevereRedLightViolations", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "ShowYellowLightOccurrences", Value = "True" },
                new MeasuresDefaults { Measure = "YellowAndRed", OptionName = "YAxisMax", Value = "15" }
                );


            context.MetricTypes.AddOrUpdate(
                c => c.MetricID,
                new MetricType
                {
                    MetricID = 1,
                    ChartName = "Purdue Phase Termination",
                    Abbreviation = "PPT",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 1
                },
                new MetricType
                {
                    MetricID = 2,
                    ChartName = "Split Monitor",
                    Abbreviation = "SM",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 5
                },
                new MetricType
                {
                    MetricID = 3,
                    ChartName = "Pedestrian Delay",
                    Abbreviation = "PedD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 10
                },
                new MetricType
                {
                    MetricID = 4,
                    ChartName = "Preemption Details",
                    Abbreviation = "PD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 15
                },
                new MetricType
                {
                    MetricID = 17,
                    ChartName = "Timing and Actuation",
                    Abbreviation = "TAA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 20
                },
                new MetricType
                {
                    MetricID = 12,
                    ChartName = "Purdue Split Failure",
                    Abbreviation = "SF",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 30
                },
                new MetricType
                {
                    MetricID = 11,
                    ChartName = "Yellow and Red Actuations",
                    Abbreviation = "YRA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 35
                },
                new MetricType
                {
                    MetricID = 5,
                    ChartName = "Turning Movement Counts",
                    Abbreviation = "TMC",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 40
                },
                new MetricType
                {
                    MetricID = 7,
                    ChartName = "Approach Volume",
                    Abbreviation = "AV",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 45
                },
                new MetricType
                {
                    MetricID = 8,
                    ChartName = "Approach Delay",
                    Abbreviation = "AD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 50
                },
                new MetricType
                {
                    MetricID = 9,
                    ChartName = "Arrivals on Red",
                    Abbreviation = "AoR",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 55
                },
                new MetricType
                {
                    MetricID = 6,
                    ChartName = "Purdue Coordination Diagram",
                    Abbreviation = "PCD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 60
                },
                new MetricType
                {
                    MetricID = 10,
                    ChartName = "Approach Speed",
                    Abbreviation = "Speed",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 65
                },
                new MetricType
                {
                    MetricID = 13,
                    ChartName = "Purdue Link Pivot",
                    Abbreviation = "LP",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 70
                },
                new MetricType
                {
                    MetricID = 15,
                    ChartName = "Preempt Service",
                    Abbreviation = "PS",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 75
                },
                new MetricType
                {
                    MetricID = 14,
                    ChartName = "Preempt Service Request",
                    Abbreviation = "PSR",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 80
                },
                new MetricType
                {
                    MetricID = 16,
                    ChartName = "Detector Activation Count",
                    Abbreviation = "DVA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 85
                },

                new MetricType
                {
                    MetricID = 18,
                    ChartName = "Approach Pcd", //"Purdue Coodination",
                    Abbreviation = "APCD", // "PCDA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 102
                },
                new MetricType
                {
                    MetricID = 19,
                    ChartName = "Approach Cycle", // "Cycle"
                    Abbreviation = "CA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 103
                },
                new MetricType
                {
                    MetricID = 20,
                    ChartName = "Approach Split Fail", //"Purdue Split Failure",
                    Abbreviation = "SFA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 104
                },
                new MetricType
                {
                    MetricID = 22,
                    ChartName = "Signal Preemption", //"Preemption",
                    Abbreviation = "PreemptA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 105
                },
                new MetricType
                {
                    MetricID = 24,
                    ChartName = "Signal Priority", // "Transit Signal Priority",
                    Abbreviation = "TSPA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 106
                },
                new MetricType
                {
                    MetricID = 25,
                    ChartName = "Approach Speed",
                    Abbreviation = "ASA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 107
                },
                new MetricType
                {
                    MetricID = 26,
                    ChartName = "Approach Yellow Red Activations", //"Yellow Red Activations",
                    Abbreviation = "YRAA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 108
                },
                new MetricType
                {
                    MetricID = 27,
                    ChartName = "Signal Event Count",
                    Abbreviation = "SEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 109
                },
                new MetricType
                {
                    MetricID = 28,
                    ChartName = "Approach Event Count",
                    Abbreviation = "AEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 110
                },
                new MetricType
                {
                    MetricID = 29,
                    ChartName = "Phase Termination",
                    Abbreviation = "AEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 111
                },
                new MetricType
                {
                    MetricID = 30,
                    ChartName = "Phase Pedestrian Delay",
                    Abbreviation = "APD",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 112
                },
                new MetricType
                {
                    MetricID = 31,
                    ChartName = "Left Turn Gap Analysis",
                    Abbreviation = "LTGA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 112
                },
                new MetricType
                {
                    MetricID = 32,
                    ChartName = "Wait Time",
                    Abbreviation = "WT",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 113
                },
                new MetricType
                {
                    MetricID = 33,
                    ChartName = "Gap Vs Demand",
                    Abbreviation = "GVD",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 115
                },
                new MetricType
                {
                    MetricID = 34,
                    ChartName = "Left Turn Gap",
                    Abbreviation = "LTG",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 114
                },
                new MetricType
                {
                    MetricID = 35,
                    ChartName = "Split Monitor",
                    Abbreviation = "SM",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 120
                }

            );
            context.SaveChanges();

            foreach (var detectionType in context.DetectionTypes)
                switch (detectionType.DetectionTypeID)
                {
                    case 1:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(1));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(2));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(3));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(4));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(14));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(15));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(17));
                        break;
                    case 2:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(6));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(8));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(9));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(13));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(32));
                        break;
                    case 3:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(10));
                        break;
                    case 4:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(5));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(31));
                        break;
                    case 5:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(11));
                        break;
                    case 6:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(12));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(31));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(32));
                        break;
                }
            context.SaveChanges();

            context.VersionActions.AddOrUpdate(
                new VersionAction { ID = 1, Description = "New" },
                new VersionAction { ID = 2, Description = "Edit" },
                new VersionAction { ID = 3, Description = "Delete" },
                new VersionAction { ID = 4, Description = "New Version" },
                new VersionAction { ID = 10, Description = "Initial" }
            );

            context.MetricsFilterTypes.AddOrUpdate(
                c => c.FilterName,
                new MetricsFilterType { FilterName = "Signal ID" },
                new MetricsFilterType { FilterName = "Primary Name" },
                new MetricsFilterType { FilterName = "Secondary Name" },
                new MetricsFilterType { FilterName = "Agency" }
            );

            context.Applications.AddOrUpdate(
                c => c.ID,
                new Application { ID = 1, Name = "GeneralSetting" },
                new Application { ID = 2, Name = "ATSPM" },
                new Application { ID = 3, Name = "SPMWatchDog" },
                new Application { ID = 4, Name = "DatabaseArchive" }
            );

            context.WatchdogApplicationSettings.AddOrUpdate(
                c => c.ApplicationID,
                new WatchDogApplicationSettings
                {
                    ApplicationID = 3,
                    ConsecutiveCount = 3,
                    DefaultEmailAddress = "SomeOne@AnEmail.address",
                    EmailServer = "send.EmailServer",
                    FromEmailAddress = "SPMWatchdog@default.com",
                    LowHitThreshold = 50,
                    MaxDegreeOfParallelism = 4,
                    MinimumRecords = 500,
                    MinPhaseTerminations = 50,
                    PercentThreshold = .9,
                    PreviousDayPMPeakEnd = 18,
                    PreviousDayPMPeakStart = 17,
                    ScanDayEndHour = 5,
                    ScanDayStartHour = 1,
                    WeekdayOnly = true,
                    MaximumPedestrianEvents = 200,
                    EmailAllErrors = false
                }
            );

            context.DatabaseArchiveSettings.AddOrUpdate(m => m.ApplicationID,
                new DatabaseArchiveSettings
                {
                    ApplicationID = 4,
                    ArchivePath = @"\\ATSPM_Backup_DataTables\tcshare2\MOEFlatFiles\",
                }
            );


            context.LaneTypes.AddOrUpdate(
                new LaneType { LaneTypeID = 1, Description = "Vehicle", Abbreviation = "V" },
                new LaneType { LaneTypeID = 2, Description = "Bike", Abbreviation = "Bike" },
                new LaneType { LaneTypeID = 3, Description = "Pedestrian", Abbreviation = "Ped" },
                new LaneType { LaneTypeID = 4, Description = "Exit", Abbreviation = "E" },
                new LaneType { LaneTypeID = 5, Description = "Light Rail Transit", Abbreviation = "LRT" },
                new LaneType { LaneTypeID = 6, Description = "Bus", Abbreviation = "Bus" },
                new LaneType { LaneTypeID = 7, Description = "High Occupancy Vehicle", Abbreviation = "HOV" }
            );

            context.SaveChanges();

            context.MovementTypes.AddOrUpdate(
                new MovementType { MovementTypeID = 1, Description = "Thru", Abbreviation = "T", DisplayOrder = 3 },
                new MovementType { MovementTypeID = 2, Description = "Right", Abbreviation = "R", DisplayOrder = 5 },
                new MovementType { MovementTypeID = 3, Description = "Left", Abbreviation = "L", DisplayOrder = 1 },
                new MovementType { MovementTypeID = 4, Description = "Thru-Right", Abbreviation = "TR", DisplayOrder = 4 },
                new MovementType { MovementTypeID = 5, Description = "Thru-Left", Abbreviation = "TL", DisplayOrder = 2 },
                new MovementType { MovementTypeID = 6, Description = "None", Abbreviation = "na", DisplayOrder = 6 }
            );
            context.SaveChanges();

            context.DirectionTypes.AddOrUpdate(
                new DirectionType
                {
                    DirectionTypeID = 1,
                    Description = "Northbound",
                    Abbreviation = "NB",
                    DisplayOrder = 3
                },
                new DirectionType
                {
                    DirectionTypeID = 2,
                    Description = "Southbound",
                    Abbreviation = "SB",
                    DisplayOrder = 4
                },
                new DirectionType
                {
                    DirectionTypeID = 3,
                    Description = "Eastbound",
                    Abbreviation = "EB",
                    DisplayOrder = 1
                },
                new DirectionType
                {
                    DirectionTypeID = 4,
                    Description = "Westbound",
                    Abbreviation = "WB",
                    DisplayOrder = 2
                },
                new DirectionType
                {
                    DirectionTypeID = 5,
                    Description = "Northeast",
                    Abbreviation = "NE",
                    DisplayOrder = 5
                },
                new DirectionType
                {
                    DirectionTypeID = 6,
                    Description = "Northwest",
                    Abbreviation = "NW",
                    DisplayOrder = 6
                },
                new DirectionType
                {
                    DirectionTypeID = 7,
                    Description = "Southeast",
                    Abbreviation = "SE",
                    DisplayOrder = 7
                },
                new DirectionType
                {
                    DirectionTypeID = 8,
                    Description = "Southwest",
                    Abbreviation = "SW",
                    DisplayOrder = 8
                }
            );

            context.Regions.AddOrUpdate(
                new Region { ID = 1, Description = "Region 1" },
                new Region { ID = 2, Description = "Region 2" },
                new Region { ID = 3, Description = "Region 3" },
                new Region { ID = 4, Description = "Region 4" },
                new Region { ID = 10, Description = "Other" }
            );

            context.Agencies.AddOrUpdate(
                new Agency { AgencyID = 1, Description = "Academics" },
                new Agency { AgencyID = 2, Description = "City Government" },
                new Agency { AgencyID = 3, Description = "Consultant" },
                new Agency { AgencyID = 4, Description = "County Government" },
                new Agency { AgencyID = 5, Description = "Federal Government" },
                new Agency { AgencyID = 6, Description = "MPO" },
                new Agency { AgencyID = 7, Description = "State Government" },
                new Agency { AgencyID = 8, Description = "Other" }
            );
            context.Actions.AddOrUpdate(
                new Action { ActionID = 1, Description = "Actuated Coord." },
                new Action { ActionID = 2, Description = "Coord On/Off" },
                new Action { ActionID = 3, Description = "Cycle Length" },
                new Action { ActionID = 4, Description = "Detector Issue" },
                new Action { ActionID = 5, Description = "Offset" },
                new Action { ActionID = 6, Description = "Sequence" },
                new Action { ActionID = 7, Description = "Time Of Day" },
                new Action { ActionID = 8, Description = "Other" },
                new Action { ActionID = 9, Description = "All-Red Interval" },
                new Action { ActionID = 10, Description = "Modeling" },
                new Action { ActionID = 11, Description = "Traffic Study" },
                new Action { ActionID = 12, Description = "Yellow Interval" },
                new Action { ActionID = 13, Description = "Force Off Type" },
                new Action { ActionID = 14, Description = "Split Adjustment" },
                new Action { ActionID = 15, Description = "Manual Command" }
            );

            context.DetectionHardwares.AddOrUpdate(
                new DetectionHardware { ID = 0, Name = "Unknown" },
                new DetectionHardware { ID = 1, Name = "Wavetronix Matrix" },
                new DetectionHardware { ID = 2, Name = "Wavetronix Advance" },
                new DetectionHardware { ID = 3, Name = "Inductive Loops" },
                new DetectionHardware { ID = 4, Name = "Sensys" },
                new DetectionHardware { ID = 5, Name = "Video" },
                new DetectionHardware { ID = 6, Name = "FLIR: Thermal Camera" }
            );

            //These are default values.  They need to be changed before the system goes into production.

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<SPMUser>(context);
            var userManager = new UserManager<SPMUser>(userStore);
            if (userManager.FindByName("DefaultAdmin@SPM.Gov") == null)
            {
                var user = new SPMUser
                {
                    UserName = "DefaultAdmin@SPM.Gov".ToLower(),
                    Email = "DefaultAdmin@SPM.Gov".ToLower()
                };
                userManager.Create(user, "L3tM3in!");
                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("User"));
                roleManager.Create(new IdentityRole("Data"));
                roleManager.Create(new IdentityRole("Technician"));
                roleManager.Create(new IdentityRole("Configuration"));
                roleManager.Create(new IdentityRole("Restricted Configuration"));
                userManager.AddToRole(user.Id, "Admin");
                userManager.AddToRole(user.Id, "User");
                userManager.AddToRole(user.Id, "Data");
                userManager.AddToRole(user.Id, "Technician");
                userManager.AddToRole(user.Id, "Configuration");
                userManager.AddToRole(user.Id, "Restricted Configuration");
            }
            else
            {
                var user = userManager.FindByName("DefaultAdmin@SPM.Gov");
                roleManager.Create(new IdentityRole("Technician"));
                roleManager.Create(new IdentityRole("Data"));
                roleManager.Create(new IdentityRole("Configuration"));
                roleManager.Create(new IdentityRole("Restricted Configuration"));
                userManager.AddToRole(user.Id, "Data");
                userManager.AddToRole(user.Id, "Technician");
                userManager.AddToRole(user.Id, "Configuration");
                userManager.AddToRole(user.Id, "Restricted Configuration");
            }

            context.SaveChanges();
        }
    }
}

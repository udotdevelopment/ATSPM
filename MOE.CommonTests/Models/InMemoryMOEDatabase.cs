using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.CommonTests.Models
{
    public class InMemoryMOEDatabase
    {
        private static readonly Random rnd = new Random();
        public List<ApplicationEvent> ApplicationEvents = new List<ApplicationEvent>();
        public List<WatchDogApplicationSettings> ApplicationSettings = new List<WatchDogApplicationSettings>();
        public List<PhaseCycleAggregation> ApproachCycleAggregations = new List<PhaseCycleAggregation>();
        public List<Approach> Approaches = new List<Approach>();
        public List<ApproachPcdAggregation> ApproachPcdAggregations = new List<ApproachPcdAggregation>();
        public List<ApproachSpeedAggregation> ApproachSpeedAggregations = new List<ApproachSpeedAggregation>();

        public List<ApproachSplitFailAggregation> ApproachSplitFailAggregations =
            new List<ApproachSplitFailAggregation>();

        public List<ApproachYellowRedActivationAggregation> ApproachYellowRedActivationAggregations
            = new List<ApproachYellowRedActivationAggregation>();



        public List<Controller_Event_Log> Controller_Event_Log = new List<Controller_Event_Log>();
        public List<ControllerType> ControllerTypes = new List<ControllerType>();
        public List<DetectionHardware> DetectionHardwares = new List<DetectionHardware>();
        public List<DetectionType> DetectionTypes = new List<DetectionType>();
        public List<Detector> Detectors = new List<Detector>();
        public List<DirectionType> DirectionTypes = new List<DirectionType>();
        public List<LaneType> LaneTypes = new List<LaneType>();
        public List<MetricType> MetricTypes = new List<MetricType>();
        public List<MovementType> MovementTypes = new List<MovementType>();
        public List<PreemptionAggregation> PreemptionAggregations = new List<PreemptionAggregation>();

        public List<PriorityAggregation> PriorityAggregations = new List<PriorityAggregation>();
        public List<DetectorEventCountAggregation> DetectorAggregations { get; set; } = new List<DetectorEventCountAggregation>();
        public List<Region> Regions = new List<Region>();
        public List<RoutePhaseDirection> RoutePhaseDirection = new List<RoutePhaseDirection>();
        public List<Route> Routes = new List<Route>();
        public List<RouteSignal> RouteSignals = new List<RouteSignal>();

        public List<Signal> Signals = new List<Signal>();
        public List<VersionAction> VersionActions = new List<VersionAction>();
        public List<DetectionTypeGraph_Detector> DetectionTypeDetectors = new List<DetectionTypeGraph_Detector>();
        public List<SignalEventCountAggregation> SignalEventCountAggregations { get; set; } = new List<SignalEventCountAggregation>();
        public List<Speed_Events> Speed_Events { get; set; } = new List<Speed_Events>();

        public void SetFilterSignal(SignalAggregationMetricOptions options)
        {
            List<FilterSignal> filterSignals = new List<FilterSignal>();
            var signal = Signals.FirstOrDefault();
            var filterSignal = new FilterSignal { SignalId = signal.SignalID, Exclude = false };
            foreach (var approach in signal.Approaches)
            {
                var filterApproach = new FilterApproach
                {
                    ApproachId = approach.ApproachID,
                    Description = String.Empty,
                    Exclude = false
                };
                filterSignal.FilterApproaches.Add(filterApproach);
                foreach (var detector in approach.Detectors)
                {
                    filterApproach.FilterDetectors.Add(new FilterDetector { Id = detector.ID, Description = String.Empty, Exclude = false });
                }
            }
            options.FilterSignals.Add(filterSignal);
            options.FilterDirections = new List<FilterDirection>();
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 0, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 1, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 2, Include = true });
            options.FilterDirections.Add(new FilterDirection { Description = "", DirectionTypeId = 3, Include = true });
            options.FilterMovements = new List<FilterMovement>();
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 0, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 1, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 2, Include = true });
            options.FilterMovements.Add(new FilterMovement { Description = "", MovementTypeId = 3, Include = true });
        }


        public InMemoryMOEDatabase()
        {
            PopulateVersionActions();
            PopulateDetectionTypes();
            PopulateMetricTypes();
            PopulateDirectionTypes();
            PopulateControllerTypes();
            PopulateMovementTypes();
            PopulateLaneTypes();
            PopulateDetectionHardware();
            PoplateMetricTypes();
            
            
        }


        public void ClearTables()
        {
            Detectors.Clear();
            Approaches.Clear();
            Signals.Clear();
            RoutePhaseDirection.Clear();
            RouteSignals.Clear();
            Routes.Clear();
            ApproachSplitFailAggregations.Clear();
            ApproachCycleAggregations.Clear();
            ApproachSpeedAggregations.Clear();
            ApproachPcdAggregations.Clear();
            ApproachSpeedAggregations.Clear();
            ApproachYellowRedActivationAggregations.Clear();
            PreemptionAggregations.Clear();
            PriorityAggregations.Clear();
            Regions.Clear();
        }

        public void PopulateRegions()
        {
            var R1 = new Region
            {
                ID = 1,
                Description = "Region 1"
            };
            var R2 = new Region
            {
                ID = 2,
                Description = "Region 2"
            };
            var R3 = new Region
            {
                ID = 3,
                Description = "Region 3"
            };
            var R4 = new Region
            {
                ID = 4,
                Description = "Region 4"
            };

            Regions.Add(R1);
            Regions.Add(R2);
            Regions.Add(R3);
            Regions.Add(R4);
        }

        public void PopulateApplicationSettings()
        {
            var record = new WatchDogApplicationSettings
            {
                ID = 1,
                ConsecutiveCount = 3,
                LowHitThreshold = 3,
                ApplicationID = 1,
                DefaultEmailAddress = "test@test.com",
                EmailServer = "test.utah.gov",
                FromEmailAddress = "sender@test.com",
                MaxDegreeOfParallelism = 5,
                MaximumPedestrianEvents = 5,
                MinPhaseTerminations = 4
            };

            ApplicationSettings.Add(record);
        }

        public void PopulatePreemptionAggregations(DateTime start, DateTime end, string signalId, int versionId)
        {
            for (var startTime = start.Date;
                startTime <= end.Date.AddHours(23).AddMinutes(59);
                startTime = startTime.AddMinutes(15))
            {
                var r = new PreemptionAggregation();
                r.SignalId = signalId;
                r.BinStartTime = startTime;
                r.PreemptNumber = rnd.Next(1, 16);
                r.PreemptServices = rnd.Next(1, 200);
                r.PreemptRequests = rnd.Next(1, 200);

                PreemptionAggregations.Add(r);
            }
        }

        public void PopulatePriorityAggregations(DateTime start, DateTime end, string signalId, int versionId)
        {
            for (var startTime = start.Date;
                startTime <= end.Date.AddHours(23).AddMinutes(59);
                startTime = startTime.AddMinutes(15))
            {
                var r = new PriorityAggregation();
                r.SignalId = signalId;
                r.BinStartTime = startTime;
                r.PriorityNumber = rnd.Next(1, 64);
                r.PriorityRequests = rnd.Next(1, 200);
                r.PriorityServiceEarlyGreen = rnd.Next(1, 200);
                r.PriorityServiceExtendedGreen = rnd.Next(1, 200);

                PriorityAggregations.Add(r);
            }
        }

        public void PopulateApproachSpeedAggregationsWithRandomRecords(DateTime start, DateTime end, Approach approach)
        {
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachSpeedAggregation();
                r.ApproachId = approach.ApproachID;
                r.BinStartTime = startTime;
                r.Speed15Th = rnd.Next(1, 5);
                r.Speed85Th = rnd.Next(1, 100);
                r.SpeedVolume = rnd.Next(1, 50);
                r.SummedSpeed = rnd.Next(1, 500);   
                ApproachSpeedAggregations.Add(r);
                if (approach.PermissivePhaseNumber != null)
                {
                    var approach2 = new ApproachSpeedAggregation();
                    approach2.ApproachId = approach.ApproachID;
                    approach2.BinStartTime = startTime;
                    approach2.Speed15Th = rnd.Next(1, 5);
                    approach2.Speed85Th = rnd.Next(1, 100);
                    approach2.SpeedVolume = rnd.Next(1, 50);
                    approach2.SummedSpeed = rnd.Next(1, 500);
                    ApproachSpeedAggregations.Add(approach2);
                }
            }
        }

        public void PopulateApproachSplitFailAggregationsWithValue3(DateTime start, DateTime end, Approach approach)
        {
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachSplitFailAggregation();
                r.ApproachId = approach.ApproachID;
                r.BinStartTime = startTime;
                r.SplitFailures = 3;
                r.IsProtectedPhase = true;
                ApproachSplitFailAggregations.Add(r);
                if (approach.PermissivePhaseNumber != null)
                {
                    var approach2 = new ApproachSplitFailAggregation();
                    approach2.ApproachId = approach.ApproachID;
                    approach2.BinStartTime = startTime;
                    approach2.SplitFailures = 3;
                    approach2.IsProtectedPhase = false;
                    ApproachSplitFailAggregations.Add(approach2);
                }
            }
        }


        public void PopulateApproachSplitFailAggregationsWithRandomRecords(DateTime start, DateTime end,
            Approach approach)
        {
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachSplitFailAggregation();
                r.ApproachId = approach.ApproachID;
                r.BinStartTime = startTime;
                r.SplitFailures = rnd.Next(1, 5);
                r.IsProtectedPhase = true;
                ApproachSplitFailAggregations.Add(r);
                if (approach.PermissivePhaseNumber != null)
                {
                    var approach2 = new ApproachSplitFailAggregation();
                    approach2.ApproachId = approach.ApproachID;
                    approach2.BinStartTime = startTime;
                    approach2.SplitFailures = rnd.Next(1, 5);
                    approach2.IsProtectedPhase = true;
                    ApproachSplitFailAggregations.Add(approach2);
                }
            }
        }

        public void PopulateApproachYellowRedActivationAggregations(DateTime start, DateTime end, int approachId)
        {
            var approach = this.Approaches.Where(a => a.ApproachID == approachId).FirstOrDefault();
            for (var startTime = start.Date;
                startTime <= end.Date.AddHours(23).AddMinutes(59);
                startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachYellowRedActivationAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.SevereRedLightViolations = rnd.Next(0, 2);
                r.TotalRedLightViolations = rnd.Next(0, 5);
                r.IsProtectedPhase = true;

                ApproachYellowRedActivationAggregations.Add(r);
                if (approach.PermissivePhaseNumber != null)
                {
                    var approach2 = new ApproachYellowRedActivationAggregation();
                    approach2.ApproachId = approach.ApproachID;
                    approach2.BinStartTime = startTime;
                    approach2.SevereRedLightViolations = rnd.Next(1, 5);
                    approach2.TotalRedLightViolations = rnd.Next(1, 5);
                    approach2.IsProtectedPhase = true;
                    ApproachYellowRedActivationAggregations.Add(approach2);
                }
            }
        }


        public void PopulateApproachSpeedAggregations(DateTime start, DateTime end, int approachId)
        {
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachSpeedAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.Speed15Th = rnd.Next(10, 20);
                r.Speed85Th = rnd.Next(40, 60);
                r.SpeedVolume = rnd.Next(1, 200);
                r.SummedSpeed = rnd.Next(1, 200);

                ApproachSpeedAggregations.Add(r);
            }
        }

        public void PopulateApproachCycleAggregations(DateTime start, DateTime end, int approachId, int phaseNumber)
        {
            for (var startTime = start.Date; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new PhaseCycleAggregation
                {
                    ApproachId = approachId,
                    BinStartTime = startTime,
                    GreenTime = rnd.Next(1, 200),
                    PhaseNumber = phaseNumber,
                    RedTime = rnd.Next(1, 200),
                    TotalGreenToGreenCycles = rnd.Next(1, 15),
                    TotalRedToRedCycles = rnd.Next(1, 15),
                    YellowTime = rnd.Next(1, 200)
                };

                ApproachCycleAggregations.Add(r);
            }
        }

        public void PopulateApproachPcdAggregations(DateTime start, DateTime end, int approachId)
        {
            for (var startTime = start.Date; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new ApproachPcdAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.ArrivalsOnGreen = rnd.Next(1, 200);
                r.ArrivalsOnRed = rnd.Next(1, 200);
                r.ArrivalsOnYellow = rnd.Next(1, 200);
                r.IsProtectedPhase = true;

                ApproachPcdAggregations.Add(r);
            }
        }

        public void PopulateRoutes()
        {
            for (var i = 1; i < 4; i++)
            {
                var r = new Route();
                r.Id = i;
                r.RouteName = "Route - " + i;
                Routes.Add(r);
            }
        }

        public void PopulateRouteWithRouteSignals()
        {
            if (Signals.Count < 1)
            {
                PopulateSignal();
                PopulateSignalsWithApproaches();
                PopulateApproachesWithDetectors();
            }

            foreach (var r in Routes)
            {
                var rsid = 1;
                r.RouteSignals = new List<RouteSignal>();
                var signals = (from s in Signals where s.RegionID == r.Id select s).ToList();
                var order = 1;
                foreach (var signal in signals)
                {
                    var rs = new RouteSignal();
                    rs.Order = order;
                    rs.RouteId = r.Id;
                    rs.SignalId = signal.SignalID;
                    order++;
                    rs.Id = rsid;
                    RouteSignals.Add(rs);
                    r.RouteSignals.Add(rs);
                    rsid++;
                }
            }
        }

        public void PopulateRouteSignalsWithPhaseDirection()
        {
            var pdid = 1;
            foreach (var rs in RouteSignals)
            {
                var rpd = new RoutePhaseDirection();
                for (var i = 1; i < 4; i++)
                {
                    rpd.DirectionTypeId = i;
                    rpd.IsOverlap = false;
                    rpd.RouteSignalId = rs.Id;
                    switch (i)
                    {
                        case 1:
                            rpd.IsPrimaryApproach = true;
                            rpd.IsOverlap = false;
                            rpd.Phase = 2;
                            break;
                        case 2:
                            rpd.IsPrimaryApproach = false;
                            rpd.IsOverlap = false;
                            rpd.Phase = 4;
                            break;
                        case 3:
                            rpd.IsPrimaryApproach = false;
                            rpd.IsOverlap = false;
                            rpd.Phase = 6;
                            break;
                        case 4:
                            rpd.IsPrimaryApproach = false;
                            rpd.IsOverlap = false;
                            rpd.Phase = 8;
                            break;
                    }
                    pdid++;
                    RoutePhaseDirection.Add(rpd);
                }
            }
        }


        public void PoplateMetricTypes()
        {
            var mt1 = new MetricType
            {
                MetricID = 1,
                ChartName = "Purdue Phase Termination",
                Abbreviation = "PPT",
                ShowOnWebsite = true
                
            };
            var mt2 = new MetricType
            {
                MetricID = 2,
                ChartName = "Split Monitor",
                Abbreviation = "SM",
                ShowOnWebsite = true
            };
            var mt3 = new MetricType
            {
                MetricID = 3,
                ChartName = "Pedestrian Delay",
                Abbreviation = "PedD",
                ShowOnWebsite = true
            };
            var mt4 = new MetricType
            {
                MetricID = 4,
                ChartName = "Preemption Details",
                Abbreviation = "PD",
                ShowOnWebsite = true
            };
            var mt5 = new MetricType
            {
                MetricID = 5,
                ChartName = "Turning Movement Counts",
                Abbreviation = "TMC",
                ShowOnWebsite = true
            };
            var mt6 = new MetricType
            {
                MetricID = 6,
                ChartName = "Purdue Coordination Diagram",
                Abbreviation = "PCD",
                ShowOnWebsite = true
            };
            var mt7 = new MetricType
            {
                MetricID = 7,
                ChartName = "Approach Volume",
                Abbreviation = "AV",
                ShowOnWebsite = true
            };
            var mt8 = new MetricType
            {
                MetricID = 8,
                ChartName = "Approach Delay",
                Abbreviation = "AD",
                ShowOnWebsite = true
            };
            var mt9 = new MetricType
            {
                MetricID = 9,
                ChartName = "Arrivals On Red",
                Abbreviation = "AoR",
                ShowOnWebsite = true
            };
            var mt10 = new MetricType
            {
                MetricID = 10,
                ChartName = "Approach Speed",
                Abbreviation = "Speed",
                ShowOnWebsite = true
            };
            var mt11 = new MetricType
            {
                MetricID = 11,
                ChartName = "Yellow and Red Actuations",
                Abbreviation = "YRA",
                ShowOnWebsite = true
            };
            var mt12 = new MetricType
            {
                MetricID = 12,
                ChartName = "Purdue Split Failure",
                Abbreviation = "SF",
                ShowOnWebsite = true
            };
            var mt13 = new MetricType
            {
                MetricID = 13,
                ChartName = "Purdue Link Pivot",
                Abbreviation = "LP",
                ShowOnWebsite = false
            };
            var mt14 = new MetricType
            {
                MetricID = 14,
                ChartName = "Preempt Service Request",
                Abbreviation = "PSR",
                ShowOnWebsite = false
            };
            var mt15 = new MetricType
            {
                MetricID = 15,
                ChartName = "Preempt Service",
                Abbreviation = "PS",
                ShowOnWebsite = false
            };

            MetricTypes.Add(mt1);
            MetricTypes.Add(mt2);
            MetricTypes.Add(mt3);
            MetricTypes.Add(mt4);
            MetricTypes.Add(mt5);
            MetricTypes.Add(mt6);
            MetricTypes.Add(mt7);
            MetricTypes.Add(mt8);
            MetricTypes.Add(mt9);
            MetricTypes.Add(mt10);
            MetricTypes.Add(mt11);
            MetricTypes.Add(mt12);
            MetricTypes.Add(mt13);
            MetricTypes.Add(mt14);
            MetricTypes.Add(mt15);
        }


        public void PopulateDetectionHardware()
        {
            var dh1 = new DetectionHardware {ID = 0, Name = "Unknown"};
            var dh2 = new DetectionHardware {ID = 1, Name = "Wavetronix Matrix"};
            var dh3 = new DetectionHardware {ID = 2, Name = "Wavetronix Advance"};
            var dh4 = new DetectionHardware {ID = 3, Name = "Inductive Loops"};
            var dh5 = new DetectionHardware {ID = 4, Name = "Sensys"};
            var dh6 = new DetectionHardware {ID = 5, Name = "Video"};

            DetectionHardwares.Add(dh1);
            DetectionHardwares.Add(dh2);
            DetectionHardwares.Add(dh3);
            DetectionHardwares.Add(dh4);
            DetectionHardwares.Add(dh5);
            DetectionHardwares.Add(dh6);
        }

        public void PopulateLaneTypes()
        {
            var l1 = new LaneType {LaneTypeID = 1, Description = "Vehicle", Abbreviation = "V"};
            var l2 = new LaneType {LaneTypeID = 2, Description = "Bike", Abbreviation = "Bike"};
            var l3 = new LaneType {LaneTypeID = 3, Description = "Pedestrian", Abbreviation = "Ped"};
            var l4 = new LaneType {LaneTypeID = 4, Description = "Exit", Abbreviation = "E"};
            var l5 = new LaneType {LaneTypeID = 5, Description = "Light Rail Transit", Abbreviation = "LRT"};
            var l6 = new LaneType {LaneTypeID = 6, Description = "Bus", Abbreviation = "Bus"};
            var l7 = new LaneType {LaneTypeID = 7, Description = "High Occupancy Vehicle", Abbreviation = "HOV"};

            LaneTypes.Add(l1);
            LaneTypes.Add(l2);
            LaneTypes.Add(l3);
            LaneTypes.Add(l4);
            LaneTypes.Add(l5);
            LaneTypes.Add(l6);
            LaneTypes.Add(l7);
        }

        public void PopulateMovementTypes()
        {
            var m1 = new MovementType {MovementTypeID = 1, Description = "Thru", Abbreviation = "T", DisplayOrder = 3};
            var m2 = new MovementType {MovementTypeID = 2, Description = "Right", Abbreviation = "R", DisplayOrder = 5};
            var m3 = new MovementType {MovementTypeID = 3, Description = "Left", Abbreviation = "L", DisplayOrder = 1};
            var m4 = new MovementType
            {
                MovementTypeID = 4,
                Description = "Thru-Right",
                Abbreviation = "TR",
                DisplayOrder = 4
            };
            var m5 = new MovementType
            {
                MovementTypeID = 5,
                Description = "Thru-Left",
                Abbreviation = "TL",
                DisplayOrder = 2
            };

            MovementTypes.Add(m1);
            MovementTypes.Add(m2);
            MovementTypes.Add(m3);
            MovementTypes.Add(m4);
            MovementTypes.Add(m5);
        }

        public void PopulateControllerTypes()
        {
            var val1 = new ControllerType
            {
                ControllerTypeID = 1,
                Description = "ASC3",
                SNMPPort = 161,
                FTPDirectory = "//Set1",
                ActiveFTP = true,
                UserName = "econolite",
                Password = "ecpi2ecpi"
            };
            var val2 = new ControllerType
            {
                ControllerTypeID = 2,
                Description = "Cobalt",
                SNMPPort = 161,
                FTPDirectory = "/set1",
                ActiveFTP = true,
                UserName = "econolite",
                Password = "ecpi2ecpi"
            };
            var val3 = new ControllerType
            {
                ControllerTypeID = 3,
                Description = "ASC3 - 2070",
                SNMPPort = 161,
                FTPDirectory = "/set1",
                ActiveFTP = true,
                UserName = "econolite",
                Password = "ecpi2ecpi"
            };
            var val4 = new ControllerType
            {
                ControllerTypeID = 4,
                Description = "MaxTime",
                SNMPPort = 161,
                FTPDirectory = "none",
                ActiveFTP = false,
                UserName = "none",
                Password = "none"
            };
            var val5 = new ControllerType
            {
                ControllerTypeID = 5,
                Description = "Trafficware",
                SNMPPort = 161,
                FTPDirectory = "none",
                ActiveFTP = true,
                UserName = "none",
                Password = "none"
            };
            var val6 = new ControllerType
            {
                ControllerTypeID = 6,
                Description = "Siemens SEPAC",
                SNMPPort = 161,
                FTPDirectory = "/mnt/sd",
                ActiveFTP = false,
                UserName = "admin",
                Password = "$adm*kon2"
            };
            var val7 = new ControllerType
            {
                ControllerTypeID = 7,
                Description = "McCain ATC EX",
                SNMPPort = 161,
                FTPDirectory = " /mnt/rd/hiResData",
                ActiveFTP = false,
                UserName = "root",
                Password = "root"
            };
            var val8 = new ControllerType
            {
                ControllerTypeID = 8,
                Description = "Peek",
                SNMPPort = 161,
                FTPDirectory = "mnt/sram/cuLogging",
                ActiveFTP = false,
                UserName = "atc",
                Password = "PeekAtc"
            };

            ControllerTypes.Add(val1);
            ControllerTypes.Add(val2);
            ControllerTypes.Add(val3);
            ControllerTypes.Add(val4);
            ControllerTypes.Add(val5);
            ControllerTypes.Add(val6);
            ControllerTypes.Add(val7);
            ControllerTypes.Add(val8);
        }

        public void PopulateSignal()
        {
            for (var i = 1; i < 6; i++)
            {
                var s = new Signal();

                s.SignalID = "10" + i;
                s.VersionID = i;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i;
                s.SecondaryName = "Secondary: " + i;
                s.VersionActionId = 1;
                s.RegionID = 1;
                Signals.Add(s);
            }

            for (var i = 1; i < 6; i++)
            {
                var s = new Signal();

                s.SignalID = "20" + i;
                s.VersionID = i + 7;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i;
                s.SecondaryName = "Secondary: " + i;
                s.VersionActionId = 1;
                s.RegionID = 2;
                Signals.Add(s);
            }

            for (var i = 1; i < 6; i++)
            {
                var s = new Signal();

                s.SignalID = "30" + i;
                s.VersionID = i + 14;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i;
                s.SecondaryName = "Secondary: " + i;
                s.VersionActionId = 1;
                s.RegionID = 3;
                Signals.Add(s);
            }
        }

        public void PopulateSignalsWithApproaches()
        {
            var i = 1;
            foreach (var s in Signals)
            {
                var a = new Approach
                {
                    ApproachID = s.VersionID * 1020 + 1,
                    Description = "NB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in DirectionTypes
                        where r.Abbreviation == "NB"
                        select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 2,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                Approaches.Add(a);

                var b = new Approach
                {
                    ApproachID = s.VersionID * 1040 + 1,
                    Description = "SB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in DirectionTypes
                        where r.Abbreviation == "SB"
                        select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 4,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                Approaches.Add(b);

                var c = new Approach
                {
                    ApproachID = s.VersionID * 1060 + 1,
                    Description = "EB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in DirectionTypes
                        where r.Abbreviation == "EB"
                        select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 6,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                Approaches.Add(c);

                var d = new Approach
                {
                    ApproachID = s.VersionID * 1080 + 1,
                    Description = "WB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in DirectionTypes
                        where r.Abbreviation == "WB"
                        select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 8,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                Approaches.Add(d);

                i++;
            }
        }

        public void PopulateApproachesWithDetectors()
        {
            foreach (var signal in Signals)
            {
                signal.Approaches = (from r in Approaches
                    where r.VersionID == signal.VersionID
                    select r).ToList();

                var i = 1;
                foreach (var approach in signal.Approaches)
                {
                    var a = new Detector
                    {
                        ApproachID = approach.ApproachID,
                        ID = approach.ApproachID + i,
                        DetChannel = approach.ProtectedPhaseNumber,
                        DetectionHardwareID = 1,
                        DateAdded = DateTime.Today,
                        LaneNumber = 1,
                        MovementTypeID = 1,
                        DetectorID = approach.SignalID + "0" + approach.ProtectedPhaseNumber
                    };
                    approach.Detectors = new List<Detector>();
                    approach.Detectors.Add(a);

                    i++;
                }
            }
        }

        public void MakeMulipleVersionsOfASignalConfiguration()
        {
            var s = new Signal();

            s.SignalID = "101";
            s.VersionID = 1;
            s.Start = Convert.ToDateTime("08/15/2017");
            s.PrimaryName = "Primary: 101";
            s.SecondaryName = "Secondary: 101";
            s.Note = "Initial Setup";

            Signals.Add(s);

            AddTestDetectorToApproach(AddTestApproachToSignal(s, 1, 1), 1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s, 2, 2), 2);

            var s2 = new Signal();

            s2.SignalID = "101";
            s2.VersionID = 2;
            s2.Start = Convert.ToDateTime("09/15/2017");
            s2.PrimaryName = "Primary: 101";
            s2.SecondaryName = "Secondary: 101";
            s2.Note = "Channel Ressaignment";

            Signals.Add(s2);

            AddTestDetectorToApproach(AddTestApproachToSignal(s2, 1, 1), 1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s2, 2, 2), 3);

            var s3 = new Signal();
            s3.SignalID = "101";
            s3.VersionID = 3;
            s3.Start = Convert.ToDateTime("1/1/9999");
            s3.PrimaryName = "Primary: 101";
            s3.SecondaryName = "Secondary: 101";
            s3.Note = "New Approach";

            Signals.Add(s3);

            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 1, 1), 1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 2, 2), 2);
            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 3, 3), 3);
        }

        public void AddTestDetectorToApproach(Approach approach, int detectorChannel)
        {
            var det = new Detector();

            if (Detectors.Count > 0)
                det.ID = (from r in Detectors
                             select r.ID).Max() + 1;
            else
                det.ID = 0;

            det.DetChannel = detectorChannel;
            det.DetectorID = approach.SignalID + detectorChannel;

            det.ApproachID = approach.ApproachID;

            Detectors.Add(det);
        }

        public Approach AddTestApproachToSignal(Signal signal, int protectedPhaseNumber, int directionTypeId)
        {
            var appr = new Approach();

            appr.SignalID = signal.SignalID;

            if (Approaches.Count > 0)
                appr.ApproachID = (from r in Approaches
                                      select r.ApproachID).Max() + 1;
            else
                appr.ApproachID = 0;

            appr.VersionID = signal.VersionID;
            appr.DirectionTypeID = directionTypeId;
            appr.ProtectedPhaseNumber = protectedPhaseNumber;

            Approaches.Add(appr);
            return appr;
        }


        public void PopulateVersionActions()
        {
            var va1 = new VersionAction
            {
                ID = 1,
                Description = "New"
            };
            VersionActions.Add(va1);

            var va2 = new VersionAction
            {
                ID = 2,
                Description = "Update current version"
            };
            VersionActions.Add(va2);

            var va3 = new VersionAction
            {
                ID = 3,
                Description = "Delete"
            };
            VersionActions.Add(va3);

            var va4 = new VersionAction
            {
                ID = 4,
                Description = "Archive"
            };
            VersionActions.Add(va4);

            var va5 = new VersionAction
            {
                ID = 5,
                Description = "New version"
            };
            VersionActions.Add(va5);

            var va6 = new VersionAction
            {
                ID = 10,
                Description = "Initial"
            };
            VersionActions.Add(va6);
        }

        public void PopulateDetectionTypes()
        {
            var a = new DetectionType {DetectionTypeID = 1, Description = "Basic"};
            DetectionTypes.Add(a);
            var b = new DetectionType {DetectionTypeID = 2, Description = "Advanced Count"};
            DetectionTypes.Add(b);
            var c = new DetectionType {DetectionTypeID = 3, Description = "Advanced Speed"};
            DetectionTypes.Add(c);
            var d = new DetectionType {DetectionTypeID = 4, Description = "Lane-by-lane Count"};
            DetectionTypes.Add(d);
            var e = new DetectionType {DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction"};
            DetectionTypes.Add(e);
            var f = new DetectionType {DetectionTypeID = 6, Description = "Stop Bar Presence"};
            DetectionTypes.Add(f);
        }

        public void PopulateMetricTypes()
        {
            var b1 = new MetricType
            {
                MetricID = 1,
                ChartName = "Purdue Phase Termination",
                Abbreviation = "PPT",
                ShowOnWebsite = true
            };
            var b2 = new MetricType
            {
                MetricID = 2,
                ChartName = "Split Monitor",
                Abbreviation = "SM",
                ShowOnWebsite = true
            };
            var b3 = new MetricType
            {
                MetricID = 3,
                ChartName = "Pedestrian Delay",
                Abbreviation = "PedD",
                ShowOnWebsite = true
            };
            var b4 = new MetricType
            {
                MetricID = 4,
                ChartName = "Preemption Details",
                Abbreviation = "PD",
                ShowOnWebsite = true
            };
            var b5 = new MetricType
            {
                MetricID = 5,
                ChartName = "Turning Movement Counts",
                Abbreviation = "TMC",
                ShowOnWebsite = true
            };
            var b6 = new MetricType
            {
                MetricID = 6,
                ChartName = "Purdue Coordination Diagram",
                Abbreviation = "PCD",
                ShowOnWebsite = true
            };
            var b7 = new MetricType
            {
                MetricID = 7,
                ChartName = "Approach Volume",
                Abbreviation = "AV",
                ShowOnWebsite = true
            };
            var b8 = new MetricType
            {
                MetricID = 8,
                ChartName = "Approach Delay",
                Abbreviation = "AD",
                ShowOnWebsite = true
            };
            var b9 = new MetricType
            {
                MetricID = 9,
                ChartName = "Arrivals On Red",
                Abbreviation = "AoR",
                ShowOnWebsite = true
            };
            var b10 = new MetricType
            {
                MetricID = 10,
                ChartName = "Approach Speed",
                Abbreviation = "Speed",
                ShowOnWebsite = true
            };
            var b11 = new MetricType
            {
                MetricID = 11,
                ChartName = "Yellow and Red Actuations",
                Abbreviation = "YRA",
                ShowOnWebsite = true
            };
            var b12 = new MetricType
            {
                MetricID = 12,
                ChartName = "Purdue Split Failure",
                Abbreviation = "SF",
                
               
                ShowOnWebsite = true
            };
            var b13 = new MetricType
            {
                MetricID = 13,
                ChartName = "Purdue Link Pivot",
                Abbreviation = "LP",
                ShowOnWebsite = false
            };
            var b14 = new MetricType
            {
                MetricID = 14,
                ChartName = "Preempt Service Request",
                Abbreviation = "PSR",
                ShowOnWebsite = false
            };
            var b15 = new MetricType
            {
                MetricID = 15,
                ChartName = "Preempt Service",
                Abbreviation = "PS",
                ShowOnWebsite = false
            };

            var b16 = new MetricType
            {
                MetricID = 16,
                ChartName = "Lane by lane Aggregation",
                Abbreviation = "LLA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b17 = new MetricType
            {
                MetricID = 17,
                ChartName = "Advanced Counts Aggregation",
                Abbreviation = "ACA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b18 = new MetricType
            {
                MetricID = 18,
                ChartName = "Arrival on Green Aggregation",
                Abbreviation = "AoGA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b19 = new MetricType
            {
                MetricID = 19,
                ChartName = "Platoon Ratio Aggregation",
                Abbreviation = "PRA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b20 = new MetricType
            {
                MetricID = 20,
                ChartName = "Purdue Split Failure Aggregation",
                Abbreviation = "SFA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b21 = new MetricType
            {
                MetricID = 21,
                ChartName = "Pedestrian Actuations Aggregation",
                Abbreviation = "PedA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b22 = new MetricType
            {
                MetricID = 22,
                ChartName = "Preemption Aggregation",
                Abbreviation = "PreemptA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b23 = new MetricType
            {
                MetricID = 23,
                ChartName = "Approach Delay Aggregation",
                Abbreviation = "ADA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b24 = new MetricType
            {
                MetricID = 24,
                ChartName = "Transit Signal Priority Aggregation",
                Abbreviation = "TSPA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b25 = new MetricType
            {
                MetricID = 25,
                ChartName = "Approach Speed Aggregation",
                Abbreviation = "ASA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };

            MetricTypes.Add(b1);
            MetricTypes.Add(b2);
            MetricTypes.Add(b3);
            MetricTypes.Add(b4);
            MetricTypes.Add(b5);
            MetricTypes.Add(b6);
            MetricTypes.Add(b7);
            MetricTypes.Add(b8);
            MetricTypes.Add(b9);
            MetricTypes.Add(b10);
            MetricTypes.Add(b11);
            MetricTypes.Add(b12);
            MetricTypes.Add(b13);
            MetricTypes.Add(b14);
            MetricTypes.Add(b15);
            MetricTypes.Add(b16);
            MetricTypes.Add(b17);
            MetricTypes.Add(b18);
            MetricTypes.Add(b19);
            MetricTypes.Add(b20);
            MetricTypes.Add(b21);
            MetricTypes.Add(b22);
            MetricTypes.Add(b23);
            MetricTypes.Add(b24);
            MetricTypes.Add(b25);
        }

        public void PopulateDirectionTypes()
        {
            var b1 = new DirectionType
            {
                DirectionTypeID = 1,
                Description = "Northbound",
                Abbreviation = "NB",
                DisplayOrder = 3
            };
            var b2 = new DirectionType
            {
                DirectionTypeID = 2,
                Description = "Southbound",
                Abbreviation = "SB",
                DisplayOrder = 4
            };
            var b3 = new DirectionType
            {
                DirectionTypeID = 3,
                Description = "Eastbound",
                Abbreviation = "EB",
                DisplayOrder = 1
            };
            var b4 = new DirectionType
            {
                DirectionTypeID = 4,
                Description = "Westbound",
                Abbreviation = "WB",
                DisplayOrder = 2
            };
            var b5 = new DirectionType
            {
                DirectionTypeID = 5,
                Description = "Northeast",
                Abbreviation = "NE",
                DisplayOrder = 5
            };
            var b6 = new DirectionType
            {
                DirectionTypeID = 6,
                Description = "Northwest",
                Abbreviation = "NW",
                DisplayOrder = 6
            };
            var b7 = new DirectionType
            {
                DirectionTypeID = 7,
                Description = "Southeast",
                Abbreviation = "SE",
                DisplayOrder = 7
            };

            var b8 = new DirectionType
            {
                DirectionTypeID = 8,
                Description = "Southwest",
                Abbreviation = "SW",
                DisplayOrder = 8
            };

            DirectionTypes.Add(b1);
            DirectionTypes.Add(b2);
            DirectionTypes.Add(b3);
            DirectionTypes.Add(b4);
            DirectionTypes.Add(b5);
            DirectionTypes.Add(b6);
            DirectionTypes.Add(b7);
            DirectionTypes.Add(b8);
        }


        public void PopulatePreemptionAggregationsWithValue3(DateTime start, DateTime end, Signal signal)
        {
            var id = 1;
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new PreemptionAggregation
                {
                    BinStartTime = startTime,
                    SignalId = signal.SignalID,
                    PreemptRequests = 3,
                    PreemptNumber = 3,
                    PreemptServices = 3, 
                     
                };
                PreemptionAggregations.Add(r);
                id++;
            }
        }

        public void PopulateDetectorAggregationsWithRandomRecords(DateTime start, DateTime end, Detector detector)
        {
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new DetectorEventCountAggregation();
                r.DetectorPrimaryId = detector.ID;
                r.BinStartTime = startTime;
                r.EventCount = rnd.Next(1, 5);
                DetectorAggregations.Add(r);
            }
        }

        public void PopulatePreemptAggregations(DateTime start, DateTime end, string signalId, int versionId)
        {
            for (var startTime = start.Date;
                startTime <= end.Date.AddHours(23).AddMinutes(59);
                startTime = startTime.AddMinutes(15))
            {
                var r = new PreemptionAggregation();
                r.SignalId = signalId;
                r.BinStartTime = startTime;
                r.PreemptNumber = rnd.Next(1, 64);
                r.PreemptRequests = rnd.Next(1, 200);
                r.PreemptServices = rnd.Next(1, 200);
                PreemptionAggregations.Add(r);
            }
        }

        public void PopulateApproachCycleAggregationsWithRandomRecords(DateTime start, DateTime end, Approach approach)
        {
            for (var startTime = start.Date;
                startTime <= end.Date.AddHours(23).AddMinutes(59);
                startTime = startTime.AddMinutes(15))
            {
                var protectedPhase = new PhaseCycleAggregation
                {
                    BinStartTime = startTime,
                    GreenTime = rnd.Next(1, 64),
                    RedTime = rnd.Next(1, 64),
                    ApproachId = approach.ApproachID,
                    TotalRedToRedCycles = rnd.Next(1, 64),
                    TotalGreenToGreenCycles = rnd.Next(1, 64),
                    YellowTime = rnd.Next(1, 64)
                };
                ApproachCycleAggregations.Add(protectedPhase);
                if (approach.PermissivePhaseNumber != null)
                {
                    var permissivePhase = new PhaseCycleAggregation
                    {
                        BinStartTime = startTime,
                        GreenTime = rnd.Next(1, 64),
                        RedTime = rnd.Next(1, 64),
                        ApproachId = approach.ApproachID,
                        TotalGreenToGreenCycles = rnd.Next(1, 64),
                        TotalRedToRedCycles = rnd.Next(1, 64),
                        YellowTime = rnd.Next(1, 64)
                    };
                    ApproachCycleAggregations.Add(permissivePhase);
                }
            }
        }

        public void PopulateSignalEventCountwithRandomValues(DateTime start, DateTime end, Signal signal)
        {
            var id = 1;
            for (var startTime = start; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                var r = new SignalEventCountAggregation()
                {
                    BinStartTime = startTime,
                    SignalId = signal.SignalID, 
                    EventCount = rnd.Next(500, 1000),
                    
                };
                SignalEventCountAggregations.Add(r);
                id++;
            }
        }

       
        public void AddTestSignalForSplitFailTest()
        {
            var signal = new Signal
            {
                SignalID = "7185",
                VersionID = 1,
                Start = Convert.ToDateTime("1/1/2010"),
                PrimaryName = "Primary: ",
                SecondaryName = "Secondary: ",
                VersionActionId = 1,
                RegionID = 1
            };
            var approach = new Approach{ ApproachID = 1, SignalID = "7185", Description = "Test", DirectionTypeID = 1, IsPermissivePhaseOverlap = false, IsProtectedPhaseOverlap = false, VersionID = 1, ProtectedPhaseNumber = 0, PermissivePhaseNumber = 8};
            var detector = new Detector { DetChannel  = 44, ApproachID = 1, DateAdded = new DateTime(2010, 1,1), DetectionHardwareID = 1, DetectorID = "718544", ID = 1, MovementTypeID = 3, LaneTypeID = 1, LaneNumber = 1};
            detector.Approach = approach;
            List<MetricType> metricTypes = new List<MetricType>();
            metricTypes.Add( new MetricType { MetricID = 12, ChartName = "Split Fail" } );
        var detectionType = new DetectionType{ DetectionTypeID = 6, Description = "Stop Bar Presence", MetricTypes = metricTypes};
                detector.DetectionTypes = new List<DetectionType>();
                detector.DetectionTypes.Add(detectionType);
            signal.Approaches = new List<Approach>();
            signal.Approaches.Add(approach);
            approach.Detectors = new List<Detector>();
            approach.Detectors.Add(detector);
            approach.Signal = signal;

            Signals.Add(signal);
        }
    }
}
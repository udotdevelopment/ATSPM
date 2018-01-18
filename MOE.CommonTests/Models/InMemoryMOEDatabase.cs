using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MOE.Common;
using MOE.Common.Models;

namespace MOE.CommonTests.Models
{
    public class InMemoryMOEDatabase
    {
        private static Random rnd = new Random();

        public List<Common.Models.Signal> Signals = new List<Common.Models.Signal>();
        public List<Common.Models.Detector> Detectors = new List<Common.Models.Detector>();
        public List<Common.Models.Approach> Approaches = new List<Common.Models.Approach>();
        public List<Common.Models.VersionAction> VersionActions = new List<Common.Models.VersionAction>();
        public List<Common.Models.DetectionType> DetectionTypes = new List<Common.Models.DetectionType>();
        public List<Common.Models.DirectionType> DirectionTypes = new List<Common.Models.DirectionType>();
        public List<Common.Models.MetricType> MetricTypes = new List<Common.Models.MetricType>();
        public List<Common.Models.ControllerType> ControllerTypes = new List<Common.Models.ControllerType>();
        public List<Common.Models.MovementType> MovementTypes = new List<Common.Models.MovementType>();
        public List<Common.Models.LaneType> LaneTypes = new List<Common.Models.LaneType>();
        public List<Common.Models.DetectionHardware> DetectionHardwares = new List<Common.Models.DetectionHardware>();
        public List<Common.Models.ApplicationEvent> ApplicationEvents = new List<ApplicationEvent>();

        public List<Common.Models.ApproachCycleAggregation> ApproachCycleAggregations = new List<ApproachCycleAggregation>();
        public List<Common.Models.ApproachPcdAggregation>   ApproachPcdAggregations = new List<ApproachPcdAggregation>();
        public List<Common.Models.ApproachSpeedAggregation>     ApproachSpeedAggregations = new List<ApproachSpeedAggregation>();
        public List<Common.Models.ApproachSplitFailAggregation> ApproachSplitFailAggregations = new List<ApproachSplitFailAggregation>();
        public List<Common.Models.ApproachYellowRedActivationAggregation> ApproachYellowRedActivationAggregations
            = new List<ApproachYellowRedActivationAggregation>();
        public List<Common.Models.PreemptionAggregation> PreemptionAggregations = new List<PreemptionAggregation>();

        public List<Common.Models.PriorityAggregation> PriorityAggregations = new List<PriorityAggregation>();


        public List<Common.Models.Route> Routes = new List<Route>();
        public List<Common.Models.RouteSignal> RouteSignals = new List<RouteSignal>();
        public List<Common.Models.RoutePhaseDirection> RoutePhaseDirection = new List<RoutePhaseDirection>();


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


        }

        public void PopulatePreemptionAggregations(DateTime start, DateTime end, string signalId, int versionId)
        {
            for (DateTime startTime = start.Date; startTime <= end.Date.AddHours(23).AddMinutes(59); startTime = startTime.AddMinutes(15))
            {
                PreemptionAggregation r = new PreemptionAggregation();
                r.VersionId = versionId;
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
            for (DateTime startTime = start.Date; startTime <= end.Date.AddHours(23).AddMinutes(59); startTime = startTime.AddMinutes(15))
            {
                PriorityAggregation r = new PriorityAggregation();
                r.VersionId = versionId;
                r.SignalID = signalId;
                r.BinStartTime = startTime;
                r.PriorityNumber = rnd.Next(1, 64);
                r.PriorityRequests = rnd.Next(1, 200);
                r.PriorityServiceEarlyGreen = rnd.Next(1, 200);
                r.PriorityServiceExtendedGreen = rnd.Next(1, 200);
                r.TotalCycles = rnd.Next(1, 200);
               
                PriorityAggregations.Add(r);
            }

        }

        public void PopulateApproachSplitFailAggregations(DateTime start, DateTime end, int approachId)
        {
            
            for (DateTime startTime = start.Date; startTime <= end.Date.AddHours(23).AddMinutes(59); startTime = startTime.AddMinutes(15))
            {
                ApproachSplitFailAggregation r = new ApproachSplitFailAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.ForceOffs = rnd.Next(0, 10);
                r.GapOuts = rnd.Next(0, 10);
                r.MaxOuts = rnd.Next(0, 10);
                r.SplitFailures = rnd.Next(0, 5);
                r.UnknownTerminationTypes = rnd.Next(0, 3);
                r.IsProtectedPhase = false;

                ApproachSplitFailAggregations.Add(r);
            }


        }

        public void PopulateApproachYellowRedActivationAggregations(DateTime start, DateTime end, int approachId)
        {
            for (DateTime startTime = start.Date; startTime <= end.Date.AddHours(23).AddMinutes(59); startTime = startTime.AddMinutes(15))
            {
                ApproachYellowRedActivationAggregation r = new ApproachYellowRedActivationAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.SevereRedLightViolations = rnd.Next(0, 2);
                r.TotalRedLightViolations = rnd.Next(0, 5);
                r.IsProtectedPhase = false;

                ApproachYellowRedActivationAggregations.Add(r);
            }


        }


        public void PopulateApproachSpeedAggregations(DateTime start, DateTime end, int approachId)
        {
            for (DateTime startTime = start.Date; startTime <= end.Date.AddHours(23).AddMinutes(59); startTime = startTime.AddMinutes(15))
            {
                ApproachSpeedAggregation r = new ApproachSpeedAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.Speed15Th = rnd.Next(10, 20);
                r.Speed85Th = rnd.Next(40, 60);
                r.SpeedVolume = rnd.Next(1, 200);
                r.SummedSpeed = rnd.Next(1, 200);
              
                r.IsProtectedPhase = false;

                ApproachSpeedAggregations.Add(r);
            }


        }

        public void PopulateApproachCycleAggregations(DateTime start, DateTime end, int approachId)
        {
            for (DateTime startTime = start.Date; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                ApproachCycleAggregation r = new ApproachCycleAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.GreenTime = rnd.Next(1, 200);
                r.PedActuations = rnd.Next(1, 200);
                r.RedTime = rnd.Next(1, 200);
                r.TotalCycles = rnd.Next(1, 15);
                r.YellowTime = rnd.Next(1, 200);
                r.IsProtectedPhase = false;

                ApproachCycleAggregations.Add(r);
            }


        }

        public void PopulateApproachPcdAggregations(DateTime start, DateTime end, int approachId)
        {
            for (DateTime startTime = start.Date; startTime <= end; startTime = startTime.AddMinutes(15))
            {
                ApproachPcdAggregation r = new ApproachPcdAggregation();
                r.ApproachId = approachId;
                r.BinStartTime = startTime;
                r.ArrivalsOnGreen = rnd.Next(1, 200);
                r.ArrivalsOnRed = rnd.Next(1, 200);
                r.ArrivalsOnYellow = rnd.Next(1, 200);
                r.IsProtectedPhase = false;

                ApproachPcdAggregations.Add(r);
            }


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

        public void PopulateRoutes()
        {
            Route r = new Route();
            for(int i = 1; i<4; i++)
            {
                r.Id = i;
                r.RouteName = "Route - " + i.ToString();
                Routes.Add(r);
            }
        }

        public void PopulateRouteWithRouteSignals()
        {
            if (Signals.Count <  1)
            {
                PopulateSignal();
                PopulateSignalsWithApproaches();
                PopulateApproachesWithDetectors();
                
            }

            foreach (var r in Routes)
            {
                int rsid = 1;
                    List<Signal> signals = (from s in Signals where s.RegionID == r.Id select s).ToList();
                    int order = 1;
                    foreach (var signal in signals)
                    {
                        RouteSignal rs = new RouteSignal();
                        rs.Order = order;
                        rs.RouteId = r.Id;
                        rs.SignalId = signal.SignalID;
                        order++;
                        rs.Id = rsid;
                        RouteSignals.Add(rs);
                        rsid++;
                    }

                
            }
        }

        public void PopulateRouteSignalsWithPhaseDirection()
        {
            int pdid = 1;
            foreach (var rs in RouteSignals)
            {
                RoutePhaseDirection rpd = new RoutePhaseDirection();
                for (int i = 1; i < 4; i++)
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

        public int  PopulateApproachSplitFailAggregationsWithRandomRecords()
        {
            int appId = Approaches.FirstOrDefault().ApproachID;
            DateTime start = DateTime.Now.AddDays(-1);
            if (appId != null)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Common.Models.ApproachSplitFailAggregation asfa = new ApproachSplitFailAggregation();
                    asfa.Id = i;
                    asfa.ApproachId = appId;
                    asfa.BinStartTime = start;
                    asfa.IsProtectedPhase = false;
                    asfa.ForceOffs = rnd.Next(1, 256);
                    asfa.GapOuts = rnd.Next(1, 256);
                    asfa.MaxOuts = rnd.Next(1, 256);
                    asfa.SplitFailures = rnd.Next(1, 256);
                    asfa.UnknownTerminationTypes = rnd.Next(1, 256);
                    start = start.AddMinutes(5);
                    ApproachSplitFailAggregations.Add(asfa);

                }
            }
            return appId;
        }

        private void PoplateMetricTypes()
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
                ShowOnWebsite = true,
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
            var mt14= new MetricType
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



        private void PopulateDetectionHardware()
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

        private void PopulateLaneTypes()
        {
            var l1 =  new LaneType {LaneTypeID = 1, Description = "Vehicle", Abbreviation = "V"};
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

        private void PopulateMovementTypes()
        {
            var m1 = new MovementType {MovementTypeID = 1, Description = "Thru", Abbreviation = "T", DisplayOrder = 3};
            var m2 = new MovementType {MovementTypeID = 2, Description = "Right", Abbreviation = "R", DisplayOrder = 5};
            var m3 = new MovementType {MovementTypeID = 3, Description = "Left", Abbreviation = "L", DisplayOrder = 1};
            var m4 = new MovementType {MovementTypeID = 4, Description = "Thru-Right", Abbreviation = "TR", DisplayOrder = 4};
            var m5 = new MovementType {MovementTypeID = 5, Description = "Thru-Left", Abbreviation = "TL", DisplayOrder = 2};

            MovementTypes.Add(m1);
            MovementTypes.Add(m2);
            MovementTypes.Add(m3);
            MovementTypes.Add(m4);
            MovementTypes.Add(m5);
        }

        private void PopulateControllerTypes()
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
            for (int i = 1; i < 6; i++)
            {
                MOE.Common.Models.Signal s = new Signal();

                s.SignalID = "10" + i.ToString();
                s.VersionID = i;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i.ToString();
                s.SecondaryName = "Secondary: " + i.ToString();
                s.VersionActionId = 1;
                s.RegionID = 1;
                Signals.Add(s);

            }

            for (int i = 1; i < 6; i++)
            {
                MOE.Common.Models.Signal s = new Signal();

                s.SignalID = "20" + i.ToString();
                s.VersionID = i+7;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i.ToString();
                s.SecondaryName = "Secondary: " + i.ToString();
                s.VersionActionId = 1;
                s.RegionID = 2;
                Signals.Add(s);

            }

            for (int i = 1; i < 6; i++)
            {
                MOE.Common.Models.Signal s = new Signal();

                s.SignalID = "30" + i.ToString();
                s.VersionID = i+14;
                s.Start = Convert.ToDateTime("1/1/2010");
                s.PrimaryName = "Primary: " + i.ToString();
                s.SecondaryName = "Secondary: " + i.ToString();
                s.VersionActionId = 1;
                s.RegionID = 3;
                Signals.Add(s);

            }

        }

        public void PopulateSignalsWithApproaches()
        {
            int i = 1;
            foreach (var s in Signals)
            {
                Approach a = new Approach
                {
                    ApproachID = (s.VersionID * 1020) + 1,
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

                Approach b = new Approach
                {
                    ApproachID = (s.VersionID * 1040) + 1,
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

                Approach c = new Approach
                {
                    ApproachID = (s.VersionID * 1060) + 1,
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

                Approach d = new Approach
                {
                    ApproachID = (s.VersionID * 1080) + 1,
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

                int i = 1;
                foreach (var appr in signal.Approaches)
                {
                    
                    MOE.Common.Models.Detector a = new Detector()
                    {
                        ApproachID = appr.ApproachID,
                        ID = appr.ApproachID+i,
                        DetChannel = appr.ProtectedPhaseNumber,
                        DetectionHardwareID = 1,
                        DateAdded = DateTime.Today,
                        LaneNumber = 1,
                        MovementTypeID = 1,
                        DetectorID = appr.SignalID + "0" + appr.ProtectedPhaseNumber.ToString(),
                    };

                    Detectors.Add(a);


                    i++;
                }
            }
        }

        public void MakeMulipleVersionsOfASignalConfiguration()
        {
            MOE.Common.Models.Signal s = new Signal();

            s.SignalID = "101";
            s.VersionID = 1;
            s.Start = Convert.ToDateTime("08/15/2017");
            s.PrimaryName = "Primary: 101" ;
            s.SecondaryName = "Secondary: 101" ;
            s.Note = "Initial Setup";

            Signals.Add(s);

            AddTestDetectorToApproach(AddTestApproachToSignal(s, 1, 1),1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s, 2, 2),2);

            MOE.Common.Models.Signal s2 = new Signal();

            s2.SignalID = "101";
            s2.VersionID = 2;
            s2.Start = Convert.ToDateTime("09/15/2017");
            s2.PrimaryName = "Primary: 101";
            s2.SecondaryName = "Secondary: 101";
            s2.Note = "Channel Ressaignment";

            Signals.Add(s2);

            AddTestDetectorToApproach(AddTestApproachToSignal(s2, 1, 1), 1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s2, 2, 2), 3);

            MOE.Common.Models.Signal s3 = new Signal();
            s3.SignalID = "101";
            s3.VersionID = 3;
            s3.Start = Convert.ToDateTime("1/1/9999");
            s3.PrimaryName = "Primary: 101";
            s3.SecondaryName = "Secondary: 101";
            s3.Note = "New Approach";

            Signals.Add(s3);

            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 1, 1),1);
            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 2, 2),2);
            AddTestDetectorToApproach(AddTestApproachToSignal(s3, 3, 3),3);
           


        }

        public void AddTestDetectorToApproach(Approach approach, int detectorChannel)
        {
            MOE.Common.Models.Detector det = new MOE.Common.Models.Detector();

            if (Detectors.Count > 0)
            {
                det.ID = (from r in Detectors
                          select r.ID).Max() + 1;
            }
            else
            {
                det.ID = 0;
            }

            det.DetChannel = detectorChannel;
            det.DetectorID = approach.SignalID + detectorChannel;




            det.ApproachID = approach.ApproachID;

            Detectors.Add(det);
        }

        public MOE.Common.Models.Approach AddTestApproachToSignal(Common.Models.Signal signal, int protectedPhaseNumber, int directionTypeId)
        {
            MOE.Common.Models.Approach appr = new MOE.Common.Models.Approach();

            appr.SignalID = signal.SignalID;

            if (Approaches.Count > 0)
            {
                appr.ApproachID = (from r in Approaches
                                      select r.ApproachID).Max() + 1;
            }
            else
            {
                appr.ApproachID = 0;
            }

            appr.VersionID = signal.VersionID;
            appr.DirectionTypeID = directionTypeId;
            appr.ProtectedPhaseNumber = protectedPhaseNumber;

            Approaches.Add(appr);
            return(appr);
        }

        



        public void PopulateVersionActions()
        {
            var va1 = new Common.Models.VersionAction
            {
                ID = 1,
                Description = "New"
            };
            VersionActions.Add(va1);

            var va2 = new Common.Models.VersionAction
            {
                ID = 2,
                Description = "Update current version"
            };
            VersionActions.Add(va2);

            var va3 = new Common.Models.VersionAction
            {
                ID = 3,
                Description = "Delete"
            };
            VersionActions.Add(va3);

            var va4 = new Common.Models.VersionAction
            {
                ID = 4,
                Description = "Archive"
            };
            VersionActions.Add(va4);

            var va5 = new Common.Models.VersionAction
            {
                ID = 5,
                Description = "New version"
            };
            VersionActions.Add(va5);

            var va6 = new Common.Models.VersionAction
            {
                ID = 10,
                Description = "Initial"
            };
            VersionActions.Add(va6);
        }

        public void PopulateDetectionTypes()
        {

            var a = new Common.Models.DetectionType { DetectionTypeID = 1, Description = "Basic" };
            DetectionTypes.Add(a);
            var b = new Common.Models.DetectionType { DetectionTypeID = 2, Description = "Advanced Count" };
            DetectionTypes.Add(b);
            var c = new Common.Models.DetectionType { DetectionTypeID = 3, Description = "Advanced Speed" };
            DetectionTypes.Add(c);
            var d = new Common.Models.DetectionType { DetectionTypeID = 4, Description = "Lane-by-lane Count" };
            DetectionTypes.Add(d);
            var e = new Common.Models.DetectionType { DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction" };
            DetectionTypes.Add(e);
            var f = new Common.Models.DetectionType { DetectionTypeID = 6, Description = "Stop Bar Presence" };
            DetectionTypes.Add(f);
        }

        public void PopulateMetricTypes()
        {

            var b1 = new MOE.Common.Models.MetricType { MetricID = 1, ChartName = "Purdue Phase Termination", Abbreviation = "PPT", ShowOnWebsite = true };
            var b2 = new MOE.Common.Models.MetricType { MetricID = 2, ChartName = "Split Monitor", Abbreviation = "SM", ShowOnWebsite = true };
            var b3 = new MOE.Common.Models.MetricType { MetricID = 3, ChartName = "Pedestrian Delay", Abbreviation = "PedD", ShowOnWebsite = true };
            var b4 = new MOE.Common.Models.MetricType { MetricID = 4, ChartName = "Preemption Details", Abbreviation = "PD", ShowOnWebsite = true };
            var b5 = new MOE.Common.Models.MetricType { MetricID = 5, ChartName = "Turning Movement Counts", Abbreviation = "TMC", ShowOnWebsite = true };
            var b6 = new MOE.Common.Models.MetricType { MetricID = 6, ChartName = "Purdue Coordination Diagram", Abbreviation = "PCD", ShowOnWebsite = true };
            var b7 = new MOE.Common.Models.MetricType { MetricID = 7, ChartName = "Approach Volume", Abbreviation = "AV", ShowOnWebsite = true };
            var b8 = new MOE.Common.Models.MetricType { MetricID = 8, ChartName = "Approach Delay", Abbreviation = "AD", ShowOnWebsite = true };
            var b9 = new MOE.Common.Models.MetricType { MetricID = 9, ChartName = "Arrivals On Red", Abbreviation = "AoR", ShowOnWebsite = true, };
            var b10 = new MOE.Common.Models.MetricType { MetricID = 10, ChartName = "Approach Speed", Abbreviation = "Speed", ShowOnWebsite = true };
            var b11 = new MOE.Common.Models.MetricType { MetricID = 11, ChartName = "Yellow and Red Actuations", Abbreviation = "YRA", ShowOnWebsite = true };
            var b12 = new MOE.Common.Models.MetricType { MetricID = 12, ChartName = "Purdue Split Failure", Abbreviation = "SF", ShowOnWebsite = true };
            var b13 = new MOE.Common.Models.MetricType { MetricID = 13, ChartName = "Purdue Link Pivot", Abbreviation = "LP", ShowOnWebsite = false };
            var b14 = new MOE.Common.Models.MetricType { MetricID = 14, ChartName = "Preempt Service Request", Abbreviation = "PSR", ShowOnWebsite = false };
            var b15 = new MOE.Common.Models.MetricType
            {
                MetricID = 15,
                ChartName = "Preempt Service",
                Abbreviation = "PS",
                ShowOnWebsite = false
            };

            var b16 = new MOE.Common.Models.MetricType
            {
                MetricID = 16,
                ChartName = "Lane by lane Aggregation",
                Abbreviation = "LLA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b17 = new MOE.Common.Models.MetricType
            {
                MetricID = 17,
                ChartName = "Advanced Counts Aggregation",
                Abbreviation = "ACA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b18 = new MOE.Common.Models.MetricType
            {
                MetricID = 18,
                ChartName = "Arrival on Green Aggregation",
                Abbreviation = "AoGA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b19 = new MOE.Common.Models.MetricType
            {
                MetricID = 19,
                ChartName = "Platoon Ratio Aggregation",
                Abbreviation = "PRA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b20 = new MOE.Common.Models.MetricType
            {
                MetricID = 20,
                ChartName = "Purdue Split Failure Aggregation",
                Abbreviation = "SFA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b21 = new MOE.Common.Models.MetricType
            {
                MetricID = 21,
                ChartName = "Pedestrian Actuations Aggregation",
                Abbreviation = "PedA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b22 = new MOE.Common.Models.MetricType
            {
                MetricID = 22,
                ChartName = "Preemption Aggregation",
                Abbreviation = "PreemptA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b23 = new MOE.Common.Models.MetricType
            {
                MetricID = 23,
                ChartName = "Approach Delay Aggregation",
                Abbreviation = "ADA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b24 = new MOE.Common.Models.MetricType
            {
                MetricID = 24,
                ChartName = "Transit Signal Priority Aggregation",
                Abbreviation = "TSPA",
                ShowOnWebsite = false,
                ShowOnAggregationSite = true
            };
            var b25 = new MOE.Common.Models.MetricType
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
            var b1 = new Common.Models.DirectionType
            {
                DirectionTypeID = 1,
                Description = "Northbound",
                Abbreviation = "NB",
                DisplayOrder = 3
            };
            var b2 = new Common.Models.DirectionType
            {
                DirectionTypeID = 2,
                Description = "Southbound",
                Abbreviation = "SB",
                DisplayOrder = 4
            };
            var b3 = new Common.Models.DirectionType
            {
                DirectionTypeID = 3,
                Description = "Eastbound",
                Abbreviation = "EB",
                DisplayOrder = 1
            };
            var b4 = new Common.Models.DirectionType
            {
                DirectionTypeID = 4,
                Description = "Westbound",
                Abbreviation = "WB",
                DisplayOrder = 2
            };
            var b5 = new Common.Models.DirectionType
            {
                DirectionTypeID = 5,
                Description = "Northeast",
                Abbreviation = "NE",
                DisplayOrder = 5
            };
            var b6 = new Common.Models.DirectionType
            {
                DirectionTypeID = 6,
                Description = "Northwest",
                Abbreviation = "NW",
                DisplayOrder = 6
            };
            var b7 = new Common.Models.DirectionType
            {
                DirectionTypeID = 7,
                Description = "Southeast",
                Abbreviation = "SE",
                DisplayOrder = 7
            };

            var b8 = new Common.Models.DirectionType
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

        

    }
}

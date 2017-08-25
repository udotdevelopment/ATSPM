using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MOE.Common.Business.SiteSecurity;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;



namespace MOE.Common.Business
{
    public class Converter
    {
        public MOE.Common.Data.Signals.SignalsDataTable SignalsTable { get; set; }
        public MOE.Common.Data.Signals.Graph_DetectorsDataTable DetectorsTable { get; set; }
        public MOE.Common.Data.Signals.SPM_CommentDataTable CommentsTable { get; set; }
        public MOE.Common.Data.Signals.ApproachRouteDetailDataTable RouteDetailTable { get; set; }
        public MOE.Common.Data.Signals.ApproachRouteDataTable RouteTable { get; set; }
        public MOE.Common.Data.Action.ActionLog_DisabledDataTable ActionLogTable { get; set; }
        public MOE.Common.Data.Action.Metric_ListDataTable MetricList { get; set; }
        public MOE.Common.Data.Action.Action_Log_MetricsDataTable ActionLogMetrics { get; set; }
        public MOE.Common.Data.Action.AgenciesDataTable ActionAgencies { get; set; }
        public MOE.Common.Data.Action.ActionsListDataTable ActionsList { get; set; }
        public MOE.Common.Data.Action.Action_Log_ActionsDataTable ActionLogActions { get; set; }
        public MOE.Common.Data.Settings.MOE_UsersDataTable OldUsersTable { get; set; }

        public List<MOE.Common.Models.Signal> SignalsList { get; set; }
        public List<MOE.Common.Models.Detector> DetectorsList { get; set; }
        public List<Models.DetectionType> DetectionsTypes { get; set; }
        public List<DetectorConverter> DetectorConverters { get; set; }


        private Models.SPM db = null;

        public Converter()
        {
            GetRecords();
            UpdateToNewDatabaseModel();
            //db.Database.Initialize(true);
            GetSignalsList();            
            SetApproaches();
            SetNonLaneDetectors();
            SaveSignalsToDB();
            ConvertApproachDetailDirections();
            ConvertActionTakenEntries();
            ConvertUsers();
        }





        public void UpdateToNewDatabaseModel()
        {
            Console.WriteLine("Updating to latest migration");        
            var config = new MOE.Common.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            migrator.Update();            
        }

        private void PrintDates()
        {
            foreach (var signal in SignalsList)
            {
                if (signal.Approaches != null)
                {
                    foreach (var approach in signal.Approaches)
                    {
                        if (approach.Detectors != null)
                        {
                            foreach (var detector in approach.Detectors)
                            {
                                Console.WriteLine(signal.SignalID + "-" + detector.DateAdded.ToString());
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
        }



        private void SetNonLaneDetectors()
        {
            List<Models.DirectionType> directions = db.DirectionTypes.ToList();
            var detectionstypeslist = db.DetectionTypes.ToList();
            foreach (MOE.Common.Data.Signals.Graph_DetectorsRow row in DetectorsTable)
            {
                if (row.Det_Channel > 0)
                {
                    var detector = CreateNewDetector(row, detectionstypeslist);
                    var signal = SignalsList.Where(s => s.SignalID == row.SignalID).FirstOrDefault();
                    if (signal != null)
                    {
                        var approach = signal.Approaches
                            .Where(a => a.DirectionType.Description == row.Direction && a.ProtectedPhaseNumber == Convert.ToInt32(row.Phase))
                            .FirstOrDefault();
                        if (approach == null)
                        {
                            approach = CreateNewApproach(row, directions);
                            signal.Approaches.Add(approach);
                        }
                        if (approach.Detectors == null)
                        {
                            approach.Detectors = new List<Models.Detector>();
                        }
                        approach.Detectors.Add(detector);
                    }
                }
            }
        }

        private Models.Detector CreateNewDetector(Data.Signals.Graph_DetectorsRow row, List<Models.DetectionType> detectionstypeslist)
        {
            Models.Detector detector = new Models.Detector();
            detector.DetectorID = row.DetectorID;
            detector.DetChannel = row.Det_Channel;
            if (!row.IsNull("Date_Added"))
            {
                detector.DateAdded = row.Date_Added;
            }
            else
            {
                detector.DateAdded = DateTime.Now;
            }
            if (detector.DetectionTypes == null && detector.DetectionTypeIDs == null)
            {
                detector.DetectionTypeIDs = new List<int>();
                detector.DetectionTypes = new List<MOE.Common.Models.DetectionType>();
            }

            GetDetectionTypes(row, detector, detectionstypeslist);

            if (row.DistanceFromStopBar > 0)
            {
                detector.DistanceFromStopBar = row.DistanceFromStopBar;
            }
            if (row.Min_Speed_Filter > 0)
            {
                detector.MinSpeedFilter = row.Min_Speed_Filter;
            }
            if (!String.IsNullOrEmpty(row.Lane) && (String.Compare(row.Lane.ToUpper(), "NULL") != 0))
            {
                detector.LaneNumber = Convert.ToInt32(row.Lane);
            }

            if (row.Decision_Point > 0)
            {
                detector.DecisionPoint = row.Decision_Point;
            }
            if (row.Movement_Delay > 0)
            {
                detector.MovementDelay = row.Movement_Delay;
            }
            if (!String.IsNullOrEmpty(row.Comments))
            {
                Models.DetectorComment comment = new Models.DetectorComment();
                comment.CommentText = row.Comments;
                comment.TimeStamp = DateTime.Now;
                detector.DetectorComments = new List<Models.DetectorComment>();
                detector.DetectorComments.Add(comment);
            }

            if (row.TMC_Lane_Type != null)
            {
                SetLaneInfo(row, detector);
            }
          

            return detector;
        }

        private void SetLaneInfo(Data.Signals.Graph_DetectorsRow row, Models.Detector detector)
        {
            switch (row.TMC_Lane_Type)
            {

                case "L1":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 3;
                    break;
                case "L2":
                    detector.LaneNumber = 2;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 3;
                    break;
                case "L3":
                    detector.LaneNumber = 3;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 3;
                    break;
                case "L4":
                    detector.LaneNumber = 4;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 3;
                    break;
                case "R1":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 2;
                    break;
                case "R2":
                    detector.LaneNumber = 2;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 2;
                    break;
                case "R3":
                    detector.LaneNumber = 3;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 2;
                    break;
                case "R4":
                    detector.LaneNumber = 4;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 2;
                    break;
                case "T1":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 1;
                    break;
                case "T2":
                    detector.LaneNumber = 2;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 1;
                    break;
                case "T3":
                    detector.LaneNumber = 3;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 1;
                    break;
                case "T4":
                    detector.LaneNumber = 4;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 1;
                    break;
                case "TL1":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 5;
                    break;
                case "TR1":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 1;
                    detector.MovementTypeID = 4;
                    break;
                case "T1E":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 4;
                    detector.MovementTypeID = 1;
                    break;
                case "T2E":
                    detector.LaneNumber = 2;
                    detector.LaneTypeID = 4;
                    detector.MovementTypeID = 1;
                    break;
                case "T3E":
                    detector.LaneNumber = 3;
                    detector.LaneTypeID = 4;
                    detector.MovementTypeID = 1;
                    break;
                case "T4E":
                    detector.LaneNumber = 4;
                    detector.LaneTypeID = 4;
                    detector.MovementTypeID = 1;
                    break;
                case "L1B":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 2;
                    detector.MovementTypeID = 3;
                    break;
                case "T1B":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 2;
                    detector.MovementTypeID = 1;
                    break;
                case "R1B":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 2;
                    detector.MovementTypeID = 2;
                    break;
                case "PED":
                    detector.LaneNumber = 1;
                    detector.LaneTypeID = 3;
                    detector.MovementTypeID = 1;
                    break;
            }

            if (detector.LaneNumber == null)
                    {
                       
                        if (!String.IsNullOrEmpty(row.Lane) && (String.Compare(row.Lane.ToUpper(), "NULL") != 0))
                        {
                            detector.LaneNumber = Convert.ToInt32(row.Lane);
                            detector.LaneTypeID = 1;
                            detector.MovementTypeID = 1;
                        }


                        if (detector.LaneNumber == null)
                        {
                            detector.LaneNumber = 1;
                            detector.LaneTypeID = 1;
                            detector.MovementTypeID = 1;
                        }
                        
                    }



        }

        private void GetDetectionTypes(Data.Signals.Graph_DetectorsRow row, Models.Detector detector, List<Models.DetectionType> detectionstypeslist)
        {
            GetDetectionType(row.Has_PCD, 2, detector, detectionstypeslist);
            GetDetectionType(row.Has_Speed_Detector, 3, detector, detectionstypeslist);
            GetDetectionType(row.Has_SplitFail, 6, detector, detectionstypeslist);
            GetDetectionType(row.Has_RLM, 5, detector, detectionstypeslist);
            GetDetectionType(row.Has_TMC, 4, detector, detectionstypeslist);
        }

        private void GetDetectionType(bool p1, int p2, Models.Detector detector, List<Models.DetectionType> detectionstypeslist)
        {
            if (p1)
            {
                MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                           where r.DetectionTypeID == p2
                                                           select r).FirstOrDefault();

                detector.DetectionTypes.Add(dettype);
              
            }
        }


        private void SetApproaches()
        {
            Console.WriteLine("Setting up New Approaches");


            List<Models.DirectionType> directions = (from r in db.DirectionTypes
                                                        select r).ToList();
            foreach (MOE.Common.Data.Signals.Graph_DetectorsRow row in DetectorsTable)
            {
                var signal = SignalsList.Where(s => s.SignalID == row.SignalID).First();
                if (signal.Approaches == null)
                {
                    signal.Approaches = new List<MOE.Common.Models.Approach>();
                }
                Models.Approach approach;
                if (row.Phase == "0")
                {
                    approach = signal.Approaches
                        .Where(s => s.DirectionType.Description == row.Direction)
                        .FirstOrDefault();
                }
                else
                {
                    approach = signal.Approaches
                        .Where(s => s.DirectionType.Description == row.Direction
                            && s.ProtectedPhaseNumber == Convert.ToInt32(row.Phase))
                        .FirstOrDefault();
                }

                if (approach == null)
                {
                    signal.Approaches.Add(CreateNewApproach(row, directions));
                }
                else if (approach.ProtectedPhaseNumber == 0 && row.Phase != "0")
                {
                    approach.ProtectedPhaseNumber = Convert.ToInt32(row.Phase);
                    approach.Description = approach.DirectionType.Description +
                        " Phase " + approach.ProtectedPhaseNumber.ToString();
                }
                else if (approach.ProtectedPhaseNumber != 0 && row.Phase != "0" &&
                    approach.ProtectedPhaseNumber != Convert.ToInt32(row.Phase))
                {
                    signal.Approaches.Add(CreateNewApproach(row, directions));
                }

                foreach (Models.DirectionType direction in directions)
                {
                    int maxMPH = (from r in signal.Approaches
                                  where r.DirectionTypeID == direction.DirectionTypeID
                                  select r.MPH).Max() ?? 0;

                    foreach (Models.Approach a in signal.Approaches)
                    {
                        if (a.DirectionTypeID == direction.DirectionTypeID)
                        {
                            a.MPH = maxMPH;
                        }
                    }
                }
            }
        }

        private Models.Approach CreateNewApproach(Data.Signals.Graph_DetectorsRow row,
            List<Models.DirectionType> directions)
        {
            MOE.Common.Models.Approach newApproach = new Models.Approach();
            newApproach.SignalID = row.SignalID;
            newApproach.ProtectedPhaseNumber = Convert.ToInt32(row.Phase);
            newApproach.DirectionType = directions.Where(d => d.Description == row.Direction).First();
            if (Convert.ToInt32(row.Phase) == 0)
            {
                newApproach.Description = newApproach.DirectionType.Description + " (Phase Only)";
            }
            else
            {
                newApproach.Description = newApproach.DirectionType.Description + " Phase " + newApproach.ProtectedPhaseNumber.ToString();
            }
            if (row.MPH > 0)
            {
                newApproach.MPH = row.MPH;
            }
            newApproach.IsProtectedPhaseOverlap = row.Is_Overlap;
            if (Convert.ToInt32(row.Perm_Phase) > 0)
            {
                newApproach.PermissivePhaseNumber = Convert.ToInt32(row.Perm_Phase);
            }
            return newApproach;
        }


        public void GetRecords()
        {
            Console.WriteLine("Getting records from old database tables.");
            MOE.Common.Data.SignalsTableAdapters.SignalsTableAdapter sta = new Data.SignalsTableAdapters.SignalsTableAdapter();
            
            SignalsTable = sta.GetData();

            MOE.Common.Data.SignalsTableAdapters.Graph_DetectorsTableAdapter gta = new Data.SignalsTableAdapters.Graph_DetectorsTableAdapter();
            DetectorsTable = gta.GetData();

            MOE.Common.Data.SignalsTableAdapters.SPM_CommentTableAdapter cta = new Data.SignalsTableAdapters.SPM_CommentTableAdapter();
            CommentsTable = cta.GetAllData();

            MOE.Common.Data.SignalsTableAdapters.ApproachRouteDetailTableAdapter rta = new Data.SignalsTableAdapters.ApproachRouteDetailTableAdapter();
            RouteDetailTable = rta.GetData();

            MOE.Common.Data.SignalsTableAdapters.ApproachRouteTableAdapter arAdapter = new Data.SignalsTableAdapters.ApproachRouteTableAdapter();
            RouteTable = arAdapter.GetData();

            MOE.Common.Data.ActionTableAdapters.ActionLog_DisabledTableAdapter alta = new Data.ActionTableAdapters.ActionLog_DisabledTableAdapter();

            ActionLogTable = alta.GetData();


            MOE.Common.Data.ActionTableAdapters.Metric_ListTableAdapter mta = new Data.ActionTableAdapters.Metric_ListTableAdapter();
            MetricList = mta.GetData();

            MOE.Common.Data.ActionTableAdapters.AgenciesTableAdapter ata = new MOE.Common.Data.ActionTableAdapters.AgenciesTableAdapter();
            ActionAgencies = ata.GetData();

            MOE.Common.Data.ActionTableAdapters.Action_Log_MetricsTableAdapter almt = new Data.ActionTableAdapters.Action_Log_MetricsTableAdapter();
            ActionLogMetrics = almt.GetData();

            MOE.Common.Data.ActionTableAdapters.ActionsListTableAdapter actionsTA = new Data.ActionTableAdapters.ActionsListTableAdapter();
            ActionsList = actionsTA.GetData();

            MOE.Common.Data.ActionTableAdapters.Action_Log_ActionsTableAdapter actionlogactionsta = new Data.ActionTableAdapters.Action_Log_ActionsTableAdapter();
            ActionLogActions = actionlogactionsta.GetData();

            MOE.Common.Data.SettingsTableAdapters.MOE_UsersTableAdapter usersTA = new MOE.Common.Data.SettingsTableAdapters.MOE_UsersTableAdapter();
            OldUsersTable = usersTA.GetData();

            db = new Models.SPM();

        }

        public void GetSignalsList()
        {
            Console.WriteLine("Getting old signals list");
            SignalsList = new List<Models.Signal>();

            foreach (MOE.Common.Data.Signals.SignalsRow row in SignalsTable)
            {
                Models.Signal s = new Models.Signal();
                s.SignalID = row.SignalID;
                s.PrimaryName = row.Primary_Name;
                s.SecondaryName = row.Secondary_Name;
                if (!row.IsNull("IP_Address") && row.IP_Address != "")
                {
                    s.IPAddress = row.IP_Address;
                }
                else
                {
                    s.IPAddress = "10.10.10.10";
                }
                s.Latitude = row.Latitude;
                s.Longitude = row.Longitude;
                s.RegionID = Convert.ToInt32(row.Region);
                s.ControllerTypeID = row.Controller_Type;
                s.Enabled = true;
                SignalsList.Add(s);
            }



        }



        public void GetDetectorList()
        {
            Console.WriteLine("Converting Detectors");
           
            DetectorConverters = new List<DetectorConverter>();

            foreach (MOE.Common.Data.Signals.Graph_DetectorsRow row in DetectorsTable)
            {
                List<MOE.Common.Models.DetectionType> detectionstypeslist = (from r in db.DetectionTypes
                                                                             select r).ToList();

                if (!row.IsNull("Phase") || row.Phase != "0" || row.Phase != "" || row.Det_Channel != 0)
                {
                    DetectorConverter dc = new DetectorConverter();

                    Models.Detector d = new Models.Detector();
                    if (!row.IsNull("DateAdded"))
                    {
                        d.DateAdded = row.Date_Added;
                    }
                    else
                    {
                        d.DateAdded = DateTime.Now;
                    }
                    d.DetChannel = row.Det_Channel;


                    d.DetectorID = row.DetectorID;

                    if (!row.IsNull("DistanceFromStopBar"))
                    {
                        d.DistanceFromStopBar = row.DistanceFromStopBar;
                    }
                    else
                    {
                        d.DistanceFromStopBar = 0;
                    }
                    if (!row.IsNull("MinSpeedFilter"))
                    {
                        d.MinSpeedFilter = row.Min_Speed_Filter;
                    }
                    else
                    {
                        d.MinSpeedFilter = 0;
                    }
                    if (d.DetectionTypes == null && d.DetectionTypeIDs == null)
                    {
                        d.DetectionTypeIDs = new List<int>();
                        d.DetectionTypes = new List<MOE.Common.Models.DetectionType>();

                    }

                    if (!row.IsNull("Has_PCD") && row.Has_PCD)
                    {
                        MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                                   where r.DetectionTypeID == 2
                                                                   select r).FirstOrDefault();

                        d.DetectionTypes.Add(dettype);
                        d.DetectionTypeIDs.Add(dettype.DetectionTypeID);
                    }
                    if (!row.IsNull("Has_Speed_Detector") && row.Has_Speed_Detector)
                    {


                        MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                                   where r.DetectionTypeID == 3
                                                                   select r).FirstOrDefault();

                        d.DetectionTypes.Add(dettype);
                        d.DetectionTypeIDs.Add(dettype.DetectionTypeID);
                    }
                    if (!row.IsNull("Has_SplitFail") && row.Has_SplitFail)
                    {
                        MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                                   where r.DetectionTypeID == 4
                                                                   select r).FirstOrDefault();

                        d.DetectionTypes.Add(dettype);
                        d.DetectionTypeIDs.Add(dettype.DetectionTypeID);
                    }
                    if (!row.IsNull("Has_RLM") && row.Has_RLM)
                    {
                        MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                                   where r.DetectionTypeID == 5
                                                                   select r).FirstOrDefault();

                        d.DetectionTypes.Add(dettype);
                        d.DetectionTypeIDs.Add(dettype.DetectionTypeID);
                    }
                    if (!row.IsNull("Has_TMC") && row.Has_TMC)
                    {
                        MOE.Common.Models.DetectionType dettype = (from r in detectionstypeslist
                                                                   where r.DetectionTypeID == 6
                                                                   select r).FirstOrDefault();

                        d.DetectionTypes.Add(dettype);
                        d.DetectionTypeIDs.Add(dettype.DetectionTypeID);
                    }
                    if (!row.IsNull("Direction") && row.Direction.ToString() != "")
                    {
                        switch (row.Direction.ToString())
                        {
                            case "Northbound":
                                dc.DirectionTypeID = 1;
                                break;
                            case "Southbound":
                                dc.DirectionTypeID = 2;
                                break;
                            case "Eastbound":
                                dc.DirectionTypeID = 3;
                                break;
                            case "Westbound":
                                dc.DirectionTypeID = 4;
                                break;

                        }
                    }

                    if (!row.IsNull("MovementDelay"))
                    {
                        dc.MovementDelay = row.Movement_Delay;
                    }
                    else
                    {
                        dc.MovementDelay = 0;
                    }

                    if (!row.IsNull("MPH"))
                    {
                        dc.MPH = row.MPH;
                    }
                    else
                    {
                        dc.MPH = 0;
                    }

                    if (!row.IsNull("TMC_Lane_Type") && row.TMC_Lane_Type != "")
                    {
                        switch (row.TMC_Lane_Type)
                        {

                            case "L1":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 3;
                                break;
                            case "L2":
                                dc.LaneNumber = 2;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 3;
                                break;
                            case "L3":
                                dc.LaneNumber = 3;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 3;
                                break;
                            case "L4":
                                dc.LaneNumber = 4;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 3;
                                break;
                            case "R1":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 2;
                                break;
                            case "R2":
                                dc.LaneNumber = 2;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 2;
                                break;
                            case "R3":
                                dc.LaneNumber = 3;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 2;
                                break;
                            case "R4":
                                dc.LaneNumber = 4;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 2;
                                break;
                            case "T1":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 1;
                                break;
                            case "T2":
                                dc.LaneNumber = 2;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 1;
                                break;
                            case "T3":
                                dc.LaneNumber = 3;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 1;
                                break;
                            case "T4":
                                dc.LaneNumber = 4;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 1;
                                break;
                            case "TL1":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 5;
                                break;
                            case "TR1":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 1;
                                dc.MovementTypeID = 4;
                                break;

                            case "T1E":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 4;
                                dc.MovementTypeID = 1;
                                break;
                            case "T2E":
                                dc.LaneNumber = 2;
                                dc.LaneTypeID = 4;
                                dc.MovementTypeID = 1;
                                break;
                            case "T3E":
                                dc.LaneNumber = 3;
                                dc.LaneTypeID = 4;
                                dc.MovementTypeID = 1;
                                break;
                            case "T4E":
                                dc.LaneNumber = 4;
                                dc.LaneTypeID = 4;
                                dc.MovementTypeID = 1;
                                break;
                            case "L1B":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 2;
                                dc.MovementTypeID = 3;
                                break;
                            case "T1B":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 2;
                                dc.MovementTypeID = 1;
                                break;
                            case "R1B":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 2;
                                dc.MovementTypeID = 2;
                                break;
                            case "PED":
                                dc.LaneNumber = 1;
                                dc.LaneTypeID = 3;
                                dc.MovementTypeID = 1;
                                break;
                        }
                    }
                    else
                    {
                        dc.LaneNumber = 1;
                        dc.LaneTypeID = 1;
                        dc.MovementTypeID = 1;
                    }

                    if (dc.LaneNumber == 0)
                    {
                        dc.LaneNumber = 1;
                    }

                    if (dc.LaneTypeID == 0)
                    {
                        dc.LaneTypeID = 1;
                    }

                    if (dc.MovementTypeID == 0)
                    {
                        dc.MovementTypeID = 1;
                    }

                    if (!row.IsNull("Perm_Phase") && row.Perm_Phase != "0" && row.Perm_Phase != "")
                    {
                        dc.PermissivePhase = Convert.ToInt32(row.Phase);
                    }

                    if (!row.IsNull("Is_Overlap") && row.Is_Overlap)
                    {
                        dc.IsOverlap = row.Is_Overlap;
                    }

                    dc.ProtectedPhase = Convert.ToInt32(row.Phase);
                    dc.GraphDetectorModel = d;
                    dc.SignalID = row.SignalID;


                    DetectorConverters.Add(dc);

                }
            }
        }


        public void SaveSignalsToDB()
        {
            db.Signals.AddRange(SignalsList.Distinct());
            Console.WriteLine("Saving Signals");
            db.SaveChanges();



        }

        public void SortDetectorsByDirection()
        {



            foreach (Models.Signal s in this.SignalsList)
            {
                List<DetectorConverter> dets;
                for (int i = 1; i < 5; i++)
                {
                    dets = (from d in DetectorConverters
                            where d.SignalID == s.SignalID && d.DirectionTypeID == i
                            select d).ToList();

                    CreateApproach(dets, s, i);


                }



            }
        }

        public void CreateApproach(List<DetectorConverter> CDetectorsForADirection, Models.Signal signal, int directiontypeID)
        {
            Models.DirectionType dir = (from r in db.DirectionTypes
                                        where r.DirectionTypeID == directiontypeID
                                        select r).FirstOrDefault();

            Models.Approach app = Models.Approach.CreateNewApproachWithDefaultValues(signal, dir, db);




            foreach (DetectorConverter dc in CDetectorsForADirection)
            {


                if (dc.MPH > app.MPH)
                {
                    app.MPH = dc.MPH;
                }



            }
            if (signal.Approaches == null)
            {
                signal.Approaches = new List<MOE.Common.Models.Approach>();
            }
            signal.Approaches.Add(app);


        }





        public void ConvertSignalComments()
        {
            Console.WriteLine("Starting Signal Comment converison.");
            List<Models.MetricComment> comments = new List<Models.MetricComment>();


            foreach (var row in CommentsTable)
            {
                if (row.EntityType == 1)
                {
                    Models.MetricComment comment = new Models.MetricComment();

                    if (comment.MetricTypeIDs == null)
                    {
                        comment.MetricTypeIDs = new List<int>();
                    }

                    comment.CommentText = row.Comment;
                    comment.SignalID = row.Entity;
                    comment.MetricTypeIDs.Add(1);
                    comment.MetricTypeIDs.Add(2);
                    comment.TimeStamp = row.TimeStamp;

                    comments.Add(comment);
                }

            }
            db.MetricComments.AddRange(comments);
            Console.WriteLine("Saving signal comments");
            db.SaveChanges();


        }


        public void ConvertApproachDetailDirections()
        {
            Console.WriteLine("Creating Approaches");
            List<MOE.Common.Models.ApproachRoute> ApproachRoutes = new List<Models.ApproachRoute>();
            foreach (var ApproachRouteRow in RouteTable)
            {
                MOE.Common.Models.ApproachRoute approachRoute = new Models.ApproachRoute();
                approachRoute.RouteName = ApproachRouteRow.RouteName;
                approachRoute.ApproachRouteId = ApproachRouteRow.ApproachRouteId;
                approachRoute.ApproachRouteDetails = new List<Models.ApproachRouteDetail>();
                ApproachRoutes.Add(approachRoute);
            }
            foreach (var DetailRow in RouteDetailTable)
            {
                var approachRoute = ApproachRoutes.Where(ar => ar.ApproachRouteId == DetailRow.ApproachRouteId).FirstOrDefault();
                var approach = db.Approaches
                    .Where(a => a.SignalID == DetailRow.SignalID && a.DirectionType.Description == DetailRow.Direction)
                    .FirstOrDefault();
                if (approach != null)
                {
                    Models.ApproachRouteDetail ard = new Models.ApproachRouteDetail();
                    ard.ApproachOrder = DetailRow.ApproachOrder;
                    ard.ApproachID = approach.ApproachID;
                    approachRoute.ApproachRouteDetails.Add(ard);
                }
            }
            db.ApproachRoutes.AddRange(ApproachRoutes);
            Console.WriteLine("Saving Approaches");
            db.SaveChanges();
        }


        public void ConvertActionTakenEntries()

        {
            Console.WriteLine("Starting Usage converison.");
            List<MOE.Common.Models.ActionLog> Actions = new List<Models.ActionLog>();

            List<MOE.Common.Models.Action> ActionTypes = (from r in db.Actions select r).ToList();

            List<MOE.Common.Models.Agency> ActionAgencies = (from r in db.Agencies select r).ToList();

            List<MOE.Common.Models.MetricType> MetricTypes = (from r in db.MetricTypes select r).ToList();

            List<MOE.Common.Data.Action.ActionLog_DisabledRow> ActionLogList = ActionLogTable.ToList();
            List<MOE.Common.Data.Action.Action_Log_ActionsRow> ActionLogActionsList = ActionLogActions.ToList();

            foreach (var row in ActionLogTable)
            {
                if (row.Name != "" || row.Comment != "")
                {
                    MOE.Common.Models.ActionLog newAction = new MOE.Common.Models.ActionLog();

                    newAction.Date = row.Date;

                    newAction.Comment = row.Comment;
                    newAction.SignalID = row.SignalId;
                    newAction.Name = row.Name;

                    if (newAction.MetricTypes == null)
                    {
                        newAction.MetricTypes = new List<MOE.Common.Models.MetricType>();
                    }

                    if (newAction.MetricTypeIDs == null)
                    {
                        newAction.MetricTypeIDs = new List<int>();
                    }

                    if (newAction.Actions == null)
                    {
                        newAction.Actions = new List<MOE.Common.Models.Action>();
                    }

                    if (newAction.ActionIDs == null)
                    {
                        newAction.ActionIDs = new List<int>();
                    }



                    var oldmetricIDs = (from r in ActionLogMetrics
                                        where r.Action_Log_Id == row.ID
                                        select r);

                    foreach (var m in oldmetricIDs)
                    {

                        if (m.Metric_Id == 1)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 6 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(6);
                        }

                        if (m.Metric_Id == 2)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 10 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(10);
                        }
                        if (m.Metric_Id == 3)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 7 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(7);
                        }
                        if (m.Metric_Id == 4)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 2 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(2);
                        }
                        if (m.Metric_Id == 5)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 1 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(1);
                        }
                        if (m.Metric_Id == 6)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 5 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(5);
                        }
                        if (m.Metric_Id == 7)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 13 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(13);
                        }
                        if (m.Metric_Id == 8)
                        {

                            newAction.MetricTypes.Add((from r in MetricTypes where r.MetricID == 11 select r).FirstOrDefault());
                            newAction.MetricTypeIDs.Add(11);
                        }

                    }




                    var oldActionIDs = (from r in ActionLogActionsList
                                        where r.Action_Log_Id == row.ID
                                        select r);

                    foreach (var v in oldActionIDs)
                    {
                        if (v.Action_Id == 1)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 1 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(1);
                        }

                        if (v.Action_Id == 2)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 2 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(2);
                        }

                        if (v.Action_Id == 3)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 3 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(3);
                        }
                        if (v.Action_Id == 4)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 4 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(4);
                        }
                        if (v.Action_Id == 5)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 5 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(5);
                        }
                        if (v.Action_Id == 6)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 6 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(6);
                        }
                        if (v.Action_Id == 7)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 7 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(7);
                        }
                        if (v.Action_Id == 8)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 8 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(8);
                        }
                        if (v.Action_Id == 9)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 9 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(9);
                        }
                        if (v.Action_Id == 10)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 10 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(10);
                        }
                        if (v.Action_Id == 11)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 11 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(11);
                        }
                        if (v.Action_Id == 12)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 12 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(12);
                        }
                        if (v.Action_Id == 13)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 13 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(13);
                        }
                        if (v.Action_Id == 14)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 14 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(14);
                        }
                        if (v.Action_Id == 15)
                        {
                            newAction.Actions.Add((from r in ActionTypes where r.ActionID == 15 select r).FirstOrDefault());
                            newAction.ActionIDs.Add(15);
                        }
                    }

                    int oldAgencyID = (from r in ActionAgencies
                                       where r.AgencyID == row.Agency
                                       select r.AgencyID).FirstOrDefault();

                    switch (oldAgencyID)
                    {
                        case 1:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 1 select r).FirstOrDefault();
                            break;
                        case 2:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 2 select r).FirstOrDefault();
                            break;
                        case 3:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 3 select r).FirstOrDefault();
                            break;
                        case 4:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 4 select r).FirstOrDefault();
                            break;
                        case 5:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 5 select r).FirstOrDefault();
                            break;
                        case 6:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 6 select r).FirstOrDefault();
                            break;
                        case 7:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 7 select r).FirstOrDefault();
                            break;
                        case 8:
                            newAction.Agency = (from r in ActionAgencies where r.AgencyID == 8 select r).FirstOrDefault();
                            break;
                    }
                    Actions.Add(newAction);



                }
            }

            db.ActionLogs.AddRange(Actions);
            try
            {
                Console.WriteLine("Saving Actions.");
                db.SaveChanges();
            }
            catch 
            {
               
            }
        }

        public void ConvertUsers()
        {
            Console.WriteLine("Starting User converison.");
           var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<SPMUser>(db);
            var userManager = new UserManager<SPMUser>(userStore);




             foreach(var row in OldUsersTable)
            {
                var user = new SPMUser { UserName = row.Email.ToLower(), Email = row.Email.ToLower() };
                userManager.Create(user, "!a2b3c4d5e6G");
                user.ReceiveAlerts = false;
                try
                {
                    userManager.AddToRole(user.Id, "User");
                }
                 catch
                {
                    Console.WriteLine("Unable to add user " + row.Name + "to the database.");
                }
               
            }
            Console.WriteLine("Saving Users");
            db.SaveChanges();
        }
    }




}






public class DetectorConverter
{
    public string SignalID { get; set; }
    [Required]
    public MOE.Common.Models.Detector GraphDetectorModel { get; set; }
    public List<int> DetectionsTypes { get; set; }
    public int MPH { get; set; }
    public int DecisionPoint { get; set; }
    public int MovementTypeID { get; set; }
    public int LaneTypeID { get; set; }
    public int DirectionTypeID { get; set; }
    public int LaneNumber { get; set; }
    public int MovementDelay { get; set; }
    public int ProtectedPhase { get; set; }
    public int PermissivePhase { get; set; }
    public bool IsOverlap { get; set; }
}








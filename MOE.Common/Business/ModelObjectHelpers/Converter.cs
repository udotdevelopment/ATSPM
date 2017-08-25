using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common;
using System.Data.Entity.Infrastructure;


namespace MOE.Common.Business.ModelObjectHelpers
{
    public class Converter
    {
        public MOE.Common.Data.Signals.SignalsDataTable SignalsTable { get; set; }
        public MOE.Common.Data.Signals.Graph_DetectorsDataTable DetectorsTable { get; set; }
        public MOE.Common.Data.Signals.SPM_CommentDataTable CommentsTable { get; set; }
        public MOE.Common.Data.Signals.ApproachRouteDetailDataTable RouteDetailTable { get; set; }
        public List<MOE.Common.Models.Signal> SignalsList { get; set; }
        public List<MOE.Common.Models.Graph_Detectors> DetectorsList { get; set; }

        public List<Models.DetectionType> DetectionsTypes { get; set; }
        public List<DetectorConverter> DetectorConverters { get; set; }
        //Models.Repositories.ILaneGroupRepository lgr = Models.Repositories.LaneGroupRepositoryFactory.CreateLaneGroupRepository();
        //Models.Repositories.ILaneRepository lanerep = Models.Repositories.LaneRepositoryFactory.CreateLaneRepository();
        //Models.Repositories.IGraphDetectorRepository gdr = Models.Repositories.GraphDetectorRepositoryFactory.CreateGraphDetectorRepository();

        private Models.SPM db = null;



        public Converter()
        {
            GetRecords();
            GetSignalsList();
            GetDetectorList();
           
            SortDetectorsByDirection();
            SaveSignalsToDB();
            ConvertSignalComments();
            ConvertApproachDetailDirections();




        }


        public void GetRecords()
        {
            MOE.Common.Data.SignalsTableAdapters.SignalsTableAdapter sta = new Data.SignalsTableAdapters.SignalsTableAdapter();
            SignalsTable = sta.GetData();

            MOE.Common.Data.SignalsTableAdapters.Graph_DetectorsTableAdapter gta = new Data.SignalsTableAdapters.Graph_DetectorsTableAdapter();
            DetectorsTable = gta.GetData();

            MOE.Common.Data.SignalsTableAdapters.SPM_CommentTableAdapter cta = new Data.SignalsTableAdapters.SPM_CommentTableAdapter();
            CommentsTable = cta.GetAllData();

            MOE.Common.Data.SignalsTableAdapters.ApproachRouteDetailTableAdapter rta = new Data.SignalsTableAdapters.ApproachRouteDetailTableAdapter();
            RouteDetailTable = rta.GetData();



            db = new Models.SPM();
            
            
        }

        public void GetSignalsList()
        {
            SignalsList = new List<Models.Signal>();

            foreach (MOE.Common.Data.Signals.SignalsRow row in SignalsTable)
            {
                Models.Signal s = new Models.Signal();
                s.SignalID = row.SignalID;
                s.Primary_Name = row.Primary_Name;
                s.Secondary_Name = row.Secondary_Name;
                if (!row.IsNull("IP_Address") && row.IP_Address != "")
                {
                    s.IP_Address = row.IP_Address;
                }
                else
                {
                    s.IP_Address = "10.10.10.10";
                }
                s.Latitude = row.Latitude;
                s.Longitude = row.Longitude;
                s.RegionID = Convert.ToInt32(row.Region);
                s.Controller_Type = row.Controller_Type;
                s.Enabled = true;

                SignalsList.Add(s);
            }



        }



        public void GetDetectorList()
        {
            //DetectorsList = new List<Models.Graph_Detectors>();
            DetectorConverters = new List<DetectorConverter>();

            foreach (MOE.Common.Data.Signals.Graph_DetectorsRow row in DetectorsTable)
            {
                List<MOE.Common.Models.DetectionType> detectionstypeslist = (from r in db.DetectionTypes
                                                                             select r).ToList();

                if (!row.IsNull("Phase") || row.Phase != "0" || row.Phase != "" || row.Det_Channel != 0)
                {
                    DetectorConverter dc = new DetectorConverter();

                    Models.Graph_Detectors d = new Models.Graph_Detectors();
                    if (!row.IsNull("Date_Added"))
                    {
                        d.Date_Added = row.Date_Added;
                    }
                    else
                    {
                        d.Date_Added = DateTime.Now;
                    }
                    d.Det_Channel = row.Det_Channel;


                    d.DetectorID = row.DetectorID;

                    if (!row.IsNull("DistanceFromStopBar"))
                    {
                        d.DistanceFromStopBar = row.DistanceFromStopBar;
                    }
                    else
                    {
                        d.DistanceFromStopBar = 0;
                    }
                    if (!row.IsNull("Min_Speed_Filter"))
                    {
                        d.Min_Speed_Filter = row.Min_Speed_Filter;
                    }
                    else
                    {
                        d.Min_Speed_Filter = 0;
                    }
                    if (d.DetectionTypes == null && d.DetectionTypeIDs == null)
                    {
                        d.DetectionTypeIDs = new List<int>();
                        d.DetectionTypes = new List<MOE.Common.Models.DetectionType>();
                        //MOE.Common.Models.DetectionType basicdettype = (from r in detectionstypeslist
                        //                                                where r.DetectionTypeID == 1
                        //                                                select r).FirstOrDefault();

                        
                        //d.DetectionTypes.Add(basicdettype);
                        //d.DetectionTypeIDs.Add(basicdettype.DetectionTypeID);

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

                    if (!row.IsNull("Movement_Delay"))
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

                    if(dc.LaneNumber == 0)
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


                if (dc.DecisionPoint > app.Decision_Point)
                {
                    app.Decision_Point = dc.DecisionPoint;
                }

                if (dc.MPH > app.MPH)
                {
                    app.MPH = dc.MPH;
                }

                if (app.Movement_Delay < dc.MovementDelay)
                {
                    app.Movement_Delay = dc.MovementDelay;
                }



            }
            if (signal.Approaches == null)
            {
                signal.Approaches = new List<MOE.Common.Models.Approach>();
            }
            signal.Approaches.Add(app);

            CreateLaneGroupsAndLanes(CDetectorsForADirection, app);
        }


        

        private void SetLaneGroupValues(Models.Approach Appr, Models.LaneGroup lg, DetectorConverter dc, int movementID, int lanetypeID)
        {

            Models.MovementType mt = (from r in db.MovementTypes
                                      where r.MovementTypeID == movementID
                                      select r).FirstOrDefault();

            lg.Description = Appr.Description + mt.Abbreviation;
            lg.MovementTypeID = movementID;
            lg.LaneGroupTypeID = lanetypeID;
            lg.ProtectedPhaseNumber = dc.ProtectedPhase;
            lg.IsProtectedPhaseOverlap = dc.IsOverlap;
            if (dc.PermissivePhase > 0)
            {
                lg.PermissivePhaseNumber = dc.PermissivePhase;
                lg.IsPermissivePhaseOverlap = false;
            }
        }

        public void CreateLaneGroupsAndLanes(List<DetectorConverter> CDetectorsForADirection, Models.Approach Appr)
        {
            Models.Approach.AddDefaultObjectsToApproach(Appr, db);

            //Update defaults based on values in DB
            for (int lanetypeID = 1; lanetypeID < 8; lanetypeID++)
            {
                var LanesByType = from d in CDetectorsForADirection
                                  where d.LaneTypeID == lanetypeID
                                  select d;

                if (LanesByType.Count() > 0)
                {
                    for (int movementID = 1; movementID < 6; movementID++)
                    {
                        Models.LaneGroup LaneGroupByTypeAndMovement;
                        LaneGroupByTypeAndMovement = (from r in Appr.LaneGroups
                                                                       where r.LaneGroupTypeID == lanetypeID
                                                                       && r.MovementTypeID == movementID
                                                                       select r).FirstOrDefault();

                        List<DetectorConverter> detectorsforMovement = (from l in LanesByType
                                                                        where l.MovementTypeID == movementID
                                                                        select l).ToList();

                        if (LaneGroupByTypeAndMovement == null && detectorsforMovement.Count > 0)
                        {
                            LaneGroupByTypeAndMovement = new Models.LaneGroup();
                            SetLaneGroupValues(Appr, LaneGroupByTypeAndMovement, detectorsforMovement.FirstOrDefault(), movementID, lanetypeID);
                            
                            
                            Appr.LaneGroups.Add(LaneGroupByTypeAndMovement);
                            LaneGroupByTypeAndMovement = (from r in Appr.LaneGroups
                                                          where r.LaneGroupTypeID == lanetypeID
                                                          && r.MovementTypeID == movementID
                                                          select r).FirstOrDefault();

                        }
                        else if (LaneGroupByTypeAndMovement != null && detectorsforMovement.Count > 0)
                        {                           
                            SetLaneGroupValues(Appr, LaneGroupByTypeAndMovement, detectorsforMovement.FirstOrDefault(), movementID, lanetypeID);
                        }
                        else if (LaneGroupByTypeAndMovement == null && detectorsforMovement.Count == 0)
                        {
                            return;
                        }


                        for (int lanenumber = 1; lanenumber < 5; lanenumber++)
                        {
                            List<DetectorConverter> detConvertersByLaneNumber = (from d in detectorsforMovement
                                                                          where d.LaneNumber == lanenumber
                                                                          select d).ToList();

                            //Models.Graph_Detectors graphDetector = (from r in LaneGroupByTypeAndMovement.Detectors
                            //                    where r.LaneNumber == lanenumber
                            //                    select r).FirstOrDefault();

                            if (detConvertersByLaneNumber != null)
                            {
                                foreach (DetectorConverter d in detConvertersByLaneNumber)
                                {
                                //detConverterByLaneNumber.GraphDetectorModel.LaneGroupID = LaneGroupByTypeAndMovement.LaneGroupID;
                                d.GraphDetectorModel.LaneNumber = lanenumber;
                                if (LaneGroupByTypeAndMovement.Detectors == null)
                                {
                                    LaneGroupByTypeAndMovement.Detectors = new List<MOE.Common.Models.Graph_Detectors>();
                                }
                                LaneGroupByTypeAndMovement.Detectors.Add(d.GraphDetectorModel);
                            }
                        }
                            //else if (detConverterByLaneNumber != null && lane != null)
                            //{

                            //    lane.Detectors.Add(detConverterByLaneNumber.GraphDetectorModel);
                            //}
                            else if (detConvertersByLaneNumber == null)
                            {
                                return;
                            }


                        }

                    }
                }
            }

        }

        public void ConvertSignalComments()
        {
            List<Models.MetricComment> comments = new List<Models.MetricComment>();


            foreach(var row in CommentsTable)
            {
                if(row.EntityType == 1)
                {
                    Models.MetricComment comment = new Models.MetricComment();

                    if(comment.MetricTypeIDs == null)
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



        }


        public void ConvertApproachDetailDirections()
        {

            //MOE.Common.Models.Repositories.IApproachRouteDetailRepository  ardr =
            //    MOE.Common.Models.Repositories.ApproachRouteDetailRepositoryFactory.Create();
  

            var directions = from r in db.DirectionTypes
                             select r;

            foreach(var row in RouteDetailTable)
            {
                 //MOE.Common.Models.ApproachRouteDetail newRouteDetail = ardr.GetByRouteID(row.ApproachRouteId).FirstOrDefault();

                MOE.Common.Models.ApproachRouteDetail newRouteDetail = (from r in db.ApproachRouteDetails 
                                                                       where r.ApproachRouteId == row.ApproachRouteId 
                                                                       select r).FirstOrDefault();
                if (newRouteDetail == null)
                {
                    newRouteDetail = new MOE.Common.Models.ApproachRouteDetail();
                    newRouteDetail.ApproachOrder = row.ApproachOrder;
                    newRouteDetail.ApproachRouteId = row.ApproachRouteId;
                    newRouteDetail.SignalID = row.SignalID;
                }

                 newRouteDetail.DirectionTypeID = (from d in directions
                                                   where d.Description == row.Direction
                                                   select d.DirectionTypeID).FirstOrDefault();

                 db.ApproachRouteDetails.Add(newRouteDetail);

                
            }
        }





    }





    public class DetectorConverter
    {
        public string SignalID { get; set; }
        public MOE.Common.Models.Graph_Detectors GraphDetectorModel { get; set; }
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


}



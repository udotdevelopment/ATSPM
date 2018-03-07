﻿using System;
using MOE.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Helpers
{
    public class XMLToListImporter
    {
        private static readonly string filePath = @".\.\XMLDataFiles\";

        private static XElement emptyElement = XElement.Parse("<x>" +
            "<string>0</string>" +
            "<integer>0</integer>" +
            "<date>01/01/2020</date>"+
            "</x>");

        

        public static void LoadControllerEventLog(string xmlFileName, InMemoryMOEDatabase _db)
        {

            string localFilePath = filePath+ @"EventLogFiles\" + xmlFileName;

           
            var doc = XElement.Load(localFilePath);

            // List<Controller_Event_Log> 
            var incomingEvents = doc.Elements("Controller_Event_Log").Select(x => new Controller_Event_Log
                {
                    EventCode = Convert.ToInt32(x.Element("EventCode").Value),
                    EventParam = Convert.ToInt32(x.Element("EventParam").Value),
                    Timestamp = Convert.ToDateTime(x.Element("timestamp").Value),
                    SignalID = x.Element("signalid").Value.ToString()
                }
            ).ToList();

            foreach (var e in incomingEvents)
            {

                _db.Controller_Event_Log.Add(e);

            }
        }

        public static void LoadSignals(string xmlFileName, InMemoryMOEDatabase _db)
        {

            string localFilePath = filePath + @"\Signals\" + xmlFileName;


            var doc = XElement.Load(localFilePath);

            // List<Controller_Event_Log> 
            var incoming = doc.Elements("Signal").Select(x => new Signal
                {
                SignalID = x.Element("SignalID").Value,
                Latitude = x.Element("Latitude").Value,
                Longitude = x.Element("Longitude").Value,
                PrimaryName = x.Element("PrimaryName").Value,
                SecondaryName = x.Element("SecondaryName").Value,
                IPAddress = x.Element("IPAddress").Value,
                RegionID = Convert.ToInt32(x.Element("RegionID").Value),
                ControllerTypeID = Convert.ToInt32(x.Element("ControllerTypeID").Value),
                Enabled = (x.Element("Enabled").Value).Equals("1"),
                VersionID = Convert.ToInt32(x.Element("VersionID").Value),
                VersionActionId = Convert.ToInt32(x.Element("VersionActionId").Value),
                Note = x.Element("Note").Value,
                Start = Convert.ToDateTime(x.Element("Start").Value)
            }
            ).ToList();

            foreach (var e in incoming)
            {

                _db.Signals.Add(e);

            }
        }

        public static void LoadApproaches(string xmlFileName, InMemoryMOEDatabase _db)
        {

            string localFilePath = filePath + "\\Approaches\\" + xmlFileName;


            var doc = XElement.Load(localFilePath);

            var incoming = new List<Approach>();

            foreach (var x in doc.Elements("Approach"))
            {
                var appr = new Approach();
                appr.ApproachID = Convert.ToInt32(x.Element("ApproachID").Value);
                appr.SignalID = x.Element("SignalID").Value;
                appr.DirectionTypeID = Convert.ToInt32(x.Element("DirectionTypeID").Value);
                appr.Description = x.Element("Description").Value;
                if (x.Element("MPH") != null)
                {
                    appr.MPH = Convert.ToInt32(x.Element("MPH").Value);
                }
                else
                {
                    appr.MPH = 0;
                }
                appr.ProtectedPhaseNumber = Convert.ToInt32(x.Element("ProtectedPhaseNumber").Value);
                appr.IsProtectedPhaseOverlap = (x.Element("IsProtectedPhaseOverlap").Value).Equals("1");
                if (x.Element("PermissivePhaseNumber") != null)
                {
                    appr.PermissivePhaseNumber = Convert.ToInt32(x.Element("PermissivePhaseNumber").Value);
                }
                else
                {
                    appr.PermissivePhaseNumber = 0;
                }
                appr.VersionID = Convert.ToInt32(x.Element("VersionID").Value);
                appr.IsPermissivePhaseOverlap = (x.Element("IsPermissivePhaseOverlap").Value).Equals("1");
                incoming.Add(appr);
            }

            

            //var incoming = doc.Elements("Approach").Select(x => new Approach
            //{
            //    ApproachID = Convert.ToInt32(x.Element("ApproachID").Value),
            //    SignalID = x.Element("SignalID").Value,
            //    DirectionTypeID = Convert.ToInt32(x.Element("DirectionTypeID").Value),
            //    Description = x.Element("Description").Value,
            //    MPH = Convert.ToInt32(x.Element("MPH").Value),
            //    ProtectedPhaseNumber = Convert.ToInt32(x.Element("ProtectedPhaseNumber").Value),
            //    IsProtectedPhaseOverlap = (x.Element("IsProtectedPhaseOverlap").Value).Equals("1"),
            //    PermissivePhaseNumber = Convert.ToInt32(x.Element("PermissivePhaseNumber").Value),
            //    VersionID = Convert.ToInt32(x.Element("VersionID").Value),
            //    IsPermissivePhaseOverlap = (x.Element("IsPermissivePhaseOverlap").Value).Equals("1")

            //    }
            //).ToList();

            foreach (var e in incoming)
            {

                _db.Approaches.Add(e);

            }
        }

        public static void LoadDetectors(string xmlFileName, InMemoryMOEDatabase _db)
        {

            string localFilePath = filePath + "\\Detectors\\" + xmlFileName;


            var doc = XElement.Load(localFilePath);
            List<Detector> incoming = new List<Detector>();

            foreach(var x in doc.Elements("Detector"))
            {
                var det = new Detector();

                det.ID = (Int32)x.Element("ID");
                det.DetectorID = (String)x.Element("DetectorID");
                det.DetChannel = (Int32)x.Element("DetChannel");
                det.DistanceFromStopBar = (Int32)(x.Element("DistanceFromStopBar") ?? emptyElement.Element("integer"));
                det.MinSpeedFilter = (Int32)(x.Element("MinSpeedFilter")?? emptyElement.Element("integer"));
                det.DateAdded = (DateTime)(x.Element("DateAdded"));
                det.DateDisabled = (DateTime)(x.Element("DateDisabled") ?? emptyElement.Element("date"));
                det.LaneNumber = (Int32)(x.Element("LaneNumber") ?? emptyElement.Element("integer"));
                det.MovementTypeID = (Int32)(x.Element("MovementTypeID") ?? emptyElement.Element("integer"));
                det.LaneTypeID = (Int32)(x.Element("LaneTypeID") ?? emptyElement.Element("integer"));
                det.DecisionPoint = (Int32)(x.Element("DecisionPoint") ?? emptyElement.Element("integer"));
                det.MovementDelay = (Int32)(x.Element("MovementDelay") ?? emptyElement.Element("integer"));
                det.ApproachID = (Int32)(x.Element("ApproachID") ?? emptyElement);
                det.DetectionHardwareID = (Int32)(x.Element("DetectionHardwareID") ?? emptyElement);
                det.LatencyCorrection = (Double)(x.Element("LatencyCorrection") ?? emptyElement.Element("integer"));

                incoming.Add(det);
            }

            // List<Controller_Event_Log> 
            //var incoming = doc.Elements("Detector").Select(x => new Detector
            //{
            //    ID = (Int32)x.Element("ID"),
            //    DetectorID = (String)x.Element("DetectorID").Value,
            //    DetChannel = (Int32)x.Element("DetChannel"),
            //    DistanceFromStopBar = Convert.ToInt32((String)x.Element("DistanceFromStopBar").Value??"0"),
            //    MinSpeedFilter = Convert.ToInt32(x.Element("MinSpeedFilter").Value),
            //    DateAdded = Convert.ToDateTime(x.Element("DateAdded").Value),
            //    DateDisabled = Convert.ToDateTime(x.Element("DateDisabled").Value),
            //    LaneNumber = Convert.ToInt32(x.Element("LaneNumber").Value),
            //    MovementTypeID = Convert.ToInt32(x.Element("MovementTypeID").Value),
            //    LaneTypeID = Convert.ToInt32(x.Element("LaneTypeID").Value),
            //    DecisionPoint = Convert.ToInt32(x.Element("DecisionPoint").Value),
            //    MovementDelay = Convert.ToInt32(x.Element("MovementDelay").Value),
            //    ApproachID = Convert.ToInt32(x.Element("ApproachID").Value),
            //    DetectionHardwareID = Convert.ToInt32(x.Element("DetectionHardwareID").Value),
            //    LatencyCorrection = Convert.ToInt32((x.Element("LatencyCorrection").Value))

            //    }
            //).ToList();

            foreach (var e in incoming)
            {
                //e.DetectionTypeIDs
                _db.Detectors.Add(e);

            }
        }

        public static void AddDetectionTypesToDetectors(string xmlFileName, InMemoryMOEDatabase _db)
        {
            string localFilePath = filePath + "\\DetectorToDetectionTypes\\" + xmlFileName;


            var doc = XElement.Load(localFilePath);

            foreach(var det in _db.Detectors)
            {
                det.DetectionTypeIDs = new List<int>();
                var types = doc.Elements("MOEAgg.dbo.DetectionTypeDetector");
                foreach (var t in types)
                    {
                    if(t.Element("ID").Value == det.ID.ToString())
                    {
                        //var detType = new DetectionType(
                        //    ID = t.Element("ID").Value,
                        //    t.Element("DetectionTypeID").Value
                        //    );

                        det.DetectionTypeIDs.Add(Convert.ToInt32(t.Element("DetectionTypeID").Value));
                           // det.DetectionTypes.Add(detType);
                    }

                }
            }
        }

        public static void AddDetectionTypesToMetricTypes(string xmlFileName, InMemoryMOEDatabase _db)
        {
            string localFilePath = filePath + "\\DetectorToDetectionTypes\\" + xmlFileName;


            var doc = XElement.Load(localFilePath);

            foreach (var det in _db.Detectors)
            {
                det.DetectionTypeIDs = new List<int>();
                var types = doc.Elements("MOEAgg.dbo.DetectionTypeDetector");
                foreach (var t in types)
                {
                    if (t.Element("ID").Value == det.ID.ToString())
                    {
                        //var detType = new DetectionType(
                        //    ID = t.Element("ID").Value,
                        //    t.Element("DetectionTypeID").Value
                        //    );

                        det.DetectionTypeIDs.Add(Convert.ToInt32(t.Element("DetectionTypeID").Value));
                        // det.DetectionTypes.Add(detType);
                    }

                }
            }
        }




    }
}
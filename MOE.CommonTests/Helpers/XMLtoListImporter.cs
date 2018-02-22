using System;
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
        private static string filePath = @".\.\XMLDataFiles\";

        public static void LoadControllerEventLog(string xmlFileName, InMemoryMOEDatabase _db)
        {

            filePath += xmlFileName;

           
            var doc = XElement.Load(filePath);

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

            filePath += xmlFileName;


            var doc = XElement.Load(filePath);

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
                Enabled = Convert.ToBoolean(x.Element("Enabled").Value),
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

            filePath += xmlFileName;


            var doc = XElement.Load(filePath);

            // List<Controller_Event_Log> 
            var incoming = doc.Elements("Approach").Select(x => new Approach
            {
                ApproachID = Convert.ToInt32(x.Element("ApproachID").Value),
                SignalID = x.Element("Latitude").Value,
                DirectionTypeID = Convert.ToInt32(x.Element("DirectionTypeID").Value),
                Description = x.Element("Description").Value,
                MPH = Convert.ToInt32(x.Element("MPH").Value),
                ProtectedPhaseNumber = Convert.ToInt32(x.Element("ProtectedPhaseNumber").Value),
                IsProtectedPhaseOverlap = Convert.ToBoolean(x.Element("IsProtectedPhaseOverlap").Value),
                PermissivePhaseNumber = Convert.ToInt32(x.Element("PermissivePhaseNumber").Value),
                VersionID = Convert.ToInt32(x.Element("VersionID").Value),
                IsPermissivePhaseOverlap = Convert.ToBoolean(x.Element("IsPermissivePhaseOverlap").Value)

                }
            ).ToList();

            foreach (var e in incoming)
            {

                _db.Approaches.Add(e);

            }
        }

        public static void LoadDetectors(string xmlFileName, InMemoryMOEDatabase _db)
        {

            filePath += xmlFileName;


            var doc = XElement.Load(filePath);

            // List<Controller_Event_Log> 
            var incoming = doc.Elements("Detector").Select(x => new Detector
                {
                    ID = Convert.ToInt32(x.Element("ID").Value),
                    DetectorID = x.Element("DetectorID").Value,
                    DetChannel = Convert.ToInt32(x.Element("DetChannel").Value),
                    DistanceFromStopBar = Convert.ToInt32(x.Element("DistanceFromStopBar").Value),
                    MinSpeedFilter = Convert.ToInt32(x.Element("MinSpeedFilter").Value),
                    DateAdded = Convert.ToDateTime(x.Element("DateAdded").Value),
                    DateDisabled = Convert.ToDateTime(x.Element("DateDisabled").Value),
                    LaneNumber = Convert.ToInt32(x.Element("LaneNumber").Value),
                    MovementTypeID = Convert.ToInt32(x.Element("MovementTypeID").Value),
                    LaneTypeID = Convert.ToInt32(x.Element("LaneTypeID").Value),
                    DecisionPoint = Convert.ToInt32(x.Element("DecisionPoint").Value),
                    MovementDelay = Convert.ToInt32(x.Element("MovementDelay").Value),
                    ApproachID = Convert.ToInt32(x.Element("ApproachID").Value),
                    DetectionHardwareID = Convert.ToInt32(x.Element("DetectionHardwareID").Value),
                    LatencyCorrection = Convert.ToInt32(x.Element("LatencyCorrection").Value)

                }
            ).ToList();

            foreach (var e in incoming)
            {
                //e.DetectionTypeIDs
                _db.Detectors.Add(e);

            }
        }


    }
}
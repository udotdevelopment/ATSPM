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
using System.Data.SqlClient;
using System.Data;

namespace MOE.CommonTests.Helpers
{
    public class XmlToListImporter
    {
        private static readonly string filePath = @".\.\XMLDataFiles\";

        private static readonly XElement emptyElement = XElement.Parse("<x>" +
            "<string>0</string>" +
            "<integer>0</integer>" +
            "<date>01/01/2020</date>"+
            "</x>");

        

        public static void LoadControllerEventLog(string xmlFileName, InMemoryMOEDatabase db)
        {

            string localFilePath = filePath+ @"EventLogFiles\" + xmlFileName;
            var doc = XElement.Load(localFilePath);
            // List<Controller_Event_Log> 
            var incomingEvents = doc.Elements("Controller_Event_Log").Select(x => new Controller_Event_Log
                {
                    EventCode = Convert.ToInt32(x.Element("EventCode").Value),
                    EventParam = Convert.ToInt32(x.Element("EventParam").Value),
                    Timestamp = Convert.ToDateTime(x.Element("Timestamp").Value),
                    SignalID = x.Element("SignalID").Value.ToString()
                }
            ).ToList();
                db.Controller_Event_Log.AddRange(incomingEvents);
        }

        public static void LoadControllerEventLogsFromMOEDB(InMemoryMOEDatabase db)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
             new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder["Data Source"] = "srwtcns54";
            builder["Password"] = "dontshareme";
            builder["Persist Security Info"] = true;
            builder["User ID"] = "datareader";
            builder["Initial Catalog"] = "MOE";
            Console.WriteLine(builder.ConnectionString);
            SqlConnection sqlConnection1 = new SqlConnection(builder.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandText = "select * from Controller_Event_Log"
                               +" Where Timestamp between '02/01/2018 00:00' and '02/01/2018 23:59'"
                               +" and SignalID = '7185'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Controller_Event_Log cel = new Controller_Event_Log();
                    cel.SignalID = reader.GetString(0);
                    cel.Timestamp = reader.GetDateTime(1);
                    cel.EventCode = reader.GetInt32(2);
                    cel.EventParam = reader.GetInt32(3);
                    db.Controller_Event_Log.Add(cel);
                }
            }
            reader.Close();
            sqlConnection1.Close();
        }

        public static void LoadSpeedEvents(string xmlFileName, InMemoryMOEDatabase db)
        {
            string localFilePath = filePath + @"SpeedEvents\" + xmlFileName;
            var doc = XElement.Load(localFilePath);
            var incomingEvents = doc.Elements("SpeedEvent").Select(x => new Speed_Events
            {
                DetectorID = (String)(x.Element("DetectorID")),
                MPH = (Int32)(x.Element("MPH")),
                KPH = (Int32)(x.Element("KPH")),
                timestamp = (DateTime)(x.Element("Timestamp"))
            }).ToList();
             db.Speed_Events.AddRange(incomingEvents);
        }

        public static void LoadSignals(string xmlFileName, InMemoryMOEDatabase db)
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
                db.Signals.Add(e);
            }
        }

        public static void LoadApproaches(string xmlFileName, InMemoryMOEDatabase db)
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
            foreach (var e in incoming)
            {
                var signal = db.Signals.Where(s => s.SignalID == e.SignalID).FirstOrDefault();
                if(signal!=null)
                {
                    signal.Approaches = new List<Approach>();
                    signal.Approaches.Add(e);
                    e.Signal = signal;
                }
                var direction = db.DirectionTypes.Where(d => d.DirectionTypeID == e.DirectionTypeID).FirstOrDefault();
                if (direction != null)
                {
                    e.DirectionType = direction;
                }
                db.Approaches.Add(e);
            }
        }

        public static void LoadDetectors(string xmlFileName, InMemoryMOEDatabase db)
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
            foreach (var e in incoming)
            {
                var appr = db.Approaches.Where(s => s.ApproachID == e.ApproachID).FirstOrDefault();
                if (appr != null)
                {
                    appr.Detectors = new List<Detector>();
                    appr.Detectors.Add(e);
                    e.Approach = appr;
                }
                var hardware = db.DetectionHardwares.Where(h => h.ID == e.DetectionHardwareID).FirstOrDefault();
                if (hardware != null)
                {
                    e.DetectionHardware = hardware;
                }
                db.Detectors.Add(e);
            }
        }

        public static void AddDetectionTypesToDetectors(string xmlFileName, InMemoryMOEDatabase db)
        {
            string localFilePath = filePath + "\\DetectorToDetectionTypes\\" + xmlFileName;
            var doc = XElement.Load(localFilePath);
            foreach(var det in db.Detectors)
            {
                det.DetectionTypeIDs = new List<int>();
                var types = doc.Elements("MOEAgg.dbo.DetectionTypeDetector");
                foreach (var t in types)
                    {
                    if(t.Element("ID").Value == det.ID.ToString())
                    {
                        det.DetectionTypeIDs.Add(Convert.ToInt32(t.Element("DetectionTypeID").Value));
                    }
                }
            }
        }

        public static void AddDetectionTypesToMetricTypes(string xmlFileName, InMemoryMOEDatabase db)
        {
            string localFilePath = filePath + "\\MetricTypeDetectorTypes\\" + xmlFileName;
            var doc = XElement.Load(localFilePath);
            foreach (var mt in db.MetricTypes)
            {
                mt.DetectionTypes = new List<DetectionType>();
                var types = doc.Elements("MetricDetectionType");
                foreach (var t in types)
                {
                    if (t.Element("MetricType_MetricID").Value == mt.MetricID.ToString())
                    {
                        var detType = db.DetectionTypes.Where(dt => dt.DetectionTypeID == (Int32)(t.Element("DetectionType_DetectionTypeID"))).FirstOrDefault();
                        if (detType != null)
                        {
                            if(detType.MetricTypes == null)
                            {
                                detType.MetricTypes = new List<MetricType>();
                            }
                            mt.DetectionTypes.Add(detType);
                            detType.MetricTypes.Add(mt);
                        }
                    }
                }
            }
        }
    }
}
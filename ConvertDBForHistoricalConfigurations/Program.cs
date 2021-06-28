using MOE.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Migrations;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE = MOE.Common.Data.MOE;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.Infrastructure;

namespace ConvertDBForHistoricalConfigurations
{
    public class Program
    {
        private static List<OldRoute> OldRouteList = new List<OldRoute>();
        private static List<OldMetricComments> _oldMetricCommentsList = new List<OldMetricComments>();
        private static List<OldMetricCommentMetricTypes> _oldMetricCommentMetricTypesList = new List<OldMetricCommentMetricTypes>();
        private static List<OldDetectionTypes> _oldDetectionTypes = new List<OldDetectionTypes>();
        private static List<OldDetectionTypeMetricTypes> _oldDetectionTypeMetricTypesList = new List<OldDetectionTypeMetricTypes>();
        private static List<OldActionLogMetricTypes> _oldActionLogMetricTypesList = new List<OldActionLogMetricTypes>();
        private static List<OldActionLogs> _oldActionLogsList = new List<OldActionLogs>();

        //Here is what has to happen:
        //Get the records from the Approachroute and ApproachRouteDetails tables BEFORE the EF migration
        //Run the EF migration
        //update the signal records with version information
        //Update Approaches with verison information
        //Update Metrics with version information
        static void Main()
        {
            //GetApproachRouteRecords(); 
            //GetApproachRouteDetails(); 

            //GetMetricComments(); 
            //GetMetricCommentMetricTypes();
            //ClearMetricComments(); 
            ////ClearMetricCommentMetricTypes(); // Derek
            ////GetDetectionTypeMetricTypes();  //Andre
            //ClearDetectionTypeMetricTypes();  
            //GetActionLogMetricTypes();  
            //ClearActionLogMetricTypes();
            //GetActionLogs();  
            ////ClearActionLogs();  //Andre

            RunMigrations(); 
            //UpdateSignalRecordsWithStartDateAndVersion();
            //UpdateApproachesWithVersionId();
            //UpdateMetriCommentsWithVersionId();
            //CreateRoutes();
            //UpdateActionLogs();
        }

        private static void UpdateActionLogs()
        {

            SPM db = new SPM();
            var allMetricTypes = db.MetricTypes.ToList();
            foreach (var oldActionLog in _oldActionLogsList)
            {
                var currentActionLog = db.ActionLogs.Find(oldActionLog.ActionLogID);
                var metricTypeRelationships = _oldActionLogMetricTypesList.Where(a => a.ActionLog_ActionLogID == currentActionLog.ActionLogID).Select(a => a.MetricType_MetricID).ToList();
                List<MetricType> metricsToAddToActionLog = new List<MetricType>();
                foreach (var metricType in allMetricTypes)
                {
                    if (metricTypeRelationships.Contains(metricType.MetricID))
                    {
                        metricsToAddToActionLog.Add(metricType);
                    }
                }
                if (currentActionLog != null)
                {
                   
                    currentActionLog.MetricTypes = metricsToAddToActionLog;
                }
            }
            db.SaveChanges();
        }

        private static void GetActionLogs()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from ActionLogs";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //Build class to hold values from database and assign values
                    OldActionLogs actionLogs = new OldActionLogs();
                    actionLogs.ActionLogID = (int)reader["ActionLogID"];
                    actionLogs.Date = (DateTime)reader["Date"];
                    actionLogs.AgencyID = (int)reader["AgencyID"];
                    if (reader.IsDBNull(3))
                    {
                        actionLogs.Comment = "";
                    }
                    else
                    {
                        actionLogs.Comment = (string)reader["Comment"];
                    }
                    actionLogs.SignalID = (string)reader["SignalID"];
                    _oldActionLogsList.Add(actionLogs);
                }
                sqlconn.Dispose();
            }

        }

        private static void GetActionLogMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from ActionLogMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())  
                {
                    OldActionLogMetricTypes actionLogMetricTypes = new OldActionLogMetricTypes();
                    actionLogMetricTypes.ActionLog_ActionLogID = (int)reader["ActionLog_ActionLogID"];
                    actionLogMetricTypes.MetricType_MetricID = (int)reader["MetricType_MetricID"];
                    _oldActionLogMetricTypesList.Add(actionLogMetricTypes);
                }
                sqlconn.Dispose();
            }
        }

        private static void GetDetectionTypeMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from DetectionTypeMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OldDetectionTypeMetricTypes detectionTypeMetricType = new OldDetectionTypeMetricTypes();
                    detectionTypeMetricType.DetectionType_DetectionTypeID = (int)reader["DetectionType_DetectionTypeID"];
                    detectionTypeMetricType.MetricType_MetricID = (int)reader["MetricType_MetricID"];
                    _oldDetectionTypeMetricTypesList.Add(detectionTypeMetricType);
                }
                sqlconn.Dispose();
            }
        }

        private static void ClearActionLogs()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            //sqlconn.ConnectionTimeout = 120; has no setter.  The default is 15.
            var slqQuery = "DELETE  from ActionLogs";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var rowCount = command.ExecuteNonQuery();
            sqlconn.Dispose();
        }

        private static void ClearActionLogMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "DELETE  from ActionLogMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var rowCount = command.ExecuteNonQuery();
            sqlconn.Dispose();
        }

        private static void ClearDetectionTypeMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "DELETE  from DetectionTypeMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var rowCount = command.ExecuteNonQuery();
            sqlconn.Dispose();
        }
        private static void ClearMetricComments()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "DELETE  from MetricComments";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var rowCount= command.ExecuteNonQuery();
            sqlconn.Dispose();
        }

        private static void ClearMetricCommentMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "DELETE  from MetricCommentMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var rowCount = command.ExecuteNonQuery();
            sqlconn.Dispose();
        }

        private static void GetMetricCommentMetricTypes()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from MetricCommentMetricTypes";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OldMetricCommentMetricTypes metricCommentMetricTypes  = new OldMetricCommentMetricTypes();
                    metricCommentMetricTypes.MetricComment_CommentId = (int) reader["MetricComment_CommentID"];
                    metricCommentMetricTypes.MetricType_MetricID = (int) reader["MetricType_MetricID"];
                    _oldMetricCommentMetricTypesList.Add(metricCommentMetricTypes);
                }
                sqlconn.Dispose();
            }
        }
        

        private static void GetMetricComments()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from MetricComments";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //Build class to hold values from database and assign values
                    OldMetricComments metricComments = new OldMetricComments();
                    metricComments.CommentId = (int) reader["CommentID"];
                    metricComments.SignalId = (string) reader["SignalID"];
                    metricComments.TimeStamp = (DateTime) reader["TimeStamp"];
                    metricComments.ComentText = (string) reader["CommentText"];
                    //metricComments.VersionId = (int) reader["VersionId"];
                    _oldMetricCommentsList.Add(metricComments);
                }
                sqlconn.Dispose();
            }
        }

        private static void CreateRoutes()
        {
            SPM db = new SPM();
            foreach (var oldRoute in OldRouteList)
            {
                Route route = new Route();
                route.RouteName = oldRoute.RouteName;
                route.RouteSignals = CreateRouteSignals(oldRoute, route, db);
                db.Routes.Add(route);
            }
            db.SaveChanges();
        }

        private static List<RouteSignal> CreateRouteSignals(OldRoute oldRoute, Route newRoute, SPM db)
        {
            List<RouteSignal> signals = new List<RouteSignal>();
            IApproachRepository appRepo = ApproachRepositoryFactory.Create(db);
            foreach (var detail in oldRoute.Details)
            {
                var approach = appRepo.GetApproachByApproachID(detail.ApproachId);
                RouteSignal signal = new RouteSignal();
                signal.Route = newRoute;
                signal.Order = detail.Order;
                signal.SignalId = approach.SignalID;
                signal.PhaseDirections = CreateRoutePhaseDirections(signal, detail, approach, db);
                signals.Add(signal);
            }
            return signals;
        }

        private static List<RoutePhaseDirection> CreateRoutePhaseDirections(RouteSignal signal, OldRouteDetail detail,
            Approach approach, SPM db)
        {
            List<RoutePhaseDirection> phaseDirecitons = new List<RoutePhaseDirection>();
            RoutePhaseDirection phaseDireciton = new RoutePhaseDirection();
            phaseDireciton.RouteSignal = signal;
            phaseDireciton.DirectionTypeId = approach.DirectionTypeID;
            phaseDireciton.Phase = approach.ProtectedPhaseNumber;
            phaseDireciton.IsOverlap = approach.IsProtectedPhaseOverlap;
            phaseDireciton.IsPrimaryApproach = true;
            phaseDirecitons.Add(phaseDireciton);
            return phaseDirecitons;
        }

        private static void RunMigrations()
        {
            UpdateMigrationsTable();
            var config = new global::MOE.Common.Migrations.Configuration();
            config.TargetDatabase = new DbConnectionInfo("SPM");
            var migrator = new DbMigrator(config);
            migrator.Update();
        }

        private static void UpdateMigrationsTable()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var sqlQuery = "Select * from __MigrationHistory";
            sqlconn.Open();
            var command = new SqlCommand(sqlQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                throw new Exception("No Migraitons found");
            }
            while (reader.Read())
            {
                string migrationName = reader["MigrationId"].ToString();
                if (migrationName == "201702162037176_4.0.1")
                {
                    var newMigrationName = "201704032058572_Version4.0.1";
                    SqlCommand updateCommand = new SqlCommand();
                    using (updateCommand)
                    {
                        updateCommand.Connection = GetDataBaseConnection();
                        updateCommand.Connection.Open();
                        updateCommand.CommandText = "update __MigrationHistory set MigrationId = '" + newMigrationName +
                                                    "' where MigrationId = '" + migrationName + "'";
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
            sqlconn.Dispose();
        }

        private static void GetApproachRouteRecords()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from ApproachRoute";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OldRoute route = new OldRoute();
                    route.RouteId = (int) reader["ApproachRouteId"];
                    route.RouteName = (string) reader["RouteName"];
                    OldRouteList.Add(route);
                }

                sqlconn.Dispose();
            }
        }

        private static SqlConnection GetDataBaseConnection()
        {
            SqlConnection sqlconn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SPM"].ToString()
            };
            return sqlconn;
        }

        private static void GetApproachRouteDetails()
        {
            SqlConnection sqlconn = GetDataBaseConnection();
            var slqQuery = "select * from ApproachRouteDetail";
            sqlconn.Open();
            var command = new SqlCommand(slqQuery, sqlconn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OldRouteDetail routeDetail = new OldRouteDetail();
                    routeDetail.RouteId = (int) reader["ApproachRouteId"];
                    routeDetail.Order = (int) reader["ApproachOrder"];
                    routeDetail.DetailId = (int) reader["RouteDetailID"];
                    routeDetail.ApproachId = (int) reader["ApproachID"];
                    var route = OldRouteList.Find(r => r.RouteId == routeDetail.RouteId);
                    if (route != null)
                    {
                        route.Details.Add(routeDetail);
                    }
                }
            }

            sqlconn.Dispose();
        }

        private static void UpdateMetriCommentsWithVersionId()
        {
            SPM db = new SPM();
            //db.Database.ExecuteSqlCommand(
            //    "ALTER TABLE [dbo].[MetricComments] DROP CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID])");
            var comments = (from r in db.MetricComments
                select r).ToList();
            foreach (var c in comments)
            {
                var oldSig = c.SignalID;
                c.VersionID = (from r in db.Signals
                    where r.SignalID == oldSig
                    select r.VersionID).FirstOrDefault();
            }
            db.SaveChanges();
            db.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[MetricComments] ADD CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID])");
            db.Dispose();
        }

        private static void UpdateApproachesWithVersionId()
        {
            SPM db = new SPM();
            //db.Database.ExecuteSqlCommand(
            //    "ALTER TABLE [dbo].[Approaches] DROP CONSTRAINT [FK_dbo.Approaches_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID]) ");
            var approaches = (from r in db.Approaches select r).ToList();
            foreach (var a in approaches)
            {
                a.Signal = (from r in db.Signals
                    where r.SignalID == a.SignalID
                    select r).FirstOrDefault();
            }
            db.SaveChanges();
            db.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Approaches] ADD CONSTRAINT [FK_dbo.Approaches_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID]) ");
            db.Dispose();
        }

        private static void UpdateSignalRecordsWithStartDateAndVersion()
        {
            SPM db = new SPM();
            //db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Signals] DROP CONSTRAINT [FK_dbo.Signals_dbo.VersionActions_VersionAction_ID] FOREIGN KEY ([VersionActionId]) REFERENCES [dbo].[VersionActions] ([ID])");
            var signals = (from r in db.Signals
                select r).ToList();
            var version = (from r in db.VersionActions
                where r.ID == 10
                select r).FirstOrDefault();
            var allMetricTypes = db.MetricTypes.ToList();
            foreach (var s in signals)
            {
                s.Start = s.FirstDate;
                s.VersionAction = version;
                if (s.Comments == null)
                    s.Comments = new List<MetricComment>();
                if (_oldMetricCommentsList.Any(m => m.SignalId == s.SignalID))
                {
                    var listComments = _oldMetricCommentsList.Where(m => m.SignalId == s.SignalID).ToList();
                    foreach (var comment in listComments)
                    {
                        var metricTypeRelationships = _oldMetricCommentMetricTypesList
                            .Where(t => t.MetricComment_CommentId == comment.CommentId).ToList();
                        List<MetricType> metricsToAddToComment = new List<MetricType>();
                        foreach (var metricType in allMetricTypes)
                        {
                            if (metricTypeRelationships.Select(r => r.MetricType_MetricID)
                                .Contains(metricType.MetricID))
                            {
                                metricsToAddToComment.Add(metricType);
                            }
                        }
                        s.Comments.Add(
                            new MetricComment
                            {
                                CommentText = comment.ComentText,
                                SignalID = comment.SignalId,
                                TimeStamp = comment.TimeStamp,
                                VersionID = s.VersionID, MetricTypes = metricsToAddToComment
                            });
                    }
                }
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Signals] ADD CONSTRAINT [FK_dbo.Signals_dbo.VersionActions_VersionAction_ID] FOREIGN KEY ([VersionActionId]) REFERENCES [dbo].[VersionActions] ([ID])");
            db.Dispose();
        }

        private static void CleanUpTasks()
        {
            SPM db = new SPM();
            var dbName = db.Database.Connection.DataSource;
            bool exists = db.Database
                              .SqlQuery<int?>(@"
                         SELECT 1 FROM sys.tables AS T
                         INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
                         WHERE S.Name = 'MOE1' AND T.Name = 'Speed_Events1'")
                              .SingleOrDefault() != null;
            if (exists)
            {
                db.Database.ExecuteSqlCommand("Drop table Speed_Events1");
            }
        }

        private class OldRoute
        {
            public int RouteId { get; set; }
            public string RouteName { get; set; }
            public List<OldRouteDetail> Details { get; set; }

            public OldRoute()
            {
                Details = new List<OldRouteDetail>();
            }
        }

        private class OldRouteDetail
        {
            public int RouteId { get; set; }
            public int Order { get; set; }
            public int DetailId { get; set; }
            public int ApproachId { get; set; }
        }

        private class OldMetricComments
        {
            public int CommentId { get; set; }
            public string SignalId { get; set; }
            public DateTime TimeStamp { get; set; }
            public string ComentText { get; set; }
            public int VersionId { get; set; }
        }

        private class OldMetricCommentMetricTypes
        {
            public int MetricComment_CommentId { get; set; }
            public int MetricType_MetricID { get; set; }
        }

        private class OldDetectionTypeMetricTypes
        {
            public int DetectionType_DetectionTypeID { get; set; }
            public int MetricType_MetricID { get; set; }
        }

        private class OldDetectionTypes
        {
            public int DetectionTypeID { get; set; }
            public int Description { get; set; }
        }

        private class OldActionLogMetricTypes
        {
            public int ActionLog_ActionLogID { get; set; }
            public int MetricType_MetricID { get; set; }
        }

        private class OldActionLogs
        {
            public int ActionLogID { get; set; }
            public DateTime Date { get; set; }
            public int AgencyID { get; set; }
            public string Comment { get; set; }
            public string SignalID { get; set; }
            public string Name { get; set; }
        }
    }
}

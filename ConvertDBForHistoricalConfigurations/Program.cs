using MOE.Common.Data;
using System;
using System.Collections.Generic;
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


        //Here is what has to happen:
        //Get the records from the Approachroute and ApproachRouteDetails tables BEFORE the EF migration
        //Run the EF migration
        //update the signal records with version information
        //Update Approaches with verison information
        //Update Metrics with version information
        static void Main()
        {
            GetApproachRouteRecords();
            GetApproachRouteDetails();
            RunMigrations();
            UpdateSignalRecordsWithStartDateAndVersion();
            UpdateApproachesWithVersionId();
            UpdateMetriCommentsWithVersionId();
            CreateRoutes();
            
            




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

        private static List<RoutePhaseDirection> CreateRoutePhaseDirections(RouteSignal signal, OldRouteDetail detail, Approach approach, SPM db)
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

        private static void  GetApproachRouteDetails()
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

            //db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[MetricComments] DROP CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID])");

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
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[MetricComments] ADD CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID])");

            db.Dispose();
        }

        private static void UpdateApproachesWithVersionId()
        {
            SPM db = new SPM();

            //db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Approaches] DROP CONSTRAINT [FK_dbo.Approaches_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID]) ");


            var approaches = (from r in db.Approaches
                              select r).ToList();

            foreach (var a in approaches)
            {
                a.Signal = (from r in db.Signals
                            where r.SignalID == a.SignalID
                            select r).FirstOrDefault();
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Approaches] ADD CONSTRAINT [FK_dbo.Approaches_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID]) ");

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

            foreach (var s in signals)
            {
                s.Start = s.FirstDate;
                s.VersionAction = version;
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Signals] ADD CONSTRAINT [FK_dbo.Signals_dbo.VersionActions_VersionAction_ID] FOREIGN KEY ([VersionActionId]) REFERENCES [dbo].[VersionActions] ([ID])");



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
            public int RouteId  { get; set; }
            public int Order { get; set; }
            public int DetailId { get; set; }
            public int ApproachId { get; set; }
        }

    }

    
}


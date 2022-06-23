using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;
using MOE.Common;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace InstallerLibrary
{
    public class InstallerLibrary
    {


       [CustomAction]
       public static ActionResult InitDatabase(Session session)
        {
            
            string csvPath = session["APPPATH"];
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
            new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder["User ID"] = session["SQLUSER"];
            builder["Password"] = session["SQLPASSSWORD"];
            builder["Data Source"] = session["DBSERVERNAME"]; ;
            builder["integrated Security"] = false;
            builder["Initial Catalog"] = session["DATABASENAME"];
            string connectionString = builder.ConnectionString;
            //MessageBox.Show("This is the connection string from the installer:" + connectionString, "Attach");
            //MessageBox.Show("This is the csv path from the installer" + csvPath, "Attach");
            session.Log("Begin Database Init");

             var context = new SPM(connectionString);
            if (!CheckDatabaseExists(connectionString, session["DATABASENAME"]))
            {
                
                context.Database.CommandTimeout = 180;
                context.Database.Connection.ConnectionString = connectionString;
                context.Database.Create();

            }

            WriteConfigLines(connectionString);
            return ActionResult.Success;
        }

        [CustomAction]
        public static void WriteTablesToCSV(SPM context, string csvPath)
        {
            var sr = new StreamWriter(Path.Combine(csvPath, "Signals.csv"));
            using (var csv = new CsvWriter(sr, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(context.Signals);
            }

            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(csvPath, "Approches.csv")),  CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(context.Approaches);
            }

            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(csvPath, "Detectors.csv")), CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(context.Detectors);
            }


        }

        public static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        private static void WriteConfigLines(string connectionString)
        {
            var updateAppSettings = @"UPDATE [dbo].[ApplicationSettings] set ImageUrl = 'http://localhost/atspm/ChartImages/', ImagePath = 'C:\inetpub\wwwroot\ATSPM\ChartImages\' where ApplicationID = 1";
            RunSQL(connectionString, updateAppSettings); 
        }

        private static void RunSQL(string connectionString, string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var rows = command.ExecuteNonQuery();
                }
            }
        }




    }
    
}

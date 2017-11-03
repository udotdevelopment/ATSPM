using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MOE.CommonTests.Models.Repositories
{
    [TestClass()]
    public class MigrationTests
    {


        [TestMethod]
        public void DecompressDatabaseMigration()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
            new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource ="srwtcns54";
            builder.InitialCatalog="MOE1";
            
            builder.UserID="SPM";
            builder.Password="SPM";
           
            builder.ApplicationName="MigrationViewer";
          


            string migrationName = "201710062056336_RouteConfiguration2";

            var sqlToExecute = String.Format("select model from __MigrationHistory where migrationId like '%{0}'", migrationName);

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(sqlToExecute, connection);

                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Now Rows to display. Probably migration name is incorrect");
                }

                while (reader.Read())
                {
                    var model = (byte[])reader["model"];
                    var decompressed = Decompress(model);
                    Console.WriteLine(decompressed);
                }
            }
        }

        /// <summary>
        /// Stealing decomposer from EF itself:
        /// http://entityframework.codeplex.com/SourceControl/latest#src/EntityFramework/Migrations/Edm/ModelCompressor.cs
        /// </summary>
        public virtual XDocument Decompress(byte[] bytes)
        {
            using (var memoryStream = new System.IO.MemoryStream(bytes))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    return XDocument.Load(gzipStream);
                }
            }
        }
       


       
public void DecompressMigrationEncoding()
        {
            //var migrationClass = (IMigrationMetadata)new MyMigration();
            //var target = migrationClass.Target;
            //var xmlDoc = Decompress(Convert.FromBase64String(target));
            //Console.WriteLine(xmlDoc);
        }
    }
}

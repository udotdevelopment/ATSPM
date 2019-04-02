//extern alias SharpSNMP;

using System;
using System.Data.SqlClient;

namespace MOE.Common.Business
{
    public class BulkCopyOptions
    {
        public BulkCopyOptions(string connectionString,
            string destinationTableName,
            bool writeToConsole,
            bool forceNonParallel,
            int maxThreads,
            bool deleteFiles,
            DateTime earliestAcceptableDate,
            int bulkCopyBatchSize,
            int bulkCopyTimeout)
        {
            ConnectionString = connectionString;

            if (DestinationTableName == null)
                DestinationTableName = "Controller_Event_Log";


            if (destinationTableName == null)
                DestinationTableName = "Controller_Event_Log";
            else
                DestinationTableName = destinationTableName;

            if (writeToConsole == null)
                writeToConsole = true;
            {
                WriteToConsole = writeToConsole;
            }


            if (forceNonParallel == null)
                forceNonParallel = false;
            else
                ForceNonParallel = forceNonParallel;

            if (maxThreads == null)
                maxThreads = 50;
            else
                MaxThreads = maxThreads;
            if (deleteFiles == null)
                deleteFiles = false;
            else
                DeleteFiles = deleteFiles;
            if (earliestAcceptableDate == null)
                earliestAcceptableDate = Convert.ToDateTime("01/01/2010");
            else
                EarliestAcceptableDate = earliestAcceptableDate;
            if (bulkCopyBatchSize == null)
                bulkCopyBatchSize = 5000;
            else
                bulkCopyBatchSize = BulkCopyBatchSize;
            if (bulkCopyTimeout == null)
                bulkCopyTimeout = 0;
            else
                bulkCopyTimeout = BulkCopyTimeout;

            Connection = new SqlConnection(ConnectionString);
        }

        public string ConnectionString { get; set; }
        public string DestinationTableName { get; set; }
        public bool WriteToConsole { get; set; }

        public bool ForceNonParallel { get; set; }

        public int MaxThreads { get; set; }

        public bool DeleteFiles { get; set; }

        public DateTime EarliestAcceptableDate { get; set; }

        public int BulkCopyBatchSize { get; set; }

        public int BulkCopyTimeout { get; set; }
        public SqlConnection Connection { get; set; }
    }
}
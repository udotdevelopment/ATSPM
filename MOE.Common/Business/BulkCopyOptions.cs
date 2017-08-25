//extern alias SharpSNMP;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using AlexPilotti;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Timers;

namespace MOE.Common.Business
{
    public class BulkCopyOptions
    {
        public String ConnectionString { get; set; }
        public String DestinationTableName { get; set; }
        public bool WriteToConsole { get; set; }

        public bool ForceNonParallel { get; set; }

        public int MaxThreads { get; set; }

        public bool DeleteFiles { get; set; }

        public DateTime EarliestAcceptableDate { get; set; }

        public int BulkCopyBatchSize { get; set; }

        public int BulkCopyTimeout { get; set; }
        public SqlConnection Connection { get; set; }
        




    public BulkCopyOptions(String connectionString,
  String destinationTableName,
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
            {
                DestinationTableName = "Controller_Event_Log";
            }


            if (destinationTableName == null)
            {
                DestinationTableName = "Controller_Event_Log";
            }
            else
            {
                DestinationTableName = destinationTableName;
            }

            if (writeToConsole == null)
            {
                writeToConsole = true;
            }
            {
                WriteToConsole = writeToConsole;
            }


            if (forceNonParallel == null)
            {
                forceNonParallel = false;
            }
            else
            {
                ForceNonParallel = forceNonParallel;
            }

            if (maxThreads == null)
            {
                maxThreads = 50;
            }
            else
            {
                MaxThreads = maxThreads;
            }
            if (deleteFiles == null)
            {
                deleteFiles = false;
            }
            else
            {
                DeleteFiles = deleteFiles;
            }
            if (earliestAcceptableDate == null)
            {
                earliestAcceptableDate = Convert.ToDateTime("01/01/2010");
            }
            else
            {
                EarliestAcceptableDate = earliestAcceptableDate;
            }
            if (bulkCopyBatchSize == null)
            {
                bulkCopyBatchSize = 5000;
            }
            else
            {
                bulkCopyBatchSize = BulkCopyBatchSize;
            }
            if (bulkCopyTimeout == null)
            {
                bulkCopyTimeout = 0;
            }
            else
            {
                bulkCopyTimeout = BulkCopyTimeout;
            }

            Connection = new SqlConnection(ConnectionString);
        }
    }
}

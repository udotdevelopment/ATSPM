using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ProducerConsumerASC3Import
{
    /// <summary>
    /// 
    ///     This class is useful in determine the amount of space
    ///     used in the log file for the Bulk operation.
    ///     
    ///     It makes use of the undocumented function fn_dblog which 
    ///     retrieves information about the log records.
    /// 
    /// </summary>
    class SqlLogStatistics
    {
        private string ConnectionString = "";

        /// <summary>
        /// 
        ///     Constructor for the class
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlLogStatistics(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        ///     Issues a CHECKPOINT to the server, in order to clear the 
        ///     dirty pages and prepare for a faster execution of the
        ///     next functions
        /// 
        /// </summary>
        public void IssueCheckPoint()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand checkPoint = new SqlCommand("CHECKPOINT", connection);
            connection.Open();
            checkPoint.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// 
        ///     Gets the size in MB of the data pages in the log file
        ///     for the specified table
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public double getLogSizeFor(string tableName)
        {
            double sizeInMb = 0;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(
                String.Format(" SELECT  CAST(( COALESCE(SUM(CAST ([Log Record Length] AS BIGINT)), CAST (0 AS BIGINT))) / (1024. * 1024) AS NUMERIC(12, 2)) AS size_mb ") +
                String.Format(" FROM    fn_dblog(NULL, NULL) AS D") +
                String.Format(" WHERE   AllocUnitName = '{0}'", tableName) +
                String.Format("         OR AllocUnitName LIKE '{0}.%' ;    ", tableName),
                connection);
            command.CommandTimeout = 0;
            connection.Open();
            sizeInMb = Convert.ToDouble(command.ExecuteScalar());
            connection.Close();
            return sizeInMb;
        }
    }
}

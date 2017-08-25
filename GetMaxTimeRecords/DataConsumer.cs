using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMaxTimeRecords
{
    class DataConsumer
    {
        private DataBuffer Buffer;
        private string TableName;

        /// <summary>
        /// 
        ///     Each producer needs to subscribe to the consumer, so that the
        ///     consumer knows how many producers are generating data.
        ///     
        ///     When a subscribed producers signals the end of the data, the
        ///     consumer checks if there are still producers that will generate
        ///     new rows, in order to determine if the complete flow of information
        ///     is finished or not.
        ///     
        ///     When a subscribed producer has signaled the end of data it is removed
        ///     from the list of subscribers
        /// 
        /// </summary>
        private List<MaxTimeRecordsProducer> subscribedProducers = new List<MaxTimeRecordsProducer> ();

        /// <summary>
        /// 
        ///     The following options are applied to the SqlBulkCopy class
        ///     in order to test its behaviour under specific circomstances
        /// 
        /// </summary>
        private string ConnectionString;
        public SqlBulkCopyOptions Options = SqlBulkCopyOptions.Default;
        public int BatchSize = 0;
        public int BulkCopyTimeout = 0;
        public bool SetTraceFlag610 = false;

        /// <summary>
        /// 
        ///     Constructor for the consumer.
        ///     
        ///     We receive the connectionString and the tableName, then we
        ///     simply create a new buffer in order to be able to respond
        ///     to AddRow and receive all data.
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        public DataConsumer (string connectionString, string tableName) {
            this.TableName = tableName;
            ConnectionString = Properties.Settings.Default.SPM;
            Buffer = new DataBuffer (ConnectionString, tableName);
        }

        /// <summary>
        /// 
        ///     Start consuming rows from the buffer.
        /// 
        /// </summary>
        public void Consume () {
            SqlBulkCopy bulkCopy;
            SqlConnection connection = new SqlConnection (ConnectionString);

            connection.Open ();
            try {
                if (SetTraceFlag610) {
                    SqlCommand setTraceFlag = new SqlCommand ("DBCC TRACEON (610)", connection);
                    setTraceFlag.ExecuteNonQuery ();
                } else {
                    SqlCommand setTraceFlag = new SqlCommand ("DBCC TRACEOFF (610)", connection);
                    setTraceFlag.ExecuteNonQuery ();
                }

                bulkCopy = new SqlBulkCopy (connection, Options, null);
                bulkCopy.BatchSize = BatchSize;
                bulkCopy.BulkCopyTimeout = BulkCopyTimeout;
                bulkCopy.DestinationTableName = TableName;
                bulkCopy.WriteToServer (Buffer);
                bulkCopy.Close ();
            } finally {
                connection.Close ();
            }
        }

        /// <summary>
        /// 
        ///     Adds row to the consumer. 
        ///     
        ///     This method should not be used, it is highly preferable
        ///     to use the BufferDataTable, in order to reduce the lock
        ///     contention on the buffer
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void AddRow (object [] values) {
            this.Buffer.AddRow (values);
        }

        /// <summary>
        /// 
        ///     Returns a data table that can be used as a buffer.
        ///     The producer can take it, fill it with values and 
        ///     then send it back to the buffer.
        ///     
        ///     Doing so, the lock contention of producers is highly
        ///     reduced since a lock will be issued for each data table
        ///     and not for each row
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetBufferDataTable () {
            return Buffer.GetBufferDataTable ();
        }

        /// <summary>
        /// 
        ///     Adds a new datatable as a chunk of the buffer
        ///     The data table should have been created by the
        ///     GetBufferDataTable method in order to be sure that
        ///     it is suitable for the buffer
        /// 
        /// </summary>
        /// <param name="tableToAdd"></param>
        public void AddBufferDataTable (DataTable tableToAdd) {
            Buffer.AddBufferDataTable (tableToAdd);
        }

        /// <summary>
        /// 
        ///     Returns the columns metadata
        /// 
        /// </summary>
        public DataColumnCollection ColumnsMetaData {
            get {
                return this.Buffer.ColumnsMetaData;
            }
        }

        /// <summary>
        /// 
        ///     Subscribes a producer to let him load data into
        ///     the consumer.
        /// 
        /// </summary>
        /// <param name="MaxTimeRecordsProducer"></param>
        public void Subscribe (MaxTimeRecordsProducer MaxTimeRecordsProducer) {
            lock (subscribedProducers) {
                subscribedProducers.Add (MaxTimeRecordsProducer);
            }
        }

        /// <summary>
        /// 
        ///     Signals the end of data of a specific data producer.
        ///     
        ///     If there are no data producers active yet, then we
        ///     can signal to the buffer that the data loading process
        ///     is finished-
        /// 
        /// </summary>
        public void SetEndOfData (MaxTimeRecordsProducer MaxTimeRecordsProducer) {
            lock (subscribedProducers) {
                subscribedProducers.Remove (MaxTimeRecordsProducer);
                if (subscribedProducers.Count == 0) {
                    Buffer.SetEndOfData ();
                }
            }
        }
    }
}

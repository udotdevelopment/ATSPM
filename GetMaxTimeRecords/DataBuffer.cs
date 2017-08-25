using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetMaxTimeRecords
{
  
class DataBuffer : IDataReader {
        /// <summary>
        /// 
        ///     DataBuffer represent the buffer used by consumers to provide data
        ///     to the SqlBulkCopy operation.
        ///     
        ///     It implements IDataReader in order to make the SqlBulkCopy operation
        ///     believe that it is being serviced by a continuous flow of data while,
        ///     in reality, we know that we are servicing it through many producers
        ///     producing data in an asyncronous way.
        ///     
        ///     A DataBuffer contains:
        ///     
        ///         - A List of DataTable, where each item represent a chunk of the overall
        ///           buffer. These datatables are dynamically added by the producers via
        ///           AddRow or AddBufferDataTable
        ///         - A pattern data table and related data reader. When the buffer is initially
        ///           created it does not contain any data and so all requests about its metadata
        ///           will be handled by the pattern data table and reader.
        ///         - A current data table and reader. During buffer reading, it contains the table
        ///           and related reader that is currently servicing Read operations. When the current
        ///           data table is finished, it is replaced by the next one in the Buffer list, if any
        ///         - A sempaphore (isDataAvailable) which is used by readers to stop when the buffer
        ///           list is empty and they need to wait for some data to be available
        ///     
        /// </summary>
        private int MaxRowCount = 10000;
        private int BufferLimit = 100;
        private List<DataTable> Buffer = new List<DataTable> ();
        private DataTable patternDataTable = new DataTable ();
        private DataTable currentDataTable = null;
        private IDataReader patternDataReader;
        private IDataReader currentDataReader;
        private DataTable newTable = null;
        private AutoResetEvent isDataAvailable = new AutoResetEvent (false);
        private AutoResetEvent isBufferSpaceAvailable = new AutoResetEvent (false);
        private bool EndOfData = false;

        /// <summary>
        /// 
        ///     During the construction, we create an empty data table that
        ///     will be used to store the table metadata and to create subsequent
        ///     buffers. Moreover, we create an empty DataReader, based on the
        ///     pattern data table, in order to provide the same metadata to
        ///     the caller
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        public DataBuffer (string connectionString, string tableName) {
            SqlDataAdapter dataAdapterForStructure = new SqlDataAdapter (
                String.Format ("SELECT * FROM {0} WHERE 1 = 0", tableName),
                new SqlConnection (connectionString));
            dataAdapterForStructure.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            dataAdapterForStructure.Fill (patternDataTable);
            newTable = GetBufferDataTable ();
            patternDataReader = patternDataTable.CreateDataReader ();
            currentDataReader = patternDataReader;
            currentDataTable = patternDataTable;
        }

        /// <summary>
        /// 
        ///     Returns the columns metadata of the buffer
        /// 
        /// </summary>
        public DataColumnCollection ColumnsMetaData {
            get {
                return patternDataTable.Columns;
            }
        }

        /// <summary>
        /// 
        ///     Adds a row to the buffer and returns it to the caller
        ///     
        ///     If the current data table got too big, then we add it to the buffer
        ///     and create a new one to hold data.
        /// 
        /// </summary>
        /// <returns></returns>
        public void AddRow (object[] values) {
            lock (this) {
                if (newTable.Rows.Count >= MaxRowCount) {
                    AddBufferDataTable (newTable);
                    newTable = GetBufferDataTable ();
                }
                newTable.Rows.Add (values);
            }
        }

        /// <summary>
        /// 
        ///     returns a new DataTable that can then be sent as a new
        ///     buffer, in order to reduce the contention produced by
        ///     the critical section in AddRow
        /// 
        /// </summary>
        public DataTable GetBufferDataTable () {
            return patternDataTable.Clone ();
        }

        /// <summary>
        /// 
        ///     Adds a whole datatable to the buffer
        /// 
        /// </summary>
        /// <param name="tableToAdd"></param>
        public void AddBufferDataTable (DataTable tableToAdd) {
            if (BufferCount > BufferLimit) {
                //if (!Program.serializeProcesses) {
                //    isBufferSpaceAvailable.WaitOne ();
                //}
            }
            lock (Buffer) {
                Buffer.Add (tableToAdd);
                isDataAvailable.Set ();
            }
        }

        /// <summary>
        /// 
        ///     Signals the end of the data
        /// 
        /// </summary>
        public void SetEndOfData () {
            if (newTable.Rows.Count > 0) {
                lock (Buffer) {
                    Buffer.Add (newTable);
                }
            }
            EndOfData = true;
            isDataAvailable.Set ();
        }

        /// <summary>
        /// 
        ///     Gets the number of buffers, performing the operation in 
        ///     a critical section controlled by lock(Buffer)
        /// 
        /// </summary>
        private int BufferCount {
            get {
                int result;
                lock (Buffer) {
                    result = Buffer.Count;
                }
                return result;
            }
        }

        /// <summary>
        /// 
        ///     Reads a row from the buffer and send it to the caller.
        ///     
        ///     If the current data reader still contains data, then we
        ///     can send it back and the work is done.
        ///     On the other hand, if no data is available in the current
        ///     data reader, then we need to close and free its memory and
        ///     then see if there is another buffer waiting to be used. If
        ///     no such buffer exists, then we wait on the semaphore for 
        ///     some thread to give us a buffer.
        ///     The whole process ends when EndOfData is set to true, which
        ///     means that all the producer have finished sending data and
        ///     we can safely close the process.
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Read () {
            if (currentDataReader.Read ()) {
                return true;
            } else {
                currentDataReader.Close ();
                currentDataReader.Dispose ();
                currentDataReader = null;
                currentDataTable.Dispose ();
                currentDataTable = null;
                while (BufferCount == 0) {
                    if (EndOfData) {
                        return false;
                    } else {
                        isDataAvailable.WaitOne ();
                    }
                }
                lock (Buffer) {
                    currentDataReader = Buffer[0].CreateDataReader ();
                    currentDataTable = Buffer[0];
                    Buffer.RemoveAt (0);
                    currentDataReader.Read ();
                    isBufferSpaceAvailable.Set ();
                }
                return true;
            }
        }

        public void Dispose () {

        }

        /// <summary>
        /// 
        ///     During close, we release all the memory still occupied
        ///     by the buffers.
        /// 
        /// </summary>
        public void Close () {
            patternDataReader.Close ();

            if (currentDataReader != null) {
                currentDataReader.Close ();
                currentDataReader.Dispose ();
                currentDataReader = null;
            }

            if (currentDataTable != null) {
                currentDataTable.Dispose ();
                currentDataTable = null;
            }
            Buffer.Clear ();
        }

        public int Depth {
            get { return currentDataReader.Depth; }
        }

        public DataTable GetSchemaTable () {
            return patternDataReader.GetSchemaTable ();
        }

        public bool IsClosed {
            get { return false; }
        }

        public bool NextResult () {
            return false;
        }


        public int RecordsAffected {
            get { return currentDataReader.RecordsAffected; }
        }

        public int FieldCount {
            get { return patternDataReader.FieldCount; }
        }

        public bool GetBoolean (int i) {
            return currentDataReader.GetBoolean (i);
        }

        public byte GetByte (int i) {
            return currentDataReader.GetByte (i);
        }

        public long GetBytes (int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
            return currentDataReader.GetBytes (i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar (int i) {
            return currentDataReader.GetChar (i);
        }

        public long GetChars (int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            return currentDataReader.GetChars (i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData (int i) {
            return currentDataReader.GetData (i);
        }

        public string GetDataTypeName (int i) {
            return patternDataReader.GetDataTypeName (i);
        }

        public DateTime GetDateTime (int i) {
            return currentDataReader.GetDateTime (i);
        }

        public decimal GetDecimal (int i) {
            return currentDataReader.GetDecimal (i);
        }

        public double GetDouble (int i) {
            return currentDataReader.GetDouble (i);
        }

        public Type GetFieldType (int i) {
            return patternDataReader.GetFieldType (i);
        }

        public float GetFloat (int i) {
            return currentDataReader.GetFloat (i);
        }

        public Guid GetGuid (int i) {
            return currentDataReader.GetGuid (i);
        }

        public short GetInt16 (int i) {
            return currentDataReader.GetInt16 (i);
        }

        public int GetInt32 (int i) {
            return currentDataReader.GetInt32 (i);
        }

        public long GetInt64 (int i) {
            return currentDataReader.GetInt64 (i);
        }

        public string GetName (int i) {
            return patternDataReader.GetName (i);
        }

        public int GetOrdinal (string name) {
            return currentDataReader.GetOrdinal (name);
        }

        public string GetString (int i) {
            return currentDataReader.GetString (i);
        }

        public object GetValue (int i) {
            return currentDataReader.GetValue (i);
        }

        public int GetValues (object[] values) {
            return currentDataReader.GetValues (values);
        }

        public bool IsDBNull (int i) {
            return currentDataReader.IsDBNull (i);
        }

        public object this[string name] {
            get { return currentDataReader[name]; }
        }

        public object this[int i] {
            get { return currentDataReader[i]; }
        }

    }
}

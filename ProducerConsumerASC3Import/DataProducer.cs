using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProducerConsumerASC3Import
{
    class DataProducer
    {
        /// <summary>
        /// 
        ///     Random generator used to create new random rows
        /// 
        /// </summary>
        //Random randomGenerator = new Random();

        /// <summary>
        /// 
        ///     Produces rows and puts them into the buffer of the consumer
        ///     which will, in turn, send them to the SqlBulkCopy operation
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void Produce(DataConsumer consumer, int numberOfRows)
        {
            int bufferSize = 100000;
            int numberOfBuffers = numberOfRows / bufferSize;

            for (int bufferNumber = 0; bufferNumber < numberOfBuffers; bufferNumber++)
            {
                DataTable buffer = consumer.GetBufferDataTable();

                for (int rowNumber = 0; rowNumber < bufferSize; rowNumber++)
                {
                    object[] values = GetRandomRow(consumer);
                    buffer.Rows.Add(values);
                }
                consumer.AddBufferDataTable(buffer);
            }
        }

        /// <summary>
        /// 
        ///     Here we create a list of random values that will be used
        ///     to fill a single row
        /// 
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns></returns>
        private object[] GetRandomRow(DataConsumer consumer)
        {
            object[] values = new object[consumer.ColumnsMetaData.Count];

            for (int i = 0; i < consumer.ColumnsMetaData.Count; i++)
            {
                DataColumn column = consumer.ColumnsMetaData[i];

                if (column.DataType == typeof(int))
                {
                    values[i] = randomGenerator.Next();
                }
                else if (column.DataType == typeof(string))
                {
                    string result = "";
                    while (result.Length < column.MaxLength)
                    {
                        result += Guid.NewGuid().ToString();
                    }
                    values[i] = result.Substring(0, column.MaxLength);
                }
            }
            return values;
        }
    }
}

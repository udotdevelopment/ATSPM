using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumerASC3Import{
  class Program {
        //
        //  Program configuration
        //
        static string connectionString = @"Data Source=srwtcns54;Initial Catalog=MOETest;Integrated Security=True";
        static string tableName = "Controller_Event_Log";
        static int numberOfProducers = 8;
        static int numberOfConsumers = 8;
        static int numberOfRows = 4000000;
        public static bool serializeProcesses = true;
        static bool truncateTableBeforeLoading = true;
        static bool setTraceFlag610 = false;

        /// <summary>
        /// 
        ///     BulkCopy options
        /// 
        /// </summary>
        static int consumerBatchSize = 5000;
        static int consumerTimeout = 0;
        static SqlBulkCopyOptions consumerOptions = SqlBulkCopyOptions.Default; // SqlBulkCopyOptions.TableLock;

        /// <summary>
        /// 
        ///     Logging variables, to get time of execution of various steps
        /// 
        /// </summary>
        static DateTime startTime;
        static DateTime endTime;
        static SqlLogStatistics logStatistics = new SqlLogStatistics (connectionString);

        //
        //  Private variables, to hold consume and producer threads
        //
        static private List<Thread> producerThreads = new List<Thread> ();
        static private List<Thread> consumerThreads = new List<Thread> ();
        static private List<DataConsumer> dataConsumers = new List<DataConsumer> ();

        /// <summary>
        /// 
        ///     Main program
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main (string[] args) {

            Console.WriteLine ("SqlBulkCopy tester - (C) 2009 SQLBI.COM");
            Console.WriteLine ();
            Console.WriteLine (String.Format ("Number of producers: {0:N0}", numberOfProducers));
            Console.WriteLine (String.Format ("Number of consumers: {0:N0}", numberOfConsumers));
            Console.WriteLine (String.Format ("Number of rows     : {0:N0}", numberOfRows));
            Console.WriteLine (String.Format ("Batch size         : {0:N0}", consumerBatchSize));
            Console.WriteLine (String.Format ("Table Locking      : {0}", IsOptionActive (consumerOptions, SqlBulkCopyOptions.TableLock)));
            Console.WriteLine (String.Format ("Transaction        : {0}", IsOptionActive (consumerOptions, SqlBulkCopyOptions.UseInternalTransaction)));

            if (numberOfProducers % numberOfConsumers != 0) {
                Console.WriteLine ();
                Console.WriteLine ("Warning, the load on consumers is not perfectly balanced!");
            }
            
            Console.WriteLine ();

            CreateConsumers ();
            CreateProducers ();

            //
            //  If requested, we truncate the table before loading it with data,
            //  in order not to make the database grow during loading
            //
            if (truncateTableBeforeLoading) {
                TruncateTable ();
            }

            logStatistics.IssueCheckPoint ();

            if (serializeProcesses) {
                StartTimer ();
                Startproducers ();
                WaitForProducers ();
                StopTimer ();
                ShowElapsed ("Produce data       : ");

                StartTimer ();
                StartConsumers ();
                WaitForConsumers ();

                StopTimer ();
                ShowElapsed ("Consume data       : ");
            } else {
                StartTimer ();
                Startproducers ();
                StartConsumers ();
                WaitForConsumers ();
                WaitForProducers ();

                StopTimer ();
                ShowElapsed ("Produce/Consume    : ");
            }

            Console.WriteLine (String.Format ("Log file usage     : {0:N2} Mb", logStatistics.getLogSizeFor (tableName)));

            Console.ReadLine ();
        }

        /// <summary>
        /// 
        ///     Returns a string representation of the fact that a specific option is
        ///     activated or not
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="optionToCheck"></param>
        /// <returns></returns>
        static string IsOptionActive (SqlBulkCopyOptions options, SqlBulkCopyOptions optionToCheck) {
            if ((options & optionToCheck) == optionToCheck) {
                return "Activated";
            } else {
                return "Disabled";
            }
        }

        /// <summary>
        /// 
        ///     Truncates the table before loading it, so that we do not
        ///     count the space needed for the database to grow, in the
        ///     case where it needs to
        /// 
        /// </summary>
        private static void TruncateTable () {
            SqlConnection truncateConnection = new SqlConnection (connectionString);
            SqlCommand cmdTruncate = new SqlCommand ("TRUNCATE TABLE " + tableName, truncateConnection);
            truncateConnection.Open ();
            cmdTruncate.ExecuteNonQuery ();
            truncateConnection.Close ();
        }

        /// <summary>
        /// 
        ///     Shows the elapsed time
        /// 
        /// </summary>
        static void ShowElapsed (string operation) {
            TimeSpan elapsed = endTime - startTime;
            Console.WriteLine ("{0}{1:N0} milliseconds", operation, elapsed.TotalMilliseconds);
        }

        /// <summary>
        /// 
        ///     Starts the timer
        /// 
        /// </summary>
        private static void StartTimer () {
            startTime = DateTime.Now;
        }

        /// <summary>
        /// 
        ///     Stops the timer
        /// 
        /// </summary>
        private static void StopTimer () {
            endTime = DateTime.Now;
        }

        /// <summary>
        /// 
        ///     Creates the array of consumers
        /// 
        /// </summary>
        private static void CreateConsumers () {
            for (int i = 0; i < numberOfConsumers; i++) {
                DataConsumer consumer = new DataConsumer (connectionString, tableName);
                consumer.Options = consumerOptions;
                consumer.BatchSize = consumerBatchSize;
                consumer.BulkCopyTimeout = consumerTimeout;
                consumer.SetTraceFlag610 = setTraceFlag610;
                dataConsumers.Add (consumer);

                Thread newThread = new Thread (delegate () {
                    consumer.Consume ();
                });
                consumerThreads.Add (newThread);
            }
        }

        /// <summary>
        /// 
        ///     Creates the array of producers threads
        ///
        ///     Each producer will provide rows to a consumer. The consumer is 
        ///     computer using the modulo operator (%) in order to get a 
        ///     round robin policy between consumers.
        ///     
        ///     Please note that if numberOfProducers % numberOfConsumers is not
        ///     zero (that is one is not a multiple of the other) then the load
        ///     of rows will be unbalanced and the final result will be more 
        ///     difficult to understand, since some consumer threads will have
        ///     processed more rows.
        /// 
        /// </summary>
        private static void CreateProducers () {
            for (int i = 0; i < numberOfProducers; i++) {
                int consumerIndex = i % numberOfConsumers;
                DataConsumer consumer = dataConsumers[consumerIndex];
                DataProducer producer = new DataProducer ();

                //
                //  Please note that we need to subscribe before to start
                //  all the threads, since a single thread might not be able
                //  to subscribe before all the others have ended sending rows
                //  and this would cause the consumer to stop receiving rows.
                //
                consumer.Subscribe (producer);

                Thread newThread = new Thread (delegate () {
                    producer.Produce (consumer, numberOfRows / numberOfProducers);
                    consumer.SetEndOfData (producer);
                });
                producerThreads.Add (newThread);
            }
        }

        /// <summary>
        /// 
        ///     Starts the consumers
        /// 
        /// </summary>
        static void StartConsumers () {
            foreach (Thread consumerThread in consumerThreads) {
                consumerThread.Start ();
            }
        }

        /// <summary>
        /// 
        ///     Starts the producers.
        ///     
        /// 
        /// </summary>
        static void Startproducers () {
            foreach (Thread producerThread in producerThreads) {
                producerThread.Start ();
            }
        }

        /// <summary>
        /// 
        ///     Waits for all the producers to stop producing data and
        ///     sets the EndOfData for all the consumers
        /// 
        /// </summary>
        static void WaitForProducers () {
            foreach (Thread producer in producerThreads) {
                producer.Join ();
            }
        }

        /// <summary>
        /// 
        ///     Waits for all the consumers to terminate
        /// 
        /// </summary>
        static void WaitForConsumers () {
            foreach (Thread consumer in consumerThreads) {
                consumer.Join ();
            }
        }
    }
}
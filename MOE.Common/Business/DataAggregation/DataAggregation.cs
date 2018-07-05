using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Business.PEDDelay;
using MOE.Common.Business.Speed;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class DataAggregation
    {
        private readonly ConcurrentQueue<ApproachCycleAggregation> _approachCycleAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachCycleAggregation>();

        private readonly ConcurrentQueue<ApproachPcdAggregation> _approachPcdAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachPcdAggregation>();

        private readonly ConcurrentQueue<ApproachSpeedAggregation> _approachSpeedAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSpeedAggregation>();

        private readonly ConcurrentQueue<ApproachSplitFailAggregation> _approachSplitFailAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSplitFailAggregation>();

        private readonly ConcurrentQueue<ApproachYellowRedActivationAggregation>
            _approachYellowRedActivationAggregationConcurrentQueue =
                new ConcurrentQueue<ApproachYellowRedActivationAggregation>();

        private readonly ConcurrentQueue<DetectorAggregation> _detectorAggregationConcurrentQueue =
            new ConcurrentQueue<DetectorAggregation>();

        private readonly ConcurrentQueue<PreemptionAggregation> _preemptAggregationConcurrentQueue =
            new ConcurrentQueue<PreemptionAggregation>();

        private readonly ConcurrentQueue<PriorityAggregation> _priorityAggregationConcurrentQueue =
            new ConcurrentQueue<PriorityAggregation>();

        private readonly ConcurrentQueue<PhaseTerminationAggregation> _phaseTerminationAggregationQueue =
            new ConcurrentQueue<PhaseTerminationAggregation>();
        private readonly ConcurrentQueue<PhasePedAggregation> _phasePedAggregations =
            new ConcurrentQueue<PhasePedAggregation>();

        private DateTime _endDate;

        private DateTime _startDate;
        private ConcurrentQueue<SignalEventCountAggregation> _signalEventAggregationConcurrentQueue = new ConcurrentQueue<SignalEventCountAggregation>();
        private ConcurrentQueue<ApproachEventCountAggregation> _approachEventAggregationConcurrentQueue = new ConcurrentQueue<ApproachEventCountAggregation>();


        public void StartAggregation(string[] args)
        {
            var signalRep =
                SignalsRepositoryFactory.Create();

            var appSettings = ConfigurationManager.AppSettings;
            var binSize = Convert.ToInt32(appSettings["BinSize"]);
            SetStartEndDate(args);
            Console.WriteLine("Starting " + _startDate.ToShortDateString());
            var db = new SPM();
            db.Configuration.LazyLoadingEnabled = false;

            var options = new ParallelOptions {MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"])};
            for (var dt = _startDate; dt < _endDate.AddDays(1); dt = dt.AddMinutes(binSize))
            {
                var tempSignalIds = new List<string>
                {
                    "1094",
                    "1095",
                    "1096",
                    "1097",
                    "7180",
                    "7181",
                    "7182",
                    "7183",
                    "7184",
                    "7185",
                    "7186",
                    "7187",
                    "7076",
                    "7188",
                    "7189",
                    "7190",
                    "7191",
                    "7192",
                    "7193",
                    "6418",
                    "6421",
                    "7076"
                };

                Console.WriteLine("Getting correct version of signals for time period");
                var versionIds = db.Signals.Where(r =>
                        r.VersionActionId != 3 && r.Start < dt && tempSignalIds.Contains(r.SignalID)
                        )
                    .GroupBy(r => r.SignalID)
                    .Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault()).Select(s => s.VersionID).ToList();
                var signals = db.Signals
                    .Where(signal => versionIds.Contains(signal.VersionID))
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                    .Include(signal => signal.Approaches.Select(a =>
                        a.Detectors.Select(d => d.DetectionTypes.Select(det => det.MetricTypes))))
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                    .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                    .ToList();
                Console.WriteLine("Begin Aggregating Signals");
                Parallel.ForEach(signals, signal =>
                //foreach (var signal in signals)
                {
                    Console.WriteLine(signal.SignalID + " " + dt.ToString());
                    ProcessSignal(signal, dt, dt.AddMinutes(binSize));
                });
                BulkSaveAllAggregateDataInParallel();
            }
            _startDate = _startDate.AddDays(1);
        }

        private void BulkSaveAllAggregateDataInParallel()
        {
            Parallel.Invoke(
                () =>
                {
                    if (_approachCycleAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Approach Cycle Data to Database...");
                        BulkSaveApproachCycleData();
                    }
                },
                () =>
                {
                    if (_approachPcdAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Approach PCD Data to Database...");
                        BulkSaveApproachPcdData();
                    }
                },

                () =>
                {
                    if (_approachSplitFailAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Approach Split Fail Data to Database...");
                        BulkSaveApproachSplitFailData();
                    }
                },

                () =>
                {
                    if (_approachYellowRedActivationAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Approach Yellow Red Activations Data to Database...");
                        BulkSaveApproachYellowRedActivationsData();
                    }
                },

                () =>
                {
                    if (_approachSpeedAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Approach Speed Data to Database...");
                        BulkSaveApproachSpeedData();
                    }
                },

                () =>
                {
                    if (_detectorAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Detector Data to Database...");
                        BulkSaveDetectorData();
                    }
                },

                () =>
                {
                    if (_priorityAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Priority Data to Database...");
                        BulkSavePriorityData();
                    }
                },

                () =>
                {
                    if (_preemptAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Preempt Data to Database...");
                        BulkSavePreemptData();
                    }
                },

                () =>
                {
                    if (_signalEventAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Signal Event Data to Database...");
                        BulkSaveSignalEventData();
                    }
                },

                () => 
                { 
                if(_phaseTerminationAggregationQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Signal Event Data to Database...");
                        BulkSavePhaseTerminationData();
                    }
                },

                () =>
                {
                    if (_approachEventAggregationConcurrentQueue.Count > 0)
                    {
                        Console.WriteLine("Saving Phase Event Data to Database...");
                        BulkSavePhaseEventData();
                    }
                });
        }

        private void BulkSavePhaseEventData()
        {
            var eventAggregationTable = new DataTable();
            eventAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            eventAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            eventAggregationTable.Columns.Add(new DataColumn("ApproachId", typeof(int)));
            eventAggregationTable.Columns.Add(new DataColumn("EventCount", typeof(int)));
            eventAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));

            while (_approachEventAggregationConcurrentQueue.TryDequeue(out var preemptionAggregation))
            {
                var dataRow = eventAggregationTable.NewRow();
                dataRow["BinStartTime"] = preemptionAggregation.BinStartTime;
                dataRow["ApproachId"] = preemptionAggregation.ApproachId;
                dataRow["EventCount"] = preemptionAggregation.EventCount;
                dataRow["IsProtectedPhase"] = preemptionAggregation.IsProtectedPhase;
                eventAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachEventCountAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(eventAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Phase Event Count Data");
                }
            }
        }
        private void BulkSaveSignalEventData()
        {
            var eventAggregationTable = new DataTable();
            eventAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            eventAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            eventAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            eventAggregationTable.Columns.Add(new DataColumn("EventCount", typeof(int)));

            while (_signalEventAggregationConcurrentQueue.TryDequeue(out var preemptionAggregation))
            {
                var dataRow = eventAggregationTable.NewRow();
                dataRow["BinStartTime"] = preemptionAggregation.BinStartTime;
                dataRow["SignalId"] = preemptionAggregation.SignalId;
                dataRow["EventCount"] = preemptionAggregation.EventCount;
                eventAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "SignalEventCountAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(eventAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Signal Event Count Data");
                }
            }
        }

        private void BulkSavePreemptData()
        {
            var preemptAggregationTable = new DataTable();
            preemptAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            preemptAggregationTable.Columns.Add(new DataColumn("SignalID", typeof(string)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptNumber", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptRequests", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptServices", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("VersionId", typeof(int)));

            while (_preemptAggregationConcurrentQueue.TryDequeue(out var preemptionAggregation))
            {
                var dataRow = preemptAggregationTable.NewRow();
                dataRow["BinStartTime"] = preemptionAggregation.BinStartTime;
                dataRow["SignalID"] = preemptionAggregation.SignalId;
                dataRow["PreemptNumber"] = preemptionAggregation.PreemptNumber;
                dataRow["PreemptRequests"] = preemptionAggregation.PreemptRequests;
                dataRow["PreemptServices"] = preemptionAggregation.PreemptServices;
                dataRow["VersionId"] = preemptionAggregation.VersionId;
                preemptAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PreemptionAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(preemptAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Preempt data");
                }
            }
        }

        private void BulkSavePriorityData()
        {
            var priorityAggregationTable = new DataTable();
            priorityAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            priorityAggregationTable.Columns.Add(new DataColumn("SignalID", typeof(string)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityNumber", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("TotalCycles", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityRequests", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityServiceEarlyGreen", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityServiceExtendedGreen", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("VersionId", typeof(int)));

            while (_priorityAggregationConcurrentQueue.TryDequeue(out var priorityAggregationData))
            {
                var dataRow = priorityAggregationTable.NewRow();
                dataRow["BinStartTime"] = priorityAggregationData.BinStartTime;
                dataRow["SignalID"] = priorityAggregationData.SignalID;
                dataRow["PriorityNumber"] = priorityAggregationData.PriorityNumber;
                dataRow["TotalCycles"] = priorityAggregationData.TotalCycles;
                dataRow["PriorityRequests"] = priorityAggregationData.PriorityRequests;
                dataRow["PriorityServiceEarlyGreen"] = priorityAggregationData.PriorityServiceEarlyGreen;
                dataRow["PriorityServiceExtendedGreen"] = priorityAggregationData.PriorityServiceExtendedGreen;
                dataRow["VersionId"] = priorityAggregationData.VersionId;
                priorityAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PriorityAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(priorityAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Priority Data");
                }
            }
        }

        private void BulkSaveDetectorData()
        {
            var detectorAggregationTable = new DataTable();
            detectorAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            detectorAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            detectorAggregationTable.Columns.Add(new DataColumn("Volume", typeof(double)));
            detectorAggregationTable.Columns.Add(new DataColumn("DetectorPrimaryId", typeof(string)));
            while (_detectorAggregationConcurrentQueue.TryDequeue(out var detectorAggregationData))
            {
                var dataRow = detectorAggregationTable.NewRow();
                dataRow["BinStartTime"] = detectorAggregationData.BinStartTime;
                dataRow["DetectorPrimaryId"] = detectorAggregationData.DetectorPrimaryId;
                dataRow["Volume"] = detectorAggregationData.Volume;
                detectorAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "DetectorAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(detectorAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Detector Data");
                }
            }
        }

        private void BulkSaveApproachSpeedData()
        {
            var approachSpeedAggregationTable = new DataTable();
            approachSpeedAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("SummedSpeed", typeof(double)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("SpeedVolume", typeof(double)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("Speed85th", typeof(double)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("Speed15th", typeof(double)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachSpeedAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachSpeedAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SummedSpeed"] = approachAggregationData.SummedSpeed;
                dataRow["SpeedVolume"] = approachAggregationData.SpeedVolume;
                dataRow["Speed85th"] = approachAggregationData.Speed85Th;
                dataRow["Speed15th"] = approachAggregationData.Speed15Th;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachSpeedAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachSpeedAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(approachSpeedAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Speed Data");
                }
            }
        }

        private void BulkSaveApproachCycleData()
        {
            var approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("RedTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("YellowTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("GreenTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalCycles", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachCycleAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["RedTime"] = approachAggregationData.RedTime;
                dataRow["YellowTime"] = approachAggregationData.YellowTime;
                dataRow["GreenTime"] = approachAggregationData.GreenTime;
                dataRow["TotalCycles"] = approachAggregationData.TotalCycles;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachCycleAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(approachAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Approach Cycle Data");
                }
            }
        }

        private void BulkSaveApproachPcdData()
        {
            var approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnGreen", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnRed", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnYellow", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            approachAggregationTable.Columns.Add(new DataColumn("Volume", typeof(bool)));
            while (_approachPcdAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["ArrivalsOnGreen"] = approachAggregationData.ArrivalsOnGreen;
                dataRow["ArrivalsOnRed"] = approachAggregationData.ArrivalsOnRed;
                dataRow["ArrivalsOnYellow"] = approachAggregationData.ArrivalsOnYellow;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                dataRow["Volume"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachPcdAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(approachAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Approach PCD Data");
                }
            }
        }

        private void BulkSaveApproachSplitFailData()
        {
            var approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("SplitFailures", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachSplitFailAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SplitFailures"] = approachAggregationData.SplitFailures;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachSplitFailAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(approachAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Split Fail Data");
                }
            }
        }

        private void BulkSavePhaseTerminationData()
        {
            var phaseTerminationAggregationTable = new DataTable();
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("GapOuts", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("ForceOffs", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("MaxOuts", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("UnknownTerminationTypes", typeof(int)));
            while (_phaseTerminationAggregationQueue.TryDequeue(out var phaseTerminationAggregation))
            {
                var dataRow = phaseTerminationAggregationTable.NewRow();
                dataRow["BinStartTime"] = phaseTerminationAggregation.BinStartTime;
                dataRow["SignalId"] = phaseTerminationAggregation.SignalId;
                dataRow["PhaseNumber"] = phaseTerminationAggregation.PhaseNumber;
                dataRow["GapOuts"] = phaseTerminationAggregation.GapOuts;
                dataRow["ForceOffs"] = phaseTerminationAggregation.ForceOffs;
                dataRow["MaxOuts"] = phaseTerminationAggregation.MaxOuts;
                dataRow["UnknownTerminationTypes"] = phaseTerminationAggregation.UnknownTerminationTypes;
                phaseTerminationAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PhaseTerminationAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(phaseTerminationAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Ped Aggregation Data");
                }
            }
        }


        private void BulkSavePedData()
        {
            var phasePedAggregationTable = new DataTable();
            phasePedAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            phasePedAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedDelay", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedCount", typeof(int)));
            while (_phasePedAggregations.TryDequeue(out var phaseTerminationAggregation))
            {
                var dataRow = phasePedAggregationTable.NewRow();
                dataRow["BinStartTime"] = phaseTerminationAggregation.BinStartTime;
                dataRow["SignalId"] = phaseTerminationAggregation.SignalId;
                dataRow["PhaseNumber"] = phaseTerminationAggregation.PhaseNumber;
                dataRow["PedDelay"] = phaseTerminationAggregation.PedDelay;
                dataRow["PedCount"] = phaseTerminationAggregation.PedCount;
                phasePedAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PhasePedAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(phasePedAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Ped Data");
                }
            }
        }


        private void BulkSaveApproachYellowRedActivationsData()
        {
            var approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("SevereRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachYellowRedActivationAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SevereRedLightViolations"] = approachAggregationData.SevereRedLightViolations;
                dataRow["TotalRedLightViolations"] = approachAggregationData.TotalRedLightViolations;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            var connectionString =
                ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachYellowRedActivationAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(approachAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("Unable to import Yellow and Red Acvtivation Data");
                }
            }
        }


        public void SetStartEndDate(string[] args)
        {
            _startDate = DateTime.Today;
            if (args.Length == 1)
            {
                _startDate = Convert.ToDateTime(args[0]);
                _endDate = DateTime.Today;
            }
            else if (args.Length == 2)
            {
                _startDate = Convert.ToDateTime(args[0]);
                _endDate = Convert.ToDateTime(args[1]);
            }
            else
            {
                try
                {
                    var db = new SPM();
                    _startDate = db.ApproachPcdAggregations.Select(s => s.BinStartTime).Max().AddMinutes(15);
                }
                catch (Exception)
                {
                    _startDate = DateTime.Today.AddDays(-1);
                    _endDate = DateTime.Today;
                }
            }
        }

        private void ProcessSignal(Models.Signal signal, DateTime startTime, DateTime endTime)
        {
            // Console.Write("-Preempt/Priority data ");
            //DateTime dt = DateTime.Now;
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
            var preemptCodes = new List<int> {102, 105};
            var priorityCodes = new List<int> {112, 113, 114};
            Parallel.Invoke(
                () =>
                {
                    int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
                    _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
                    {
                        BinStartTime = startTime,
                        EventCount = eventCount,
                        SignalId = signal.SignalID
                    });
                },
                () =>
                {
                    AggregatePhaseTerminations(startTime, endTime, signal);
                },
                () =>
                {
                    AggregatePedDelay(startTime, endTime, signal);
                },
                () =>
                {
                if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
                    AggregatePreemptCodes(startTime, records, signal, preemptCodes);
                },
                () =>
                {
                    if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
                        AggregatePriorityCodes(startTime, records, signal, priorityCodes);
                },
                () =>
                {
                    if (signal.Approaches != null)
                        ProcessApproach(signal, startTime, endTime, records);
                }
            );
        }

        private void AggregatePedDelay(DateTime startTime, DateTime endTime, Models.Signal signal)
        {
            PedDelaySignal pedDelaySignal = new PedDelaySignal(signal.SignalID, startTime, endTime);
            foreach (var pedPhase in pedDelaySignal.PedPhases)
            {
                PhasePedAggregation pedAggregation = new PhasePedAggregation
                {
                    SignalId = signal.SignalID,
                    PhaseNumber = pedPhase.PhaseNumber,
                    BinStartTime = startTime,
                    PedCount =  pedPhase.Cycles.Count,
                    PedDelay = pedPhase.TotalDelay,
                };
                _phasePedAggregations.Enqueue(pedAggregation);
            }
        }

        private void AggregatePhaseTerminations(DateTime startTime, DateTime endTime, Models.Signal signal)
        {
            AnalysisPhaseCollection analysisPhaseCollection = new AnalysisPhaseCollection(signal.SignalID, startTime, endTime, 1);
            foreach (var analysisPhase in analysisPhaseCollection.Items)
            {
                PhaseTerminationAggregation phaseTerminationAggregation = new PhaseTerminationAggregation
                {
                    BinStartTime = startTime,
                    SignalId = signal.SignalID,
                    ForceOffs = analysisPhase.ConsecutiveForceOff.Count,
                    MaxOuts = analysisPhase.ConsecutiveMaxOut.Count,
                    GapOuts = analysisPhase.ConsecutiveGapOuts.Count,
                    PhaseNumber = analysisPhase.PhaseNumber,
                    UnknownTerminationTypes = analysisPhase.UnknownTermination.Count
                };
                _phaseTerminationAggregationQueue.Enqueue(phaseTerminationAggregation);
            }
        }

        private void ProcessApproach(Models.Signal signal, DateTime startTime, DateTime endTime,
            List<Controller_Event_Log> records)
        {

            if (signal.Approaches != null)
                Parallel.ForEach(signal.Approaches, signalApproach =>
                    //foreach (var signalApproach in signal.Approaches)
                {
                    if (signalApproach.Detectors != null && signalApproach.Detectors.Count > 0)
                        Parallel.Invoke(
                            () => { SetApproachSpeedAggregationData(startTime, endTime, signalApproach); },
                            () => { SetApproachAggregationData(startTime, endTime, records, signalApproach); },
                            () => { SetDetectorAggregationData(startTime, endTime, signalApproach); }
                        );
                });
        }

        private void SetDetectorAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach)
        {
            //Console.Write("\n-Aggregate Detector data ");
            //DateTime dt = DateTime.Now;
            //Parallel.ForEach(signalApproach.Detectors, detector =>
            foreach (var detector in signalApproach.Detectors)
            {
                var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
                var count = controllerEventLogRepository.GetDetectorActivationCount(signalApproach.SignalID, startTime,
                    endTime, detector.DetChannel);
                var detectorAggregation = new DetectorAggregation
                {
                    DetectorPrimaryId = detector.ID,
                    BinStartTime = startTime,
                    Volume = count
                };
                _detectorAggregationConcurrentQueue.Enqueue(detectorAggregation);
            }//);
            //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
        }


        private void SetApproachAggregationData(DateTime startTime, DateTime endTime,
            List<Controller_Event_Log> records, Approach approach)
        {
            SetSplitFailData(startTime, endTime, approach, false);
            var signalPhase = new SignalPhase(startTime, endTime, approach, false, 15, 6, false);
            var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            int eventCount =
                controllerEventLogRepository.GetApproachEventsCountBetweenDates(approach.ApproachID,
                    startTime, endTime, approach.ProtectedPhaseNumber);
            Parallel.Invoke(() => { SetApproachCycleData(signalPhase, startTime, approach, records, false); },
                () => { SetApproachPcdData(signalPhase, startTime, approach); },
                () => { SetSplitFailData(startTime, endTime, approach, false); },
                () => { SetYellowRedActivationData(startTime, endTime, approach, false); });
            if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
            {
                SetSplitFailData(startTime, endTime, approach, true);
                eventCount +=
                    controllerEventLogRepository.GetApproachEventsCountBetweenDates(approach.ApproachID,
                        startTime, endTime, (int)approach.PermissivePhaseNumber);
                var permissiveSignalPhase = new SignalPhase(startTime, endTime, approach, false, 15, 6, true);
                Parallel.Invoke(
                    () => { SetApproachCycleData(permissiveSignalPhase, startTime, approach, records, true); },
                    () => { SetApproachPcdData(permissiveSignalPhase, startTime, approach); },
                    () => { SetSplitFailData(startTime, endTime, approach, true); },
                    () => { SetYellowRedActivationData(startTime, endTime, approach, true); });
            }
            _approachEventAggregationConcurrentQueue.Enqueue(new ApproachEventCountAggregation
            {
                BinStartTime = startTime,
                EventCount = eventCount,
                ApproachId = approach.ApproachID,
                IsProtectedPhase = approach.IsProtectedPhaseOverlap
            });

        }

        private void SetYellowRedActivationData(DateTime startTime, DateTime endTime, Approach approach,
            bool isPermissivePhase)
        {
            if (approach.GetDetectorsForMetricType(11).Any())
            {
                //Console.Write("\n-Aggregate Yellow Red data ");
                //DateTime dt = DateTime.Now;
                var options = new YellowAndRedOptions();
                options.SetDefaults();
                options.StartDate = startTime;
                options.EndDate = endTime;
                var yellowRedAcuationsPhase = new RLMSignalPhase(startTime, endTime, 15, options.SevereLevelSeconds,
                    approach, isPermissivePhase);
                _approachYellowRedActivationAggregationConcurrentQueue.Enqueue(
                    new ApproachYellowRedActivationAggregation
                    {
                        ApproachId = approach.ApproachID,
                        BinStartTime = startTime,
                        SevereRedLightViolations = Convert.ToInt32(yellowRedAcuationsPhase.SevereRedLightViolations),
                        TotalRedLightViolations = Convert.ToInt32(yellowRedAcuationsPhase.Violations),
                        IsProtectedPhase = approach.IsProtectedPhaseOverlap
                    });
                //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
            }
        }

        private void SetApproachPcdData(SignalPhase signalPhase, DateTime startTime, Approach approach)
        {
            if (approach.GetDetectorsForMetricType(6).Any())
                _approachPcdAggregationConcurrentQueue.Enqueue(new ApproachPcdAggregation
                {
                    ApproachId = approach.ApproachID,
                    ArrivalsOnGreen = Convert.ToInt32(signalPhase.TotalArrivalOnGreen),
                    ArrivalsOnRed = Convert.ToInt32(signalPhase.TotalArrivalOnRed),
                    ArrivalsOnYellow = Convert.ToInt32(signalPhase.TotalArrivalOnYellow),
                    Volume = Convert.ToInt32(signalPhase.TotalArrivalOnGreen+ signalPhase.TotalArrivalOnRed+ signalPhase.TotalArrivalOnYellow),
                    BinStartTime = startTime,
                    IsProtectedPhase = approach.IsProtectedPhaseOverlap
                });
        }

        private void SetApproachCycleData(SignalPhase signalPhase, DateTime startTime, Approach approach,
            List<Controller_Event_Log> records, bool isPermissivePhase)
        {
            //Console.Write("\n-Aggregate Cycle data ");
            //DateTime dt = DateTime.Now;
            var pedActuations = 0;
            var totalCycles = 0;

            if (isPermissivePhase)
            {
                totalCycles = records.Count(r => r.EventCode == 1 && r.EventParam == approach.PermissivePhaseNumber);
            }
            else
            {
                totalCycles = records.Count(r => r.EventCode == 1 && r.EventParam == approach.ProtectedPhaseNumber);
            }
            var approachAggregation = new ApproachCycleAggregation
            {
                BinStartTime = startTime,
                ApproachId = approach.ApproachID,
                GreenTime = signalPhase.TotalGreenTime,
                RedTime = signalPhase.TotalRedTime,
                YellowTime = signalPhase.TotalYellowTime,
                PedActuations = pedActuations,
                TotalCycles = totalCycles,
                IsProtectedPhase = approach.IsProtectedPhaseOverlap
            };
            _approachCycleAggregationConcurrentQueue.Enqueue(approachAggregation);

            //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
        }

        private void SetSplitFailData(DateTime startTime, DateTime endTime, Approach approach, bool getPermissivePhase)
        {
            if (!approach.GetDetectorsForMetricType(12).Any()) return;
            //Console.Write("\n-Aggregate Split Fail data ");
            //DateTime dt = DateTime.Now;
            var splitFailOptions = new SplitFailOptions
            {
                FirstSecondsOfRed = 5,
                StartDate = startTime,
                EndDate = endTime,
                MetricTypeID = 12
            };
            var splitFailPhase = new SplitFailPhase(approach, splitFailOptions, getPermissivePhase);
            _approachSplitFailAggregationConcurrentQueue.Enqueue(new ApproachSplitFailAggregation
            {
                ApproachId = approach.ApproachID,
                BinStartTime = startTime,
                SplitFailures = splitFailPhase.TotalFails,
                IsProtectedPhase = approach.IsProtectedPhaseOverlap
            });

            //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
        }

        private void SetApproachSpeedAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach)
        {
            var speedDetectors = signalApproach.GetDetectorsForMetricType(10);
            if (speedDetectors.Count > 0)
                foreach (var detector in speedDetectors)
                {
                    var detectorSpeed = new DetectorSpeed(detector, startTime, endTime, 15, false);
                    if (detectorSpeed.AvgSpeedBucketCollection.AvgSpeedBuckets.Any())
                    {
                        var speedBucket = detectorSpeed.AvgSpeedBucketCollection.AvgSpeedBuckets.FirstOrDefault();
                        var approachSpeedAggregation =
                            new ApproachSpeedAggregation
                            {
                                ApproachId = signalApproach.ApproachID,
                                BinStartTime = startTime,
                                Speed85Th = speedBucket.EightyFifth,
                                Speed15Th = speedBucket.FifteenthPercentile,
                                SpeedVolume = speedBucket.SpeedVolume,
                                SummedSpeed = speedBucket.SummedSpeed,
                                IsProtectedPhase = signalApproach.IsProtectedPhaseOverlap
                            };
                        _approachSpeedAggregationConcurrentQueue.Enqueue(approachSpeedAggregation);
                    }
                }
        }

        private void AggregatePriorityCodes(DateTime startTime, List<Controller_Event_Log> records,
            Models.Signal signal, List<int> eventCodes)
        {
            for (var i = 0; i <= 10; i++)
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    var priorityAggregation = new PriorityAggregation
                    {
                        SignalID = signal.SignalID,
                        VersionId = signal.VersionID,
                        BinStartTime = startTime,
                        PriorityNumber = i,
                        PriorityRequests = records.Count(r => r.EventCode == 112),
                        PriorityServiceEarlyGreen = records.Count(r => r.EventCode == 113),
                        PriorityServiceExtendedGreen = records.Count(r => r.EventCode == 114)
                    };
                    _priorityAggregationConcurrentQueue.Enqueue(priorityAggregation);
                }
        }

        private void AggregatePreemptCodes(DateTime startTime, List<Controller_Event_Log> records, Models.Signal signal,
            List<int> eventCodes)
        {
            for (var i = 0; i <= 10; i++)
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    var preemptionAggregationData = new PreemptionAggregation
                    {
                        SignalId = signal.SignalID,
                        VersionId = signal.VersionID,
                        BinStartTime = startTime,
                        PreemptNumber = i,
                        PreemptRequests = records.Count(r => r.EventCode == 102),
                        PreemptServices = records.Count(r => r.EventCode == 105)
                    };
                    _preemptAggregationConcurrentQueue.Enqueue(preemptionAggregationData);
                }
        }
    }
}
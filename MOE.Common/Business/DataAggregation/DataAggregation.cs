using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Speed;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System.Data.Entity;
using System.Data.Entity.Validation;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.DataAggregation
{
    public class DataAggregation
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private ConcurrentQueue<ApproachSpeedAggregation> _approachSpeedAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSpeedAggregation>();
        private ConcurrentQueue<ApproachCycleAggregation> _approachCycleAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachCycleAggregation>();
        private ConcurrentQueue<ApproachPcdAggregation> _approachPcdAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachPcdAggregation>();
        private ConcurrentQueue<ApproachSplitFailAggregation> _approachSplitFailAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSplitFailAggregation>();
        private ConcurrentQueue<ApproachYellowRedActivationAggregation> _approachYellowRedActivationAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachYellowRedActivationAggregation>();
        private ConcurrentQueue<DetectorAggregation> _detectorAggregationConcurrentQueue =
            new ConcurrentQueue<DetectorAggregation>();


        public void StartAggregation(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            int binSize = Convert.ToInt32(appSettings["BinSize"]);
            SetStartEndDate(args);
            Console.WriteLine("Starting " + _startDate.ToShortDateString());
            SPM db = new SPM();
            db.Configuration.LazyLoadingEnabled = false;
            var signals = db.Signals
                .Where(signal => signal.Enabled == true && signal.SignalID == "7219")
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes.Select(dt => dt.MetricTypes))))
                .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .ToList();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            for (DateTime dt = _startDate; dt < _startDate.AddDays(1); dt = dt.AddMinutes(binSize))
            {
                Parallel.ForEach(signals, signal =>
                {
                    Console.WriteLine(signal.SignalID + " " + dt.ToString());
                    ProcessSignal(signal, dt, dt.AddMinutes(binSize));
                });
                if (_approachCycleAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach Cycle Data to Database...");
                    BulkSaveApproachCycleData();
                }
                if (_approachPcdAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach PCD Data to Database...");
                    BulkSaveApproachPcdData();
                }
                if (_approachSplitFailAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach Split Fail Data to Database...");
                    BulkSaveApproachSplitFailData();
                }
                if (_approachYellowRedActivationAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach Yellow Red Activations Data to Database...");
                    BulkSaveApproachYellowRedActivationsData();
                }
                if (_approachSpeedAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach Speed Data to Database...");
                    BulkSaveApproachSpeedData();
                }
                if (_detectorAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Detector Data to Database...");
                    BulkSaveDetectorData();
                }
            }
            _startDate = _startDate.AddDays(1);
        }

        private void BulkSaveDetectorData()
        {
            DataTable detectorAggregationTable = new DataTable();
            detectorAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            detectorAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            detectorAggregationTable.Columns.Add(new DataColumn("DetectorID", typeof(string)));
            detectorAggregationTable.Columns.Add(new DataColumn("Volume", typeof(double)));
            while (_detectorAggregationConcurrentQueue.TryDequeue(out var detectorAggregationData))
            {
                DataRow dataRow = detectorAggregationTable.NewRow();
                dataRow["BinStartTime"] = detectorAggregationData.BinStartTime;
                dataRow["DetectorID"] = detectorAggregationData.DetectorId;
                dataRow["Volume"] = detectorAggregationData.Volume;
                detectorAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }

        private void BulkSaveApproachSpeedData()
        {
            DataTable approachSpeedAggregationTable = new DataTable();
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
                DataRow dataRow = approachSpeedAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SummedSpeed"] = approachAggregationData.SummedSpeed;
                dataRow["SpeedVolume"] = approachAggregationData.SpeedVolume;
                dataRow["Speed85th"] = approachAggregationData.Speed85Th;
                dataRow["Speed15th"] = approachAggregationData.Speed15Th;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachSpeedAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }

        private void BulkSaveApproachCycleData()
        {
            DataTable approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("RedTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("YellowTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("GreenTime", typeof(double)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalCycles", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("PedActuations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachCycleAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                DataRow dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["RedTime"] = approachAggregationData.RedTime;
                dataRow["YellowTime"] = approachAggregationData.YellowTime;
                dataRow["GreenTime"] = approachAggregationData.GreenTime;
                dataRow["TotalCycles"] = approachAggregationData.TotalCycles;
                dataRow["PedActuations"] = approachAggregationData.PedActuations;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }

        private void BulkSaveApproachPcdData()
        {
            DataTable approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnGreen", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnRed", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnYellow", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachPcdAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                DataRow dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["ArrivalsOnGreen"] = approachAggregationData.ArrivalsOnGreen;
                dataRow["ArrivalsOnRed"] = approachAggregationData.ArrivalsOnRed;
                dataRow["ArrivalsOnYellow"] = approachAggregationData.ArrivalsOnYellow;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }

        private void BulkSaveApproachSplitFailData()
        {
            DataTable approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("SplitFailures", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachSplitFailAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                DataRow dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SplitFailures"] = approachAggregationData.SplitFailures;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }

        private void BulkSaveApproachYellowRedActivationsData()
        {
            DataTable approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("SevereRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            while (_approachYellowRedActivationAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                DataRow dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SevereRedLightViolations"] = approachAggregationData.SevereRedLightViolations;
                dataRow["TotalRedLightViolations"] = approachAggregationData.TotalRedLightViolations;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                approachAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
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
                    IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                    applicationEventRepository.QuickAdd("AggregateAtspmData", "AggregateAtspmData", "BulkSave", ApplicationEvent.SeverityLevels.High, e.Message);
                }
            }
        }
        

        private void SetStartEndDate(string[] args)
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
                _endDate = Convert.ToDateTime(args[1]).AddDays(1);
            }
            else
            {
                try
                {
                    SPM db = new SPM();
                    _startDate = db.ApproachPcdAggregations.Select(s => s.BinStartTime).Max().AddMinutes(15);
                }
                catch (Exception)
                {
                    _startDate = DateTime.Today.AddDays(-1); 
                    _endDate = DateTime.Today;
                }
            }
        }

        private void ProcessSignal(MOE.Common.Models.Signal signal, DateTime startTime, DateTime endTime)
        {
            IControllerEventLogRepository controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);            
            List<int> preemptCodes = new List<int> { 102, 105 };
            List<int> priorityCodes = new List<int> { 112, 113, 114 };
            if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            {
                AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            }
            if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            {
                AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            }
            if (signal.Approaches != null)
            {
                ProcessApproach(signal, startTime, endTime, records);
            }
        }

        private void ProcessApproach(MOE.Common.Models.Signal signal, DateTime startTime, DateTime endTime, List<Controller_Event_Log> records)
        {
            if (signal.Approaches != null)
            {
                foreach (var signalApproach in signal.Approaches)
                {
                    if (signalApproach.Detectors != null && signalApproach.Detectors.Count > 0)
                    {
                        SetApproachSpeedAggregationData(startTime, endTime, signalApproach);
                        SetApproachAggregationData(startTime, endTime, records, signalApproach);
                        SetDetectorAggregationData(startTime, endTime, records, signalApproach);
                    }
                }
            }
        }

        private void SetDetectorAggregationData(DateTime startTime, DateTime endTime, List<Controller_Event_Log> records, Approach signalApproach)
        {
            foreach (var detector in signalApproach.Detectors)
            {
                DetectorAggregation detectorAggregation = new DetectorAggregation
                {
                    DetectorId = detector.DetectorID,
                    BinStartTime = startTime,
                    Volume = records.Count(r => r.EventCode == 82 && r.EventParam == detector.DetChannel)
                };
                _detectorAggregationConcurrentQueue.Enqueue(detectorAggregation);
            }
        }
        

        private void SetApproachAggregationData(DateTime startTime, DateTime endTime, List<Controller_Event_Log> records, Approach approach)
        {
            SignalPhase signalPhase = new SignalPhase(startTime, endTime, approach, false, 15, 6, false);
            SetApproachCycleData(signalPhase, startTime, approach, records, false);
            SetApproachPcdData(signalPhase, startTime, approach);
            SetSplitFailData(startTime, endTime, approach, false);
            SetYellowRedActivationData(startTime, endTime, approach, false);
            if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
            {
                SignalPhase permissiveSignalPhase = new SignalPhase(startTime, endTime, approach, false, 15, 6, true);
                SetApproachCycleData(permissiveSignalPhase, startTime, approach, records, true);
                SetApproachPcdData(permissiveSignalPhase, startTime, approach);
                SetSplitFailData(startTime, endTime, approach, true);
                SetYellowRedActivationData(startTime, endTime, approach, true);
            }
        }

        private void SetYellowRedActivationData(DateTime startTime, DateTime endTime, Approach approach, bool isPermissivePhase)
        {
            if (approach.GetDetectorsForMetricType(11).Any())
            {
                YellowAndRedOptions options = new YellowAndRedOptions();
                options.SetDefaults();
                options.StartDate = startTime;
                options.EndDate = endTime;
                RLMSignalPhase yellowRedAcuationsPhase = null;
                if (isPermissivePhase)
                {
                    yellowRedAcuationsPhase = new RLMSignalPhase(startTime, endTime, 15, options.SevereLevelSeconds,
                        approach, true);
                }
                else
                {
                    yellowRedAcuationsPhase = new RLMSignalPhase(startTime, endTime, 15, options.SevereLevelSeconds,
                        approach, false);
                }
                _approachYellowRedActivationAggregationConcurrentQueue.Enqueue(new ApproachYellowRedActivationAggregation
                {
                    ApproachId = approach.ApproachID,
                    BinStartTime = startTime,
                    SevereRedLightViolations = Convert.ToInt32(yellowRedAcuationsPhase.SevereRedLightViolations),
                    TotalRedLightViolations = Convert.ToInt32(yellowRedAcuationsPhase.Violations),
                    IsProtectedPhase = approach.IsProtectedPhaseOverlap
                });
            }
        }

        private void SetApproachPcdData(SignalPhase signalPhase, DateTime startTime, Approach approach)
        {
            if (approach.GetDetectorsForMetricType(6).Any())
            {
                _approachPcdAggregationConcurrentQueue.Enqueue(new ApproachPcdAggregation
                {
                    ApproachId = approach.ApproachID,
                    ArrivalsOnGreen = Convert.ToInt32(signalPhase.TotalArrivalOnGreen),
                    ArrivalsOnRed = Convert.ToInt32(signalPhase.TotalArrivalOnRed),
                    ArrivalsOnYellow = Convert.ToInt32(signalPhase.TotalArrivalOnYellow),
                    BinStartTime = startTime,
                    IsProtectedPhase = approach.IsProtectedPhaseOverlap
                });
            }
        }

        private void SetApproachCycleData(SignalPhase signalPhase, DateTime startTime, Approach approach, List<Controller_Event_Log> records, bool isPermissivePhase)
        {
            int pedActuations = 0;
            int totalCycles = 0;

            if (isPermissivePhase)
            {
                pedActuations = records.Count(r => r.EventCode == 45 && r.EventParam == approach.PermissivePhaseNumber);
                totalCycles = records.Count(r => r.EventCode == 1 && r.EventParam == approach.PermissivePhaseNumber);
            }
            else
            {
                pedActuations = records.Count(r => r.EventCode == 45 && r.EventParam == approach.ProtectedPhaseNumber);
                totalCycles = records.Count(r => r.EventCode == 1 && r.EventParam == approach.ProtectedPhaseNumber);
            }
            ApproachCycleAggregation approachAggregation = new ApproachCycleAggregation {
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
        }
        
        private void SetSplitFailData(DateTime startTime, DateTime endTime, Approach signalApproach, bool isPermissive)
        {
            if (!signalApproach.GetDetectorsForMetricType(12).Any()) return;
            Phase phase;
            if (isPermissive)
            {
                phase = new Phase(signalApproach, startTime, endTime, new List<int> {1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64}, 1,true);
            }
            else
            {
                phase = new Phase(signalApproach, startTime, endTime, new List<int> {1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64}, 1, false);
            }
            SplitFailOptions splitFailOptions = new SplitFailOptions();
            splitFailOptions.SetDefaults();
            splitFailOptions.StartDate = startTime;
            splitFailOptions.EndDate = endTime;
            SplitFailPhase splitFailPhase = new SplitFailPhase(signalApproach.ProtectedPhaseNumber, signalApproach, splitFailOptions, phase);
            _approachSplitFailAggregationConcurrentQueue.Enqueue(new ApproachSplitFailAggregation
            {
                ApproachId = signalApproach.ApproachID,
                BinStartTime = startTime,
                SplitFailures = splitFailPhase.TotalFails,
                IsProtectedPhase = signalApproach.IsProtectedPhaseOverlap
            });
        }
        
        private void SetApproachSpeedAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach)
        {
            var speedDetectors = signalApproach.GetDetectorsForMetricType(10);
            if (speedDetectors.Count > 0)
            {
                foreach (var detector in speedDetectors)
                {
                    DetectorSpeed detectorSpeed = new DetectorSpeed(detector, startTime, endTime, 15);
                    var speedBucket = detectorSpeed.Plans.FirstOrDefault()
                        ?.AvgSpeedBucketCollection.Items
                        .FirstOrDefault();
                    if (speedBucket != null)
                    {
                        ApproachSpeedAggregation approachSpeedAggregation =
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
        }

        private void AggregatePriorityCodes(DateTime startTime, List<Controller_Event_Log> records, MOE.Common.Models.Signal signal, List<int> eventCodes)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    PriorityAggregation priorityAggregation = new PriorityAggregation
                    {
                        SignalID = signal.SignalID,
                        VersionId = signal.VersionID,
                        BinStartTime = startTime,
                        PriorityNumber = i,
                        PriorityRequests = records.Count(r => r.EventCode == 112),
                        PriorityServiceEarlyGreen = records.Count(r => r.EventCode == 113),
                        PriorityServiceExtendedGreen = records.Count(r => r.EventCode == 114)
                    };
                    var priorityAggregationDataRepository = PriorityAggregationDatasRepositoryFactory.Create();
                    priorityAggregationDataRepository.Save(priorityAggregation);
                }
            }
        }

        private void AggregatePreemptCodes(DateTime startTime, List<Controller_Event_Log> records, MOE.Common.Models.Signal signal, List<int> eventCodes)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    PreemptionAggregation preemptionAggregationData = new PreemptionAggregation
                    {
                        SignalId = signal.SignalID,
                        VersionId = signal.VersionID,
                        BinStartTime = startTime,
                        PreemptNumber = i,
                        PreemptRequests = records.Count(r => r.EventCode == 102),
                        PreemptServices = records.Count(r => r.EventCode == 105)
                    };
                    var priorityAggregationDataRepository = PreemptAggregationDatasRepositoryFactory.Create();
                    priorityAggregationDataRepository.Save(preemptionAggregationData);
                }
            }
        }
        
    }
}

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

namespace MOE.Common.Business.DataAggregation
{
    public class DataAggregation
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private ConcurrentQueue<SignalAggregationData> _signalAggregationConcurrentQueue =
            new ConcurrentQueue<SignalAggregationData>();
        private ConcurrentQueue<ApproachSpeedAggregationData> _approachSpeedAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSpeedAggregationData>();
        private ConcurrentQueue<ApproachAggregationData> _approachAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachAggregationData>();


        public void StartAggregation(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            int binSize = Convert.ToInt32(appSettings["BinSize"]);
            SetStartEndDate(args);
            Console.WriteLine("Starting " + _startDate.ToShortDateString());
            SPM db = new SPM();
            db.Configuration.LazyLoadingEnabled = false;
            var signals = db.Signals
                .Where(signal => signal.Enabled == true)
                .Include(signal => signal.Approaches.Select(a => a.Detectors))
                .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                .ToList();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            for (DateTime dt = _startDate; dt < _startDate.AddDays(1); dt = dt.AddMinutes(binSize))
            {
                Parallel.ForEach(signals, options, signal =>
                {
                    Console.WriteLine(signal.SignalID + " " + dt.ToString());
                    ProcessSignal(signal, dt, dt.AddMinutes(binSize));
                });
                if (_signalAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Signal Data to Database...");
                    BulkSaveSignalData();
                }
                if (_approachAggregationConcurrentQueue.Count > 0)
                {
                    Console.WriteLine("Saving Approach Data to Database...");
                    BulkSaveApproachData();
                }
            }
            _startDate = _startDate.AddDays(1);
        }

        private void BulkSaveApproachData()
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
            approachAggregationTable.Columns.Add(new DataColumn("SplitFailures", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnGreen", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnRed", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnYellow", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("SevereRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalRedLightViolations", typeof(int)));
            while (_approachAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                DataRow dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachID;
                dataRow["RedTime"] = approachAggregationData.RedTime;
                dataRow["YellowTime"] = approachAggregationData.YellowTime;
                dataRow["GreenTime"] = approachAggregationData.GreenTime;
                dataRow["TotalCycles"] = approachAggregationData.TotalCycles;
                dataRow["PedActuations"] = approachAggregationData.PedActuations;
                dataRow["SplitFailures"] = approachAggregationData.SplitFailures;
                dataRow["ArrivalsOnGreen"] = approachAggregationData.ArrivalsOnGreen;
                dataRow["ArrivalsOnRed"] = approachAggregationData.ArrivalsOnRed;
                dataRow["ArrivalsOnYellow"] = approachAggregationData.ArrivalsOnYellow;
                dataRow["SevereRedLightViolations"] = approachAggregationData.SevereRedLightViolations;
                dataRow["TotalRedLightViolations"] = approachAggregationData.TotalRedLightViolations;
                approachAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "ApproachAggregationDatas";
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

        private void BulkSaveSignalData()
        {
            DataTable signalAggregationTable = new DataTable();
            signalAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            signalAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            signalAggregationTable.Columns.Add(new DataColumn("SignalID", typeof(string)));
            signalAggregationTable.Columns.Add(new DataColumn("TotalCycles", typeof(int)));
            signalAggregationTable.Columns.Add(new DataColumn("AddCyclesInTransition", typeof(int)));
            signalAggregationTable.Columns.Add(new DataColumn("SubtractCyclesInTransition", typeof(int)));
            signalAggregationTable.Columns.Add(new DataColumn("DwellCyclesInTransition", typeof(int)));
            while (_signalAggregationConcurrentQueue.TryDequeue(out var signalAggregationData))
            {
                DataRow dataRow = signalAggregationTable.NewRow();
                dataRow["BinStartTime"] = signalAggregationData.BinStartTime;
                dataRow["SignalID"] = signalAggregationData.SignalID;
                dataRow["TotalCycles"] = signalAggregationData.TotalCycles;
                dataRow["AddCyclesInTransition"] = signalAggregationData.AddCyclesInTransition;
                dataRow["SubtractCyclesInTransition"] = signalAggregationData.SubtractCyclesInTransition;
                dataRow["DwellCyclesInTransition"] = signalAggregationData.DwellCyclesInTransition;
                signalAggregationTable.Rows.Add(dataRow);
            }
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["SPM"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "SignalAggregationDatas";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(signalAggregationTable);
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
                _startDate = DateTime.Today.AddDays(-1);//archivedMetricsRepository.GetLastArchiveRunDate()).AddMinutes(15);
                _endDate = DateTime.Today;
            }
        }

        private void ProcessSignal(MOE.Common.Models.Signal signal, DateTime startTime, DateTime endTime)
        {
            IControllerEventLogRepository controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            AggregateSignalTransitions(startTime, records, signal.SignalID);
            List<int> preemptCodes = new List<int> { 102, 105 };
            List<int> priorityCodes = new List<int> { 112, 113, 114 };
            if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            {
                AggregatePreemptCodes(startTime, records, signal.SignalID, preemptCodes);
            }
            if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            {
                AggregatePriorityCodes(startTime, records, signal.SignalID, priorityCodes);
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
                    }
                }
            }
        }

        private void SetApproachAggregationData(DateTime startTime, DateTime endTime, List<Controller_Event_Log> records, Approach signalApproach)
        {
            MOE.Common.Business.SignalPhase signalPhase =
                new SignalPhase(startTime, endTime, signalApproach, false, 15, 6);
            ApproachAggregationData approachAggregationData = new ApproachAggregationData
            {
                ApproachID = signalApproach.ApproachID,
                PedActuations = records.Count(r =>
                    r.EventCode == 45 && r.EventParam == signalApproach.ProtectedPhaseNumber),
                TotalCycles = records.Count(r =>
                    r.EventCode == 61 && r.EventParam == signalApproach.ProtectedPhaseNumber),
                GreenTime = signalPhase.TotalGreenTime,
                YellowTime = signalPhase.TotalYellowTime,
                RedTime = signalPhase.TotalRedTime,
                ArrivalsOnGreen = Convert.ToInt32(signalPhase.TotalArrivalOnGreen),
                ArrivalsOnRed = Convert.ToInt32(signalPhase.TotalArrivalOnRed),
                ArrivalsOnYellow = 0,
                BinStartTime = startTime,
                SplitFailures = 0,
                SevereRedLightViolations = 0,
                TotalRedLightViolations = 0
            };
            _approachAggregationConcurrentQueue.Enqueue(approachAggregationData);
        }

        private void SetApproachSpeedAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach)
        {
            var speedDetectors = signalApproach.GetDetectorsForMetricType(10);
            if (speedDetectors.Count > 0)
            {
                foreach (var detector in speedDetectors)
                {
                    DetectorSpeed detectorSpeed = new DetectorSpeed(detector, startTime, endTime, 15);
                    var speedBucket = detectorSpeed.Plans.PlanList.FirstOrDefault()
                        ?.AvgSpeedBucketCollection.Items
                        .FirstOrDefault();
                    if (speedBucket != null)
                    {
                        ApproachSpeedAggregationData approachSpeedAggregationData =
                            new ApproachSpeedAggregationData
                            {
                                ApproachID = signalApproach.ApproachID,
                                BinStartTime = startTime,
                                Speed85th = speedBucket.EightyFifth,
                                Speed15th = speedBucket.FifteenthPercentile,
                                SpeedVolume = speedBucket.SpeedVolume,
                                SummedSpeed = speedBucket.SummedSpeed
                            };
                        _approachSpeedAggregationConcurrentQueue.Enqueue(approachSpeedAggregationData);
                    }
                }
            }
        }

        private void AggregatePriorityCodes(DateTime startTime, List<Controller_Event_Log> records, string signalID, List<int> eventCodes)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    PriorityAggregationData priorityAggregationData = new PriorityAggregationData
                    {
                        SignalID = signalID,
                        BinStartTime = startTime,
                        PriorityNumber = i,
                        PriorityRequests = records.Count(r => r.EventCode == 112),
                        PriorityServiceEarlyGreen = records.Count(r => r.EventCode == 113),
                        PriorityServiceExtendedGreen = records.Count(r => r.EventCode == 114)
                    };
                    var priorityAggregationDataRepository = PriorityAggregationDatasRepositoryFactory.Create();
                    priorityAggregationDataRepository.Save(priorityAggregationData);
                }
            }
        }

        private void AggregatePreemptCodes(DateTime startTime, List<Controller_Event_Log> records, string signalID, List<int> eventCodes)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (records.Count(r => r.EventParam == i && eventCodes.Contains(r.EventCode)) > 0)
                {
                    PreemptionAggregationData preemptionAggregationData = new PreemptionAggregationData
                    {
                        SignalID = signalID,
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

        private void AggregateSignalTransitions(DateTime startTime, List<Controller_Event_Log> records, string signalId)
        {
            SignalAggregationData signalAggregationData = new SignalAggregationData
            {
                BinStartTime = startTime,
                TotalCycles = records.Count(r => r.EventCode == 150 && r.EventParam == 1),
                AddCyclesInTransition = records.Count(r => r.EventCode == 150 && r.EventParam == 2),
                SubtractCyclesInTransition = records.Count(r => r.EventCode == 150 && r.EventParam == 3),
                DwellCyclesInTransition = records.Count(r => r.EventCode == 150 && r.EventParam == 4),
                SignalID = signalId
            };
            _signalAggregationConcurrentQueue.Enqueue(signalAggregationData);
        }
    }
}

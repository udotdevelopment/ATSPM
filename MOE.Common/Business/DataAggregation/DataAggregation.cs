using MOE.Common.Business.PEDDelay;
using MOE.Common.Business.Speed;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DateTime = System.DateTime;

namespace MOE.Common.Business.DataAggregation
{
    public class DataAggregation
    {
        private ConcurrentQueue<PhaseCycleAggregation> _approachCycleAggregationConcurrentQueue =
            new ConcurrentQueue<PhaseCycleAggregation>();

        private ConcurrentQueue<ApproachPcdAggregation> _approachPcdAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachPcdAggregation>();

        private ConcurrentQueue<ApproachSpeedAggregation> _approachSpeedAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSpeedAggregation>();

        private ConcurrentQueue<ApproachSplitFailAggregation> _approachSplitFailAggregationConcurrentQueue =
            new ConcurrentQueue<ApproachSplitFailAggregation>();

        private ConcurrentQueue<ApproachYellowRedActivationAggregation>
            _approachYellowRedActivationAggregationConcurrentQueue =
                new ConcurrentQueue<ApproachYellowRedActivationAggregation>();

        private ConcurrentQueue<DetectorEventCountAggregation> _detectorAggregationConcurrentQueue =
            new ConcurrentQueue<DetectorEventCountAggregation>();

        private ConcurrentQueue<PreemptionAggregation> _preemptAggregationConcurrentQueue =
            new ConcurrentQueue<PreemptionAggregation>();

        private ConcurrentQueue<PriorityAggregation> _priorityAggregationConcurrentQueue =
            new ConcurrentQueue<PriorityAggregation>();

        private ConcurrentQueue<PhaseTerminationAggregation> _phaseTerminationAggregationQueue =
            new ConcurrentQueue<PhaseTerminationAggregation>();

        private ConcurrentQueue<PhasePedAggregation> _phasePedAggregations =
            new ConcurrentQueue<PhasePedAggregation>();

        private DateTime _endDate;
        private DateTime _startDate;
        //private DateTime _endBackwardTime;
        //private int _processDration;

        // private bool _commandLineArgs;

        //private bool _moreProcessing;
        private int _numberOfRows;
        private DateTime _newTime;

        private ConcurrentQueue<SignalEventCountAggregation> _signalEventAggregationConcurrentQueue =
            new ConcurrentQueue<SignalEventCountAggregation>();

        private ConcurrentQueue<SignalPlanAggregation> _signalPlanAggregationConcurrentQueue =
            new ConcurrentQueue<SignalPlanAggregation>();

        private ConcurrentQueue<PhaseSplitMonitorAggregation> _phaseSplitMonitorAggregationConcurrentQueue =
            new ConcurrentQueue<PhaseSplitMonitorAggregation>();

        private ConcurrentQueue<PhaseLeftTurnGapAggregation> _phaseLeftTurnGapAggregationAggregationConcurrentQueue =
            new ConcurrentQueue<PhaseLeftTurnGapAggregation>();
        //private bool _allStop;
        private long _maxMemoryLimit;
        private DateTime _testDate;
        public int _binSize;
        private string[] _restrictSignals;

        public void StartAggregationSignalPlan(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalOnlyVersionByDate(_startDate);//.Where(s=> s.SignalID == "1001").ToList();
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        //foreach (var signal in signals)
                        {
                            ProcessSignalPlanData(signal, startDateTime, startDateTime.AddMinutes(_binSize));
                        });
                        signals = new List<Signal>();
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                    catch (Exception e)
                    {
                        //ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalOnlyVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveSignalPlanData();

            }
        }

        private void ProcessSignalPlanData(Signal signal, DateTime startDateTime, DateTime endDateTime)
        {
            Console.Write(signal.SignalID + "    \r");
            var db = new SPM();
            List<Plan> plans = PlanFactory.GetBasicPlans(startDateTime, endDateTime, signal.SignalID, db);
            foreach (var plan in plans)
            {
                _signalPlanAggregationConcurrentQueue.Enqueue(new SignalPlanAggregation
                {
                    SignalId = signal.SignalID,
                    Start = plan.StartTime,
                    End = plan.EndTime,
                    PlanNumber = plan.PlanNumber
                });
            }
        }

        public void StartAggregationSignalEventData(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalOnlyVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        //foreach (var signal in signals)
                        {
                            ProcessSignalEventData(signal, startDateTime, startDateTime.AddMinutes(_binSize), options);
                        });
                        signals = new List<Signal>();
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                    catch (Exception e)
                    {
                        //ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalOnlyVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveAllAggregateDataInParallel();

            }
        }

        public void StartAggregationSignalPhaseTermination(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalOnlyVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {

                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            ProcessSignalPhaseTerminationData(signal, startDateTime, startDateTime.AddMinutes(_binSize), options);
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalOnlyVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSavePhaseTerminationData();

            }
        }

        public void StartAggregationSignalPedDelay(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            int _timeBuffer = Convert.ToInt32(appSettings["TimeBuffer"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalApproachVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            ProcessSignalPedDelayData(signal, startDateTime, startDateTime.AddMinutes(_binSize), _timeBuffer, options);
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalApproachVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSavePedData();

            }
        }

        public void StartAggregationSignalPreemptPriority(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalOnlyVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            ProcessSignalPreemptPriorityData(signal, startDateTime, startDateTime.AddMinutes(_binSize), options);
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalOnlyVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSavePreemptData();
                BulkSavePriorityData();

            }
        }

        public void StartAggregationApproachSpeed(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime.AddMinutes(_binSize) < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            ProcessApproachSpeedData(signal, startDateTime, startDateTime.AddMinutes(_binSize), options);
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveApproachSpeedData();

            }
        }


        public void StartAggregationLeftTurnAnalysis(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            Console.Write(signal.SignalID + "    \r");
                            ProcessLeftTurnGapAnalysis(signal, startDateTime, startDateTime.AddMinutes(_binSize), options);
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSavePhaseLeftTurnGapData();

            }
        }

        private void ProcessLeftTurnGapAnalysis(Signal signal, DateTime startDateTime, DateTime endDateTime, ParallelOptions parallelOptions)
        {
            int EVENT_GREEN = 1;
            int EVENT_RED = 10;
            int EVENT_DET = 81;
            var eventLogs = new ControllerEventLogs(signal.SignalID, startDateTime.AddSeconds(-900), endDateTime.AddSeconds(900),
                new List<int> { EVENT_DET, EVENT_GREEN, EVENT_RED });

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            double gap1Min = Convert.ToDouble(appSettings["Gap1Min"]);
            double gap1Max = Convert.ToDouble(appSettings["Gap1Max"]);
            double gap2Min = Convert.ToDouble(appSettings["Gap2Min"]);
            double gap2Max = Convert.ToDouble(appSettings["Gap2Max"]);
            double gap3Min = Convert.ToDouble(appSettings["Gap3Min"]);
            double gap3Max = Convert.ToDouble(appSettings["Gap3Max"]);
            double gap4Min = Convert.ToDouble(appSettings["Gap4Min"]);
            double? gap4Max = Convert.ToDouble(appSettings["Gap4Max"]);
            double? gap5Min = Convert.ToDouble(appSettings["Gap5Min"]);
            double? gap5Max = Convert.ToDouble(appSettings["Gap5Max"]);
            double? gap6Min = Convert.ToDouble(appSettings["Gap6Min"]);
            double? gap6Max = Convert.ToDouble(appSettings["Gap6Max"]);
            double? gap7Min = Convert.ToDouble(appSettings["Gap7Min"]);
            double? gap7Max = Convert.ToDouble(appSettings["Gap7Max"]);
            double? gap8Min = Convert.ToDouble(appSettings["Gap8Min"]);
            double? gap8Max = Convert.ToDouble(appSettings["Gap8Max"]);
            double? gap9Min = Convert.ToDouble(appSettings["Gap9Min"]);
            double? gap9Max = Convert.ToDouble(appSettings["Gap9Max"]);
            double? gap10Min = Convert.ToDouble(appSettings["Gap10Min"]);
            double? gap10Max = Convert.ToDouble(appSettings["Gap10Max"]);
            double? gap11Min = Convert.ToDouble(appSettings["Gap11Min"]);
            double? gap11Max = Convert.ToDouble(appSettings["Gap11Max"]);
            double? sumGapDuration1 = Convert.ToDouble(appSettings["SumGapDuration1"]);
            double? sumGapDuration2 = Convert.ToDouble(appSettings["SumGapDuration2"]);
            double? sumGapDuration3 = Convert.ToDouble(appSettings["SumGapDuration3"]);


            LeftTurnGapAnalysisOptions options = new LeftTurnGapAnalysisOptions(signal.SignalID, startDateTime, endDateTime, gap1Min, gap1Max, gap2Min, gap2Max, gap3Min, gap3Max,
                gap4Min, gap4Max, gap5Min, gap5Max, gap6Min, gap6Max, gap7Min, gap7Max, gap8Min, gap8Max, gap9Min, gap9Max, gap10Min, gap10Max, gap11Min, gap11Max, sumGapDuration1,
                sumGapDuration2, sumGapDuration3, 7.4);

            //Get phase + check for opposing phase before creating chart
            Parallel.Invoke(() =>
            {
                var ebPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 6);
                if (ebPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 2))
                {
                    var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(ebPhase, eventLogs, options);
                    SetLeftTurnGapData(leftTurnGapData, startDateTime);
                }
            },
                () =>
                {
                    var nbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 8);
                    if (nbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 4))
                    {
                        var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(nbPhase, eventLogs, options);
                        SetLeftTurnGapData(leftTurnGapData, startDateTime);
                    }
                },
                () =>
                {
                    var wbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 2);
                    if (wbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 6))
                    {
                        var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(wbPhase, eventLogs, options);
                        SetLeftTurnGapData(leftTurnGapData, startDateTime);
                    }
                },
                () =>
                {
                    var sbPhase = signal.Approaches.FirstOrDefault(x => x.ProtectedPhaseNumber == 4);
                    if (sbPhase != null && signal.Approaches.Any(x => x.ProtectedPhaseNumber == 8))
                    {
                        var leftTurnGapData = new LeftTurnGapAnalysis.LeftTurnGapAnalysis(sbPhase, eventLogs, options);
                        SetLeftTurnGapData(leftTurnGapData, startDateTime);
                    }
                });







        }

        private void SetLeftTurnGapData(LeftTurnGapAnalysis.LeftTurnGapAnalysis leftTurnGapData, DateTime binStartTime)
        {
            var leftTurnGapDataAggregation = new PhaseLeftTurnGapAggregation
            {
                BinStartTime = binStartTime,
                SignalId = leftTurnGapData.LeftTurnGapAnalysisOptions.SignalID,
                PhaseNumber = leftTurnGapData.Approach.ProtectedPhaseNumber,
                ApproachId = leftTurnGapData.Approach.ApproachID,
                GapCount1 = leftTurnGapData.Gaps1.Sum(g => g.Value),
                GapCount2 = leftTurnGapData.Gaps2.Sum(g => g.Value),
                GapCount3 = leftTurnGapData.Gaps3.Sum(g => g.Value),
                GapCount4 = leftTurnGapData.Gaps4.Sum(g => g.Value),
                GapCount5 = leftTurnGapData.Gaps5.Sum(g => g.Value),
                GapCount6 = leftTurnGapData.Gaps6.Sum(g => g.Value),
                GapCount7 = leftTurnGapData.Gaps7.Sum(g => g.Value),
                GapCount8 = leftTurnGapData.Gaps8.Sum(g => g.Value),
                GapCount9 = leftTurnGapData.Gaps9.Sum(g => g.Value),
                GapCount10 = leftTurnGapData.Gaps10.Sum(g => g.Value),
                GapCount11 = leftTurnGapData.Gaps11.Sum(g => g.Value),
                SumGapDuration1 = leftTurnGapData.SumDuration1.Value,
                SumGapDuration2 = leftTurnGapData.SumDuration2.Value,
                SumGapDuration3 = leftTurnGapData.SumDuration3.Value,
                SumGreenTime = leftTurnGapData.SumGreenTime
            };
            _phaseLeftTurnGapAggregationAggregationConcurrentQueue.Enqueue(leftTurnGapDataAggregation);
        }



        public void StartAggregationApproachSignalPhase(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {
                            var phases = signal.Approaches.Select(a => a.ProtectedPhaseNumber).Distinct();
                            Console.Write(signal.SignalID + "    \r");
                            Parallel.ForEach(phases, options, phase =>
                            {
                                SetApproachSignalPhase(startDateTime, startDateTime.AddMinutes(_binSize), signal.Approaches.FirstOrDefault(a => a.ProtectedPhaseNumber == phase));
                            });
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveApproachPcdData();
            }
        }

        public void StartAggregationApproachCycle(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            var dbRepository = MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            var settings = dbRepository.GetGeneralSettings();
            if (settings != null)
                for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
                {
                    Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                        startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                    if (nextSignals.Any())
                    {
                        signals = nextSignals;
                        nextSignals = new List<Signal>();
                    }
                    Parallel.Invoke(
                    () =>
                    {
                        try
                        {
                            Parallel.ForEach(signals, options, signal =>
                            //foreach(var signal in signals)
                            {
                                var phases = signal.Approaches.Select(a => a.ProtectedPhaseNumber).Distinct();
                                Console.Write(signal.SignalID + "    \r");
                                Parallel.ForEach(phases, options, phase =>
                                //foreach (var approach in signal.Approaches)
                                {
                                    if (phase > 0)
                                    {
                                        SetApproachCycleData(startDateTime, startDateTime.AddMinutes(_binSize), signal.Approaches.FirstOrDefault(a => a.ProtectedPhaseNumber == phase));
                                    }
                                });
                            });
                            signals = new List<Signal>();
                        }
                        catch (Exception e)
                        {
                            ClearCollections(DateTime.Now);
                            Console.WriteLine("Inside ProcessSignal Catch: " +
                                              "was Processing Signals, an execption has occurred.");
                            Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                              e.Message);
                            throw e;
                        }
                    },
                    () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                    Console.WriteLine(
                        "At {0}, the data for {1}, is being written to the database.",
                        DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                    BulkSaveApproachCycleData();
                }
        }


        public void StartAggregationApproachTurningMovementCountsForDashboard(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddDays(1))//.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {

                            var phases = signal.Approaches.Select(a => a.ProtectedPhaseNumber).Distinct();
                            Console.Write(signal.SignalID + "    \r");
                            Parallel.ForEach(phases, options, phase =>
                            {
                                if (phase > 0)
                                {
                                    SetApproachSignalPhase(startDateTime, startDateTime.AddMinutes(_binSize),
                                        signal.Approaches.FirstOrDefault(a => a.ProtectedPhaseNumber == phase));
                                }
                            });
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveApproachPcdData();
            }
        }

        public void StartAggregationApproachSplitFail(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        //foreach(var signal in signals)
                        Parallel.ForEach(signals, options, signal =>
                        {
                            //var phases = signal.Approaches.Select(a => a.ProtectedPhaseNumber).Distinct();
                            //var approaches = signal.Approaches.GetApproachesForAggregation();
                            Console.Write(signal.SignalID + "    \r");
                            //foreach(var approach in approaches)
                            Parallel.ForEach(signal.Approaches, options, approach =>
                            {
                                SetApproachSplitFailData(startDateTime, startDateTime.AddMinutes(_binSize),
                                    approach);
                            });
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveApproachSplitFailData();
            }
        }

        public void StartAggregationSplitMonitor(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalOnlyVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        //foreach (var signal in signals)
                        {
                            Console.Write(signal.SignalID + "    \r");
                            var analysisPhases = new AnalysisPhaseCollection(signal.SignalID, startDateTime, startDateTime.AddMinutes(_binSize));
                            foreach (var plan in analysisPhases.Plans)
                            {
                                plan.SetProgrammedSplits(signal.SignalID);
                                plan.SetHighCycleCount(analysisPhases);
                            }
                            Parallel.ForEach(analysisPhases.Items, options, phase => { SetSplitMonitorData(analysisPhases.Plans, phase, startDateTime, startDateTime.AddMinutes(_binSize)); });
                        });
                        signals = new List<Signal>();
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                    catch (Exception e)
                    {
                        //ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalOnlyVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveSplitMonitorData();

            }
        }



        public void StartAggregationApproachYellowRedActivation(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {

                            var phases = signal.Approaches.Select(a => a.ProtectedPhaseNumber).Distinct();
                            Console.Write(signal.SignalID + "    \r");
                            Parallel.ForEach(phases, options, phase =>
                            {
                                if (phase > 0)
                                {
                                    SetApproachYellowRedActivation(startDateTime, startDateTime.AddMinutes(_binSize),
                                        signal.Approaches.FirstOrDefault(a => a.ProtectedPhaseNumber == phase));
                                }
                            });
                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveApproachYellowRedActivationsData();
            }
        }

        public void StartAggregationDetectorActivation(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _binSize = Convert.ToInt32(appSettings["BinSize"]);
            _maxMemoryLimit = Convert.ToInt64(appSettings["MaxMemoryLimit"]);
            SetStartEndDate(args);

            Console.WriteLine("Begining of Data Aggregation  " + _startDate.ToString("yyyy-MM-dd HH:mm"));
            ParallelOptions options =
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(appSettings["MaxThreads"]) };
            List<Signal> signals = GetSignalVersionByDate(_startDate);
            List<Signal> nextSignals = new List<Signal>();
            for (var startDateTime = _startDate; startDateTime < _endDate; startDateTime = startDateTime.AddMinutes(_binSize))
            {
                Console.WriteLine("Starting Aggregation:for {0} to {1} ",
                    startDateTime.ToString("yyyy-MM-dd HH:mm"), startDateTime.AddMinutes(_binSize).ToString("yyyy-MM-dd HH:mm"));

                if (nextSignals.Any())
                {
                    signals = nextSignals;
                    nextSignals = new List<Signal>();
                }
                Parallel.Invoke(
                () =>
                {
                    try
                    {
                        Parallel.ForEach(signals, options, signal =>
                        {


                            Console.Write(signal.SignalID + "    \r");
                            Parallel.ForEach(signal.Approaches, options,
                                approach =>
                                {
                                    SetDetectorAggregationData(startDateTime, startDateTime.AddMinutes(_binSize),
                                        approach, options);
                                });


                        });
                        signals = new List<Signal>();
                    }
                    catch (Exception e)
                    {
                        ClearCollections(DateTime.Now);
                        Console.WriteLine("Inside ProcessSignal Catch: " +
                                          "was Processing Signals, an execption has occurred.");
                        Console.WriteLine("e.TargetSite: " + e.TargetSite + " Message is: " +
                                          e.Message);
                        throw e;
                    }
                },
                () => { nextSignals = GetSignalVersionByDate(startDateTime.AddMinutes(_binSize)); });

                Console.WriteLine(
                    "At {0}, the data for {1}, is being written to the database.",
                    DateTime.Now.ToString("HH:mm"), startDateTime.ToString("MM-dd HH:mm"));
                BulkSaveDetectorData();
            }
        }


        private void SetApproachSplitFailData(DateTime startDateTime, DateTime endDateTime, Approach approach)
        {
            Parallel.Invoke(
                () =>
                {
                    if (approach.ProtectedPhaseNumber > 0)
                    {
                        SetSplitFailData(startDateTime, endDateTime, approach, false);
                    }
                },
                () =>
                {
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        SetSplitFailData(startDateTime, endDateTime, approach, true);
                    }
                });

        }

        private void SetApproachYellowRedActivation(DateTime startDateTime, DateTime endDateTime, Approach approach)
        {
            Parallel.Invoke(
                () =>
                {
                    if (approach.ProtectedPhaseNumber > 0)
                    {
                        SetYellowRedActivationData(startDateTime, endDateTime, approach, false);
                    }
                },
                () =>
                {
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        SetYellowRedActivationData(startDateTime, endDateTime, approach, true);
                    }
                });
        }

        private bool IsThereBinStartTimeInMoeStatus(DateTime testTime)
        {
            bool foundData;
            var aggConnectionString =
                ConfigurationManager.ConnectionStrings["AggStatus"].ConnectionString;
            var aggStatus = new SqlConnection(aggConnectionString);
            string seeIfTimePresent = testTime.ToString("G");
            aggStatus.Open();
            {
                string sql = "Select BinStartTime From TimesAggregated Where BinStartTime = '" + seeIfTimePresent + "'";
                var command = new SqlCommand(sql, aggStatus);
                SqlDataReader dataReader = command.ExecuteReader();
                foundData = dataReader.HasRows;
            }
            aggStatus.Close();
            return foundData;
        }

        private bool InsertBinStartTimeInMoeStatus(DateTime binStartTime)
        {
            bool rowPresent = false;
            var aggConnectionString =
                ConfigurationManager.ConnectionStrings["AggStatus"].ConnectionString;
            var aggStatus = new SqlConnection(aggConnectionString);
            aggStatus.Open();
            string seeIfTimePresent = binStartTime.ToString("G");
            string sql = "Select BinStartTime From TimesAggregated Where BinStartTime = '"
                         + seeIfTimePresent + "'";
            var command = new SqlCommand(sql, aggStatus);
            SqlDataReader dataReader = command.ExecuteReader();
            rowPresent = dataReader.HasRows;
            if (!rowPresent)
            {
                sql = "Insert into TimesAggregated(BinStartTime, StartTime) values('"
                      + binStartTime.ToString("G") + "','" + DateTime.Now.ToString("G") + "')";
                command = new SqlCommand(sql, aggStatus);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(sql, aggStatus);
                adapter.InsertCommand.ExecuteNonQuery();
            }

            command.Dispose();
            aggStatus.Close();
            return !rowPresent;
        }

        private void UpdateNumberRowsInMoeStatus(DateTime updateTime, int rowCount)
        {
            var aggConnectionString =
                ConfigurationManager.ConnectionStrings["AggStatus"].ConnectionString;
            var aggStatus = new SqlConnection(aggConnectionString);
            aggStatus.Open();
            string sql = "Update TimesAggregated set NumberRows = " + rowCount + " Where BinStartTime = '"
                         + updateTime.ToString("G") + "'";
            var command = new SqlCommand(sql, aggStatus);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = new SqlCommand(sql, aggStatus);
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            aggStatus.Close();
            return;
        }

        private List<Signal> GetSignalOnlyVersionByDate(DateTime dt)
        {
            var db = new SPM();
            db.Configuration.LazyLoadingEnabled = false;
            List<int> versionIds = new List<int>();
            if (_restrictSignals != null)
            {
                versionIds = db.Signals.Where(
                        r => r.VersionActionId != 3 && r.Start < dt && _restrictSignals.Contains(r.SignalID)
                    ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                    .Select(s => s.VersionID).ToList();
            }
            else
            {
                versionIds = db.Signals.Where(
                    r => r.VersionActionId != 3 && r.Start < dt //&& (r.SignalID == "4029")
                ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                .Select(s => s.VersionID).ToList();
            }
            var signals = db.Signals.Where(signal => versionIds.Contains(signal.VersionID))
                .OrderBy(signal => signal.SignalID).ToList();
            return signals;
        }

        private List<Signal> GetSignalApproachVersionByDate(DateTime dt)
        {
            var db = new SPM();
            db.Configuration.LazyLoadingEnabled = false;

            List<int> versionIds = new List<int>();
            if (_restrictSignals != null)
            {
                versionIds = db.Signals.Where(
                        r => r.VersionActionId != 3 && r.Start < dt && _restrictSignals.Contains(r.SignalID)
                    ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                    .Select(s => s.VersionID).ToList();
            }
            else
            {
                versionIds = db.Signals.Where(
                        r => r.VersionActionId != 3 && r.Start < dt //&& (r.SignalID == "7060")
                    ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                    .Select(s => s.VersionID).ToList();
            }

            var signals = db.Signals.Where(signal => versionIds.Contains(signal.VersionID))
                .Include(signal => signal.Approaches)
                .OrderBy(signal => signal.SignalID).ToList();
            return signals;
        }

        private List<Signal> GetSignalVersionByDate(DateTime dt)
        {
            using (var db = new SPM())
            {
                db.Configuration.LazyLoadingEnabled = false;

                List<int> versionIds = new List<int>();
                if (_restrictSignals != null)
                {
                    versionIds = db.Signals.Where(
                            r => r.VersionActionId != 3 && r.Start < dt && _restrictSignals.Contains(r.SignalID)
                        ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                        .Select(s => s.VersionID).ToList();
                }
                else
                {
                    versionIds = db.Signals.Where(
                            r => r.VersionActionId != 3 && r.Start < dt 
                        ).GroupBy(r => r.SignalID).Select(g => g.OrderByDescending(r => r.Start).FirstOrDefault())
                        .Select(s => s.VersionID).ToList();
                }

                var signals = db.Signals.Where(signal => versionIds.Contains(signal.VersionID))
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionTypes)))
                    .Include(signal => signal.Approaches.Select(a =>
                        a.Detectors.Select(d => d.DetectionTypes.Select(det => det.MetricTypes))))
                    .Include(signal => signal.Approaches.Select(a => a.Detectors.Select(d => d.DetectionHardware)))
                    .Include(signal => signal.Approaches.Select(a => a.DirectionType))
                    .OrderBy(signal => signal.SignalID).ToList();

                return signals;
            }
        }



        private void ClearCollections(DateTime dt)
        {
            var memoryStartClearCollections = GC.GetTotalMemory(false);

            _approachCycleAggregationConcurrentQueue = new ConcurrentQueue<PhaseCycleAggregation>();
            _approachPcdAggregationConcurrentQueue = new ConcurrentQueue<ApproachPcdAggregation>();
            _approachSplitFailAggregationConcurrentQueue = new ConcurrentQueue<ApproachSplitFailAggregation>();
            _approachYellowRedActivationAggregationConcurrentQueue =
                new ConcurrentQueue<ApproachYellowRedActivationAggregation>();
            _approachSpeedAggregationConcurrentQueue = new ConcurrentQueue<ApproachSpeedAggregation>();
            _detectorAggregationConcurrentQueue = new ConcurrentQueue<DetectorEventCountAggregation>();
            _priorityAggregationConcurrentQueue = new ConcurrentQueue<PriorityAggregation>();
            _preemptAggregationConcurrentQueue = new ConcurrentQueue<PreemptionAggregation>();
            _signalEventAggregationConcurrentQueue = new ConcurrentQueue<SignalEventCountAggregation>();
            _phaseTerminationAggregationQueue = new ConcurrentQueue<PhaseTerminationAggregation>();
            _phasePedAggregations = new ConcurrentQueue<PhasePedAggregation>();
            GC.Collect();
            var memoryEndClearCollections = GC.GetTotalMemory(false);
            var memoryInformation = dt + "Memory Limit is " + _maxMemoryLimit.ToString("0,0") +
                                    ". Memory at the start of clear collection " +
                                    memoryStartClearCollections.ToString("0,0") +
                                    ". Memory at the end of Clear collection " +
                                    memoryEndClearCollections.ToString("0,0");
            ////Console.WriteLine(memoryInformation);
            var errorLog = ApplicationEventRepositoryFactory.Create();
            errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                this.GetType().Name, "clearCollections", ApplicationEvent.SeverityLevels.Medium,
                memoryInformation);
        }

        private void BulkSaveAllAggregateDataInParallel()
        {
            BulkSaveSignalEventData();
            //Parallel.Invoke(
            //    () =>
            //    {
            //        if (_approachCycleAggregationConcurrentQueue.Count > 0)
            //{
            //    Console.WriteLine(" 1.  Saving Approach Cycle Data to Database...");
            //    BulkSaveApproachCycleData();
            //}

            //    },
            //    () =>
            //    {
            //        if (_approachPcdAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 2.  Saving Approach PCD Data to Database...");

            //            BulkSaveApproachPcdData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_approachSplitFailAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 3.  Saving Approach Split Fail Data to Database...");
            //            BulkSaveApproachSplitFailData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_approachYellowRedActivationAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 4.  Saving Approach Yellow Red Activations Data to Database...");
            //            BulkSaveApproachYellowRedActivationsData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_approachSpeedAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 5.  Saving Approach Speed Data to Database...");
            //            BulkSaveApproachSpeedData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_detectorAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 6.  Saving Detector Data to Database...");
            //            BulkSaveDetectorData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_priorityAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 7.  Saving Priority Data to Database...");
            //            BulkSavePriorityData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_preemptAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 8.  Saving Preempt Data to Database...");
            //            BulkSavePreemptData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_signalEventAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine(" 9.  Saving Signal Event Data to Database...");
            //            BulkSaveSignalEventData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_phaseTerminationAggregationQueue.Count > 0)
            //        {
            //            Console.WriteLine("10.  Saving Phase Termination Data to Database...");
            //            BulkSavePhaseTerminationData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_approachEventAggregationConcurrentQueue.Count > 0)
            //        {
            //            Console.WriteLine("11.  Saving Phase Event Data to Database...");
            //            BulkSavePhaseEventData();
            //        }

            //    },
            //    () =>
            //    {
            //        if (_phasePedAggregations.Count > -1)
            //        {
            //            Console.WriteLine("12.  Saving Ped Data to Database...");
            //            BulkSavePedData();
            //        }

            //    }
            //);
            ClearCollections(DateTime.Now);
        }

        private void BulkSaveSplitMonitorData()
        //  Name has bee changed!  sqlBulkCopy.DestinationTableName = "ApproachEventCountAggregations"
        {
            var splitMonitorAggregationTable = new DataTable();
            splitMonitorAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            splitMonitorAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            splitMonitorAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            splitMonitorAggregationTable.Columns.Add(new DataColumn("EightyFifthPercentileSplit", typeof(int)));
            splitMonitorAggregationTable.Columns.Add(new DataColumn("SkippedCount", typeof(int)));
            //eventAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));

            while (_phaseSplitMonitorAggregationConcurrentQueue.TryDequeue(out var phaseSplitMonitorAggregation))
            {
                var dataRow = splitMonitorAggregationTable.NewRow();
                dataRow["BinStartTime"] = phaseSplitMonitorAggregation.BinStartTime;
                dataRow["SignalId"] = phaseSplitMonitorAggregation.SignalId;
                dataRow["PhaseNumber"] = phaseSplitMonitorAggregation.PhaseNumber;
                dataRow["EightyFifthPercentileSplit"] = phaseSplitMonitorAggregation.EightyFifthPercentileSplit;
                dataRow["SkippedCount"] = phaseSplitMonitorAggregation.SkippedCount;
                splitMonitorAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PhaseSplitMonitorAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(splitMonitorAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Approach (Phase) Event Count Data");
                }
            }

            splitMonitorAggregationTable.Dispose();
        }

        private void BulkSaveSignalPlanData()
        {
            var planAggregationTable = new DataTable();
            planAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            planAggregationTable.Columns.Add(new DataColumn("Start", typeof(DateTime)));
            planAggregationTable.Columns.Add(new DataColumn("End", typeof(DateTime)));
            planAggregationTable.Columns.Add(new DataColumn("PlanNumber", typeof(int)));

            while (_signalPlanAggregationConcurrentQueue.TryDequeue(out var plan))
            {
                var dataRow = planAggregationTable.NewRow();
                dataRow["SignalId"] = plan.SignalId;
                dataRow["Start"] = plan.Start;
                dataRow["End"] = plan.End;
                dataRow["PlanNumber"] = plan.PlanNumber;
                planAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "SignalPlanAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(planAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Signal Event Count Data");
                }
            }

            planAggregationTable.Dispose();
        }

        private void BulkSaveSignalEventData()
        {
            var eventAggregationTable = new DataTable();
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
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Signal Event Count Data");
                }
            }

            eventAggregationTable.Dispose();
        }

        private void BulkSavePreemptData()
        {
            var preemptAggregationTable = new DataTable();
            preemptAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            preemptAggregationTable.Columns.Add(new DataColumn("SignalID", typeof(string)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptNumber", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptRequests", typeof(int)));
            preemptAggregationTable.Columns.Add(new DataColumn("PreemptServices", typeof(int)));

            while (_preemptAggregationConcurrentQueue.TryDequeue(out var preemptionAggregation))
            {
                var dataRow = preemptAggregationTable.NewRow();
                dataRow["BinStartTime"] = preemptionAggregation.BinStartTime;
                dataRow["SignalID"] = preemptionAggregation.SignalId;
                dataRow["PreemptNumber"] = preemptionAggregation.PreemptNumber;
                dataRow["PreemptRequests"] = preemptionAggregation.PreemptRequests;
                dataRow["PreemptServices"] = preemptionAggregation.PreemptServices;
                preemptAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Preempt data");
                }
            }

            preemptAggregationTable.Dispose();
        }

        private void BulkSavePriorityData()
        {
            var priorityAggregationTable = new DataTable();
            priorityAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            priorityAggregationTable.Columns.Add(new DataColumn("SignalID", typeof(string)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityNumber", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityRequests", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityServiceEarlyGreen", typeof(int)));
            priorityAggregationTable.Columns.Add(new DataColumn("PriorityServiceExtendedGreen", typeof(int)));

            while (_priorityAggregationConcurrentQueue.TryDequeue(out var priorityAggregationData))
            {
                var dataRow = priorityAggregationTable.NewRow();
                dataRow["BinStartTime"] = priorityAggregationData.BinStartTime;
                dataRow["SignalID"] = priorityAggregationData.SignalId;
                dataRow["PriorityNumber"] = priorityAggregationData.PriorityNumber;
                dataRow["PriorityRequests"] = priorityAggregationData.PriorityRequests;
                dataRow["PriorityServiceEarlyGreen"] = priorityAggregationData.PriorityServiceEarlyGreen;
                dataRow["PriorityServiceExtendedGreen"] = priorityAggregationData.PriorityServiceExtendedGreen;
                priorityAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Priority Data");
                }
            }

            priorityAggregationTable.Dispose();
        }

        private void BulkSaveDetectorData()
        {
            var detectorAggregationTable = new DataTable();

            detectorAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            detectorAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            detectorAggregationTable.Columns.Add(new DataColumn("ApproachId", typeof(int)));
            detectorAggregationTable.Columns.Add(new DataColumn("DetectorPrimaryId", typeof(int)));
            detectorAggregationTable.Columns.Add(new DataColumn("EventCount", typeof(int)));

            while (_detectorAggregationConcurrentQueue.TryDequeue(out var detectorAggregationData))
            {
                var dataRow = detectorAggregationTable.NewRow();
                dataRow["SignalId"] = detectorAggregationData.SignalId;
                dataRow["ApproachId"] = detectorAggregationData.ApproachId;
                dataRow["BinStartTime"] = detectorAggregationData.BinStartTime;
                dataRow["DetectorPrimaryId"] = detectorAggregationData.DetectorPrimaryId;
                dataRow["EventCount"] = detectorAggregationData.EventCount;
                detectorAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "DetectorEventCountAggregations";
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Detector Data");
                }
            }

            detectorAggregationTable.Dispose();
        }

        private void BulkSaveApproachSpeedData()
        {
            var approachSpeedAggregationTable = new DataTable();
            approachSpeedAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("SummedSpeed", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("SpeedVolume", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("Speed85th", typeof(int)));
            approachSpeedAggregationTable.Columns.Add(new DataColumn("Speed15th", typeof(int)));

            while (_approachSpeedAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachSpeedAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SummedSpeed"] = approachAggregationData.SummedSpeed;
                dataRow["SpeedVolume"] = approachAggregationData.SpeedVolume;
                dataRow["Speed85th"] = approachAggregationData.Speed85Th;
                dataRow["Speed15th"] = approachAggregationData.Speed15Th;
                dataRow["SignalId"] = approachAggregationData.SignalId;

                approachSpeedAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Approach Speed Data");
                }
            }

            approachSpeedAggregationTable.Dispose();
        }

        private void BulkSaveApproachCycleData()
        {
            var phaseCycleAggregationTable = new DataTable();
            phaseCycleAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("ApproachId", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("RedTime", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("YellowTime", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("GreenTime", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("TotalRedToRedCycles", typeof(int)));
            phaseCycleAggregationTable.Columns.Add(new DataColumn("TotalGreenToGreenCycles", typeof(int)));
            while (_approachCycleAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = phaseCycleAggregationTable.NewRow();
                dataRow["ApproachId"] = approachAggregationData.ApproachId;
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["RedTime"] = approachAggregationData.RedTime;
                dataRow["YellowTime"] = approachAggregationData.YellowTime;
                dataRow["GreenTime"] = approachAggregationData.GreenTime;
                dataRow["SignalId"] = approachAggregationData.SignalId;
                dataRow["PhaseNumber"] = approachAggregationData.PhaseNumber;
                dataRow["TotalRedToRedCycles"] = approachAggregationData.TotalRedToRedCycles;
                dataRow["TotalGreenToGreenCycles"] = approachAggregationData.TotalGreenToGreenCycles;
                phaseCycleAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PhaseCycleAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(phaseCycleAggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Phase Cycle Data");
                }

            }

            phaseCycleAggregationTable.Dispose();
        }

        private void BulkSaveApproachPcdData()
        {
            var approachAggregationTable = new DataTable();
            //approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnGreen", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnRed", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ArrivalsOnYellow", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("Volume", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalDelay", typeof(double)));
            while (_approachPcdAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["ArrivalsOnGreen"] = approachAggregationData.ArrivalsOnGreen;
                dataRow["ArrivalsOnRed"] = approachAggregationData.ArrivalsOnRed;
                dataRow["ArrivalsOnYellow"] = approachAggregationData.ArrivalsOnYellow;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                dataRow["Volume"] = approachAggregationData.Volume;
                dataRow["SignalId"] = approachAggregationData.SignalId;
                dataRow["PhaseNumber"] = approachAggregationData.PhaseNumber;
                dataRow["TotalDelay"] = approachAggregationData.TotalDelay;
                approachAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Approach PCD Data");
                }

            }

            approachAggregationTable.Dispose();
        }

        private void BulkSaveApproachSplitFailData()
        {
            var approachAggregationTable = new DataTable();
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            approachAggregationTable.Columns.Add(new DataColumn("SplitFailures", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("GreenOccupancySum", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("RedOccupancySum", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("GreenTimeSum", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("RedTimeSum", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("Cycles", typeof(int)));
            while (_approachSplitFailAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SplitFailures"] = approachAggregationData.SplitFailures;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                dataRow["SignalId"] = approachAggregationData.SignalId;
                dataRow["GreenOccupancySum"] = approachAggregationData.GreenOccupancySum;
                dataRow["RedOccupancySum"] = approachAggregationData.RedOccupancySum;
                dataRow["GreenTimeSum"] = approachAggregationData.GreenTimeSum;
                dataRow["RedTimeSum"] = approachAggregationData.RedTimeSum;
                dataRow["Cycles"] = approachAggregationData.Cycles;
                dataRow["PhaseNumber"] = approachAggregationData.PhaseNumber;
                approachAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Approach Split Fail Data");
                }
            }

            approachAggregationTable.Dispose();
        }

        private void BulkSavePhaseTerminationData()
        {
            var phaseTerminationAggregationTable = new DataTable();
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("GapOuts", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("ForceOffs", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("MaxOuts", typeof(int)));
            phaseTerminationAggregationTable.Columns.Add(new DataColumn("Unknown", typeof(int)));
            while (_phaseTerminationAggregationQueue.TryDequeue(out var phaseTerminationAggregation))
            {
                var dataRow = phaseTerminationAggregationTable.NewRow();
                dataRow["BinStartTime"] = phaseTerminationAggregation.BinStartTime;
                dataRow["SignalId"] = phaseTerminationAggregation.SignalId;
                dataRow["PhaseNumber"] = phaseTerminationAggregation.PhaseNumber;
                dataRow["GapOuts"] = phaseTerminationAggregation.GapOuts;
                dataRow["ForceOffs"] = phaseTerminationAggregation.ForceOffs;
                dataRow["MaxOuts"] = phaseTerminationAggregation.MaxOuts;
                dataRow["Unknown"] = phaseTerminationAggregation.UnknownTerminationTypes;
                phaseTerminationAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Phase Termination Data");
                }
            }

            phaseTerminationAggregationTable.Dispose();
        }

        private void BulkSavePhaseLeftTurnGapData()
        {
            var aggregationTable = new DataTable();
            aggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            aggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            aggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("ApproachId", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount1", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount2", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount3", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount4", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount5", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount6", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount7", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount8", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount9", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount10", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("GapCount11", typeof(int)));
            aggregationTable.Columns.Add(new DataColumn("SumGapDuration1", typeof(double)));
            aggregationTable.Columns.Add(new DataColumn("SumGapDuration2", typeof(double)));
            aggregationTable.Columns.Add(new DataColumn("SumGapDuration3", typeof(double)));
            aggregationTable.Columns.Add(new DataColumn("SumGreenTime", typeof(double)));
            while (_phaseLeftTurnGapAggregationAggregationConcurrentQueue.TryDequeue(out var phaseLeftTurnAggregation))
            {
                var dataRow = aggregationTable.NewRow();
                dataRow["BinStartTime"] = phaseLeftTurnAggregation.BinStartTime;
                dataRow["SignalId"] = phaseLeftTurnAggregation.SignalId;
                dataRow["PhaseNumber"] = phaseLeftTurnAggregation.PhaseNumber;
                dataRow["ApproachId"] = phaseLeftTurnAggregation.ApproachId;
                dataRow["GapCount1"] = phaseLeftTurnAggregation.GapCount1;
                dataRow["GapCount2"] = phaseLeftTurnAggregation.GapCount2;
                dataRow["GapCount3"] = phaseLeftTurnAggregation.GapCount3;
                dataRow["GapCount4"] = phaseLeftTurnAggregation.GapCount4;
                dataRow["GapCount5"] = phaseLeftTurnAggregation.GapCount5;
                dataRow["GapCount6"] = phaseLeftTurnAggregation.GapCount6;
                dataRow["GapCount7"] = phaseLeftTurnAggregation.GapCount7;
                dataRow["GapCount8"] = phaseLeftTurnAggregation.GapCount8;
                dataRow["GapCount9"] = phaseLeftTurnAggregation.GapCount9;
                dataRow["GapCount10"] = phaseLeftTurnAggregation.GapCount10;
                dataRow["GapCount11"] = phaseLeftTurnAggregation.GapCount11;
                dataRow["SumGapDuration1"] = phaseLeftTurnAggregation.SumGapDuration1;
                dataRow["SumGapDuration2"] = phaseLeftTurnAggregation.SumGapDuration2;
                dataRow["SumGapDuration3"] = phaseLeftTurnAggregation.SumGapDuration3;
                dataRow["SumGreenTime"] = phaseLeftTurnAggregation.SumGreenTime;
                aggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
                sqlBulkCopy.DestinationTableName = "PhaseLeftTurnGapAggregations";
                sqlBulkCopy.BulkCopyTimeout = 180;
                sqlBulkCopy.BatchSize = 50000;
                try
                {
                    connection.Open();
                    sqlBulkCopy.WriteToServer(aggregationTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Phase Termination Data");
                }
            }

            aggregationTable.Dispose();
        }

        private void BulkSavePedData()
        {
            var phasePedAggregationTable = new DataTable();
            phasePedAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            phasePedAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedCycles", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedDelaySum", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("MinPedDelay", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("MaxPedDelay", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("ImputedPedCallsRegistered", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("UniquePedDetections", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedBeginWalkCount", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedCallsRegisteredCount", typeof(int)));
            phasePedAggregationTable.Columns.Add(new DataColumn("PedRequests", typeof(int)));
            while (_phasePedAggregations.TryDequeue(out var phasePedAggregation))
            {
                var dataRow = phasePedAggregationTable.NewRow();
                dataRow["BinStartTime"] = phasePedAggregation.BinStartTime;
                dataRow["SignalId"] = phasePedAggregation.SignalId;
                dataRow["PhaseNumber"] = phasePedAggregation.PhaseNumber;
                dataRow["PedCycles"] = phasePedAggregation.PedCycles;
                dataRow["PedDelaySum"] = phasePedAggregation.PedDelaySum;
                dataRow["MinPedDelay"] = phasePedAggregation.MinPedDelay;
                dataRow["MaxPedDelay"] = phasePedAggregation.MaxPedDelay;
                dataRow["ImputedPedCallsRegistered"] = phasePedAggregation.ImputedPedCallsRegistered;
                dataRow["UniquePedDetections"] = phasePedAggregation.UniquePedDetections;
                dataRow["PedBeginWalkCount"] = phasePedAggregation.PedBeginWalkCount;
                dataRow["PedCallsRegisteredCount"] = phasePedAggregation.PedCallsRegisteredCount;
                dataRow["PedRequests"] = phasePedAggregation.PedRequests;
                phasePedAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Phase Ped Data");
                }
            }

            phasePedAggregationTable.Dispose();
        }

        private void BulkSaveApproachYellowRedActivationsData()
        {
            var approachAggregationTable = new DataTable();
            //approachAggregationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("BinStartTime", typeof(DateTime)));
            approachAggregationTable.Columns.Add(new DataColumn("SignalId", typeof(string)));
            approachAggregationTable.Columns.Add(new DataColumn("ApproachID", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("PhaseNumber", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("IsProtectedPhase", typeof(bool)));
            approachAggregationTable.Columns.Add(new DataColumn("SevereRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("TotalRedLightViolations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("YellowActivations", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("ViolationTime", typeof(int)));
            approachAggregationTable.Columns.Add(new DataColumn("Cycles", typeof(int)));
            while (_approachYellowRedActivationAggregationConcurrentQueue.TryDequeue(out var approachAggregationData))
            {
                var dataRow = approachAggregationTable.NewRow();
                dataRow["BinStartTime"] = approachAggregationData.BinStartTime;
                dataRow["ApproachID"] = approachAggregationData.ApproachId;
                dataRow["SevereRedLightViolations"] = approachAggregationData.SevereRedLightViolations;
                dataRow["TotalRedLightViolations"] = approachAggregationData.TotalRedLightViolations;
                dataRow["IsProtectedPhase"] = approachAggregationData.IsProtectedPhase;
                dataRow["YellowActivations"] = approachAggregationData.YellowActivations;
                dataRow["ViolationTime"] = approachAggregationData.ViolationTime;
                dataRow["Cycles"] = approachAggregationData.Cycles;
                dataRow["SignalId"] = approachAggregationData.SignalId;
                dataRow["PhaseNumber"] = approachAggregationData.PhaseNumber;
                approachAggregationTable.Rows.Add(dataRow);
            }

            var connectionString =
                ConfigurationManager.ConnectionStrings["SPMImport"].ConnectionString;
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
                        this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("Unable to import Approach Yellow and Red Acvtivation Data");
                }
            }

            approachAggregationTable.Dispose();
        }


        public void SetStartEndDate(string[] args)
        {
            _startDate = DateTime.Today;
            if (args.Length == 1)
            {
                _startDate = Convert.ToDateTime(args[0]);
                _endDate = _startDate.AddDays(1);
            }
            else if (args.Length == 2)
            {
                _startDate = Convert.ToDateTime(args[0]);
                _endDate = Convert.ToDateTime(args[1]);
            }
            else if (args.Length == 3)
            {
                _startDate = Convert.ToDateTime(args[0]);
                _endDate = Convert.ToDateTime(args[1]);
                _restrictSignals = fl_remove_Escape_Sequences(args[2].ToString()).Split(',');
            }
            else
            {
                _startDate = GetNextTime();
                _endDate = _startDate.AddDays(1);
            }
        }


        public static string fl_remove_Escape_Sequences(string sText, string sReplace = "")

{
sText = sText.Replace("\a", sReplace); // Warning

sText = sText.Replace("\b", sReplace); // BACKSPACE

sText = sText.Replace("\f", sReplace); // Form-feed

sText = sText.Replace("\n", sReplace); // Line reverse

sText = sText.Replace("\r", sReplace); // Carriage return

sText = sText.Replace("\t", sReplace); // Horizontal tab

sText = sText.Replace("\v", sReplace); // Vertical tab

sText = sText.Replace("\'", sReplace); // Single quote

sText = sText.Replace("\"", sReplace); // Double quote

sText = sText.Replace("\\", sReplace); // Backslash



    return sText;

}


    private DateTime GetNextTime()
        {
            var db = new SPM();
            if (db.SignalEventCountAggregations.Any())
            {
                return db.SignalEventCountAggregations.Max(s => s.BinStartTime).AddMinutes(_binSize);
            }
            else
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                return Convert.ToDateTime(appSettings["EndBackwardTIme"]);
            }
        }


        private void ProcessSignalEventData(Signal signal, DateTime startTime, DateTime endTime, ParallelOptions options)
        {
            Console.Write(signal.SignalID + "    \r");
            try
            {
                if (!string.IsNullOrEmpty(signal.SignalID) && signal.SignalID != "null")
                {
                    var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
                    int eventCount =
                        controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime,
                            endTime);
                    _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
                    {
                        BinStartTime = startTime,
                        EventCount = eventCount,
                        SignalId = signal.SignalID
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            //var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            //Console.Write(signal.SignalID + "    \r");
            //var preemptCodes = new List<int> { 102, 105 };
            //var priorityCodes = new List<int> { 112, 113, 114 };


            ////int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            ////_signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            ////{
            ////    BinStartTime = startTime,
            ////    EventCount = eventCount,
            ////    SignalId = signal.SignalID
            ////});
            ////AggregatePhaseTerminations(startTime, endTime, signal);
            ////AggregatePedDelay(startTime, endTime, signal);
            ////if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            ////if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            ////if (signal.Approaches != null)
            ////    ProcessApproach(signal, startTime, endTime, options);


            //Parallel.Invoke(
            //     () =>
            //     {
            //         int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            //         _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            //         {
            //             BinStartTime = startTime,
            //             EventCount = eventCount,
            //             SignalId = signal.SignalID
            //         });
            //     },
            //     () =>
            //     {
            //         AggregatePhaseTerminations(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         AggregatePedDelay(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            //             AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            //             AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            //     },
            //     () =>
            //     {
            //         if (signal.Approaches != null)
            //             ProcessApproach(signal, startTime, endTime, options);
            //     }
            // );
        }

        private void ProcessSignalPhaseTerminationData(Signal signal, DateTime startTime, DateTime endTime, ParallelOptions options)
        {
            Console.Write(signal.SignalID + "    \r");
            try
            {
                if (!string.IsNullOrEmpty(signal.SignalID) && signal.SignalID != "null")
                {
                    AggregatePhaseTerminations(startTime, endTime, signal);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            //var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            //Console.Write(signal.SignalID + "    \r");
            //var preemptCodes = new List<int> { 102, 105 };
            //var priorityCodes = new List<int> { 112, 113, 114 };


            ////int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            ////_signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            ////{
            ////    BinStartTime = startTime,
            ////    EventCount = eventCount,
            ////    SignalId = signal.SignalID
            ////});
            ////AggregatePhaseTerminations(startTime, endTime, signal);
            ////AggregatePedDelay(startTime, endTime, signal);
            ////if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            ////if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            ////if (signal.Approaches != null)
            ////    ProcessApproach(signal, startTime, endTime, options);


            //Parallel.Invoke(
            //     () =>
            //     {
            //         int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            //         _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            //         {
            //             BinStartTime = startTime,
            //             EventCount = eventCount,
            //             SignalId = signal.SignalID
            //         });
            //     },
            //     () =>
            //     {
            //         AggregatePhaseTerminations(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         AggregatePedDelay(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            //             AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            //             AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            //     },
            //     () =>
            //     {
            //         if (signal.Approaches != null)
            //             ProcessApproach(signal, startTime, endTime, options);
            //     }
            // );
        }

        private void ProcessSignalPedDelayData(Signal signal, DateTime startTime, DateTime endTime, int _timeBuffer, ParallelOptions options)
        {
            Console.Write(signal.SignalID + "    \r");
            try
            {
                if (!string.IsNullOrEmpty(signal.SignalID) && signal.SignalID != "null")
                {
                    AggregatePedDelay(startTime, endTime, signal, _timeBuffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            //var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            //Console.Write(signal.SignalID + "    \r");
            //var preemptCodes = new List<int> { 102, 105 };
            //var priorityCodes = new List<int> { 112, 113, 114 };


            ////int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            ////_signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            ////{
            ////    BinStartTime = startTime,
            ////    EventCount = eventCount,
            ////    SignalId = signal.SignalID
            ////});
            ////AggregatePhaseTerminations(startTime, endTime, signal);
            ////AggregatePedDelay(startTime, endTime, signal);
            ////if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            ////if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            ////if (signal.Approaches != null)
            ////    ProcessApproach(signal, startTime, endTime, options);


            //Parallel.Invoke(
            //     () =>
            //     {
            //         int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            //         _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            //         {
            //             BinStartTime = startTime,
            //             EventCount = eventCount,
            //             SignalId = signal.SignalID
            //         });
            //     },
            //     () =>
            //     {
            //         AggregatePhaseTerminations(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         AggregatePedDelay(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            //             AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            //             AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            //     },
            //     () =>
            //     {
            //         if (signal.Approaches != null)
            //             ProcessApproach(signal, startTime, endTime, options);
            //     }
            // );
        }

        private void ProcessSignalPreemptPriorityData(Signal signal, DateTime startTime, DateTime endTime, ParallelOptions options)
        {
            Console.Write(signal.SignalID + "    \r");
            try
            {
                if (!string.IsNullOrEmpty(signal.SignalID) && signal.SignalID != "null")
                {
                    //var engine = new PreemptCycleEngine();
                    //var cycles = engine.CreatePreemptCycle(dttb);



                    var testDate = Convert.ToDateTime("12/1/2020 7:00 AM");
                    var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
                    var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
                    var preemptCodes = new List<int> { 102, 105 };
                    var priorityCodes = new List<int> { 112, 113, 114 };
                    if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
                        AggregatePreemptCodes(startTime, records, signal, preemptCodes);
                    if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
                        AggregatePriorityCodes(startTime, records, signal, priorityCodes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();
            //var records = controllerEventLogRepository.GetAllAggregationCodes(signal.SignalID, startTime, endTime);
            //Console.Write(signal.SignalID + "    \r");
            //var preemptCodes = new List<int> { 102, 105 };
            //var priorityCodes = new List<int> { 112, 113, 114 };


            ////int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            ////_signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            ////{
            ////    BinStartTime = startTime,
            ////    EventCount = eventCount,
            ////    SignalId = signal.SignalID
            ////});
            ////AggregatePhaseTerminations(startTime, endTime, signal);
            ////AggregatePedDelay(startTime, endTime, signal);
            ////if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            ////if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            ////    AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            ////if (signal.Approaches != null)
            ////    ProcessApproach(signal, startTime, endTime, options);


            //Parallel.Invoke(
            //     () =>
            //     {
            //         int eventCount = controllerEventLogRepository.GetSignalEventsCountBetweenDates(signal.SignalID, startTime, endTime);
            //         _signalEventAggregationConcurrentQueue.Enqueue(new SignalEventCountAggregation
            //         {
            //             BinStartTime = startTime,
            //             EventCount = eventCount,
            //             SignalId = signal.SignalID
            //         });
            //     },
            //     () =>
            //     {
            //         AggregatePhaseTerminations(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         AggregatePedDelay(startTime, endTime, signal);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => preemptCodes.Contains(r.EventCode)) > 0)
            //             AggregatePreemptCodes(startTime, records, signal, preemptCodes);
            //     },
            //     () =>
            //     {
            //         if (records.Count(r => priorityCodes.Contains(r.EventCode)) > 0)
            //             AggregatePriorityCodes(startTime, records, signal, priorityCodes);
            //     },
            //     () =>
            //     {
            //         if (signal.Approaches != null)
            //             ProcessApproach(signal, startTime, endTime, options);
            //     }
            // );
        }

        private void ProcessApproachSpeedData(Signal signal, DateTime startTime, DateTime endTime, ParallelOptions options)
        {
            Console.Write(signal.SignalID + "    \r");
            try
            {
                Parallel.ForEach(signal.Approaches, options, signalApproach =>
                {
                    if (signalApproach.Detectors != null && signalApproach.Detectors.Count > 0)
                    {
                        SetApproachSpeedAggregationData(startTime, endTime, signalApproach, false);
                    }

                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }


        private void AggregatePedDelay(DateTime startTime, DateTime endTime, Models.Signal signal, int timeBuffer)
        {
            PedDelaySignal pedDelaySignal = new PedDelaySignal(signal, timeBuffer, startTime, endTime);
            foreach (var pedPhase in pedDelaySignal.PedPhases)
            {
                PhasePedAggregation pedAggregation = new PhasePedAggregation
                {
                    SignalId = signal.SignalID,
                    PhaseNumber = pedPhase.PhaseNumber,
                    BinStartTime = startTime,
                    PedCycles = pedPhase.Cycles.Count,
                    PedDelaySum = Convert.ToInt32(Math.Round(pedPhase.TotalDelay)),
                    MinPedDelay = Convert.ToInt32(Math.Round(pedPhase.MinDelay)),
                    MaxPedDelay = Convert.ToInt32(Math.Round(pedPhase.MaxDelay)),
                    PedRequests = Convert.ToInt32(pedPhase.PedRequests),
                    ImputedPedCallsRegistered = Convert.ToInt32(pedPhase.ImputedPedCallsRegistered),
                    UniquePedDetections = Convert.ToInt32(pedPhase.UniquePedDetections),
                    PedBeginWalkCount = Convert.ToInt32(pedPhase.PedBeginWalkCount),
                    PedCallsRegisteredCount = Convert.ToInt32(pedPhase.PedCallsRegisteredCount)
                };
                _phasePedAggregations.Enqueue(pedAggregation);
            }
        }



        private void AggregatePhaseTerminations(DateTime startTime, DateTime endTime, Models.Signal signal)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            var generateFakeData = Convert.ToBoolean(appSettings["GenerateFakeData"]);
            if (generateFakeData)
            {
                foreach (var phase in signal.GetPhasesForSignal())
                {
                    var random = new Random();
                    _phaseTerminationAggregationQueue.Enqueue(new PhaseTerminationAggregation { BinStartTime = startTime, ForceOffs = random.Next(0, 5), GapOuts = random.Next(0, 5), MaxOuts = random.Next(0, 5), SignalId = signal.SignalID, PhaseNumber = phase, UnknownTerminationTypes = random.Next(0, 5) });
                }
            }
            else
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
                        //PhaseSkipped = analysisPhase.
                    };
                    _phaseTerminationAggregationQueue.Enqueue(phaseTerminationAggregation);
                }
            }
        }



        private void SetDetectorAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach,
            ParallelOptions options)
        {
            //Console.Write("\n-Aggregate Detector data ");
            //DateTime dt = DateTime.Now;

            Parallel.ForEach(signalApproach.Detectors, options, detector =>
            //foreach (var detector in signalApproach.Detectors)
            {
                int count = 0;
                using (var db = new SPM())
                {
                    var controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create(db);
                    count = controllerEventLogRepository.GetDetectorActivationCount(signalApproach.SignalID,
                        startTime,
                        endTime, detector.DetChannel);
                }

                var detectorAggregation = new DetectorEventCountAggregation
                {
                    SignalId = signalApproach.SignalID,
                    ApproachId = signalApproach.ApproachID,
                    DetectorPrimaryId = detector.ID,
                    BinStartTime = startTime,
                    EventCount = count
                };
                _detectorAggregationConcurrentQueue.Enqueue(detectorAggregation);

            });



            //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
        }


        private void SetApproachSignalPhase(DateTime startTime, DateTime endTime, Approach approach)
        {

            Parallel.Invoke(
                () =>
                {
                    if (approach.ProtectedPhaseNumber > 0)
                    {
                        var signalPhase = new SignalPhase(startTime, endTime, approach, true, 15, 6, false);
                        SetApproachPcdData(signalPhase, startTime, approach, false);
                    }
                },
                () =>
                {
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        var permissiveSignalPhase = new SignalPhase(startTime, endTime, approach, false, 15, 6, true);
                        SetApproachPcdData(permissiveSignalPhase, startTime, approach, true);
                    }
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
                        IsProtectedPhase = !isPermissivePhase,
                        YellowActivations = Convert.ToInt32(yellowRedAcuationsPhase.YellowOccurrences),
                        ViolationTime = Convert.ToInt32(Math.Round(yellowRedAcuationsPhase.ViolationTime)),
                        Cycles = yellowRedAcuationsPhase.Plans.PlanList.Sum(p => p.CycleCount),
                        SignalId = approach.SignalID,
                        PhaseNumber = isPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber,
                    });
                //Console.Write((DateTime.Now - dt).Milliseconds.ToString());
            }
        }

        private void SetApproachPcdData(SignalPhase signalPhase, DateTime startTime, Approach approach, bool isPermissivePhase)
        {
            if (approach.GetDetectorsForMetricType(6).Any())
                _approachPcdAggregationConcurrentQueue.Enqueue(new ApproachPcdAggregation
                {
                    ApproachId = approach.ApproachID,
                    ArrivalsOnGreen = Convert.ToInt32(signalPhase.TotalArrivalOnGreen),
                    ArrivalsOnRed = Convert.ToInt32(signalPhase.TotalArrivalOnRed),
                    ArrivalsOnYellow = Convert.ToInt32(signalPhase.TotalArrivalOnYellow),
                    Volume = Convert.ToInt32(signalPhase.TotalVolume),
                    BinStartTime = startTime,
                    IsProtectedPhase = !isPermissivePhase,
                    SignalId = approach.SignalID,
                    PhaseNumber = isPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber,
                    TotalDelay = Convert.ToInt32(Math.Round(signalPhase.TotalDelay))

                });
        }

        private void SetApproachCycleData(DateTime startTime, DateTime endTime, Approach approach)
        {
            if (approach != null)
            {
                SPM db = new SPM();
                var cel = ControllerEventLogRepositoryFactory.Create(db);
                var cycleEventNumbers = approach.IsPermissivePhaseOverlap
                    ? new List<int> { 61, 63, 64 }
                    : new List<int> { 1, 8, 9 };
                var cycleEvents = cel.GetEventsByEventCodesParam(approach.SignalID, startTime, endTime.AddSeconds(900),
                    cycleEventNumbers,
                    approach.ProtectedPhaseNumber);
                List<RedToRedCycle> redCycles = new List<RedToRedCycle>();
                List<GreenToGreenCycle> greenCycles = new List<GreenToGreenCycle>();
                Parallel.Invoke(
                    () =>
                    {
                        redCycles = CycleFactory.GetRedToRedCycles(approach, startTime, endTime, false, cycleEvents);
                    },
                    () =>
                    {
                        greenCycles =
                            CycleFactory.GetGreenToGreenCycles(approach, startTime, endTime, false, cycleEvents);
                    });

                _approachCycleAggregationConcurrentQueue.Enqueue(new PhaseCycleAggregation
                {
                    BinStartTime = startTime,
                    SignalId = approach.SignalID,
                    ApproachId = approach.ApproachID,
                    PhaseNumber = approach.ProtectedPhaseNumber,
                    RedTime = Convert.ToInt32(Math.Round(redCycles.Sum(c => c.TotalRedTime))),
                    YellowTime = Convert.ToInt32(Math.Round(redCycles.Sum(c => c.TotalYellowTime))),
                    GreenTime = Convert.ToInt32(Math.Round(redCycles.Sum(c => c.TotalGreenTime))),
                    TotalRedToRedCycles = redCycles.Count,
                    TotalGreenToGreenCycles = greenCycles.Count
                });
            }
        }

        private void SetSplitFailData(DateTime startTime, DateTime endTime, Approach approach, bool getPermissivePhase)
        {
            if (!approach.GetDetectorsForMetricType(12).Any()) return;
            //Console.Write("\n-Aggregate Split Fail data ");
            var splitFailOptions = new SplitFailOptions
            {
                FirstSecondsOfRed = 5,
                StartDate = startTime,
                EndDate = endTime,
                MetricTypeID = 12,
                SignalID = approach.SignalID
            };
            var splitFailPhase = new SplitFailPhase(approach, splitFailOptions, getPermissivePhase);
            _approachSplitFailAggregationConcurrentQueue.Enqueue(new ApproachSplitFailAggregation
            {
                SignalId = approach.SignalID,
                ApproachId = approach.ApproachID,
                BinStartTime = startTime,
                SplitFailures = splitFailPhase.TotalFails,
                IsProtectedPhase = !getPermissivePhase,
                GreenOccupancySum = Convert.ToInt32(Math.Round(splitFailPhase.Cycles.Sum(c => c.GreenOccupancyTimeInMilliseconds / 1000))),
                RedOccupancySum = Convert.ToInt32(Math.Round(splitFailPhase.Cycles.Sum(c => c.RedOccupancyTimeInMilliseconds) / 1000)),
                GreenTimeSum = Convert.ToInt32(Math.Round(splitFailPhase.Cycles.Sum(c => c.TotalGreenTime))),
                RedTimeSum = Convert.ToInt32(Math.Round(splitFailPhase.Cycles.Sum(c => c.TotalRedTime))),
                Cycles = splitFailPhase.Cycles.Count,
                PhaseNumber = getPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber
            });
        }


        private void SetSplitMonitorData(List<PlanSplitMonitor> plans, AnalysisPhase phase, DateTime start, DateTime end)
        {
            int skippedCount = 0;
            foreach (var plan in plans)
            {
                var cycles = from cycle in phase.Cycles.Items
                             where cycle.StartTime >= plan.StartTime && cycle.EndTime < plan.EndTime
                             orderby cycle.Duration
                             select cycle;

                if (plan.CycleCount > 0)
                {
                    skippedCount += plan.CycleCount - cycles.Count();
                }
            }

            double percentileResult = 0;
            if (phase.Cycles.Items.Count() > 2)
            {
                var percentile = Convert.ToDouble(85) / 100;


                var percentilIndex = percentile * phase.Cycles.Items.Count();
                if (percentilIndex % 1 == 0)
                {
                    percentileResult = phase.Cycles.Items.ElementAt(Convert.ToInt16(percentilIndex) - 1).Duration
                        .TotalSeconds;
                }
                else
                {
                    var indexMod = percentilIndex % 1;
                    //subtracting .5 leaves just the integer after the convert.
                    //There was probably another way to do that, but this is easy.
                    int indexInt = Convert.ToInt16(percentilIndex - .5);

                    var step1 = phase.Cycles.Items.ElementAt(Convert.ToInt16(indexInt) - 1).Duration.TotalSeconds;
                    var step2 = phase.Cycles.Items.ElementAt(Convert.ToInt16(indexInt)).Duration.TotalSeconds;
                    var stepDiff = step2 - step1;
                    var step3 = stepDiff * indexMod;
                    percentileResult = step1 + step3;
                }
            }

            _phaseSplitMonitorAggregationConcurrentQueue.Enqueue(new PhaseSplitMonitorAggregation
            {
                SignalId = phase.SignalID,
                BinStartTime = start,
                PhaseNumber = phase.PhaseNumber,
                EightyFifthPercentileSplit = Convert.ToInt32(Math.Round(percentileResult)),
                SkippedCount = skippedCount
            });
        }

        private void SetApproachSpeedAggregationData(DateTime startTime, DateTime endTime, Approach signalApproach, bool getPermissivePhase)
        {
            var speedDetectors = signalApproach.GetDetectorsForMetricType(10);
            if (speedDetectors.Count > 0)
                foreach (var detector in speedDetectors)
                {
                    var detectorSpeed = new DetectorSpeed(detector, startTime, endTime, 15, getPermissivePhase);
                    if (detectorSpeed.AvgSpeedBucketCollection.AvgSpeedBuckets.Any())
                    {
                        var speedBucket = detectorSpeed.AvgSpeedBucketCollection.AvgSpeedBuckets.FirstOrDefault();
                        var approachSpeedAggregation =
                            new ApproachSpeedAggregation
                            {
                                ApproachId = signalApproach.ApproachID,
                                SignalId = signalApproach.SignalID,
                                BinStartTime = startTime,
                                Speed85Th = speedBucket.EightyFifth,
                                Speed15Th = speedBucket.FifteenthPercentile,
                                SpeedVolume = speedBucket.SpeedVolume,
                                SummedSpeed = speedBucket.SummedSpeed
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
                        SignalId = signal.SignalID,
                        BinStartTime = startTime,
                        PriorityNumber = i,
                        PriorityRequests = records.Count(r => r.EventCode == 112 && r.EventParam == i),
                        PriorityServiceEarlyGreen = records.Count(r => r.EventCode == 113 && r.EventParam == i),
                        PriorityServiceExtendedGreen = records.Count(r => r.EventCode == 114 && r.EventParam == i)
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
                        BinStartTime = startTime,
                        PreemptNumber = i,
                        PreemptRequests = records.Count(r => r.EventCode == 102 && r.EventParam == i),
                        PreemptServices = records.Count(r => r.EventCode == 105 && r.EventParam == i)
                    };
                    _preemptAggregationConcurrentQueue.Enqueue(preemptionAggregationData);
                }
        }



    }
}

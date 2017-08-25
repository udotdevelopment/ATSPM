using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class RLMDetectorCollection
    {
        private List<MOE.Common.Business.Detector> NBdetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBDetectors
        {
            get { return NBdetectors; }
        }

        private List<MOE.Common.Business.Detector> SBdetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBDetectors
        {
            get { return SBdetectors; }
        }

        private List<MOE.Common.Business.Detector> EBdetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBDetectors
        {
            get { return EBdetectors; }
        }

        private List<MOE.Common.Business.Detector> WBdetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBDetectors
        {
            get { return WBdetectors; }
        }

        private List<MOE.Common.Business.Detector> NBbikedetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBBikeDetectors
        {
            get { return NBbikedetectors; }
        }

        private List<MOE.Common.Business.Detector> SBbikedetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBBikeDetectors
        {
            get { return SBbikedetectors; }
        }

        private List<MOE.Common.Business.Detector> EBbikedetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBBikeDetectors
        {
            get { return EBbikedetectors; }
        }

        private List<MOE.Common.Business.Detector> WBbikedetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBBikeDetectors
        {
            get { return WBbikedetectors; }
        }

        private List<MOE.Common.Business.Detector> NBpeddetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBPedDetectors
        {
            get { return NBpeddetectors; }
        }

        private List<MOE.Common.Business.Detector> SBpeddetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBPedDetectors
        {
            get { return SBpeddetectors; }
        }

        private List<MOE.Common.Business.Detector> EBpeddetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBPedDetectors
        {
            get { return EBpeddetectors; }
        }

        private List<MOE.Common.Business.Detector> WBpeddetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBPedDetectors
        {
            get { return WBpeddetectors; }
        }

        private List<List<Detector>> pedDetectors = new List<List<Detector>>();
        public List<List<Detector>> PedDetectors
        {
            get { return PedDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBleftDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBLeftDetectors
        {
            get { return NBleftDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBleftDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBLeftDetectors
        {
            get { return SBleftDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBleftDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBLeftDetectors
        {
            get { return EBleftDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBleftDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBLeftDetectors
        {
            get { return WBleftDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBrightDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBRightDetectors
        {
            get { return NBrightDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBrightDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBRightDetectors
        {
            get { return SBrightDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBrightDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBRightDetectors
        {
            get { return EBrightDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBrightDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBRightDetectors
        {
            get { return WBrightDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBthruDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBThruDetectors
        {
            get { return NBthruDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBthruDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBThruDetectors
        {
            get { return SBthruDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBthruDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBThruDetectors
        {
            get { return EBthruDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBthruDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBThruDetectors
        {
            get { return WBthruDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBexitDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBExitDetectors
        {
            get { return NBexitDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBexitDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBExitDetectors
        {
            get { return SBexitDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBexitDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBExitDetectors
        {
            get { return EBexitDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBexitDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBExitDetectors
        {
            get { return WBexitDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBLTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBLTBikeDetectors
        {
            get { return NBLTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBRTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBRTBikeDetectors
        {
            get { return NBRTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> NBThrubikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NBThruBikeDetectors
        {
            get { return NBThrubikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBLTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBLTBikeDetectors
        {
            get { return SBLTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBRTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBRTBikeDetectors
        {
            get { return SBRTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> SBThrubikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> SBThruBikeDetectors
        {
            get { return SBThrubikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBLTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBLTBikeDetectors
        {
            get { return EBLTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBRTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBRTBikeDetectors
        {
            get { return EBRTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> EBThrubikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> EBThruBikeDetectors
        {
            get { return EBThrubikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBLTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBLTBikeDetectors
        {
            get { return WBLTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBRTbikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBRTBikeDetectors
        {
            get { return WBRTbikeDetectors; }
        }

        private List<MOE.Common.Business.Detector> WBThrubikeDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> WBThruBikeDetectors
        {
            get { return WBThrubikeDetectors; }
        }

        private string signalId;
        public string SignalId
        {
            get { return signalId; }
        }

        public List<MOE.Common.Business.Detector> DetectorsForRLM = new List<Detector>();

        /// <summary>
        /// Default constructor for the DetectorCollection used in the Turning Movement Counts charts
        /// </summary>
        /// <param name="signalID"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="binsize"></param>
        public  RLMDetectorCollection(DateTime startdate, DateTime enddate, int binsize, 
            MOE.Common.Models.Approach approach)
        {
            var dets = approach.GetDetectorsForMetricType(11);
            foreach (MOE.Common.Models.Detector detector in dets)
            {
                MOE.Common.Business.Detector Detector = new Detector(detector, startdate, enddate, binsize);
                DetectorsForRLM.Add(Detector);
            }
            //SortDetectors();
        }


        /// <summary>
        /// Alternate Constructor for PDC type data.
        /// </summary>
        /// <param name="signalid"></param>
        /// <param name="ApproachDirection"></param>
        public RLMDetectorCollection(string signalID, string ApproachDirection)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
            List<MOE.Common.Models.Detector> dets = signal.GetDetectorsForSignalThatSupportAMetricByApproachDirection(11, ApproachDirection);

            foreach (MOE.Common.Models.Detector row in dets)
            {
               // MOE.Common.Business.Detector Detector = new Detector(row.DetectorID.ToString(), signalID, row.Det_Channel, row.Lane.LaneType, ApproachDirection);
                MOE.Common.Business.Detector Detector = new Detector(row.DetectorID.ToString(), signalID, row.DetChannel, row.Approach);
                DetectorsForRLM.Add(Detector);
            }

        }

        public MOE.Common.Business.ControllerEventLogs CombineDetectorData(DateTime startDate, DateTime endDate, string signalId)
        {
            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            MOE.Common.Business.ControllerEventLogs detectortable = 
                new MOE.Common.Business.ControllerEventLogs(signalId, startDate, endDate);
            List<MOE.Common.Business.ControllerEventLogs> Tables = new List<MOE.Common.Business.ControllerEventLogs>();
            foreach (MOE.Common.Business.Detector Detector in DetectorsForRLM)
            {
                MOE.Common.Business.ControllerEventLogs TEMPdetectortable =
                    new MOE.Common.Business.ControllerEventLogs(signalId, startDate, endDate, Detector.Channel, 
                        new List<int>() { 82 });
                Tables.Add(TEMPdetectortable);
            }
            foreach (MOE.Common.Business.ControllerEventLogs Table in Tables)
            {
                detectortable.MergeEvents(Table);
            }
            return detectortable;
        }
        


    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MOE.Common.Business
{
    public class DetectorCollection


    {

        public List<MOE.Common.Business.Detector> NBDetectors { get; set; }
        public List<MOE.Common.Business.Detector> SBDetectors { get; set; }
        public List<MOE.Common.Business.Detector> EBDetectors { get; set; }
        public List<MOE.Common.Business.Detector> WBDetectors { get; set; }
        public List<MOE.Common.Business.Detector> NBBikeDetectors { get; set; }
        public List<MOE.Common.Business.Detector> SBBikeDetectors { get; set; }
        public List<MOE.Common.Business.Detector> EBBikeDetectors { get; set; }
        public List<MOE.Common.Business.Detector> WBBikeDetectors { get; set; }
        public List<MOE.Common.Business.Detector> NBPedDetectors { get; set; }
        public List<MOE.Common.Business.Detector> SBPedDetectors { get; set; }
        public List<MOE.Common.Business.Detector> EBPedDetectors { get; set; }
        public List<MOE.Common.Business.Detector> WBPedDetectors { get; set; }
        public List<MOE.Common.Business.Detector> PedDetectors { get; set; }

        public Models.DetectionHardware DetectionHardware { 
            
            get; set; }


        




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

        private string _signalId;
        public string SignalId
        {
            get { return _signalId; }
        }

        public List<MOE.Common.Business.Detector> Items = new List<Detector>();

        public List<MOE.Common.Models.Detector> ApproachCountDetectors = new List<Models.Detector>();




        /// <summary>
        /// Alternate Constructor for PCD type data.
        /// </summary>
        /// <param name="signalid"></param>
        /// <param name="approach"></param>
        public DetectorCollection(MOE.Common.Models.Approach approach)
        {
            _signalId = approach.SignalID;
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
           

            var PCDDetectors = approach.GetDetectorsForMetricType(6);

            foreach (MOE.Common.Models.Detector row in PCDDetectors)
            {
               
                MOE.Common.Business.Detector Detector = new Detector(row);
                Items.Add(Detector);
            }
        }

        public ControllerEventLogs CombineDetectorData(DateTime startDate, DateTime endDate, double offset, string signalId)
        {
            ControllerEventLogs detectortable = new ControllerEventLogs(signalId, startDate, endDate);
            List<ControllerEventLogs> Tables = new List<ControllerEventLogs>();
            foreach (MOE.Common.Business.Detector Detector in Items)
            {
                ControllerEventLogs TEMPdetectortable = new ControllerEventLogs(signalId, startDate, endDate, new List<int>() { 82 });
                
                Tables.Add(TEMPdetectortable);
            }

            foreach (ControllerEventLogs Table in Tables)
            {
                detectortable.MergeEvents(Table);
            }


            return detectortable;
        }

        public ControllerEventLogs CombineDetectorDataByApproachAndType(DateTime startDate, DateTime endDate,MOE.Common.Models.Approach approach, bool Has_PCD, bool Has_TMC)
        {

            MOE.Common.Models.Repositories.IDetectorRepository gr = MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();

            string signalId = approach.SignalID;
            

            if(Has_TMC)
            {
                ApproachCountDetectors.Clear();

               
            ApproachCountDetectors.AddRange(RemoveExitDetectors(approach.GetDetectorsForMetricType(5)));
           
            
            }

            if(Has_PCD)
            {
                ApproachCountDetectors.Clear();
                ApproachCountDetectors.AddRange(RemoveExitDetectors(approach.GetDetectorsForMetricType(6)));
            }



            List<ControllerEventLogs> eventsList = new List<ControllerEventLogs>();

            ControllerEventLogs MergedEvents = new ControllerEventLogs(signalId, startDate, endDate);

            foreach (Models.Detector detector in ApproachCountDetectors)
            {
                List<int> li = new List<int> { 82 };
                ControllerEventLogs cs = new ControllerEventLogs(signalId, startDate, endDate, detector.DetChannel, li);
                eventsList.Add(cs);
                
            }

            foreach (ControllerEventLogs Events in eventsList)
            {
                MergedEvents.MergeEvents(Events);
            }


            return MergedEvents;
        }

        private IEnumerable<Models.Detector> RemoveExitDetectors(List<Models.Detector> list)
        {
            List<Models.Detector> filteredList = (from r in list
                                                  where r.LaneTypeID != 4
                                                  select r).ToList();

            return filteredList;
        }
        

    }
}

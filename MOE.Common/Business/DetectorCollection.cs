using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class DetectorCollection


    {
        public List<Models.Detector> ApproachCountDetectors = new List<Models.Detector>();

        public List<Detector> Items = new List<Detector>();


        /// <summary>
        ///     Alternate Constructor for PCD type data.
        /// </summary>
        /// <param name="signalid"></param>
        /// <param name="approach"></param>
        public DetectorCollection(Approach approach)
        {
            SignalId = approach.SignalID;
            var repository =
                SignalsRepositoryFactory.Create();


            var PCDDetectors = approach.GetDetectorsForMetricType(6);

            foreach (var row in PCDDetectors)
            {
                var Detector = new Detector(row);
                Items.Add(Detector);
            }
        }

        public List<Detector> NBDetectors { get; set; }
        public List<Detector> SBDetectors { get; set; }
        public List<Detector> EBDetectors { get; set; }
        public List<Detector> WBDetectors { get; set; }
        public List<Detector> NBBikeDetectors { get; set; }
        public List<Detector> SBBikeDetectors { get; set; }
        public List<Detector> EBBikeDetectors { get; set; }
        public List<Detector> WBBikeDetectors { get; set; }
        public List<Detector> NBPedDetectors { get; set; }
        public List<Detector> SBPedDetectors { get; set; }
        public List<Detector> EBPedDetectors { get; set; }
        public List<Detector> WBPedDetectors { get; set; }
        public List<Detector> PedDetectors { get; set; }

        public DetectionHardware DetectionHardware { get; set; }

        public List<Detector> NBLeftDetectors { get; } = new List<Detector>();

        public List<Detector> SBLeftDetectors { get; } = new List<Detector>();

        public List<Detector> EBLeftDetectors { get; } = new List<Detector>();

        public List<Detector> WBLeftDetectors { get; } = new List<Detector>();

        public List<Detector> NBRightDetectors { get; } = new List<Detector>();

        public List<Detector> SBRightDetectors { get; } = new List<Detector>();

        public List<Detector> EBRightDetectors { get; } = new List<Detector>();

        public List<Detector> WBRightDetectors { get; } = new List<Detector>();

        public List<Detector> NBThruDetectors { get; } = new List<Detector>();

        public List<Detector> SBThruDetectors { get; } = new List<Detector>();

        public List<Detector> EBThruDetectors { get; } = new List<Detector>();

        public List<Detector> WBThruDetectors { get; } = new List<Detector>();

        public List<Detector> NBExitDetectors { get; } = new List<Detector>();

        public List<Detector> SBExitDetectors { get; } = new List<Detector>();

        public List<Detector> EBExitDetectors { get; } = new List<Detector>();

        public List<Detector> WBExitDetectors { get; } = new List<Detector>();

        public List<Detector> NBLTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> NBRTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> NBThruBikeDetectors { get; } = new List<Detector>();

        public List<Detector> SBLTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> SBRTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> SBThruBikeDetectors { get; } = new List<Detector>();

        public List<Detector> EBLTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> EBRTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> EBThruBikeDetectors { get; } = new List<Detector>();

        public List<Detector> WBLTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> WBRTBikeDetectors { get; } = new List<Detector>();

        public List<Detector> WBThruBikeDetectors { get; } = new List<Detector>();

        public string SignalId { get; }

        public ControllerEventLogs CombineDetectorData(DateTime startDate, DateTime endDate, double offset,
            string signalId)
        {
            var detectortable = new ControllerEventLogs(signalId, startDate, endDate);
            var Tables = new List<ControllerEventLogs>();
            foreach (var Detector in Items)
            {
                var TEMPdetectortable = new ControllerEventLogs(signalId, startDate, endDate, new List<int> {82});

                Tables.Add(TEMPdetectortable);
            }

            foreach (var Table in Tables)
                detectortable.MergeEvents(Table);


            return detectortable;
        }

        public ControllerEventLogs CombineDetectorDataByApproachAndType(DateTime startDate, DateTime endDate,
            Approach approach, bool Has_PCD, bool Has_TMC)
        {
            var gr = DetectorRepositoryFactory.Create();
            var signalId = approach.SignalID;
            if (Has_TMC)
            {
                ApproachCountDetectors.Clear();
                ApproachCountDetectors.AddRange(RemoveExitDetectors(approach.GetDetectorsForMetricType(5)));
            }
            if (Has_PCD)
            {
                ApproachCountDetectors.Clear();
                ApproachCountDetectors.AddRange(RemoveExitDetectors(approach.GetDetectorsForMetricType(6)));
            }
            var eventsList = new List<ControllerEventLogs>();
            var MergedEvents = new ControllerEventLogs(signalId, startDate, endDate);
            foreach (var detector in ApproachCountDetectors)
            {
                var li = new List<int> {82};
                var cs = new ControllerEventLogs(signalId, startDate, endDate, detector.DetChannel, li);
                eventsList.Add(cs);
            }

            foreach (var Events in eventsList)
                MergedEvents.MergeEvents(Events);


            return MergedEvents;
        }

        private IEnumerable<Models.Detector> RemoveExitDetectors(List<Models.Detector> list)
        {
            var filteredList = (from r in list
                where r.LaneTypeID != 4
                select r).ToList();

            return filteredList;
        }
    }
}
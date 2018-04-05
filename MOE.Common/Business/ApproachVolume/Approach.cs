using System;

namespace MOE.Common.Business.ApproachVolume
{
    public class Approach
    {
        public Approach(Models.Approach approach)
        {
            SignalID = approach.SignalID;
            Direction = approach.DirectionType.Description;
            ApproachModel = approach;
            Detectors = new DetectorCollection(approach);
            DetectorEvents = null;
        }

        public string SignalID { get; set; }

        public string Direction { get; set; }

        public DetectorCollection Detectors { get; }

        public VolumeCollection Volume { get; private set; }

        public ControllerEventLogs DetectorEvents { get; private set; }

        public Models.Approach ApproachModel { get; set; }

        public void SetDetectorEvents(Models.Approach approach, DateTime startdate, DateTime enddate, bool has_pcd,
            bool has_tmc)
        {
            DetectorEvents =
                Detectors.CombineDetectorDataByApproachAndType(startdate, enddate, approach, has_pcd, has_tmc);
        }

        public void SetVolume(DateTime startDate, DateTime endDate,
            int binSize)
        {
            if (DetectorEvents.Events != null)
                Volume = new VolumeCollection(startDate, endDate, DetectorEvents.Events, binSize);
        }
    }
}
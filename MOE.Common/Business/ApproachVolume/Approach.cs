using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ApproachVolume
{
    public class Approach
    {
        public string SignalID
        {
            get;
            set;
        }

        public string Direction
        {
            get;
            set;
        }

        private DetectorCollection detectors;
        public DetectorCollection Detectors
        {
            get { return detectors; }
        }

        

        private VolumeCollection volume;
        public VolumeCollection Volume
        {
            get { return volume; }
        }

        private ControllerEventLogs detectorevents;
        public ControllerEventLogs DetectorEvents
        {
            get { return detectorevents;  }
        }

        public MOE.Common.Models.Approach ApproachModel
        {
            get;
            set;
        }

        public Approach(MOE.Common.Models.Approach approach)
        {
            SignalID = approach.SignalID;
            Direction = approach.DirectionType.Description;
            ApproachModel = approach;
            detectors = new DetectorCollection(approach);

            detectorevents = null;

        }

        public void SetDetectorEvents(MOE.Common.Models.Approach approach, DateTime startdate, DateTime enddate, bool has_pcd, bool has_tmc)
        {
            detectorevents = detectors.CombineDetectorDataByApproachAndType(startdate, enddate, approach, has_pcd, has_tmc);


        }

        public void SetVolume(DateTime startDate, DateTime endDate,
            int binSize)
        {
           
            if (this.DetectorEvents.Events != null)
            {

                volume = new VolumeCollection(startDate, endDate, this.DetectorEvents.Events, binSize);
            }

        }

    }
}

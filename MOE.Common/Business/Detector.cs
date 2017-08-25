using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class Detector
    {
        public override string ToString()
        {
            return DetID + " " + Approach.DirectionType.Description;
        }

        private string detID;
        public string DetID
        {
            get { return detID; }
        }

        private int channel;
        public int Channel
        {
            get { return channel; }
        }

        public int ApproachID { get; set; }
        public MOE.Common.Models.Approach Approach { get; set; }
        

        private MOE.Common.Business.VolumeCollection volumes;
        public MOE.Common.Business.VolumeCollection Volumes
        {
            get { return volumes; }
        }

        private string signal;
        public string SignalID
        {
            get {return signal;}
    }

        private string phase;
        public string Phase
        {
            get { return phase; }
        }

        private int order;
        public int Order
        {
            get { return order; }
        }

        public MOE.Common.Models.LaneType LaneType { get; set; }

        public MOE.Common.Models.Detector DetectorModel { get; set; }

  /// <summary>
  /// Default constructor for the Detector class use in the Turning Movement Count Charts
  /// </summary>
  /// <param name="detid"></param>
  /// <param name="signalid"></param>
  /// <param name="channelid"></param>
  /// <param name="laneid"></param>
  /// <param name="approachdirection"></param>
  /// <param name="startDate"></param>
  /// <param name="endDate"></param>
  /// <param name="binsize"></param>
        public Detector(MOE.Common.Models.Detector detector, DateTime startDate, DateTime endDate, int binsize)
        {
            detID = detector.DetectorID;
            channel = detector.DetChannel;
            Approach = detector.Approach;
            signal = detector.Approach.SignalID;
            LaneType = detector.LaneType;
            MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Models.Controller_Event_Log> detectorEvents = new List<Models.Controller_Event_Log>();

                detectorEvents.AddRange(celRepository.GetEventsByEventCodesParam(detector.Approach.SignalID, startDate,
                    endDate, new List<int> { 82 }, channel));
        
            volumes = new VolumeCollection(startDate, endDate, detectorEvents, binsize);
        }

        /// <summary>
        /// alternate construtcor used for PCDs
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        public Detector(MOE.Common.Models.Detector detector)
        {
            detID = detector.DetectorID;
            channel = detector.DetChannel;
            Approach = detector.Approach;
            DetectorModel = detector;
        }

        /// <summary>
        /// Constructor Used For Data Aggregation
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        public Detector(string detid, string signalid, int channelid, MOE.Common.Models.Approach approach, string phasenumber)
        {
            detID = detid;
            channel = channelid;
            Approach = approach; 
            phase = phasenumber;
        }

        /// <summary>
        /// Contrutor Used for RouteManagement
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        /// <param name="phasenumber"></param>
        /// <param name="routeorder"></param>
        public Detector(string detid, string signalid, int channelid, MOE.Common.Models.Approach approach, string phasenumber, int routeorder)
        {
            detID = detid;
            channel = channelid;
            Approach = approach; 
            phase = phasenumber;
            order = routeorder;
        }

        public int OccupancyDuringMovementType(int EventCode, int StartPoint, int EndPoint)
        {
            int percentOccupancy = 0;



            return percentOccupancy;
        }

        public int TotalOccupancy()
        {
            int percentOccupancy = 0;
            return percentOccupancy;
        }

        
    }
}

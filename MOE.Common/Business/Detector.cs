using System;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class Detector
    {
        /// <summary>
        ///     Default constructor for the Detector class use in the Turning Movement Count Charts
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="binsize"></param>
        public Detector(Models.Detector detector, DateTime startDate, DateTime endDate, int binsize)
        {
            DetID = detector.DetectorID;
            Channel = detector.DetChannel;
            Approach = detector.Approach;
            SignalID = detector.Approach.SignalID;
            LaneType = detector.LaneType;
            var celRepository =
                ControllerEventLogRepositoryFactory.Create();
            var detectorEvents = new List<Controller_Event_Log>();

            detectorEvents.AddRange(celRepository.GetEventsByEventCodesParam(detector.Approach.SignalID, startDate,
                endDate, new List<int> {82}, Channel));

            Volumes = new VolumeCollection(startDate, endDate, detectorEvents, binsize);
        }

        /// <summary>
        ///     alternate construtcor used for PCDs
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        public Detector(Models.Detector detector)
        {
            DetID = detector.DetectorID;
            Channel = detector.DetChannel;
            Approach = detector.Approach;
            DetectorModel = detector;
        }

        /// <summary>
        ///     Constructor Used For Data Aggregation
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        public Detector(string detid, string signalid, int channelid, Approach approach, string phasenumber)
        {
            DetID = detid;
            Channel = channelid;
            Approach = approach;
            Phase = phasenumber;
        }

        /// <summary>
        ///     Contrutor Used for RouteManagement
        /// </summary>
        /// <param name="detid"></param>
        /// <param name="signalid"></param>
        /// <param name="channelid"></param>
        /// <param name="laneid"></param>
        /// <param name="approachdirection"></param>
        /// <param name="phasenumber"></param>
        /// <param name="routeorder"></param>
        public Detector(string detid, string signalid, int channelid, Approach approach, string phasenumber,
            int routeorder)
        {
            DetID = detid;
            Channel = channelid;
            Approach = approach;
            Phase = phasenumber;
            Order = routeorder;
        }

        public string DetID { get; }

        public int Channel { get; }

        public int ApproachID { get; set; }
        public Approach Approach { get; set; }

        public VolumeCollection Volumes { get; }

        public string SignalID { get; }

        public string Phase { get; }

        public int Order { get; }

        public LaneType LaneType { get; set; }

        public Models.Detector DetectorModel { get; set; }

        public override string ToString()
        {
            return DetID + " " + Approach.DirectionType.Description;
        }

        public int OccupancyDuringMovementType(int EventCode, int StartPoint, int EndPoint)
        {
            var percentOccupancy = 0;


            return percentOccupancy;
        }

        public int TotalOccupancy()
        {
            var percentOccupancy = 0;
            return percentOccupancy;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class SpeedExportApproachDirection
    {
        //private Data.MOE.SpeedDataTable speedTable;
        public SpeedExportAvgSpeedCollection AvgSpeeds;

        public List<RedToRedCycle> Cycles = new List<RedToRedCycle>();

        private readonly SPM db = new SPM();


        private List<Speed_Events> speedTable = new List<Speed_Events>();


        /// <summary>
        ///     Constructor for Signal phase
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        /// <param name="eventData1"></param>
        /// <param name="region"></param>
        /// <param name="detChannel"></param>
        public SpeedExportApproachDirection(DateTime startDate, DateTime endDate, string signalId, int eventData1,
            string detID, string dir, int pMph, int movementdelay, int decisionpoint,
            int binSize, int minspeedfilter, int distancefromstopbar)
        {
            Direction = dir;

            Phase = eventData1;

            MovementDelay = movementdelay;

            MinSpeedFilter = minspeedfilter;

            DistanceFromStopBar = distancefromstopbar;

            DistanceFromStopBar = distancefromstopbar;

            GetSpeedTable(detID, startDate, endDate);

            GetAverageSpeeds(startDate, endDate, binSize, minspeedfilter, MovementDelay, decisionpoint, signalId,
                eventData1);
        }

        public string Direction { get; }

        public int Phase { get; }

        public int MovementDelay { get; }

        public int DistanceFromStopBar { get; }

        public int MinSpeedFilter { get; }

        private void GetSpeedTable(string detID, DateTime startDate, DateTime endDate)
        {
            //Data.MOETableAdapters.SpeedTableAdapter adapter = new Data.MOETableAdapters.SpeedTableAdapter();

            speedTable = (from r in db.Speed_Events
                where r.DetectorID == detID
                      && r.timestamp > startDate && r.timestamp < endDate
                select r).ToList();
        }


        private void GetAverageSpeeds(DateTime startDate, DateTime endDate, int binSize, int minSpeedFilter,
            int movementDelay, int decisionPoint, string signalId, int phaseNumber)
        {
            var signaltable =
                new ControllerEventLogs(signalId, startDate, endDate, phaseNumber, new List<int> {1, 8, 10});

            AvgSpeeds = new SpeedExportAvgSpeedCollection(startDate, endDate, binSize,
                minSpeedFilter, Cycles);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace MOE.Common.Business
{
    public class SpeedExportApproachDirection
    {
        public List<Cycle> Cycles = new List<Cycle>();

        private string direction;
        public string Direction
        {
            get { return direction; }
        }


        private int phase;
        public int Phase
        {
            get { return phase; }
        }

        private int movementDelay;
        public int MovementDelay
        {
            get { return movementDelay; }
        }

        private int distanceFromStopBar;
        public int DistanceFromStopBar
        {
            get { return distanceFromStopBar; }
        }

        private int minSpeedFilter;
        public int MinSpeedFilter
        {
            get { return minSpeedFilter; }
        }

        Models.SPM db = new Models.SPM();



        List<MOE.Common.Models.Speed_Events> speedTable = new List<MOE.Common.Models.Speed_Events>();

        

        //private Data.MOE.SpeedDataTable speedTable;
        public SpeedExportAvgSpeedCollection AvgSpeeds;

        
        /// <summary>
        /// Constructor for Signal phase
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
            
            direction = dir;

            phase = eventData1;

            movementDelay = movementdelay;

            minSpeedFilter = minspeedfilter;

            distanceFromStopBar = distancefromstopbar;

            GetSpeedTable(detID, startDate, endDate);

            GetAverageSpeeds(startDate, endDate, binSize, minspeedfilter, movementDelay, decisionpoint, signalId, 
                eventData1);

        }

        private void GetSpeedTable(string detID, DateTime startDate, DateTime endDate)
        {
            //Data.MOETableAdapters.SpeedTableAdapter adapter = new Data.MOETableAdapters.SpeedTableAdapter();

            speedTable = (from r in db.Speed_Events
                             where r.DetectorID == detID
                             && r.timestamp > startDate && r.timestamp < endDate
                          select r).ToList<MOE.Common.Models.Speed_Events>();

            
        }

        
        private void GetAverageSpeeds(DateTime startDate, DateTime endDate, int binSize, int minSpeedFilter,
            int movementDelay, int decisionPoint, string signalId, int eventData1)
        {


            MOE.Common.Business.ControllerEventLogs signaltable = new ControllerEventLogs(signalId, startDate, endDate, eventData1,new List<int>{1,8,10});

            AvgSpeeds = new SpeedExportAvgSpeedCollection(startDate, endDate, binSize,
                minSpeedFilter, Cycles);
            
        }
        
    }
}

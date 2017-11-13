using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace MOE.Common.Business
{
    public class SpeedExportApproachDirection
    {
        public List<PhaseCycleBase> Cycles = new List<PhaseCycleBase>();

       
        public string Direction { get; set; }



    
        public int Phase { get; set; }
   

     
        public int MovementDelay { get; set; }


        public int DistanceFromStopBar { get; set; }

        public int MinSpeedFilter { get; set; }

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
        public SpeedExportApproachDirection(DateTime startDate, DateTime endDate, string signalId, int phaseNumber, 
            string detID, string direction, int pMph, int movementdelay, int decisionpoint,  
            int binSize, int minspeedfilter, int distancefromstopbar)
        {

            Direction = direction;

            Phase = phaseNumber;

            MovementDelay = movementdelay;

            MinSpeedFilter = minspeedfilter;

            DistanceFromStopBar = distancefromstopbar;

            GetSpeedTable(detID, startDate, endDate);

            GetAverageSpeeds(startDate, endDate, binSize, minspeedfilter, MovementDelay, decisionpoint, signalId,
                Phase);

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
            int movementDelay, int decisionPoint, string signalId, int phaseNumber)
        {


            MOE.Common.Business.ControllerEventLogs signaltable = new ControllerEventLogs(signalId, startDate, endDate, phaseNumber, new List<int>{1,8,10});

            AvgSpeeds = new SpeedExportAvgSpeedCollection(startDate, endDate, binSize,
                minSpeedFilter, Cycles);
            
        }
        
    }
}

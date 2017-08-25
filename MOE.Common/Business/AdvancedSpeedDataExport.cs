using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class AdvancedSpeedDataExport
    {
        public Dictionary<String, List<SpeedExportAvgSpeed>> Approaches =
                new Dictionary<String, List<SpeedExportAvgSpeed>>();
        public List<String> Directions = new List<string>();


        public AdvancedSpeedDataExport(string signalId, string location, DateTime startDate, DateTime endDate, 
            int binSize, List<string> DayOfweek )
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalId);
            //Get the dates that match the daytype for the given period
            List<DateTime> dtList = new List<DateTime>();
            DateTime tempDate = startDate;
            
            while (tempDate <= endDate)
            {
                if(DayOfweek.Contains(tempDate.DayOfWeek.ToString()))
                {
                    dtList.Add(tempDate);
                }
                tempDate = tempDate.AddDays(1);
            }            
            var table = signal.GetDetectorsForSignalThatSupportAMetric(10);
            foreach (MOE.Common.Models.Detector row in table)
            {
                string direction = row.Approach.DirectionType.Description;
                Approaches.Add(direction, new List<SpeedExportAvgSpeed>());
            }
            //Create approach direction collections for each date in the list
            foreach (DateTime dt in dtList)
            {   
                DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day,
                    endDate.Hour, endDate.Minute, endDate.Second);
                Business.SpeedExportApproachDirectionCollection approachDirectioncollection =
                            new SpeedExportApproachDirectionCollection(dt,
                                dtEnd, signalId, binSize);
                foreach (SpeedExportApproachDirection sad in approachDirectioncollection.List)
                {
                    foreach (SpeedExportAvgSpeed sea in sad.AvgSpeeds.Items)
                    {
                        Approaches[sad.Direction].Add(sea);                        
                    }                    
                }
            }
        }


        
    }
}

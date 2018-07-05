using System;
using System.Collections.Generic;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class AdvancedSpeedDataExport
    {
        public Dictionary<string, List<SpeedExportAvgSpeed>> Approaches =
            new Dictionary<string, List<SpeedExportAvgSpeed>>();

        public List<string> Directions = new List<string>();


        public AdvancedSpeedDataExport(string signalId, string location, DateTime startDate, DateTime endDate,
            int binSize, List<string> DayOfweek)
        {
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetLatestVersionOfSignalBySignalID(signalId);
            //Get the dates that match the daytype for the given period
            var dtList = new List<DateTime>();
            var tempDate = startDate;

            while (tempDate <= endDate)
            {
                if (DayOfweek.Contains(tempDate.DayOfWeek.ToString()))
                    dtList.Add(tempDate);
                tempDate = tempDate.AddDays(1);
            }
            var table = signal.GetDetectorsForSignalThatSupportAMetric(10);
            foreach (var row in table)
            {
                var direction = row.Approach.DirectionType.Description;
                Approaches.Add(direction, new List<SpeedExportAvgSpeed>());
            }
            //Create approach direction collections for each date in the list
            foreach (var dt in dtList)
            {
                var dtEnd = new DateTime(dt.Year, dt.Month, dt.Day,
                    endDate.Hour, endDate.Minute, endDate.Second);
                var approachDirectioncollection =
                    new SpeedExportApproachDirectionCollection(dt,
                        dtEnd, signalId, binSize);
                foreach (var sad in approachDirectioncollection.List)
                foreach (var sea in sad.AvgSpeeds.Items)
                    Approaches[sad.Direction].Add(sea);
            }
        }
    }
}
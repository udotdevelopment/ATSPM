namespace MOE.Common.Business
{
    public class AdvancedVolumeDataExport
    {
        // TODO: When we re-implement volume export, this entire section needs to be reworked.
        //List<DateTime> dtList = new List<DateTime>();
        //public MOE.Common.Data.MOE.VolumeExportDataTable masterTable = new Data.MOE.VolumeExportDataTable();

        //public SortedDictionary<int, string> directions = new SortedDictionary<int, string>();


        //public AdvancedVolumeDataExport(DateTime startDate, DateTime endDate, List<string> DayOfweek,
        //    string signalId, string location, int volumeBinSize)
        //{
        //    BuildDateList(startDate, endDate, DayOfweek, signalId, location, volumeBinSize);
        //}

        //private void BuildDateList(DateTime startDate, DateTime endDate, List<string> DayOfweek, 
        //    string signalId, string location, int volumeBinSize)
        //{
        //   //Get the dates that match the daytype for the given period
        //    DateTime tempDate = startDate;
        //    while (tempDate <= endDate)
        //    {
        //        if(DayOfweek.Contains(tempDate.DayOfWeek.ToString()))
        //        {
        //            dtList.Add(tempDate);
        //        }
        //        tempDate = tempDate.AddDays(1);
        //    }

        //    //Create approach direction collections for each date in the list
        //    foreach (DateTime dt in dtList)
        //    {
        //        DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 
        //            endDate.Hour, endDate.Minute, endDate.Second);
        //        Business.SignalPhaseCollection approachDirectioncollection =
        //                   new MOE.Common.Business.SignalPhaseCollection(dt,
        //                       dtEnd, signalId, //region, 
        //                       true, volumeBinSize,0);

        //        if (directions.Count == 0)
        //        {
        //            foreach (Business.SignalPhase approachDirection in approachDirectioncollection.SignalPhaseList)
        //            {
        //                if (approachDirection.Approach.DirectionType.Description == "Westbound")
        //                {
        //                    directions.Add(1, approachDirection.Approach.DirectionType.Description);
        //                }
        //                else if (approachDirection.Approach.DirectionType.Description == "Eastbound")
        //                {
        //                    directions.Add(2, approachDirection.Approach.DirectionType.Description);
        //                }
        //                else if (approachDirection.Approach.DirectionType.Description == "Northbound")
        //                {
        //                    directions.Add(3, approachDirection.Approach.DirectionType.Description);
        //                }
        //                else if (approachDirection.Approach.DirectionType.Description == "Southbound")
        //                {
        //                    directions.Add(4, approachDirection.Approach.DirectionType.Description);
        //                }
        //            }
        //        }

        //        if (approachDirectioncollection.SignalPhaseList.Count > 0)
        //        {

        //            Data.MOE.VolumeExportDataTable tempTable = approachDirectioncollection.GetVolumeExportData(dt, dtEnd, signalId,
        //            volumeBinSize, directions);
        //            masterTable.Merge(tempTable);

        //        }
        //    }
        //}
    }
}
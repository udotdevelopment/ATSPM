using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MOE.Common;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VolumeExport" in both code and config file together.
    public class VolumeExport : IVolumeExport
    {

        #region IVolumeExport Members

        public DataSet GetExportTable(string signalId, DateTime startDate, DateTime endDate, int binSize,
            List<String> dayOfWeek)
        {
            MOE.Common.Business.AdvancedVolumeDataExport export =
                new MOE.Common.Business.AdvancedVolumeDataExport(startDate, endDate, dayOfWeek, signalId, "DEFAULT",
                    binSize);
            DataSet ds = new DataSet();
            ds.Tables.Add(export.masterTable);
            return ds;
             
        }

        public List<SpeedBin> GetSpeedData(string signalId, string location, 
            DateTime startDate, DateTime endDate, 
            int binSize, List<string> DayOfweek)
        {
            List<SpeedBin> results = new List<SpeedBin>();

            MOE.Common.Business.AdvancedSpeedDataExport export =
                new MOE.Common.Business.AdvancedSpeedDataExport(
                signalId, location, startDate, endDate, 
                binSize, DayOfweek);

            List<string> directions = new List<string>(export.Approaches.Keys);
            int count = export.Approaches[directions[0]].Count;

            for (int i = 0; i < count; i++)
            {
                SpeedBin sb = new SpeedBin();
                sb.Direction1 = directions[0];
                sb.StartDate =  export.Approaches[directions[0]][i].StartTime;
                sb.EndDate = export.Approaches[directions[0]][i].EndTime;
                sb.Direction1AverageSpeed = export.Approaches[directions[0]][i].AvgSpeed;

                if (directions.Count > 1)
                {
                    sb.Direction2 = directions[1];
                    sb.Direction2AverageSpeed = export.Approaches[directions[1]][i].AvgSpeed;
                }

                if (directions.Count > 2)
                {
                    sb.Direction3 = directions[2];
                    sb.Direction3AverageSpeed = export.Approaches[directions[2]][i].AvgSpeed;
                }

                if (directions.Count > 3)
                {
                    sb.Direction4 = directions[3];
                    sb.Direction4AverageSpeed = export.Approaches[directions[3]][i].AvgSpeed;
                }

                results.Add(sb);
            }

            return results;
        }

        public int return1()
        {
            return 1;
        }

        

        #endregion
    }
}

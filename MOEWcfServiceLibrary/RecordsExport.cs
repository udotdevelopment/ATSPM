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
    
    public class RecordsExport : IRecordsExport
    {

        #region IRecordsExport Members

        public DataSet GetExportTable(string signalId, DateTime startDate, DateTime endDate)
        {
            MOE.Common.Business.RecordsExport export =
                new MOE.Common.Business.RecordsExport(signalId, startDate, endDate);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            foreach(MOE.Common.Models.Controller_Event_Log row in export.RecordsTable.Events)
            {
                //DataRow dr = new DataRow();
                //dr[0] = row.SignalID;
                //dr[1] = row.Timestamp;
                //dr[2] = row.EventCode;
                //dr[3] = row.EventParam;

                dt.Rows.Add(row);


            }
            ds.Tables.Add(dt);
            return ds;
             
        }

        

        public int return1()
        {
            return 1;
        }

        

        #endregion
    }
}

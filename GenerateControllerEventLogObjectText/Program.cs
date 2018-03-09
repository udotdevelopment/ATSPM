using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;

namespace GenerateControllerEventLogObjectText
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MOE.Common.Models.Controller_Event_Log> controllerEventLogs = new List<Controller_Event_Log>();
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
                new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder["Data Source"] = "srwtcmoe";
            builder["Password"] = "dontshareme";
            builder["Persist Security Info"] = true;
            builder["User ID"] = "datareader";
            builder["Initial Catalog"] = "MOE";
            Console.WriteLine(builder.ConnectionString);

            SqlConnection sqlConnection1 = new SqlConnection(builder.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "select * from Controller_Event_Log"
                              + " Where Timestamp between '10/17/2017 17:08:13.0' and '10/17/2017 17:11:00.7'"
                              + " and SignalID = '7185'";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();



            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    controllerEventLogs.Add(new Controller_Event_Log
                    {
                        SignalID = reader.GetString(0),
                        Timestamp = reader.GetDateTime(1),
                        EventCode = reader.GetInt32(2),
                        EventParam = reader.GetInt32(3)
                    });
                }
            }
            reader.Close();
            sqlConnection1.Close();
            
                //controllerEventLogRepository.GetSignalEventsBetweenDates("7185",
                //    Convert.ToDateTime("10/17/2017 17:08:13"), Convert.ToDateTime("10/17/2017 17:11:00.7"));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var controllerEventLog in controllerEventLogs)
            {
                stringBuilder.Append("inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log{SignalID =\"");
                stringBuilder.Append(controllerEventLog.SignalID + "\",\nTimestamp = Convert.ToDateTime( \"");
                stringBuilder.Append(controllerEventLog.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"),\nEventParam = ");
                stringBuilder.Append(controllerEventLog.EventParam +",\nEventCode = ");
                stringBuilder.Append(controllerEventLog.EventCode + "});\n\n");
            }
            File.WriteAllText("C:\\Temp\\ControllerEventLogs.txt", stringBuilder.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SignalFTPInfo
    {
        public string SignalID { get; set; }
        public string PrimaryName { get; set; }
        public string Secondary_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string FTP_Directory { get; set; }
        public string IP_Address { get; set; }
        public long SNMPPort { get; set; }
        public bool ActiveFTP { get; set; }
        public int ControllerType { get; set; }
    }
}

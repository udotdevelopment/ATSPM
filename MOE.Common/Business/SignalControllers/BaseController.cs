using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MOE.Common.Business.SignalControllers
{
    public class BaseController
    {
        public String SignalID { get; set; }
        public IPAddress IPAddress { get; set; }


    }
}

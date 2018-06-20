using System.Net;

namespace MOE.Common.Business.SignalControllers
{
    public class BaseController
    {
        public string SignalID { get; set; }
        public IPAddress IPAddress { get; set; }
    }
}
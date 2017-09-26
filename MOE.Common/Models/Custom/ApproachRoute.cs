namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MOE.Common.Models;

    public partial class ApproachRoute
    {
        public List<Models.Signal> GetRouteNonMember()
        {
            MOE.Common.Models.Repositories.ISignalsRepository sr = 
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();

            var members = from r in ApproachRouteDetails
                          select r.Signal;

            return sr.GetAllSignals().Except(members).ToList();
        }

    }
}
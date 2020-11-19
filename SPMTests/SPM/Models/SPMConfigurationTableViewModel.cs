using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class SPMConfigurationTableViewModel
    {
        
        public IEnumerable<MOE.Common.Business.Helpers.ConfigurationRecord> Records { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.WebConfigTool
{
    public class WebConfigToolViewModel
    {
        public Chart.SignalSearchViewModel SignalSearch { get; set; }
        public WebConfigToolViewModel()
        {
            SignalSearch = new Chart.SignalSearchViewModel();
        }

        public WebConfigToolViewModel(IRegionsRepository regionRepositry, IMetricTypeRepository metricRepository)
        {
            SignalSearch = new Chart.SignalSearchViewModel(regionRepositry, metricRepository);
        }
    }
}

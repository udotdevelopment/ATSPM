using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.WebConfigTool
{
    public class WebConfigToolViewModel
    {
        public Chart.SignalSearchViewModel SignalSearch { get; set; }
        public WebConfigToolViewModel()
        {
            SignalSearch = new Chart.SignalSearchViewModel();
        }
    }
}

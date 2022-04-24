using MOE.Common.Models.Repositories;
using MOE.Common.Models.ViewModel.Chart;

namespace MOE.Common.Models.ViewModel.WebConfigTool
{
    public class WebConfigToolViewModel
    {
        public WebConfigToolViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
        }

        public WebConfigToolViewModel(IRegionsRepository regionRepositry, IMetricTypeRepository metricRepository, IJurisdictionRepository jurisdictionRepository)
        {
            SignalSearch = new SignalSearchViewModel(regionRepositry, metricRepository, jurisdictionRepository);
        }

        public SignalSearchViewModel SignalSearch { get; set; }
    }
}
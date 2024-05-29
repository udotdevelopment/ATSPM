using MOE.Common.Models.Repositories;
using MOE.Common.Models.ViewModel.Chart;

namespace MOE.Common.Models.ViewModel.WebConfigTool
{
    public class WebConfigToolViewModel
    {
        public WebConfigToolViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
            SignalMultiSelect = new SignalMultiSelectViewModel();
        }

        public WebConfigToolViewModel(IRegionsRepository regionRepositry, IMetricTypeRepository metricRepository, IJurisdictionRepository jurisdictionRepository, IAreaRepository areaRepository)
        {
            SignalSearch = new SignalSearchViewModel(regionRepositry, metricRepository, jurisdictionRepository, areaRepository);
            SignalMultiSelect = new SignalMultiSelectViewModel();
        }

        public SignalSearchViewModel SignalSearch { get; set; }

        public SignalMultiSelectViewModel SignalMultiSelect { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalSearchViewModel
    {
        private readonly IMetricTypeRepository _metricRepository;

        private readonly IRegionsRepository _regionRepository;

        public SignalSearchViewModel()
        {
            _regionRepository = RegionsRepositoryFactory.Create();
            _metricRepository = MetricTypeRepositoryFactory.Create();
            GetRegions(_regionRepository);
            GetMetrics(_metricRepository);
        }

        public SignalSearchViewModel(IRegionsRepository regionRepositry, IMetricTypeRepository metricRepository)
        {
            GetRegions(regionRepositry);
            GetMetrics(metricRepository);
        }

        //public List<Models.Signal> Signals { get; set; }       
        [Required]
        [Display(Name = "Signal ID")]
        public string SignalID { get; set; }

        public List<Region> Regions { get; set; }
        public int? SelectedRegionID { get; set; }

        public List<SelectListItem> MapMetricsList { get; set; }
        public List<string> ImageLocation { get; set; }

        public void GetMetrics(IMetricTypeRepository metricRepository)
        {
            //MetricTypeRepositoryFactory.SetMetricsRepository(new TestMetricTypeRepository());

            var metricTypes = metricRepository.GetAllToDisplayMetrics();
            MapMetricsList = new List<SelectListItem>();
            foreach (var m in metricTypes)
                MapMetricsList.Add(new SelectListItem {Value = m.MetricID.ToString(), Text = m.ChartName});
        }

        public void GetRegions(IRegionsRepository regionRepository)
        {
            Regions = regionRepository.GetAllRegions();
        }
    }
}
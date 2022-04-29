using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalSearchViewModel
    {
        private readonly IMetricTypeRepository _metricRepository;

        private readonly IRegionsRepository _regionRepository;
        private readonly IJurisdictionRepository _jurisdictionsRepository;

        public SignalSearchViewModel()
        {
            _regionRepository = RegionsRepositoryFactory.Create();
            _metricRepository = MetricTypeRepositoryFactory.Create();
            _jurisdictionsRepository = JurisdictionRepositoryFactory.Create();
            GetRegions(_regionRepository);
            GetMetrics(_metricRepository);
            GetJurisdictions(_jurisdictionsRepository);
        }

        private void GetJurisdictions(IJurisdictionRepository jurisdictionsRepository)
        {
            JurisdictionList = jurisdictionsRepository.GetAllJurisdictions();
        }

        public SignalSearchViewModel(IRegionsRepository regionRepositry, IMetricTypeRepository metricRepository, IJurisdictionRepository jurisdictionRepository)
        {
            GetRegions(regionRepositry);
            GetMetrics(metricRepository);
            GetJurisdictions(jurisdictionRepository);
        }

        //public List<Models.Signal> Signals { get; set; }       
        [Required]
        [Display(Name = "Signal ID")]
        public string SignalID { get; set; }

        public List<Region> Regions { get; set; }
        public int? SelectedRegionID { get; set; }
        public int? SelectedJurisdictionId { get; set; }

        public List<SelectListItem> MapMetricsList { get; set; }
        public List<Jurisdiction> JurisdictionList { get; set; }
        public List<string> ImageLocation { get; set; }

        public void GetMetrics(IMetricTypeRepository metricRepository)
        {
            //MetricTypeRepositoryFactory.SetMetricsRepository(new TestMetricTypeRepository());

            var metricTypes = metricRepository.GetAllToDisplayMetrics().OrderBy( m=> m.DisplayOrder);
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
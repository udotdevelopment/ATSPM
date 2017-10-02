using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MOE.Common.Business;
using MOE.Common.Models.Repositories;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalSearchViewModel
    {
        //public List<Models.Signal> Signals { get; set; }       
        [Required]
        [Display(Name="Signal ID")]
        public string SignalID { get; set; }        
        public List<Models.Region> Regions { get; set; }
        public int? SelectedRegionID { get; set; }
        
        public List<SelectListItem> MapMetricsList { get; set; }
        public List<string> ImageLocation { get; set; }

        private IRegionsRepository _regionRepository;
        private IMetricTypeRepository _metricRepository;

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

        public void GetMetrics(IMetricTypeRepository metricRepository)
        {
            //MetricTypeRepositoryFactory.SetMetricsRepository(new TestMetricTypeRepository());
            
            List<MOE.Common.Models.MetricType> metricTypes = metricRepository.GetAllToDisplayMetrics();
            MapMetricsList = new List<SelectListItem>();
            foreach (MOE.Common.Models.MetricType m in metricTypes)
            {
                MapMetricsList.Add(new SelectListItem { Value = m.MetricID.ToString(), Text = m.ChartName });
            }
        }

        public void GetRegions(IRegionsRepository regionRepository)
        {
            
            Regions = regionRepository.GetAllRegions();
        }
    }
}

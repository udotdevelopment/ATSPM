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

        public SignalSearchViewModel()
        {
            GetRegions();
            GetMetrics();
        }

        public void GetMetrics()
        {
            //MetricTypeRepositoryFactory.SetMetricsRepository(new TestMetricTypeRepository());
            IMetricTypeRepository repository = MetricTypeRepositoryFactory.Create();
            List<MOE.Common.Models.MetricType> metricTypes = repository.GetAllToDisplayMetrics();
            MapMetricsList = new List<SelectListItem>();
            foreach (MOE.Common.Models.MetricType m in metricTypes)
            {
                MapMetricsList.Add(new SelectListItem { Value = m.MetricID.ToString(), Text = m.ChartName });
            }
        }

        public void GetRegions()
        {
            IRegionsRepository repository = RegionsRepositoryFactory.Create();
            Regions = repository.GetAllRegions();
        }
    }
}

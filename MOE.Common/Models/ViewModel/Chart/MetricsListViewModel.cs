using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class MetricsListViewModel
    {
        public MetricsListViewModel(string signalID, int? selectedMetricID)
        {
            SelectedMetricID = selectedMetricID == null?1:selectedMetricID;
            GetMetricsForSignal(signalID);
        }

        [Display(Name = "Metrics List")]
        public List<SelectListItem> MetricsList { get; set; }

        public int? SelectedMetricID { get; set; }

        private void GetMetricsForSignal(string signalID)
        {
            
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetLatestVersionOfSignalBySignalID(signalID);
            MetricsList = new List<SelectListItem>();
            var availableMetrics = signal.GetAvailableMetricsVisibleToWebsite().Where(m => m.ShowOnWebsite).OrderBy(m => m.DisplayOrder);
            if (signal != null)
            {
                foreach (var m in availableMetrics)
                {
                    if (SelectedMetricID != null && SelectedMetricID == m.MetricID)
                    {
                        MetricsList.Add(new SelectListItem
                        {
                            Value = m.MetricID.ToString(),
                            Text = m.ChartName,
                            Selected = true
                        });
                    }
                    else
                    {
                        
                            MetricsList.Add(new SelectListItem { Value = m.MetricID.ToString(), Text = m.ChartName });
                    }
                }
                
            }
        }
    }
}
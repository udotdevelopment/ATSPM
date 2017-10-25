using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class MetricsListViewModel
    {

        [Display(Name = "Metrics List")]
        public List<SelectListItem> MetricsList { get; set; }
        public int? SelectedMetricID { get; set; }

        public MetricsListViewModel(string signalID, int? selectedMetricID)
        {
            SelectedMetricID = selectedMetricID;
            GetMetricsForSignal(signalID);
        }

        private void GetMetricsForSignal(string signalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
            MetricsList = new List<SelectListItem>();
            if (signal != null)
            {
                foreach (Models.MetricType m in signal.GetAvailableMetricsVisibleToWebsite())
                {
                    if (m.ShowOnWebsite)
                    {
                        if (SelectedMetricID != null && SelectedMetricID == m.MetricID)
                        {
                            MetricsList.Add(new SelectListItem { Value = m.MetricID.ToString(), Text = m.ChartName, Selected = true });
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
}

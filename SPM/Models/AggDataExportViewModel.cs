using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOE.Common.Models.Repositories;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Models
{
    public class AggDataExportViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual List<int> MetricTypeIDs { get; set; }
        public virtual ICollection<MetricType> AllMetricTypes { get; set; }
        public virtual List<int> ApproachTypeIDs { get; set; }
        public virtual ICollection<DirectionType> AllApproachTypes { get; set; }
        public virtual List<int> MovementTypeIDs { get; set; }
        public virtual ICollection<MovementType> AllMovementTypes { get; set; }
        public virtual List<int> LaneTypeIDs { get; set; }
        public virtual ICollection<LaneType> AllLaneTypes { get; set; }
        //public List<SelectListItem> AggregateMetricsList { get; set; }

        public List<Route> Routes { get; set; }
        public int SelectedRouteID { get; set; }

        public SignalSearchViewModel SignalSearchViewModel { get; set; }
         
        //[Required]
        //[Display(Name = "Signal ID")]
        //public string SignalId { get; set; }
        [Required]
        public DateTime StartDateDate { get; set; }
        [Required]
        public DateTime EndDateDate { get; set; }
        public int? StartDateHour { get; set; }
        public int? StartDateMinute { get; set; }
        public int? EndDateHour { get; set; }
        public int? EndDateMinute { get; set; }
        public int? Count { get; set; }

        private IMetricTypeRepository _metricRepository;
        public AggDataExportViewModel()
        {
            _metricRepository = MetricTypeRepositoryFactory.Create();
            var regionRepositry = MOE.Common.Models.Repositories.RegionsRepositoryFactory.Create();
            GetMetrics(_metricRepository);
            SignalSearchViewModel = new MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel(regionRepositry, _metricRepository);
        }
        public void GetMetrics(IMetricTypeRepository metricRepository)
        {
            List<MOE.Common.Models.MetricType> metricTypes = metricRepository.GetAllToDisplayMetrics();
            //TODO: change metricTypes to aggregateMetricTypes after Shane's check-in
            //AggregateMetricsList = new List<SelectListItem>();
            //foreach (MOE.Common.Models.MetricType m in metricTypes)
            //{
            //    AggregateMetricsList.Add(new SelectListItem { Value = m.MetricID.ToString(), Text = m.ChartName });
            //}
        }
    }
}
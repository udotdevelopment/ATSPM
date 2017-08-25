using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caseiro.Mvc.PagedList.Attributes;
using Caseiro.Mvc.PagedList.Extensions;
using Caseiro.Mvc.PagedList.Helpers;
using Caseiro.Mvc.PagedList;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class FillSignalsViewModel
    {
        [IgnoreQueryString]
        public int Page { get; set; }
        public string OrderField { get; set; }
        public OrderDirection OrderDirection { get; set; }
        public int? SelectedFilterID { get; set; }
        [Display(Name="Filter Criteria")]
        public string FilterCriteria { get; set; }
        public List<MOE.Common.Models.MetricsFilterType> Filters { get; set; }
        public PagedList<MOE.Common.Models.Signal> Signals { get; private set; }

        public FillSignalsViewModel()
        {
            this.Page = 1;
            this.OrderField = "SignalID";
            this.OrderDirection = OrderDirection.Ascending;              
            GetFilters();
            SetPagedList();
        }

        public FillSignalsViewModel(int page, int? filterType, string filterCriteria)
        {
            this.Page = page;
            this.OrderField = "SignalID";
            this.OrderDirection = OrderDirection.Ascending;
            if(filterType != null)
            {
                this.SelectedFilterID = filterType;
                this.FilterCriteria = filterCriteria;
            }
            GetFilters();
            SetPagedList();
        }


        public void SetPagedList()
        { 
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //MOE.Common.Models.Repositories.SignalsRepositoryTest repository = 
            //    new MOE.Common.Models.Repositories.SignalsRepositoryTest();
            var queryable = repository.GetAllEnabledSignals().AsQueryable();
            if (this.SelectedFilterID != null)
            {
                if (this.SelectedFilterID == 1)
                {
                    queryable = queryable.Where(q => q.SignalID.Contains(this.FilterCriteria)).Select(q => q);
                }
                else if (this.SelectedFilterID == 2)
                {
                    queryable = queryable.Where(q => q.PrimaryName.ToUpper().Contains(this.FilterCriteria.ToUpper())).Select(q => q);
                }
                else if (this.SelectedFilterID == 3)
                {
                    queryable = queryable.Where(q => q.SecondaryName.ToUpper().Contains(this.FilterCriteria.ToUpper())).Select(q => q);
                }                
            }
            this.Signals = queryable.ToPagedList(this.Page, 5, this.OrderField, this.OrderDirection);

        }


        public void GetFilters()
        {
            MOE.Common.Models.Repositories.IMetricFilterTypesRepository repository = 
                MOE.Common.Models.Repositories.MetricFilterTypesRepositoryFactory.Create();
            Filters = repository.GetAllFilters();
        }
    }
}
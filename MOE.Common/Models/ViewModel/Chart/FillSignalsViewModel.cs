using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Caseiro.Mvc.PagedList;
using Caseiro.Mvc.PagedList.Attributes;
using Caseiro.Mvc.PagedList.Extensions;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class FillSignalsViewModel
    {
        public FillSignalsViewModel()
        {
            Page = 1;
            OrderField = "SignalID";
            OrderDirection = OrderDirection.Ascending;
            GetFilters();
            SetPagedList();
        }

        public FillSignalsViewModel(int page, int? filterType, string filterCriteria)
        {
            Page = page;
            OrderField = "SignalID";
            OrderDirection = OrderDirection.Ascending;
            if (filterType != null)
            {
                SelectedFilterID = filterType;
                FilterCriteria = filterCriteria;
            }
            GetFilters();
            SetPagedList();
        }

        [IgnoreQueryString]
        public int Page { get; set; }

        public string OrderField { get; set; }
        public OrderDirection OrderDirection { get; set; }
        public int? SelectedFilterID { get; set; }

        [Display(Name = "Filter Criteria")]
        public string FilterCriteria { get; set; }

        public List<MetricsFilterType> Filters { get; set; }
        public PagedList<Signal> Signals { get; private set; }


        public void SetPagedList()
        {
            var repository =
                SignalsRepositoryFactory.Create();
            //MOE.Common.Models.Repositories.SignalsRepositoryTest repository = 
            //    new MOE.Common.Models.Repositories.SignalsRepositoryTest();
            var queryable = repository.GetAllEnabledSignals().AsQueryable();
            if (SelectedFilterID != null)
                switch (SelectedFilterID)
                {
                    case 1:
                        queryable = queryable.Where(q => q.SignalID.Contains(FilterCriteria)).Select(q => q);
                        break;
                    case 2:
                        queryable = queryable.Where(q => q.PrimaryName.ToUpper().Contains(FilterCriteria.ToUpper()))
                            .Select(q => q);
                        break;
                    case 3:
                        queryable = queryable.Where(q => q.SecondaryName.ToUpper().Contains(FilterCriteria.ToUpper()))
                            .Select(q => q);
                        break;
                    case 4:
                        queryable = queryable.Where(q => q.Jurisdiction.JurisdictionName.ToUpper().Contains(FilterCriteria.ToUpper()))
                            .Select(q => q);
                        break;
                    default:
                        queryable = queryable.Select(q => q);
                        break;
                }
            Signals = queryable.ToPagedList(Page, 5, OrderField, OrderDirection);
        }


        public void GetFilters()
        {
            var repository =
                MetricFilterTypesRepositoryFactory.Create();
            Filters = repository.GetAllFilters();
        }
    }
}
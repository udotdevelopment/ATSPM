using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalMultiSelectViewModel
    {
        public SignalMultiSelectViewModel()
        {
            Signals = new List<SelectListItem>();
            Load();
        }

        public List<SelectListItem> Signals { get; set; }

        public List<string> SelectedSignals { get; set; }

        private void Load()
        {
            var repository = SignalsRepositoryFactory.Create();
            var queryable = repository.GetLatestVersionOfAllSignalsAsQueryable();
            var signals = queryable.ToList();
            
            foreach (var signal in signals)
            {
                Signals.Add(new SelectListItem
                {
                    Value = signal.SignalID,
                    Text = $"{signal.SignalID}: {signal.PrimaryName} & {signal.SecondaryName}"
                });
            }
        }
    }
}

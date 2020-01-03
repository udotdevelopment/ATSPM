using System.Collections.Generic;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalInfoBoxViewModel
    {
        public SignalInfoBoxViewModel(string signalID)
        {
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetLatestVersionOfSignalBySignalID(signalID);
            SetTitle(signal);
            SetDescription(signal);
            SetMetrics(signal);
            SignalID = signalID;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<MetricType> MetricTypes { get; set; }
        public string SignalID { get; set; }

        private void SetDescription(Signal signal)
        {
            if (signal != null && signal.PrimaryName != null &&signal.SecondaryName != null)
            {
                Description = signal.PrimaryName + " " + signal.SecondaryName;
            }
            else
            {
                Description = "Primary Name or Secondary Name is not defined!";
            }
        }

        private void SetMetrics(Signal signal)
        {
            MetricTypes = signal.GetAvailableMetrics();
        }

        private void SetTitle(Signal signal)
        {
            if (SignalID != null && signal.PrimaryName != null && signal.SecondaryName != null)
            {
                Title = signal.SignalID + " - " + signal.PrimaryName + " " + signal.SecondaryName;
            }
            else
            {
                Title = "SignalID is Null or Primary Name is null or Secondary name is null";
            } 
        }
    }
}
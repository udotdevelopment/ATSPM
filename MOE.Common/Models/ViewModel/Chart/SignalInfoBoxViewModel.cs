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
            Description = signal.PrimaryName + " " + signal.SecondaryName;
        }

        private void SetMetrics(Signal signal)
        {
            MetricTypes = signal.GetAvailableMetrics();
        }

        private void SetTitle(Signal signal)
        {
            Title = signal.SignalID + " - " + signal.PrimaryName + " " + signal.SecondaryName;
        }
    }
}
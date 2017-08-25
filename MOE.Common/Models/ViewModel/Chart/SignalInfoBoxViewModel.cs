using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class SignalInfoBoxViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Models.MetricType> MetricTypes { get; set; }
        public string SignalID { get; set; }
        
        public SignalInfoBoxViewModel(string signalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
            SetTitle(signal);
            SetDescription(signal);
            SetMetrics(signal);
            SignalID = signalID;
        }

        private void SetDescription(Signal signal)
        {
            Description = signal.PrimaryName + " " + signal.SecondaryName;
        }

        private void SetMetrics(MOE.Common.Models.Signal signal)
        {
            MetricTypes = signal.GetAvailableMetrics();            
        }

        private void SetTitle(Models.Signal signal)
        {
            Title = signal.SignalID + " - " + signal.PrimaryName + " " + signal.SecondaryName;
        }
    }
}

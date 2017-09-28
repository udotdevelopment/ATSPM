using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ISignalsRepository
    {
        

        List<Models.Signal> GetAllSignals();
        List<Models.Signal> GetAllEnabledSignals();
        List<Models.Signal> GetAllWithGraphDetectors();
        List<Models.Signal> EagerLoadAllSignals();
        List<Models.Signal> EagerLoadAllEnabledSignals();
        Models.Signal GetSignalBySignalID(string signalID);
        SignalFTPInfo GetSignalFTPInfoByID(string signalID);
        void AddOrUpdate(MOE.Common.Models.Signal signal);
        //void Add(MOE.Common.Models.Signal signal);
        void Remove(MOE.Common.Models.Signal signal);
        List<MOE.Common.Business.Pin> GetPinInfo();
        string GetSignalLocation(string signalID);
        void AddList(List<MOE.Common.Models.Signal> signals);
        void Remove(string id);
        void UpdateWithNewVersion(MOE.Common.Models.Signal incomingSignal);
        List<MOE.Common.Models.Signal> GetAllVersionsOfSignalBySignalID(string signalID);
        Common.Models.Signal GetLatestVersionOfSignalBySignalID(string signalID);

         List<Signal> GetLatestVerionOfAllSignals();
         int CheckVersionWithLastDate(string signalId);

        List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId);

        Signal GetVersionOfSignalByDate(string signalId, DateTime startDate);
    }
}

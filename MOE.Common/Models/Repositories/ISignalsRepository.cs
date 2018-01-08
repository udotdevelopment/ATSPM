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
        string GetSignalDescription(string signalId);
        List<Models.Signal> GetAllEnabledSignals();
        List<Models.Signal> EagerLoadAllSignals();
        Models.Signal GetLatestVersionOfSignalBySignalID(string signalID);
        SignalFTPInfo GetSignalFTPInfoByID(string signalID);
        List<SignalFTPInfo> GetSignalFTPInfoForAllFTPSignals();
        void AddOrUpdate(MOE.Common.Models.Signal signal);
        List<MOE.Common.Business.Pin> GetPinInfo();
        string GetSignalLocation(string signalID);
        void AddList(List<MOE.Common.Models.Signal> signals);
        Signal CopySignalToNewVersion(Signal originalVersion);
        List<MOE.Common.Models.Signal> GetAllVersionsOfSignalBySignalID(string signalID);
        List<Signal> GetLatestVersionOfAllSignals();
        int CheckVersionWithFirstDate(string signalId);
        List<Signal> GetLatestVerionOfAllSignalsByControllerType(int controllerTypeId);
        Signal GetVersionOfSignalByDate(string signalId, DateTime startDate);
        Signal GetSignalVersionByVersionId(int versionId);
        void SetVersionToDeleted(int versionId);
        void SetAllVersionsOfASignalToDeleted(string id);
        List<Signal> GetSignalsBetweenDates(string signalId, DateTime startDate, DateTime endDate);
    }
}

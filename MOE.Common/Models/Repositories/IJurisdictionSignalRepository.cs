using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IJurisdictionSignalsRepository
    {
        List<JurisdictionSignal> GetAllJurisdictionsDetails();
        List<JurisdictionSignal> GetByJurisdictionID(int JurisdictionID);
        void DeleteByJurisdictionID(int JurisdictionID);
        void DeleteById(int id);
        void UpdateByJurisdictionAndApproachID(int JurisdictionID, string signalId, int newOrderNumber);
        void Add(JurisdictionSignal newJurisdictionDetail);
        JurisdictionSignal GetByJurisdictionSignalId(int id);
        void MoveJurisdictionSignalUp(int JurisdictionId, int JurisdictionSignalId);
        void MoveJurisdictionSignalDown(int JurisdictionId, int JurisdictionSignalId);
    }
}
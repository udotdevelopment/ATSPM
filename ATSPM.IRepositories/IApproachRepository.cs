using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IApproachRepository
    {
        List<Approach> GetAllApproaches();
        Approach GetApproachByApproachID(int approachID);
        void AddOrUpdate(Approach approach);

        Approach FindAppoachByVersionIdPhaseOverlapAndDirection(int versionId, int phaseNumber, bool isOverlap,
            int directionTypeId);

        void Remove(Approach approach);
        void Remove(int approachID);
        List<Approach> GetApproachesByIds(List<int> excludedApproachIds);
    }
}
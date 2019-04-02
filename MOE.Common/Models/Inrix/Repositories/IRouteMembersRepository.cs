namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IRouteMembersRepository
    {
        void AddRouteMember();
        void RemoveSegmentFromRoute(int segmentID);
    }
}
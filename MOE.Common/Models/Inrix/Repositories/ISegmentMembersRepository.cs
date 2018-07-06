namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ISegmentMembersRepository
    {
        void DeleteMembersBySegmentID(int segmentID);
        void InsertSegmentMembers(int segmentID, string TMC, int Order);
    }
}
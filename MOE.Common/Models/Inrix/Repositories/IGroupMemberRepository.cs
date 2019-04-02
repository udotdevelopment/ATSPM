namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IGroupMemberRepository
    {
        void Add(Group_Members groupMember);
        void DeleteByGroupID(int ID);
    }
}
using ForumCustom.BLL.DTO;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Contract.Manager
{
    public interface IMemberManager
    {
        Task<bool> DeleteMemberForUser(MemberInfo item, UserInfo userInfo);

        Task<bool> ChangeInfo(MemberInfo item, UserInfo user);

        Task<bool> ActivateMember(MemberInfo item);

        Task<bool> DeactivateMember(MemberInfo item);

        Task<bool> ChangeIsActivate(MemberInfo item, bool isActivate);

        Task<bool> AddMemberForUser(MemberInfo item, UserInfo userInfo);

        Task<MemberInfo> GetMemberInfo(UserInfo user);

        MemberInfo FindByNickName(string nickName);
    }
}
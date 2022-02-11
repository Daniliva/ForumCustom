using ForumCustom.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Contract.Manager
{
    public interface IUserManager
    {
        Task<bool> RegistrationUserInfo(string login, string password);

        Task<UserInfo> Login(string login, string password);

        Task<bool> ChangeUser(UserInfo userInfo, string newPassword, string newLogin);

        Task<bool> ChangeRole(UserInfo userInfo, List<string> role, UserInfo admin);

        Task<bool> DeleteUserInfo(UserInfo userInfo, string password);

        Task<List<UserInfo>> GetAll();

        Task<UserInfo> GetUserByLogin(string s);

        Task<UserInfo> GetId(int id);

        Task<List<RoleInfo>> GetAllRoles();
    }
}
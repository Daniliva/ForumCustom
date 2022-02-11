using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;
using System;
using System.Linq;

namespace ForumCustom.WEB.Domain.Transform
{
    public class UserRoleTransform : ITransform<UserRoleModel, UserInfo>
    {
        public UserRoleModel Transform(UserInfo item)
        {
            return new UserRoleModel() { Login = item.Login, ModifyTime = item.ModifyTime, UserId = item.UserId, Roles = String.Join(", ", item.Role.ToArray()) };
        }
    }
}
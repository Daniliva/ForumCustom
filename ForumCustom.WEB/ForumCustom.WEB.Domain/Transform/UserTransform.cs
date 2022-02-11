using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;
using System;
using System.Linq;

namespace ForumCustom.WEB.Domain.Transform
{
    public class UserTransform : ITransform<UserModel, UserInfo>
    {
        public UserModel Transform(UserInfo item)
        {
            return new UserModel() { Login = item.Login, ModifyTime = item.ModifyTime, UserId = item.UserId, Roles = String.Join(", ", item.Role.ToArray()) };
        }
    }
}
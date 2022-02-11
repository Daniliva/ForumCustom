using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.DAL.Entities;
using System;
using System.Linq;

namespace ForumCustom.BLL.Transform
{
    internal class UserTransform : ITransform<User, UserInfo>
    {
        public User Transform(UserInfo item)
        {
            try
            {
                if (item == null)
                    return null;
                return new User { Id = item.UserId, Login = item.Login, ModifyTime = item.ModifyTime };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public UserInfo Transform(User item)
        {
            try
            {
                if (item == null)
                    return null;
                var role = item.RoleUsers.Select(x => x.Role.Name);
                return new UserInfo { UserId = item.Id, Login = item.Login, ModifyTime = item.ModifyTime, Role = role };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}
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

    public class MemberTransform : ITransform<MemberModel, MemberInfo>
    {
        public MemberModel Transform(MemberInfo item)
        {
            return new MemberModel()
            {
                MemberId = item.MemberId,
                CreateTime = item.CreateTime,
                DateOfBirth = item.DateOfBirth,
                Email = item.Email,
                FirstName = item.FirstName,
                IsActive = item.IsActive,
                LastName = item.LastName,
                ModifyTime = item.ModifyTime,
                NickName = item.NickName
            };
        }
    }
}
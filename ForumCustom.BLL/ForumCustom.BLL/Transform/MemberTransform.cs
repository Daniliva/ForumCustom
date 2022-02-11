using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.DAL.Entities;
using System;

namespace ForumCustom.BLL.Transform
{
    internal class MemberTransform : ITransform<Member, MemberInfo>
    {
        public Member Transform(MemberInfo item)
        {
            try
            {
                return new Member
                {
                    Id = item.MemberId,
                    CreateTime = item.CreateTime,
                    DateOfBirth = item.DateOfBirth,
                    Email = item.Email,
                    Firstname = item.FirstName,
                    IsActive = item.IsActive,
                    Lastname = item.LastName,
                    ModifyTime = item.ModifyTime,
                    Nickname = item.NickName
                };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public MemberInfo Transform(Member item)
        {
            try
            {
                return new MemberInfo
                {
                    MemberId = item.Id,
                    CreateTime = item.CreateTime,
                    DateOfBirth = item.DateOfBirth,
                    Email = item.Email,
                    FirstName = item.Firstname,
                    IsActive = item.IsActive,
                    LastName = item.Lastname,
                    ModifyTime = item.ModifyTime,
                    NickName = item.Nickname
                };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}
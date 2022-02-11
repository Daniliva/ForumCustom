using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.BLL.Transform;
using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Manager
{
    public class MemberManager : IMemberManager
    {
        private readonly ITransform<Member, MemberInfo> _memberTransform;
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<User> _userRepository;

        public MemberManager(IRepository<Member> memberRepository, IRepository<User> userRepository)
        {
            _memberTransform = new MemberTransform();
            _memberRepository = memberRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> DeleteMemberForUser(MemberInfo item, UserInfo userInfo)
        {
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(x => x.Login == userInfo.Login).Result.FirstOrDefault();
                var memberInfo = FindByNickName(item.NickName);
                if (memberInfo == null && userFind == null)
                    throw new ChangeException("");

                var member = _memberRepository.Get(memberInfo.MemberId).Result;
                if (member != null)
                {
                    if (member.ModifyTime != item.ModifyTime)
                        throw new OutdatedException();
                    if (userInfo.ModifyTime != userFind.ModifyTime)
                        throw new OutdatedException();

                    _memberRepository.Delete(member.Id);

                    _userRepository.Update(userFind);
                    return true;
                }

                return false;
            });
        }

        public async Task<bool> ChangeInfo(MemberInfo item, UserInfo user)
        {
            return await Task.Run(() =>
            {
                var member = _memberRepository.Get(item.MemberId).Result;
                if (member != null)
                {
                    if (member.ModifyTime != item.ModifyTime && member.User.Login != user.Login)
                        throw new OutdatedException();

                    if (FindByNickName(item.NickName) == null || FindByNickName(item.NickName).MemberId == item.MemberId)
                    {
                        member.CreateTime = item.CreateTime;
                        member.DateOfBirth = item.DateOfBirth;
                        member.Email = item.Email;
                        member.Firstname = item.FirstName;
                        member.Lastname = item.LastName;
                        member.Nickname = item.NickName;
                        member.ModifyTime = DateTime.Now;
                        _memberRepository.Update(member).Wait();
                    }

                    return true;
                }
                else
                {
                    return AddMemberForUser(item, user).Result;
                }
            });
        }

        public async Task<bool> ActivateMember(MemberInfo item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return ChangeIsActivate(item, true);
                }
                catch (NullReferenceException e)
                {
                    throw new ChangeException("");
                }
            });
        }

        public async Task<bool> DeactivateMember(MemberInfo item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return ChangeIsActivate(item, false);
                }
                catch (NullReferenceException e)
                {
                    throw new ChangeException("");
                }
            });
        }

        public async Task<bool> ChangeIsActivate(MemberInfo item, bool isActivate)
        {
            /*  if (FindByNickName(item.Nickname) != null)
                  return false;*/

            var member = await _memberRepository.Get(item.MemberId);
            if (member.ModifyTime != item.ModifyTime)
                throw new OutdatedException();
            member.IsActive = isActivate;
            member.ModifyTime = DateTime.Now;
            try
            {
                _memberRepository.Update(member);
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }

            return true;
        }

        public async Task<bool> AddMemberForUser(MemberInfo item, UserInfo userInfo)
        {
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(user => user.Login == userInfo.Login).Result.FirstOrDefault();
                if (userInfo.ModifyTime != userFind.ModifyTime)
                    throw new OutdatedException();
                if (FindByNickName(item.NickName) == null)
                {
                    try
                    {
                        var member = _memberTransform.Transform(item);
                        member.User = userFind;
                        member.Id = 0;
                        member.ModifyTime = DateTime.Now;
                        member.CreateTime = DateTime.Now;
                        _memberRepository.Add(member).Wait();
                        return true;
                    }
                    catch (NullReferenceException e)
                    {
                        throw new ChangeException("");
                    }
                }

                return false;
            });
        }

        public async Task<MemberInfo> GetMemberInfo(UserInfo user)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var memberFind =
                        _memberRepository
                            .FindAsync(member => member.User.Id == user.UserId && member.User.Login == user.Login)
                            .Result;
                    if (memberFind == null)
                        return null;
                    var member = memberFind.FirstOrDefault();
                    if (member == null)
                        member = new Member { Id = -1 };
                    return _memberTransform.Transform(member);
                }
                catch (NullReferenceException e)
                {
                    throw new ChangeException("");
                }
            });
        }

        public MemberInfo FindByNickName(string nickName)
        {
            try
            {
                var memberFind = _memberRepository.FindAsync(member => member.Nickname == nickName).Result.FirstOrDefault();
                return memberFind == null ? null : _memberTransform.Transform(memberFind);
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}
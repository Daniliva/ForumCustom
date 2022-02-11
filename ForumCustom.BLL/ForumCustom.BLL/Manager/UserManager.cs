using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.BLL.Transform;
using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITransform<User, UserInfo> _userTransform;
        private readonly IRepository<Role> _roleRepository;

        public UserManager(IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            _userTransform = new UserTransform();
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> RegistrationUserInfo(string login, string password)
        {
            /*  if (GetUserByLogin(login) == null)
              {
                  _userRepository.Add(new User { Login = login, Password = password, ModifyTime = DateTime.Now });

                  return true;
              }
              return false;*/
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(user => user.Login == login && user.Password == password).Result
                    .FirstOrDefault();
                if (userFind == null)
                {
                    var role = _roleRepository.FindAsync(x => x.Name.ToLower().Contains("user")).Result.FirstOrDefault();

                    _userRepository.Add(new User
                    {
                        Login = login,
                        Password = password,
                        ModifyTime = DateTime.Now,
                        RoleUsers = new List<RoleUser>() { new RoleUser(){ Role = role}
                    }
                    }).Wait();

                    return true;
                }
                return false;
            });
        }

        public async Task<UserInfo> Login(string login, string password)
        {
            // var userFind = _userRepository.Find(user => user.Login == login && user.Password == password).FirstOrDefault();
            /*   Task<UserInfo> user = new Task<UserInfo>(
                   () =>
                   {
                       var userFind = _userRepository.Find(user => user.Login == login && user.Password == password)
                           .FirstOrDefault();
                       return _userTransform.Transform(userFind);
                   });*/
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(user => user.Login == login && user.Password == password).Result
                    .FirstOrDefault();
                return _userTransform.Transform(userFind);
            }); //await user;
        }

        public async Task<UserInfo> GetId(int id)
        {
            // var userFind = _userRepository.Find(user => user.Login == login && user.Password == password).FirstOrDefault();
            /*   Task<UserInfo> user = new Task<UserInfo>(
                   () =>
                   {
                       var userFind = _userRepository.Find(user => user.Login == login && user.Password == password)
                           .FirstOrDefault();
                       return _userTransform.Transform(userFind);
                   });*/
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(user => user.Id == id).Result
                    .FirstOrDefault();
                return _userTransform.Transform(userFind);
            }); //await user;
        }

        public async Task<bool> ChangeUser(UserInfo userInfo, string newPassword, string newLogin)
        {
            return await Task.Run(() =>
            {
                var user = _userRepository.Get(userInfo.UserId).Result;
                if (user == null || user.Login == newLogin) return false;
                if (userInfo.ModifyTime != user.ModifyTime)
                    throw new OutdatedException();
                else
                {
                    if (GetUserByLogin(newLogin) != null)
                    {
                        throw new Exception("Passwords aren't similar. Check password,please!");
                    }
                    else
                    {
                        user.Login = newLogin;
                        user.Password = newPassword;
                        user.ModifyTime = DateTime.Now;
                        _userRepository.Update(user);
                        return true;
                    }
                }
            });
        }

        public async Task<UserInfo> GetUserByLogin(string login)
        {
            var userFind = await _userRepository.FindAsync(user => user.Login == login);

            return _userTransform.Transform(userFind.FirstOrDefault());

            /*    return await Task.Run(() => {     var userFind = _userRepository.Find(user =>  user.Login == login).FirstOrDefault();
                return _userTransform.Transform(userFind); });
                */
        }

        public async Task<bool> DeleteUserInfo(UserInfo userInfo, string password)
        {
            /* var userFind = _userRepository.Find(user =>
                 user.Password == password &
                 user.Login == userInfo.Login &
                 user.Id == userInfo.Id).FirstOrDefault();
             if (userFind == null)
             { return false; }

             if (userFind.ModifyTime != userInfo.ModifyTime)
             {
                 throw new OutdatedException();
             }

             _userRepository.Delete(userInfo.Id);    return true;*/
            return await Task.Run(() =>
            {
                var userFind = _userRepository.FindAsync(user =>
                    user.Password == password &
                    user.Login == userInfo.Login &
                    user.Id == userInfo.UserId).Result.FirstOrDefault();
                if (userFind == null)
                { return false; }

                if (userFind.ModifyTime != userInfo.ModifyTime)
                {
                    throw new OutdatedException();
                }

                _userRepository.Delete(userInfo.UserId); return true;
            });
        }

        public async Task<List<UserInfo>> GetAll()
        {
            var users = await _userRepository.GetAll();
            try
            {
                return users.Select(x => _userTransform.Transform(x)).ToList();
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public async Task<bool> ChangeRole(UserInfo userInfo, List<string> role, UserInfo admin)
        {
            var userFind = (await _userRepository.FindAsync(user =>
                 user.Login == userInfo.Login &
                 user.Id == userInfo.UserId)).FirstOrDefault();

            return await Task.Run(() =>
            {
                if (userFind != null)
                {
                    userFind.RoleUsers = new List<RoleUser>();
                    foreach (var name in role)
                    {
                        var rolel = _roleRepository.FindAsync(x => x.Name.ToLower().Contains(name.ToLower())).Result.FirstOrDefault();
                        userFind.RoleUsers.Add(new RoleUser() { Role = rolel });
                    }

                    _userRepository.Update(userFind);

                    return true;
                }
                return false;
            });
        }

        public async Task<List<RoleInfo>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAll();
            return roles.Select(x => new RoleInfo() { Id = x.Id, Name = x.Name }
            ).ToList();
        }
    }
} /*  public bool Add(UserDTO item)

          {
              if (GetUserByLogin(item.Login) == null)
              {
                  _iUnitOfWork.GetRepository<IRepository<User>>().Add(_userTransformDto.Transform(item));
                  _iUnitOfWork.Commit();
                  return true;
              }

              return false;
          }

          public async Task<bool> Delete(int? id)
          {
              if (id != null)
              {
                  if (_iUnitOfWork.GetRepository<IRepository<User>>().Get((int)id) != null)
                  {
                      _iUnitOfWork.GetRepository<IRepository<User>>().Delete((int)id);
                      _iUnitOfWork.Commit();
                      return true;
                  }

                  return false;
              }

              return false;
          }

          public async Task<bool> Update(UserDTO item)
          {
              var user = _iUnitOfWork.GetRepository<IRepository<User>>().Get(item.Id);
              if (user != null)
              {
                  if (item.Login != user.Login)
                      if (GetUserByLogin(item.Login) == null)
                          user.Login = item.Login;
                  user.Password = item.Password;
                  _iUnitOfWork.GetRepository<IRepository<User>>().Update(user);
                  _iUnitOfWork.Commit();
                  return true;
              }

              return false;
          }

          public UserDTO Login(string newLogin, string password)
          {
              Func<User, bool> userLogin = User =>
              {
                  if (User.Login == newLogin)
                      if (User.Password == password)
                          return true;
                  return false;
              };
              var user = _iUnitOfWork.GetRepository<IRepository<User>>().Find(userLogin).FirstOrDefault();
              return _userTransformDto.Transform(user);
          }

         */
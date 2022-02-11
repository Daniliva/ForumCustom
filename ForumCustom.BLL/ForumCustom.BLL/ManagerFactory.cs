using ForumCustom.BLL.Contract;
using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.Manager;
using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.Entities;
using ForumCustom.DAL.Factory;
using System;
using System.Collections.Generic;

namespace ForumCustom.BLL
{
    public class ManagerFactory : IManagerFactory
    {
        private readonly Dictionary<Type, object> _managerCollection;

        public ManagerFactory(string connectionString)
        {
            UnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(connectionString);
            _managerCollection = new Dictionary<Type, object>();
            var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            _managerCollection.Add(typeof(IUserManager), new UserManager(unitOfWork.GetRepository<IRepository<User>>(), unitOfWork.GetRepository<IRepository<Role>>()));
            _managerCollection.Add(typeof(IMemberManager), new MemberManager(unitOfWork.GetRepository<IRepository<Member>>(), unitOfWork.GetRepository<IRepository<User>>()));
            _managerCollection.Add(typeof(ITopicManager), new TopicManager(unitOfWork.GetRepository<IRepository<Member>>(), unitOfWork.GetRepository<IRepository<Topic>>()));
            _managerCollection.Add(typeof(ICommentManager), new CommentManager(unitOfWork.GetRepository<IRepository<Comment>>(), unitOfWork.GetRepository<IRepository<Topic>>()));
        }

        public T GetManager<T>()
        {
            var type = typeof(T);
            return (T)_managerCollection[type];
        }
    }
}
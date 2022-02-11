using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.EF;
using ForumCustom.DAL.Entities;
using ForumCustom.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ForumCustom.DAL.Factory
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ForumCustomContext>();
            var options = optionsBuilder
                .UseSqlServer(_connectionString)
                .Options;
            optionsBuilder.UseSqlServer(_connectionString,
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            var context = new ForumCustomContext(options);
            var repositoryCollection = new Dictionary<Type, object>();
            repositoryCollection.Add(typeof(IRepository<User>), new UserRepository(context));
            repositoryCollection.Add(typeof(IRepository<Member>), new MemberRepository(context));
            repositoryCollection.Add(typeof(IRepository<Comment>), new CommentRepository(context));
            //repositoryCollection.Add(typeof(IRepository<RoleUser>), new RoleUserRepository(context));
            repositoryCollection.Add(typeof(IRepository<Role>), new RoleRepository(context));
            repositoryCollection.Add(typeof(IRepository<Topic>), new TopicRepository(context));
            IUnitOfWork unitOfWork = new EfUnitOfWork(context, repositoryCollection);
            return unitOfWork;
        }
    }
}
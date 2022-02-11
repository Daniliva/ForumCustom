using ForumCustom.DAL.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ForumCustom.DAL
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _databaseContext;
        private readonly Dictionary<Type, object> _repositoryCollection;

        public EfUnitOfWork(DbContext databaseContext, Dictionary<Type, object> repositoryCollection)
        {
            this._databaseContext = databaseContext;
            this._repositoryCollection = repositoryCollection;
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
        }

        public void Commit()
        {
            _databaseContext.SaveChanges();
        }

        public T GetRepository<T>()
        {
            var type = typeof(T);
            return (T)_repositoryCollection[type];
        }
    }
}
using System;

namespace ForumCustom.DAL.Contract.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        T GetRepository<T>();
    }
}
using ForumCustom.BLL.DTO;
using System;
using System.Collections.Generic;

namespace ForumCustom.BLL.Contract.Manager
{
    public interface IManager<T>
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetRange(int start, int count, Func<T, bool> predicate = null);

        T Get(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);

        void Add(T item, UserInfo user);

        void Update(T item, UserInfo user);

        void Delete(int id, UserInfo user);
    }
}
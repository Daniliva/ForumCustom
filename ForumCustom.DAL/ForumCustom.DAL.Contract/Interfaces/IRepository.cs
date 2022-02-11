using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumCustom.DAL.Contract.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> Get(int id);

        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);

        Task Add(T item);

        Task Update(T item);

        Task Delete(int id);
    }
}
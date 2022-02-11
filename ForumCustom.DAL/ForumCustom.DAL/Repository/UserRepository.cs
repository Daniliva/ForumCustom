using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.EF;
using ForumCustom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumCustom.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly ForumCustomContext _db;

        public UserRepository(ForumCustomContext context)
        {
            _db = context;
            _db.Users.Include(c => c.RoleUsers).ThenInclude(c => c.Role).ToList();
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> FindAsync(Func<User, bool> predicate)
        {
            var list = await GetAll();

            return list.Where(predicate).ToList();
        }

        public async Task Add(User item)
        {
            await _db.Users.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(User item)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _db.Users.Remove(await _db.Users.FindAsync(id));
            await _db.SaveChangesAsync();
        }
    }
}
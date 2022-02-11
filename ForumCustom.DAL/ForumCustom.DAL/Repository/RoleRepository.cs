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
    public class RoleRepository : IRepository<Role>
    {
        private readonly ForumCustomContext _db;

        public RoleRepository(ForumCustomContext context)
        {
            _db = context;
        }

        public async Task<List<Role>> GetAll()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<Role> Get(int id)
        {
            return await _db.Roles.FindAsync(id);
        }

        public async Task<IEnumerable<Role>> FindAsync(Func<Role, bool> predicate)
        {
            var list = await GetAll();

            return list.Where(predicate).ToList();
        }

        public async Task Add(Role item)
        {
            await _db.Roles.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Role item)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _db.Roles.Remove(await _db.Roles.FindAsync(id));
            await _db.SaveChangesAsync();
        }
    }
}
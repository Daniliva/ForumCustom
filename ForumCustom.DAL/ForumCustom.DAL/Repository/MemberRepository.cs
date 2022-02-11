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
    public class MemberRepository : IRepository<Member>
    {
        private readonly ForumCustomContext _db;

        public MemberRepository(ForumCustomContext context)
        {
            _db = context;
            _db.Members.Include(x => x.User).ToList();
        }

        public async Task<List<Member>> GetAll()
        {
            return await _db.Members.ToListAsync();
        }

        public async Task<Member> Get(int id)
        {
            return await _db.Members.FindAsync(id);
        }

        public async Task<IEnumerable<Member>> FindAsync(Func<Member, bool> predicate)
        {
            var list = await GetAll();

            return list.Where(predicate).ToList();
        }

        public async Task Add(Member item)
        {
            await _db.Members.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Member item)
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
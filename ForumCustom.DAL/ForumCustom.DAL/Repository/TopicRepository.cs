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
    public class TopicRepository : IRepository<Topic>
    {
        private readonly ForumCustomContext _db;

        public TopicRepository(ForumCustomContext context)
        {
            _db = context;
            _db.Topics.Include(x => x.Comments).Include(x => x.Member).ToList();
        }

        public async Task<List<Topic>> GetAll()
        {
            return await _db.Topics.ToListAsync();
        }

        public async Task<Topic> Get(int id)
        {
            return await _db.Topics.FindAsync(id);
        }

        public async Task<IEnumerable<Topic>> FindAsync(Func<Topic, bool> predicate)
        {
            var list = await GetAll();

            return list.Where(predicate).ToList();
        }

        public async Task Add(Topic item)
        {
            await _db.Topics.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Topic item)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            _db.Topics.Include(x => x.Comments).Include(x => x.Member);
        }

        public async Task Delete(int id)
        {
            _db.Topics.Remove(await _db.Topics.FindAsync(id));
            await _db.SaveChangesAsync();
        }
    }
}
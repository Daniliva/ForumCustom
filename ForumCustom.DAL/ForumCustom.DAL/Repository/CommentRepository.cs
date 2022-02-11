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
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ForumCustomContext _db;

        public CommentRepository(ForumCustomContext context)
        {
            _db = context;
            _ = _db.Comments.Include(x => x.Member).ToList();
        }

        public async Task<List<Comment>> GetAll()
        {
            return await _db.Comments.ToListAsync();
        }

        public async Task<Comment> Get(int id)
        {
            return await _db.Comments.FindAsync(id);
        }

        public async Task<IEnumerable<Comment>> FindAsync(Func<Comment, bool> predicate)
        {
            var list = await GetAll();

            return list.Where(predicate).ToList();
        }

        public async Task Add(Comment item)
        {
            await _db.Comments.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Comment item)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _db.Comments.Remove(await _db.Comments.FindAsync(id));
            await _db.SaveChangesAsync();
        }
    }
}
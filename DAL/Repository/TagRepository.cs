using ArticlesTestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesTestTask.DAL.Repository
{
    public interface ITagRepository
    {
        Task<Tag?> GetById(long id);
        Task<List<Tag>> GetByIds(List<long> ids);
        Task<Tag?> GetByLowerName(string name);
        Task<List<Tag>> GetList();
        Task<Tag> Create(string name);
    }

    public class TagRepository : ITagRepository
    {
        private readonly ArticleContext _context;

        public TagRepository(ArticleContext context)
        {
            _context = context;
        }

        public async Task<Tag?> GetById(long id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<List<Tag>> GetByIds(List<long> ids)
        {
            return await _context.Tags
                .Where(i => ids.Contains(i.Id))
                .ToListAsync();
        }

        public async Task<Tag?> GetByLowerName(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Tag>> GetList()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> Create(string name)
        {
            var tag = new Tag { Name = name };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }
    }
}

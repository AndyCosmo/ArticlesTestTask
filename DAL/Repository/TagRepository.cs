using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticlesTestTask.DAL.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ArticleContext _context;

        public TagRepository(ArticleContext context)
        {
            _context = context;
        }

        public async Task<Tag?> GetByLowerName(string name, CancellationToken ct = default)
        {
            name = name.ToLower();

            return await _context.Tags
                .FirstOrDefaultAsync(i => i.NameLower == name, ct);
        }

        public async Task<Tag> Add(string name, CancellationToken ct = default)
        {
            var tag = new Tag();
            tag.SetName(name);

            await _context.Tags.AddAsync(tag, ct);
            await _context.SaveChangesAsync(ct);

            return tag;
        }
    }
}

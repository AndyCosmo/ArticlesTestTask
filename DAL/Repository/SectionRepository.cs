using ArticlesTestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesTestTask.DAL.Repository
{
    public interface ISectionRepository
    {
        Task<List<Section>> GetList();
        Task<Section?> GetByTagIds(List<long> tagIds);
        Task<Section> Add(List<Tag> tags);
        Task DeleteUnusedSections();
    }

    public class SectionRepository : ISectionRepository
    {
        private readonly ArticleContext _context;

        public SectionRepository(ArticleContext articleContext)
        {
            _context = articleContext;
        }

        public async Task<List<Section>> GetList()
        {
            return await _context.Sections
                .Include(i => i.Tags)
                .Include(i => i.Articles)
                .OrderByDescending(i => i.Articles.Count)
                .ToListAsync();
        }

        public async Task<Section?> GetByTagIds(List<long> tagIds)
        {
            // Для начала поищем, есть ли вообще разделы с таким же количеством тегов
            var sectionIdsWithSameTagsCount = await _context.Sections
                .Where(i => i.Tags.Count == tagIds.Count)
                .Select(i => i.Id)
                .CountAsync();

            if (sectionIdsWithSameTagsCount == 0)
            {
                return null;
            }

            return await _context.Sections
                .Include(i => i.Tags)
                .Include(i => i.Articles)
                .Where(i => i.Tags.Select(tag => tag.Id).All(tagId => tagIds.Contains(tagId)))
                .FirstOrDefaultAsync();
        }

        public async Task<Section> Add(List<Tag> tags)
        {
            string name = string.Join(", ", tags.Select(t => t.Name));

            Section section = new()
            {
                Name = name.Length > 1024 ? name.Substring(0, 1024) : name,
                Tags = tags
            };

            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();

            return section;
        }

        public async Task DeleteUnusedSections()
        {
            // Удаляем неиспользуемые разделы
            await _context.Sections
                .Include(i => i.Articles)
                .Where(i => i.Articles.Count == 0)
                .ExecuteDeleteAsync();
        }
    }
}

using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Section = ArticlesTestTask.DAL.Models.Section;

namespace ArticlesTestTask.DAL.Repository
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ArticleContext _context;

        public SectionRepository(ArticleContext articleContext)
        {
            _context = articleContext;
        }

        public async Task<int> GetCount(CancellationToken ct = default)
        {
            return await _context.Sections
                .CountAsync(ct);
        }

        public async Task<List<Section>> GetList(int pageNum = 1, int perPage = 10, CancellationToken ct = default)
        {
            return await _context.Sections
                .OrderByDescending(i => i.ArticlesCount)
                .ThenBy(i => i.Name)
                .Skip((pageNum - 1) * perPage)
                .Take(perPage)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Section?> GetByTagIds(List<long> tagIds, CancellationToken ct = default)
        {
            // Для начала поищем, есть ли вообще разделы с таким же количеством тегов
            var sectionIdsWithSameTagsCount = await _context.Sections
                .Where(i => i.Tags.Count == tagIds.Count)
                .Select(i => i.Id)
                .CountAsync(ct);

            if (sectionIdsWithSameTagsCount == 0)
            {
                return null;
            }

            return await _context.Sections
                .Include(i => i.Tags)
                .Where(i => i.Tags.Select(tag => tag.Id).All(tagId => tagIds.Contains(tagId)))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Section> Add(List<Tag> tags, CancellationToken ct = default)
        {
            string name = string.Join(", ", tags.Select(t => t.Name));

            Section section = new()
            {
                Name = name.Length > 1024 ? name.Substring(0, 1024) : name,
                Tags = tags,
            };

            await _context.Sections.AddAsync(section, ct);
            await _context.SaveChangesAsync(ct);

            return section;
        }

        public async Task<bool> Exists(long id, CancellationToken ct = default)
        {
            return await _context.Sections
                .AnyAsync(i => i.Id == id, ct);
        }

        public async Task DeleteUnusedSections(CancellationToken ct = default)
        {
            // Удаляем неиспользуемые разделы
            await _context.Sections
                .Where(i => i.ArticlesCount == 0)
                .ExecuteDeleteAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}

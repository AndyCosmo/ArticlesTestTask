using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticlesTestTask.DAL.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleContext _context;

        public ArticleRepository(ArticleContext articleContext)
        {
            _context = articleContext;
        }

        public async Task<Article?> GetById(long id, CancellationToken ct = default)
        {
            return await _context.Articles
                .Include(i => i.Tags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<int> GetCountBySectionId(long sectionId, CancellationToken ct = default)
        {
            return await _context.Articles
                .Include(i => i.Section)
                .Where(i => i.SectionId == sectionId)
                .CountAsync(ct);
        }

        public async Task<List<Article>> GetListBySectionId(long sectionId, int pageNum = 1, int perPage = 10, CancellationToken ct = default)
        {
            return await _context.Articles
                .Where(i => i.SectionId == sectionId)
                .OrderByDescending(i => i.LastActivityAt)
                .ThenBy(i => i.Name)
                .Skip((pageNum - 1) * perPage)
                .Take(perPage)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Article> Add(Article article, List<long> tagIds, CancellationToken ct = default)
        {
            article.Tags = new List<ArticleTag>();

            // Прикрепляем теги к статье
            for (int i = 0; i < tagIds.Count; i++)
            {
                article.Tags.Add(new ArticleTag
                {
                    TagId = tagIds[i],
                    Order = i
                });
            }

            await _context.Articles.AddAsync(article, ct);
            await _context.SaveChangesAsync(ct);

            return article;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateArticleTags(Article article, List<long> tagIds, CancellationToken ct = default)
        {
            // Удаляем старые теги
            _context.ArticleTags.RemoveRange(article.Tags);

            // Прикрепляем теги к статье
            for (int i = 0; i < tagIds.Count; i++)
            {
                article.Tags.Add(new ArticleTag
                {
                    TagId = tagIds[i],
                    Order = i
                });
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}

using ArticlesTestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesTestTask.DAL.Repository
{
    public interface IArticleRepository
    {
        Task<List<Article>> Get();
        Task<Article?> GetById(long id);
        Task<List<Article>> GetListBySectionId(long sectionId);
        Task<Article> Add(Article item, List<long> tagIds);
        Task Update(Article article, List<long> tagIds);
        Task SaveChangesAsync();
    }

    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleContext _context;

        public ArticleRepository(ArticleContext articleContext)
        {
            _context = articleContext;
        }

        public async Task<List<Article>> Get()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetById(long id)
        {
            return await _context.Articles
                .Include(i => i.Tags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.Section)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Article>> GetListBySectionId(long sectionId)
        {
            return await _context.Articles
                .Include(i => i.Tags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.Section)
                .Where(i => i.Section != null && i.Section.Id == sectionId)
                .OrderByDescending(i => i.UpdatedAt != null ? i.UpdatedAt : i.CreatedAt)
                .ToListAsync();
        }

        public async Task<Article> Add(Article article, List<long> tagIds)
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

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return article;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Article article, List<long> tagIds)
        {
            // TODO: не удалять все связи статьи с тегами, а только те которые убрали?

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

            await _context.SaveChangesAsync();
        }
    }
}

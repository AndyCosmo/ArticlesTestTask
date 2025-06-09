using ArticlesTestTask.DAL.Models;

namespace ArticlesTestTask.DAL.Repository.Interfaces
{
    public interface IArticleRepository
    {
        Task<Article?> GetById(long id, CancellationToken ct = default);
        Task<int> GetCountBySectionId(long sectionId, CancellationToken ct = default);
        Task<List<Article>> GetListBySectionId(
            long sectionId,
            int pageNum = 1,
            int perPage = 10,
            CancellationToken ct = default);
        Task<Article> Add(Article item, List<long> tagIds, CancellationToken ct = default);
        Task UpdateArticleTags(Article article, List<long> tagIds, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
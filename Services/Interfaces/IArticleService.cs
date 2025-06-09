using ArticlesTestTask.Contracts.Requests;
using ArticlesTestTask.Contracts.Responses;

namespace ArticlesTestTask.Services.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleResponse?> GetArticleById(long id, CancellationToken ct = default);
        Task<PagedList<SectionResponse>> GetSections(int pageNum = 1, int perPage = 10, CancellationToken ct = default);
        Task<PagedList<ArticleResponse>> GetArticlesBySectionId(long sectionId, int pageNum = 1, int perPage = 10,
            CancellationToken cancellationToken = default);
        Task<ArticleResponse?> CreateArticle(ArticleCreateUpdateRequest articleRequest, CancellationToken ct = default);
        Task<ArticleResponse?> UpdateArticle(long id, ArticleCreateUpdateRequest articleRequest, CancellationToken ct = default);
    }
}

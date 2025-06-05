using ArticlesTestTask.Contracts.Requests;
using ArticlesTestTask.Contracts.Responses;
using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.DAL.Repository;
using static System.Collections.Specialized.BitVector32;

namespace ArticlesTestTask.Services
{
    public interface IArticleService
    {
        Task<ArticleResponse?> GetArticleById(long id);
        Task<List<SectionResponse>> GetSections();
        Task<List<ArticleResponse>> GetArticlesBySectionId(long sectionId);
        Task<ArticleResponse?> CreateArticle(ArticleCreateUpdateRequest articleRequest);
        Task<ArticleResponse?> UpdateArticle(long id, ArticleCreateUpdateRequest articleRequest);
    }

    public class ArticleService : IArticleService
    {
        public readonly IArticleRepository _articleRepository;
        public readonly ISectionRepository _sectionRepository;
        public readonly ITagRepository _tagRepository;

        public ArticleService(IArticleRepository articleRepository,
            ISectionRepository sectionRepository,
            ITagRepository tagRepository)
        {
            _articleRepository = articleRepository;
            _sectionRepository = sectionRepository;
            _tagRepository = tagRepository;
        }

        public async Task<ArticleResponse?> GetArticleById(long id)
        {
            Article? article = await _articleRepository.GetById(id);

            if (article != null)
            {
                article.Tags = article.Tags.OrderBy(i => i.Order).ToList();
            }

            return MapToArticleResponse(article);
        }

        public async Task<List<SectionResponse>> GetSections()
        {
            List<DAL.Models.Section> sections = await _sectionRepository.GetList();

            return sections.Select(MapToSectionResponse).ToList();
        }

        public async Task<List<ArticleResponse>> GetArticlesBySectionId(long sectionId)
        {
            List<Article> articles = await _articleRepository.GetListBySectionId(sectionId);

            return articles.Select(MapToArticleResponse).ToList();
        }

        public async Task<ArticleResponse?> CreateArticle(ArticleCreateUpdateRequest articleRequest)
        {
            // Создадим или найдем теги
            List<Tag> tags = new();
            List<long> tagIds = new();
            foreach (var tagName in articleRequest.Tags)
            {
                var tag = await _tagRepository.GetByLowerName(tagName);

                if (tag == null)
                {
                    tag = await _tagRepository.Create(tagName);
                }

                tags.Add(tag);
                tagIds.Add(tag.Id);
            }

            Article article = new()
            {
                Name = articleRequest.Name,
            };

            // Создадим статью
            article = await _articleRepository.Add(article, tagIds);

            // Создадим или найдем раздел с уникальным набором тегов
            var section = await _sectionRepository.GetByTagIds(tagIds);
            if (section == null && tagIds.Count > 0)
            {
                section = await _sectionRepository.Add(tags);
            }

            // Обновим раздел статьи
            article.Section = section;
            await _articleRepository.SaveChangesAsync();

            return MapToArticleResponse(article);
        }

        public async Task<ArticleResponse?> UpdateArticle(long id, ArticleCreateUpdateRequest articleRequest)
        {
            Article? article = await _articleRepository.GetById(id);

            if (article == null)
            {
                return null;
            }

            // Создадим или найдем теги
            List<Tag> tags = new();
            List<long> tagIds = new();
            foreach (var tagName in articleRequest.Tags)
            {
                var tag = await _tagRepository.GetByLowerName(tagName);

                if (tag == null)
                {
                    tag = await _tagRepository.Create(tagName);
                }

                tags.Add(tag);
                tagIds.Add(tag.Id);
            }

            // Обновим статью
            article.Name = articleRequest.Name;
            await _articleRepository.Update(article, tagIds);

            // Создадим или найдем раздел с уникальным набором тегов
            var section = await _sectionRepository.GetByTagIds(tagIds);
            if (section == null)
            {
                section = await _sectionRepository.Add(tags);
            }

            // Обновим раздел статьи, если он изменился
            if (article.Section?.Id != section?.Id)
            {
                article.Section = section;
                await _articleRepository.SaveChangesAsync();

                // Удалим раздел, если на него больше никто не ссылается
                await _sectionRepository.DeleteUnusedSections();
            }

            // TODO: удалять теги если на них больше никто не ссылается

            return MapToArticleResponse(article);
        }

        private ArticleResponse? MapToArticleResponse(Article article)
        {
            if (article == null)
            {
                return null;
            }

            return new ArticleResponse
            {
                Id = article.Id,
                Name = article.Name,
                Tags = article.Tags
                    .OrderBy(i => i.Order)
                    .Select(i => i.Tag.Name)
                    .ToList(),
                SectionId = article.Section?.Id,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
            };
        }

        private SectionResponse? MapToSectionResponse(DAL.Models.Section section)
        {
            if (section == null)
            {
                return null;
            }

            return new SectionResponse
            {
                Id = section.Id,
                Name = section.Name,
                ArticlesCount = section.Articles.Count,
                Tags = section.Tags
                    .OrderBy(i => i.Name)
                    .Select(i => i.Name)
                    .ToList()
            };
        }
    }
}

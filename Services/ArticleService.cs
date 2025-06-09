using ArticlesTestTask.Contracts.Requests;
using ArticlesTestTask.Contracts.Responses;
using ArticlesTestTask.Controllers;
using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.DAL.Repository.Interfaces;
using ArticlesTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Section = ArticlesTestTask.DAL.Models.Section;

namespace ArticlesTestTask.Services
{
    public class ArticleService : IArticleService
    {
        public readonly IArticleRepository _articleRepository;
        public readonly ISectionRepository _sectionRepository;
        public readonly ITagRepository _tagRepository;
        private readonly ILogger<ArticleService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ArticleService(IArticleRepository articleRepository,
            ISectionRepository sectionRepository,
            ITagRepository tagRepository,
            ILogger<ArticleService> logger)
        {
            _articleRepository = articleRepository;
            _sectionRepository = sectionRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public async Task<ArticleResponse?> GetArticleById(long id, CancellationToken ct = default)
        {
            Article? article = await _articleRepository.GetById(id, ct);

            if (article != null)
            {
                article.Tags = article.Tags.OrderBy(i => i.Order).ToList();
            }

            return MapToArticleResponse(article);
        }

        public async Task<PagedList<SectionResponse>> GetSections(int pageNum = 1, int perPage = 10, CancellationToken ct = default)
        {
            List<Section> sections = await _sectionRepository.GetList(pageNum, perPage, ct);

            int? count = null;
            if (pageNum == 1)
            {
                count = await _sectionRepository.GetCount(ct);
            }

            return new PagedList<SectionResponse>
            {
                Items = [.. sections.Select(MapToSectionResponse)],
                Count = count
            };
        }

        public async Task<PagedList<ArticleResponse>> GetArticlesBySectionId(long sectionId, int pageNum = 1, int perPage = 10,
            CancellationToken ct = default)
        {
            var sectionExists = await _sectionRepository.Exists(sectionId, ct);

            if (!sectionExists)
            {
                throw new KeyNotFoundException("Не найдено раздела с таким идентификатором");
            }

            List<Article> articles = await _articleRepository.GetListBySectionId(sectionId, pageNum, perPage, ct);

            int? count = null;
            if (pageNum == 1)
            {
                count = await _articleRepository.GetCountBySectionId(sectionId, ct);
            }

            return new PagedList<ArticleResponse>
            { 
                Items = [.. articles.Select(MapToArticleResponse)],
                Count = count
            };
        }

        public async Task<ArticleResponse?> CreateArticle(ArticleCreateUpdateRequest articleRequest, CancellationToken ct = default)
        {
            // Создадим или найдем теги
            List<Tag> tags = [];
            List<long> tagIds = [];
            foreach (var tagName in articleRequest.Tags)
            {
                var tag = await _tagRepository.GetByLowerName(tagName, ct);

                if (tag == null)
                {
                    tag = await _tagRepository.Add(tagName, ct);
                }

                tags.Add(tag);
                tagIds.Add(tag.Id);
            }

            Article article = new()
            {
                Name = articleRequest.Name,
            };

            // Создадим статью
            article = await _articleRepository.Add(article, tagIds, ct);

            // Создадим или найдем раздел с уникальным набором тегов
            Section? section = await _sectionRepository.GetByTagIds(tagIds, ct);

            if (section == null)
            {
                article.Section = await _sectionRepository.Add(tags, ct);
            }
            else
            {
                article.Section = section;
            }

            article.Section.ArticlesCount++; 
            
            try
            {
                await _articleRepository.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Конфликт параллелизма при добавлении статьи с id = {article.Id} в раздел {article.Section.Id}. Попробуйте обновить статью еще раз. {ex.Message}");
                throw;
            }

            return MapToArticleResponse(article);
        }

        public async Task<ArticleResponse?> UpdateArticle(long id, ArticleCreateUpdateRequest articleRequest, CancellationToken ct = default)
        {
            Article? article = await _articleRepository.GetById(id, ct);

            if (article == null)
            {
                return null;
            }

            // Создадим или найдем теги
            List<Tag> tags = [];
            List<long> tagIds = [];
            foreach (var tagName in articleRequest.Tags)
            {
                var tag = await _tagRepository.GetByLowerName(tagName, ct);

                if (tag == null)
                {
                    tag = await _tagRepository.Add(tagName, ct);
                }

                tags.Add(tag);
                tagIds.Add(tag.Id);
            }

            // Обновим статью
            article.Name = articleRequest.Name;
            await _articleRepository.UpdateArticleTags(article, tagIds, ct);

            // Создадим или найдем раздел с уникальным набором тегов
            Section? section = await _sectionRepository.GetByTagIds(tagIds, ct);

            if (section == null)
            {
                article.Section = await _sectionRepository.Add(tags, ct);
                article.Section.ArticlesCount++;

                try
                {
                    await _articleRepository.SaveChangesAsync(ct);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Конфликт параллелизма при редактировании статьи с id = {id}. Попробуйте отредактировать статью еще раз. {ex.Message}");
                    throw;
                }
            }

            // Обновим раздел, если он изменился
            else if (article.SectionId != section.Id)
            {
                if (article.Section != null)
                {
                    article.Section.ArticlesCount--;
                }

                article.Section = section;
                section.ArticlesCount++;

                try
                {
                    await _articleRepository.SaveChangesAsync(ct);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Конфликт параллелизма при редактировании статьи с id = {id}. Попробуйте отредактировать статью еще раз. {ex.Message}");
                    throw;
                }

                // Удалим раздел, если на него больше никто не ссылается
                await _sectionRepository.DeleteUnusedSections(ct);
            }

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
                SectionId = article.SectionId,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
            };
        }

        private SectionResponse? MapToSectionResponse(Section section)
        {
            if (section == null)
            {
                return null;
            }

            return new SectionResponse
            {
                Id = section.Id,
                Name = section.Name,
                ArticlesCount = section.ArticlesCount,
            };
        }
    }
}
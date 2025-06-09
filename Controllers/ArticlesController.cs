using ArticlesTestTask.Contracts.Requests;
using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ArticlesTestTask.Controllers
{
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleService _articleService;

        public ArticlesController(ILogger<ArticlesController> logger,
            IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        /// <summary>
        /// Получить статью по идентификатору
        /// </summary>
        [HttpGet("api/v1/articles/{id}")]
        public async Task<IActionResult> GetArticleById(long id, CancellationToken ct = default)
        {
            try
            {
                var article = await _articleService.GetArticleById(id, ct);

                if (article == null)
                {
                    return NotFound();
                }

                return new JsonResult(
                    new { Success = true, Data = article },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Запрос отменен");
            }
        }

        /// <summary>
        /// Получить список разделов
        /// </summary>
        [HttpGet("api/v1/sections")]
        public async Task<IActionResult> GetSections(int pageNum = 1, int perPage = 10, CancellationToken ct = default)
        {
            try
            {
                var sections = await _articleService.GetSections(pageNum, perPage, ct);

                return new JsonResult(
                    new { Success = true, Data = sections },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Запрос отменен");
            }
        }

        /// <summary>
        /// Получить список статей в разделе
        /// </summary>
        /// <param name="sectionId">id раздела</param>
        /// <returns></returns>
        [HttpGet("api/v1/articles/bySection/{sectionId}")]
        public async Task<IActionResult> GetArticlesBySection(long sectionId, [FromQuery] int pageNum = 1, [FromQuery] int perPage = 10,
            CancellationToken ct = default)
        {
            try
            {
                var articles = await _articleService.GetArticlesBySectionId(sectionId, pageNum, perPage, ct);

                return new JsonResult(
                    new { Success = true, Data = articles },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Запрос отменен");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Создать статью
        /// </summary>
        /// <param name="item">Модель добавления статьи</param>
        /// <returns></returns>
        [HttpPost("articles")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateUpdateRequest item, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var article = await _articleService.CreateArticle(item, ct);

                return new JsonResult(new { Success = true, Data = article },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Запрос отменен");
            }
            catch(DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Конфликт параллелизма при добавлении статьи. Сервер не может выполнить запрос");
            }
        }

        /// <summary>
        /// Изменить статью
        /// </summary>
        /// <param name="id">Идентификатор статьи</param>
        /// <param name="item">Модель измененной статьи</param>
        /// <returns></returns>
        [HttpPut("articles/{id}")]
        public async Task<IActionResult> UpdateArticle(long id, [FromBody] ArticleCreateUpdateRequest item, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var article = await _articleService.UpdateArticle(id, item, ct);

                if (article == null)
                {
                    return NotFound();
                }

                return new JsonResult(new { Success = true, Data = article },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Запрос отменен");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Конфликт параллелизма при редактировании статьи. Сервер не может выполнить запрос");
            }
        }
    }
}
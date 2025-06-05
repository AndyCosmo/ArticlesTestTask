using ArticlesTestTask.Contracts.Requests;
using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.Services;
using Microsoft.AspNetCore.Mvc;
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
        /// �������� ������ �� ��������������
        /// </summary>
        [HttpGet("api/v1/articles/{id}")]
        public async Task<IActionResult> GetArticleById(long id)
        {
            var article = await _articleService.GetArticleById(id);

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

        /// <summary>
        /// �������� ������ ��������
        /// </summary>
        [HttpGet("api/v1/sections")]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _articleService.GetSections();

            return new JsonResult(
                new { Success = true, Data = sections },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// �������� ������ ������ � �������
        /// </summary>
        /// <param name="sectionId">id �������</param>
        /// <returns></returns>
        [HttpGet("api/v1/articles/bySection/{sectionId}")]
        public async Task<IActionResult> GetArticlesBySection(long sectionId)
        {
            var articles = await _articleService.GetArticlesBySectionId(sectionId);

            return new JsonResult(
                new { Success = true, Data = articles },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="item">������ ���������� ������</param>
        /// <returns></returns>
        [HttpPost("articles")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateUpdateRequest item)
        {
            if (item.Tags.Count > 256)
                return BadRequest("�������� ���� �� ����� 256 �����");

            var article = await _articleService.CreateArticle(item);

            return new JsonResult(
                new { Success = true, Data = article },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="id">������������� ������</param>
        /// <param name="item">������ ���������� ������</param>
        /// <returns></returns>
        [HttpPut("articles/{id}")]
        public async Task<IActionResult> UpdateArticle(long id, [FromBody] ArticleCreateUpdateRequest item)
        {
            if (item.Tags.Count > 256)
                return BadRequest("�������� ���� �� ����� 256 �����");

            var article = await _articleService.UpdateArticle(id, item);

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
    }
}
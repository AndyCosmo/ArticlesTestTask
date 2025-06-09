using System.ComponentModel.DataAnnotations;

namespace ArticlesTestTask.Contracts.Requests
{
    public class ArticleCreateUpdateRequest
    {
        /// <summary>
        /// Название статьи
        /// </summary>
        [MaxLength(256, ErrorMessage = "Допустимо название статьи длиной не более 256 символов")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        [MaxLength(256, ErrorMessage = "Допустим ввод не более 256 тегов")]
        public List<string> Tags { get; set; }
    }
}

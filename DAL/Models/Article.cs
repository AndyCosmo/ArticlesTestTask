using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlesTestTask.DAL.Models
{
    /// <summary>
    /// Статья
    /// </summary>
    [Table("articles")]
    public class Article : BaseModel
    {
        /// <summary>
        /// Название
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Список связей Статья-Тег
        /// </summary>
        [MaxLength(256)]
        public List<ArticleTag> Tags { get; set; } = new List<ArticleTag>();

        /// <summary>
        /// Раздел
        /// </summary>
        public Section? Section { get; set; }
    }
}

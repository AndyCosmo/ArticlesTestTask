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
        [ConcurrencyCheck]
        public string Name { get; set; }

        /// <summary>
        /// Список связей Статья-Тег
        /// </summary>
        [MaxLength(256)]
        public List<ArticleTag> Tags { get; set; } = new List<ArticleTag>();

        /// <summary>
        /// Раздел
        /// </summary>
        [ConcurrencyCheck]
        public long? SectionId { get; set; }

        /// <summary>
        /// Раздел
        /// </summary>
        public Section? Section { get; set; }

        /// <summary>
        /// Метка времени последней активности (создание или изменение)
        /// </summary>
        [Column("last_activity_at")]
        public DateTime LastActivityAt { get; set; }
    }
}
